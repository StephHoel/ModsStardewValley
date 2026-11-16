namespace StephHoel.ConfigureMachineSpeed;

/// <summary>Resolves machine settings using stable qualified IDs before legacy names.</summary>
public static class MachineConfigResolver
{
    public static bool TryResolve(
        IDictionary<string, MachineConfig> configsByQualifiedId,
        IDictionary<string, MachineConfig> configsByLegacyName,
        string? qualifiedItemId,
        string? legacyName,
        out MachineConfig config)
    {
        if (!string.IsNullOrWhiteSpace(qualifiedItemId)
            && configsByQualifiedId.TryGetValue(qualifiedItemId, out config!))
            return true;

        if (!string.IsNullOrWhiteSpace(legacyName)
            && configsByLegacyName.TryGetValue(legacyName, out config!))
            return true;

        config = null!;
        return false;
    }
}
