using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using SS2;
using UnityEngine;
namespace StarstormSquared.Changes.Survivors;


internal static class SurvivorIcons
{
    [SystemInitializer(dependencies: typeof(BodyCatalog))]
    private static void OnBodyCatalogLoaded()
    {
        if (ModAssets.Loaded)
        {
            ChangeSurvivorIcons();
        }
        else
        {
            ModAssets.OnModAssetsLoaded += ModAssets_OnModAssetsLoaded;
        }
    }
    private static void ModAssets_OnModAssetsLoaded()
    {
        ChangeSurvivorIcons();
    }




    internal static void ChangeSurvivorIcons()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }
        if (!ModAssets.Loaded)
        {
            ModAssets.OnModAssetsLoaded += ModAssets_OnModAssetsLoaded;
            return;
        }


        GameObject knightGameObject = BodyCatalog.FindBodyPrefab("KnightBody");
        if (knightGameObject == null || !knightGameObject.TryGetComponent<CharacterBody>(out var knightBody))
        {
            Log.Warning("Knight could not be found to give a new portait icon to!");
        }
        else
        {
            knightBody.portraitIcon = ModAssets.SurvivorIcons.AssetBundle.LoadAsset<Texture>("texIconKnight");
        }



        GameObject cyborgGameObject = BodyCatalog.FindBodyPrefab("Cyborg2Body");
        if (cyborgGameObject == null || !cyborgGameObject.TryGetComponent<CharacterBody>(out var cyborgBody))
        {
            Log.Warning("Cyborg could not be found to give a new portait icon to!");
        }
        else
        {
            cyborgBody.portraitIcon = ModAssets.SurvivorIcons.AssetBundle.LoadAsset<Texture>("texIconCyborg");
        }
    }
}