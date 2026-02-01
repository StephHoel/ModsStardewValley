using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StephHoel.AddMoney.Events;

public class OnButtonPressed(Func<ModConfig> getConfig, IMonitor monitor)
{
    /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event data.</param>
    public void Main(object? sender, ButtonPressedEventArgs e)
    {
        if (!Context.IsWorldReady)
            return;

        var config = getConfig();

        if (e.Button != config.ButtonToAddMoney)
            return;

        var gold = config.GoldToAdd;

        Game1.player.Money += gold;

        Game1.addHUDMessage(new HUDMessage($"{gold}{I18n.Message()}", 2));

        monitor.Log($"{Game1.player.Name} added {gold}G.", LogLevel.Debug);
    }
}