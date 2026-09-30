using System;
using UnityEngine;
using RoR2;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2.Orbs;
using System.Runtime.CompilerServices;
using R2API;
using SS2;
namespace StarstormSquared.Changes.Items;


[MonoDetourTargets(typeof(SS2.Items.PortableReactor))]
internal static class PortableReactor
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        if (!ConfigOptions.ItemChanges.PortableReactor.PortableReactorChange.Value)
        {
            return;
        }

        Mdh.SS2.Items.PortableReactor.GetStatCoefficients.Postfix(AlsoGiveArmorWhenActive);
        Mdh.SS2.Items.PortableReactor.Behavior.OnIncomingDamageServer.ILHook(RemoveInvulnerability);
    }


    private static void AlsoGiveArmorWhenActive(SS2.Items.PortableReactor self, ref CharacterBody sender, ref RecalculateStatsAPI.StatHookEventArgs args)
    {
        if (sender.HasBuff(SS2Content.Buffs.BuffReactor))
        {
            args.armorAdd += 100;
        }
    }


    private static void RemoveInvulnerability(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        Instruction startOfBadLine = null!;
        Instruction endOfBadLine = null!;


        ILWeaverResult result = w.MatchRelaxed(
            x => x.MatchLdarg(1) && w.SetInstructionTo(ref startOfBadLine, x),
            x => x.MatchLdcI4(1),
            x => x.MatchStfld<DamageInfo>("rejected") && w.SetInstructionTo(ref endOfBadLine, x)
        );
        if (!result.IsValid)
        {
            ILHelpers.LogCantHookMessage("Removing invincibility from portable reactor", result.FailureMessage);
            return;
        }
        w.InsertBranchOver(startOfBadLine, endOfBadLine);
    }


    [SystemInitializer(dependencies: typeof(ItemCatalog))]
    private static void ChangeTokens()
    {
        if (!ConfigOptions.ItemChanges.PortableReactor.PortableReactorChange.Value || SS2Content.Items.PortableReactor == null)
        {
            return;
        }
        
        SS2Content.Items.PortableReactor.pickupToken = "SS22_ITEM_PORTABLEREACTOR_EDIT_PICKUP";
        SS2Content.Items.PortableReactor.descriptionToken = "SS22_ITEM_PORTABLEREACTOR_EDIT_DESC";
    }
}