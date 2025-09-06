namespace StephHoel.ConfigureMachineSpeed;

public class ConfigUtils
{
    public static ModConfig ProcessConfig(ModConfig cfg, ModConfig currentConfig)
    {
        if (cfg.UpdateInterval == 0)
            cfg.UpdateInterval = 1u;

        MachineConfig[] machines = currentConfig.Machines;

        foreach (MachineConfig machineConfig in machines)
        {
            if (!machineConfig.UsePercent && machineConfig.Time <= 0)
                machineConfig.Time = 10;
        }

        return cfg;
    }
}