using MonoDetour.HookGen;
using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using MonoDetour;
using EntityStates.CaptainSupplyDrop;
using UnityEngine;
using StarstormSquared.ModSupport;
namespace StarstormSquared.Changes.Interactables.PrimalBirthrightChest;


[MonoDetourTargets(typeof(HackingMainState))]
internal static class PreventHackingForCaptain
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.EntityStates.CaptainSupplyDrop.HackingMainState.ScanForTarget.Postfix(PreventCaptainHack);
    }


    private static void PreventCaptainHack(HackingMainState self, ref PurchaseInteraction returnValue)
    {
        if (ConfigOptions.Interactables.PrimalBirthrightChest.PreventHackedByCaptain.Value && returnValue.name == "PrimalChest(Clone)")
        {
            returnValue = null;
        }
    }
}


[MonoDetourTargets(typeof(RobomandoMod.Survivors.Robomando.SkillStates.Hack))]
internal static class PreventHackingForRobomando
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.RobomandoMod.Survivors.Robomando.SkillStates.Hack.CanHack.Postfix(PreventRobomandoHack);
    }


    private static void PreventRobomandoHack(ref GameObject device, ref bool returnValue)
    {
        if (ConfigOptions.Interactables.PrimalBirthrightChest.PreventHackedByRobomando.Value && device.name == "PrimalChest(Clone)")
        {
            returnValue = false;
        }
    }
}