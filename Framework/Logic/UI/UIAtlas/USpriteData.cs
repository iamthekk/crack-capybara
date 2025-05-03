using System;
using UnityEngine;

namespace Framework.Logic.UI.UIAtlas
{
	[Serializable]
	public class USpriteData
	{
		public string m_name;

		public Rect m_rect;

		public Vector2 m_pivot;

		public Vector2[] m_uv;

		public Sprite m_sprite;

		public string m_sourceTextureGuid;
	}
}
