using System;
using System.IO;
using UnityEngine;
namespace StarstormSquared;


internal static class ModAssets
{
    internal static bool Loaded = false;
    public static event Action OnModAssetsLoaded;


    internal static void Init()
    {
        ShardIcons.Init();
        SurvivorIcons.Init();
        SuperEliteIcons.Init();
        RiskOfOptionsIcon.Init();
        Loaded = true;
        OnModAssetsLoaded?.Invoke();
    }


    internal static class ShardIcons
    {
        public static AssetBundle AssetBundle;
        public const string BundleName = "ss22_shard_icons";

        public static string AssetBundlePath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Plugin.PluginInfo.Location), BundleName);
            }
        }

        internal static void Init()
        {
            AssetBundle = AssetBundle.LoadFromFile(AssetBundlePath);
        }
    }


    internal static class SurvivorIcons
    {
        public static AssetBundle AssetBundle;
        public const string BundleName = "ss22_survivor_icons";

        public static string AssetBundlePath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Plugin.PluginInfo.Location), BundleName);
            }
        }

        internal static void Init()
        {
            AssetBundle = AssetBundle.LoadFromFile(AssetBundlePath);
        }
    }


    internal static class SuperEliteIcons
    {
        public static AssetBundle AssetBundle;
        public const string BundleName = "ss22_super_elite_icons";

        public static string AssetBundlePath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Plugin.PluginInfo.Location), BundleName);
            }
        }

        internal static void Init()
        {
            AssetBundle = AssetBundle.LoadFromFile(AssetBundlePath);
        }
    }


    internal static class RiskOfOptionsIcon
    {
        public static AssetBundle AssetBundle;
        public const string BundleName = "ss22_roo_icon";

        public static string AssetBundlePath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Plugin.PluginInfo.Location), BundleName);
            }
        }

        internal static void Init()
        {
            AssetBundle = AssetBundle.LoadFromFile(AssetBundlePath);
        }
    }
}
