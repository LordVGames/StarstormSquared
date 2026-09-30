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
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.Networking;
using ERContent = EnemiesReturns.Content;
namespace StarstormSquared.Changes.EtherealStuff;


internal static class TradeForArraignItem
{
    private static PickupIndex _tradableRockPickupIndex;
    private static PickupIndex _lunarFlowerPickupIndex;
    private static bool CanSetup
    {
        get
        {
            return ModSupport.EnemiesReturns.ModIsRunning && ConfigOptions.Ethereal.TradeWithZanzanForArraignItem.Value;
        }
    }




    [SystemInitializer(dependencies: typeof(PickupCatalog))]
    private static void OnPickupCatalog()
    {
        if (CanSetup)
        {
            GetPickupIndices();
        }
    }
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private static void GetPickupIndices()
    {
        _tradableRockPickupIndex = PickupCatalog.FindPickupIndex(ERContent.Items.TradableRock.itemIndex);
        _lunarFlowerPickupIndex = PickupCatalog.FindPickupIndex(ERContent.Items.LunarFlower.itemIndex);
    }




    [MonoDetourTargets(typeof(EnemiesReturns.Enemies.Judgement.SetupJudgementPath), GenerateControlFlowVariants = true)]
    internal static class EnemiesReturnsEdits
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!CanSetup)
            {
                return;
            }


            Mdh.EnemiesReturns.Enemies.Judgement.SetupJudgementPath.AddInteractabilityToNewt.ControlFlowPrefix(NoNewtTrading);
            Mdh.EnemiesReturns.Enemies.Judgement.SetupJudgementPath.BazaarAddMessageIfPlayersWithRock.ControlFlowPrefix(YouveBeenBlocked);
        }


        private static ReturnFlow NoNewtTrading()
        {
            return ReturnFlow.HardReturn;
        }


        private static ReturnFlow YouveBeenBlocked(ref Stage stage)
        {
            return ReturnFlow.HardReturn;
        }
    }




    [MonoDetourTargets(typeof(TraderController), GenerateControlFlowVariants = true)]
    internal static class SS2Edits
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!CanSetup)
            {
                return;
            }


            // the way some of these hooks are done is bad but idc no one else is gonna be hooking these anyways
            Mdh.SS2.Components.TraderController.Awake.Postfix(GiveConstructSpecialValue);
            Mdh.SS2.Components.TraderController.IsSpecial.ControlFlowPrefix(MakeConstructItemAlsoSpecial);
            Mdh.SS2.Components.TraderController.BeginTrade.ILHook(SetupConstructTrade);
        }


        private static void GiveConstructSpecialValue(TraderController self)
        {
            if (!NetworkServer.active)
            {
                return;
            }


            // it shows up as 999% regardless of what i put here, but SS2 will error if i don't set anything so
            self.itemValues.Add(_tradableRockPickupIndex, 999);
        }


        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static ReturnFlow MakeConstructItemAlsoSpecial(TraderController self, ref PickupIndex tradedPickupIndex, ref bool returnValue)
        {
            ItemIndex tradedItemIndex = PickupCatalog.GetPickupDef(tradedPickupIndex).itemIndex;
            returnValue =
                tradedItemIndex == SS2Content.Items.ScavengersFortune.itemIndex
                || tradedItemIndex == ERContent.Items.TradableRock.itemIndex;
            return ReturnFlow.SkipOriginal;
        }


        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void SetupConstructTrade(ILManipulationInfo info)
        {
            ILWeaver w = new(info);


            // going after line:
            // array = new PickupIndex[] { PickupCatalog.FindPickupIndex(SS2Content.Items.ShardScav.itemIndex) };
            ILWeaverResult result = w.MatchRelaxed(
                x => x.MatchCallOrCallvirt(out _),
                x => x.MatchStelemAny<PickupIndex>(),
                x => x.MatchStloc(2) && w.SetCurrentTo(x)
            );
            if (!result.IsValid)
            {
                ILHelpers.LogCantHookMessage("Trade with Zanzan for Arraign item", result.FailureMessage);
                return;
            }
            w.InsertAfterCurrent(
                w.Create(OpCodes.Ldloc_2), // trade result array
                w.Create(OpCodes.Ldloc_0), // traded pickup index
                w.CreateDelegateCall((PickupIndex[] tradeResultArray, PickupIndex tradedPickupIndex) =>
                {
                    if (PickupCatalog.GetPickupDef(tradedPickupIndex).itemIndex != ERContent.Items.TradableRock.itemIndex)
                    {
                        return;
                    }

                    tradeResultArray[0] = _lunarFlowerPickupIndex;
                })
            );
        }
    }
}