using HarmonyLib;
using StardewValley;
using StardewValley.Objects;
using StephHoel.ConfigureMachineSpeed.Config;

namespace StephHoel.ConfigureMachineSpeed.Patches;

[HarmonyPatch(typeof(StardewValley.Object), nameof(StardewValley.Object.performObjectDropInAction))]
public static class ObjectDropInPatch
{
    public static void Postfix(StardewValley.Object __instance, Item dropInItem, bool probe, Farmer who)
    {
        if (__instance is Cask cask && !probe && cask.heldObject.Value is null)
        {
            cask.ResetMachine();
            return;
        }

        if (probe
            || !__instance.bigCraftable.Value
            || dropInItem == null
            || __instance.heldObject.Value == null
            || __instance.Location == null
            || __instance.Name.IsMachineExcluded())
            return;

        // if (__instance.Location == Game1.player.currentLocation)
        // {
        //     ModEntry.Instance.Monitor.Log($"[TESTE] Item <{dropInItem.DisplayName}> inserido em <{__instance.DisplayName}>", LogLevel.Debug);
        // }

        __instance.ConfigureOneMachine(ModEntry.Instance!.Helper.ReadConfig<ModConfig>(), ModEntry.Instance.Monitor);

        // ModEntry.Instance.Monitor.Log($"[TESTE] Maquina <{__instance.DisplayName}> configurada", LogLevel.Debug);
    }
}