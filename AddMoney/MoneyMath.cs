namespace StephHoel.AddMoney;

/// <summary>Calculates a safe wallet increase without overflowing the game's Int32 balance.</summary>
public static class MoneyMath
{
    public static bool TryCalculateAddition(int currentBalance, int requestedAmount, out int amountToAdd)
    {
        amountToAdd = 0;

        if (requestedAmount <= 0)
            return false;

        var remainingCapacity = (long)int.MaxValue - currentBalance;
        if (remainingCapacity <= 0)
            return false;

        amountToAdd = (int)Math.Min(requestedAmount, remainingCapacity);
        return amountToAdd > 0;
    }
}
