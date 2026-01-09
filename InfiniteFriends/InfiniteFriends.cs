using HarmonyLib;
using System;
using System.Reflection;
#if BEPINEX
using BepInEx;
#elif MODWEAVER
using modweaver.core;
#elif SILK
using Silk;
#endif

namespace InfiniteFriends;

internal partial class InfiniteFriends
{
    public const int MaxPlayerHardCap = 32;

    public static InfiniteFriends Instance;
    internal new static ILogger Logger { get; private set; }

    public InfiniteFriends()
    {
        InfiniteFriends.Instance = this;
    }

    private static void HarmonyPatch(string harmonyInstanceId)
    {
        try
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId);
        }
        catch (Exception ex)
        {
            InfiniteFriends.Logger.LogError(ex.ToString());
        }
    }
}

#if BEPINEX
[BepInPlugin(Metadata.PluginGuid, Metadata.PluginName, Metadata.PluginVersion)]
[BepInProcess("SpiderHeckApp.exe")]
internal partial class InfiniteFriends : BaseUnityPlugin
{
    protected void Awake()
    {
        InfiniteFriends.Logger = new BepInExLogger(BepInEx.Logging.Logger.CreateLogSource(Metadata.PluginNameShort));
        InfiniteFriends.HarmonyPatch(Metadata.PluginGuid);
    }
}
#elif MODWEAVER
[ModMainClass]
internal partial class InfiniteFriends : Mod
{
    public override void Init()
    {
        InfiniteFriends.Logger = new ModWeaverLogger(base.Logger);
        InfiniteFriends.HarmonyPatch(this.Metadata.id);
    }

    public override void Ready() { }
    public override void OnGUI(ModsMenuPopup ui) { }
}
#elif SILK
#if SILK_070
[SilkMod(Metadata.PluginName, new[] { "Dylan" }, Metadata.PluginVersion, "0.7.0", Metadata.PluginGuid, 0)]
#else
[SilkMod(Metadata.PluginName, new[] { "Dylan" }, Metadata.PluginVersion, "0.6.1", Metadata.PluginGuid, 0)]
#endif
internal partial class InfiniteFriends : SilkMod
{
    public override void Initialize()
    {
        InfiniteFriends.Logger = new SilkLoggerWrapper();
        InfiniteFriends.HarmonyPatch(Metadata.PluginGuid);
    }

    public override void Unload()
    {
    }
}
#endif
