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
        int inventoryLdlocNumber = 0;
        int itemIndexLdlocNumber = 0;


        w.MatchRelaxed(
            x => x.MatchLdloc(0),
            x => x.MatchCallOrCallvirt<CharacterBody>("get_inventory"),
            x => x.MatchStloc(out inventoryLdlocNumber)
        ).ThrowIfFailure();


        w.MatchRelaxed(
            x => x.MatchLdfld<Inventory>("itemAcquisitionOrder"),
            x => x.MatchLdloc(out _), // currently is 6
            x => x.MatchCallOrCallvirt(out _),
            x => x.MatchStloc(out itemIndexLdlocNumber)
        ).ThrowIfFailure();


        w.MatchRelaxed(
            x => x.MatchLdloc(out _),
            x => x.MatchLdsfld<PickupIndex>("none"),
            x => x.MatchCallOrCallvirt(out _),
            x => x.MatchBrfalse(out skipInForLoopLabel) && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertAfterCurrent(
            w.Create(OpCodes.Ldloc, inventoryLdlocNumber),
            w.Create(OpCodes.Ldloc, itemIndexLdlocNumber),
            w.CreateDelegateCall((Inventory inventory, ItemIndex currentItemIndex) =>
            {
                // zanzan can still do proper trades if you have any of the real version of an item alongside temp ones
                return inventory.GetItemCountTemp(currentItemIndex) > 0 && inventory.GetItemCountPermanent(currentItemIndex) < 1;
            }),
            w.Create(OpCodes.Brtrue, skipInForLoopLabel)
        );
    }
}