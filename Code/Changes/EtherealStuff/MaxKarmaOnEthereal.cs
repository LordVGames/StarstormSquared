using Mdh.SS2.EtherealBehavior;
using MonoDetour;
using MonoDetour.HookGen;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.EtherealStuff;


[MonoDetourTargets]
internal static class MaxKarmaOnEthereal
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        // would use the onDifficultyUpdatedGlobal event but it isn't implemented yet
        Mdh.SS2.EtherealBehavior.SetEtherealsCompleted.Postfix(OnEtherealCompleted);
    }


    private static void OnEtherealCompleted(EtherealBehavior self, ref int etherealsCompleted)
    {
        if (etherealsCompleted > 0 && ConfigOptions.Ethereal.MaxKarmaOnEthereal.Value)
        {
            RoR2.KarmaManager.instance.effectiveKarma = RoR2.KarmaManager.maxKarma;
        }
    }
}