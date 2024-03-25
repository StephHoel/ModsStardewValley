using StardewModdingAPI;

namespace ConfigureMachineSpeed;

internal class ModConfig
{
    public uint UpdateInterval { get; set; } = 10u;

    public MachineConfig[] Machines { get; set; }

    public SButton? ReloadConfigKey { get; set; } = SButton.L;

    public ModConfig()
    {
        this.Machines = this.DefaultMachines();
    }

    private MachineConfig[] DefaultMachines()
    {
        List<string> source = [
            "Bee House", 
            "Cask", 
            "Charcoal Kiln", 
            "Cheese Press", 
            "Crystalarium", 
            "Furnace", 
            "Incubator", 
            "Keg", 
            "Lightning Rod", 
            "Loom",
            "Mayonnaise Machine", 
            "Oil Maker", 
            "Preserves Jar", 
            "Recycling Machine", 
            "Seed Maker", 
            "Slime Egg-Press", 
            "Slime Incubator", 
            "Tapper", 
            "Worm Bin"
            ];
        return source.Select((string x) => new MachineConfig(x)).ToArray();
    }
}