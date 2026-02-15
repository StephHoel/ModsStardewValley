using StardewModdingAPI;
using StardewValley.Extensions;

namespace StephHoel.ConfigureMachineSpeed;

public class ConfigUtils
{
    public static ModConfig Normalize(ModConfig cfg, IMonitor? monitor = null)
    {
        if (cfg.UpdateInterval < 10)
        {
            cfg.UpdateInterval = 10u;
            // monitor?.Log($"[DEV] UpdateInterval adjusted to {cfg.UpdateInterval}.", LogLevel.Debug);
        }

        cfg.Machines ??= Machines.GetNewMachines();

        if (cfg.Machines.All(m => !string.IsNullOrWhiteSpace(m.Id) && m.Id.StartsWithIgnoreCase("(BC)")))
            return cfg;

        var migratedEntries = 0;

        foreach (var m in cfg.Machines)
        {
            // monitor?.Log($"[DEV] Machine name is '{m.Name}' and Machine Id is '{m.Id}'.", LogLevel.Debug);

            if (string.IsNullOrWhiteSpace(m.Id) && !string.IsNullOrWhiteSpace(m.Name))
            {
                if (Machines.TryResolveLegacyNameToId(m.Name, out var machineId))
                {
                    m.Id = machineId;
                    migratedEntries++;
                    // monitor?.Log($"[DEV] Migrated machine config '{m.Name}' to id '{m.Id}'.", LogLevel.Debug);
                }
                else
                {
                    if (m.Name.StartsWithIgnoreCase("(BC)"))
                    {
                        m.Id = m.Name;
                        m.Name = null;
                        migratedEntries++;
                    }
                    // else
                    //     monitor?.Log($"[DEV] Could not resolve legacy machine name '{m.Name}'. Keeping empty id.", LogLevel.Warn);
                }
            }

            if (!m.UsePercent && m.Time <= 10)
            {
                m.Time = 10;
                // monitor?.Log($"[DEV] Fixed machine '{m.Id}' with invalid absolute time. Set to 10.", LogLevel.Debug);
            }
        }


        // monitor?.Log($"[DEV] Migration {(migratedEntries > 0 ? "" : "in")}complete. Updated {migratedEntries} machine entries from Name to Id.", LogLevel.Info);

        return cfg;
    }
}