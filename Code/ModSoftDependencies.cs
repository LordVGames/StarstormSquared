using System;
using System.Runtime.CompilerServices;
using RoR2;
namespace StarstormSquared;


internal static class ModSoftDependencies
{
    internal static class LordsItemEditsMod
    {
        private static bool? _enabled;
        internal static bool ModIsRunning
        {
            get
            {
                _enabled ??= BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(ReheatedItems.Plugin.Id);
                return (bool)_enabled;
            }
        }


        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static float GetEditedICBMDamageMult(CharacterBody victimBody)
        {
            return ReheatedItems.ItemChanges.PocketICBMMissilesToDamage.PocketICBM.GetICBMDamageMultForCharacterBody(victimBody);
        }
    }
}