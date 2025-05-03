using System;
using Framework;
using Framework.Logic;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class LoginViewModule_SDKLogin : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_isSDKLoginFinished = false;
			this.m_isCanLoading = false;
			this.m_loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.m_isSDKLoginFinished = GameApp.SDK.Login.IsInitSuccess;
			this.m_loadTxtInfo = Singleton<LanguageManager>.Instance.GetInfoByID("130");
			this.m_loadingTxt.text = this.m_loadTxtInfo;
			this.m_time = 0f;
			GameApp.SDK.Login.OnSDKLogin(new Action<string>(this.OnSDKLoginCallBack));
		}

		private void OnSDKLoginCallBack(string deviceID)
		{
			if (string.IsNullOrEmpty(deviceID))
			{
				this.m_loadTxtInfo = "登录失败";
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (!this.m_isSDKLoginFinished && GameApp.SDK.Login.IsInitSuccess)
			{
				this.m_isSDKLoginFinished = GameApp.SDK.Login.IsInitSuccess;
			}
			if (this.m_isSDKLoginFinished && !this.m_isCanLoading)
			{
				string text = GameApp.SDK.Login.m_account;
				if (string.IsNullOrEmpty(text))
				{
					text = Utility.PlayerPrefs.GetString("ACCOUNT_Key", GameApp.SDK.Login.m_account);
				}
				string text2 = GameApp.SDK.Login.m_deviceId;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = Utility.PlayerPrefs.GetString("DEVICEID_Key", GameApp.SDK.Login.m_deviceId);
				}
				string text3 = GameApp.SDK.Login.m_account2;
				if (string.IsNullOrEmpty(text3))
				{
					text3 = Utility.PlayerPrefs.GetString("ACCOUNT2_Key", GameApp.SDK.Login.m_account2);
				}
				EventArgsStartLogin instance = Singleton<EventArgsStartLogin>.Instance;
				instance.SetData(text, text2, text3);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_LoginViewModule_StartPullServerInfo, instance);
				this.m_isCanLoading = true;
			}
		}

		protected override void OnDeInit()
		{
			this.m_loginDataModule = null;
			this.m_time = 0f;
			this.m_pointCount = 1;
			this.m_isSDKLoginFinished = false;
			this.m_isCanLoading = false;
		}

		private void OnUpdateText()
		{
			if (Time.time - this.m_time >= 0.25f)
			{
				this.m_pointCount++;
				if (this.m_pointCount > 3)
				{
					this.m_pointCount = 1;
				}
				string text = "";
				for (int i = 0; i < this.m_pointCount; i++)
				{
					text += ".";
				}
				this.m_loadingTxt.text = string.Format(this.m_loadTxtInfo, text);
				this.m_time = Time.time;
			}
		}

		public CustomText m_loadingTxt;

		[Label]
		public bool m_isSDKLoginFinished;

		[Label]
		public bool m_isCanLoading;

		[Label]
		public string m_loadTxtInfo = string.Empty;

		[Label]
		public float m_time;

		[Label]
		public int m_pointCount;

		private LoginDataModule m_loginDataModule;
	}
}
