using StardewValley;

namespace Utils;

/// <summary>
/// Enumerates loaded world locations and their loaded, instanced building
/// interiors using the public Stardew Valley 1.6 API.
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

            // Non-instanced interiors (such as the farmhouse and greenhouse)
            // are already in Game1.locations. This API returns interiors which
            // belong to a building instance and aren't listed there separately.
            foreach (var interior in location.GetInstancedBuildingInteriors())
            {
                if (interior is null || !visited.Add(interior))
                    continue;

                yield return interior;
            }
        }
    }
}
