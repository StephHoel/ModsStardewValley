using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace AutoWater;

public class ModEntry : Mod
{
    public override void Entry(IModHelper helper)
    {
        helper.Events.GameLoop.DayStarted += WaterEverything;
    }

    private void WaterEverything(object sender, EventArgs e)
    {
        foreach (HoeDirt farm in Game1.getLocationFromName("Farm").terrainFeatures.Values.OfType<HoeDirt>())
        {
            farm.state.Value = 1;
        }
        foreach (HoeDirt greenhouse in Game1.getLocationFromName("Greenhouse").terrainFeatures.Values.OfType<HoeDirt>())
        {
            greenhouse.state.Value = 1;
        }
        foreach (HoeDirt GrandpasShed in Game1.getLocationFromName("Custom_GrandpasShedGreenhouse").terrainFeatures.Values.OfType<HoeDirt>())
        {
            GrandpasShed.state.Value = 1;
        }
        foreach (HoeDirt IslandWest in Game1.getLocationFromName("IslandWest").terrainFeatures.Values.OfType<HoeDirt>())
        {
            IslandWest.state.Value = 1;
        }
        foreach (HoeDirt CommGarden in Game1.getLocationFromName("Custom_Garden").terrainFeatures.Values.OfType<HoeDirt>())
        {
            CommGarden.state.Value = 1;
        }
    }
}