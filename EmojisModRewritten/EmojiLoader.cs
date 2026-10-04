using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AmongUs.GameOptions;
using BepInEx;
using BepInEx.Logging;
using EmojisModRewritten.Utilities;
using Reactor.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace EmojisModRewritten;

public static class EmojiLoader
{
    private static string _emojisPath = OperatingSystem.IsAndroid() || OperatingSystem.IsIOS() ? Environment.GetEnvironmentVariable("STAR_DATA_PATH") : Paths.GameRootPath;
    private static ManualLogSource _log = BepInEx.Logging.Logger.CreateLogSource("EmojiLoader");
    public static TMP_SpriteAsset SpriteAsset;
    public static void LoadEmojis()
    {
        Coroutines.Start(CoLoadEmojis());
    }

    private static IEnumerator CoLoadEmojis()
    {
        var textures = new List<Texture2D>();
        LoadLocalEmojis(textures);
        SpriteAsset = TmpSpriteAssetBuilder.CreateTMPSpriteAsset(textures, "Emojis_Asset_TMP");
        yield break;
    }

    private static void LoadLocalEmojis(List<Texture2D> textures)
    {
        var emojisDirectory = Path.Combine(_emojisPath, "Emojis");
        if (!Directory.Exists(emojisDirectory))
        {
            Directory.CreateDirectory(emojisDirectory);
        }
        
        foreach (var file in Directory.GetFiles(emojisDirectory))
        {
            if (file.EndsWith(".png"))
            {
                if (Path.GetFileNameWithoutExtension(file).StartsWith("playershader_"))
                {
                    LoadPlayerShaderLocalEmoji(file, textures);
                }
                else
                {
                    textures.Add(LoadLocalEmoji(file));
                }
            }
        }
    }

    private static Texture2D LoadLocalEmoji(string file)
    {
        _log.LogInfo($"Loading emoji from {file}");
        var tex = SpriteTools.LoadExternalTexture(file);
        if (tex == null)
        {
            _log.LogError($"Failed to load emoji from {file}");
            return null;
        }
        tex.name = Path.GetFileNameWithoutExtension(file).ToLower();
        _log.LogInfo($"Loaded emoji: {tex.name}");
        return tex;
    }
    
    private static void LoadPlayerShaderLocalEmoji(string file, List<Texture2D> textures)
    {
        _log.LogInfo($"Loading playershader emoji from {file}");
        var tex = SpriteTools.LoadExternalTexture(file);
        if (tex == null)
        {
            _log.LogError($"Failed to load emoji from {file}");
            return;
        }

        tex.name = Path.GetFileNameWithoutExtension(file).ToLower();
        tex.name = tex.name.Replace("playershader_", "");
        var mat = new Material(HatManager.Instance.PlayerMaterial);
        var prevActive = RenderTexture.active;

        try
        {
            for (int i = 0; i < Palette.PlayerColors.Count; i++)
            {
                PlayerMaterial.SetColors(i, mat);

                var rt = RenderTexture.GetTemporary(
                    tex.width, tex.height, 0,
                    RenderTextureFormat.ARGB32,
                    tex.isDataSRGB ? RenderTextureReadWrite.sRGB : RenderTextureReadWrite.Linear);

                try
                {
                    RenderTexture.active = rt;
                    GL.Clear(true, true, Color.clear);

                    Graphics.Blit(tex, rt, mat);

                    RenderTexture.active = rt;

                    var shadedTex = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
                    shadedTex.name = tex.name + "_" +
                                     TranslationController.Instance.GetString(Palette.ColorNames[i]);
                    shadedTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                    shadedTex.Apply(false, false);

                    textures.Add(shadedTex);
                }
                finally
                {
                    RenderTexture.active = prevActive;
                    RenderTexture.ReleaseTemporary(rt);
                }
            }
        }
        finally
        {
            RenderTexture.active = prevActive;
            Object.Destroy(mat);
        }
    }
}