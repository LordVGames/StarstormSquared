using EntityStates.CloneDrone;
using MonoDetour;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.Drones.DupeDrone;


[MonoDetourTargets(typeof(CloneDroneSearch), GenerateControlFlowVariants = true)]
internal static class IgnoreTempItems
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.EntityStates.CloneDrone.CloneDroneSearch.Clone.ControlFlowPrefix(CheckIfTempFirst);
    }


    private static ReturnFlow CheckIfTempFirst(CloneDroneSearch self)
    {
        if (self.gpc == null || self.gpc.pickup == null || self.gpc.pickup.isTempItem)
        {
            return ReturnFlow.SkipOriginal;
        }
        return ReturnFlow.None;
    }
}