using Utils.Config;
using Utils.Lists;

namespace Utils;

public class Machines
{
    public static MachineConfig[] GetMachines()
    {
        var source = ListMachines.List;
        return source.Select((string x) => new MachineConfig(x)).ToArray();
    }
}