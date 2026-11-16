using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StephHoel.AutoWater.Config;
using Utils;

namespace StephHoel.AutoWater;

public class ModEntry : Mod
{
    private ModConfig Config = new();

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        FileUtils.RemoveObsoleteFiles(helper, Monitor);
        Config = helper.ReadConfig<ModConfig>();

        helper.Events.GameLoop.GameLaunched += (_, _) =>
        {
            GMCM.Register(helper, ModManifest, () => Config, config => Config = config);
        };

        helper.Events.Input.ButtonPressed += OnButtonPressed;

        helper.Events.GameLoop.DayStarted += (_, _) =>
        {
            // Avoid running twice in multiplayer.
            if (!Context.IsMainPlayer || !Config.IsActive) return;

            WaterAllLocations();
            WaterPetBowl();
        };
    }

    private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
        if (!Context.IsWorldReady || !Context.IsMainPlayer || Game1.eventUp)
            return;

        // Do not steal the configured key while the player is interacting with a
        // menu or entering text in the chat box.
        if (Game1.activeClickableMenu is not null || Game1.chatBox?.isActive() == true)
            return;

        if (e.Button != Config.Toggle)
            return;

        Config.IsActive = !Config.IsActive;
        Helper.WriteConfig(Config);

        var message = Config.IsActive ? I18n.Enabled() : I18n.Disabled();
        Game1.addHUDMessage(new HUDMessage(message, 2));
        Monitor.Log($"AutoWater {(Config.IsActive ? "enabled" : "disabled")} using {e.Button}.", LogLevel.Debug);
    }

    private static void WaterAllLocations()
    {
        foreach (var location in Game1.locations)
        {
            WaterGardenPots(location);
            WaterHoeDirts(location);
        }
    }

    private static void WaterGardenPots(GameLocation location)
    {
        foreach (var pot in location.objects.Values.OfType<IndoorPot>())
            pot.Water();
    }

    private static void WaterHoeDirts(GameLocation location)
    {
        foreach (var terrain in location.terrainFeatures.Values.OfType<HoeDirt>())
            terrain.state.Value = 1;
    }

    private void WaterPetBowl()
    {
        // Pet bowl can be missing (e.g., no pet yet, custom farms, etc.).
        try
        {
            var farm = Game1.getFarm();
            var bowl = farm?.getBuildingByType("Pet Bowl");

            if (bowl is PetBowl pet)
                pet.watered.Value = true;
            else
                Monitor.Log("Pet bowl not found.", LogLevel.Trace);
        }
        catch (Exception ex)
        {
            Monitor.Log($"Failed to water pet bowl: {ex}", LogLevel.Trace);
        }
    }
}
