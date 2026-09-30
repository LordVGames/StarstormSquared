using MiscFixes.Modules;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.Survivors.DUT;


[MonoDetourTargets(typeof(EntityStates.DUT.ChargeDamage))]
internal static class GiveAttacksDamageSources
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }


        Mdh.EntityStates.DUT.ChargeDamage.SiphonEnemies.ILHook(AddSiphonEnemiesDamageSource);
        Mdh.EntityStates.DUT.ChargeDamage.SiphonSelf.ILHook(AddSiphonSelfDamageSource);
        Mdh.EntityStates.DUT.ChargeDamage.Discharge.ILHook(AddShootDamageSource);
    }





    private static void AddSiphonEnemiesDamageSource(ILManipulationInfo info)
    {
        ILWeaver w = new(info);


        ILWeaverResult result = w.MatchRelaxed(
            x => x.MatchCallOrCallvirt(out _),
            x => x.MatchStfld<BlastAttack>("damageType") && w.SetCurrentTo(x)
        );
        if (!result.IsValid)
        {
            ILHelpers.LogCantHookMessage("Adding a DamageSource to DU-T siphon enemies", result.FailureMessage);
            return;
        }
        w.InsertAfterCurrent(
            w.Create(OpCodes.Dup),
            w.CreateDelegateCall((BlastAttack blastAttack) =>
            {
                blastAttack.damageType.damageSource = DamageSource.Primary;
            })
        );
    }


    private static void AddSiphonSelfDamageSource(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        Instruction loadDamageInfoInstruction = null!;


        ILWeaverResult result = w.MatchRelaxed(
            x => x.MatchStfld<DamageInfo>("damageType") && w.SetCurrentTo(x),
            x => x.MatchAny(out loadDamageInfoInstruction)
        );
        if (!result.IsValid)
        {
            ILHelpers.LogCantHookMessage("Adding a DamageSource to DU-T siphon self", result.FailureMessage);
            return;
        }
        w.InsertAfterCurrent(
            loadDamageInfoInstruction,
            w.CreateDelegateCall((DamageInfo damageInfo) =>
            {
                damageInfo.damageType.damageSource = DamageSource.Primary;
            })
        );
    }


    private static void AddShootDamageSource(ILManipulationInfo info)
    {
        ILWeaver w = new(info);


        ILWeaverResult result = w.MatchRelaxed(
            x => x.MatchStfld<BulletAttack>("damageType") && w.SetCurrentTo(x)
        );
        if (!result.IsValid)
        {
            ILHelpers.LogCantHookMessage("Adding a DamgeSource to DU-T laser shot", result.FailureMessage);
            return;
        }
        w.InsertAfterCurrent(
            w.Create(OpCodes.Dup),
            w.CreateDelegateCall((BulletAttack bulletAttack) =>
            {
                bulletAttack.damageType.damageSource = DamageSource.Primary;
            })
        );
    }
}