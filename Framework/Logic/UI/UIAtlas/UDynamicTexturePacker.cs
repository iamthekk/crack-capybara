using System;
using UnityEngine;

namespace Framework.Logic.UI.UIAtlas
{
	public class UDynamicTexturePacker
	{
		public static UAtlasData DynamicTexturePacker(ref Texture2D texture, Texture2D[] textures, int padding, int maxSize, bool forceSquare)
		{
			Rect[] array = UTexturePacker.PackTextures(texture, textures, 4, 4, padding, maxSize, forceSquare);
			if (array != null)
			{
				UAtlasData uatlasData = ScriptableObject.CreateInstance<UAtlasData>();
				uatlasData.m_padding = padding;
				uatlasData.m_forceSquare = forceSquare;
				uatlasData.m_unityPacker = false;
				uatlasData.m_sprites = new USpriteData[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					uatlasData.m_sprites[i] = new USpriteData();
					uatlasData.m_sprites[i].m_name = textures[i].name;
					Rect rect;
					rect..ctor((float)Mathf.RoundToInt(array[i].x * (float)texture.width), (float)Mathf.RoundToInt(array[i].y * (float)texture.height), (float)Mathf.RoundToInt(array[i].width * (float)texture.width), (float)Mathf.RoundToInt(array[i].height * (float)texture.height));
					uatlasData.m_sprites[i].m_sprite = Sprite.Create(texture, rect, rect.center);
					uatlasData.m_sprites[i].m_rect = rect;
					uatlasData.m_sprites[i].m_pivot = uatlasData.m_sprites[i].m_sprite.pivot;
					uatlasData.m_sprites[i].m_uv = uatlasData.m_sprites[i].m_sprite.uv;
					uatlasData.m_sprites[i].m_sprite.name = textures[i].name;
				}
				return uatlasData;
			}
			return null;
		}
	}
}
