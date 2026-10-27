using BepInEx;
using MonoDetour;
[assembly: HG.Reflection.SearchableAttribute.OptIn]
namespace StarstormSquared;


// making these next 2 soft dependencies since i can't hard depend on either and be fine with one being missing
[BepInDependency(SS2.SS2Main.GUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(R2API.LanguageAPI.PluginGUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(ReheatedItems.Plugin.Id, BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency(RiskOfOptions.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]
[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    public static PluginInfo PluginInfo { get; private set; }
    public void Awake()
    {
        PluginInfo = Info;
        ConfigOptions.BindAllConfigOptions(Config);
        Log.Init(Logger);
        ModAssets.Init();
        LoadedAssets.LoadAssets();
        MonoDetourManager.InvokeHookInitializers(typeof(Plugin).Assembly, reportUnloadableTypes: false);
    }
}