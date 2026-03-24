using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnGameLaunched(
    IManifest manifest,
    IModHelper helper,
    IMonitor monitor
)
{
    public void Main(object? sender, GameLaunchedEventArgs e)
    {
        // monitor.Log("[OnGameLaunched] Starting configuration flow", LogLevel.Trace);

        var config = helper.ReadConfig<ModConfig>()
                           .NormalizeMachineConfig(monitor);

        helper.WriteConfig(config);
        // monitor?.Log("[ConfigUtils] Config written to disk", LogLevel.Trace);

        ConfigGenericModConfigMenu(config);
    }

    private void ConfigGenericModConfigMenu(ModConfig config)
    {
        // get Generic Mod Config Menu's API (if it's installed)
        var configMenu = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (configMenu is null)
        {
            // monitor.Log("[OnGameLaunched] GenericModConfigMenu not found", LogLevel.Trace);
            return;
        }

        // monitor.Log("[OnGameLaunched] Registering options with GenericModConfigMenu", LogLevel.Trace);

        // register mod
        configMenu.Register(
            mod: manifest,
            reset: () => config = new ModConfig(),
            save: () => helper.WriteConfig(config)
        );

        configMenu.AddSectionTitle(
            mod: manifest,
            text: I18n.ConfigTitleGeneralOptions
        );

        // Machines
        foreach (var machine in config.Machines.OrderByMachineName())
        {
            configMenu.AddSectionTitle(
                mod: manifest,
                text: () => machine.Name ?? Machines.GetTranslation(machine.Id)
            );

            configMenu.AddNumberOption(
                mod: manifest,
                name: I18n.ConfigTimeName,
                getValue: () => machine.Time,
                setValue: val => machine.Time = val,
                min: 1,
                max: 100
            );

            configMenu.AddBoolOption(
                mod: manifest,
                name: I18n.ConfigPercentName,
                getValue: () => machine.UsePercent,
                setValue: val => machine.UsePercent = val
            );
        }
    }
}