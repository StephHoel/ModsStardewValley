using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;

namespace StephHoel.AutoWater;

public class ModEntry : Mod
{
    public override void Entry(IModHelper helper)
    {
        helper.Events.GameLoop.DayStarted += OnDayStarted;
    }

    private void OnDayStarted(object? sender, EventArgs e)
    {
        WaterAllLocations();
        WaterPetBowl();
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
        try
        {
            var farmPetBowl = Game1.getFarm().getBuildingByType("Pet Bowl");

            if (farmPetBowl is PetBowl pet)
                pet.watered.Value = true;
            else
                Monitor.Log("PetBowl do not exist", LogLevel.Debug);
        }
        catch
        {
            Monitor.Log("PetBowl do not exist", LogLevel.Debug);
        }
    }
}