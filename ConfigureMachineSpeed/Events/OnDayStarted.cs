using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnDayStarted(ModConfig config, MachineConfigurator machineConfigurator)
{
    public void Main(object? sender, DayStartedEventArgs e)
    {
        if (Context.IsMainPlayer)
            machineConfigurator.ConfigureAllMachines(config);
    }
}