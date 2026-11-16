using StardewModdingAPI;
using StardewValley.Objects;
using StephHoel.ConfigureMachineSpeed.Config;
using Utils;
using Object = StardewValley.Object;

namespace StephHoel.ConfigureMachineSpeed;

public static class MachineConfigurator
{
    #region Public Methods
    public static void ConfigureAllMachines(this ModConfig config, IMonitor monitor)
    {
        if (!Context.IsMainPlayer) return;

        monitor.Log("[ConfigureAllMachines] Starting machine configuration.", LogLevel.Trace);

        var (cfgById, cfgByLegacyName) = BuildMachineConfigDictionaries(config);
        var locations = Locations.GetLocations().Distinct();

        foreach (var location in locations)
        {
            foreach (var obj in location.objects.Values)
            {
                if (obj is null || !TryGetConfig(cfgById, cfgByLegacyName, obj, out var machineConfig))
                    continue;

                obj.ConfigureMachine(machineConfig);
            }
        }
    }

    public static void ConfigureOneMachine(this Object obj, ModConfig config, IMonitor monitor)
    {
        if (!Context.IsMainPlayer) return;

        monitor.Log("[ConfigureOneMachine] Starting single-machine configuration.", LogLevel.Trace);

        var (cfgById, cfgByLegacyName) = BuildMachineConfigDictionaries(config);
        if (TryGetConfig(cfgById, cfgByLegacyName, obj, out var machineConfig))
            obj.ConfigureMachine(machineConfig);
    }
    #endregion Public Methods

    #region Private Methods
    private static (Dictionary<string, MachineConfig>, Dictionary<string, MachineConfig>) BuildMachineConfigDictionaries(ModConfig config)
    {
        var machines = config.Machines ?? [];
        var cfgById = machines.Where(m => !string.IsNullOrWhiteSpace(m.Id))
            .GroupBy(m => m.Id, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

        var cfgByLegacyName = machines.Where(m => !string.IsNullOrWhiteSpace(m.Name))
            .GroupBy(m => m.Name!, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

        return (cfgById, cfgByLegacyName);
    }

    private static bool TryGetConfig(
        Dictionary<string, MachineConfig> cfgById,
        Dictionary<string, MachineConfig> cfgByLegacyName,
        Object obj,
        out MachineConfig cfg)
    {
        // Qualified IDs are authoritative. Legacy internal names are a fallback
        // only for old configuration entries which haven't been migrated yet.
        return MachineConfigResolver.TryResolve(cfgById, cfgByLegacyName, obj.QualifiedItemId, obj.name, out cfg);
    }

    private static void ConfigureMachine(this Object obj, MachineConfig cfg)
    {
        if (obj.MinutesUntilReady <= 1)
        {
            obj.ResetMachine();
            return;
        }

        // Exclusions take precedence over configured speed values.
        if (obj.QualifiedItemId.IsMachineExcluded() || obj.ItemId.IsMachineExcluded())
            return;

        var target = cfg.CalculateTarget(obj.MinutesUntilReady);
        if (!IsMachineReady(obj, target))
            return;

        obj.modData[Constants.AppliedKey] = target.ToString();

        if (obj.ProcessCask())
            return;

        obj.MinutesUntilReady = target;
    }

    private static bool IsMachineReady(Object obj, int target)
    {
        if (obj is Cask)
            return true;

        if (obj.modData.TryGetValue(Constants.AppliedKey, out var applied)
            && int.TryParse(applied, out var appliedValue)
            && appliedValue == target
            && obj.MinutesUntilReady <= target)
            return false;

        return true;
    }

    private static bool ProcessCask(this Object obj)
    {
        if (obj is not Cask cask)
            return false;

        if (cask.heldObject.Value is not Object)
            return true;

        if (!cask.ShouldContinueAging())
            return true;

        cask.AdvanceQualityItem();
        return true;
    }

    private static bool ShouldContinueAging(this Cask cask)
    {
        return !cask.modData.TryGetValue(Constants.StopAgingKey, out var value) || value != "true";
    }

    private static void AdvanceQualityItem(this Cask cask)
    {
        const int hardCap = 100;
        var item = cask.heldObject.Value;
        if (item is null)
            return;

        var originalQuality = item.Quality;
        var maxIterations = Math.Clamp(cask.daysToMature.Value, 1, hardCap);

        for (var iteration = 0; iteration < maxIterations; iteration++)
        {
            if (!ReferenceEquals(cask.heldObject.Value, item))
                return;

            cask.DayUpdate();

            if (cask.heldObject.Value is not Object currentItem
                || !ReferenceEquals(currentItem, item))
                return;

            if (currentItem.Quality <= originalQuality)
                continue;

            cask.modData[Constants.StopAgingKey] = "true";
            cask.MinutesUntilReady = 1;
            cask.readyForHarvest.Value = true;
            return;
        }
    }
    #endregion Private Methods
}
