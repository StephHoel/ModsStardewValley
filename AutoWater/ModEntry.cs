using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;

namespace AutoWater;

public class ModEntry : Mod
{
    public override void Entry(IModHelper helper)
    {
        helper.Events.GameLoop.DayStarted += WaterEverything;
    }

    private async void WaterEverything(object sender, EventArgs e)
    {
        foreach (var location in Utils.Locations.GetLocations())
        {
            //Monitor.Log($"{location} is {(locationGame is null ? "null" : "not null")}", LogLevel.Debug);

            if (location is not null)
            {
                // Garden Pots
                var objects = location.objects.Values.OfType<IndoorPot>();
                foreach (var pot in objects)
                {
                    //Monitor.Log($"{location} with Garden Pot to add water", LogLevel.Debug);
                    pot.Water();
                }

                // Hoe Dirts
                var terrains = location.terrainFeatures.Values.OfType<HoeDirt>();
                foreach (var terrain in terrains)
                {
                    //Monitor.Log($"{location} with HoeDirt to add water", LogLevel.Debug);
                    terrain.state.Value = 1;
                }
            }
        }
    }
}