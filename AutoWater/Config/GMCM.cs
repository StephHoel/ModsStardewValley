using StardewModdingAPI;

namespace StephHoel.AutoWater.Config;

public static class GMCM
{
    public static void Register(
        IModHelper helper,
        IManifest manifest,
        Func<ModConfig> getConfig,
        Action<ModConfig> setConfig)
    {
        var configMenu = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        if (configMenu is null)
            return;

        configMenu.Register(
            mod: manifest,
            reset: () =>
            {
                var config = new ModConfig();
                setConfig(config);
                helper.WriteConfig(config);
            },
            save: () => helper.WriteConfig(getConfig())
        );

        configMenu.AddSectionTitle(
            mod: manifest,
            text: I18n.Title
        );

        configMenu.AddKeybind(
            mod: manifest,
            name: I18n.ConfigButton,
            getValue: () => getConfig().Toggle,
            setValue: value =>
            {
                var config = getConfig();
                config.Toggle = value;
                setConfig(config);
            }
        );

        configMenu.AddBoolOption(
            mod: manifest,
            name: I18n.ConfigToggle,
            getValue: () => getConfig().IsActive,
            setValue: value =>
            {
                var config = getConfig();
                config.IsActive = value;
                setConfig(config);
            }
        );
    }
}
