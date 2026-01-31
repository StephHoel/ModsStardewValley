using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Machines;

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
        { "Worm Bin", I18n.WormBin },
    };
    private static List<string>? CachedMachineNames;
    private static bool CacheIncludesGameData;

    public static string GetTranslation(string machineName)
    {
        if (TranslationMap.TryGetValue(machineName, out var translationFunc))
            return translationFunc();

        if (Context.IsGameLaunched)
        {
            var itemData = ItemRegistry.GetData(machineName);
            if (!string.IsNullOrWhiteSpace(itemData?.DisplayName))
                return itemData.DisplayName;
        }

        return machineName;
    }

    public static List<string> MachineNames
        => GetMachineNames();

    public static MachineConfig[] GetNewMachines(IEnumerable<string>? machineNames = null)
    {
        var source = machineNames?.Distinct(StringComparer.Ordinal).ToList() ?? MachineNames;
        var newMachines = source.Select(x => new MachineConfig(x)).ToArray();
        return newMachines;
    }

    public static MachineConfig[] SetMachines(IEnumerable<MachineConfig?> machines, IEnumerable<string>? machineNames = null)
    {
        var machinesSet = new HashSet<MachineConfig>(machines.Where(m => m != null).Cast<MachineConfig>(), new MachinesComparer());

        foreach (var machine in GetNewMachines(machineNames))
            machinesSet.Add(machine);

        return [.. machinesSet];
    }

    private static List<string> GetMachineNames()
    {
        if (CachedMachineNames is not null && (!Context.IsGameLaunched || CacheIncludesGameData))
            return CachedMachineNames;

        var machineNames = new HashSet<string>(TranslationMap.Select(translationMap => translationMap.Key), StringComparer.Ordinal);

        if (Context.IsGameLaunched)
        {
            var machineData = Game1.content.Load<Dictionary<string, MachineData>>("Data/Machines");

            foreach (var machineId in machineData.Keys)
            {
                var itemData = ItemRegistry.GetData(machineId);
                var name = itemData?.InternalName;

                if (string.IsNullOrWhiteSpace(name))
                    name = itemData?.DisplayName;

                if (string.IsNullOrWhiteSpace(name))
                    name = machineId;

                machineNames.Add(name);
            }
        }

        CachedMachineNames = [.. machineNames.OrderBy(name => name, StringComparer.OrdinalIgnoreCase)];
        CacheIncludesGameData = Context.IsGameLaunched;

        return CachedMachineNames;
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