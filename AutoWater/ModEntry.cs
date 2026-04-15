using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StephHoel.AutoWater.Config;
using Utils;

namespace StephHoel.AutoWater;

public class ModEntry : Mod
{
    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        FileUtils.RemoveObsoleteFiles(helper, Monitor);

        helper.Events.GameLoop.GameLaunched += (_, _) =>
        {
            GMCM.Register(helper, ModManifest);
        };

        helper.Events.GameLoop.DayStarted += (_, _) =>
        {
            // Avoid running twice in multiplayer.
            if (!Context.IsMainPlayer) return;

            if (!helper.ReadConfig<ModConfig>().IsActive) return;

            WaterAllLocations();
            WaterPetBowl();
        };
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