using HarmonyLib;
using StardewModdingAPI;
using StephHoel.ConfigureMachineSpeed.Config;
using Utils;

namespace StephHoel.ConfigureMachineSpeed;

public class ModEntry : Mod
{
    internal static ModEntry? Instance;
    private static bool patchesApplied;
    private bool configMenuRegistered;

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

        helper.Events.GameLoop.GameLaunched += (_, _) =>
        {
            var config = helper.ReadConfig<ModConfig>();
            config.NormalizeMachineConfig(Monitor);
            helper.WriteConfig(config);

            // GMCM is optional. Register once at the lifecycle event intended
            // for API integration, rather than on every rendered menu frame.
            if (configMenuRegistered)
                return;

            GenericModConfigMenu.Register(ModManifest, helper, Monitor);
            configMenuRegistered = true;
        };

        helper.Events.GameLoop.DayStarted += (_, _) =>
        {
            helper.ReadConfig<ModConfig>().ConfigureAllMachines(Monitor);
        };

        helper.Events.GameLoop.TimeChanged += (_, _)
            => helper.ReadConfig<ModConfig>().ConfigureAllMachines(Monitor);
    }
}
