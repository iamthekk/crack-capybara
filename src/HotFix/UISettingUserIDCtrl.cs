using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UISettingUserIDCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_loginDataModule = GameApp.Data.GetDataModule<LoginDataModule>(105);
			this.m_txt.text = Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_uid_2") + this.m_loginDataModule.userId.ToString();
			this.m_bt.m_onClick = new Action(this.OnClickBt);
		}

		protected override void OnDeInit()
		{
		}

		private void OnClickBt()
		{
			GUIUtility.systemCopyBuffer = this.m_loginDataModule.userId.ToString();
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("30017"));
		}

		public CustomButton m_bt;

		public CustomText m_txt;

		private LoginDataModule m_loginDataModule;
	}
}
