using HG;
using MiscFixes.Modules;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using RoR2.UI;
using SS2.Components;
using SS2.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
namespace StarstormSquared.Changes.Interactables.Zanzan;


[MonoDetourTargets(typeof(TraderController))]
internal static class IgnoreTempItems
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.Components.TraderController.AssignPotentialInteractor.ILHook(CheckForTemp);
    }


    private static void CheckForTemp(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        ILLabel skipInForLoopLabel = w.DefineLabel();
        w.DeclareVariable(typeof(ItemIndex), out var itemIndexVariable);
        int inventoryLdlocNumber = 0;
        int iterationLdlocNumber = 0;


        w.MatchRelaxed(
            x => x.MatchLdloc(out inventoryLdlocNumber) && w.SetCurrentTo(x),
            x => x.MatchLdfld<Inventory>("itemAcquisitionOrder"),
            x => x.MatchLdloc(out iterationLdlocNumber),
            x => x.MatchCallOrCallvirt(out _),
            x => x.MatchCallOrCallvirt(out _)
        ).ThrowIfFailure();


        w.MatchRelaxed(
            x => x.MatchLdloc(out _),
            x => x.MatchLdsfld<PickupIndex>("none"),
            x => x.MatchCallOrCallvirt(out _),
            x => x.MatchBrfalse(out skipInForLoopLabel) && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertAfterCurrent(
            w.Create(OpCodes.Ldloc, inventoryLdlocNumber),
            w.Create(OpCodes.Ldloc, iterationLdlocNumber),
            w.CreateDelegateCall((Inventory inventory, int iteration) =>
            {
                // zanzan can still do proper trades if you have any of the real version of an item alongside temp ones
                return inventory.GetItemCountTemp(inventory.itemAcquisitionOrder[iteration]) > 0 && inventory.GetItemCountPermanent(inventory.itemAcquisitionOrder[iteration]) < 1;
            }),
            w.Create(OpCodes.Brtrue, skipInForLoopLabel)
        );
    }
}