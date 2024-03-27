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
        foreach (var location in Locations.List)
        {
            var locationGame = Game1.getLocationFromName(location);
            // Garden Pots
            if (locationGame is not null)
            {
                //Monitor.Log($"{location} is not nul", LogLevel.Debug);
                var objects = locationGame.objects.Values.OfType<IndoorPot>();
                foreach (var pot in objects)
                {
                    //Monitor.Log($"{location} with Garden Pot to add water", LogLevel.Debug);
                    pot.Water();
                }

                // Hoe Dirts
                var terrains = locationGame.terrainFeatures.Values.OfType<HoeDirt>();
                foreach (var terrain in terrains)
                {
                    //Monitor.Log($"{location} with HoeDirt to add water", LogLevel.Debug);
                    terrain.state.Value = 1;
                }
            }
            else
            {
                //Monitor.Log($"{location} is null", LogLevel.Debug);
            }
        }
    }
}