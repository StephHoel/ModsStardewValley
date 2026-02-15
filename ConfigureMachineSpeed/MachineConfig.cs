using System.Text.Json.Serialization;

namespace StephHoel.ConfigureMachineSpeed;

public class MachineConfig
{
    public string Id { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    public int Time { get; set; } = 100;

    public bool UsePercent { get; set; } = true;

    // public MachineConfig()
    // {
    // }

    public MachineConfig(string id)
    {
        Id = id;
    }

    public bool IsDefault() => UsePercent && Time == 100;
}