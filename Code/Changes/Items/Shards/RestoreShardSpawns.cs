using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using SS2;
using SS2.Items;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using MonoMod.RuntimeDetour;
using Newtonsoft.Json.Utilities;
namespace StarstormSquared.Changes.Items.Shards;


internal static class RestoreShardSpawns
{
    [MonoDetourTargets(typeof(ShardGold))]
    internal static class RestoreGoldShardDrop
    {
        internal static ShardGold shardGoldInstance;

        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!ConfigOptions.Shards.RestoreGoldShardDrop.Value)
            {
                return;
            }

            Mdh.SS2.Items.ShardGold.Initialize.Postfix(AddCommentedOutHook);
        }

        private static void AddCommentedOutHook(ShardGold self)
        {
            shardGoldInstance = self;
            // would use a monodetour version of the on hooks but then i couldn't use the existing ss2 method since monodetour hooks don't come with an orig
            On.RoR2.HalcyoniteShrineInteractable.DropRewards += YouWillDropGoldShards;
        }

        private static void YouWillDropGoldShards(On.RoR2.HalcyoniteShrineInteractable.orig_DropRewards orig, HalcyoniteShrineInteractable self)
        {
            // not calling orig(self) here is fine bc the ss2 method calls it for me
            // i feel like this shouldn't work but it does
            Mdh.SS2.Items.ShardGold.SpawnGoldShard.Target().Invoke(shardGoldInstance, [orig, self]);
        }
    }



    [MonoDetourTargets(typeof(ShardVoid))]
    internal static class RestoreVoidShardDrop
    {
        internal static ShardVoid shardVoidInstance;

        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!ConfigOptions.Shards.RestoreVoidShardDrop.Value)
            {
                return;
            }

            Mdh.SS2.Items.ShardVoid.Initialize.Postfix(AddCommentedOutHook);
        }

        private static void AddCommentedOutHook(ShardVoid self)
        {
            shardVoidInstance = self;
            On.EntityStates.VoidCamp.Deactivate.OnEnter += YouWillDropVoidShards;
        }

        private static void YouWillDropVoidShards(On.EntityStates.VoidCamp.Deactivate.orig_OnEnter orig, EntityStates.VoidCamp.Deactivate self)
        {
            Mdh.SS2.Items.ShardVoid.SpawnVoidShard.Target().Invoke(shardVoidInstance, [orig, self]);
        }
    }


    [MonoDetourTargets(typeof(EntityStates.Events.Storm))]
    internal static class RestoreStormShardDrop
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!ConfigOptions.Shards.RestoreStormShardDrops.Value || !Storm.ReworkedStorm)
            {
                return;
            }

            Mdh.EntityStates.Events.Storm.OnEnter.Postfix(AddShardDropOnCompletion);
            Mdh.EntityStates.Events.Storm.OnExit.Postfix(RemoveShardDropOnCompletion);
        }


        private static void AddShardDropOnCompletion(EntityStates.Events.Storm storm)
        {
            if (!NetworkServer.active)
            {
                return;
            }
            CombatDirector bossDirector = TeleporterInteraction.instance?.bossDirector;
            if (bossDirector == null || storm.stormLevel < EntityStates.Events.Storm.bossEliteLevel)
            {
                return;
            }

            BossGroup.onBossGroupDefeatedServer += OnBossGroupDefeatedServer;
        }
        private static void RemoveShardDropOnCompletion(EntityStates.Events.Storm storm)
        {
            if (!NetworkServer.active)
            {
                return;
            }
            CombatDirector bossDirector = TeleporterInteraction.instance?.bossDirector;
            if (bossDirector == null || storm.stormLevel < EntityStates.Events.Storm.bossEliteLevel)
            {
                return;
            }

            BossGroup.onBossGroupDefeatedServer -= OnBossGroupDefeatedServer;
        }
        private static void OnBossGroupDefeatedServer(BossGroup bossGroup)
        {
            if (bossGroup == TeleporterInteraction.instance.bossGroup && Run.instance.participatingPlayerCount > 0)
            {
                int playerCount = Run.instance.participatingPlayerCount;
                float angle = 360f / (float)playerCount;
                Vector3 vector = Quaternion.AngleAxis((float)UnityEngine.Random.Range(0, 360), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 5f);
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
                PickupIndex drop = PickupCatalog.FindPickupIndex(SS2Content.Items.ShardStorm.itemIndex);
                int i = 0;
                while (i < playerCount)
                {
                    PickupDropletController.CreatePickupDroplet(new UniquePickup { pickupIndex = drop }, bossGroup.dropPosition.position, vector, false);
                    i++;
                    vector = rotation * vector;
                }
            }
        }
    }
}