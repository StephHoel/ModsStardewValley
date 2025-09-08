using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnSaveLoaded(ModConfig config, MachineConfigurator machineConfigurator)
{
    public void Main(object? sender, SaveLoadedEventArgs e)
    {
        if (Context.IsMainPlayer)
            machineConfigurator.ConfigureAllMachines(config);
    }
}