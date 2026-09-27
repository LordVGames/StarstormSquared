using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using RoR2.Orbs;
using SS2;
using SS2.Items;
using System;
using UnityEngine;
namespace StarstormSquared.Changes.Items;


[MonoDetourTargets(typeof(ErraticGadget), GenerateControlFlowVariants = true)]
internal static class ErraticGadgetItem
{
    private static ErraticGadget erraticGadgetInstance;


    [MonoDetourHookInitialize]
    private static void Setup()
    {
        if (ConfigOptions.ItemChanges.ErraticGadgetItem.AllowVoidLightning.Value)
        {
            Mdh.SS2.Items.ErraticGadget.Initialize.Postfix(ReAddVoidLightningHook);
        }
        switch (ConfigOptions.ItemChanges.ErraticGadgetItem.Change.Value)
        {
            case ConfigOptions.ItemChanges.ErraticGadgetItem.ErraticGadgetChangeType.None:
                return;
            case ConfigOptions.ItemChanges.ErraticGadgetItem.ErraticGadgetChangeType.DamageMultAndOnHitProc:
                Mdh.SS2.Items.ErraticGadget.LightningOrb_OnArrival.ILHook(SkipDoublingProc);
                Mdh.SS2.Items.ErraticGadget.LightningStrikeOrb_OnArrival.ControlFlowPrefix(LightningStrikeOrb_JustMoreDamage);
                Mdh.SS2.Items.ErraticGadget.SimpleLightningStrikeOrb_OnArrival.ControlFlowPrefix(SimpleLightningStrikeOrb_JustMoreDamage);
                Mdh.SS2.Items.ErraticGadget.VoidLightningOrb_Begin.ControlFlowPrefix(VoidLightningOrb_JustMoreDamage);
                break;
            case ConfigOptions.ItemChanges.ErraticGadgetItem.ErraticGadgetChangeType.OnlyDamageMult:
                Mdh.SS2.Items.ErraticGadget.LightningOrb_OnArrival.ILHook(SkipDoublingProc);
                Mdh.SS2.Items.ErraticGadget.LightningStrikeOrb_OnArrival.ControlFlowPrefix(LightningStrikeOrb_JustMoreDamage);
                Mdh.SS2.Items.ErraticGadget.SimpleLightningStrikeOrb_OnArrival.ControlFlowPrefix(SimpleLightningStrikeOrb_JustMoreDamage);
                Mdh.SS2.Items.ErraticGadget.VoidLightningOrb_Begin.ControlFlowPrefix(VoidLightningOrb_JustMoreDamage);
                Mdh.SS2.Items.ErraticGadget.Behavior.OnDamageDealtServer.ControlFlowPrefix(ErraticGadget_OnDamageDealtServer);
                break;
        }
    }




    [SystemInitializer(dependencies: typeof(ItemCatalog))]
    private static void ChangeTokens()
    {
        if (SS2Content.Items.ErraticGadget == null)
        {
            return;
        }


        switch (ConfigOptions.ItemChanges.ErraticGadgetItem.Change.Value)
        {
            case ConfigOptions.ItemChanges.ErraticGadgetItem.ErraticGadgetChangeType.None:
                return;
            case ConfigOptions.ItemChanges.ErraticGadgetItem.ErraticGadgetChangeType.DamageMultAndOnHitProc:
                // idk how to make these more modular yet
                if (ConfigOptions.ItemChanges.ErraticGadgetItem.AllowVoidLightning.Value)
                {
                    SS2Content.Items.ErraticGadget.pickupToken = "SS22_ITEM_ERRATICGADGET_CHANGE1_VOID_ALLOWED_PICKUP";
                    SS2Content.Items.ErraticGadget.descriptionToken = "SS22_ITEM_ERRATICGADGET_CHANGE1_VOID_ALLOWED_DESC";
                }
                else
                {
                    SS2Content.Items.ErraticGadget.pickupToken = "SS22_ITEM_ERRATICGADGET_CHANGE1_PICKUP";
                    SS2Content.Items.ErraticGadget.descriptionToken = "SS22_ITEM_ERRATICGADGET_CHANGE1_DESC";
                }
                break;
            case ConfigOptions.ItemChanges.ErraticGadgetItem.ErraticGadgetChangeType.OnlyDamageMult:
                if (ConfigOptions.ItemChanges.ErraticGadgetItem.AllowVoidLightning.Value)
                {
                    SS2Content.Items.ErraticGadget.pickupToken = "SS22_ITEM_ERRATICGADGET_CHANGE2_VOID_ALLOWED_PICKUP";
                    SS2Content.Items.ErraticGadget.descriptionToken = "SS22_ITEM_ERRATICGADGET_CHANGE2_VOID_ALLOWED_DESC";
                }
                else
                {
                    SS2Content.Items.ErraticGadget.pickupToken = "SS22_ITEM_ERRATICGADGET_CHANGE2_PICKUP";
                    SS2Content.Items.ErraticGadget.descriptionToken = "SS22_ITEM_ERRATICGADGET_CHANGE2_DESC";
                }
                break;
        }
    }




