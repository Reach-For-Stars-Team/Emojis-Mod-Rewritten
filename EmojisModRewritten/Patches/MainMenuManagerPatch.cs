using HarmonyLib;

namespace EmojisModRewritten.Patches;

[HarmonyPatch(typeof(MainMenuManager))]
public static class MainMenuManagerPatch
{
    [HarmonyPatch(nameof(MainMenuManager.Start))]
    [HarmonyPostfix]
    public static void MainMenuManager_Start_Postfix(MainMenuManager __instance)
    {
        EmojiLoader.LoadEmojis();
    }
}