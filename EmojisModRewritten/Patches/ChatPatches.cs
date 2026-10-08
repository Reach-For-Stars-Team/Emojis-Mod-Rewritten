using EmojisModRewritten.Components;
using EmojisModRewritten.Utilities;
using HarmonyLib;
using UnityEngine;

namespace EmojisModRewritten.Patches;

[HarmonyPatch]
public class ChatPatches
{
    [HarmonyPatch(typeof(ChatBubble), nameof(ChatBubble.SetText))]
    [HarmonyPrefix]
    public static void ChatBubble_SetText_Prefix(ChatBubble __instance, ref string chatText)
    {
        __instance.TextArea.fontSize *= 1;
    }
    
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Awake))]
    [HarmonyPostfix]
    public static void ChatController_Awake_Postfix(ChatController __instance)
    {
        __instance.freeChatField.textArea.allowAllCharacters = 
            __instance.freeChatField.textArea.AllowEmail = 
                __instance.freeChatField.textArea.AllowPaste = 
                    __instance.freeChatField.textArea.AllowSymbols
                        = true;
        var menu = Object.Instantiate(EmojisModAssets.EmojisMenuPrefab, __instance.chatScreen.transform);
        menu.gameObject.SetActive(false);
        var button = PassiveButtonUtilities.CreatePassiveButton("EmojiButton", EmojisModAssets.EmojiButtonSpriteActive, EmojisModAssets.EmojiButtonSprite, Vector2.one,
            () =>
            {
                menu.gameObject.SetActive(!menu.gameObject.activeSelf);
                __instance.chatScreen.transform.FindChild("CloseBackground").gameObject.SetActive(!menu.gameObject.activeSelf);
            });
        button.transform.SetParent(__instance.freeChatField.transform);
        button.transform.localPosition = new Vector3(1.75f, 0, -10);
        __instance.freeChatField.charCountText.gameObject.transform.localPosition = new Vector3(3.1f, 0.725f, 0);
        __instance.freeChatField.charCountText.fontSize = 1.4f;
        __instance.scroller.transform.localPosition = new Vector3(0, 0.25f, -1);
        __instance.sendRateMessageText.transform.localPosition = new Vector3(-3.25f, -1, -5);
        Object.Instantiate(EmojisModAssets.MarkdownMenuPrefab, __instance.chatScreen.transform);
        Object.Instantiate(EmojisModAssets.EmojiSuggestionPrefab, __instance.chatScreen.transform);
    }

    [HarmonyPatch(typeof(ChatBubble), nameof(ChatBubble.SetText))]
    [HarmonyPostfix]
    public static void ChatBubble_SetText_Postfix(ChatBubble __instance, ref string chatText)
    {
        if (EmojiLoader.SpriteAsset == null) return;
        __instance.TextArea.spriteAsset = __instance.TextArea.m_spriteAsset = EmojiLoader.SpriteAsset;
        __instance.TextArea.text = TextReplacementUtilities.ReformatForPlayerNames(chatText);
        __instance.TextArea.text = TextReplacementUtilities.ReformatForEmojis(__instance.TextArea.text);
        __instance.TextArea.text = TextReplacementUtilities.ReformatForMarkdown(__instance.TextArea.text);
    }

    [HarmonyPatch(typeof(ChatBubble), nameof(ChatBubble.SetCosmetics))]
    [HarmonyPostfix]
    public static void ChatBubble_SetCosmetics_Postfix(ChatBubble __instance, ref NetworkedPlayerInfo playerInfo)
    {
        if (!__instance.TryGetComponent(out ExtendedChatBubbleComponent bubble))
        {
            bubble = __instance.gameObject.AddComponent<ExtendedChatBubbleComponent>();
            bubble.Initialize(__instance);
        }
        bubble.SetTimeStamp();
    }
    
    [HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.IsCharAllowed))]
    [HarmonyPostfix]
    public static void TextBoxTMP_IsCharAllowed(TextBoxTMP __instance, ref char i, out bool __result)
    {
        __result = i.ToString() != "\b";
    }
}