using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using AppsFlyerSDK;
using Facebook.Unity;
using Firebase;
using Firebase.Analytics;
using Firebase.Crashlytics;
using Framework.HabbyWebview;
using Framework.Logic;
using Framework.Logic.Modules;
using Framework.Logic.Platform;
using Framework.Platfrom;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Habby.Base;
using Habby.Reyun.Runtimes;
using HabbySDK.CloudConfig;
using HabbySDK.CloudConfig.Net;
using HabbySdk.Common;
using HabbySDK.WebGame;
using ThinkingData.Analytics;
using UnityEngine;

namespace Framework.SDKManager
{
	public class SDKManager : MonoBehaviour
	{
		private void InitAppsFlyer()
		{
			AppsFlyer.setIsDebug(false);
			AppsFlyer.initSDK("KVCJbTZMRAoqmfhVQ2Dsfb", "");
			AppsFlyer.setMinTimeBetweenSessions(180);
			AppsFlyer.startSDK();
		}

		public void InitATT()
		{
			this.AttManager = new ATTManager();
			this.AttManager.OnInit();
		}

		public bool CloudConfigRequestEnd { get; private set; }

		public void InitCloudConfig(Action<bool> onCallback)
		{
			this._tgaTrackedDic.Clear();
			this.CloudConfigRequestEnd = false;
			bool isReleaseServer = GameApp.Config.GetBool("IsReleaseServer");
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["CloudConfig_IsReleaseServer"] = isReleaseServer;
			SDKManager.SDKTGA analyze = this.Analyze;
			if (analyze != null)
			{
				analyze.Track("RequestCloudConfig", dictionary, true);
			}
			AbSingleton<HabbyCloudConfigManager>.Instance.RequestCloudConfig(this.makeClientData(), isReleaseServer, delegate(bool rsp)
			{
				this.CloudConfigRequestEnd = true;
				Action<bool> onCallback2 = onCallback;
				if (onCallback2 != null)
				{
					onCallback2(rsp);
				}
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2["CloudConfig_IsReleaseServer"] = isReleaseServer;
				dictionary2["CloudConfig_Result"] = rsp;
				SDKManager.SDKTGA analyze2 = this.Analyze;
				if (analyze2 == null)
				{
					return;
				}
				analyze2.Track("CloundConfigResponse", dictionary2, true);
			}, 0);
		}

		public HabbyCloudData CloudData
		{
			get
			{
				return AbSingleton<HabbyCloudConfigManager>.Instance.CloudData;
			}
		}

		public T GetCloudDataValue<T>(string key, T defaultValue)
		{
			T t;
			if (this.CloudData == null)
			{
				HLog.LogError("CloudConfig  CloudData == null");
				t = defaultValue;
			}
			else
			{
				t = this.CloudData.Get<T>(key, defaultValue);
			}
			if (!this._tgaTrackedDic.ContainsKey(key))
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["CloudConfig_Key"] = key;
				dictionary["CloudConfig_IsRequestEnd"] = this.CloudConfigRequestEnd;
				dictionary["CloudConfig_Value"] = t;
				SDKManager.SDKTGA analyze = this.Analyze;
				if (analyze != null)
				{
					analyze.Track("GetCloudDataValue", dictionary, true);
				}
				this._tgaTrackedDic.Add(key, true);
			}
			return t;
		}

		public bool IfWebGameAppleReviewMode
		{
			get
			{
				return false;
			}
		}

		public bool IfShowWechatFriendsList
		{
			get
			{
				return false;
			}
		}

