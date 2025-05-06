using System;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UISettingQualityCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_bt.m_onClick = new Action(this.OnClickBt);
			this.OnRefresh();
		}

		protected override void OnDeInit()
		{
		}

		public void OnRefresh()
		{
			int currentQuality = (int)Singleton<QualityManager>.Instance.GetCurrentQuality();
			this.m_btTxt.ChangeLanguageID((24 + currentQuality).ToString());
		}

		private void OnClickBt()
		{
			Singleton<QualityManager>.Instance.NextQuality();
			this.OnRefresh();
		}

		public CustomButton m_bt;

		public CustomLanguageText m_btTxt;
	}
}
