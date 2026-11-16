using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TranslationSummary.Enums;

namespace TranslationSummary;

public class ModScanner
{
    public static ModFolder[] GetModFolders(string solutionFolder, Dictionary<string, string> renameMods)
    {
        var i18nDirs = new DirectoryInfo(solutionFolder).GetFiles("default.json", SearchOption.AllDirectories)
            .Select(file => file.Directory)
            .Where(dir => dir?.Name == "i18n")
            .OrderByDescending(dir => dir!.FullName, StringComparer.Ordinal)
            .ToArray();

        var folders = new List<ModFolder>();
        ModFolder? folder = null;

        foreach (var dir in i18nDirs)
        {
            var modDir = dir!.Parent!;
            var relativePath = Path.GetRelativePath(solutionFolder, modDir.FullName);

            if (folder is null || !relativePath.StartsWith(folder.RelativePath + Path.DirectorySeparatorChar, StringComparison.Ordinal))
            {
                if (folder is not null)
                    folders.Add(folder);

                var manifestFile = modDir.GetFiles("manifest.json").FirstOrDefault();
                if (manifestFile is null)
                {
                    folder = null;
                    continue;
                }

                var manifest = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(manifestFile.FullName))
                    ?? throw new InvalidDataException($"Manifesto inválido: {manifestFile.FullName}");

                var modName = manifest.UniqueId is not null
                    ? renameMods.GetValueOrDefault(manifest.UniqueId) ?? manifest.Name
                    : manifest.Name;

                folder = new ModFolder(modDir, relativePath, modName);
            }

            folder.TranslationDirs.Add(dir);
        }

        if (folder is not null)
            folders.Add(folder);

        return folders
            .Where(item => !ShouldIgnore(item.RelativePath))
            .OrderBy(item => item.ModName, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public static void PopulateTranslationStatus(ModFolder[] modFolders, Dictionary<string, ModLanguage> languages)
    {
        // Populate each translation directory exactly once per mod.
        foreach (var folder in modFolders)
            PopulateTranslationStatusEnum(folder);

        var knownLocales = GetKnownLocales(modFolders, languages);
        foreach (var folder in modFolders)
        {
            foreach (var entry in folder.TranslationStatus.Values)
            {
                foreach (var locale in knownLocales)
                    entry.TryAdd(locale, TranslationStatusEnum.Missing);
            }
        }
    }

    private static void PopulateTranslationStatusEnum(ModFolder modFolder)
    {
        foreach (DirectoryInfo dir in modFolder.TranslationDirs)
        {
            string dirRelativePath = Path.GetRelativePath(modFolder.ModDir.FullName, dir.FullName);
            var statuses = modFolder.TranslationStatus[dirRelativePath] = new();
            HashSet<string> defaultKeys = new(StringComparer.OrdinalIgnoreCase);
            var defaultEntry = dir.GetFiles("default.json").First();
            string content = File.ReadAllText(defaultEntry.FullName);
            foreach (var key in JsonConvert.DeserializeObject<Dictionary<string, string>>(content)!.Keys)
                defaultKeys.Add(key);

            foreach (FileInfo file in dir.GetFiles().OrderBy(item => item.Name, StringComparer.Ordinal))
            {
                string fileContent = File.ReadAllText(file.FullName);
                if (file.Name == "default.json" || file.Extension != ".json")
                    continue;

                string locale = Path.GetFileNameWithoutExtension(file.Name);
                var translation = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileContent);
                if (translation is null)
                    continue;

                var keys = new HashSet<string>(translation.Keys, StringComparer.OrdinalIgnoreCase);
                var status = Regex.IsMatch(fileContent, @"//[ \t]*TODO\b", RegexOptions.IgnoreCase)
                    || defaultKeys.Any(key => !keys.Contains(key))
                    ? TranslationStatusEnum.Incomplete
                    : TranslationStatusEnum.Complete;

                statuses[locale] = status;
            }
        }
    }

    private static HashSet<string> GetKnownLocales(ModFolder[] folders, Dictionary<string, ModLanguage> languages)
    {
        HashSet<string> knownLocales = new(languages.Where(pair => pair.Value.Required).Select(pair => pair.Key));
        var keys = folders.SelectMany(folder => folder.TranslationStatus.Values.SelectMany(statuses => statuses.Keys));
        foreach (var locale in keys)
            knownLocales.Add(locale);
        return knownLocales;
    }

    private static bool ShouldIgnore(string relativePath)
    {
        if (Constants.IgnoreRelativePaths.Any(ignorePath =>
            relativePath == ignorePath || relativePath.StartsWith(ignorePath + Path.DirectorySeparatorChar, StringComparison.Ordinal)))
            return true;

        return Constants.IgnorePathSubstrings.Any(ignoreSubstr => relativePath.Contains(ignoreSubstr, StringComparison.Ordinal));
    }
}
