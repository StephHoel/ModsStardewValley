namespace StephHoel.ConfigureMachineSpeed;

public class MachineConfig(string Name)
{
    public string Name { get; set; } = Name;

    public int Time { get; set; } = 100;

    public bool UsePercent { get; set; } = true;
}