using StardewModdingAPI;

namespace StephHoel.AutoWater;

/// <summary>
/// Configuration settings for the AutoWater mod
/// </summary>
public class ModConfig
{
    /// <summary>
    /// Button to toggle the mod active or inactive
    /// </summary>
    public SButton Toggle { get; set; } = SButton.O;

    /// <summary>
    /// Whether the mod is active on game start
    /// </summary>
    public bool IsActive { get; set; } = true;
}