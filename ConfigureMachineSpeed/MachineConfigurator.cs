using StardewValley;
using Utils;
using Object = StardewValley.Object;

namespace StephHoel.ConfigureMachineSpeed;

public class MachineConfigurator
{
    private const string AppliedKey = "StephHoel.ConfigureMachineSpeed.applied";
    private const string OriginalKey = "StephHoel.ConfigureMachineSpeed.original";

    public void ConfigureAllMachines(ModConfig config)
    {
        var cfgById = config.Machines
            .Where(m => !m.IsDefault())
            .Where(m => !string.IsNullOrWhiteSpace(m.Id))
            .GroupBy(m => m.Id, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

        var cfgByLegacyName = config.Machines
            .Where(m => !m.IsDefault() && !string.IsNullOrWhiteSpace(m.Name))
            .GroupBy(m => m.Name!, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

        foreach (GameLocation location in Locations.GetLocations())
        {
            if (location.objects is null)
                continue;

            foreach (var pair in location.objects.Pairs)
            {
                var obj = pair.Value;
                if (obj is null)
                    continue;

                if (TryGetConfig(cfgById, cfgByLegacyName, obj, out var cfg))
                    ConfigureMachine(cfg, obj);
            }
        }
    }

    private bool TryGetConfig(
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

    private void ConfigureMachine(MachineConfig cfg, Object obj)
    {
        if (cfg.UsePercent && cfg.Time == 100)
            return;

        if (obj.MinutesUntilReady <= 0)
        {
            obj.modData.Remove(OriginalKey);
            obj.modData.Remove(AppliedKey);
            return;
        }

        int original = obj.MinutesUntilReady;
        if (obj.modData.TryGetValue(OriginalKey, out string storedOriginal) &&
            int.TryParse(storedOriginal, out int parsedOriginal) &&
            parsedOriginal > 0)
            original = parsedOriginal;

        int target = cfg.UsePercent
            ? Math.Max(1, (int)Math.Ceiling(original * (cfg.Time / 100.0)))
            : Math.Max(1, cfg.Time);

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
        obj.MinutesUntilReady = target;
    }
}