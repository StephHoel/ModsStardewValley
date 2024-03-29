namespace Utils.Config;

public class MachineConfig
{
    public string Name { get; set; }

    public int Time { get; set; } = 100;

    public bool UsePercent { get; set; } = true;

    public MachineConfig(string Name)
    {
        this.Name = Name;
    }
}