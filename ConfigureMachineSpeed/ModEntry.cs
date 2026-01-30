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

        Config = ConfigUtils.Normalize(helper.ReadConfig<ModConfig>());
        helper.WriteConfig(Config);

        Configurator = new MachineConfigurator();

        var onGameLaunched = new OnGameLaunched(
                    ModManifest,
                    helper,
                    setConfig: cfg => this.Config = cfg
                );

        var onSaveLoaded = new OnSaveLoaded(
                    helper,
                    Configurator,
                    setConfig: cfg => this.Config = cfg
                );

        var onDayStarted = new OnDayStarted(
                    helper,
                    Configurator,
                    setConfig: cfg => this.Config = cfg
                );

        var onUpdateTicking = new OnUpdateTicking(
                    helper,
                    Configurator,
                    getConfig: () => this.Config,
                    setConfig: cfg => this.Config = cfg
                );

        var onButtonPressed = new OnButtonPressed(
                    helper,
                    Monitor,
                    Configurator,
                    getConfig: () => this.Config,
                    setConfig: cfg => this.Config = cfg
                );

        // var newMachines = Machines.GetNewMachines();

        // if (Config.Machines.Length != newMachines.Length)
        // {
        //     var machinesExcept = newMachines.Except(Config.Machines, new MachinesComparer());

        //     var mac = Config.Machines.Concat(machinesExcept).ToArray();

        //     Config.Machines = mac;

        //     helper.WriteConfig(Config);
        // }

        helper.Events.GameLoop.GameLaunched += onGameLaunched.Main;
        helper.Events.GameLoop.DayStarted += onDayStarted.Main;
        helper.Events.GameLoop.SaveLoaded += onSaveLoaded.Main;
        helper.Events.GameLoop.UpdateTicking += onUpdateTicking.Main;
        helper.Events.Input.ButtonPressed += onButtonPressed.Main;
    }
}