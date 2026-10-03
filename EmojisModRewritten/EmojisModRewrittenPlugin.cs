using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using Reactor.Utilities;

namespace EmojisModRewritten;

[BepInAutoPlugin("com.missingpixel.emojis", "Emojis Mod: Rewritten", "1.0.0")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
public partial class EmojisModRewrittenPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public override void Load()
    {
        Harmony.PatchAll();
        EmojisModAssets.Initialize();
        ReactorCredits.Register<EmojisModRewrittenPlugin>(_ => true);
        Log.LogInfo("Emojis Mod Rewritten loaded successfully! >.<");
    }
}