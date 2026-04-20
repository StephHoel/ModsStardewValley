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
        if (obj.MinutesUntilReady <= 1)
        {
            obj.ResetMachine();
            return;
        }

        int original = obj.MinutesUntilReady;
        int target = cfg.CalculateTarget(original);

        if (!IsMachineReady(obj, target)) return;
        if (obj.ItemId.IsMachineExcluded()) return;

        obj.modData[Constants.AppliedKey] = target.ToString();

        if (obj.ProcessCask(target)) return;

        obj.MinutesUntilReady = target;

        // if (obj.BaseName.IsIncubatorMachine()) // TODO pensar nessa lógica à parte
        // {
        //     obj.DayUpdate();
        // }
    }

    private static bool IsMachineReady(Object obj, int target)
    {
        if (obj is Cask) return true;

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

    private static bool ProcessCask(this Object obj, int target)
    {
        if (obj is not Cask cask) return false;

        if (cask.MinutesUntilReady > (999999 - target)) return true;

        if (cask.heldObject.Value is not Object held) return true;

        if (!cask.ShouldContinueAging()) return true;

        cask.AdvanceQualityItem();
        return true;
    }

    private static bool ShouldContinueAging(this Cask cask)
    {
        return !cask.modData.TryGetValue(Constants.StopAgingKey, out var v) || v != "true";
    }

    private static void AdvanceQualityItem(this Cask cask)
    {
        var safety = 0;
        const int hardCap = 100;
        var maxIterations = Math.Min(cask.daysToMature.Value, hardCap);

        var item = cask.heldObject.Value;
        int oldQuality = item.Quality;

        while (oldQuality == item.Quality && safety < maxIterations)
        {
            cask.DayUpdate();
            safety++;
        }

        if (item.Quality > oldQuality)
        {
            cask.modData[Constants.StopAgingKey] = "true";
            cask.MinutesUntilReady = 1;
            cask.readyForHarvest.Value = true;
        }
    }
    #endregion Private Methods
}