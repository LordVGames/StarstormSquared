using System;
using System.Collections.Generic;
using System.Text;
using MiscFixes.Modules;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
namespace StarstormSquared.Changes.Survivors.Chirr;


[MonoDetourTargets(typeof(SS2.Components.ChirrFriendController))]
internal static class ChirrMinionNoHealingItems
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        if (!ConfigOptions.Chirr.ChirrMinionsNoHealingItems.Value)
        {
            return;
        }

        Mdh.SS2.Components.ChirrFriendController.ItemFilter.ILHook(FilterHealingItems);
    }

    private static void FilterHealingItems(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        ILLabel endIfFalse = w.DefineLabel();
        Instruction loadItemDefInstruction = null!;


        w.MatchRelaxed(
            x => x.MatchAny(out loadItemDefInstruction) && w.SetCurrentTo(x),
            x => x.MatchLdcI4(20),
            x => x.MatchCallvirt<ItemDef>("DoesNotContainTag"),
            x => x.MatchBrfalse(out endIfFalse)
        ).ThrowIfFailure()
        .InsertBeforeCurrent(
            loadItemDefInstruction,
            w.Create(OpCodes.Ldc_I4_2), // itemtag 2 is healing
            w.Create(OpCodes.Callvirt, typeof(ItemDef).GetMethod("DoesNotContainTag")),
            w.Create(OpCodes.Brfalse, endIfFalse)
        );
    }
}