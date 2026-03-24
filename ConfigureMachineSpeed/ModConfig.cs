namespace StephHoel.ConfigureMachineSpeed;

public class ModConfig
{
    public MachineConfig[] Machines { get; set; }

    public ModConfig()
    {
        Machines ??= DefaultMachines();
    }

    private static MachineConfig[] DefaultMachines()
    {
        var machinesDefault = ConfigureMachineSpeed.Machines.GetNewMachines();
        return machinesDefault;
    }
}