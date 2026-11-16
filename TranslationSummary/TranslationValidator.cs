using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TranslationSummary;

public sealed record TranslationValidationIssue(string FilePath, string Message, bool IsError = true)
{
    public override string ToString() => $"{FilePath}: {Message}";
}

/// <summary>Validates translation JSON files against their default locale.</summary>
public static class TranslationValidator
{
    private static readonly Regex FormattingTokenPattern = new(
        @"\{\d+\}|%[0-9]*\$?[sdif]|</?[A-Za-z][^>]*>|\[\[[^\]]+\]\]",
        RegexOptions.Compiled
    );

    private static readonly Regex TodoMarkerPattern = new(
        @"//[ \t]*TODO\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled
    );

    public static IReadOnlyList<TranslationValidationIssue> Validate(ModFolder[] modFolders)
    {
        var issues = new List<TranslationValidationIssue>();

        foreach (var folder in modFolders.OrderBy(item => item.RelativePath, StringComparer.Ordinal))
        {
            foreach (var directory in folder.TranslationDirs.OrderBy(item => item.FullName, StringComparer.Ordinal))
            {
                var defaultFile = directory.GetFiles("default.json").FirstOrDefault();
                if (defaultFile is null)
                {
                    AddIssue(folder, issues, directory.FullName, "Arquivo default.json não encontrado.");
                    continue;
                }

                if (!TryReadTranslations(folder, defaultFile, issues, out var defaults))
                    continue;

                foreach (var translationFile in directory.GetFiles("*.json")
                    .Where(file => !string.Equals(file.Name, "default.json", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(file => file.Name, StringComparer.Ordinal))
                {
                    if (!TryReadTranslations(folder, translationFile, issues, out var translations))
                        continue;

                    var relativePath = Path.GetRelativePath(folder.ModDir.FullName, translationFile.FullName);
                    var defaultKeys = defaults.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);
                    var translationKeys = translations.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);

                    foreach (var missingKey in defaultKeys.Except(translationKeys, StringComparer.OrdinalIgnoreCase).OrderBy(key => key, StringComparer.OrdinalIgnoreCase))
                        AddIssue(folder, issues, relativePath, $"Chave ausente na tradução: '{missingKey}'.");

                    foreach (var obsoleteKey in translationKeys.Except(defaultKeys, StringComparer.OrdinalIgnoreCase).OrderBy(key => key, StringComparer.OrdinalIgnoreCase))
                        AddIssue(folder, issues, relativePath, $"Chave adicional/obsoleta na tradução: '{obsoleteKey}'.");

                    foreach (var key in defaultKeys.Intersect(translationKeys, StringComparer.OrdinalIgnoreCase).OrderBy(key => key, StringComparer.OrdinalIgnoreCase))
                    {
                        var expectedTokens = GetFormattingTokens(defaults[key]);
                        var actualTokens = GetFormattingTokens(translations[key]);

                        if (!expectedTokens.SequenceEqual(actualTokens, StringComparer.Ordinal))
                        {
                            AddIssue(
                                folder,
                                issues,
                                relativePath,
                                $"Tokens de formatação divergentes na chave '{key}'. Esperado: [{string.Join(", ", expectedTokens)}]; encontrado: [{string.Join(", ", actualTokens)}]."
                            );
                        }
                    }

                    var fileContent = File.ReadAllText(translationFile.FullName);
                    if (TodoMarkerPattern.IsMatch(fileContent))
                        AddIssue(folder, issues, relativePath, "Marcador TODO encontrado; tradução ainda requer revisão.", isError: false);
                }
            }
        }

        return issues
            .OrderBy(issue => issue.FilePath, StringComparer.Ordinal)
            .ThenBy(issue => issue.Message, StringComparer.Ordinal)
            .ToArray();
    }

    private static bool TryReadTranslations(
        ModFolder folder,
        FileInfo file,
        List<TranslationValidationIssue> issues,
        out Dictionary<string, string> translations)
    {
        translations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        try
        {
            var json = JObject.Parse(File.ReadAllText(file.FullName));
            foreach (var property in json.Properties())
            {
                if (property.Value.Type is not (JTokenType.String or JTokenType.Null))
                {
                    AddIssue(folder, issues, file.FullName, $"O valor da chave '{property.Name}' precisa ser uma string ou null.");
                    return false;
                }

                translations[property.Name] = property.Value.Value<string>() ?? string.Empty;
            }

            return true;
        }
        catch (JsonReaderException ex)
        {
            AddIssue(folder, issues, file.FullName, $"JSON inválido na linha {ex.LineNumber}, coluna {ex.LinePosition}: {ex.Message}");
            return false;
        }
        catch (JsonException ex)
        {
            AddIssue(folder, issues, file.FullName, $"JSON inválido: {ex.Message}");
            return false;
        }
        catch (IOException ex)
        {
            AddIssue(folder, issues, file.FullName, $"Não foi possível ler o arquivo: {ex.Message}");
            return false;
        }
    }

    private static string[] GetFormattingTokens(string value)
    {
        return FormattingTokenPattern.Matches(value)
            .Cast<Match>()
            .Select(match => match.Value)
            .OrderBy(token => token, StringComparer.Ordinal)
            .ToArray();
    }

    private static void AddIssue(
        ModFolder folder,
        List<TranslationValidationIssue> issues,
        string path,
        string message,
        bool isError = true)
    {
        var relativePath = Path.IsPathRooted(path)
            ? Path.GetRelativePath(folder.ModDir.FullName, path)
            : path;

        var issue = new TranslationValidationIssue(relativePath, message, isError);
        issues.Add(issue);
        folder.Warnings.Add(issue.ToString());
    }
}
