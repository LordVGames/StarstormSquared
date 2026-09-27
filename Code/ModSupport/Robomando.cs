using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.ModSupport;


internal static class Robomando
{
    private static bool? _enabled;

    internal static bool ModIsRunning
    {
        get
        {
            _enabled ??= BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(RobomandoMod.RobomandoPlugin.MODUID);
            return (bool)_enabled;
        }
    }
}