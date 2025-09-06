using Microsoft.Xna.Framework;
using Netcode;
using StardewValley;
using StardewValley.Objects;
using Object = StardewValley.Object;

namespace StephHoel.ConfigureMachineSpeed;

public class MachineConfigurator
{
    private readonly float EPSILON = 0.01f;

    public void ConfigureAllMachines(ModConfig config)
    {
        var locations = Game1.locations.Concat(
            from location in Game1.locations
            from building in location.buildings
            where building.indoors.Value != null
            select building.indoors.Value
        );

        foreach (GameLocation item in locations)
        {
            foreach (MachineConfig cfg in config.Machines)
            {
                bool func(KeyValuePair<Vector2, Object> p) => p.Value.name == cfg.Name;

                var pairs = item.objects.Pairs;

                var enumerator2 = pairs.GetEnumerator();

                try
                {
                    while (enumerator2.MoveNext())
                    {
                        var current2 = enumerator2.Current;

                        if (func(current2))
                            ConfigureMachine(cfg, current2.Value);
                    }
                }
                finally
                {
                    enumerator2.Dispose();
                }
            }
        }
    }

    private void ConfigureMachine(MachineConfig cfg, Object obj)
    {
        if (obj is Cask val && obj.heldObject.Value != null)
        {
            float num = val.heldObject.Value.ParentSheetIndex switch
            {
                426 => 4f,
                424 => 4f,
                459 => 2f,
                303 => 1.66f,
                346 => 2f,
                _ => 1f
            };

            if (cfg.UsePercent && Math.Abs(cfg.Time - 100f) > EPSILON && (int)Math.Round(val.agingRate.Value * 1000f) % 10 != 1)
            {
                val.agingRate.Value = num * 100f / cfg.Time;
                val.agingRate.Value = (float)Math.Round(val.agingRate.Value, 2);
                NetFloat agingRate = val.agingRate;
                agingRate.Value += 0.001f;
            }
            else if (!cfg.UsePercent && (int)Math.Round(val.agingRate.Value * 1000f) % 10 != 1)
            {
                val.agingRate.Value = val.daysToMature.Value / num * 1440f / cfg.Time;
                val.agingRate.Value = (float)Math.Round(val.agingRate.Value, 2);
                NetFloat agingRate2 = val.agingRate;
                agingRate2.Value += 0.001f;
            }
        }
        else if (obj.MinutesUntilReady % 10 != 8 && obj.MinutesUntilReady > 0)
        {
            if (cfg.UsePercent && Math.Abs(cfg.Time - 100f) > EPSILON)
            {
                obj.MinutesUntilReady = Math.Max((int)(obj.MinutesUntilReady * cfg.Time / 100f / 10f) * 10 - 2, 8);
            }
            else if (!cfg.UsePercent)
            {
                obj.MinutesUntilReady = Math.Max((int)(cfg.Time / 10f) * 10 - 2, 8);
            }
        }
    }
}
