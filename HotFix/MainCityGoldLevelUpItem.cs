using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class MainCityGoldLevelUpItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void RefreshUI(int itemDataID, long from, long to, bool isFull)
		{
			if (itemDataID > 0)
			{
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemDataID);
				if (this.m_icon != null)
				{
					this.m_icon.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
				}
			}
			if (this.m_fromTxt != null)
			{
				this.m_fromTxt.text = string.Format("{0}/h", from);
			}
			if (this.m_arrowObj != null)
			{
				this.m_arrowObj.gameObject.SetActive(!isFull);
			}
			if (this.m_toTxt != null)
			{
				this.m_toTxt.gameObject.SetActive(!isFull);
				this.m_toTxt.text = string.Format("{0}/h", to);
			}
			LayoutRebuilder.MarkLayoutForRebuild(base.rectTransform);
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

		public CustomText m_fromTxt;

		public CustomText m_toTxt;

		public GameObject m_arrowObj;

		public ParticleSystem m_particleSystem;
	}
}
