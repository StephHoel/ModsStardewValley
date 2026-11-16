using System.Text;

namespace TranslationSummary;

public static class Program
{
    public static int Main(string[] args)
    {
        var checkOnly = args.Contains("--check", StringComparer.OrdinalIgnoreCase);
        var solutionPath = args.FirstOrDefault(arg => !arg.StartsWith("--", StringComparison.Ordinal)) ?? "../../../../";

        try
        {
            var solutionFolder = Path.GetFullPath(solutionPath);
            if (!Directory.Exists(solutionFolder))
            {
                Console.Error.WriteLine($"Tool error: diretório do repositório não encontrado: {solutionFolder}");
                return 2;
            }

            var modFolders = ModScanner.GetModFolders(solutionFolder, Constants.RenameMods);
            var issues = TranslationValidator.Validate(modFolders);

            foreach (var issue in issues)
                Console.Error.WriteLine(issue);

            if (issues.Any(issue => issue.IsError))
            {
                Console.Error.WriteLine($"Validação reprovada: {issues.Count(issue => issue.IsError)} erro(s), {issues.Count(issue => !issue.IsError)} aviso(s).");
                return 1;
            }

            if (checkOnly)
            {
                if (issues.Count > 0)
                {
                    Console.Error.WriteLine($"Validação reprovada: {issues.Count} aviso(s).");
                    return 1;
                }

                Console.WriteLine("Validação de traduções concluída sem divergências.");
                return 0;
            }

            ModScanner.PopulateTranslationStatus(modFolders, Constants.Languages);
            var warnings = WarningCollector.Collect(modFolders);

            var markdown = new StringBuilder();
            markdown.AppendLine(Constants.InitReadme.Trim() + "\n");

            if (warnings.Any())
                markdown.Append(MarkdownGenerator.GenerateWarnings(warnings));

            markdown.Append(MarkdownGenerator.GenerateSummary(modFolders));
            markdown.AppendLine(Constants.FinalReadme.Trim());

            var readmePath = Path.GetFullPath(Path.Combine(solutionFolder, "README.md"));
            File.WriteAllText(readmePath, markdown.ToString(), Encoding.UTF8);

            Console.WriteLine($"Resumo de tradução e avisos salvos em: {readmePath}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Tool error: {ex}");
            return 2;
        }
    }
}
