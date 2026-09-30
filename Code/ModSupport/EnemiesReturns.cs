using EnemiesReturns;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.ModSupport;


internal static class EnemiesReturns
{
    private static bool? _modexists;
    internal static bool ModIsRunning
    {
        get
        {
            _modexists ??= BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(EnemiesReturnsPlugin.GUID);
            return (bool)_modexists;
        }
    }
}