using StardewValley;
using Utils;
using Object = StardewValley.Object;

namespace StephHoel.ConfigureMachineSpeed;

public class MachineConfigurator
{
    // private readonly float EPSILON = 0.01f;
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

                if (cfgByName.TryGetValue(obj.name, out var cfg))
                    ConfigureMachine(cfg, obj);
            }
        }
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

        // if (obj is Cask val && obj.heldObject.Value != null)
        // {
        //     float num = val.heldObject.Value.ParentSheetIndex switch
        //     {
        //         426 => 4f,
        //         424 => 4f,
        //         459 => 2f,
        //         303 => 1.66f,
        //         346 => 2f,
        //         _ => 1f
        //     };

        //     if (cfg.UsePercent && Math.Abs(cfg.Time - 100f) > EPSILON && (int)Math.Round(val.agingRate.Value * 1000f) % 10 != 1)
        //     {
        //         val.agingRate.Value = (float)Math.Round(num * 100f / cfg.Time, 2);

        //         NetFloat agingRate = val.agingRate;

        //         agingRate.Value += 0.001f;
        //     }
        //     else if (!cfg.UsePercent && (int)Math.Round(val.agingRate.Value * 1000f) % 10 != 1)
        //     {
        //         val.agingRate.Value = (float)Math.Round(val.daysToMature.Value / num * 1440f / cfg.Time, 2);

        //         NetFloat agingRate2 = val.agingRate;

        //         agingRate2.Value += 0.001f;
        //     }
        // }
        // else if (obj.MinutesUntilReady % 10 != 8 && obj.MinutesUntilReady > 0)
        // {
        //     if (cfg.UsePercent && Math.Abs(cfg.Time - 100f) > EPSILON)
        //         obj.MinutesUntilReady = Math.Max((int)(obj.MinutesUntilReady * cfg.Time / 100f / 10f) * 10 - 2, 8);
        //     else if (!cfg.UsePercent)
        //         obj.MinutesUntilReady = Math.Max((int)(cfg.Time / 10f) * 10 - 2, 8);
        // }
    }
}
