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
        var cfgByName = config.Machines
            .Where(m => !m.IsDefault())
            .GroupBy(m => m.Name, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

        foreach (GameLocation location in Locations.GetLocations())
        {
            foreach (var pair in location.objects.Pairs)
            {
                var obj = pair.Value;
                if (obj is null)
                    continue;

                if (TryGetConfig(cfgByName, obj, out var cfg))
                    ConfigureMachine(cfg, obj);
            }
        }
    }

    private bool TryGetConfig(Dictionary<string, MachineConfig> cfgByName, Object obj, out MachineConfig cfg)
    {
        if (!string.IsNullOrWhiteSpace(obj.QualifiedItemId) && cfgByName.TryGetValue(obj.QualifiedItemId, out cfg!))
            return true;

        if (cfgByName.TryGetValue(obj.name, out cfg!))
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
