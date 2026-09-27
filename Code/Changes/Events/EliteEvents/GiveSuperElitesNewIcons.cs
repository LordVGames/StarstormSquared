using MonoDetour;
using MonoDetour.HookGen;
using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Events.EliteEvents;


[MonoDetourTargets]
internal static class GiveSuperElitesNewIcons
{
    private static bool _buffCatalogLoaded = false;
    private static bool _equipmentCatalogLoaded = false;
    private static Sprite _blazingSuperEliteSprite;
    private static Sprite _glacialSuperEliteSprite;
    private static Sprite _mendingSuperEliteSprite;
    private static Sprite _overloadingSuperEliteSprite;




    [MonoDetourHookInitialize]
    private static void Setup()
    {
        ModAssets.OnModAssetsLoaded += LoadBuffSprites;
    }
    private static void LoadBuffSprites()
    {
        _blazingSuperEliteSprite = ModAssets.SuperEliteIcons.AssetBundle.LoadAsset<Sprite>("SuperEliteBlazing");
        _glacialSuperEliteSprite = ModAssets.SuperEliteIcons.AssetBundle.LoadAsset<Sprite>("SuperEliteGlacial");
        _mendingSuperEliteSprite = ModAssets.SuperEliteIcons.AssetBundle.LoadAsset<Sprite>("SuperEliteMending");
        _overloadingSuperEliteSprite = ModAssets.SuperEliteIcons.AssetBundle.LoadAsset<Sprite>("SuperEliteOverloading");
        if (_buffCatalogLoaded)
        {
            AddBuffIcons();
        }
        if (_equipmentCatalogLoaded)
        {
            AddEquipmentIcons();
        }
    }




    [SystemInitializer(dependencies: typeof(BuffCatalog))]
    private static void OnBuffCatalogLoaded()
    {
        _buffCatalogLoaded = true;
        if (ModAssets.Loaded)
        {
            AddBuffIcons();
        }
    }
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





    [SystemInitializer(dependencies: typeof(EquipmentCatalog))]
    private static void OnEquipmentCatalogLoaded()
    {
        _equipmentCatalogLoaded = true;
        if (ModAssets.Loaded)
        {
            AddEquipmentIcons();
        }
    }
    private static void AddEquipmentIcons()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }
        LoadedAssets.LoadEquipments();


        // have to load the equipments this way bc the SS2Content versions are null at this point?????
        LoadedAssets.superFireEquipmentDef.pickupIconSprite = _blazingSuperEliteSprite;
        LoadedAssets.superIceEquipmentDef.pickupIconSprite = _glacialSuperEliteSprite;
        LoadedAssets.superEarthEquipmentDef.pickupIconSprite = _mendingSuperEliteSprite;
        LoadedAssets.superLightningEquipmentDef.pickupIconSprite = _overloadingSuperEliteSprite;
    }
}