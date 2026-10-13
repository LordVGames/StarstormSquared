using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Newtonsoft.Json.Utilities;
using RoR2;
using SS2;
using SS2.Items;
using StarstormSquared.Changes.Stages.SlateMines;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
namespace StarstormSquared.Changes.Items.Shards;


internal static class RestoreShardSpawns
{
    [MonoDetourTargets(typeof(PickupPickerController))]
    internal static class RestoreGoldShardDrop
    {
        private static bool stageHasHalcyonShrine = false;
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!ConfigOptions.Shards.RestoreGoldShardDrop.Value)
            {
                return;
            }


            // doing this off of MultiOptionPickerPanel doesn't work, this does however
            Mdh.RoR2.PickupPickerController.KillPanel.Prefix(YouWillDropGoldShards);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }


        private static void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            stageHasHalcyonShrine = false;
            foreach (var gameObject in scene.GetRootGameObjects())
            {
                if (gameObject.name.Contains("ShrineHalcyonite"))
                {
                    stageHasHalcyonShrine = true;
                    return;
                }
            }
        }


        private static void YouWillDropGoldShards(PickupPickerController self)
        {
            if (self.gameObject.name.Equals("FragmentPotentialPickup(Clone)") && stageHasHalcyonShrine)
            {
                self.GetComponent<PickupPickerController>()?.CreatePickup(PickupCatalog.FindPickupIndex(SS2Content.Items.ShardGold.itemIndex));
            }
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
            if (
                bossGroup != null
                && TeleporterInteraction.instance.bossGroup != null
                && bossGroup == TeleporterInteraction.instance.bossGroup
                && Run.instance.participatingPlayerCount > 0
            )
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