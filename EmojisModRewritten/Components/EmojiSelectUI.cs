using System;
using System.Collections.Generic;
using System.Linq;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EmojisModRewritten.Components;

[RegisterInIl2Cpp]
public class EmojiSelectMenu(IntPtr ptr) : MonoBehaviour(ptr)
{
    public Il2CppReferenceField<Transform> commonlyUsedEmojisParent;
    public Il2CppReferenceField<Transform> emojisParent;
    public Il2CppReferenceField<Button> emojiButtonPrefab;
    public List<Button> buttons = new List<Button>();
    public void HandleSearch(string prompt)
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(button.gameObject.name.ToLower().Contains(prompt.ToLower()));
        }
    }

    private void Start()
    {
        commonlyUsedEmojisParent.Value.parent.gameObject.SetActive(false);
        foreach (var character in EmojiLoader.SpriteAsset.m_SpriteCharacterTable)
        {
            var button = Instantiate(emojiButtonPrefab.Value, emojisParent.Value);
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(new System.Action(() => SelectEmoji(character.name)));
            var preview = button.transform.GetChild(1).GetComponent<Image>();
            preview.sprite = character.glyph.TryCast<TMP_SpriteGlyph>()?.sprite;
            button.gameObject.name = character.name;
            buttons.Add(button);
        }
    }

    private void OnEnable()
    {
        if (!HudManager.InstanceExists)
        {
            return;
        }

        var inputField = HudManager.Instance.Chat.freeChatField;
        var rectTransform = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(inputField.background.sprite.texture.width, inputField.background.sprite.texture.width / 2f);
        rectTransform.position = HudManager.Instance.UICamera.WorldToScreenPoint(inputField.transform.position) - new Vector3(0, 80, 0);
        HudManager.Instance.StartCoroutine(Effects.Bloop(0, rectTransform.transform, 0.8f));
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void SelectEmoji(string name)
    {
        if (!HudManager.InstanceExists) return;
        
        HudManager.Instance.Chat.freeChatField.textArea.text = HudManager.Instance.Chat.freeChatField.textArea.text.Trim();
        HudManager.Instance.Chat.freeChatField.textArea.SetText(HudManager.Instance.Chat.freeChatField.textArea.text + $" :{name}:");
    }
}