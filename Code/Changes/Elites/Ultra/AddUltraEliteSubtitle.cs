using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2;
using SS2.Items;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.Elites.Ultra;


[MonoDetourTargets(typeof(AffixUltra), GenerateControlFlowVariants = true)]
internal static class AddUltraEliteSubtitle
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }


        Mdh.SS2.Items.AffixUltra.BodyBehavior.Start.Postfix(AddSS1Subtitle);
    }


    // can't really store the subtitle without messing with FixedConditionalWeakTables but i don't think it matters that much
    private static void AddSS1Subtitle(AffixUltra.BodyBehavior self)
    {
        self.body?.subtitleNameToken = "SS22_ULTRA_SUBTITLE_SS1";
    }
}