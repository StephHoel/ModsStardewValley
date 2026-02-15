using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnUpdateTicking(
    MachineConfigurator configurator,
    Func<ModConfig> getConfig
)
{
    public void Main(object? sender, UpdateTickingEventArgs e)
    {
        if (!Context.IsMainPlayer)
            return;

        var config = getConfig();

        if (e.IsMultipleOf(config.UpdateInterval))
            configurator.ConfigureAllMachines(config);
    }
}