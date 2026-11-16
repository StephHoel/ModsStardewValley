namespace StephHoel.AddMoney;

public static class ConfigUtils
{
    /// <summary>
    /// Keeps configured values intact so the action can reject invalid amounts
    /// with user feedback instead of silently changing them.
    /// </summary>
    public static ModConfig Normalize(ModConfig cfg) => cfg;
}
