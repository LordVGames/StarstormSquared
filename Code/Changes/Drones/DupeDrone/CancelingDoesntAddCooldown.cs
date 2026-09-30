using EntityStates;
using EntityStates.CloneDrone;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using RoR2BepInExPack.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Drones.DupeDrone;


[MonoDetourTargets(typeof(CloneDroneCook), GenerateControlFlowVariants = true)]
internal static class CancelingDoesntAddCooldown
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.EntityStates.CloneDrone.CloneDroneCook.FixedUpdate.ILHook(RemoveCooldownIncrease);
    }


    private static void RemoveCooldownIncrease(ILManipulationInfo info)
    {
        ILWeaver w = new(info);


        // going to line:
        // base.skillLocator.primary.rechargeStopwatch *= 0.25f;
        ILWeaverResult result = w.MatchRelaxed(
            x => x.MatchLdarg(0),
            x => x.MatchCallOrCallvirt<EntityState>("get_skillLocator"),
            x => x.MatchLdfld<SkillLocator>("primary"),
            x => x.MatchDup(),
            x => x.MatchCallOrCallvirt<GenericSkill>("get_rechargeStopwatch"),
            x => x.MatchLdcR4(0.25f) && w.SetCurrentTo(x)
        );
        if (!result.IsValid)
        {
            ILHelpers.LogCantHookMessage("Canceling the dupe drone duping doesn't incur the full cooldown", result.FailureMessage);
            return;
        }
        w.ReplaceCurrentOperand(0.01f);
    }
}