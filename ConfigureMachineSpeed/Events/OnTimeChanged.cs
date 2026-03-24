using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnTimeChanged(IModHelper helper, IMonitor monitor)
{
    public void Main(object? sender, TimeChangedEventArgs t)
    {
        if (!Context.IsMainPlayer)
        {
            // monitor.Log("[OnTimeChanged] Skipping because not main player", LogLevel.Trace);
            return;
        }

        helper.ReadConfig<ModConfig>().ConfigureAllMachines(monitor);
    }
}