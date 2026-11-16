using TranslationSummary;
using TranslationSummary.Enums;

namespace TranslationSummary.Tests;

public class ModFolderTests
{
    [Fact]
    public void GetStatusForLocale_ReturnsStoredStatusAndMarksMissingLocales()
    {
        var folder = new ModFolder(new DirectoryInfo("AddMoney"), "AddMoney", "AddMoney");
        folder.TranslationStatus["i18n"] = new Dictionary<string, TranslationStatusEnum>
        {
            ["pt"] = TranslationStatusEnum.Complete,
            ["de"] = TranslationStatusEnum.Incomplete
        };

        var result = folder.GetStatusForLocale("pt");

        Assert.Equal(TranslationStatusEnum.Complete, result["i18n"]);
        Assert.Equal(TranslationStatusEnum.Missing, folder.GetStatusForLocale("fr")["i18n"]);
    }

    [Fact]
    public void WarningCollector_ReturnsOnlyModsWithWarnings()
    {
        var clean = new ModFolder(new DirectoryInfo("CleanMod"), "CleanMod", "Clean Mod");
        var warned = new ModFolder(new DirectoryInfo("WarnedMod"), "WarnedMod", "Warned Mod");
        warned.Warnings.Add("Chave ausente: title");

        var result = WarningCollector.Collect([clean, warned]).ToArray();

        var warning = Assert.Single(result);
        Assert.Equal("Warned Mod", warning.ModName);
        Assert.Equal("WarnedMod", warning.RelativePath);
        Assert.Contains("Chave ausente: title", warning.Warnings);
    }
}

public class MarkdownGeneratorTests
{
    [Fact]
    public void GenerateSummary_ContainsModNameAndTranslationStatuses()
    {
        var folder = new ModFolder(new DirectoryInfo("AddMoney"), "AddMoney", "Add Money");
        folder.TranslationStatus["i18n"] = new Dictionary<string, TranslationStatusEnum>
        {
            ["pt"] = TranslationStatusEnum.Complete,
            ["de"] = TranslationStatusEnum.Missing
        };

        var markdown = MarkdownGenerator.GenerateSummary([folder]);

        Assert.Contains("Add Money", markdown);
        Assert.Contains("pt.json", markdown);
        Assert.Contains("✓", markdown);
        Assert.Contains("❑", markdown);
    }

    [Fact]
    public void GenerateWarnings_ContainsModPathAndWarningDetails()
    {
        var warnings = new[]
        {
            ("AutoWater", "AutoWater", new HashSet<string> { "Marcador {0} ausente" })
        };

        var markdown = MarkdownGenerator.GenerateWarnings(warnings);

        Assert.Contains("AutoWater", markdown);
        Assert.Contains("Marcador {0} ausente", markdown);
    }
}
