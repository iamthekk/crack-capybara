using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class UIGray : MonoBehaviour
	{
		private Material GetGrayMat()
		{
			if (this.m_grayMat == null)
			{
				Shader shader = Shader.Find("UI/UIGray");
				if (shader == null)
				{
					return null;
				}
				Material material = new Material(shader);
				this.m_grayMat = material;
			}
			return this.m_grayMat;
		}

		public void SetUIGray()
		{
			if (this.m_target == null)
			{
				return;
			}
			this.m_target.material = this.GetGrayMat();
			this.m_target.SetMaterialDirty();
		}

		public void Recovery()
		{
			if (this.m_target == null)
			{
				return;
			}
			this.m_target.material = this.m_defalutMat;
		}

		[Header("Target Setting")]
		public Graphic m_target;

		[Header("Materail Setting")]
		public Material m_grayMat;

		public Material m_defalutMat;
	}
}
