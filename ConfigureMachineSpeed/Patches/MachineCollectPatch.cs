using HarmonyLib;
using StardewValley.Extensions;
using StardewValley.Objects;
using Object = StardewValley.Object;

namespace StephHoel.ConfigureMachineSpeed.Patches;

[HarmonyPatch(typeof(Object), nameof(Object.checkForAction))]
public static class MachineCollectPatch
{
    static bool wasReady;
    static Object? previousHeld;

    public static void Prefix(Object __instance)
    {
        if (__instance is Cask cask)
        {
            wasReady = cask.readyForHarvest.Value;
            previousHeld = cask.heldObject.Value;
        }
    }

    public static void Postfix(Object __instance, bool __result)
    {
        if (__instance is Cask cask
            && wasReady
            && !cask.readyForHarvest.Value
            && previousHeld != null
            && cask.heldObject.Value == null)
        {
            cask.ResetMachine();
        }

        if (!IsDropIn(__instance, __result))
            return;

        if (__instance.IsCrystalarium())
            __instance.ConfigureOneMachine(ModEntry.Instance!.Helper.ReadConfig<ModConfig>(), ModEntry.Instance.Monitor);
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