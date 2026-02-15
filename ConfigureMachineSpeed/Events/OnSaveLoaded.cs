using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnSaveLoaded(
    IModHelper helper,
    IMonitor monitor,
    MachineConfigurator configurator,
    Action<ModConfig> setConfig
)
{
    public void Main(object? sender, SaveLoadedEventArgs e)
    {
        var config = ConfigUtils.Normalize(helper.ReadConfig<ModConfig>(), monitor);
        helper.WriteConfig(config);

        setConfig(config);

        if (Context.IsMainPlayer)
            configurator.ConfigureAllMachines(config);
    }
}