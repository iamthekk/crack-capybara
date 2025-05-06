using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.UI.UIAtlas
{
	public class UAtlasData : ScriptableObject
	{
		private void OnEnable()
		{
			if (this.m_spriteDataDic == null)
			{
				if (this.m_sprites == null)
				{
					return;
				}
				this.m_spriteDataDic = new Dictionary<string, USpriteData>();
				for (int i = 0; i < this.m_sprites.Length; i++)
				{
					this.m_spriteDataDic[this.m_sprites[i].m_name] = this.m_sprites[i];
				}
			}
		}

		private void OnDestroy()
		{
			this.m_spriteDataDic.Clear();
		}

		public USpriteData GetSpriteDataByName(string name)
		{
			USpriteData uspriteData = null;
			if (this.m_spriteDataDic != null)
			{
				this.m_spriteDataDic.TryGetValue(name, out uspriteData);
			}
			else
			{
				for (int i = 0; i < this.m_sprites.Length; i++)
				{
					if (this.m_sprites[i].m_name == name)
					{
						uspriteData = this.m_sprites[i];
						break;
					}
				}
			}
			return uspriteData;
		}

		public Sprite GetSpriteByName(string spriteName)
		{
			USpriteData spriteDataByName = this.GetSpriteDataByName(spriteName);
			if (spriteDataByName != null)
			{
				return spriteDataByName.m_sprite;
			}
			return null;
		}

		public Sprite GetSpriteByNameOrRect(string spriteName)
		{
			USpriteData spriteDataByName = this.GetSpriteDataByName(spriteName);
			if (spriteDataByName != null)
			{
				if (spriteDataByName.m_sprite != null)
				{
					return spriteDataByName.m_sprite;
				}
				if (this.m_texture != null)
				{
					return Sprite.Create(this.m_texture, spriteDataByName.m_rect, spriteDataByName.m_rect.center);
				}
			}
			return null;
		}

		public Texture2D GetSpriteTextureByUISpriteData(USpriteData spriteData)
		{
			if (spriteData != null && spriteData.m_sprite && this.m_texture != null)
			{
				return this.GetSpriteTextureBySprite(spriteData.m_sprite);
			}
			return null;
		}

		public Texture2D GetSpriteTextureBySprite(Sprite sprite)
		{
			if (sprite != null && this.m_texture != null)
			{
				int num = Mathf.RoundToInt(sprite.rect.x);
				int num2 = Mathf.RoundToInt(sprite.rect.y);
				int num3 = Mathf.RoundToInt(sprite.rect.width);
				int num4 = Mathf.RoundToInt(sprite.rect.height);
				Texture2D texture2D = new Texture2D(num3, num4);
				Color[] pixels = this.m_texture.GetPixels(num, num2, num3, num4);
				texture2D.SetPixels(pixels);
				texture2D.name = sprite.name;
				texture2D.Apply();
				return texture2D;
			}
			return null;
		}

		public Texture2D GetSpriteTextureByName(string spriteName)
		{
			USpriteData spriteDataByName = this.GetSpriteDataByName(spriteName);
			if (spriteDataByName != null && this.m_texture != null)
			{
				return this.GetSpriteTextureByUISpriteData(spriteDataByName);
			}
			return null;
		}

		public int m_padding = 2;

		public bool m_unityPacker = true;

		public bool m_forceSquare = true;

		public bool m_texturePacker;

		public Texture2D m_texture;

		public Material m_material;

		public USpriteData[] m_sprites;

		private Dictionary<string, USpriteData> m_spriteDataDic;
	}
}
