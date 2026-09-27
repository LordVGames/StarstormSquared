using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Items.Shards;


internal static class ShardIcons
{
    [SystemInitializer(dependencies: typeof(ItemCatalog))]
    private static void OnItemCatalogLoaded()
    {
        if (ModAssets.Loaded)
        {
            ChangeIcons();
        }
        else
        {
            ModAssets.OnModAssetsLoaded += ModAssets_OnModAssetsLoaded;
        }
    }
    private static void ModAssets_OnModAssetsLoaded()
    {
        ChangeIcons();
    }




    private static void ChangeIcons()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }


        Sprite shardScavIcon = ModAssets.ShardIcons.AssetBundle.LoadAsset<Sprite>("texIconShardScav");
        Sprite shardGoldIcon = ModAssets.ShardIcons.AssetBundle.LoadAsset<Sprite>("texIconShardGold");
        Sprite shardVoidIcon = ModAssets.ShardIcons.AssetBundle.LoadAsset<Sprite>("texIconShardVoid");
        Sprite shardIceIcon = ModAssets.ShardIcons.AssetBundle.LoadAsset<Sprite>("texIconShardIce");
        Sprite shardFireIcon = ModAssets.ShardIcons.AssetBundle.LoadAsset<Sprite>("texIconShardFire");
        Sprite shardStormIcon = ModAssets.ShardIcons.AssetBundle.LoadAsset<Sprite>("texIconShardStorm");
        Sprite shardEarthIcon = ModAssets.ShardIcons.AssetBundle.LoadAsset<Sprite>("texIconShardEarth");
        Sprite shardLightningIcon = ModAssets.ShardIcons.AssetBundle.LoadAsset<Sprite>("texIconShardLightning");


        SS2Content.Items.ShardScav.pickupIconSprite = shardScavIcon;
        // need to do it again this way because otherwise it shows the old icon in picker menus like the trade tele
        PickupCatalog.FindPickupIndex(SS2Content.Items.ShardScav.itemIndex).pickupDef.iconSprite = shardScavIcon;

        SS2Content.Items.ShardGold.pickupIconSprite = shardGoldIcon;
        PickupCatalog.FindPickupIndex(SS2Content.Items.ShardGold.itemIndex).pickupDef.iconSprite = shardGoldIcon;

        SS2Content.Items.ShardVoid.pickupIconSprite = shardVoidIcon;
        PickupCatalog.FindPickupIndex(SS2Content.Items.ShardVoid.itemIndex).pickupDef.iconSprite = shardVoidIcon;

        SS2Content.Items.ShardFire.pickupIconSprite = shardFireIcon;
        PickupCatalog.FindPickupIndex(SS2Content.Items.ShardFire.itemIndex).pickupDef.iconSprite = shardFireIcon;

        SS2Content.Items.ShardEarth.pickupIconSprite = shardEarthIcon;
        PickupCatalog.FindPickupIndex(SS2Content.Items.ShardEarth.itemIndex).pickupDef.iconSprite = shardEarthIcon;

        SS2Content.Items.ShardLightning.pickupIconSprite = shardLightningIcon;
        PickupCatalog.FindPickupIndex(SS2Content.Items.ShardLightning.itemIndex).pickupDef.iconSprite = shardLightningIcon;

        SS2Content.Items.ShardIce.pickupIconSprite = shardIceIcon;
        PickupCatalog.FindPickupIndex(SS2Content.Items.ShardIce.itemIndex).pickupDef.iconSprite = shardIceIcon;

        // storm shards got fully disabled for now
        if (SS2Content.Items.ShardStorm != null)
        {
            SS2Content.Items.ShardStorm.pickupIconSprite = shardStormIcon;
            PickupCatalog.FindPickupIndex(SS2Content.Items.ShardStorm.itemIndex).pickupDef.iconSprite = shardStormIcon;
        }
    }
}