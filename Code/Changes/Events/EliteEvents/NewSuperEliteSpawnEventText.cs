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
using static MSU.GameplayEventTextController;
namespace StarstormSquared.Changes.Events.EliteEvents;


[MonoDetourTargets(typeof(EliteEventMissionController), GenerateControlFlowVariants = true)]
internal static class NewSuperEliteSpawnEventText
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        if (!ConfigOptions.Events.NewSuperEliteSpawnEventText.Value || !SS2Config.enableBeta.value)
        {
            return;
        }


        Mdh.SS2.EliteEventMissionController.SpawnBoss.ILHook(ChangeSuperEliteSpawnText);
    }


    private static void ChangeSuperEliteSpawnText(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        int finalEventTextRequestLdlocNumber = 0;


        ILWeaverResult result = w.MatchRelaxed(
            x => x.MatchStfld<GameplayEventTextController.EventTextRequest>("textDuration"),
            x => x.MatchLdloc(out _),
            x => x.MatchStloc(out finalEventTextRequestLdlocNumber) && w.SetCurrentTo(x)
        );
        if (!result.IsValid)
        {
            ILHelpers.LogCantHookMessage("Changing the super elite spawn text", result.FailureMessage);
            return;
        }
        w.InsertAfterCurrent(
            w.Create(OpCodes.Ldloc, finalEventTextRequestLdlocNumber),
            w.CreateDelegateCall((EventTextRequest eventTextRequest) =>
            {
                eventTextRequest.eventToken = "SS22_SUPER_ELITE_SPAWN";
                return eventTextRequest;
            }),
            w.Create(OpCodes.Stloc, finalEventTextRequestLdlocNumber)
        );
    }
}