using StardewModdingAPI;
using StephHoel.ConfigureMachineSpeed.Events;
using Utils;

namespace StephHoel.ConfigureMachineSpeed;

public class ModEntry : Mod
{
    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        FileUtils.RemoveObsoleteFiles(helper, Monitor);

        helper.ReadConfig<ModConfig>(); // read or create

        var onGameLaunched = new OnGameLaunched(
                    ModManifest
                    , helper
                    , Monitor
                );

        var onTimeChanged = new OnTimeChanged(
                    helper
                    , Monitor
                );

        helper.Events.GameLoop.GameLaunched += onGameLaunched.Main;
        helper.Events.GameLoop.TimeChanged += onTimeChanged.Main;
    }
}