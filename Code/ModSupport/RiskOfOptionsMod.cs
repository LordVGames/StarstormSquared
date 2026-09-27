using MonoDetour;
using MonoDetour.HookGen;
using RiskOfOptions;
using StarstormSquared;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
namespace StarstormSquared.ModSupport;


[MonoDetourTargets]
internal class RiskOfOptionsMod
{
    private static bool? _modexists;
    internal static bool ModIsRunning
    {
        get
        {
            _modexists ??= BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(PluginInfo.PLUGIN_GUID);
            return (bool)_modexists;
        }
    }
    private const string _iconName = "icon.png";




    [MonoDetourHookInitialize]
    private static void SetupIconAndDescription()
    {
        if (ModIsRunning)
        {
            ModAssets.OnModAssetsLoaded += ModAssets_OnModAssetsLoaded;
        }
    }
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private static void ModAssets_OnModAssetsLoaded()
    {
        ModSettingsManager.SetModIcon(ModAssets.RiskOfOptionsIcon.AssetBundle.LoadAsset<Sprite>(_iconName));
        ModSettingsManager.SetModDescription("An addon mod for Starstorm 2 that fixes, changes or rebalances parts of the mod, especially beta content.");
    }
}