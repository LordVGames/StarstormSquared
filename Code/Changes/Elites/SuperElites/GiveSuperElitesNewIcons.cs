using MonoDetour;
using MonoDetour.HookGen;
using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Elites.SuperElites;


[MonoDetourTargets]
internal static class GiveSuperElitesNewIcons
{
    private static Sprite _blazingSuperEliteSprite;
    private static Sprite _glacialSuperEliteSprite;
    private static Sprite _mendingSuperEliteSprite;
    private static Sprite _overloadingSuperEliteSprite;








    
    [MonoDetourHookInitialize]
    private static void LoadBuffSprites()
    {
        _blazingSuperEliteSprite = ModAssets.SuperEliteIcons.AssetBundle.LoadAsset<Sprite>("SuperEliteBlazing");
        _glacialSuperEliteSprite = ModAssets.SuperEliteIcons.AssetBundle.LoadAsset<Sprite>("SuperEliteGlacial");
        _mendingSuperEliteSprite = ModAssets.SuperEliteIcons.AssetBundle.LoadAsset<Sprite>("SuperEliteMending");
        _overloadingSuperEliteSprite = ModAssets.SuperEliteIcons.AssetBundle.LoadAsset<Sprite>("SuperEliteOverloading");
    }
    [SystemInitializer(dependencies: typeof(EquipmentCatalog))]
    private static void OnEquipmentCatalog()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }


        EquipmentIndex superFireEquipmentIndex = EquipmentCatalog.FindEquipmentIndex("EliteSuperFireEquipment");
        EquipmentIndex superIceEquipmentIndex = EquipmentCatalog.FindEquipmentIndex("EliteSuperIceEquipment");
        EquipmentIndex superEarthEquipmentIndex = EquipmentCatalog.FindEquipmentIndex("EliteSuperEarthEquipment");
        EquipmentIndex superLightningEquipmentIndex = EquipmentCatalog.FindEquipmentIndex("EliteSuperLightningEquipment");


        EquipmentDef superFireEquipmentDef = EquipmentCatalog.GetEquipmentDef(superFireEquipmentIndex);
        EquipmentDef superIceEquipmentDef = EquipmentCatalog.GetEquipmentDef(superIceEquipmentIndex);
        EquipmentDef superEarthEquipmentDef = EquipmentCatalog.GetEquipmentDef(superEarthEquipmentIndex);
        EquipmentDef superLightningEquipmentDef = EquipmentCatalog.GetEquipmentDef(superLightningEquipmentIndex);


#pragma warning disable CS0618 // Type or member is obsolete
        superFireEquipmentDef.pickupIconSprite = _blazingSuperEliteSprite;
        superIceEquipmentDef.pickupIconSprite = _glacialSuperEliteSprite;
        superEarthEquipmentDef.pickupIconSprite = _mendingSuperEliteSprite;
        superLightningEquipmentDef.pickupIconSprite = _overloadingSuperEliteSprite;
#pragma warning restore CS0618 // Type or member is obsolete
    }




    [SystemInitializer(dependencies: typeof(BuffCatalog))]
    private static void AddBuffIcons()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }


        SS2Content.Buffs.BuffAffixSuperFire.iconSprite = _blazingSuperEliteSprite;
        // why is only super fire solid red??? what
        SS2Content.Buffs.BuffAffixSuperFire.buffColor = SS2Content.Buffs.BuffAffixSuperIce.buffColor;
        SS2Content.Buffs.BuffAffixSuperIce.iconSprite = _glacialSuperEliteSprite;
        SS2Content.Buffs.BuffAffixSuperEarth.iconSprite = _mendingSuperEliteSprite;
        SS2Content.Buffs.BuffAffixSuperLightning.iconSprite = _overloadingSuperEliteSprite;
    }
}