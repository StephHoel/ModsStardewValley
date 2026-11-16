using StardewValley;
using StardewValley.Locations;

namespace Utils;

/// <summary>
/// Enumerates loaded world locations and the loaded interiors of buildable
/// locations using public Stardew Valley APIs.
/// </summary>
public static class Locations
{
    public static IEnumerable<GameLocation> GetLocations()
    {
        var visited = new HashSet<GameLocation>();

        foreach (var location in Game1.locations)
        {
            if (location is null || !visited.Add(location))
                continue;

            yield return location;

            if (location is not BuildableGameLocation buildableLocation)
                continue;

            foreach (var building in buildableLocation.buildings)
            {
                var interior = building.indoors.Value;
                if (interior is null || !visited.Add(interior))
                    continue;

                yield return interior;
            }
        }
    }
}
