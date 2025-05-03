using System;
using UnityEngine;

namespace Framework.Logic.UI
{
	public class OutlineEx_Custom : OutlineEx
	{
		protected override float GetOutlineWidth()
		{
			return this.m_width;
		}

		protected override Vector4 GetMin()
		{
			return this.m_customMin;
		}

		protected override Vector4 GetMax()
		{
			return this.m_customMax;
		}

		public float m_width = 10f;

		public Vector4 m_customMin = new Vector4(0f, 0f, 0.5f, 0f);

		public Vector4 m_customMax = new Vector4(0.2f, 0.13f, 3f, 0f);
	}
}
