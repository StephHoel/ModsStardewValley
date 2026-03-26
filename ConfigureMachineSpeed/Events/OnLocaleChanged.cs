using StardewModdingAPI;
using StardewModdingAPI.Events;
using StephHoel.ConfigureMachineSpeed.Config;

namespace StephHoel.ConfigureMachineSpeed.Events;


public class OnLocaleChanged(IManifest ModManifest, IModHelper helper, IMonitor Monitor)
{
    public void Main(object? sender, LocaleChangedEventArgs e)
    {
        GenericModConfigMenu.Register(ModManifest, helper, Monitor);
    }
}