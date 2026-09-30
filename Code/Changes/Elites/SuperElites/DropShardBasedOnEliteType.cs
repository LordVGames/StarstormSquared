using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Elites.SuperElites;


[MonoDetourTargets]
internal static class DropShardBasedOnEliteType
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.EliteEventMissionController.OnBossKilledServer.OnKilledServer.ILHook(SkipOverShardDrop);
        Mdh.SS2.EliteEventMissionController.Awake.Postfix(ShowBossDrop);
    }


    private static void ShowBossDrop(EliteEventMissionController self)
    {
        Log.Debug($"self.bossDrop is {self.bossDrop == null}");
        Log.Debug($"self.bossEliteEquipmentis {self.bossEliteEquipment == null}");
        if (self.bossDrop != null)
        {
            Log.Debug($"self.bossDrop is {self.bossDrop.name}");
        }
        if (self.bossEliteEquipment != null)
        {
            Log.Debug($"self.bossEliteEquipmentis {self.bossEliteEquipment.name}");
        }
    }


    private static void SkipOverShardDrop(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        Instruction startOfSkip = null!;
        Instruction endOfSkip = null!;


        ILWeaverResult result = w.MatchRelaxed(
            x => x.MatchLdsfld("SS2.SS2Content/Items", "ShardStorm") && w.SetInstructionTo(ref startOfSkip, x) && w.SetCurrentTo(x),
            x => x.MatchCallOrCallvirt<ItemDef>("get_itemIndex"),
            x => x.MatchCallOrCallvirt("RoR2.PickupCatalog", "FindPickupIndex"),
            x => x.MatchStloc(out _) && w.SetInstructionTo(ref endOfSkip, x)
        );
        if (!result.IsValid)
        {
            ILHelpers.LogCantHookMessage("Skipping over storm shard drop for super elites", result.FailureMessage);
            return;
        }
        w.InsertBranchOver(startOfSkip, endOfSkip);
    }
}