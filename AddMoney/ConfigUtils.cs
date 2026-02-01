namespace StephHoel.AddMoney;

public static class ConfigUtils
{
    public static ModConfig Normalize(ModConfig cfg)
    {
        if (cfg.GoldToAdd <= 0)
            cfg.GoldToAdd = 1;

        return cfg;
    }
}
