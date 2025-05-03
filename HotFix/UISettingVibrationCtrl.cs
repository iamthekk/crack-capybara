using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UISettingVibrationCtrl : CustomBehaviour
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
			if (this.m_settingData.GetValue(SettingDataName.Vibration))
			{
				this.m_bt.SetSelect(true);
				this.m_btTxt.ChangeLanguageID("6");
				DeviceVibration.SetOpen(true);
				return;
			}
			this.m_bt.SetSelect(false);
			this.m_btTxt.ChangeLanguageID("7");
			DeviceVibration.SetOpen(false);
		}

		private void OnClickBt(CustomChooseButton button)
		{
			this.m_settingData.SetValue(SettingDataName.Vibration, !this.m_bt.IsSelected);
			this.OnRefresh();
		}

		public CustomChooseButton m_bt;

		public CustomLanguageText m_btTxt;

		public SettingDataModule m_settingData;
	}
}
