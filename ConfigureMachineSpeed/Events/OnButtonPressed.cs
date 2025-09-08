using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StephHoel.ConfigureMachineSpeed.Events;

public class OnButtonPressed(ModConfig config, IModHelper helper)
{
    public void Main(object? sender, ButtonPressedEventArgs e)
    {
        if (Context.IsPlayerFree && Game1.currentMinigame == null && e.Button == config.ReloadConfigKey)
        {
            config = ConfigUtils.ProcessConfig(helper.ReadConfig<ModConfig>(), config);
            Game1.addHUDMessage(new(I18n.Message(), 2));
        }
    }
}