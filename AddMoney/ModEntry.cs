using StardewModdingAPI;
using StephHoel.AddMoney.Events;
using Utils;

namespace StephHoel.AddMoney;

/// <summary>The mod entry point.</summary>
public class ModEntry : Mod
{
    private ModConfig Config = null!;

    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="helper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        FileUtils.RemoveObsoleteFiles(helper, Monitor);

        Config = ConfigUtils.Normalize(helper.ReadConfig<ModConfig>());
        helper.WriteConfig(Config);

        var onGameLaunched = new OnGameLaunched(
            helper: helper,
            manifest: this.ModManifest,
            getConfig: () => this.Config,
            setConfig: cfg => this.Config = cfg
        );

        var onButtonPressed = new OnButtonPressed(
            getConfig: () => this.Config,
            monitor: Monitor
        );

        helper.Events.GameLoop.GameLaunched += onGameLaunched.Main;
        helper.Events.Input.ButtonPressed += onButtonPressed.Main;
    }
}