namespace StephHoel.ConfigureMachineSpeed;

public static class MachineTimeRules
{
    public static int NormalizeConfiguredTime(int value)
    {
        var longValue = (long)value;
        var remainder = (int)(Math.Abs(longValue) % 10);
        var rounded = longValue - (longValue >= 0 ? remainder : -remainder);
        if (remainder >= 5) rounded += longValue >= 0 ? 10 : -10;
        return (int)Math.Clamp(rounded, 10L, int.MaxValue);
    }

    public static int CalculateTarget(int configuredTime, int originalTime)
        => Math.Min(originalTime, NormalizeConfiguredTime(configuredTime));
}
