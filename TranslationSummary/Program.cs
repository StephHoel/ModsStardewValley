using System.Text;

namespace TranslationSummary;

public static class Program
{
    public static void Main()
    {
        var solutionFolder = "../../../../";

        // Buscar mods
        var modFolders = ModScanner.GetModFolders(solutionFolder, Constants.RenameMods);

        // Analisar traduções
        ModScanner.PopulateTranslationStatus(modFolders, Constants.Languages);

        // Coletar warnings
        var warnings = WarningCollector.Collect(modFolders);

        // Gerar markdown
        var markdown = new StringBuilder();
        markdown.AppendLine(Constants.InitReadme.Trim() + "\n");

        if (warnings.Any())
            markdown.Append(MarkdownGenerator.GenerateWarnings(warnings));

        markdown.Append(MarkdownGenerator.GenerateSummary(modFolders));

        markdown.AppendLine(Constants.FinalReadme.Trim());

        // Escrever no README.md
        var readmePath = Path.GetFullPath(Path.Combine(solutionFolder, "README.md"));

        File.WriteAllText(readmePath, markdown.ToString(), Encoding.UTF8);

        Console.WriteLine($"Resumo de tradução e warnings estão em: {readmePath}");
    }
}