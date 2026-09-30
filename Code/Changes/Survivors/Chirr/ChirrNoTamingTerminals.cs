using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2;
using SS2.Components;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.Survivors.Chirr;


[MonoDetourTargets(typeof(ChirrFriendTracker), GenerateControlFlowVariants = true)]
internal static class ChirrNoTamingTerminals
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        if (!ConfigOptions.Chirr.ChirrNoTamingTerminals.Value)
        {
            return;
        }

        // i could IL hook this but ehhhhhhh whatever this is easier
        Mdh.SS2.Components.ChirrFriendTracker.CheckBody.ControlFlowPrefix(DontLetTerminalsBeTamed);
    }


    private static ReturnFlow DontLetTerminalsBeTamed(ChirrFriendTracker self, ref CharacterBody body, ref bool returnValue)
    {
        if (body == null || body.inventory == null)
        {
            returnValue = false;
            return ReturnFlow.SkipOriginal;
        }

        bool isTerminal = body.inventory.GetItemCountPermanent(SS2Content.Items.TerminationHelper) > 0;
        returnValue = !body.isBoss && !isTerminal;
        return ReturnFlow.SkipOriginal;
    }
}