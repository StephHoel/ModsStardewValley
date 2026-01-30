using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnDayStarted(
    IModHelper helper,
    MachineConfigurator configurator,
    Action<ModConfig> setConfig
)
{
    public void Main(object? sender, DayStartedEventArgs e)
    {
        var config = ConfigUtils.Normalize(helper.ReadConfig<ModConfig>());
        helper.WriteConfig(config);

        setConfig(config);

        if (Context.IsMainPlayer)
            configurator.ConfigureAllMachines(config);
    }
}