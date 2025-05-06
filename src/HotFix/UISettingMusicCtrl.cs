using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UISettingMusicCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_settingData = GameApp.Data.GetDataModule(DataName.SettingModule);
			this.OnRefresh();
			this.m_bt.OnClickButton = new Action<CustomChooseButton>(this.OnClickBt);
		}

		protected override void OnDeInit()
		{
		}

		public void OnRefresh()
		{
			if (this.m_settingData.GetValue(SettingDataName.Background))
			{
				this.m_bt.SetSelect(true);
				this.m_btTxt.ChangeLanguageID("4");
				GameApp.Sound.SetBackgroundVolume(0f);
				return;
			}
			this.m_bt.SetSelect(false);
			this.m_btTxt.ChangeLanguageID("5");
			GameApp.Sound.SetBackgroundVolume(-80f);
		}

		private void OnClickBt(CustomChooseButton button)
		{
			this.m_settingData.SetValue(SettingDataName.Background, !this.m_bt.IsSelected);
			this.OnRefresh();
		}

		public CustomChooseButton m_bt;

		public CustomLanguageText m_btTxt;

		private SettingDataModule m_settingData;
	}
}
