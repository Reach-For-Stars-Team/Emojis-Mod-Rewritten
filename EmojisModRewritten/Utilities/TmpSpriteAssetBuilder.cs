using System.Collections.Generic;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

namespace EmojisModRewritten.Utilities;

public static class TmpSpriteAssetBuilder
{
    private static Shader _spriteShader;

    public static TMP_SpriteAsset CreateTMPSpriteAsset(IList<Texture2D> textures, string assetName,
        float scale = 1f, int padding = 2, int maxAtlasSize = 4096)
    {
        var atlas = new Texture2D(2, 2, TextureFormat.RGBA32, false)
        {
            name = assetName + " Atlas",
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp,
        };
        var input = new Il2CppReferenceArray<Texture2D>(textures.Count);
        for (int i = 0; i < textures.Count; i++) input[i] = textures[i];
        var uvRects = atlas.PackTextures(input, padding, maxAtlasSize);
        atlas.Apply(false, false);
        atlas = atlas.DontUnload().DontDestroy();

        _spriteShader ??= Shader.Find("TextMeshPro/Sprite");
        var material = new Material(_spriteShader) { name = assetName + " Material" };
        material.SetTexture(ShaderUtilities.ID_MainTex, atlas);
        material.SetFloat(ShaderUtilities.ID_StencilComp, 0);
        material.SetFloat(ShaderUtilities.ID_StencilID, 0);
        material.SetFloat(ShaderUtilities.ID_StencilOp, 0);
        material.SetFloat(ShaderUtilities.ID_StencilWriteMask, 255);
        material.SetFloat(ShaderUtilities.ID_StencilReadMask, 255);
        material.SetFloat(ShaderUtilities.ShaderTag_CullMode, 0);
        material.mainTexture = atlas;

        var asset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
        asset.name = assetName;
        asset.spriteSheet = atlas;
        asset.material = material;
        asset.fallbackSpriteAssets = new();
        asset.spriteInfoList = new Il2CppSystem.Collections.Generic.List<TMP_Sprite>();
        asset.spriteGlyphTable = new Il2CppSystem.Collections.Generic.List<TMP_SpriteGlyph>();
        asset.spriteCharacterTable = new Il2CppSystem.Collections.Generic.List<TMP_SpriteCharacter>();

        for (int i = 0; i < textures.Count; i++)
        {
            var uv = uvRects[i];
            int x = Mathf.RoundToInt(uv.x * atlas.width);
            int y = Mathf.RoundToInt(uv.y * atlas.height);
            int w = Mathf.RoundToInt(uv.width * atlas.width);
            int h = Mathf.RoundToInt(uv.height * atlas.height);
            string name = textures[i].name;

            float xOffset = -(w / 3f) + (scale - 1) * (w / 2f);
            float yOffset = h / 1.25f;

            var tex = textures[i];
            var glyph = new TMP_SpriteGlyph
            {
                index = (uint)i,
                metrics = new GlyphMetrics(w, h, xOffset, yOffset, w),
                glyphRect = new GlyphRect(x, y, w, h),
                scale = 1.8f,
                atlasIndex = i,
                sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f)),
            };
            asset.spriteGlyphTable.Add(glyph);

            var character = new TMP_SpriteCharacter(0xFFFE, glyph)
            {
                name = name,
                scale = scale,
                glyphIndex = (uint)i,
            };
            asset.spriteCharacterTable.Add(character);
            asset.m_FaceInfo.scale = asset.m_FaceInfo.m_PointSize = 1;
        }

        asset = asset.DontUnload().DontDestroy();
        asset.UpdateLookupTables();

        return asset;
    }
}