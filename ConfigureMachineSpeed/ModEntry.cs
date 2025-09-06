using StardewModdingAPI;
using StardewModdingAPI.Events;
using Utils;

namespace StephHoel.ConfigureMachineSpeed;

public class ModEntry : Mod
{
    public ModConfig Config { get; set; }
    public IModHelper Helper { get; set; }

    private EventHandlers _eventHandlers;
    private MachineConfigurator _machineConfigurator;

    public override void Entry(IModHelper helper)
    {
        Helper = helper;
        I18n.Init(helper.Translation);

        FileUtils.RemoveObsoleteFiles(helper, ["Utils.pdb"], Monitor);

        Config = helper.ReadConfig<ModConfig>();
        _machineConfigurator = new MachineConfigurator();
        _eventHandlers = new EventHandlers(this, _machineConfigurator);

        // Check if there are new machines that are not in the config file
        var newMachines = Machines.GetNewMachines();
        if (Config.Machines.Length != newMachines.Length)
        {
            var machinesExcept = newMachines.Except(Config.Machines, new MachinesComparer());
            var mac = Config.Machines.Concat(machinesExcept).ToArray();
            Config.Machines = mac;
            helper.WriteConfig(Config);
        }

        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
        helper.Events.GameLoop.DayStarted += _eventHandlers.OnDayStarted;
        helper.Events.GameLoop.SaveLoaded += _eventHandlers.OnSaveLoaded;
        helper.Events.GameLoop.UpdateTicking += _eventHandlers.OnUpdateTicking;
        helper.Events.Input.ButtonPressed += _eventHandlers.OnButtonPressed;
    }

    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        // get Generic Mod Config Menu's API (if it's installed)
        var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (configMenu is null) return;

        // register mod
        configMenu.Register(
            mod: ModManifest,
            reset: () => Config = new ModConfig(),
            save: () => Helper.WriteConfig(Config)
        );

        configMenu.AddSectionTitle(
            mod: ModManifest,
            text: I18n.ConfigTitleGeneralOptions
        );

        // UpdateInterval
        configMenu.AddNumberOption(
            mod: ModManifest,
            name: I18n.ConfigUpdateIntervalName,
            getValue: () => Config.UpdateInterval,
            setValue: val => Config.UpdateInterval = (uint)val,
            min: 1,
            max: 10
        );

        // ReloadConfigKey
        configMenu.AddKeybind(
            mod: ModManifest,
            name: I18n.ConfigReloadConfigKeyName,
            getValue: () => Config.ReloadConfigKey,
            setValue: value => Config.ReloadConfigKey = value
        );

        // Machines
        foreach (var machine in Config.Machines)
        {
            configMenu.AddSectionTitle(
                mod: ModManifest,
                text: () => Machines.GetTranslation(machine.Name)
            );

            configMenu.AddNumberOption(
                mod: ModManifest,
                name: I18n.ConfigTimeName,
                getValue: () => machine.Time,
                setValue: val => machine.Time = val,
                min: 1,
                max: 100
            );

            configMenu.AddBoolOption(
                mod: ModManifest,
                name: I18n.ConfigPercentName,
                getValue: () => machine.UsePercent,
                setValue: val => machine.UsePercent = val
            );
        }
    }
}