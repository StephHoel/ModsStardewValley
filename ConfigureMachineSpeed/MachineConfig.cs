namespace ConfigureMachineSpeed;

internal class MachineConfig
{
    public string Name { get; set; }

    public float Time { get; set; } = 100f;

    public bool UsePercent { get; set; } = true;

    public MachineConfig(string Name)
    {
        this.Name = Name;
    }
}