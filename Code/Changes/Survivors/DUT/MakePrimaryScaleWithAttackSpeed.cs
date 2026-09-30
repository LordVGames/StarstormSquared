using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.Survivors.DUT;


[MonoDetourTargets]
internal static class MakePrimaryScaleWithAttackSpeed
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }


        Mdh.EntityStates.DUT.ChargeDamage.FixedUpdate.ILHook(AddAttackSpeedScaling);
    }


    private static void AddAttackSpeedScaling(ILManipulationInfo info)
    {
        ILWeaver w = new(info);


         ILWeaverResult result = w.MatchMultipleRelaxed(
            onMatch: (Action<ILWeaver>)(mW =>
            {
                mW.InsertAfterCurrent(
                    w.Create(OpCodes.Ldarg_0),
                    w.CreateDelegateCall((float baseDuration, EntityStates.DUT.ChargeDamage chargeDamage) =>
                    {
                        return baseDuration / chargeDamage.controller.characterBody.attackSpeed;
                    })
                );
            }),
            x => x.MatchLdsfld<EntityStates.DUT.ChargeDamage>("baseDuration") && w.SetCurrentTo(x)
        );
        if (!result.IsValid)
        {
            ILHelpers.LogCantHookMessage("Adding attack speed scaling to DU-T charge damage", result.FailureMessage);
            return;
        }
    }
}