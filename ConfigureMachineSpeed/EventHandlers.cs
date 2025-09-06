using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StephHoel.ConfigureMachineSpeed;

public class EventHandlers(ModEntry modEntry, MachineConfigurator machineConfigurator)
{
    public void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
    {
        if (Context.IsMainPlayer)
            machineConfigurator.ConfigureAllMachines(modEntry.Config);
    }

    public void OnUpdateTicking(object? sender, UpdateTickingEventArgs e)
    {
        if (Context.IsMainPlayer && e.IsMultipleOf(modEntry.Config.UpdateInterval))
            machineConfigurator.ConfigureAllMachines(modEntry.Config);
    }

    public void OnDayStarted(object? sender, DayStartedEventArgs e)
    {
        if (Context.IsMainPlayer)
            machineConfigurator.ConfigureAllMachines(modEntry.Config);
    }

    public void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
        if (Context.IsPlayerFree && Game1.currentMinigame == null && e.Button == modEntry.Config.ReloadConfigKey)
        {
            modEntry.Config = ConfigUtils.ProcessConfig(modEntry.Helper.ReadConfig<ModConfig>(), modEntry.Config);
            Game1.addHUDMessage(new(I18n.Message(), 2));
        }
    }
}