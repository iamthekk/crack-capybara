using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UISettingSoundCtrl : CustomBehaviour
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
			if (this.m_settingData.GetValue(SettingDataName.SoundEffect))
			{
				this.m_bt.SetSelect(true);
				this.m_btTxt.ChangeLanguageID("6");
				GameApp.Sound.SetSoundEffectOpen(true);
				return;
			}
			this.m_bt.SetSelect(false);
			this.m_btTxt.ChangeLanguageID("7");
			GameApp.Sound.SetSoundEffectOpen(false);
		}

		private void OnClickBt(CustomChooseButton button)
		{
			this.m_settingData.SetValue(SettingDataName.SoundEffect, !this.m_bt.IsSelected);
			this.OnRefresh();
		}

		public CustomChooseButton m_bt;

		public CustomLanguageText m_btTxt;

		public SettingDataModule m_settingData;
	}
}
