using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2.ContentManagement;
using SS2;
using SS2.Equipments;
using SS2.Items;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.Elites.Toxic;


[MonoDetourTargets(typeof(AffixPurple), GenerateControlFlowVariants = true)]
internal static class ToxicEliteToggle
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        Mdh.SS2.Equipments.AffixPurple.IsAvailable.ControlFlowPrefix(ShouldWeOrNot);
    }


    private static ReturnFlow ShouldWeOrNot(AffixPurple self, ref ContentPack contentPack, ref bool returnValue)
    {
        returnValue = SS2Config.enableBeta.value && ConfigOptions.Elites.Toxic.EnableToxicElite.Value;
        return ReturnFlow.SkipOriginal;
    }
}