		public bool IfEnableAd
		{
			get
			{
				try
				{
					return this.CloudData.IfEnableAd;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
				return false;
			}
		}

		private ClientData makeClientData()
		{
			LanguageDataModule dataModule = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
			return new ClientData
			{
				deviceId = (SDKManager.SDKLogin.channel_login_get_deviceid() ?? Guid.NewGuid().ToString()),
				appBundle = (Application.identifier ?? "com.habby.capybara"),
				appVersion = (Application.version ?? "1.0.0"),
				osVersion = (SystemInfo.operatingSystem ?? ""),
				appLanguage = (dataModule.GetLanguageAbbr(dataModule.GetCurrentLanguageType) ?? "en"),
				systemLanguage = (Application.systemLanguage.ToString() ?? "ChineseSimplified"),
				deviceModel = (SystemInfo.deviceModel ?? ""),
				channelId = 2,
				os = 2
			};
		}

		public void Init()
		{
			this.InitATT();
			GameAdsManager.Instance.InitUmp(true);
			this.FirebaseSDK = new SDKManager.SDKFirebase(this);
			this.AppsFlyerSDK = new SDKManager.SDKAppsFlyer(this);
			this.Analyze = new SDKManager.SDKTGA(this);
			this.FBSDK = new SDKManager.SDKFaceBook(this);
			this.Login = new SDKManager.SDKLogin(this);
			this.InitFirebase();
			this.InitAppsFlyer();
			this.InitTGA();
			this.InitFaceBook();
			GameAdsManager.Instance.InitAds();
			this.WebView = new SDKManager.SDKWebView(this);
			this.Rate = new SDKManager.SDKRate(this);
		}

		internal void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.AttManager != null)
			{
				this.AttManager.OnUpdate();
			}
			this.Analyze.OnUpdate(deltaTime);
		}

		private async void InitFaceBook()
		{
			await TaskExpand.Delay(1);
			this.FBSDK.OnInitFaceBook();
		}

		public void InitFirebase()
		{
			this.FirebaseSDK.InitFireBaseSDK();
		}

		public void ShowGameClubButton(RectTransform targetRectTransform)
		{
			Camera worldCamera = targetRectTransform.GetComponentInParent<Canvas>().worldCamera;
			Vector3 vector;
			vector..ctor(-targetRectTransform.rect.width * targetRectTransform.pivot.x, targetRectTransform.rect.height * (1f - targetRectTransform.pivot.y));
			Vector3 vector2;
			vector2..ctor(targetRectTransform.rect.width * targetRectTransform.pivot.x, -targetRectTransform.rect.height * (1f - targetRectTransform.pivot.y));
			Vector3 vector3 = targetRectTransform.TransformPoint(vector);
			Vector3 vector4 = targetRectTransform.TransformPoint(vector2);
			RectTransformUtility.WorldToScreenPoint(worldCamera, vector3);
			RectTransformUtility.WorldToScreenPoint(worldCamera, targetRectTransform.transform.position);
			RectTransformUtility.WorldToScreenPoint(worldCamera, vector4);
		}

		public void HideGameClubButton()
		{
		}

		public void SetClipboardData(string text)
		{
			GUIUtility.systemCopyBuffer = text;
		}

		public void OpenPrivacyContract()
		{
		}

		public bool IsFringeDevice()
		{
			return false;
		}

		public void CleanCache()
		{
		}

		public void ExitProgram()
		{
		}

		public void RestartProgram()
		{
		}

		private void InitReYun()
		{
			this.ReYun = ReYunAPIHelper.GetReYunAPI();
		}

		private void InitTGA()
		{
			this.Analyze.InitTGA();
		}

		public IWebGame WebGameAPI
		{
			get
			{
				return SingletonScript<WebGameManager>.Instance.WebGameAPI;
			}
		}

		public WebGameConfig WebGameConfig
		{
			get
			{
				return SingletonScript<WebGameManager>.Instance.Config;
			}
		}

		public async void InitWebGame()
		{
			this.WebGameSDKAPI = new SDKManager.HabbyWebGameAPI();
		}

		public static string channel_login_get_deviceid()
		{
			string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
			string.IsNullOrEmpty(deviceUniqueIdentifier);
			return deviceUniqueIdentifier;
		}

		public SDKManager.SDKAppsFlyer AppsFlyerSDK;

		public ATTManager AttManager;

		private Dictionary<string, bool> _tgaTrackedDic = new Dictionary<string, bool>();

		public bool IsRelease;

		public SDKManager.SDKFaceBook FBSDK;

		public SDKManager.SDKFirebase FirebaseSDK;

		public SDKManager.SDKLogin Login;

		public SDKManager.SDKRate Rate;

		public IReYunAPI ReYun;

		public SDKManager.SDKTGA Analyze;

		public SDKManager.HabbyWebGameAPI WebGameSDKAPI;

		public SDKManager.SDKWebView WebView;

		public class SDKAppsFlyer
		{
			public SDKAppsFlyer(SDKManager mgr)
			{
				this.mMgr = mgr;
			}

			public string GetAppsFlyerId()
			{
				return AppsFlyer.getAppsFlyerId();
			}

			public void SetCustomerUserId(string id)
			{
				AppsFlyer.setCustomerUserId(id);
			}

			public void setConsentData()
			{
				AppsFlyer.setConsentData(AppsFlyerConsent.ForGDPRUser(true, true));
			}

			public void sendEvent(string eventName, Dictionary<string, string> properties)
			{
				AppsFlyer.sendEvent(eventName, properties);
			}

			public string GetAFInAppEvents(AFInAppEventEnum e)
			{
				switch (e)
				{
				case AFInAppEventEnum.LEVEL_ACHIEVED:
					return "af_level_achieved";
				case AFInAppEventEnum.ADD_PAYMENT_INFO:
					return "af_add_payment_info";
				case AFInAppEventEnum.ADD_TO_CART:
					return "af_add_to_cart";
				case AFInAppEventEnum.ADD_TO_WISH_LIST:
					return "af_add_to_wishlist";
				case AFInAppEventEnum.COMPLETE_REGISTRATION:
					return "af_complete_registration";
				case AFInAppEventEnum.TUTORIAL_COMPLETION:
					return "af_tutorial_completion";
				case AFInAppEventEnum.INITIATED_CHECKOUT:
					return "af_initiated_checkout";
				case AFInAppEventEnum.PURCHASE:
					return "af_purchase";
				case AFInAppEventEnum.RATE:
					return "af_rate";
				case AFInAppEventEnum.SEARCH:
					return "af_search";
				case AFInAppEventEnum.SPENT_CREDIT:
					return "af_spent_credits";
				case AFInAppEventEnum.ACHIEVEMENT_UNLOCKED:
					return "af_achievement_unlocked";
				case AFInAppEventEnum.CONTENT_VIEW:
					return "af_content_view";
				case AFInAppEventEnum.TRAVEL_BOOKING:
					return "af_travel_booking";
				case AFInAppEventEnum.SHARE:
					return "af_share";
				case AFInAppEventEnum.INVITE:
					return "af_invite";
				case AFInAppEventEnum.LOGIN:
					return "af_login";
				case AFInAppEventEnum.RE_ENGAGE:
					return "af_re_engage";
				case AFInAppEventEnum.UPDATE:
					return "af_update";
				case AFInAppEventEnum.OPENED_FROM_PUSH_NOTIFICATION:
					return "af_opened_from_push_notification";
				case AFInAppEventEnum.LOCATION_CHANGED:
					return "af_location_changed";
				case AFInAppEventEnum.LOCATION_COORDINATES:
					return "af_location_coordinates";
				case AFInAppEventEnum.ORDER_ID:
					return "af_order_id";
				case AFInAppEventEnum.LEVEL:
					return "af_level";
				case AFInAppEventEnum.SCORE:
					return "af_score";
				case AFInAppEventEnum.SUCCESS:
					return "af_success";
				case AFInAppEventEnum.PRICE:
					return "af_price";
				case AFInAppEventEnum.CONTENT_TYPE:
					return "af_content_type";
				case AFInAppEventEnum.CONTENT_ID:
					return "af_content_id";
				case AFInAppEventEnum.CONTENT_LIST:
					return "af_content_list";
				case AFInAppEventEnum.CURRENCY:
					return "af_currency";
				case AFInAppEventEnum.QUANTITY:
					return "af_quantity";
				case AFInAppEventEnum.REGSITRATION_METHOD:
					return "af_registration_method";
				case AFInAppEventEnum.PAYMENT_INFO_AVAILIBLE:
					return "af_payment_info_available";
				case AFInAppEventEnum.MAX_RATING_VALUE:
					return "af_max_rating_value";
				case AFInAppEventEnum.RATING_VALUE:
					return "af_rating_value";
				case AFInAppEventEnum.SEARCH_STRING:
					return "af_search_string";
				case AFInAppEventEnum.DATE_A:
					return "af_date_a";
				case AFInAppEventEnum.DATE_B:
					return "af_date_b";
				case AFInAppEventEnum.DESTINATION_A:
					return "af_destination_a";
				case AFInAppEventEnum.DESTINATION_B:
					return "af_destination_b";
				case AFInAppEventEnum.DESCRIPTION:
					return "af_description";
				case AFInAppEventEnum.CLASS:
					return "af_class";
				case AFInAppEventEnum.EVENT_START:
					return "af_event_start";
				case AFInAppEventEnum.EVENT_END:
					return "af_event_end";
				case AFInAppEventEnum.LATITUDE:
					return "af_lat";
				case AFInAppEventEnum.LONGTITUDE:
					return "af_long";
				case AFInAppEventEnum.CUSTOMER_USER_ID:
					return "af_customer_user_id";
				case AFInAppEventEnum.VALIDATED:
					return "af_validated";
				case AFInAppEventEnum.REVENUE:
					return "af_revenue";
				case AFInAppEventEnum.RECEIPT_ID:
					return "af_receipt_id";
				case AFInAppEventEnum.PARAM_1:
					return "af_param_1";
				case AFInAppEventEnum.PARAM_2:
					return "af_param_2";
				case AFInAppEventEnum.PARAM_3:
					return "af_param_3";
				case AFInAppEventEnum.PARAM_4:
					return "af_param_4";
				case AFInAppEventEnum.PARAM_5:
					return "af_param_5";
				case AFInAppEventEnum.PARAM_6:
					return "af_param_6";
				case AFInAppEventEnum.PARAM_7:
					return "af_param_7";
				case AFInAppEventEnum.PARAM_8:
					return "af_param_8";
				case AFInAppEventEnum.PARAM_9:
					return "af_param_9";
				case AFInAppEventEnum.PARAM_10:
					return "af_param_10";
				default:
					return "";
				}
			}

			private SDKManager mMgr;
		}

		public class SDKFaceBook
		{
			public bool IsInitSuccess
			{
				get
				{
					return FB.IsInitialized;
				}
			}

			public SDKFaceBook(SDKManager mgr)
			{
				this.mMgr = mgr;
			}

			public void OnInitFaceBook()
			{
				if (!FB.IsInitialized)
				{
					FB.Init(new InitDelegate(this.OnInitComplete), new HideUnityDelegate(this.OnHideUnity), null);
					return;
				}
				FB.ActivateApp();
			}

			private void OnInitComplete()
			{
				if (FB.IsInitialized)
				{
					FB.ActivateApp();
				}
			}

			private void OnHideUnity(bool isUnityShown)
			{
			}

			private SDKManager mMgr;
		}

		public class SDKFirebase
		{
			public bool IsInitSuccess { get; private set; }

			public SDKFirebase(SDKManager mgr)
			{
				this.mMgr = mgr;
			}

			public void InitFireBaseSDK()
			{
				string text = "FireBase/FireBaseConfig";
				this.IsInitSuccess = false;
				this.mConfig = Resources.Load<FirebaseConfig>(text);
				if (this.mConfig == null)
				{
					this.IsInitSuccess = false;
					return;
				}
				FirebaseApp.LogLevel = 4;
				FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(delegate(Task<DependencyStatus> task)
				{
					DependencyStatus result = task.Result;
					if (result == null)
					{
						FirebaseApp defaultInstance = FirebaseApp.DefaultInstance;
						if (this.mConfig.EnableDebug)
						{
							StringBuilder stringBuilder = new StringBuilder();
							stringBuilder.Append(" ApiKey = ");
							stringBuilder.Append(defaultInstance.Options.ApiKey);
							stringBuilder.Append(" AppId = ");
							stringBuilder.Append(defaultInstance.Options.AppId);
							stringBuilder.Append(" DatabaseUrl = ");
							stringBuilder.Append(defaultInstance.Options.DatabaseUrl);
							stringBuilder.Append(" MessageSenderId = ");
							stringBuilder.Append(defaultInstance.Options.MessageSenderId);
							stringBuilder.Append(" StorageBucket = ");
							stringBuilder.Append(defaultInstance.Options.StorageBucket);
							stringBuilder.Append(" ProjectId = ");
							stringBuilder.Append(defaultInstance.Options.ProjectId);
						}
						this.IsInitSuccess = true;
						return;
					}
					HLog.LogError(string.Format("[SDK][Firebase]Firebase Init Fail Could not resolve all Firebase dependencies: {0}", result));
				});
			}

			private void OnHandheld(string condition, string stackTrace, LogType type)
			{
				if (type == 1 || type == 4 || type == null)
				{
					this.ReportCrashLog(string.Format("[{0}]{1}\r\n{2}", type, condition, stackTrace));
					return;
				}
				if (type == 2 && !string.IsNullOrEmpty(condition) && condition.IndexOf("DOTWEEN") >= 0)
				{
					this.ReportCrashLog(string.Format("[{0}]{1}\r\n{2}", type, condition, stackTrace));
				}
			}

			public void SetUserID(string userid)
			{
				if (this.IsInitSuccess && !string.IsNullOrEmpty(userid))
				{
					Crashlytics.SetUserId(userid);
				}
			}

			public void SetCustomKey(string key, string value)
			{
				if (this.IsInitSuccess && !string.IsNullOrEmpty(key))
				{
					Crashlytics.SetCustomKey(key, value);
				}
			}

			public void ReportException(Exception e)
			{
				if (this.IsInitSuccess)
				{
					Crashlytics.LogException(e);
				}
			}

			public void ReportCrashLog(string log)
			{
				if (this.IsInitSuccess)
				{
					Crashlytics.Log(log);
				}
			}

			public void SendEvent(string eventName, Dictionary<string, string> properties)
			{
				Parameter[] array = new Parameter[properties.Count];
				int num = 0;
				foreach (KeyValuePair<string, string> keyValuePair in properties)
				{
					array[num] = new Parameter(keyValuePair.Key, keyValuePair.Value);
					num++;
				}
				FirebaseAnalytics.LogEvent(eventName, array);
			}

			public void SetUserId(string id)
			{
				FirebaseAnalytics.SetUserId(id);
			}

			private FirebaseConfig mConfig;

			private SDKManager mMgr;
		}

		public class SDKLogin
		{
			public string m_deviceId
			{
				get
				{
					return SDKManager.SDKLogin.channel_login_get_deviceid();
				}
			}

			public bool IsInitOver { get; private set; }

			public bool IsInitSuccess { get; private set; }

			public bool IsLoginViaSDK
			{
				get
				{
					return true;
				}
			}

			public SDKLogin(SDKManager mgr)
			{
				this.mMgr = mgr;
			}

			public void OnSDKLogin(Action<string> callback)
			{
				this.m_account = string.Empty;
				this.IsInitOver = false;
				this.IsInitSuccess = false;
				this.mActionLogin = callback;
				if (this.IsLoginViaSDK)
				{
					this.Channel_Login(delegate
					{
						this.m_account = this.channel_login_get_userid();
						this.m_account2 = this.channel_login_get_userid2();
						this.IsInitSuccess = !string.IsNullOrEmpty(this.m_deviceId);
						this.IsInitOver = true;
						Action<string> action2 = this.mActionLogin;
						if (action2 != null)
						{
							action2(this.m_deviceId);
						}
						this.Channel_Login_Cancel();
					});
					return;
				}
				this.IsInitSuccess = false;
				this.IsInitOver = true;
				Action<string> action = this.mActionLogin;
				if (action == null)
				{
					return;
				}
				action(this.m_account);
			}

			public int GetGPStatus()
			{
				return this.m_gp_status;
			}

			public void GPLoginCancel()
			{
				this.m_gp_status = 10;
			}

			public void Channel_Login(Action callback)
			{
				this.channel_login_googleplay(callback);
			}

			public void Channel_Login_Cancel()
			{
			}

			public void channel_login_set_userid(string userid)
			{
				if (string.IsNullOrEmpty(userid))
				{
					return;
				}
				Utility.PlayerPrefs.GetString("PLAYERPREFS_PLAYERID");
				Utility.PlayerPrefs.SetString("PLAYERPREFS_PLAYERID", userid);
			}

			public string channel_login_get_userid()
			{
				return Utility.PlayerPrefs.GetString("PLAYERPREFS_PLAYERID");
			}

			public void channel_login_set_userid2(string userid2)
			{
				if (string.IsNullOrEmpty(userid2))
				{
					return;
				}
				Utility.PlayerPrefs.GetString("PLAYERPREFS_PLAYERID2");
				Utility.PlayerPrefs.SetString("PLAYERPREFS_PLAYERID2", userid2);
			}

			public string channel_login_get_userid2()
			{
				return Utility.PlayerPrefs.GetString("PLAYERPREFS_PLAYERID2");
			}

			public static string channel_login_get_deviceid()
			{
				return SystemInfo.deviceUniqueIdentifier;
			}

			private void channel_login_gamecenter(Action callback)
			{
			}

			private void channel_login_googleplay(Action callback)
			{
				if (this.m_gp_status <= 0)
				{
					this.m_gp_status = 1;
					PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build());
					PlayGamesPlatform.DebugLogEnabled = true;
					PlayGamesPlatform.Activate();
					Social.Active.Authenticate(Social.Active.localUser, delegate(bool success, string error_msg)
					{
						if (this.m_gp_status == 10)
						{
							Action callback3 = callback;
							if (callback3 == null)
							{
								return;
							}
							callback3();
							return;
						}
						else
						{
							if (success)
							{
								if (this.m_gp_status == 1)
								{
									this.m_gp_status = 2;
								}
								PlayGamesLocalUser playGamesLocalUser = (PlayGamesLocalUser)Social.Active.localUser;
								this.channel_login_set_userid(playGamesLocalUser.id);
							}
							else if (this.m_gp_status == 1)
							{
								this.m_gp_status = 3;
							}
							Action callback4 = callback;
							if (callback4 == null)
							{
								return;
							}
							callback4();
							return;
						}
					});
					return;
				}
				Action callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2();
			}

			public void GameCenter_add_login_count()
			{
				int num = this.GameCenter_get_login_count();
				Utility.PlayerPrefs.SetInt("PLAYERPREFS_GCOPENCOUNT", num + 1);
			}

			public int GameCenter_get_login_count()
			{
				return Utility.PlayerPrefs.GetInt("PLAYERPREFS_GCOPENCOUNT", 0);
			}

			public void GameCenter_clear_login_count()
			{
				Utility.PlayerPrefs.SetInt("PLAYERPREFS_GCOPENCOUNT", 0);
			}

			public string get_gamecenter_playerid()
			{
				return "";
			}

			public string get_gamecenter_teamplayerid()
			{
				return "";
			}

			public string m_account = string.Empty;

			public string m_account2 = string.Empty;

			private Action<string> mActionLogin;

			private SDKManager mMgr;

			public const string PlayerPrefs_GCOpenCount = "PLAYERPREFS_GCOPENCOUNT";

			public const string PlayerPrefs_PlayerID = "PLAYERPREFS_PLAYERID";

			public const string PlayerPrefs_PlayerID2 = "PLAYERPREFS_PLAYERID2";

			public const float mGameCenterWaitMaxTimeout = 5f;

			private int m_gp_status;
		}

		public class SDKRate
		{
			public SDKRate(SDKManager mgr)
			{
				this.mMgr = mgr;
			}

			public void OpenRate()
			{
				Extensions.GetRate(HabbySdkManager.Instance).OpenReview(delegate(bool success, string error)
				{
				}, 0f);
			}

			private SDKManager mMgr;
		}

		public class SDKTGA
		{
			public bool IsThinkingInitSuccess { get; private set; }

			public string Signature
			{
				get
				{
					if (string.IsNullOrEmpty(this.mSignature))
					{
						this.mSignature = PlatformHelper.GetSignatureHash();
					}
					return this.mSignature;
				}
			}

			public SDKTGA(SDKManager mgr)
			{
				this.mMgr = mgr;
				SDKManager.SDKTGA.ResumeFromBackground = false;
				this.m_AppStartTime = DateTime.MinValue;
			}

			public void InitTGA()
			{
				if (this.mMgr == null)
				{
					return;
				}
				string text = string.Empty;
				text = "TGA/TGA_Android";
				if (string.IsNullOrEmpty(text))
				{
					return;
				}
				Object.Instantiate<GameObject>(Resources.Load<GameObject>(text)).name = "ThinkingAnalytics";
				TDAnalytics.EnableAutoTrack(51, null, "");
				this.IsThinkingInitSuccess = true;
				TDAnalytics.EnableLog(true);
				this.TrackFirstEvent("first_open", new Dictionary<string, object>(), "");
				this.AppInstall();
				this.AppStart(null);
				GameApp.State.RegisterChangeState(new Action<int>(this.ChangeState));
				TDAnalytics.EnableAutoTrack(1, new AutoTrackECB(), "");
			}

			public void OnUpdate(float deltaTime)
			{
			}

			public bool IsLogin { get; private set; }

			public void Login(string id)
			{
				if (string.IsNullOrEmpty(id) || this.mTGALoginID == id)
				{
					return;
				}
				this.mTGALoginID = id;
				TDAnalytics.Login(id, "");
				this.IsLogin = true;
			}

			public void Logout()
			{
				if (string.IsNullOrEmpty(this.mTGALoginID))
				{
					return;
				}
				TDAnalytics.Logout("");
				this.mTGALoginID = string.Empty;
				this.IsLogin = false;
			}

			public void Track(string eventName, Dictionary<string, object> properties, bool isForce = true)
			{
				this.AddAFID(properties);
				this.AddSignature(properties);
				if (!this.isPost && !isForce)
				{
					TGATrackData tgatrackData = new TGATrackData(eventName, properties);
					this.trackList.Add(tgatrackData);
					return;
				}
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				if (properties == null)
				{
					properties = new Dictionary<string, object>();
				}
				this.SetDateTime(properties);
				TDAnalytics.Track(eventName, properties, "");
			}

			public void Track(string eventName)
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.Track(eventName, dictionary, true);
			}

			public void TrackFirstEvent(string eventName, Dictionary<string, object> properties, string firstEventID = "")
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				this.AddAFID(properties);
				this.AddSignature(properties);
				this.SetDateTime(properties);
				if (!string.IsNullOrEmpty(firstEventID))
				{
					TDAnalytics.Track(new TDFirstEventModel(eventName, firstEventID)
					{
						Properties = properties
					}, "");
					return;
				}
				TDAnalytics.Track(new TDFirstEventModel(eventName)
				{
					Properties = properties
				}, "");
			}

			public void AddAFID(Dictionary<string, object> dic)
			{
				if (dic == null)
				{
					return;
				}
				if (!string.IsNullOrEmpty(GameApp.SDK.AppsFlyerSDK.GetAppsFlyerId()))
				{
					dic["af_id"] = GameApp.SDK.AppsFlyerSDK.GetAppsFlyerId();
				}
			}

			public void AddSignature(Dictionary<string, object> dic)
			{
				if (dic == null)
				{
					dic = new Dictionary<string, object>();
				}
				string signature = this.Signature;
				if (!string.IsNullOrEmpty(signature))
				{
					dic["signature"] = signature;
				}
			}

			public void SetDateTime(Dictionary<string, object> properties)
			{
				DateTime dateTime = DateTime.UtcNow;
				if (!properties.ContainsKey("event_time_utc"))
				{
					properties.Add("event_time_utc", dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
				}
				dateTime = dateTime.AddHours(8.0);
				if (!properties.ContainsKey("event_time_beijing"))
				{
					properties.Add("event_time_beijing", dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
				}
				if (!properties.ContainsKey("guid"))
				{
					properties.Add("guid", Guid.NewGuid().ToString());
				}
				if (!properties.ContainsKey("is_release"))
				{
					properties.Add("is_release", GameApp.SDK.IsRelease ? "1" : "0");
				}
			}

			public void ClearSuperProperties()
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				TDAnalytics.ClearSuperProperties("");
			}

			public void SetSuperProperties(Dictionary<string, object> superProperties)
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				TDAnalytics.SetSuperProperties(superProperties, "");
			}

			public void TimeEvent(string eventName)
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				TDAnalytics.TimeEvent(eventName, "");
			}

			public void UnsetSuperProperty(string superPropertyName)
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				TDAnalytics.UnsetSuperProperty(superPropertyName, "");
			}

			public void User_add(Dictionary<string, object> properties)
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				this.SetDateTime(properties);
				TDAnalytics.UserAdd(properties, "");
			}

			public void User_add(string propertyKey, double propertyValue)
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary[propertyKey] = propertyValue;
				this.User_add(dictionary);
			}

			public void User_delete()
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				TDAnalytics.UserDelete("");
			}

			public void User_set(Dictionary<string, object> properties, bool setBaseProperties = false)
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				this.SetDateTime(properties);
				if (setBaseProperties)
				{
					TDPresetProperties presetProperties = TDAnalytics.GetPresetProperties("");
					if (presetProperties != null)
					{
						properties["device_id"] = presetProperties.DeviceId;
						properties["bundle_id"] = presetProperties.BundleId;
						properties["app_version"] = presetProperties.AppVersion;
					}
					CultureInfo currentCulture = CultureInfo.CurrentCulture;
					if (currentCulture != null)
					{
						string twoLetterISORegionName = new RegionInfo(currentCulture.LCID).TwoLetterISORegionName;
						properties["country_code"] = twoLetterISORegionName;
					}
					properties["version_code"] = GameApp.Config.GetString("VersionCode");
					properties["af_id"] = GameApp.SDK.AppsFlyerSDK.GetAppsFlyerId();
					string signature = this.Signature;
					if (!string.IsNullOrEmpty(signature))
					{
						properties["signature"] = signature;
					}
				}
				if (properties.Count > 0)
				{
					TDAnalytics.UserSet(properties, "");
				}
			}

			public void User_setOnce(Dictionary<string, object> properties)
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				this.SetDateTime(properties);
				TDAnalytics.UserSetOnce(properties, "");
			}

			public string GetDeviceID()
			{
				return TDAnalytics.GetDeviceId();
			}

			public string GetDistinctID()
			{
				return TDAnalytics.GetDistinctId("");
			}

			public void AppInstall()
			{
				if (PlayerPrefs.GetInt("TGA_AppInstall", 0) == 0)
				{
					this.Track("app_install");
					PlayerPrefs.SetInt("TGA_AppInstall", 1);
				}
			}

			public void AppStart(Dictionary<string, object> dic = null)
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				if (this.m_AppStartTime != DateTime.MinValue)
				{
					return;
				}
				if (dic != null)
				{
					dic["resume_from_background"] = SDKManager.SDKTGA.ResumeFromBackground;
					this.Track("app_start", dic, true);
				}
				else
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary["resume_from_background"] = SDKManager.SDKTGA.ResumeFromBackground;
					this.Track("app_start", dictionary, true);
				}
				this.OnAppStart();
			}

			public void AppEnd(Dictionary<string, object> dic = null)
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				if (this.m_AppStartTime == DateTime.MinValue)
				{
					return;
				}
				if (dic != null)
				{
					dic["duration"] = this.AppRunSeconds();
					this.Track("app_end", dic, true);
				}
				else
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary["duration"] = this.AppRunSeconds();
					this.Track("app_end", dictionary, true);
				}
				this.OnAppEnd();
			}

			public void OnAppStart()
			{
				if (this.m_AppStartTime == DateTime.MinValue)
				{
					this.m_AppStartTime = DateTime.Now;
				}
			}

			public void OnAppEnd()
			{
				this.m_AppStartTime = DateTime.MinValue;
			}

			public int AppRunSeconds()
			{
				if (this.m_AppStartTime == DateTime.MinValue)
				{
					return 0;
				}
				return (int)(DateTime.Now - this.m_AppStartTime).TotalSeconds;
			}

			public void TrackLogin(string step, Dictionary<string, object> dic = null)
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				Dictionary<string, object> dictionary = ((dic == null) ? new Dictionary<string, object>() : dic);
				dictionary["step"] = step;
				this.Track("log_in", dictionary, true);
			}

			public void TrackHotUpdate(string step, long size, string hotVersion, string errorCode = "")
			{
				if (!this.IsThinkingInitSuccess)
				{
					return;
				}
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["step"] = step;
				if (!(step == "start"))
				{
					if (step == "success")
					{
						if (this.TimeConsuming > 0f)
						{
							dictionary["update_duration"] = ((int)(Time.realtimeSinceStartup - this.TimeConsuming)).ToString();
						}
					}
				}
				else
				{
					this.TimeConsuming = Time.realtimeSinceStartup;
					dictionary["update_filesize"] = (int)(size / 1024L / 1024L);
				}
				if (!string.IsNullOrEmpty(hotVersion))
				{
					dictionary["hot_version_new"] = hotVersion;
				}
				if (!string.IsNullOrEmpty(errorCode))
				{
					dictionary["error_code"] = errorCode;
				}
				this.Track("hot_update", dictionary, true);
			}

			private void ChangeState(int state)
			{
				if (state != 103)
				{
					this.isPost = false;
					return;
				}
				this.isPost = true;
				this.PostList();
			}

			private void PostList()
			{
				foreach (TGATrackData tgatrackData in this.trackList)
				{
					this.Track(tgatrackData.key, tgatrackData.dic, true);
				}
				this.trackList.Clear();
			}

			private SDKManager mMgr;

			private string mTGALoginID = "";

			private string mSignature = "";

			private const string Key_AppInstall = "TGA_AppInstall";

			public static bool ResumeFromBackground;

			private DateTime m_AppStartTime = DateTime.MinValue;

			private bool isPost;

			private List<TGATrackData> trackList = new List<TGATrackData>();

			private float TimeConsuming;

			private const string HotUpdateStepStart = "start";

			private const string HotUpdateStepEnd = "success";
		}

		public class HabbyWebGameAPI
		{
			public bool IsSDKInited { get; private set; }

			public bool IsSDKLogin { get; private set; }

			public void StartLogin(Action<bool, string> callback)
			{
				SingletonScript<WebGameManager>.Instance.WebGameAPI.Login(delegate(WebGameLoginRsp rsp)
				{
					this.IsSDKLogin = rsp.isSuccess;
					Action<bool, string> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(rsp.isSuccess, rsp.isSuccess ? rsp.openId : rsp.errorMessage);
				});
			}

			public void Purchase(Action<bool, string> callback)
			{
				SingletonScript<WebGameManager>.Instance.WebGameAPI.Login(delegate(WebGameLoginRsp rsp)
				{
					this.IsSDKLogin = rsp.isSuccess;
					Action<bool, string> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(rsp.isSuccess, rsp.isSuccess ? rsp.authCode : rsp.errorMessage);
				});
			}
		}

		public class SDKWebView
		{
			public int ServerUserid { get; private set; }

			public string AccountID { get; private set; } = "";

			public SDKWebView(SDKManager mgr)
			{
				this.mMgr = mgr;
			}

			public void OpenFeedbackWebview(string url, Action<bool> onFinished = null)
			{
				if (!GameApp.NetWork.IsNetConnect)
				{
					if (onFinished != null)
					{
						onFinished(false);
					}
					return;
				}
				if (this._gameFeedbackWebview != null)
				{
					this._gameFeedbackWebview.Clean();
					this._gameFeedbackWebview = null;
				}
				this._gameFeedbackWebview = new GameFeedbackWebview(url);
				this._gameFeedbackWebview.OnClose = onFinished;
				this._gameFeedbackWebview.Show();
			}

			public void SetServerUserid(int serverUserid)
			{
				this.ServerUserid = serverUserid;
			}

			public void SetAccountID(string accountid)
			{
				this.AccountID = accountid;
			}

			private SDKManager mMgr;

			private GameFeedbackWebview _gameFeedbackWebview;
		}
	}
}
