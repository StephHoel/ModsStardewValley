using StardewModdingAPI;
using StardewValley.Extensions;

namespace StephHoel.ConfigureMachineSpeed;

public static class ConfigUtils
{
    public static ModConfig NormalizeMachineConfig(this ModConfig cfg, IMonitor? monitor = null)
    {

        monitor?.Log($"[ConfigUtils] Starting normalize config", LogLevel.Trace);

        cfg.Machines = Machines.GetMachines(cfg.Machines);

        var machinesList = new List<MachineConfig>();

        foreach (var m in cfg.Machines)
        {
            var machine = TryResolveId(m, monitor) ?? m;

            machine = NormalizeTime(machine);

            if (machinesList.Any(m => m.Id == machine.Id))
                continue;

            machinesList.Add(machine);
        }

        cfg.Machines = [.. machinesList];

        return cfg;
    }

    private static MachineConfig NormalizeTime(MachineConfig machine)
    {
        if (machine.Time < 10)
            machine.Time = 10;

        machine.Time = RoundedTime(machine.Time);

        return machine;
    }

    public static int RoundedTime(int value)
    {
        int remainder = Math.Abs(value) % 10;
        int rounded = value - (value >= 0 ? remainder : -remainder);

        if (remainder >= 5)
            rounded += (value >= 0) ? 10 : -10;

        if (rounded < 10)
            rounded = 10;

        return rounded;
    }

    private static MachineConfig TryResolveId(MachineConfig m, IMonitor? monitor = null)
    {
        if (string.IsNullOrWhiteSpace(m.Id) && !string.IsNullOrWhiteSpace(m.Name))
        {
            if (Machines.TryResolveLegacyNameToId(m.Name, out var machineId))
            {
                m.Id = machineId ?? string.Empty;
                m.Name = null;

                monitor?.Log($"[ConfigUtils.Normalize] Migrated machine config '{m.Name}' to id '{m.Id}'.", LogLevel.Trace);
            }
            else
            {
                if (m.Name.StartsWithIgnoreCase("(BC)"))
                {
                    m.Id = m.Name;
                    m.Name = null;
                }
                else
                    monitor?.Log($"[ConfigUtils] Could not resolve legacy machine name '{m.Name}'. Keeping empty id.", LogLevel.Trace);

                if (m.Id.StartsWithIgnoreCase("(BC)"))
                    m.Name = null;
            }
        }

        return m;
    }
}