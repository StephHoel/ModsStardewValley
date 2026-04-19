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

        monitor.Log("[ConfigureAllMachines] Starting configuration machines", LogLevel.Trace);

        var (cfgById, cfgByLegacyName) = BuildMachineConfigDictionaries(config);

        var objList = Locations.GetLocations()
                               .Where(l => l.objects is not null)
                               .SelectMany(l => l.objects.Pairs.Select(p => p.Value))
                               .Where(p => p is not null);

        foreach (var obj in objList)
        {
            if (!TryGetConfig(cfgById, cfgByLegacyName, obj, out var cfg))
                continue;

            obj.ConfigureMachine(cfg);
        }
    }

    public static void ConfigureOneMachine(this Object obj, ModConfig config, IMonitor monitor)
    {
        if (!Context.IsMainPlayer) return;

        monitor.Log("[ConfigureOneMachine] Starting configuration machine", LogLevel.Trace);

        var (cfgById, cfgByLegacyName) = BuildMachineConfigDictionaries(config);

        if (!TryGetConfig(cfgById, cfgByLegacyName, obj, out var cfg))
            return;

        obj.ConfigureMachine(cfg);
    }
    #endregion Public Methods

    #region Private Methods
    private static (Dictionary<string, MachineConfig>, Dictionary<string, MachineConfig>) BuildMachineConfigDictionaries(ModConfig config)
    {
        var cfgById = config.Machines.Where(m => !string.IsNullOrWhiteSpace(m.Id))
                                     .GroupBy(m => m.Id, StringComparer.Ordinal)
                                     .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

        var cfgByLegacyName = config.Machines.Where(m => !string.IsNullOrWhiteSpace(m.Name))
                                             .GroupBy(m => m.Name!, StringComparer.Ordinal)
                                             .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

        return (cfgById, cfgByLegacyName);
    }

    private static bool TryGetConfig(Dictionary<string, MachineConfig> cfgById,
                                     Dictionary<string, MachineConfig> cfgByLegacyName,
                                     Object obj,
                                     out MachineConfig cfg)
    {
        if (!string.IsNullOrWhiteSpace(obj.QualifiedItemId) && cfgById.TryGetValue(obj.QualifiedItemId, out cfg!))
            return true;

        if (cfgByLegacyName.TryGetValue(obj.name, out cfg!))
            return true;

        cfg = null!;
        return false;
    }

    private static void ConfigureMachine(this Object obj, MachineConfig cfg)
    {
        if (obj.MinutesUntilReady <= 0)
        {
            obj.modData[Constants.OriginalKey] = string.Empty;
            obj.modData[Constants.AppliedKey] = string.Empty;
            return;
        }

        int original = obj.GetOriginalTime();

        int target = cfg.CalculateTarget(original);

        if (!IsMachineReady(obj, target)) return;

        obj.modData[Constants.OriginalKey] = original.ToString();
        obj.modData[Constants.AppliedKey] = target.ToString();

        if (obj is Cask cask)
        {
            cask.ConfigureCask(target);
            return;
        }

        if (obj.ItemId.IsMachineExcluded()) return;

        obj.MinutesUntilReady = target;

        // if (obj.BaseName.IsIncubatorMachine()) // TODO pensar nessa lógica à parte
        // {
        //     obj.DayUpdate();
        // }
    }

    private static int GetOriginalTime(this Object obj)
    {
        if (obj.modData.TryGetValue(Constants.OriginalKey, out string storedOriginal)
            && int.TryParse(storedOriginal, out int parsedOriginal)
            && parsedOriginal > 0)
            return parsedOriginal;

        return obj.MinutesUntilReady;
    }

    private static bool IsMachineReady(Object obj, int target)
    {
        // if (obj.MinutesUntilReady <= target) return false;

        if (obj.modData.TryGetValue(Constants.AppliedKey, out string applied))
        {
            if (int.TryParse(applied, out int appliedValue))
            {
                if (appliedValue == target && obj.MinutesUntilReady <= target)
                    return false;
            }
            else if (applied == "1" && obj.MinutesUntilReady <= target)
            {
                return false;
            }
        }

        return true;
    }

    public static void ConfigureCask(this Cask cask, int target)
    {
        // monitor.Log($"[ConfigureMachine] [{obj.DisplayName}] original={original}, applied={target}", LogLevel.Debug);

        if (cask.heldObject.Value is not Object held)
        {
            return;
        }

        // TODO o cask não está voltando ao original antes de aceitar novo item

        cask.ResetCask();
        cask.EnsureFreshState(held);

        if (cask.ShouldStopAging())
            return;

        int oldQuality = held.Quality;

        cask.AdvanceQualityItem(held, oldQuality);

        // monitor.Log($"[ConfigureMachine] OldQuality={oldQuality} NewQuality={newQuality}", LogLevel.Debug);

        if (held.Quality > oldQuality)
        {
            cask.modData[Constants.StopAgingKey] = "true";
            cask.readyForHarvest.Value = true;

            cask.onReadyForHarvest();
        }
        else
        {
            held.MinutesUntilReady = target;
            cask.MinutesUntilReady = target;
        }
    }

    private static void EnsureFreshState(this Cask cask, Object held)
    {
        if (!cask.modData.TryGetValue("LastItemId", out var last) || last != held.ItemId)
        {
            cask.modData[Constants.StopAgingKey] = "false";
            cask.modData["LastItemId"] = held.ItemId;
        }
    }

    private static bool ShouldStopAging(this Cask cask)
    {
        return cask.modData.TryGetValue(Constants.StopAgingKey, out var v) && v == "true";
    }

    private static void ResetCask(this Cask cask)
    {
        cask.modData[Constants.StopAgingKey] = "false";
        cask.readyForHarvest.Value = false;
        cask.MinutesUntilReady = -1;
    }

    private static void AdvanceQualityItem(this Cask cask, Object held, int oldQuality)
    {
        var safety = 0;
        const int hardCap = 100; // proteção absoluta
        var maxIterations = Math.Min(cask.daysToMature.Value, hardCap);


        while (oldQuality == held.Quality && safety < maxIterations)
        {
            cask.DayUpdate();
            safety++;
        }
    }
    #endregion Private Methods
}