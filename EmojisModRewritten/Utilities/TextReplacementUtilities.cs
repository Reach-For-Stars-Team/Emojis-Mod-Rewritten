using System;
using UnityEngine;

namespace EmojisModRewritten.Utilities;

public static class TextReplacementUtilities
{
    public static string ReformatForEmojis(string text)
    {
        if (EmojiLoader.SpriteAsset == null) return text;
        var final = text;

        foreach (var emoji in EmojiLoader.SpriteAsset.spriteCharacterTable)
            if (final.Contains($":{emoji.name}:"))
                final = final.Replace($":{emoji.name}:", $"<sprite name={emoji.name}>");

        return final;
    }
    
    public static string ReformatForPlayerNames(string text)
    {
        var final = text;

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            string expectedName = p.Data.PlayerName;
            if (final.Contains(expectedName))
            {
                final = final.Replace(expectedName, $"<color=#{ColorUtility.ToHtmlStringRGB(p.Data.Color)}>{p.Data.PlayerName} <b>({p.Data.ColorName})</b></color>");
            }
        }

        return final;
    }
}