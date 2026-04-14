namespace StephHoel.ConfigureMachineSpeed.Config;

public static class MachineList
{
    private static readonly Dictionary<string, string> ExcludedMachines = new()
    {
        { "(BC)BaitMaker", "Bait Maker"},
        { "(BC)264", "Heavy Tapper"},
        { "(BC)9", "Lightning Rod"},
        { "(BC)231", "Solar Panel"}, // TODO reavaliar
        { "(BC)105", "Tapper"},
        { "(BC)Anvil", "Anvil"},
    };

    private static readonly Dictionary<string, string> DayUpdateMachines = new()
    {
        {"(BC)10","Bee House"},
        {"(BC)163","Cask"},
        {"(BC)246","Coffee Maker"},
        {"710", "Crab Pot"},
        {"(BC)Dehydrator","Dehydrator"},
        {"(BC)DeluxeWormBin","Deluxe Worm Bin"},
        {"(BC)MushroomLog","Mushroom Log"},
        {"(BC)154","Worm Bin"},
    };

    private static readonly Dictionary<string, string> IncubatorMachine = new()
    {
        { "(BC)101", "Incubator" },
        { "(BC)254", "Ostrich Incubator" },
        { "(BC)156", "Slime Incubator" },
    };

    public static bool IsMachineExcluded(this string? machineId)
    {
        var isExcluded = ExcludedMachines.Any(m => m.Key.Equals(machineId, StringComparison.InvariantCultureIgnoreCase));

        return isExcluded || IsDayUpdateMachine(machineId) || IsIncubatorMachine(machineId);
    }

    public static bool IsDayUpdateMachine(this string? machineId)
        => DayUpdateMachines.Any(m => m.Key.Equals(machineId, StringComparison.InvariantCultureIgnoreCase));

    public static bool IsIncubatorMachine(this string? machineId)
        => IncubatorMachine.Any(m => m.Key.Equals(machineId, StringComparison.InvariantCultureIgnoreCase));
}