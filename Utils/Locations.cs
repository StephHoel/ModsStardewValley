using StardewModdingAPI.Framework.ModLoading.Rewriters.StardewValley_1_6;
using StardewValley;
using StardewValley.Buildings;

namespace Utils;

public class Locations
{
    public static IEnumerable<GameLocation> GetLocations()
    {
        return Game1.locations.Concat(
            from location in Game1.locations.OfType<BuildableGameLocationFacade>()
            from building in (IEnumerable<Building>)location.buildings
            where building.indoors.Value != null
            select building.indoors.Value
            );
    }
}