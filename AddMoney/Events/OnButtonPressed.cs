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

        var requestedGold = config.GoldToAdd;

        if (requestedGold <= 0)
        {
            Game1.addHUDMessage(new HUDMessage(I18n.InvalidAmount(), HUDMessage.error_type));
            monitor.Log($"Invalid GoldToAdd value ({requestedGold}). The player's balance was not changed.", LogLevel.Warn);
            return;
        }

        var player = Game1.player;
        if (player is null)
        {
            monitor.Log("The player is unavailable. The balance was not changed.", LogLevel.Warn);
            return;
        }

        // Money is stored as an Int32 by the game. Calculate the remaining capacity
        // using Int64 so the subtraction and comparison cannot overflow.
        var remainingCapacity = (long)int.MaxValue - player.Money;
        var actualGold = (int)Math.Min(requestedGold, Math.Max(0L, remainingCapacity));

        if (actualGold == 0)
        {
            Game1.addHUDMessage(new HUDMessage(I18n.BalanceLimitReached(), HUDMessage.error_type));
            monitor.Log($"{player.Name}'s balance is already at the supported limit.", LogLevel.Warn);
            return;
        }

        player.Money += actualGold;

        Game1.addHUDMessage(new HUDMessage($"{actualGold}{I18n.Message()}", 2));

        if (actualGold < requestedGold)
        {
            monitor.Log($"The requested addition ({requestedGold}G) exceeded the remaining balance capacity. Added {actualGold}G instead.", LogLevel.Info);
        }
        else
        {
            monitor.Log($"{player.Name} added {actualGold}G.", LogLevel.Debug);
        }
    }
}
