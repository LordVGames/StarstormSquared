using MonoDetour;
using MonoDetour.HookGen;
using R2API;
using RoR2;
using RoR2.ContentManagement;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace StarstormSquared.Changes.Items.RestoreUnused;


[MonoDetourTargets(typeof(EquipmentCatalog))]
internal static class GForceAccelerator
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        if (!ConfigOptions.RestoreUnused.GForceAccelerator.Value)
        {
            return;
        }


        Mdh.RoR2.EquipmentCatalog.Init.Prefix(BeforeInit);
    }


    private static void BeforeInit()
    {
        DLC3Content.Equipment.GroundEnemies.canDrop = true;
        DLC3Content.Equipment.GroundEnemies.canBeRandomlyTriggered = true;
        DLC3Content.Equipment.GroundEnemies.appearsInSinglePlayer = true;
        DLC3Content.Equipment.GroundEnemies.appearsInMultiPlayer = true;
        DLC3Content.Equipment.GroundEnemies.colorIndex = ColorCatalog.ColorIndex.Equipment;
        DLC3Content.Equipment.GroundEnemies.enigmaCompatible = true;
        DLC3Content.Equipment.GroundEnemies.foodRelated = false;
        DLC3Content.Equipment.GroundEnemies.isConsumed = false;
    }
}