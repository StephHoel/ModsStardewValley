using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.AddMoney.Events;

/// <summary>
/// Handles the GameLaunched event and config menu registration.
/// </summary>
public class OnGameLaunched(IModHelper helper, IMod mod, ModConfig config)
{
    private readonly IManifest _modManifest = mod.ModManifest;

    public void Main(object? sender, GameLaunchedEventArgs e)
    {
        var configMenu = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

        if (configMenu is null)
            return;

        configMenu.Register(
            mod: _modManifest,
            reset: () => config = new ModConfig(),
            save: () => helper.WriteConfig(config)
        );

        configMenu.AddSectionTitle(
            mod: _modManifest,
            text: I18n.ConfigTitleGeneralOptions
        );

        configMenu.AddKeybind(
            mod: _modManifest,
            name: I18n.ConfigButtonAddToMoneyKeyName,
            getValue: () => config.ButtonToAddMoney,
            setValue: value => config.ButtonToAddMoney = value
        );

        configMenu.AddNumberOption(
            mod: _modManifest,
            name: I18n.ConfigGoldAddName,
            getValue: () => config.GoldToAdd,
            setValue: val => config.GoldToAdd = val
        );
    }
}