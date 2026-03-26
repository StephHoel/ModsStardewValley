using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnGameLaunched(IModHelper helper, IMonitor monitor)
{
    public void Main(object? sender, GameLaunchedEventArgs e)
    {
        // monitor.Log("[OnGameLaunched] Starting configuration flow", LogLevel.Trace);

        var config = helper.ReadConfig<ModConfig>()
                           .NormalizeMachineConfig(monitor);

        helper.WriteConfig(config);
        // monitor?.Log("[OnGameLaunched] Config written to disk", LogLevel.Trace);
    }
}