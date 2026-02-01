using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.AddMoney.Events;

/// <summary>Handles the GameLaunched event and config menu registration.</summary>
public class OnGameLaunched(
    IModHelper helper,
    IManifest manifest,
    Func<ModConfig> getConfig,
    Action<ModConfig> setConfig
)
{
    public void Main(object? sender, GameLaunchedEventArgs e)
    {
        var configMenu = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

        if (configMenu is null)
            return;

        configMenu.Register(
            mod: manifest,
            reset: () =>
            {
                var cfg = ConfigUtils.Normalize(new ModConfig());
                setConfig(cfg);
            },
            save: () => helper.WriteConfig(getConfig())
        );

        configMenu.AddSectionTitle(
            mod: manifest,
            text: I18n.ConfigTitleGeneralOptions
        );

        configMenu.AddKeybind(
            mod: manifest,
            name: I18n.ConfigButtonAddToMoneyKeyName,
            getValue: () => getConfig().ButtonToAddMoney,
            setValue: value =>
            {
                var cfg = getConfig();
                cfg.ButtonToAddMoney = value;
                setConfig(cfg);
            }
        );

        configMenu.AddNumberOption(
            mod: manifest,
            name: I18n.ConfigGoldAddName,
            getValue: () => getConfig().GoldToAdd,
            setValue: value =>
            {
                var cfg = getConfig();
                cfg.GoldToAdd = value;
                setConfig(ConfigUtils.Normalize(cfg));
            }
        );
    }
}