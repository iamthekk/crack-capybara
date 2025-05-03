using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class MainCityGoldLevelUpLevelItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void RefreshUI(int from, int to, bool isFull)
		{
			if (this.m_fromTxt != null)
			{
				this.m_fromTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6517, new object[] { from });
			}
			if (this.m_arrowObj != null)
			{
				this.m_arrowObj.gameObject.SetActive(!isFull);
			}
			if (this.m_toTxt != null)
			{
				this.m_toTxt.gameObject.SetActive(!isFull);
				this.m_toTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6517, new object[] { to });
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

		public CustomText m_fromTxt;

		public CustomText m_toTxt;

		public GameObject m_arrowObj;

		public ParticleSystem m_particleSystem;
	}
}
