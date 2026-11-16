using System.Text.Json.Serialization;

namespace StephHoel.ConfigureMachineSpeed;

public class MachineConfig(string id)
{
    public string Id { get; set; } = id;

    // Read legacy names from existing config files so they can be migrated to IDs.
    // Once migrated, null names are omitted from the serialized configuration.
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }

    public int Time { get; set; } = 100;
}
