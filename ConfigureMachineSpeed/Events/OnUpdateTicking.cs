using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnUpdateTicking(
    IModHelper helper,
    MachineConfigurator configurator,
    Func<ModConfig> getConfig,
    Action<ModConfig> setConfig
)
{
    public void Main(object? sender, UpdateTickingEventArgs e)
    {
        var cfg = getConfig();

        var config = ConfigUtils.Normalize(helper.ReadConfig<ModConfig>());
        helper.WriteConfig(config);

        setConfig(config);

        if (!Context.IsMainPlayer)
            return;

        if (e.IsMultipleOf(cfg.UpdateInterval))
            configurator.ConfigureAllMachines(config);
    }
}