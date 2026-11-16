using StephHoel.AddMoney;
using StephHoel.ConfigureMachineSpeed;
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
    public void TryCalculateAddition_RejectsNonPositiveAmounts(int requestedAmount)
    {
        Assert.False(MoneyMath.TryCalculateAddition(100, requestedAmount, out var amount));
        Assert.Equal(0, amount);
    }

    [Fact]
    public void TryCalculateAddition_ClampsToRemainingBalanceCapacity()
    {
        var currentBalance = int.MaxValue - 50;

        Assert.True(MoneyMath.TryCalculateAddition(currentBalance, 100, out var amount));
        Assert.Equal(50, amount);
        Assert.Equal(int.MaxValue, currentBalance + amount);
    }

    [Fact]
    public void TryCalculateAddition_RejectsWalletAlreadyAtLimit()
    {
        Assert.False(MoneyMath.TryCalculateAddition(int.MaxValue, 1, out var amount));
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
    public void NormalizeConfiguredTime_EnforcesMinimumAndRounds(int input, int expected)
    {
        Assert.Equal(expected, MachineTimeRules.NormalizeConfiguredTime(input));
    }

    [Fact]
    public void CalculateTarget_NeverSlowsMachineBelowOriginalTime()
    {
        Assert.Equal(30, MachineTimeRules.CalculateTarget(100, 30));
        Assert.Equal(20, MachineTimeRules.CalculateTarget(20, 60));
    }
}

public class MachineConfigResolverTests
{
    [Fact]
    public void TryResolve_PrefersQualifiedIdOverLegacyName()
    {
        var byId = new Dictionary<string, MachineConfig>(StringComparer.Ordinal)
        {
            ["(BC)10"] = new MachineConfig("(BC)10") { Time = 20 }
        };
        var byName = new Dictionary<string, MachineConfig>(StringComparer.OrdinalIgnoreCase)
        {
            ["Keg"] = new MachineConfig("") { Name = "Keg", Time = 60 }
        };

        Assert.True(MachineConfigResolver.TryResolve(byId, byName, "(BC)10", "Keg", out var result));
        Assert.Same(byId["(BC)10"], result);
    }

    [Fact]
    public void TryResolve_UsesLegacyNameWhenQualifiedIdIsUnavailable()
    {
        var byId = new Dictionary<string, MachineConfig>(StringComparer.Ordinal);
        var legacy = new MachineConfig("") { Name = "Keg", Time = 60 };
        var byName = new Dictionary<string, MachineConfig>(StringComparer.OrdinalIgnoreCase)
        {
            ["Keg"] = legacy
        };

        Assert.True(MachineConfigResolver.TryResolve(byId, byName, "(BC)999", "Keg", out var result));
        Assert.Same(legacy, result);
    }
}
