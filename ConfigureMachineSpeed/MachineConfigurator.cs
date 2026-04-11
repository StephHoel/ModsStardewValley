using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using Utils;
using Object = StardewValley.Object;

namespace StephHoel.ConfigureMachineSpeed;

public static class MachineConfigurator
{
    private const string AppliedKey = "StephHoel.ConfigureMachineSpeed.applied";
    private const string OriginalKey = "StephHoel.ConfigureMachineSpeed.original";
    private const string StopAgingKey = "StephHoel.ConfigureMachineSpeed.stopAging";

    public static void ConfigureAllMachines(this ModConfig config, IMonitor monitor)
    {
        monitor.Log("[ConfigureAllMachines] Starting configuration machines", LogLevel.Trace);

        var cfgById = config.Machines
            // .Where(m => !m.IsDefault())
            .Where(m => !string.IsNullOrWhiteSpace(m.Id))
            .GroupBy(m => m.Id, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

        var cfgByLegacyName = config.Machines
            .Where(m => /*!m.IsDefault() &&*/ !string.IsNullOrWhiteSpace(m.Name))
            .GroupBy(m => m.Name!, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

        foreach (GameLocation location in Locations.GetLocations())
        {
            if (location.objects is null)
                continue;

            foreach (var pair in location.objects.Pairs)
            {
                if (pair.Value is not Object obj)
                    continue;

                if (TryGetConfig(cfgById, cfgByLegacyName, obj, out var cfg))
                    ConfigureMachine(cfg, obj);
            }
        }
    }

    private static bool TryGetConfig(
        Dictionary<string, MachineConfig> cfgById,
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

    private static void ConfigureMachine(MachineConfig cfg, Object obj)
    {
        // if (cfg.UsePercent && cfg.Time == 100)
        //     return;

        if (obj.MinutesUntilReady <= 0)
        {
            obj.modData.Remove(OriginalKey);
            obj.modData.Remove(AppliedKey);
            return;
        }

        // monitor.Log($"[ConfigureMachine] Found object {obj.Name} (QualifiedItemId={obj.QualifiedItemId}) MinutesUntilReady={obj.MinutesUntilReady}", LogLevel.Trace);

        int original = obj.MinutesUntilReady;

        if (obj.modData.TryGetValue(OriginalKey, out string storedOriginal) &&
            int.TryParse(storedOriginal, out int parsedOriginal) &&
            parsedOriginal > 0)
            original = parsedOriginal;

        // monitor.Log($"[ConfigureMachine] original (from modData? {obj.modData.ContainsKey(OriginalKey)}) = {original}", LogLevel.Debug);

        int target = CalculateTarget(cfg, original);

        // monitor.Log($"[ConfigureMachine] calculated target (UsePercent={cfg.UsePercent}, Time={cfg.Time}) = {target}", LogLevel.Debug);

        if (obj.modData.TryGetValue(AppliedKey, out string applied))
        {
            if (int.TryParse(applied, out int appliedValue))
            {
                if (appliedValue == target && obj.MinutesUntilReady == target)
                    return;
            }
            else if (applied == "1" && obj.MinutesUntilReady == target)
            {
                return;
            }
        }

        obj.modData[OriginalKey] = original.ToString();
        obj.modData[AppliedKey] = target.ToString();

        // if (!obj.BaseName.ContainsIgnoreCase("Cask"))
        obj.MinutesUntilReady = target;

        // if (obj.BaseName.ContainsIgnoreCase("Cask") && obj is Cask cask)
        //     cask.ConfigureCask(target);
    }

    private static void ConfigureCask(this Cask cask, int target)
    {
        // monitor.Log($"[ConfigureMachine] [{obj.DisplayName}] original={original}, applied={target}", LogLevel.Debug);

        if (cask.heldObject.Value == null)
        {
            cask.modData.Remove(StopAgingKey);

            cask.readyForHarvest.Value = false;
            cask.MinutesUntilReady = 0;

            return;
        }

        if (cask?.heldObject.Value is not Object held)
            return;

        if (held.modData.ContainsKey(StopAgingKey))
            return;

        held.MinutesUntilReady = target;

        int oldQuality = held.Quality;

        var days = GetDaysForNextQuality(oldQuality, cask.agingRate.Value);

        for (var i = 0; i < days; i++)
            cask.DayUpdate();

        var newQuality = held.Quality;

        // monitor.Log($"[ConfigureMachine] OldQuality={oldQuality} NewQuality={newQuality}", LogLevel.Debug);

        if (newQuality > oldQuality)
        {
            cask.modData[StopAgingKey] = "true";
            cask.MinutesUntilReady = target;
            held.MinutesUntilReady = target;
            cask.readyForHarvest.Value = true;
        }
    }

    private static int CalculateTarget(MachineConfig cfg, int original)
    {
        if (cfg is null)
            return original;

        // if (cfg.UsePercent)
        // {
        //     int percent = cfg.Time;
        //     int calculated = (int)Math.Ceiling(original * (percent / 100.0));
        //     calculated = Math.Max(1, calculated);
        //     calculated = Math.Min(100, calculated);
        //     cfg.Time = calculated;
        // }

        return ConfigUtils.RoundedTime(cfg.Time);
    }

    private static int GetDaysForNextQuality(int quality, float agingRate)
    { // TODO ajustar baseado no tempo total
        return quality switch
        {
            // normal → silver
            0 => (int)agingRate,

            // silver → gold
            1 => (int)agingRate,

            // gold → iridium
            2 => (int)(agingRate * 2),

            // já é iridium
            _ => 0,
        };
    }

    private static int CalculaAvancoEmDias(int tempoTotal, int qualidade)
    {
        return (int)(tempoTotal * PorcentagemPorQualidade(qualidade));
    }

    private static double PorcentagemPorQualidade(int qualidade)
    {
        return qualidade switch
        {
            0 => 0.25, // normal
            1 => 0.50, // silver
            2 => 0.75, // gold
            _ => 1.00, // iridium
        };
    }
}