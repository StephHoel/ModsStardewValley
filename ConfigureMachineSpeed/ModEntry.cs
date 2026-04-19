using HarmonyLib;
using StardewModdingAPI;
using StephHoel.ConfigureMachineSpeed.Config;
using Utils;

namespace StephHoel.ConfigureMachineSpeed;

public class ModEntry : Mod
{
    internal static ModEntry Instance;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        FileUtils.RemoveObsoleteFiles(helper, Monitor);

        Instance = this;

        var harmony = new Harmony(this.ModManifest.UniqueID);
        harmony.PatchAll();

        helper.Events.Display.RenderedActiveMenu += (_, _)
            => GenericModConfigMenu.Register(ModManifest, helper, Monitor);

        helper.Events.Content.LocaleChanged += (_, _)
            => GenericModConfigMenu.Register(ModManifest, helper, Monitor);

        helper.Events.GameLoop.GameLaunched += (_, _) =>
        {
            var config = helper.ReadConfig<ModConfig>();
            config.NormalizeMachineConfig(Monitor);
            helper.WriteConfig(config);
        };

        helper.Events.GameLoop.TimeChanged += (_, _)
            => helper.ReadConfig<ModConfig>().ConfigureAllMachines(Monitor);
    }
}