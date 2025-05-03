using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIRelicDetailsAttributeNode : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
			this.m_data = null;
		}

		public void RefreshData(UIRelicDetailsAttributeGroup.NodeData data)
		{
			this.m_data = data;
			if (data == null)
			{
				return;
			}
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = data.m_name;
			}
			if (this.m_fromTxt != null)
			{
				this.m_fromTxt.text = data.m_from;
			}
			if (this.m_toTxt != null)
			{
				this.m_toTxt.text = data.m_to;
			}
		}

		public void PlayParticleSystem()
		{
			if (this.m_particleSystem == null)
			{
				return;
			}
			this.m_particleSystem.Play();
		}

		public RectTransform m_root;

		public CustomText m_nameTxt;

		public CustomText m_fromTxt;

		public CustomText m_toTxt;

		public ParticleSystem m_particleSystem;

		public UIRelicDetailsAttributeGroup.NodeData m_data;
	}
}
