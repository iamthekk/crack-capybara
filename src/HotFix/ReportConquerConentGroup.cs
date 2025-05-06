using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine.UI;

namespace HotFix
{
	public class ReportConquerConentGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void RefreshUI(SocialityInteractiveData data)
		{
			this.m_data = data;
			Sociality_Report elementById = GameApp.Table.GetManager().GetSociality_ReportModelInstance().GetElementById(this.m_data.m_id);
			if (this.m_contentTxt != null)
			{
				Text contentTxt = this.m_contentTxt;
				LanguageManager instance = Singleton<LanguageManager>.Instance;
				string contentLanguage = elementById.contentLanguage;
				object[] array = DxxTools.GetParameterForSocialityReport(this.m_data.m_id, this.m_data.m_params).ToArray();
				contentTxt.text = instance.GetInfoByID(contentLanguage, array);
			}
			if (this.m_timeTxt != null)
			{
				this.m_timeTxt.text = Singleton<LanguageManager>.Instance.GetAllTime(this.m_data.m_time);
			}
		}

		public CustomText m_contentTxt;

		public CustomText m_timeTxt;

		private SocialityInteractiveData m_data;
	}
}
