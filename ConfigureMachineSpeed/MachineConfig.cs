using System.Text.Json.Serialization;

namespace StephHoel.ConfigureMachineSpeed;

public class MachineConfig(string id)
{
    public string Id { get; set; } = id;

    [JsonIgnore]
    public string? Name { get; set; }

    public int Time { get; set; } = 100;
}