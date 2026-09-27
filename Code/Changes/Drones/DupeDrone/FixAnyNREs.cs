using HarmonyLib;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Drones.DupeDrone;


[MonoDetourTargets]
internal static class FixAnyNREs
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.EntityStates.CloneDrone.CloneDroneSearch.Search.ILHook(FixSearchNRE);
    }


    private static void FixSearchNRE(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        ILLabel skipIteration = w.DefineLabel();
        int oneBehindLoadColliderLdlocNumber = 0;


        w.MatchRelaxed(
            x => x.MatchBr(out skipIteration),
            x => x.MatchLdloca(out oneBehindLoadColliderLdlocNumber),
            x => x.MatchCallOrCallvirt(out _),
            x => x.MatchStloc(oneBehindLoadColliderLdlocNumber + 1),
            x => x.MatchNop(),
            x => x.MatchLdloc(oneBehindLoadColliderLdlocNumber + 1),
            x => x.MatchCallOrCallvirt<Component>("get_transform") && w.SetCurrentTo(x),
            x => x.MatchCallOrCallvirt<Transform>("get_parent"),
            x => x.MatchCallOrCallvirt<Component>("get_gameObject")
        ).ThrowIfFailure()
        .InsertBeforeCurrent(
            w.CreateDelegateCall((Collider collider) =>
            {
                // SS2 NREs here because some item transforms apparently have (to quote wheatley) "well um...lack of parent(s)"
                return collider == null
                    || collider.transform == null
                    || collider.transform.parent == null;
            }),
            w.Create(OpCodes.Brtrue, skipIteration),
            w.Create(OpCodes.Ldloc, oneBehindLoadColliderLdlocNumber + 1)
        );
    }
}