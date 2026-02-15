using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnButtonPressed(
    IModHelper helper,
    IMonitor monitor,
    MachineConfigurator configurator,
    Func<ModConfig> getConfig,
    Action<ModConfig> setConfig
)
{
    public void Main(object? sender, ButtonPressedEventArgs e)
    {
        if (!Context.IsPlayerFree || Game1.currentMinigame != null)
            return;

        var cfg = getConfig();

        if (e.Button != cfg.ReloadConfigKey)
            return;

        var newCfg = ConfigUtils.Normalize(helper.ReadConfig<ModConfig>(), monitor);
        helper.WriteConfig(newCfg);

        setConfig(newCfg);

        configurator.ConfigureAllMachines(newCfg);

        Game1.addHUDMessage(new HUDMessage(I18n.Message(), 2));
        monitor.Log("Config recharge and reapplied.", LogLevel.Info);
    }
}