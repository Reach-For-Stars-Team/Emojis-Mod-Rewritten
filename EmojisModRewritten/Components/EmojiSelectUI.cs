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
    private RectTransform _rectTransform;
    private Canvas _canvas;

    public void HandleSearch(string prompt)
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(button.gameObject.name.ToLower().Contains(prompt.ToLower()));
        }
    }

    private void Start()
    {
        _rectTransform = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        _canvas = transform.GetChild(1).GetComponent<Canvas>();
        commonlyUsedEmojisParent.Value.parent.gameObject.SetActive(false);
        foreach (var character in EmojiLoader.SpriteAsset.m_SpriteCharacterTable)
        {
            if (character.name == "empty") continue;
            var button = Instantiate(emojiButtonPrefab.Value, emojisParent.Value);
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(new System.Action(() => SelectEmoji(character.name)));
            var preview = button.transform.GetChild(1).GetComponent<Image>();
            preview.sprite = character.glyph.TryCast<TMP_SpriteGlyph>()?.sprite;
            button.gameObject.name = character.name;
            buttons.Add(button);
        }
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (!HudManager.InstanceExists)
        {
            return;
        }
        HudManager.Instance.StartCoroutine(Effects.Bloop(0, _rectTransform.transform, 0.8f));
    }
    
    private void Update()
    {
        if (!HudManager.InstanceExists) return;

        var field = HudManager.Instance.Chat.freeChatField;
        var cam = HudManager.Instance.UICamera;
        var bounds = field.background.bounds;

        Vector2 min = cam.WorldToScreenPoint(bounds.min);
        Vector2 max = cam.WorldToScreenPoint(bounds.max);
        float fieldW = max.x - min.x;
        float fieldH = max.y - min.y;

        const float widthFraction  = 1f;
        const float heightFraction = 6f;
        const float gapFraction = 0.25f;

        float s = _canvas.scaleFactor;
        
        _rectTransform.pivot = Vector2.zero;
        _rectTransform.sizeDelta = new Vector2(fieldW * widthFraction, fieldH * heightFraction) / s;
        _rectTransform.position = new Vector3(
            min.x + fieldW * (1f - widthFraction) * 0.5f,
            max.y + fieldH * gapFraction,
            0f);
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