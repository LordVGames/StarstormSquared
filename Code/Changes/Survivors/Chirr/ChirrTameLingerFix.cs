using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2;
using SS2.Components;
using SS2.Items;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.Survivors.Chirr;


[MonoDetourTargets(typeof(ChirrFriendOrb.ConvertBehavior))]
internal static class ChirrTameLingerFixAgain
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        Mdh.SS2.Components.ChirrFriendOrb.ConvertBehavior.OnTakeDamageServer.ILHook(DoChirrTameLingerBandaidFix);
    }


    private static void DoChirrTameLingerBandaidFix(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        ILLabel returnLabel = w.DefineLabel();


        ILWeaverResult result = w.MatchRelaxed(
            x => x.MatchLdarg(1),
            x => x.MatchLdfld<DamageReport>("victimBody"),
            x => x.MatchCallvirt<CharacterBody>("get_isPlayerControlled"),
            x => x.MatchBrtrue(out returnLabel) && w.SetCurrentTo(x)
        );
        if (!result.IsValid)
        {
            ILHelpers.LogCantHookMessage("Fixing chirr tame effect lingering after the debuff expires", result.FailureMessage);
            return;
        }
        w.InsertAfterCurrent(
            w.Create(OpCodes.Ldarg_1),
            w.CreateDelegateCall((DamageReport damageReport) =>
            {
                return damageReport.victimBody != null && damageReport.victimBody.HasBuff(SS2Content.Buffs.BuffChirrConvert);
            }),
            w.Create(OpCodes.Brfalse, returnLabel)
        );
    }
}