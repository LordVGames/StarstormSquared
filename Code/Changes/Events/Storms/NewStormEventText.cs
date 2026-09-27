using MiscFixes.Modules;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using MSU;
using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Events.Storms;


[MonoDetourTargets(typeof(EntityStates.Events.Storm))]
internal static class NewStormEventText
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        if (!ConfigOptions.Events.NewStormEventText.Value || !Storm.ReworkedStorm.value)
        {
            return;
        }

        Mdh.EntityStates.Events.Storm.OnEnter.ILHook(ChangeStormEventText);
        Mdh.EntityStates.Events.Storm.FixedUpdate.ILHook(ChangeStormEventEndText);
    }


    private static void ChangeStormEventText(ILManipulationInfo info)
    {
        ILWeaver w = new(info);


        w.MatchRelaxed(
            x => x.MatchLdloc(out _),
            x => x.MatchStloc(2) && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertAfterCurrent(
            w.Create(OpCodes.Ldarg_0),
            w.Create(OpCodes.Ldloc_2),
            w.CreateCall(GetCorrectStormTextToken),
            w.Create(OpCodes.Stloc_2)
        );
    }
    private static GameplayEventTextController.EventTextRequest GetCorrectStormTextToken(EntityStates.Events.Storm storm, GameplayEventTextController.EventTextRequest eventTextRequest)
    {
        eventTextRequest.eventToken = storm.stormLevel switch
        {
            1 => "SS2_EVENT_THUNDERSTORM_START",
            2 => "SS22_STORM_LEVEL_2",
            3 => "SS22_STORM_LEVEL_3",
            4 => "SS22_STORM_LEVEL_4",
            _ => Language.GetStringFormatted("SS22_STORM_LEVEL_OTHER", storm.stormLevel),
        };
        return eventTextRequest;
    }


    private static void ChangeStormEventEndText(ILManipulationInfo info)
    {
        ILWeaver w = new(info);

        w.MatchRelaxed(
            x => x.MatchLdstr("ermmmm..... bye storm") && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .ReplaceCurrentOperand("SS2_EVENT_THUNDERSTORM_END");
    }
}