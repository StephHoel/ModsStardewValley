﻿namespace ConfigureMachineSpeed;

public class Machines
{
    public static MachineConfig[] GetNewMachines()
    {
        var source = MachineList;
        var newMachines = source.Select((string x) => new MachineConfig(x)).ToArray();
        return newMachines;
    }

    public static MachineConfig[] SetMachines(MachineConfig?[] machines)
    {
        var machinesList = machines.ToList();
        var newMachines = GetNewMachines();

        foreach (var machine in newMachines)
        {
            if (!machinesList.Contains(machine))
                machinesList.Add(machine);
        }

        return machinesList.ToArray()!;
    }

    private static readonly List<string> MachineList = [
        "Bee House",
        "Bone Mill",
        "Cask",
        "Charcoal Kiln",
        "Cheese Press",
        "Crystalarium",
        "Dehydrator",
        "Fish Smoker",
        "Furnace",
        "Geode Crusher",
        "Heavy Furnace",
        "Heavy Tapper",
        "Incubator",
        "Keg",
        "Lightning Rod",
        "Loom",
        "Mayonnaise Machine",
        "Oil Maker",
        "Ostrich Incubator",
        "Preserves Jar",
        "Recycling Machine",
        "Seed Maker",
        "Slime Egg-Press",
        "Slime Incubator",
        "Solar Panel",
        "Tapper",
        "Worm Bin"
    ];
}