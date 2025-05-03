using System;
using System.Collections.Generic;
using System.Diagnostics;
using Dxx.Chat;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.UI;
using Framework.SDKManager;
using Framework.ViewModule;
using Habby.CustomEvent;
using HabbySDK.HabbySDK_LogFile;
using LoadingTimeTool;
using Proto.Chapter;
using Proto.Common;
using Proto.Guild;
using Proto.User;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class LoginViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.LoadingReset();
		}

		public override void OnOpen(object data)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_LoginViewModule_StartPullServerInfo, new HandlerEvent(this.OnEventPullServerInfo));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_LoginViewModule_StartLogin, new HandlerEvent(this.OnEventStartLogin));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_LoginViewModule_LoginAllready, new HandlerEvent(this.OnEventLoginAllReady));
			this.ShowProgress(false, LoginViewModule.LoadingState.None);
			this.m_customLogin.SetActive(false);
			this.m_sdkLogin.SetActive(false);
			if (GameApp.SDK.Login.IsLoginViaSDK)
			{
				this.m_sdkLogin.Init();
				this.m_sdkLogin.SetActive(true);
			}
			else
			{
				this.m_customLogin.Init();
				this.m_customLogin.SetActive(true);
			}
			this.m_isFinished = false;
			this.m_isGuildLoginFinished = false;
			this.m_isIAPShopFinished = false;
			this.Text_Version.text = Singleton<LanguageManager>.Instance.GetInfoByID("app_version_1", new object[] { Singleton<HotfixVersionMgr>.Instance.GetAppVersion() });
		}

		[Conditional("ENABLE_PROFILER")]
		public void EndLoadingPoint()
		{
			try
			{
				Dictionary<string, JsonDataOutput> jsonData = LoadingTime.GetJsonData();
				GameBootLogFile gameBootLogFile = new GameBootLogFile("CAPYBARA", "https://loganalyze.lezuan9.com/", 100);
				foreach (KeyValuePair<string, JsonDataOutput> keyValuePair in jsonData)
				{
					foreach (LoadingData loadingData in keyValuePair.Value.dataList)
					{
						GameBootLog gameBootLog = default(GameBootLog);
						gameBootLog.group = loadingData.group;
						gameBootLog.title = loadingData.title;
						gameBootLog.methodName = loadingData.methodName;
						gameBootLog.className = loadingData.className;
						gameBootLog.time = loadingData.time;
						gameBootLog.delayTime = loadingData.delayTime;
						gameBootLog.stepIndex = loadingData.stepIndex;
						gameBootLog.lastStepIndex = loadingData.lastStepIndex;
						GameBootLog gameBootLog2 = gameBootLog;
						gameBootLogFile.AddLogObject(gameBootLog2);
					}
				}
				gameBootLogFile.UploadLog();
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			LoginViewModule_SDKLogin sdkLogin = this.m_sdkLogin;
			if (sdkLogin != null)
			{
				sdkLogin.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			LoginViewModule_CustomLogin customLogin = this.m_customLogin;
			if (customLogin != null)
			{
				customLogin.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (this.m_isFinished && this.m_isGuildLoginFinished && this.m_isIAPShopFinished)
			{
				this.OnLoginFinished();
				this.m_isFinished = false;
			}
			this.UpdateLoginLoading(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_LoginViewModule_StartPullServerInfo, new HandlerEvent(this.OnEventPullServerInfo));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_LoginViewModule_StartLogin, new HandlerEvent(this.OnEventStartLogin));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_LoginViewModule_LoginAllready, new HandlerEvent(this.OnEventLoginAllReady));
			if (GameApp.SDK.Login.IsLoginViaSDK)
			{
				this.m_sdkLogin.DeInit();
				this.m_sdkLogin.SetActive(false);
				return;
			}
			this.m_customLogin.DeInit();
			this.m_customLogin.SetActive(false);
		}

		public override void OnDelete()
		{
			if (this.m_customLogin != null)
			{
				this.m_customLogin.DeInit();
			}
			if (this.m_sdkLogin != null)
			{
				this.m_sdkLogin.DeInit();
			}
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnEventPullServerInfo(object sender, int eventID, BaseEventArgs eventArgs)
		{
			EventArgsStartLogin eventArgsStartLogin = eventArgs as EventArgsStartLogin;
			if (eventArgsStartLogin == null)
			{
				return;
			}
			GameApp.NetWork.m_account = eventArgsStartLogin.m_account;
			GameApp.NetWork.m_deviceID = eventArgsStartLogin.m_deviceId;
			GameApp.NetWork.m_account2 = eventArgsStartLogin.m_account2;
			this.ShowProgress(true, LoginViewModule.LoadingState.NetProgress);
			EventArgsStartLogin instance = Singleton<EventArgsStartLogin>.Instance;
			instance.SetData(GameApp.NetWork.m_account, GameApp.NetWork.m_deviceID, GameApp.NetWork.m_account2);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_LoginViewModule_StartLogin, instance);
		}

		private void OnEventStartLogin(object sender, int eventID, BaseEventArgs eventArgs)
		{
			EventArgsStartLogin eventArgsStartLogin = eventArgs as EventArgsStartLogin;
			if (eventArgsStartLogin == null)
			{
				return;
			}
			this.OnLogin(eventArgsStartLogin.m_account, eventArgsStartLogin.m_deviceId, eventArgsStartLogin.m_account2);
			this.ShowLoginButton(false);
		}

		private void OnLogin(string account, string deviceID, string account2)
		{
			GameApp.NetWork.m_account = account;
			GameApp.NetWork.m_deviceID = deviceID;
			GameApp.NetWork.m_account2 = account2;
			GameApp.SDK.Analyze.TrackLogin("登录", null);
			NetworkUtils.Login.UserLoginRequest(delegate(bool result, UserLoginResponse response)
			{
				if (!result)
				{
					this.OnLoginFailure(10101, response.Code);
					return;
				}
				GameApp.SDK.Analyze.TGALogin(response.AccountKey);
				SDKManager.SDKAppsFlyer appsFlyerSDK = GameApp.SDK.AppsFlyerSDK;
				if (appsFlyerSDK != null)
				{
					appsFlyerSDK.SetCustomerUserId(response.AccountKey);
				}
				SDKManager.SDKFirebase firebaseSDK = GameApp.SDK.FirebaseSDK;
				if (firebaseSDK != null)
				{
					firebaseSDK.SetUserId(response.AccountKey);
				}
				Utility.PlayerPrefs.SetString("ACCOUNT_Key", GameApp.NetWork.m_account);
				if (!string.IsNullOrEmpty(GameApp.NetWork.m_deviceID))
				{
					Utility.PlayerPrefs.SetString("DEVICEID_Key", GameApp.NetWork.m_deviceID);
				}
				if (!string.IsNullOrEmpty(GameApp.NetWork.m_deviceID))
				{
					Utility.PlayerPrefs.SetString("ACCOUNT2_Key", GameApp.NetWork.m_account2);
				}
				Utility.PlayerPrefs.SetUserId(response.UserId.ToString());
				GameApp.SocketNet.Init();
				EventLogin instance = Singleton<EventLogin>.Instance;
				instance.SetData(response);
				GameApp.Event.DispatchNow(this, 107, instance);
				if (GameApp.GuildConfig.IsEnable)
				{
					this.OnGuildLogin(account, deviceID, account2, response);
				}
				else
				{
					this.m_isGuildLoginFinished = true;
				}
				if (GameApp.Mail.IsEnable)
				{
					this.OnMailLogin(response.UserId.ToString());
				}
				Singleton<NoticeManager>.Instance.Init(response.UserId.ToString());
				IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
				dataModule.UpdateShopDrawCount(response.ShopDrawDto);
				dataModule.UpdateShopSUpPoolDrawCount(response.ShopSupCount);
				dataModule.UpdateTGAInfo(response.TgaInfoDto);
				this.LoginInitShoData(response.ShopAllDataDto);
				GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateTGAInfo(response.TgaInfoDto);
				NetworkUtils.Login.OnLoginGetActivityCommonInfo(new Action<int, int>(this.OnLoginOtherFailure));
				NetworkUtils.Login.OnLoginGetActivitySlotTrainInfo(new Action<int, int>(this.OnLoginOtherFailure));
				NetworkUtils.Login.OnLoginGetWorldBossInfo(new Action<int, int>(this.OnLoginOtherFailure));
				this.AddSceneSocketGroup();
				GameAFTools.Ins.OnLogin(response);
				GameTGATools.Ins.ClearExtraADCount();
				this.m_isFinished = true;
				GameApp.SDK.Analyze.Track_Login("登陆成功");
				string text = "TGA_ReActive";
				DateTime dateTime = DxxTools.Time.UnixTimestampToDateTime(response.Timestamp);
				DateTime dateTime2 = DxxTools.Time.UnixTimestampToDateTime((double)response.TgaInfoDto.CreateTime);
				if (Mathf.Abs((float)(dateTime - dateTime2).TotalSeconds) > 10f)
				{
					if (PlayerPrefs.GetInt(text, 0) == 0)
					{
						GameApp.SDK.Analyze.Track_Reactive();
						PlayerPrefs.SetInt(text, 1);
					}
				}
				else
				{
					PlayerPrefs.SetInt(text, 1);
				}
				GameApp.SDK.Analyze.Track_FirstActive(response.AccountKey);
				this.SwitchPreHotFix();
				HabbyEventDispatch.Send("event.game.user.login.success", new Dictionary<string, object>
				{
					{ "userid", response.UserId },
					{ "isNew", response.IsNewUser },
					{ "loginDays", 0 },
					{ "loginCount", 0 },
					{ "purchaseAmount", 0 },
					{ "purchaseCount", 0 },
					{ "purchaseMonthAmount", 0 }
				});
			});
		}

		private void OnGuildLogin(string account, string platformUid, string platformUid2, UserLoginResponse response)
		{
			GuildNetUtil.Guild.LoginSetUserData(account, response);
			GuildNetUtil.Guild.DoRequest_GuildLoginRequest(delegate(bool result, GuildGetInfoResponse resp)
			{
				if (result)
				{
					this.m_isGuildLoginFinished = true;
				}
			});
		}

		private void LoginInitShoData(ShopAllDataDto dataDto)
		{
			try
			{
				IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
				if (dataDto.IapInfo == null)
				{
					dataDto.IapInfo = new IAPDto();
				}
				dataModule.InitForServer(dataDto);
				GameApp.Data.GetDataModule(DataName.AdDataModule).InitServerData(dataDto.AdData);
				this.m_isIAPShopFinished = true;
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				this.LoadingReset();
				GameApp.View.CloseAllView(new int[] { 101, 102, 106 });
				GameApp.State.ActiveState(StateName.LoginState);
			}
		}

		private void OnMailLogin(string userID)
		{
			GameApp.Mail.GetManager().Login(userID);
			GameApp.Mail.GetManager().OnRefreshMailListData(null);
		}

		private void AddSceneSocketGroup()
		{
			GameApp.SocketNet.SetSocketGroup(2, ChatProxy.SceneChat.GetGroupID());
		}

		private void OnLoginFailure(int sendMsgId, int errorCode)
		{
			this.isShowLoginFailUI = true;
			string text = Singleton<LanguageManager>.Instance.GetInfoByID("156", new object[] { sendMsgId, errorCode });
			if (errorCode == 113)
			{
				string text2 = string.Format("server_err_{0}", errorCode);
				text = Singleton<LanguageManager>.Instance.GetInfoByID(text2);
			}
			HLog.LogError(text);
			this.ShowProgress(false, LoginViewModule.LoadingState.None);
			this.ShowLoginButton(true);
			if (errorCode == 113)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("17");
				DxxTools.UI.OpenPopCommon(text, delegate(int id)
				{
					if (id == 1)
					{
						GameApp.Quit();
					}
				}, string.Empty, infoByID, string.Empty, false, 2);
				return;
			}
			DxxTools.UI.OpenPopCommonNoCancle(text, delegate(int id)
			{
				this.isShowLoginFailUI = false;
				EventArgsStartLogin instance = Singleton<EventArgsStartLogin>.Instance;
				instance.SetData(GameApp.NetWork.m_account, GameApp.NetWork.m_deviceID, GameApp.NetWork.m_account2);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_LoginViewModule_StartPullServerInfo, instance);
			});
		}

		private void OnLoginOtherFailure(int severCodeID, int gameCodeID)
		{
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("156", new object[] { severCodeID, gameCodeID });
			if (this.isShowLoginFailUI)
			{
				HLog.LogError(infoByID);
				return;
			}
			this.isShowLoginFailUI = true;
			DxxTools.UI.OpenPopCommonNoCancle(infoByID, delegate(int id)
			{
				this.isShowLoginFailUI = false;
			});
		}

		private void OnLoginFinished()
		{
			MainState.ResetEnterCount();
			this.ShowProgress(true, LoginViewModule.LoadingState.ResLoaderProgress);
			if (this.isLoginEnter)
			{
				return;
			}
			this.isLoginEnter = true;
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			ChapterDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			if (!dataModule.IsNewUser)
			{
				this.EnterMain();
				return;
			}
			if (Singleton<EventRecordController>.Instance.IsHaveChapterRecord())
			{
				this.EnterBattle();
				return;
			}
			UserTicket ticket = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.UserLife);
			if (ticket != null && (ulong)ticket.NewNum >= (ulong)((long)dataModule2.CurrentChapter.CostEnergy))
			{
				NetworkUtils.Chapter.DoStartChapterRequest(dataModule2.ChapterID, delegate(bool isOk, StartChapterResponse res)
				{
					if (isOk)
					{
						this.EnterBattle();
						return;
					}
					this.EnterMain();
				});
				return;
			}
			this.EnterMain();
		}

		private void EnterMain()
		{
			GameApp.State.ActiveState(StateName.MainState);
			this.isLoginEnter = false;
		}

		private void EnterBattle()
		{
			EventArgsGameDataEnter instance = Singleton<EventArgsGameDataEnter>.Instance;
			instance.SetData(GameModel.Chapter, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance);
			GameApp.State.ActiveState(StateName.BattleChapterState);
			this.isLoginEnter = false;
		}

		private void SwitchPreHotFix()
		{
			string text = "SwitchPreHotFix";
			if (GameApp.SDK.GetCloudDataValue<bool>(text, false))
			{
				PlayerPrefs.SetInt(text, 1);
				return;
			}
			if (PlayerPrefs.HasKey(text))
			{
				PlayerPrefs.DeleteKey(text);
			}
		}

		private void LoadingReset()
		{
			this.txtProgress.text = "";
			this.progressCtrl.value = 0f;
			this.progressCtrl.gameObject.SetActive(false);
			this.timer = 0f;
			this.loadingState = LoginViewModule.LoadingState.None;
			this.remainTime = 5.9f;
			this.isAllReady = false;
			this.m_time = 0f;
			this.m_pointCount = 0;
		}

		private void UpdateLoginLoading(float deltaTime, float unscaledDeltaTime)
		{
			if (!this.isShowProgress || this.loadingState == LoginViewModule.LoadingState.None)
			{
				return;
			}
			if (this.loadingState == LoginViewModule.LoadingState.ResLoaderProgress && this.remainTime < 2.4f)
			{
				this.remainTime = 2.4f;
			}
			float num = this.progressCtrl.value;
			if (this.remainTime > unscaledDeltaTime)
			{
				float num2 = unscaledDeltaTime / this.remainTime;
				num += num2;
				this.remainTime -= unscaledDeltaTime;
			}
			else
			{
				num = 1f;
			}
			if (!this.isAllReady && num > 0.85f)
			{
				num = 0.85f;
			}
			this.progressCtrl.value = num;
			this.OnUpdateText();
			if (this.progressCtrl.value >= 1f && this.isAllReady)
			{
				this.isAllReady = false;
				if (GameApp.View.IsOpened(ViewName.LoginViewModule))
				{
					GameApp.View.CloseView(ViewName.LoginViewModule, null);
				}
			}
		}

		public void ShowProgress(bool active, LoginViewModule.LoadingState state)
		{
			this.isShowProgress = active;
			this.loadingState = state;
			if (!active)
			{
				this.LoadingReset();
			}
			if (this.progressCtrl.gameObject.activeSelf != active)
			{
				this.progressCtrl.gameObject.SetActive(active);
			}
			if (active)
			{
				this.txtProgress.text = Singleton<LanguageManager>.Instance.GetInfoByID("login_loading");
			}
		}

		public void ShowLoginButton(bool active)
		{
			this.m_sdkLogin.SetActive(active && GameApp.SDK.Login.IsLoginViaSDK);
			this.m_customLogin.SetActive(active && !GameApp.SDK.Login.IsLoginViaSDK);
		}

		private void OnEventLoginAllReady(object sender, int eventID, BaseEventArgs eventArgs)
		{
			this.isAllReady = true;
			if (this.remainTime > 0.3f)
			{
				this.remainTime = 0.3f;
			}
		}

		private void OnUpdateText()
		{
			if (this.isShowProgress && Time.time - this.m_time >= 0.25f)
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
				this.txtProgress.text = Singleton<LanguageManager>.Instance.GetInfoByID("login_loading") + text;
				this.m_time = Time.time;
			}
		}

		public LoginViewModule_CustomLogin m_customLogin;

		public LoginViewModule_SDKLogin m_sdkLogin;

		public LoginViewModule_MiniGame m_webGameLogin;

		[SerializeField]
		[Label]
		private bool m_isGuildLoginFinished;

		[SerializeField]
		[Label]
		private bool m_isIAPShopFinished;

		[SerializeField]
		[Label]
		private bool m_isFinished;

		public Slider progressCtrl;

		public CustomText txtProgress;

		public CustomText Text_Version;

		private bool isLoginEnter;

		private bool isShowLoginFailUI;

		private bool isShowProgress;

		private const float loginDuration1 = 2.4f;

		private const float loginDuration2 = 3.5f;

		private float timer;

		private LoginViewModule.LoadingState loadingState;

		private float remainTime = 5.9f;

		private bool isAllReady;

		private float m_time;

		private int m_pointCount;

		public enum LoadingState
		{
			None,
			NetProgress,
			ResLoaderProgress
		}
	}
}
