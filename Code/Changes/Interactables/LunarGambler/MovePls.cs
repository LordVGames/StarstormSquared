using MonoDetour;
using MonoDetour.HookGen;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Interactables.LunarGambler;


[MonoDetourTargets(typeof(SS2.Interactables.LunarGambler))]
internal static class MovePls
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.Interactables.LunarGambler.SpawnTable.Prefix(ChangePosition);
    }


    private static void ChangePosition(SS2.Interactables.LunarGambler self, ref Stage stage)
    {
        SS2.Interactables.LunarGambler.position = new Vector3(-133.3257f, -25.276f, -22.0506f);
        SS2.Interactables.LunarGambler.rotation = Vector3.zero;
    }
}