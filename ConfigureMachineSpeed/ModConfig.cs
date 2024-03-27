using StardewModdingAPI;

namespace ConfigureMachineSpeed;

internal class ModConfig
{
    public uint UpdateInterval { get; set; } = 10u;

    public SButton? ReloadConfigKey { get; set; } = SButton.L;

}