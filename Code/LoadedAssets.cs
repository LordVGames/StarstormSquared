using IL.RoR2.Skills;
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace StarstormSquared;


internal static class LoadedAssets
{
    private static readonly AssetReferenceT<ExpansionDef> _dlc1AssetReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_DLC1_Common.DLC1_asset);
    internal static ExpansionDef Dlc1;

    private static readonly AssetReferenceT<ExpansionDef> _dlc3AssetReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_DLC3.DLC3_asset);
    internal static ExpansionDef Dlc3;

    private static readonly AssetReferenceT<GameObject> _craftingChefAssetReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_DLC3_MealPrep.MealPrep_prefab);
    internal static GameObject CraftingChef;

    private static readonly AssetReferenceT<Material> _stage5TeleWaterMatReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_Base_Teleporters.matLunarTeleporterWater_mat);
    internal static Material Stage5TeleWaterMat;

    private static readonly AssetReferenceT<Material> _voidGauntletWaterMatReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_DLC1_gauntlets.matVoidGauntletWater_mat);
    internal static Material VoidGauntletWaterMat;

    private static readonly AssetReferenceT<Material> _awuFeathersMatReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_Base_RoboBallBoss.matSuperRoboBallBossBodyFeathers_mat);
    internal static Material AwuFeathersMat;

    private static readonly AssetReferenceT<Material> _smLampMatReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_Base_RoboBallBuddy.texTrimSheetAlien1RustyGreenDiffuse_png);
    internal static Material SmLampMat;

    private static readonly AssetReferenceT<RoR2.Skills.SkillDef> _toolbotDashReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_Base_Toolbot.ToolbotBodyToolbotDash_asset);
    internal static Sprite ToolbotDashIcon;

    private static readonly AssetReferenceT<RoR2.Skills.SkillDef> _toolbotSwapReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_Base_Toolbot.ToolbotBodySwap_asset);
    internal static Sprite ToolbotSwapIcon;

    private static readonly AssetReferenceT<RoR2.Skills.SkillDef> _healingDroneSkillDefReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_Base_Drones.Drone2BodyHealingBeam_asset);
    internal static Sprite HealingDroneSkillIcon;

    private static readonly AssetReferenceT<RoR2.Skills.SkillDef> _bombardmentDroneSkillDefReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_DLC3_Drones.BombardmentDroneRemoteOpSkillDef_asset);
    internal static Sprite BombardmentDroneSkillIcon;

    private static readonly AssetReferenceT<Sprite> _voidFiendHealOrbIconReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_DLC1_VoidSurvivor.texVoidSurvivorSkillIcons_png_texVoidSurvivorSkillIcons_3_);
    internal static Sprite VoidFiendHealOrbIcon;

    private static readonly AssetReferenceT<Sprite> _voidFiendCorruptSecondaryIconReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_DLC1_VoidSurvivor.texVoidSurvivorSkillIcons_png_texVoidSurvivorSkillIcons_5_);
    internal static Sprite VoidFiendCorruptSecondaryIcon;

    private static readonly AssetReferenceT<Sprite> _railgunnerShotIconReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_DLC1_Railgunner.texRailgunnerSkillIcons_png_texRailgunnerSkillIcons_10_);
    internal static Sprite RailgunnerShotIcon;
    internal static void LoadAssets()
    {
        AssetAsyncReferenceManager<ExpansionDef>.LoadAsset(_dlc1AssetReference).Completed += (handle) =>
        {
            Dlc1 = handle.Result;
        };
        AssetAsyncReferenceManager<ExpansionDef>.LoadAsset(_dlc3AssetReference).Completed += (handle) =>
        {
            Dlc3 = handle.Result;
        };
        AssetAsyncReferenceManager<GameObject>.LoadAsset(_craftingChefAssetReference).Completed += (handle) =>
        {
            CraftingChef = handle.Result;
        };
        AssetAsyncReferenceManager<Material>.LoadAsset(_stage5TeleWaterMatReference).Completed += (handle) =>
        {
            Stage5TeleWaterMat = handle.Result;
        };
        AssetAsyncReferenceManager<Material>.LoadAsset(_voidGauntletWaterMatReference).Completed += (handle) =>
        {
            VoidGauntletWaterMat = handle.Result;
        };
        AssetAsyncReferenceManager<Material>.LoadAsset(_awuFeathersMatReference).Completed += (handle) =>
        {
            AwuFeathersMat = handle.Result;
        };
        AssetAsyncReferenceManager<Material>.LoadAsset(_smLampMatReference).Completed += (handle) =>
        {
            SmLampMat = handle.Result;
        };
        AssetAsyncReferenceManager<RoR2.Skills.SkillDef>.LoadAsset(_toolbotDashReference).Completed += (handle) =>
        {
            ToolbotDashIcon = handle.Result.icon;
        };
        AssetAsyncReferenceManager<RoR2.Skills.SkillDef>.LoadAsset(_toolbotSwapReference).Completed += (handle) =>
        {
            ToolbotSwapIcon = handle.Result.icon;
        };
        AssetAsyncReferenceManager<RoR2.Skills.SkillDef>.LoadAsset(_healingDroneSkillDefReference).Completed += (handle) =>
        {
            HealingDroneSkillIcon = handle.Result.icon;
        };
        AssetAsyncReferenceManager<RoR2.Skills.SkillDef>.LoadAsset(_bombardmentDroneSkillDefReference).Completed += (handle) =>
        {
            BombardmentDroneSkillIcon = handle.Result.icon;
        };
        AssetAsyncReferenceManager<Sprite>.LoadAsset(_voidFiendHealOrbIconReference).Completed += (handle) =>
        {
            VoidFiendHealOrbIcon = handle.Result;
        };
        AssetAsyncReferenceManager<Sprite>.LoadAsset(_voidFiendCorruptSecondaryIconReference).Completed += (handle) =>
        {
            VoidFiendCorruptSecondaryIcon = handle.Result;
        };
        AssetAsyncReferenceManager<Sprite>.LoadAsset(_railgunnerShotIconReference).Completed += (handle) =>
        {
            RailgunnerShotIcon = handle.Result;
        };
    }
}