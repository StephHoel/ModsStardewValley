using StardewModdingAPI;
using StardewModdingAPI.Events;
using StephHoel.ConfigureMachineSpeed.Config;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnRenderedActiveMenu(IManifest ModManifest, IModHelper helper, IMonitor Monitor)
{
    public void Main(object? sender, RenderedActiveMenuEventArgs e)
    {
        GenericModConfigMenu.Register(ModManifest, helper, Monitor);
    }
}
