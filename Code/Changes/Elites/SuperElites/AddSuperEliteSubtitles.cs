using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2;
using SS2.Equipments;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.Elites.SuperElites;


internal static class AddSuperEliteSubtitles
{
    [MonoDetourTargets(typeof(AffixSuperFire.Behavior))]
    private static class SuperFireSubtitle
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!SS2Config.enableBeta.value)
            {
                return;
            }

            Mdh.SS2.Equipments.AffixSuperFire.Behavior.OnEnable.Postfix(AddSubtitle);
        }


        private static void AddSubtitle(AffixSuperFire.Behavior self)
        {
            self.characterBody.subtitleNameToken = "SS2_EQUIP_AFFIXSUPERFIRE_SUBTITLE";
        }
    }



    [MonoDetourTargets(typeof(AffixSuperIce.Behavior))]
    private static class SuperIceSubtitle
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!SS2Config.enableBeta.value)
            {
                return;
            }

            Mdh.SS2.Equipments.AffixSuperIce.Behavior.OnEnable.Postfix(AddSubtitle);
        }


        private static void AddSubtitle(AffixSuperIce.Behavior self)
        {
            self.characterBody.subtitleNameToken = "SS2_EQUIP_AFFIXSUPERICE_SUBTITLE";
        }
    }



    [MonoDetourTargets(typeof(AffixSuperLightning.Behavior))]
    private static class SuperLightningSubtitle
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!SS2Config.enableBeta.value)
            {
                return;
            }

            Mdh.SS2.Equipments.AffixSuperLightning.Behavior.OnEnable.Postfix(AddSubtitle);
        }


        private static void AddSubtitle(AffixSuperLightning.Behavior self)
        {
            self.characterBody.subtitleNameToken = "SS2_EQUIP_AFFIXSUPERLIGHTNING_SUBTITLE";
        }
    }



    [MonoDetourTargets(typeof(AffixSuperEarth.Behavior))]
    private static class SuperEarthSubtitle
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!SS2Config.enableBeta.value)
            {
                return;
            }

            Mdh.SS2.Equipments.AffixSuperEarth.Behavior.OnEnable.Postfix(AddSubtitle);
        }


        private static void AddSubtitle(AffixSuperEarth.Behavior self)
        {
            self.characterBody.subtitleNameToken = "SS2_EQUIP_AFFIXSUPEREARTH_SUBTITLE";
        }
    }
}