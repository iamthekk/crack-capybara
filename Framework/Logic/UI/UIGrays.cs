using System;
using Framework.Logic.AttributeExpansion;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class UIGrays : MonoBehaviour
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

		[ContextMenu("SetUIGray")]
		public void SetUIGray()
		{
			this.m_isGray = true;
			if (this.m_targets == null)
			{
				return;
			}
			for (int i = 0; i < this.m_targets.Length; i++)
			{
				Graphic graphic = this.m_targets[i];
				if (!(graphic == null) && graphic)
				{
					graphic.material = this.GetGrayMat();
					graphic.SetMaterialDirty();
				}
			}
		}

		[ContextMenu("Recovery")]
		public void Recovery()
		{
			this.m_isGray = false;
			if (this.m_targets == null)
			{
				return;
			}
			for (int i = 0; i < this.m_targets.Length; i++)
			{
				Graphic graphic = this.m_targets[i];
				if (graphic == null)
				{
					HLog.LogError(string.Format("UIGrays.m_targets index:{0} is null", i));
				}
				else
				{
					graphic.material = null;
					graphic.SetMaterialDirty();
				}
			}
		}

		[ContextMenu("Find Graphic")]
		public void FindGraphic()
		{
			this.m_targets = base.gameObject.GetComponentsInChildren<Graphic>();
		}

		[Header("Target Setting")]
		public Graphic[] m_targets;

		[Header("Materail Setting")]
		public Material m_grayMat;

		[Label]
		public bool m_isGray;
	}
}
