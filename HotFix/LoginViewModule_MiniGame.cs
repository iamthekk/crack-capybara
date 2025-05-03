using System;
using Framework;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.Component;
using Framework.Logic.Platform;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class LoginViewModule_MiniGame : CustomBehaviour
	{
		protected override void OnInit()
		{
			Debug.Log("#[Login] LoginViewModule_MiniGame OnInit");
			this.m_loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			Debug.Log("#[Login] LoginViewModule_MiniGame OnInit1");
			if (!GameApp.SDK.WebGameAPI.GetOpenId().IsEmpty() && GameApp.SDK.WebGameAPI.LoginState == 2)
			{
				this.m_isCanLoading = true;
				string openId = GameApp.SDK.WebGameAPI.GetOpenId();
				string text = GameApp.SDK.WebGameAPI.GetOpenId();
				if (string.IsNullOrEmpty(text) || text == "n/a")
				{
					text = PlatformHelper.GetUUID();
					Debug.Log("[Login]  LoginViewModule_MiniGame m_deviceId is null,getdefault:m_deviceId:" + text);
				}
				EventArgsStartLogin instance = Singleton<EventArgsStartLogin>.Instance;
				instance.SetData(openId, text, "account2");
				Debug.Log("[Login] dispath CC_LoginViewModule_StartPullServerInfo memorylogin m_account:" + openId + " m_deviceId:" + text);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_LoginViewModule_StartPullServerInfo, instance);
				return;
			}
		}

		public void OnClickCleanCache()
		{
			GameApp.SDK.CleanCache();
			GameApp.SDK.RestartProgram();
		}

		protected override void OnDeInit()
		{
			throw new NotImplementedException();
		}

		private LoginDataModule m_loginDataModule;

		[Label]
		public bool m_isSDKLoginFinished;

		[Label]
		public bool m_isCanLoading;

		public Text m_namePlaceholder;
	}
}
