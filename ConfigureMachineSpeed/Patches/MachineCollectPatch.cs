using HarmonyLib;

namespace StephHoel.ConfigureMachineSpeed.Patches;

[HarmonyPatch(typeof(StardewValley.Object), nameof(StardewValley.Object.checkForAction))]
public static class MachineCollectPatch
{
    public static void Postfix(StardewValley.Object __instance, bool __result)
    {
        if (__result
            || !__instance.bigCraftable.Value
            || __instance.heldObject.Value == null)
            return;

        __instance.ConfigureOneMachine(ModEntry.Instance.Helper.ReadConfig<ModConfig>());
    }
}