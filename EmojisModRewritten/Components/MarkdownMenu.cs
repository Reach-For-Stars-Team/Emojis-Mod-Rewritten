using System;
using System.Collections;
using System.Collections.Generic;
using Reactor.Utilities.Attributes;
using UnityEngine;

namespace EmojisModRewritten.Components;

[RegisterInIl2Cpp]
public class MarkdownMenu(IntPtr ptr) : MonoBehaviour(ptr)
{
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas  = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
    if (!HudManager.InstanceExists) return;

    var field = HudManager.Instance.Chat.freeChatField;
    var cam = HudManager.Instance.UICamera;
    var bounds = field.background.bounds; // world-space bounds

    Vector2 min = cam.WorldToScreenPoint(bounds.min);
    Vector2 max = cam.WorldToScreenPoint(bounds.max);
    float fieldW = max.x - min.x;
    float fieldH = max.y - min.y;

    const float widthFraction  = 0.97f; // toolbar width relative to the field
    const float heightFraction = 0.75f; // toolbar height relative to the field
    const float gapFraction    = 1.4f;  // distance above the field's top edge

    float s = _canvas.scaleFactor;

    _rectTransform.sizeDelta = new Vector2(fieldW * widthFraction, fieldH * heightFraction) / s;
    _rectTransform.position = new Vector3(
        min.x + fieldW * (1f - widthFraction) * 0.5f,
        max.y + fieldH * gapFraction,
        0f);
    }

    public void OnClickBold()
    {
        var inputField = HudManager.Instance.Chat.freeChatField;
        string text = inputField.textArea.text;
        
        if (text.EndsWith(" **"))
        {
            text = inputField.textArea.text.TrimEnd(" **").ToString();
        }
        else text = inputField.textArea.text.Trim() + " **";
        
        inputField.textArea.SetText(text);
    }

    public void OnClickItalic()
    {
        var inputField = HudManager.Instance.Chat.freeChatField;
        if (inputField.textArea.text.EndsWith("*"))
        {
            inputField.textArea.text = inputField.textArea.text.TrimEnd("*").ToString();
            return;
        }
        inputField.textArea.SetText(inputField.textArea.text.Trim() + " *");
    }

    public void OnClickUnderline()
    {
        var inputField = HudManager.Instance.Chat.freeChatField;
        if (inputField.textArea.text.EndsWith("__"))
        {
            inputField.textArea.text = inputField.textArea.text.TrimEnd("__").ToString();
            return;
        }
        inputField.textArea.SetText(inputField.textArea.text.Trim() + " __");
    }

    public void OnClickStrike()
    {
        var inputField = HudManager.Instance.Chat.freeChatField;
        if (inputField.textArea.text.EndsWith("~~"))
        {
            inputField.textArea.text = inputField.textArea.text.TrimEnd("~~").ToString();
            return;
        }
        inputField.textArea.SetText(inputField.textArea.text.Trim() + " ~~");
    }
}
