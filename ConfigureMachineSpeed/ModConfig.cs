using StardewModdingAPI;
using Utils.Config;
using static StardewValley.Menus.CharacterCustomization;

namespace ConfigureMachineSpeed;

internal class ModConfig
{
    public uint UpdateInterval { get; set; } = 10u;

    public SButton? ReloadConfigKey { get; set; } = SButton.L;

    public MachineConfig[] Machines { get; set; }

    public ModConfig()
    {
        Machines ??= DefaultMachines();
    }

    private static MachineConfig[] DefaultMachines()
    {
        var machinesDefault = Utils.Machines.GetNewMachines();
        return machinesDefault;
    }

}