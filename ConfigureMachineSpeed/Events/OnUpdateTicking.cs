using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnUpdateTicking(ModConfig config, MachineConfigurator machineConfigurator)
{
    public void Main(object? sender, UpdateTickingEventArgs e)
    {
        if (Context.IsMainPlayer && e.IsMultipleOf(config.UpdateInterval))
            machineConfigurator.ConfigureAllMachines(config);
    }
}