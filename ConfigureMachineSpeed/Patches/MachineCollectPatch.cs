using HarmonyLib;
using StardewValley.Extensions;
using StardewValley.Objects;
using Object = StardewValley.Object;

namespace StephHoel.ConfigureMachineSpeed.Patches;

public readonly record struct CaskActionState(bool WasReady, Object? PreviousHeld);

[HarmonyPatch(typeof(Object), nameof(Object.checkForAction))]
public static class MachineCollectPatch
{
    public static void Prefix(Object __instance, out CaskActionState __state)
    {
        __state = __instance is Cask cask
            ? new CaskActionState(cask.readyForHarvest.Value, cask.heldObject.Value)
            : default;
    }

    public static void Postfix(Object __instance, bool __result, CaskActionState __state)
    {
        if (__instance is Cask cask
            && __state.WasReady
            && !cask.readyForHarvest.Value
            && __state.PreviousHeld is not null
            && cask.heldObject.Value is null)
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
