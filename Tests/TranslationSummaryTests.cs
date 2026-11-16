using TranslationSummary;
using TranslationSummary.Enums;
using Xunit;

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

        var result = WarningCollector.Collect(new[] { clean, warned }).ToArray();

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

        var markdown = MarkdownGenerator.GenerateSummary(new[] { folder });

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

public class TranslationValidatorTests
{
    [Fact]
    public void Validate_ReportsMissingObsoleteKeysAndFormattingTokenMismatch()
    {
        var (folder, root, i18nPath) = CreateModFolder();
        try
        {
            File.WriteAllText(Path.Combine(i18nPath, "default.json"),
                """{"greeting":"Hello {0} <b>farmer</b>","defaultOnly":"Required"}""");
            File.WriteAllText(Path.Combine(i18nPath, "pt.json"),
                """{"greeting":"Olá {1} <b>fazendeiro</b>","obsolete":"Old"}""");

            var issues = TranslationValidator.Validate(new[] { folder });

            Assert.Contains(issues, issue => issue.Message.Contains("ausente", StringComparison.OrdinalIgnoreCase));
            Assert.Contains(issues, issue => issue.Message.Contains("obsoleta", StringComparison.OrdinalIgnoreCase));
            Assert.Contains(issues, issue => issue.Message.Contains("Tokens de formatação divergentes", StringComparison.Ordinal));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void Validate_ReportsInvalidJsonWithFileAndLineInformation()
    {
        var (folder, root, i18nPath) = CreateModFolder();
        try
        {
            File.WriteAllText(Path.Combine(i18nPath, "default.json"), """{"key":"value"}""");
            File.WriteAllText(Path.Combine(i18nPath, "pt.json"), "{\n  \"key\": \"value\",\n");

            var issue = Assert.Single(TranslationValidator.Validate(new[] { folder }));

            Assert.Contains("pt.json", issue.FilePath);
            Assert.Contains("linha", issue.Message, StringComparison.OrdinalIgnoreCase);
            Assert.True(issue.IsError);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void Validate_ReturnsIssuesInDeterministicOrder()
    {
        var (folder, root, i18nPath) = CreateModFolder();
        try
        {
            File.WriteAllText(Path.Combine(i18nPath, "default.json"),
                """{"first":"First","second":"Second"}""");
            File.WriteAllText(Path.Combine(i18nPath, "pt.json"),
                """{"second":"Segundo","extra":"Extra"}""");

            var firstRun = TranslationValidator.Validate(new[] { folder }).Select(issue => issue.ToString()).ToArray();
            var secondRun = TranslationValidator.Validate(new[] { folder }).Select(issue => issue.ToString()).ToArray();

            Assert.Equal(firstRun, secondRun);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static (ModFolder Folder, string Root, string I18nPath) CreateModFolder()
    {
        var root = Path.Combine(Path.GetTempPath(), $"TranslationSummary-{Guid.NewGuid():N}");
        var modPath = Directory.CreateDirectory(Path.Combine(root, "TestMod")).FullName;
        var i18nPath = Directory.CreateDirectory(Path.Combine(modPath, "i18n")).FullName;
        var folder = new ModFolder(new DirectoryInfo(modPath), "TestMod", "Test Mod");
        folder.TranslationDirs.Add(new DirectoryInfo(i18nPath));
        return (folder, root, i18nPath);
    }
}
