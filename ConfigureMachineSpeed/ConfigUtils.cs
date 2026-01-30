namespace StephHoel.ConfigureMachineSpeed;

public class ConfigUtils
{
    public static ModConfig Normalize(ModConfig cfg)
    {
        if (cfg.UpdateInterval == 0)
            cfg.UpdateInterval = 1u;

        cfg.Machines ??= Machines.GetNewMachines();

        foreach (var m in cfg.Machines)
        {
            if (!m.UsePercent && m.Time <= 0)
                m.Time = 10;
        }

        return cfg;
    }
}