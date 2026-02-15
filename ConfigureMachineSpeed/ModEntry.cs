using StardewModdingAPI;
using StephHoel.ConfigureMachineSpeed.Events;
using Utils;

namespace StephHoel.ConfigureMachineSpeed;

public class ModEntry : Mod
{
    private ModConfig Config = null!;
    private MachineConfigurator Configurator = null!;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        FileUtils.RemoveObsoleteFiles(helper, Monitor);

        Config = ConfigUtils.Normalize(helper.ReadConfig<ModConfig>(), Monitor);
        helper.WriteConfig(Config);

        Configurator = new MachineConfigurator();

        var onGameLaunched = new OnGameLaunched(
                    ModManifest,
                    helper,
                    Monitor,
                    setConfig: cfg => this.Config = cfg
                );

        var onSaveLoaded = new OnSaveLoaded(
                    helper,
                    Monitor,
                    Configurator,
                    setConfig: cfg => this.Config = cfg
                );

        var onDayStarted = new OnDayStarted(
                    Configurator,
                    getConfig: () => this.Config
                );

        var onUpdateTicking = new OnUpdateTicking(
                    Configurator,
                    getConfig: () => this.Config
                );

        var onButtonPressed = new OnButtonPressed(
                    helper,
                    Monitor,
                    Configurator,
                    getConfig: () => this.Config,
                    setConfig: cfg => this.Config = cfg
                );

        helper.Events.GameLoop.GameLaunched += onGameLaunched.Main;
        helper.Events.GameLoop.DayStarted += onDayStarted.Main;
        helper.Events.GameLoop.SaveLoaded += onSaveLoaded.Main;
        helper.Events.GameLoop.UpdateTicking += onUpdateTicking.Main;
        helper.Events.Input.ButtonPressed += onButtonPressed.Main;
    }
}