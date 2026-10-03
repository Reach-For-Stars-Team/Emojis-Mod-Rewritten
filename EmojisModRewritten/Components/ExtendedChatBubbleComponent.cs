using System;
using Reactor.Utilities.Attributes;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EmojisModRewritten.Components;

[RegisterInIl2Cpp]
public class ExtendedChatBubbleComponent(IntPtr ptr) : MonoBehaviour(ptr)
{
    private ChatBubble _chatBubble;
    private TextMeshPro _timestampText;
    public void Initialize(ChatBubble chatBubble)
    {
        _chatBubble = chatBubble;
        _timestampText = Object.Instantiate(chatBubble.NameText, chatBubble.transform);
        _timestampText.color = new Color32(255, 255, 255, 150);
        _timestampText.alignment = TextAlignmentOptions.TopLeft;
    }

    public void SetTimeStamp()
    {
        _timestampText.text = DateTime.Now.ToLocalTime().ToShortTimeString();
        if (_chatBubble.playerInfo.AmOwner)
        {
            _timestampText.transform.localPosition = new Vector3(4.25f, 0.358f, 0);
        }
        else
        {
            _timestampText.transform.localPosition = new Vector3(4.35f, 0.358f, 0);
        }
    }
}