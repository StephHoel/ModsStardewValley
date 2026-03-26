using StardewModdingAPI;

namespace StephHoel.ConfigureMachineSpeed.Config;

public class GenericModConfigMenu
{
    public static void Register(
        IManifest manifest,
        IModHelper helper,
        IMonitor monitor
    )
    {
        // get Generic Mod Config Menu's API (if it's installed)
        var configMenu = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (configMenu is null)
        {
            // monitor.Log("[GenericModConfigMenu] GenericModConfigMenu not found", LogLevel.Trace);
            return;
        }

        monitor.Log("[GenericModConfigMenu] Registering options with GenericModConfigMenu", LogLevel.Trace);

        var config = helper.ReadConfig<ModConfig>();

        configMenu.Unregister(manifest);

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
        foreach (var machine in config.Machines.OrderByMachineName(monitor))
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