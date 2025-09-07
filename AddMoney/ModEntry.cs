using AddMoney.Events;
using StardewModdingAPI;
using Utils;

namespace AddMoney;

/// <summary>The mod entry point.</summary>
public class ModEntry : Mod
{
    /// <summary>The mod entry point, called after the mod is first loaded.</summary>
    /// <param name="helper">Provides simplified APIs for writing mods.</param>
    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        FileUtils.RemoveObsoleteFiles(helper, Monitor);

        var config = helper.ReadConfig<ModConfig>();

        var onGameLaunched = new OnGameLaunched(helper, this, config);
        var onButtonPressed = new OnButtonPressed(config, Monitor);

        helper.Events.GameLoop.GameLaunched += onGameLaunched.Main;
        helper.Events.Input.ButtonPressed += onButtonPressed.Main;
    }
}