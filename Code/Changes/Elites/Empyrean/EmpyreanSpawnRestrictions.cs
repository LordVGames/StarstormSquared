using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using SS2;
using SS2.Components;
using SS2.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;
using RoR2;
namespace StarstormSquared.Changes.Elites.Empyrean;


[MonoDetourTargets]
internal static class EmpyreanSpawnRestrictions
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        Mdh.SS2.Components.Empyrean.IsAvailable.ControlFlowPrefix(ActuallyAvailableOrNo);
    }


    private static ReturnFlow ActuallyAvailableOrNo(SS2.Components.Empyrean self, ref bool returnValue)
    {
        if (!ConfigOptions.Elites.Empyrean.AllowEmpyreanSpawn.Value)
        {
            returnValue = false;
        }
        else if (
            // idk which of these were null once so fuck it just null check all of it
            !ConfigOptions.Elites.Empyrean.AllowEmpyreansInJudgement.Value
            && Run.instance != null
            && Stage.instance != null
            && Stage.instance.sceneDef != null
        )
        {
            returnValue = Run.instance.stageClearCount > 7 && Stage.instance.sceneDef.cachedName != "enemiesreturns_outoftime";
        }
        else
        {
            return ReturnFlow.None;
        }


        return ReturnFlow.SkipOriginal;
    }
}