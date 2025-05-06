using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIEquipDetailsAttributeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public void SetData(string AttrTextTableID, long value)
		{
			this.m_currentKey = AttrTextTableID;
			Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(AttrTextTableID);
			if (elementById != null && this.m_icon != null)
			{
				this.m_icon.SetImage(GameApp.Table.GetAtlasPath(elementById.iconAtlasID), elementById.iconName);
			}
			if (this.m_valueTxt != null)
			{
				this.m_valueTxt.text = DxxTools.FormatNumber(value);
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

		public CustomImage m_icon;

		public CustomText m_valueTxt;

		public string m_currentKey = string.Empty;

		public ParticleSystem m_particleSystem;
	}
}
