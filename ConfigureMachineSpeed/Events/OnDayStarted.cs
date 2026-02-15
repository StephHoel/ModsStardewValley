using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnDayStarted(
    MachineConfigurator configurator,
    Func<ModConfig> getConfig
)
{
    public void Main(object? sender, DayStartedEventArgs e)
    {
        if (Context.IsMainPlayer)
            configurator.ConfigureAllMachines(getConfig());
    }
}