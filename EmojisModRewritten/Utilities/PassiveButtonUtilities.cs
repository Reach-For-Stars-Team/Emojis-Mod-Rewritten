using System;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EmojisModRewritten.Utilities;

public static class PassiveButtonUtilities
{
    public static PassiveButton CreatePassiveButton(GameObject go, GameObject active, GameObject inactive, Action listener)
    {
        var passive = go.AddComponent<PassiveButton>();

        passive.activeSprites = active;

        passive.inactiveSprites = inactive;

        passive.ClickMask = go.AddComponent<BoxCollider2D>();
        passive.Colliders = new Il2CppReferenceArray<Collider2D>([passive.ClickMask]);

        passive.OnMouseOver = new();
        passive.OnMouseOut = new();

        passive.OnClick = new Button.ButtonClickedEvent();
        passive.OnClick.AddListener(listener);

        return passive;
    }

    /// <summary>
    /// Creates A PassiveButton.
    /// </summary>
    /// <param name="ObjectName">The name of the new <see cref="GameObject"/>.</param>
    /// <param name="active">The sprite which will be shown on hover.</param>
    /// <param name="inactive">The sprite which will be shown normally.</param>
    /// <param name="ColliderSize">The size of the <see cref="BoxCollider2D"/>.</param>
    /// <param name="listener">the <see cref="Action"/> which gets called upon clicking.</param>
    /// <param name="Layer">The GameObject's Layer, is set to the UI layer by default.</param>
    /// <param name="HasText">Determines if the PassiveButton has text</param>
    /// <param name="Text">The ButtonText's text</param>
    /// <returns></returns>
    public static PassiveButton CreatePassiveButton(string ObjectName, Sprite active, Sprite inactive,
        Vector2 ColliderSize, Action listener, string Layer = "UI", bool HasText = false, string Text = "")

    {
        GameObject go = new GameObject(ObjectName);
        go.layer = LayerMask.NameToLayer(Layer);
        var passive = go.AddComponent<PassiveButton>();

        passive.activeSprites = new GameObject("Active");
        passive.activeSprites.transform.SetParent(go.transform);
        passive.activeSprites.transform.localScale = Vector3.one;
        passive.activeSprites.transform.localPosition = Vector3.zero;
        passive.activeSprites.AddComponent<SpriteRenderer>().sprite = active;
        passive.activeSprites.layer = LayerMask.NameToLayer(Layer);

        passive.inactiveSprites = new GameObject("Inactive");
        passive.inactiveSprites.transform.SetParent(go.transform);
        passive.inactiveSprites.transform.localScale = Vector3.one;
        passive.inactiveSprites.transform.localPosition = Vector3.zero;
        passive.inactiveSprites.AddComponent<SpriteRenderer>().sprite = inactive;
        passive.inactiveSprites.layer = LayerMask.NameToLayer(Layer);
        var col = go.AddComponent<BoxCollider2D>();
        col.size = ColliderSize;
        col.isTrigger = true;
        passive.ClickMask = col;
        passive.Colliders = new Il2CppReferenceArray<Collider2D>([passive.ClickMask]);

        passive.OnMouseOver = new();
        passive.OnMouseOut = new();

        passive.OnClick = new Button.ButtonClickedEvent();
        passive.OnClick.AddListener(listener);

        if (HasText)
        {
            var text = new GameObject("Text").AddComponent<TextMeshPro>();
            text.transform.SetParent(go.transform);
            text.transform.localScale = Vector3.one;
            text.transform.localPosition = Vector3.zero;
            text.text = Text;
            passive.buttonText = text;
        }
        return passive;
    }
}