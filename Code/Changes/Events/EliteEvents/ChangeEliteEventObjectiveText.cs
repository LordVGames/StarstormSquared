using System;
using MonoDetour;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using RoR2;
using SS2;
namespace StarstormSquared.Changes.Events.EliteEvents;


[MonoDetourTargets]
internal static class ChangeEliteEventObjectiveText
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.EliteEventMissionController.EliteObjectiveTracker.GenerateString.ControlFlowPrefix(ReturnNewString);
    }


    private static ReturnFlow ReturnNewString(EliteEventMissionController.EliteObjectiveTracker self, ref string returnValue)
    {
        EliteEventMissionController mission = (EliteEventMissionController)self.sourceDescriptor.source;
        returnValue = Language.GetStringFormatted("SS22_ELITE_EVENT_OBJECTIVE", (float)mission.currentEliteKills / (float)mission.requiredEliteKills * 100f);
        return ReturnFlow.SkipOriginal;
    }
}