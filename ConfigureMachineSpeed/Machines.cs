using System.Globalization;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Machines;

namespace StephHoel.ConfigureMachineSpeed;

public static class Machines
{
    private static Dictionary<string, string>? CachedLegacyNameToId;
    private static List<string>? CachedMachineIds;

    public static string GetTranslation(string machineId, IMonitor? monitor = null)
    {
        if (!Context.IsGameLaunched)
            return machineId;

        var itemData = ItemRegistry.GetData(machineId);

        // monitor?.Log($"[GetTranslation] MachineId={machineId} DisplayName={itemData?.DisplayName} InternalName={itemData?.InternalName}", LogLevel.Debug);

        if (!string.IsNullOrWhiteSpace(itemData?.DisplayName))
            return itemData.DisplayName;

        if (!string.IsNullOrWhiteSpace(itemData?.InternalName))
            return itemData.InternalName;

        return machineId;
    }

    public static MachineConfig[] OrderByMachineName(this MachineConfig[] machines, IMonitor monitor)
    {
        foreach (var machine in machines)
            machine.Name = GetTranslation(machine.Id, monitor);

        return machines
            .OrderBy(m => GetTranslation(m.Id, monitor), StringComparer.CurrentCultureIgnoreCase)
            .ToArray();
    }

    public static List<string> MachineIds
        => GetMachineIds();

    public static MachineConfig[] GetNewMachines(IEnumerable<string>? machineIds = null)
    {
        var source = machineIds?.Distinct(StringComparer.Ordinal).ToList() ?? MachineIds;

        return source.Select(id => new MachineConfig(id)).ToArray();
    }

    public static MachineConfig[] SetMachines(IEnumerable<MachineConfig?> machines, IEnumerable<string>? machineIds = null)
    {
        var machinesSet = new HashSet<MachineConfig>(machines.Where(m => m != null).Cast<MachineConfig>(), new MachinesComparer());

        foreach (var machine in GetNewMachines(machineIds))
            machinesSet.Add(machine);

        return machinesSet.ToArray();
    }

    public static bool TryResolveLegacyNameToId(string legacyName, out string? machineId)
    {
        machineId = null;

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

        CachedMachineIds = machineIds.OrderBy(id => id, StringComparer.OrdinalIgnoreCase).ToList();

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

    public static string? GetIdByMachineName(string machineName)
    {
        var map = BuildLegacyNameToIdMap();

        if (map.TryGetValue(machineName, out var id))
            return id;

        return null;
    }
}

public class MachinesComparer : IEqualityComparer<MachineConfig>
{
    public bool Equals(MachineConfig? x, MachineConfig? y)
        => x?.Id == y?.Id && x?.Name == y?.Name;

    public int GetHashCode(MachineConfig obj)
        => obj.Id?.GetHashCode() ?? default;
}