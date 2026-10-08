using System;
using System.Collections.Generic;
using System.Text;
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
            if (final.Contains(expectedName, StringComparison.CurrentCultureIgnoreCase))
            {
                final = final.Replace(expectedName,
                    $"<color=#{ColorUtility.ToHtmlStringRGB(p.Data.Color)}>{p.Data.PlayerName} <b>{p.Data.ColorName}</b></color>");
            }
        }

        return final;
    }

    public static readonly List<TextFormatter> TextFormatters =
    [
        // Longest identifiers first, so "**" is consumed before "*" can see it
        new TextFormatter("**", "<b>", "</b>"),
        new TextFormatter("__", "<u>", "</u>"),
        new TextFormatter("~~", "<s>", "</s>"),
        new TextFormatter("*", "<i>", "</i>")
    ];

    public static string ReformatForMarkdown(string text)
    {
        var result = text;
        foreach (var formatter in TextFormatters)
            result = ApplyFormatter(result, formatter);
        return result;
    }

    private static string ApplyFormatter(string text, TextFormatter f)
    {
        var sb = new StringBuilder(text.Length + 16);
        int pos = 0;

        while (true)
        {
            int open = text.IndexOf(f.Identifier, pos, StringComparison.Ordinal);
            if (open < 0) break;

            int contentStart = open + f.Identifier.Length;
            int close = text.IndexOf(f.Identifier, contentStart, StringComparison.Ordinal);
            if (close < 0) break;

            sb.Append(text, pos, open - pos)
                .Append(f.OpeningTag)
                .Append(text, contentStart, close - contentStart)
                .Append(f.ClosingTag);

            pos = close + f.Identifier.Length;
        }

        sb.Append(text, pos, text.Length - pos);
        return sb.ToString();
    }
}

public class TextFormatter(string identifier, string openingTag, string closingTag)
{
    public string Identifier = identifier;
    public string OpeningTag = openingTag;
    public string ClosingTag = closingTag;
}