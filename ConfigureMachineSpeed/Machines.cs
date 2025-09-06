namespace StephHoel.ConfigureMachineSpeed;

public class Machines
{
    private static readonly Dictionary<string, Func<string>> TranslationMap = new()
    {
        { "Bee House", I18n.BeeHouse },
        { "Bone Mill", I18n.BoneMill },
        { "Cask", I18n.Cask },
        { "Charcoal Kiln", I18n.CharcoalKiln },
        { "Cheese Press", I18n.CheesePress },
        { "Crystalarium", I18n.Crystalarium },
        { "Dehydrator", I18n.Dehydrator },
        { "Deconstructor", I18n.Deconstructor },
        { "Fish Smoker", I18n.FishSmoker },
        { "Furnace", I18n.Furnace },
        { "Geode Crusher", I18n.GeodeCrusher },
        { "Heavy Furnace", I18n.HeavyFurnace },
        { "Heavy Tapper", I18n.HeavyTapper },
        { "Incubator", I18n.Incubator },
        { "Keg", I18n.Keg },
        { "Lightning Rod", I18n.LightningRod },
        { "Loom", I18n.Loom },
        { "Mayonnaise Machine", I18n.MayonnaiseMachine },
        { "Mushroom Log", I18n.MushroomLog },
        { "Oil Maker", I18n.OilMaker },
        { "Ostrich Incubator", I18n.OstrichIncubator },
        { "Preserves Jar", I18n.PreservesJar },
        { "Recycling Machine", I18n.RecyclingMachine },
        { "Seed Maker", I18n.SeedMaker },
        { "Slime Egg-Press", I18n.SlimeEggPress },
        { "Slime Incubator", I18n.SlimeIncubator },
        { "Solar Panel", I18n.SolarPanel },
        { "Tapper", I18n.Tapper },
        { "Wood Chipper", I18n.WoodChipper },
        { "Worm Bin", I18n.WormBin }
    };

    public static string GetTranslation(string machineName)
    {
        if (TranslationMap.TryGetValue(machineName, out var translationFunc))
            return translationFunc();

        return machineName;
    }

    public static List<string> MachineNames
        => [.. TranslationMap.Select(TranslationMap => TranslationMap.Key)];

    public static MachineConfig[] GetNewMachines()
    {
        var source = MachineNames;
        var newMachines = source.Select(x => new MachineConfig(x)).ToArray();
        return newMachines;
    }

    public static MachineConfig[] SetMachines(MachineConfig?[] machines)
    {
        var machinesSet = new HashSet<MachineConfig>(machines.Where(m => m != null).Cast<MachineConfig>(), new MachinesComparer());

        foreach (var machine in GetNewMachines())
            machinesSet.Add(machine);

        return [.. machinesSet];
    }
}

public class MachinesComparer : IEqualityComparer<MachineConfig>
{
    public bool Equals(MachineConfig x, MachineConfig y)
    {
        return x.Name == y.Name;
    }

    public int GetHashCode(MachineConfig obj)
    {
        return obj.Name.GetHashCode();
    }
}