using HarmonyLib;
using StardewValley.Extensions;
using Object = StardewValley.Object;

namespace StephHoel.ConfigureMachineSpeed.Patches;

[HarmonyPatch(typeof(Object), nameof(Object.checkForAction))]
public static class MachineCollectPatch
{
    public static void Postfix(Object __instance, bool __result)
    {
        if (!IsDropIn(__instance, __result))
            return;

        if (__instance.IsCrystalarium())
            __instance.ConfigureOneMachine(ModEntry.Instance.Helper.ReadConfig<ModConfig>());
    }

    private static bool IsDropIn(Object __instance, bool __result)
    {
        return !__result || __instance.bigCraftable.Value || __instance.heldObject.Value is not null;
    }

    private static bool IsCrystalarium(this Object machine)
    {
        return machine.BaseName.EqualsIgnoreCase("Crystalarium");
    }
}