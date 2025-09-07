namespace TranslationSummary;

public class Constants
{
    public static readonly string[] IgnoreRelativePaths = new[] { "_archived", "_Archived", "TestDataLayersMod" };

    public static readonly string[] IgnorePathSubstrings = new[] {
        Path.Combine("bin", "Debug"),
        Path.Combine("bin", "Release")
    };

    public static readonly Dictionary<string, string> RenameMods = new()
    {
        ["Pathoschild.CentralStation.Content"] = "Central Station"
    };

    public static readonly Dictionary<string, ModLanguage> Languages = new(StringComparer.OrdinalIgnoreCase)
    {
        ["de"] = new ModLanguage("German"),
        ["es"] = new ModLanguage("Spanish"),
        ["fr"] = new ModLanguage("French"),
        ["hu"] = new ModLanguage("Hungarian"),
        ["it"] = new ModLanguage("Italian"),
        ["ja"] = new ModLanguage("Japanese"),
        ["ko"] = new ModLanguage("Korean"),
        ["pt"] = new ModLanguage("Portuguese"),
        ["ru"] = new ModLanguage("Russian"),
        ["tr"] = new ModLanguage("Turkish"),
        ["zh"] = new ModLanguage("Chinese"),
        ["pl"] = new ModLanguage("Polish", Required: false, Url: "https://www.nexusmods.com/stardewvalley/mods/3616"),
        ["th"] = new ModLanguage("Thai", Required: false, Url: "https://www.nexusmods.com/stardewvalley/mods/7052"),
        ["uk"] = new ModLanguage("Ukrainian", Required: false, Url: "https://www.nexusmods.com/stardewvalley/mods/8427"),
        ["vi"] = new ModLanguage("Vietnamese", Required: false, Url: "https://www.nexusmods.com/stardewvalley/mods/24371")
    };

    public const string InitReadme = @"
# My Stardew Valley Mods

This repository contains my mods for [Stardew Valley for PC](http://stardewvalley.net/):

* [AddMoney](AddMoney) ([Nexus](https://www.nexusmods.com/stardewvalley/mods/21016))
* [AutoWater SDV1.6](AutoWater) ([Nexus](https://www.nexusmods.com/stardewvalley/mods/21022))
* [Configure Machine Speed SDV1.6](ConfigureMachineSpeed) ([Nexus](https://www.nexusmods.com/stardewvalley/mods/21005))

This codes are based on:

* AddMoney by [Pempi](https://www.nexusmods.com/stardewvalley/mods/20111)
* AutoWater SDV1.6 by [KochiPiccle](https://www.nexusmods.com/stardewvalley/mods/1666)
* Configure Machine Speed SDV1.6 by [BayesianBandit](https://www.nexusmods.com/stardewvalley/mods/3519)
";

    public const string FinalReadme = @"
## Dev

* [Steph Hoel](https://github.com/stephhoel)

## Thanks

Thanks for Pathoschild to help me.
";
}