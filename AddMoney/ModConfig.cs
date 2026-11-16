using StardewModdingAPI;

namespace StephHoel.AddMoney;

public class ModConfig
{
    /// <summary>
    /// Button to add money. Defaults to G.
    /// </summary>
    public SButton ButtonToAddMoney { get; set; } = SButton.G;

    /// <summary>
    /// Amount of gold to add per key press. The default is 100,000 gold.
    /// Existing player values are preserved.
    /// </summary>
    public int GoldToAdd { get; set; } = 100_000;
}
