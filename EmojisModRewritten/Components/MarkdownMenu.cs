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

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!HudManager.InstanceExists) return;
        
        var inputField = HudManager.Instance.Chat.freeChatField;
        _rectTransform.sizeDelta = new Vector2(inputField.background.sprite.texture.width - 75, 50);
        var chatScale = HudManager.Instance.Chat.chatScreen.transform.localScale;
        _rectTransform.localScale = new Vector3(0.7375f * chatScale.x, 0.7375f * chatScale.y, 0.7375f * chatScale.z);
        _rectTransform.position = HudManager.Instance.UICamera.WorldToScreenPoint(inputField.transform.position) + new Vector3(-35, 75, 0);
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