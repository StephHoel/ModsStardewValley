using StardewModdingAPI;
using StardewValley;
using StephHoel.ConfigureMachineSpeed.Config;
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

        var onGameLaunched = new OnGameLaunched(helper, Monitor);
        var onTimeChanged = new OnTimeChanged(helper, Monitor);
        var onRenderedActiveMenu = new OnRenderedActiveMenu(ModManifest, helper, Monitor);
        var onLocaleChanged = new OnLocaleChanged(ModManifest, helper, Monitor);

        helper.Events.Display.RenderedActiveMenu += onRenderedActiveMenu.Main;
        helper.Events.Content.LocaleChanged += onLocaleChanged.Main;
        helper.Events.GameLoop.GameLaunched += onGameLaunched.Main;
        helper.Events.GameLoop.TimeChanged += onTimeChanged.Main;
    }
}