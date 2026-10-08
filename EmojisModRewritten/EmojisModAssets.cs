using System.Reflection;
using EmojisModRewritten.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace EmojisModRewritten;

public static class EmojisModAssets
{
    public static AssetBundle Bundle;
    public static GameObject EmojisMenuPrefab;
    public static GameObject MarkdownMenuPrefab;
    public static GameObject EmojiSuggestionPrefab;
    public static Sprite EmojiButtonSprite;
    public static Sprite EmojiButtonSpriteActive;
    public static void Initialize()
    {
        Bundle = AssetBundleManager.Load("emojibundle");
        EmojisMenuPrefab = Bundle.LoadAsset<GameObject>("EmojiSelector").DontUnload().DontDestroy();
        MarkdownMenuPrefab = Bundle.LoadAsset<GameObject>("MarkdownMenu").DontUnload().DontDestroy();
        EmojiSuggestionPrefab = Bundle.LoadAsset<GameObject>("EmojiSuggestionsMenu").DontUnload().DontDestroy();
        EmojiButtonSprite = SpriteTools.LoadSpriteFromPath("EmojisModRewritten.Resources.EmojisButton.png", Assembly.GetAssembly(
            typeof(EmojisModAssets)), 256).DontDestroy().DontUnload();
        EmojiButtonSpriteActive = SpriteTools.LoadSpriteFromPath("EmojisModRewritten.Resources.EmojisButton_Hover.png", Assembly.GetAssembly(
            typeof(EmojisModAssets)), 256).DontDestroy().DontUnload();
    }
}