using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnGameLaunched(
    IManifest manifest,
    IModHelper helper,
    Action<ModConfig> setConfig
)
{
    public void Main(object? sender, GameLaunchedEventArgs e)
    {
        var config = ConfigUtils.Normalize(helper.ReadConfig<ModConfig>());
        helper.WriteConfig(config);

        setConfig(config);

        // get Generic Mod Config Menu's API (if it's installed)
        var configMenu = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (configMenu is null) return;

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

        // UpdateInterval
        configMenu.AddNumberOption(
            mod: manifest,
            name: I18n.ConfigUpdateIntervalName,
            getValue: () => config.UpdateInterval,
            setValue: val => config.UpdateInterval = (uint)val,
            min: 1,
            max: 10
        );

        // ReloadConfigKey
        configMenu.AddKeybind(
            mod: manifest,
            name: I18n.ConfigReloadConfigKeyName,
            getValue: () => config.ReloadConfigKey,
            setValue: value => config.ReloadConfigKey = value
        );

        // Machines
        foreach (var machine in config.Machines)
        {
            configMenu.AddSectionTitle(
                mod: manifest,
                text: () => Machines.GetTranslation(machine.Name)
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