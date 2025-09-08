using StardewModdingAPI;

namespace StephHoel.AddMoney;

public class ModConfig
{
    /// <summary>
    /// Button to Add Money
    /// </summary>
    public SButton ButtonToAddMoney { get; set; } = SButton.G;

    /// <summary>
    /// Gold to Add on Wallet
    /// </summary>
    public int GoldToAdd { get; set; } = 100000;
}