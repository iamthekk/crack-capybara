using System;
using Framework;
using Framework.Logic;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.Component;
using Framework.Logic.Platform;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class LoginViewModule_CustomLogin : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_isClickBt = false;
			this.m_isCanLoading = false;
			this.m_loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.m_loadTxtInfo = Singleton<LanguageManager>.Instance.GetInfoByID("130");
			this.m_account = Utility.PlayerPrefs.GetString("ACCOUNT_Key", Utility.Math.Random(100000, 999999).ToString());
			this.m_deviceId = Utility.PlayerPrefs.GetString("DEVICEID_Key", PlatformHelper.GetUUID());
			this.m_account2 = Utility.PlayerPrefs.GetString("ACCOUNT2_Key", string.Empty);
			this.m_namePlaceholder.text = this.m_account;
			this.m_loadingTxt.gameObject.SetActive(false);
			this.m_loginObj.SetActive(true);
			this.m_loginBt.onClick.AddListener(new UnityAction(this.OnClickLoginBt));
			this.m_pointCount = 1;
			this.m_time = 0f;
			this.OnUpdateText();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_isClickBt)
			{
				if (!this.m_isCanLoading)
				{
					EventArgsStartLogin instance = Singleton<EventArgsStartLogin>.Instance;
					instance.SetData(this.m_account, this.m_deviceId, this.m_account2);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_LoginViewModule_StartPullServerInfo, instance);
					this.m_isCanLoading = true;
				}
				this.OnUpdateText();
			}
		}

		protected override void OnDeInit()
		{
			this.m_loginBt.onClick.RemoveAllListeners();
			this.m_loginDataModule = null;
			this.m_isClickBt = false;
			this.m_isCanLoading = false;
			this.m_time = 0f;
			this.m_pointCount = 1;
		}

		private void OnUpdateText()
		{
			if (Time.time - this.m_time >= 1f)
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

		private void OnClickLoginBt()
		{
			if (!string.IsNullOrEmpty(this.m_nameInput.text))
			{
				this.m_account = this.m_nameInput.text;
			}
			if (string.IsNullOrEmpty(this.m_account))
			{
				return;
			}
			this.m_loadingTxt.gameObject.SetActive(true);
			this.m_loginObj.SetActive(false);
			this.m_isClickBt = true;
		}

		public GameObject m_loginObj;

		public CustomText m_loadingTxt;

		public CustomButton m_loginBt;

		public InputField m_nameInput;

		public Text m_namePlaceholder;

		[Label]
		public bool m_isClickBt;

		[Label]
		public bool m_isCanLoading;

		[Label]
		public string m_loadTxtInfo = string.Empty;

		[Label]
		public float m_time;

		[Label]
		public int m_pointCount = 1;

		[Label]
		public string m_account = string.Empty;

		[Label]
		public string m_deviceId = string.Empty;

		[Label]
		public string m_account2 = string.Empty;

		private LoginDataModule m_loginDataModule;
	}
}
