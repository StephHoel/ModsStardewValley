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

**Current versions:** AddMoney 1.3.0; AutoWater 1.4.2; Configure Machine Speed 2.1.3.

**Compatibility:** All three mods target Stardew Valley 1.6.x and require SMAPI 4.2.0 or later. Generic Mod Config Menu is optional.

**AddMoney default:** press G to add 100,000 gold. The amount and key can be changed in the config file or Generic Mod Config Menu.

**AutoWater:** waters supported crops and garden pots daily, including loaded building interiors. A configurable key toggles the mod; only the host runs watering in multiplayer.

**Configure Machine Speed:** adjusts supported machine processing times. Configured times are at least 10 minutes and never exceed the machine's original processing time. Casks and DayUpdate-dependent machines have dedicated behavior.

This code is based on:

* AddMoney by [Pempi](https://www.nexusmods.com/stardewvalley/mods/20111)
* AutoWater SDV1.6 by [KochiPiccle](https://www.nexusmods.com/stardewvalley/mods/1666)
* Configure Machine Speed SDV1.6 by [BayesianBandit](https://www.nexusmods.com/stardewvalley/mods/3519)
";

    public const string FinalReadme = @"
## Dev

* [Steph Hoel](https://github.com/stephhoel)

## Thanks

Thanks to Pathoschild for the help.
";
}
