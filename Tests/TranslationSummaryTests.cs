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


public class MoneyMathTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void RejectsNonPositiveAmounts(int requested)
    {
        Assert.False(StephHoel.AddMoney.MoneyMath.TryCalculateAddition(100, requested, out var amount));
        Assert.Equal(0, amount);
    }

    [Fact]
    public void ClampsAdditionToWalletCapacity()
    {
        var balance = int.MaxValue - 50;
        Assert.True(StephHoel.AddMoney.MoneyMath.TryCalculateAddition(balance, 100, out var amount));
        Assert.Equal(50, amount);
        Assert.Equal(int.MaxValue, balance + amount);
    }

    [Fact]
    public void RejectsWalletAtMaximum()
    {
        Assert.False(StephHoel.AddMoney.MoneyMath.TryCalculateAddition(int.MaxValue, 1, out var amount));
        Assert.Equal(0, amount);
    }
}

public class MachineTimeRulesTests
{
    [Theory]
    [InlineData(0, 10)]
    [InlineData(-25, 10)]
    [InlineData(14, 10)]
    [InlineData(15, 20)]
    [InlineData(19, 20)]
    [InlineData(25, 30)]
    [InlineData(int.MinValue, 10)]
    public void NormalizesAndRoundsConfiguredTime(int input, int expected)
        => Assert.Equal(expected, StephHoel.ConfigureMachineSpeed.MachineTimeRules.NormalizeConfiguredTime(input));

    [Fact]
    public void NeverSlowsMachineBelowOriginalTime()
    {
        Assert.Equal(30, StephHoel.ConfigureMachineSpeed.MachineTimeRules.CalculateTarget(100, 30));
        Assert.Equal(20, StephHoel.ConfigureMachineSpeed.MachineTimeRules.CalculateTarget(20, 60));
    }
}

public class MachineConfigResolverTests
{
    [Fact]
    public void QualifiedIdTakesPrecedenceOverLegacyName()
    {
        var byId = new Dictionary<string, StephHoel.ConfigureMachineSpeed.MachineConfig>(StringComparer.Ordinal)
        {
            ["(BC)10"] = new("(BC)10") { Time = 20 }
        };
        var legacy = new StephHoel.ConfigureMachineSpeed.MachineConfig("") { Name = "Keg", Time = 60 };
        var byName = new Dictionary<string, StephHoel.ConfigureMachineSpeed.MachineConfig>(StringComparer.OrdinalIgnoreCase) { ["Keg"] = legacy };

        Assert.True(StephHoel.ConfigureMachineSpeed.MachineConfigResolver.TryResolve(byId, byName, "(BC)10", "Keg", out var result));
        Assert.Same(byId["(BC)10"], result);
    }

    [Fact]
    public void FallsBackToLegacyNameWhenIdDoesNotMatch()
    {
        var byId = new Dictionary<string, StephHoel.ConfigureMachineSpeed.MachineConfig>(StringComparer.Ordinal);
        var legacy = new StephHoel.ConfigureMachineSpeed.MachineConfig("") { Name = "Keg", Time = 60 };
        var byName = new Dictionary<string, StephHoel.ConfigureMachineSpeed.MachineConfig>(StringComparer.OrdinalIgnoreCase) { ["Keg"] = legacy };

        Assert.True(StephHoel.ConfigureMachineSpeed.MachineConfigResolver.TryResolve(byId, byName, "(BC)999", "Keg", out var result));
        Assert.Same(legacy, result);
    }
}
