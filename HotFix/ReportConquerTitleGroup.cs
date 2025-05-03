using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class ReportConquerTitleGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
			this.m_data = null;
		}

		public void RefreshUI(SocialityInteractiveData data)
		{
			this.m_data = data;
			Sociality_Report elementById = GameApp.Table.GetManager().GetSociality_ReportModelInstance().GetElementById(this.m_data.m_id);
			if (this.m_icon != null)
			{
				this.m_icon.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.iconName);
			}
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.titleLanguage);
			}
			if (this.m_timeTxt != null)
			{
				this.m_timeTxt.text = Singleton<LanguageManager>.Instance.GetGoTime(this.m_data.m_duration);
			}
		}

		public CustomImage m_icon;

		public CustomText m_nameTxt;

		public CustomText m_timeTxt;

		public SocialityInteractiveData m_data;
	}
}
