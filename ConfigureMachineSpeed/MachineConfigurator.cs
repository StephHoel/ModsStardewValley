using Netcode;
using StardewValley;
using StardewValley.Objects;
using Utils;
using Object = StardewValley.Object;

namespace StephHoel.ConfigureMachineSpeed;

public class MachineConfigurator
{
    private readonly float EPSILON = 0.01f;

    public void ConfigureAllMachines(ModConfig config)
    {
        foreach (GameLocation item in Locations.GetLocations())
        {
            var enumerator = item.objects.Pairs.GetEnumerator();

            foreach (MachineConfig cfg in config.Machines)
            {
                try
                {
                    while (enumerator.MoveNext())
                    {
                        var currentValue = enumerator.Current.Value;

                        if (currentValue.name == cfg.Name)
                            ConfigureMachine(cfg, currentValue);
                    }
                }
                finally
                {
                    enumerator.Dispose();
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
                val.agingRate.Value = (float)Math.Round(num * 100f / cfg.Time, 2);

                NetFloat agingRate = val.agingRate;

                agingRate.Value += 0.001f;
            }
            else if (!cfg.UsePercent && (int)Math.Round(val.agingRate.Value * 1000f) % 10 != 1)
            {
                val.agingRate.Value = (float)Math.Round(val.daysToMature.Value / num * 1440f / cfg.Time, 2);

                NetFloat agingRate2 = val.agingRate;

                agingRate2.Value += 0.001f;
            }
        }
        else if (obj.MinutesUntilReady % 10 != 8 && obj.MinutesUntilReady > 0)
        {
            if (cfg.UsePercent && Math.Abs(cfg.Time - 100f) > EPSILON)
                obj.MinutesUntilReady = Math.Max((int)(obj.MinutesUntilReady * cfg.Time / 100f / 10f) * 10 - 2, 8);
            else if (!cfg.UsePercent)
                obj.MinutesUntilReady = Math.Max((int)(cfg.Time / 10f) * 10 - 2, 8);
        }
    }
}