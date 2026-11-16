using StardewModdingAPI;
using StardewValley.Extensions;

namespace StephHoel.ConfigureMachineSpeed;

public static class ConfigUtils
{
    public static ModConfig NormalizeMachineConfig(this ModConfig cfg, IMonitor? monitor = null)
    {
        monitor?.Log("[ConfigUtils] Starting config normalization.", LogLevel.Trace);

        cfg.Machines ??= [];
        cfg.Machines = Machines.GetMachines(cfg.Machines);

        var machinesList = new List<MachineConfig>();
        foreach (var machineConfig in cfg.Machines)
        {
            var machine = TryResolveId(machineConfig, monitor);
            machine = NormalizeTime(machine);

            if (string.IsNullOrWhiteSpace(machine.Id))
            {
                monitor?.Log($"Ignoring machine configuration with unresolved legacy name '{machine.Name}'.", LogLevel.Warn);
                continue;
            }

            if (machinesList.Any(existing => string.Equals(existing.Id, machine.Id, StringComparison.Ordinal)))
                continue;

            machinesList.Add(machine);
        }

        cfg.Machines = [.. machinesList];
        return cfg;
    }

    private static MachineConfig NormalizeTime(MachineConfig machine)
    {
        machine.Time = MachineTimeRules.NormalizeConfiguredTime(machine.Time);
        return machine;
    }

    public static int RoundedTime(int value) => MachineTimeRules.NormalizeConfiguredTime(value);

    public static int CalculateTarget(this MachineConfig cfg, int original)
    {
        if (cfg is null)
            return original;

        return MachineTimeRules.CalculateTarget(cfg.Time, original);
    }

    private static MachineConfig TryResolveId(MachineConfig machine, IMonitor? monitor = null)
    {
        if (!string.IsNullOrWhiteSpace(machine.Id))
            return machine;

        var legacyName = machine.Name;
        if (string.IsNullOrWhiteSpace(legacyName))
            return machine;

        if (Machines.TryResolveLegacyNameToId(legacyName, out var machineId)
            && !string.IsNullOrWhiteSpace(machineId))
        {
            machine.Id = machineId;
            machine.Name = null;
            monitor?.Log($"Migrated legacy machine name '{legacyName}' to qualified ID '{machine.Id}'.", LogLevel.Trace);
            return machine;
        }

        if (legacyName.StartsWithIgnoreCase("(BC)"))
        {
            machine.Id = legacyName;
            machine.Name = null;
            monitor?.Log($"Kept qualified legacy machine ID '{machine.Id}'.", LogLevel.Trace);
            return machine;
        }

        monitor?.Log($"Could not resolve legacy machine name '{legacyName}'. The entry will be skipped safely.", LogLevel.Warn);
        return machine;
    }

    public static void ResetMachine(this StardewValley.Object machine)
    {
        machine.modData.Remove(Constants.StopAgingKey);
        machine.modData.Remove(Constants.AppliedKey);
    }
}
