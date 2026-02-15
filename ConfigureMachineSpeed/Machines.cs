using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Machines;

namespace StephHoel.ConfigureMachineSpeed;

public class Machines
{
    private static Dictionary<string, string>? CachedLegacyNameToId;
    private static List<string>? CachedMachineIds;

    public static string GetTranslation(string machineId)
    {
        if (!Context.IsGameLaunched)
            return machineId;

        var itemData = ItemRegistry.GetData(machineId);
        if (!string.IsNullOrWhiteSpace(itemData?.DisplayName))
            return itemData.DisplayName;

        if (!string.IsNullOrWhiteSpace(itemData?.InternalName))
            return itemData.InternalName;

        return machineId;
    }

    public static List<string> MachineIds
        => GetMachineIds();

    public static MachineConfig[] GetNewMachines(IEnumerable<string>? machineIds = null)
    {
        var source = machineIds?.Distinct(StringComparer.Ordinal).ToList() ?? MachineIds;
        return [.. source.Select(id => new MachineConfig(id))];
    }

    public static MachineConfig[] SetMachines(IEnumerable<MachineConfig?> machines, IEnumerable<string>? machineIds = null)
    {
        var machinesSet = new HashSet<MachineConfig>(machines.Where(m => m != null).Cast<MachineConfig>(), new MachinesComparer());

        foreach (var machine in GetNewMachines(machineIds))
            machinesSet.Add(machine);

        return [.. machinesSet];
    }

    public static bool TryResolveLegacyNameToId(string legacyName, out string machineId)
    {
        machineId = string.Empty;

        if (string.IsNullOrWhiteSpace(legacyName) || !Context.IsGameLaunched)
            return false;

        CachedLegacyNameToId ??= BuildLegacyNameToIdMap();
        return CachedLegacyNameToId.TryGetValue(legacyName, out machineId);
    }

    private static List<string> GetMachineIds()
    {
        if (CachedMachineIds is not null)
            return CachedMachineIds;

        var machineIds = new HashSet<string>(StringComparer.Ordinal);

        var machineData = Game1.content.Load<Dictionary<string, MachineData>>("Data/Machines");
        foreach (var machineId in machineData.Keys)
            machineIds.Add(machineId);

        CachedMachineIds = [.. machineIds.OrderBy(id => id, StringComparer.OrdinalIgnoreCase)];

        return CachedMachineIds;
    }

    private static Dictionary<string, string> BuildLegacyNameToIdMap()
    {
        var map = new Dictionary<string, string>(StringComparer.Ordinal);
        var machineData = Game1.content.Load<Dictionary<string, MachineData>>("Data/Machines");

        foreach (var machineId in machineData.Keys)
        {
            var itemData = ItemRegistry.GetData(machineId);
            if (!string.IsNullOrWhiteSpace(itemData?.InternalName))
                map.TryAdd(itemData.InternalName, machineId);

            if (!string.IsNullOrWhiteSpace(itemData?.DisplayName))
                map.TryAdd(itemData.DisplayName, machineId);
        }

        return map;
    }
}

public class MachinesComparer : IEqualityComparer<MachineConfig>
{
    public bool Equals(MachineConfig x, MachineConfig y)
    {
        return x.Id == y.Id;
    }

    public int GetHashCode(MachineConfig obj)
    {
        return obj.Id?.GetHashCode() ?? default;
    }
}