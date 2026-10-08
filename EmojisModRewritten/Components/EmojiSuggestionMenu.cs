using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EmojisModRewritten.Components;

[RegisterInIl2Cpp]
public class EmojiSuggestionMenu(IntPtr ptr) : MonoBehaviour(ptr)
{
    public Il2CppReferenceField<Button> SuggestionPrefab;
    public List<Button> Buttons = new();
    private RectTransform _rectTransform;
    public void Refresh()
    {
        foreach (var button in Buttons)
        {
            button.gameObject.Destroy();
        }

        Buttons = new();

        string text = HudManager.Instance.Chat.freeChatField.Text;
        if (text.IsNullOrWhiteSpace() || text.Count(x => x.ToString() == ":") % 2 == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        int searchStartIndex = text.ToList().FindLastIndex(x => x.ToString() == ":");
        string searchTerm = text.Substring(searchStartIndex).Replace(":", "");
        foreach (var spr in EmojiLoader.SpriteAsset.spriteCharacterTable.ToArray().Where(x => x.name.Contains(searchTerm)).Take(6))
        {
            var btn = Instantiate(SuggestionPrefab.Value, transform);
            Buttons.Add(btn);
            btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = spr.name;
            btn.transform.GetChild(1).GetComponent<Image>().sprite = spr.glyph.TryCast<TMP_SpriteGlyph>()?.sprite;
            btn.onClick.AddListener(new Action(() =>
            {
                var targetTextBox = HudManager.Instance.Chat.freeChatField.textArea;
                targetTextBox.SetText(targetTextBox.text.Replace(":" + searchTerm, "").Trim() + $" :{spr.name}:");
                gameObject.SetActive(false);
            }));
        }
        if (Buttons.Count == 0) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        HudManager.Instance.Chat.freeChatField.OnChangedEvent += new Action(Refresh);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (!HudManager.InstanceExists)
        {
            return;
        }
        HudManager.Instance.StartCoroutine(Effects.Bloop(0, _rectTransform.transform, 0.7375f));
    }

    private void Update()
    {
        var inputField = HudManager.Instance.Chat.freeChatField;
        _rectTransform.sizeDelta = new Vector2(inputField.background.sprite.texture.width, inputField.background.sprite.texture.width / 2f);
        _rectTransform.position = HudManager.Instance.UICamera.WorldToScreenPoint(inputField.transform.position) + new Vector3(0, 50, 0);
    }
}