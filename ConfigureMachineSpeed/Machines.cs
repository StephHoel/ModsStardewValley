using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Machines;

namespace StephHoel.ConfigureMachineSpeed;

public static class Machines
{
    private static Dictionary<string, string>? CachedLegacyNameToId;
    private static List<MachineConfig>? CachedMachines;

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

    public static List<MachineConfig> MachinesList
        => GetMachines();

    public static MachineConfig[] GetMachines(IEnumerable<MachineConfig>? machineIds = null)
    {
        machineIds ??= [];
        var source = machineIds.ToList();
        source.AddRange(MachinesList);
        source = source.GroupBy(m => m.Id).Select(g => g.First()).ToList();
        return [.. source];
    }

    public static bool TryResolveLegacyNameToId(string legacyName, out string? machineId)
    {
        machineId = null;

        if (string.IsNullOrWhiteSpace(legacyName) || !Context.IsGameLaunched)
            return false;

        CachedLegacyNameToId ??= BuildLegacyNameToIdMap();
        return CachedLegacyNameToId.TryGetValue(legacyName, out machineId);
    }

    private static List<MachineConfig> GetMachines()
    {
        if (CachedMachines is not null)
            return CachedMachines;

        var machines = new HashSet<MachineConfig>();

        var machineData = Game1.content.Load<Dictionary<string, MachineData>>("Data/Machines");
        foreach (var machineId in machineData.Keys)
            machines.Add(new MachineConfig(machineId));



        CachedMachines = machines.GroupBy(m => m.Id)
                                 .Select(g => g.First())
                                 .OrderBy(m => m.Id, StringComparer.OrdinalIgnoreCase)
                                 .ToList();

        return CachedMachines;
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