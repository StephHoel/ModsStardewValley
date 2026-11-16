using HarmonyLib;
using StardewModdingAPI;
using StephHoel.ConfigureMachineSpeed.Config;
using Utils;

namespace StephHoel.ConfigureMachineSpeed;

public class ModEntry : Mod
{
    internal static ModEntry? Instance;
    private static bool patchesApplied;

    public override void Entry(IModHelper helper)
    {
        I18n.Init(helper.Translation);
        FileUtils.RemoveObsoleteFiles(helper, Monitor);

        Instance = this;

        if (!patchesApplied)
        {
            try
            {
                var harmony = new Harmony(ModManifest.UniqueID);
                harmony.PatchAll();
                patchesApplied = true;
                Monitor.Log("Harmony patches applied successfully.", LogLevel.Trace);
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed to apply Harmony patches for {ModManifest.UniqueID}: {ex}", LogLevel.Error);
            }
        }

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

        helper.Events.GameLoop.DayStarted += (_, _) =>
        {
            helper.ReadConfig<ModConfig>().ConfigureAllMachines(Monitor);
        };

        helper.Events.GameLoop.TimeChanged += (_, _)
            => helper.ReadConfig<ModConfig>().ConfigureAllMachines(Monitor);
    }
}
