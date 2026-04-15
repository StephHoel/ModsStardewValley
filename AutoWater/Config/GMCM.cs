using StardewModdingAPI;

namespace StephHoel.AutoWater.Config;

public static class GMCM
{
    public static void Register(IModHelper helper, IManifest manifest)
    {

        var configMenu = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

        if (configMenu is null) return;

        var config = helper.ReadConfig<ModConfig>();

        configMenu.Register(
            mod: manifest,
            reset: () => helper.WriteConfig(new ModConfig()),
            save: () => helper.WriteConfig(config)
        );

        configMenu.AddSectionTitle(
            mod: manifest,
            text: I18n.Title
        );

        configMenu.AddKeybind(
            mod: manifest,
            name: I18n.ConfigButton,
            getValue: () => config.Toggle,
            setValue: value =>  config.Toggle = value
        );

        configMenu.AddBoolOption(
            mod: manifest,
            name: I18n.ConfigToggle,
            getValue: () => config.IsActive,
            setValue: value => config.IsActive = value
        );
    }
}