    private static void SkipDoublingProc(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        ILLabel skipDoublingProc = w.DefineLabel();


        // next 2 blocks are to skip over the part where lightning is doubled
        // going to "bool flag2 = false;"
        w.MatchRelaxed(
            x => x.MatchLdcI4(0) && w.SetCurrentTo(x),
            x => x.MatchStloc(0)
        ).ThrowIfFailure()
        .InsertBeforeCurrentStealLabels(
            w.Create(OpCodes.Br, skipDoublingProc)
        );


        // going to the 2nd "orig.Invoke(self);"
        w.MatchRelaxed(
            x => x.MatchLdarg(1) && w.SetCurrentTo(x),
            x => x.MatchLdarg(2),
            x => x.MatchCallvirt(out _),
            x => x.MatchNop(),
            x => x.MatchLdarg(2)
        ).ThrowIfFailure()
        .InsertBeforeCurrent(w.Create(OpCodes.Ldarg_2))
        .MarkLabelToCurrent(skipDoublingProc)
        .InsertBeforeCurrent(
            w.CreateCall(JustDealMoreDamage)
        );
    }
    private static void JustDealMoreDamage(LightningOrb lightningOrb)
    {
        if (!AllowErraticGadgetDamageBuff(lightningOrb.attacker))
        {
            return;
        }


        float mult = GetErraticGadgetDamageMult(lightningOrb.attacker);
        lightningOrb.damageValue *= mult;
        lightningOrb.procCoefficient *= mult;
    }




    // polylute
    private static void ReAddVoidLightningHook(ErraticGadget self)
    {
        erraticGadgetInstance = self;
        On.RoR2.Orbs.VoidLightningOrb.Begin += AffectVoidLightningAnyways;
    }
    private static void AffectVoidLightningAnyways(On.RoR2.Orbs.VoidLightningOrb.orig_Begin orig, VoidLightningOrb self)
    {
        Mdh.SS2.Items.ErraticGadget.VoidLightningOrb_Begin.Target().Invoke(erraticGadgetInstance, [orig, self]);
    }
    private static ReturnFlow VoidLightningOrb_JustMoreDamage(ErraticGadget self, ref On.RoR2.Orbs.VoidLightningOrb.orig_Begin orig, ref VoidLightningOrb voidLightningOrb)
    {
        if (!AllowErraticGadgetDamageBuff(voidLightningOrb.attacker))
        {
            orig(voidLightningOrb);
            return ReturnFlow.SkipOriginal;
        }


        float mult = GetErraticGadgetDamageMult(voidLightningOrb.attacker);
        voidLightningOrb.damageValue *= mult;
        voidLightningOrb.procCoefficient *= mult;


        orig(voidLightningOrb);
        return ReturnFlow.SkipOriginal;
    }




    // charged perforator
    private static ReturnFlow SimpleLightningStrikeOrb_JustMoreDamage(ErraticGadget self, ref On.RoR2.Orbs.SimpleLightningStrikeOrb.orig_OnArrival orig, ref SimpleLightningStrikeOrb simpleLightningStrikeOrb)
    {
        if (!AllowErraticGadgetDamageBuff(simpleLightningStrikeOrb.attacker))
        {
            orig(simpleLightningStrikeOrb);
            return ReturnFlow.SkipOriginal;
        }


        float mult = GetErraticGadgetDamageMult(simpleLightningStrikeOrb.attacker);
        simpleLightningStrikeOrb.damageValue *= mult;
        simpleLightningStrikeOrb.procCoefficient *= mult;


        orig(simpleLightningStrikeOrb);
        return ReturnFlow.SkipOriginal;
    }




    // royal capacitor
    private static ReturnFlow LightningStrikeOrb_JustMoreDamage(ErraticGadget self, ref On.RoR2.Orbs.LightningStrikeOrb.orig_OnArrival orig, ref LightningStrikeOrb lightningStrikeOrb)
    {
        if (!AllowErraticGadgetDamageBuff(lightningStrikeOrb.attacker))
        {
            orig(lightningStrikeOrb);
            return ReturnFlow.SkipOriginal;
        }

        lightningStrikeOrb.damageValue *= GetErraticGadgetDamageMult(lightningStrikeOrb.attacker);


        orig(lightningStrikeOrb);
        return ReturnFlow.SkipOriginal;
    }




    private static bool AllowErraticGadgetDamageBuff(GameObject attacker)
    {
        if (
            attacker == null
            || !attacker.TryGetComponent<CharacterBody>(out CharacterBody attackerBody)
            || attackerBody.inventory == null
            || attackerBody.inventory.GetItemCountEffective(SS2Content.Items.ErraticGadget) < 1
        )
        {
            return false;
        }
        return true;
    }




    private static float GetErraticGadgetDamageMult(GameObject attacker)
    {
        // not using AllowErraticGadgetDamageBuff here bc i need to store the erratic gadget count
        // so without it it's one less GetItemCountEffective call
        if (
            attacker == null
            || !attacker.TryGetComponent<CharacterBody>(out CharacterBody attackerBody)
            || attackerBody.inventory == null
        )
        {
            return 1;
        }
        int currentErraticGadgetCount = attackerBody.inventory.GetItemCountEffective(SS2Content.Items.ErraticGadget);
        if (currentErraticGadgetCount < 1)
        {
            return 1;
        }


        if (ConfigOptions.ItemChanges.ErraticGadgetItem.Change.Value == ConfigOptions.ItemChanges.ErraticGadgetItem.ErraticGadgetChangeType.DamageMultAndOnHitProc)
        {
            return 2;
        }
        else if (ConfigOptions.ItemChanges.ErraticGadgetItem.Change.Value == ConfigOptions.ItemChanges.ErraticGadgetItem.ErraticGadgetChangeType.OnlyDamageMult)
        {
            return 3 + (1.5f * (currentErraticGadgetCount - 1));
        }


        return 1;
    }




    private static ReturnFlow ErraticGadget_OnDamageDealtServer(ErraticGadget.Behavior self, ref DamageReport report)
    {
        return ReturnFlow.SkipOriginal;
    }
}