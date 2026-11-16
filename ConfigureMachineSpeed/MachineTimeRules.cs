namespace StephHoel.ConfigureMachineSpeed;

/// <summary>Pure rules for normalizing and applying configured machine times.</summary>
public static class MachineTimeRules
{
    public static int NormalizeConfiguredTime(int value)
    {
        // Use Int64 to safely handle int.MinValue from a manually edited config.
        var longValue = (long)value;
        var remainder = (int)(Math.Abs(longValue) % 10);
        var rounded = longValue - (longValue >= 0 ? remainder : -remainder);

        if (remainder >= 5)
            rounded += longValue >= 0 ? 10 : -10;

        return (int)Math.Clamp(rounded, 10L, int.MaxValue);
    }

    public static int CalculateTarget(int configuredTime, int originalTime)
    {
        // A configured value is an upper bound: never slow a machine below its
        // original processing speed.
        return Math.Min(originalTime, NormalizeConfiguredTime(configuredTime));
    }
}
