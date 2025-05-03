using System;
using System.Collections.Generic;
using System.Text;
using Framework;
using Framework.Platfrom;
using GoogleMobileAds.Ump.Api;
using Newtonsoft.Json;
using UnityEngine;

namespace Habby.Ads
{
	public class AdsRequestHelper : MonoBehaviour
	{
		internal static AdsRequestHelper.RemoteLogLevel remoteLogLevel
		{
			get
			{
				return (AdsRequestHelper.RemoteLogLevel)AdsRequestHelper.get_ext_config().remote_log_level;
			}
		}

		public static bool isInitialized()
		{
			return AdsRequestHelper.inst != null;
		}

		public static bool isActive()
		{
			return false | AdsRequestHelper.isActiveMax();
		}

		public static void showDebugger(int debuggerType)
		{
			AdsRequestHelper.showAdmobDebug();
			AdsRequestHelper.showMaxDebug();
		}

		protected internal static void setGDPRConsentGranted()
		{
			AdsRequestHelper.setAdmobGDPRConsentGranted();
			AdsRequestHelper.setMaxGDPRConsentGranted();
			AdsRequestHelper.inst.gdprConsentGranted = true;
		}

		public static void Init(GameAdsConfigInterface gameAdsConfigInterface, Action onComplete = null)
		{
			if (AdsRequestHelper.inst != null)
			{
				return;
			}
			try
			{
				AdsRequestHelper.InitAdsConfig();
				GameObject gameObject = new GameObject("AdsHelperObject", new Type[] { typeof(AdsRequestHelper) });
				Object.DontDestroyOnLoad(gameObject);
				AdsRequestHelper.inst = gameObject.GetComponent<AdsRequestHelper>();
				AdsRequestHelper.inst.config = (gameAdsConfigInterface.isTest() ? gameAdsConfigInterface.GetDebugConfig() : gameAdsConfigInterface.GetProductionConfig());
				AdsRequestHelper.initALMax(gameAdsConfigInterface, onComplete);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		internal AdsRequestHelper.AdsAdapter getInterstitialAdapterInternal(int adapterId)
		{
			if (this.interstitialAdapters[adapterId] == null)
			{
				if (this.interstitialDrivers == null)
				{
					this.interstitialDrivers = new Dictionary<char, BaseDriver>();
					foreach (AdsRequestHelper.AdsSlot adsSlot in this.config.interstitialSlots)
					{
						if (adsSlot.mediation == AdsRequestHelper.AdsMediationType.WECHAT)
						{
							this.interstitialDrivers[adsSlot.code] = new WrappedDriver(adsSlot.code, new WeChatMiniRewardDriver(adsSlot.slotId));
						}
						AdsRequestHelper.AdsMediationType mediation = adsSlot.mediation;
						if (adsSlot.mediation == AdsRequestHelper.AdsMediationType.ALMAX)
						{
							this.interstitialDrivers[adsSlot.code] = new WrappedDriver(adsSlot.code, new ALMaxInterstitialDriver(adsSlot.slotId));
						}
					}
				}
				if (string.IsNullOrEmpty(this.config.interstitial[adapterId].slots))
				{
					List<string> list = new List<string>();
					foreach (char c in this.interstitialDrivers.Keys)
					{
						list.Add(c.ToString());
					}
					this.config.interstitial[adapterId].slots = string.Join(",", list.ToArray());
				}
				this.interstitialAdapters[adapterId] = new AdsRequestHelper.WrappedAdapter(new CombinedDriver(this.interstitialDrivers, this.config.interstitial[adapterId].slots));
			}
			return this.interstitialAdapters[adapterId];
		}

		internal static AdsRequestHelper.AdsAdapter getInterstitialAdapter(int adapterId = 0)
		{
			if (adapterId < 0 || adapterId >= 5)
			{
				return null;
			}
			if (AdsRequestHelper.inst == null)
			{
				if (AdsRequestHelper.interstitialAdaptersDummy[adapterId] == null)
				{
					AdsRequestHelper.interstitialAdaptersDummy[adapterId] = new AdsRequestHelper.DummyAdapter();
				}
				return AdsRequestHelper.interstitialAdaptersDummy[adapterId];
			}
			AdsRequestHelper.AdsAdapter adsAdapter;
			try
			{
				if (AdsRequestHelper.interstitialAdaptersDummy[adapterId] != null)
				{
					AdsRequestHelper.interstitialAdaptersDummy[adapterId].SetAdapter(AdsRequestHelper.inst.getInterstitialAdapterInternal(adapterId));
				}
				adsAdapter = AdsRequestHelper.inst.getInterstitialAdapterInternal(adapterId);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				if (AdsRequestHelper.interstitialAdaptersDummy[adapterId] == null)
				{
					AdsRequestHelper.interstitialAdaptersDummy[adapterId] = new AdsRequestHelper.DummyAdapter();
				}
				adsAdapter = AdsRequestHelper.interstitialAdaptersDummy[adapterId];
			}
			return adsAdapter;
		}

		internal AdsRequestHelper.AdsAdapter getRewardedAdapterInternal(int adapterId)
		{
			if (this.rewardedAdapters[adapterId] == null)
			{
				if (this.rewardedDrivers == null)
				{
					this.rewardedDrivers = new Dictionary<char, BaseDriver>();
					foreach (AdsRequestHelper.AdsSlot adsSlot in this.config.rewardedSlots)
					{
						AdsRequestHelper.AdsMediationType mediation = adsSlot.mediation;
						if (adsSlot.mediation == AdsRequestHelper.AdsMediationType.ALMAX)
						{
							this.rewardedDrivers[adsSlot.code] = new WrappedDriver(adsSlot.code, new ALMaxRewardedDriver(adsSlot.slotId));
						}
						if (adsSlot.mediation == AdsRequestHelper.AdsMediationType.WECHAT)
						{
							this.rewardedDrivers[adsSlot.code] = new WrappedDriver(adsSlot.code, new WeChatMiniRewardDriver(adsSlot.slotId));
						}
					}
				}
				if (string.IsNullOrEmpty(this.config.rewarded[adapterId].slots))
				{
					List<string> list = new List<string>();
					foreach (char c in this.rewardedDrivers.Keys)
					{
						list.Add(c.ToString());
					}
					this.config.rewarded[adapterId].slots = string.Join(",", list.ToArray());
				}
				this.rewardedAdapters[adapterId] = new AdsRequestHelper.WrappedAdapter(new CombinedDriver(this.rewardedDrivers, this.config.rewarded[adapterId].slots));
			}
			return this.rewardedAdapters[adapterId];
		}

		internal static AdsRequestHelper.AdsAdapter getRewardedAdapter(int adapterId = 0)
		{
			if (adapterId < 0 || adapterId >= 5)
			{
				return null;
			}
			if (AdsRequestHelper.inst == null)
			{
				if (AdsRequestHelper.rewardedAdaptersDummy[adapterId] == null)
				{
					AdsRequestHelper.rewardedAdaptersDummy[adapterId] = new AdsRequestHelper.DummyAdapter();
				}
				return AdsRequestHelper.rewardedAdaptersDummy[adapterId];
			}
			AdsRequestHelper.AdsAdapter adsAdapter;
			try
			{
				if (AdsRequestHelper.rewardedAdaptersDummy[adapterId] != null)
				{
					AdsRequestHelper.rewardedAdaptersDummy[adapterId].SetAdapter(AdsRequestHelper.inst.getRewardedAdapterInternal(adapterId));
				}
				adsAdapter = AdsRequestHelper.inst.getRewardedAdapterInternal(adapterId);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				if (AdsRequestHelper.rewardedAdaptersDummy[adapterId] == null)
				{
					AdsRequestHelper.rewardedAdaptersDummy[adapterId] = new AdsRequestHelper.DummyAdapter();
				}
				adsAdapter = AdsRequestHelper.rewardedAdaptersDummy[adapterId];
			}
			return adsAdapter;
		}

		protected static bool checkInterstitialSlotAvailability(char slotCode)
		{
			return AdsRequestHelper.inst != null && AdsRequestHelper.inst.interstitialDrivers != null && AdsRequestHelper.inst.interstitialDrivers.ContainsKey(slotCode) && AdsRequestHelper.inst.interstitialDrivers[slotCode].isLoaded();
		}

		protected static bool checkRewardedSlotAvailability(char slotCode)
		{
			return AdsRequestHelper.inst != null && AdsRequestHelper.inst.rewardedDrivers != null && AdsRequestHelper.inst.rewardedDrivers.ContainsKey(slotCode) && AdsRequestHelper.inst.rewardedDrivers[slotCode].isLoaded();
		}

		public static string getAdSource()
		{
			if (AdsRequestHelper.inst != null)
			{
				return AdsRequestHelper.inst.adSource;
			}
			return string.Empty;
		}

		public static AdsRequestHelper Instance
		{
			get
			{
				if (!AdsRequestHelper.instance)
				{
					AdsRequestHelper.instance = Object.FindObjectOfType<AdsRequestHelper>();
				}
				return AdsRequestHelper.instance;
			}
		}

		private void Update()
		{
			int num = 600;
			try
			{
				this.frame++;
				this.queue.Update();
				if (this.frame >= num)
				{
					this.frame = 0;
					if (GameApp.State.GetCurrentStateName() == 103)
					{
						this.RefreshAds();
					}
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public void RefreshAds()
		{
			Dictionary<char, AdsRequestHelper.RequestLoop>.Enumerator enumerator = this.loops.GetEnumerator();
			while (enumerator.MoveNext())
			{
				AdsRequestHelper.AdsConfigData ext_config = AdsRequestHelper.get_ext_config();
				KeyValuePair<char, AdsRequestHelper.RequestLoop> keyValuePair = enumerator.Current;
				AdsRequestHelper.AdsItemConfigData adsItemConfigData = ext_config.get_slot_config(keyValuePair.Key);
				if (adsItemConfigData.enabled)
				{
					keyValuePair = enumerator.Current;
					keyValuePair.Value.checkRequest(adsItemConfigData, adsItemConfigData.is_high && AdsRequestHelper.send_high);
				}
			}
			AdsRequestHelper.send_high = false;
		}

		private static void initAdmob(GameAdsConfigInterface gameAdsConfigInterface, Action onComplete = null)
		{
		}

		private static void setAdmobGDPRConsentGranted()
		{
		}

		public static void InitUmp(GameAdsConfigInterface gameAdsConfigInterface, bool isShowConsent = true)
		{
			AdsRequestHelper.isshowConsent = isShowConsent;
			ConsentRequestParameters consentRequestParameters = new ConsentRequestParameters
			{
				TagForUnderAgeOfConsent = false
			};
			if (gameAdsConfigInterface.isTest())
			{
				ConsentInformation.Reset();
				ConsentDebugSettings consentDebugSettings = new ConsentDebugSettings
				{
					DebugGeography = 1,
					TestDeviceHashedIds = gameAdsConfigInterface.GetTestDeviceId()
				};
				consentRequestParameters.ConsentDebugSettings = consentDebugSettings;
			}
			ConsentInformation.Update(consentRequestParameters, new Action<FormError>(AdsRequestHelper.OnConsentInfoUpdated));
		}

		private static void OnConsentInfoUpdated(FormError consentError)
		{
			if (consentError != null)
			{
				return;
			}
			if (!AdsRequestHelper.isshowConsent)
			{
				return;
			}
			ConsentForm.LoadAndShowConsentFormIfRequired(delegate(FormError formError)
			{
				if (formError != null)
				{
					return;
				}
				GameApp.SDK.AppsFlyerSDK.setConsentData();
				AdsRequestHelper.setMaxGDPRConsentGranted();
			});
		}

		private static void InitFirebaseConsent(bool consent)
		{
		}

		private static void showAdmobDebug()
		{
		}

		private static bool isActiveAdmob()
		{
			return false;
		}

		private static void initALMax(GameAdsConfigInterface gameAdsConfigInterface, Action onComplete = null)
		{
			AdsRequestHelper.AdsConfiguration adsConfiguration;
			if (gameAdsConfigInterface.isTest())
			{
				adsConfiguration = gameAdsConfigInterface.GetDebugConfig();
				MaxSdkAndroid.SetVerboseLogging(true);
			}
			else
			{
				adsConfiguration = gameAdsConfigInterface.GetProductionConfig();
			}
			Action <>9__2;
			MaxSdkCallbacks.OnSdkInitializedEvent += delegate(MaxSdkBase.SdkConfiguration sdkConfiguration)
			{
				AdsRequestHelper.MsgQueue msgQueue = AdsRequestHelper.inst.queue;
				Action action2;
				if ((action2 = <>9__2) == null)
				{
					action2 = (<>9__2 = delegate
					{
						Action onComplete2 = onComplete;
						if (onComplete2 == null)
						{
							return;
						}
						onComplete2();
					});
				}
				msgQueue.Run(action2);
			};
			Action<string, MaxSdkBase.AdInfo> action = delegate(string adUnit, MaxSdkBase.AdInfo adInfo)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("Placement", adInfo.Placement);
				dictionary.Add("Revenue", adInfo.Revenue);
				dictionary.Add("CreativeIdentifier", adInfo.CreativeIdentifier);
				dictionary.Add("NetworkName", adInfo.NetworkName);
				dictionary.Add("NetworkPlacement", adInfo.NetworkPlacement);
				dictionary.Add("AdUnitIdentifier", adInfo.AdUnitIdentifier);
			};
			MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += action;
			MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += action;
			MaxSdkAndroid.SetSdkKey("stPphgZDgaleMDg6AP4lhFTIrZiB3Bzv3Nhm3FzkriMBw2I5RMr7bNPQy6b_IZvMyb6_s4ign4th69oXWcf6_4");
			MaxSdkAndroid.SetExtraParameter("disable_auto_retry_ad_formats", "INTER,REWARDED");
			List<string> list = new List<string>();
			if (adsConfiguration.interstitialSlots != null)
			{
				foreach (AdsRequestHelper.AdsSlot adsSlot in adsConfiguration.interstitialSlots)
				{
					if (adsSlot.mediation == AdsRequestHelper.AdsMediationType.ALMAX)
					{
						list.Add(adsSlot.slotId);
					}
				}
			}
			if (adsConfiguration.rewardedSlots != null)
			{
				foreach (AdsRequestHelper.AdsSlot adsSlot2 in adsConfiguration.rewardedSlots)
				{
					if (adsSlot2.mediation == AdsRequestHelper.AdsMediationType.ALMAX)
					{
						list.Add(adsSlot2.slotId);
					}
				}
			}
			string text = string.Join<string>(',', list);
			if (!string.IsNullOrEmpty(text))
			{
				MaxSdkAndroid.SetExtraParameter("disable_b2b_ad_unit_ids", text);
			}
			MaxSdkAndroid.InitializeSdk(null);
		}

		private static void setMaxGDPRConsentGranted()
		{
			MaxSdkAndroid.SetHasUserConsent(true);
		}

		private static void showMaxDebug()
		{
			MaxSdkAndroid.ShowMediationDebugger();
		}

		private static bool isActiveMax()
		{
			return MaxSdkAndroid.IsInitialized();
		}

		public static void enter_battle()
		{
			AdsRequestHelper.send_high = true;
		}

		public static void exit_battle()
		{
			AdsRequestHelper.send_high = true;
		}

		public static void rewarded_addcallback(AdsCallback callback, int adapterId = 0)
		{
			AdsRequestHelper.getRewardedAdapter(adapterId).AddCallback(callback);
		}

		public static void rewarded_removecallback(AdsCallback callback, int adapterId = 0)
		{
			AdsRequestHelper.getRewardedAdapter(adapterId).RemoveCallback(callback);
		}

		public static bool rewarded_ads_isLoaded(int adapterId)
		{
			return AdsRequestHelper.getRewardedAdapter(adapterId).isLoaded();
		}

		public static void rewarded_ads_show(int adapterId, AdsCallback callback, int source = 0)
		{
			AdsRequestHelper.ad_source = source;
			AdsRequestHelper.getRewardedAdapter(adapterId).Show(callback);
		}

		public static bool rewarded_high_eCPM_isLoaded()
		{
			return AdsRequestHelper.rewarded_ads_isLoaded(1);
		}

		public static void rewarded_high_eCPM_show(AdsCallback callback, int source = 0)
		{
			AdsRequestHelper.rewarded_ads_show(1, callback, source);
		}

		protected static AdsRequestHelper.AdsConfigData get_ext_config()
		{
			if (AdsRequestHelper.ads_config == null)
			{
				if (AdsRequestHelper.default_ads_config == null)
				{
					AdsRequestHelper.default_ads_config = new AdsRequestHelper.AdsConfigData();
				}
				return AdsRequestHelper.default_ads_config;
			}
			return AdsRequestHelper.ads_config;
		}

		protected static void InitAdsConfig()
		{
			try
			{
				string @string = PlayerPrefsExpand.GetString("ads_config");
				if (!string.IsNullOrEmpty(@string))
				{
					AdsRequestHelper.ads_config = JsonConvert.DeserializeObject<AdsRequestHelper.AdsConfigData>(@string);
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public static void UpdateAdsConfig(AdsRequestHelper.AdsConfigData config)
		{
			try
			{
				AdsRequestHelper.ads_config = config;
				if (config == null)
				{
					PlayerPrefsExpand.DeleteKey("ads_config");
				}
				else
				{
					PlayerPrefsExpand.SetString("ads_config", JsonConvert.SerializeObject(config));
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		protected string GetLoadedRewardAdsSlots()
		{
			if (this.rewardedDrivers == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<char, BaseDriver> keyValuePair in this.rewardedDrivers)
			{
				if (keyValuePair.Value.isLoaded())
				{
					stringBuilder.Append(keyValuePair.Key);
				}
			}
			return stringBuilder.ToString();
		}

		public static string GetLoadedRewardAdsSlotsStatic()
		{
			if (AdsRequestHelper.inst == null)
			{
				return null;
			}
			return AdsRequestHelper.inst.GetLoadedRewardAdsSlots();
		}

		internal static void RemoteLog(string logAdUnit, string logEvent, bool isInterstitial, string errMsg, int tryCount, float duration, string requestId, string networkName)
		{
			try
			{
				DateTime adTime = DateTime.UtcNow;
				if (AdsRequestHelper.inst != null && AdsRequestHelper.inst.queue != null)
				{
					AdsRequestHelper.inst.queue.Run(delegate
					{
						string text = AdsRequestHelper.ad_source.ToString();
						if (logEvent.Equals("load") || logEvent.Equals("init") || logEvent.Equals("initNet") || logEvent.Equals("loadFail") || logEvent.Equals("request"))
						{
							text = null;
						}
						AdsTgaUtil.send_ad_debug(adTime, logAdUnit, logEvent, isInterstitial, errMsg, tryCount, duration, requestId, networkName, text);
					});
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		private static void initAdWebGame(GameAdsConfigInterface gameAdsConfigInterface, Action onComplete = null)
		{
		}

		internal static AdsRequestHelper inst;

		private AdsRequestHelper.AdsConfiguration config;

		internal bool gdprConsentGranted;

		internal AdsRequestHelper.MsgQueue queue = new AdsRequestHelper.MsgQueue();

		internal Dictionary<char, AdsRequestHelper.RequestLoop> loops = new Dictionary<char, AdsRequestHelper.RequestLoop>();

		public string adSource = "";

		private const int MAX_ADAPTER_COUNT = 5;

		private Dictionary<char, BaseDriver> interstitialDrivers;

		private Dictionary<char, BaseDriver> rewardedDrivers;

		private AdsRequestHelper.AdsAdapter[] interstitialAdapters = new AdsRequestHelper.AdsAdapter[5];

		private AdsRequestHelper.AdsAdapter[] rewardedAdapters = new AdsRequestHelper.AdsAdapter[5];

		private static AdsRequestHelper.DummyAdapter[] interstitialAdaptersDummy = new AdsRequestHelper.DummyAdapter[5];

		private static AdsRequestHelper.DummyAdapter[] rewardedAdaptersDummy = new AdsRequestHelper.DummyAdapter[5];

		private int frame;

		private static AdsRequestHelper instance = null;

		private static bool isshowConsent = true;

		public static bool send_high = true;

		protected static int ad_source;

		private static AdsRequestHelper.AdsConfigData default_ads_config = null;

		private const string KEY_ADS_CONFIG = "ads_config";

		private static AdsRequestHelper.AdsConfigData ads_config;

		internal class RequestLoop
		{
			public RequestLoop(AdsRequestHelper.RequestLoop.DoRequest doRequest, AdsRequestHelper.RequestLoop.CheckLoaded checkLoaded, AdsRequestHelper.RequestLoop.ReportError reportError)
			{
				this.doRequest = doRequest;
				this.checkLoaded = checkLoaded;
				this.reportError = reportError;
			}

			public void Init()
			{
				this.Request(false);
			}

			private void Request(bool isRetry = false)
			{
				if (!isRetry)
				{
					this.retryCount = 0;
				}
				this.doRequest();
				this.lastRequestTime = Time.realtimeSinceStartup;
				this.status = AdsRequestHelper.RequestLoop.Status.REQUESTING;
			}

			public void checkRequest(AdsRequestHelper.AdsItemConfigData slotCfg, bool ignoreRetryLimit = false)
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				switch (this.status)
				{
				case AdsRequestHelper.RequestLoop.Status.REQUESTING:
					if (this.lastRequestTime + 600f <= realtimeSinceStartup)
					{
						this.reportError("ADS Requesting Timeout");
						this.Request(false);
						return;
					}
					break;
				case AdsRequestHelper.RequestLoop.Status.FAILED:
					if (ignoreRetryLimit)
					{
						this.Request(false);
						return;
					}
					this.failCheck(slotCfg, realtimeSinceStartup);
					return;
				case AdsRequestHelper.RequestLoop.Status.LOADED:
					if (this.lastLoadedTime + 600f <= realtimeSinceStartup && !this.checkLoaded())
					{
						this.reportError("ADS Expired");
						this.Request(false);
						return;
					}
					break;
				case AdsRequestHelper.RequestLoop.Status.SHOWING:
					if (this.lastLoadedTime + 300f <= realtimeSinceStartup)
					{
						this.reportError("ADS Showing too long");
						this.Request(false);
					}
					break;
				default:
					return;
				}
			}

			protected void failCheck(AdsRequestHelper.AdsItemConfigData slotCfg, float time)
			{
				if (slotCfg.request_limit == -1 || this.retryCount < slotCfg.request_limit)
				{
					int num = (slotCfg.progressive_interval ? Mathf.Clamp((1 << this.retryCount) + slotCfg.request_interval_min, 0, slotCfg.request_interval_max) : slotCfg.request_interval_min);
					if (this.lastRequestTime + (float)num <= time)
					{
						this.retryCount++;
						this.Request(true);
					}
				}
			}

			public void onLoad()
			{
				this.lastLoadedTime = Time.realtimeSinceStartup;
				this.status = AdsRequestHelper.RequestLoop.Status.LOADED;
			}

			public void onFail()
			{
				this.status = AdsRequestHelper.RequestLoop.Status.FAILED;
			}

			public void onOpen()
			{
				this.lastLoadedTime = Time.realtimeSinceStartup;
				this.status = AdsRequestHelper.RequestLoop.Status.SHOWING;
			}

			public void onClose()
			{
				this.Request(false);
			}

			public bool isLoaded()
			{
				return this.status == AdsRequestHelper.RequestLoop.Status.LOADED;
			}

			private float lastRequestTime;

			private float lastLoadedTime;

			private const int MIN_REQUEST_INTERVAL = 10;

			private const int MAX_REQUEST_INTERVAL = 600;

			private const int MAX_LOADED_INTERVAL = 600;

			private const int MAX_SHOW_INTERVAL = 300;

			protected int retryCount;

			private AdsRequestHelper.RequestLoop.Status status;

			private AdsRequestHelper.RequestLoop.DoRequest doRequest;

			private AdsRequestHelper.RequestLoop.CheckLoaded checkLoaded;

			private AdsRequestHelper.RequestLoop.ReportError reportError;

			private enum Status
			{
				UNINITIALIZED,
				REQUESTING,
				FAILED,
				LOADED,
				SHOWING
			}

			public delegate void DoRequest();

			public delegate bool CheckLoaded();

			public delegate void ReportError(string error);
		}

		internal class MsgQueue
		{
			public void Update()
			{
				Action action = null;
				Queue<Action> executionQueue = this._executionQueue;
				lock (executionQueue)
				{
					if (this._executionQueue.Count > 0)
					{
						action = this._executionQueue.Dequeue();
					}
				}
				if (action != null)
				{
					action();
				}
			}

			public void Run(Action action)
			{
				Queue<Action> executionQueue = this._executionQueue;
				lock (executionQueue)
				{
					this._executionQueue.Enqueue(action);
				}
			}

			private readonly Queue<Action> _executionQueue = new Queue<Action>();
		}

		public enum RemoteLogLevel
		{
			NONE,
			IMPRESSION_ONLY,
			IMPRESSION_AND_FILL,
			ALL
		}

		internal interface AdsAdapter : AdsDriver, CallbackManager
		{
			bool Show(AdsCallback enabledCallback);

			bool Show(AdsCallback enabledCallback, string source);

			void UpdateConfig(string config);
		}

		internal class WrappedAdapter : AdsRequestHelper.AdsAdapter, AdsDriver, CallbackManager
		{
			public WrappedAdapter(BaseDriver driver)
			{
				this.driver = driver;
				driver.Init(this.callbacks);
			}

			public bool isLoaded()
			{
				bool flag;
				try
				{
					flag = this.driver.isLoaded();
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					flag = false;
				}
				return flag;
			}

			public bool isBusy()
			{
				bool flag;
				try
				{
					flag = this.driver.isBusy();
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					flag = false;
				}
				return flag;
			}

			public bool isPlaying()
			{
				bool flag;
				try
				{
					flag = this.driver.isPlaying();
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					flag = false;
				}
				return flag;
			}

			public bool Show()
			{
				return this.Show(null, string.Empty);
			}

			public bool Show(AdsCallback enabledCallback)
			{
				return this.Show(enabledCallback, string.Empty);
			}

			public bool Show(AdsCallback enabledCallback, string source)
			{
				bool flag;
				try
				{
					AdsRequestHelper.inst.adSource = source;
					this.callbacks.SetExclusiveCallback(enabledCallback);
					flag = this.driver.Show();
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
					flag = false;
				}
				return flag;
			}

			public void UpdateConfig(string config)
			{
				try
				{
					this.driver.updateConfig(config);
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
				}
			}

			public void AddCallback(AdsCallback callback)
			{
				this.callbacks.AddCallback(callback);
			}

			public void RemoveCallback(AdsCallback callback)
			{
				this.callbacks.RemoveCallback(callback);
			}

			private CallbackRouter callbacks = new CallbackRouter();

			private BaseDriver driver;
		}

		internal class DummyAdapter : AdsRequestHelper.AdsAdapter, AdsDriver, CallbackManager
		{
			public bool isLoaded()
			{
				return this.adapter != null && this.adapter.isLoaded();
			}

			public bool isBusy()
			{
				return this.adapter != null && this.adapter.isBusy();
			}

			public bool isPlaying()
			{
				return this.adapter != null && this.adapter.isPlaying();
			}

			public bool Show()
			{
				return this.adapter != null && this.adapter.Show();
			}

			public bool Show(AdsCallback enabledCallback)
			{
				return this.adapter != null && this.adapter.Show(enabledCallback);
			}

			public bool Show(AdsCallback enabledCallback, string source)
			{
				return this.adapter != null && this.adapter.Show(enabledCallback, source);
			}

			public void UpdateConfig(string config)
			{
				if (this.adapter != null)
				{
					this.adapter.UpdateConfig(config);
					return;
				}
				this.config = config;
			}

			public void AddCallback(AdsCallback callback)
			{
				try
				{
					if (this.adapter != null)
					{
						this.adapter.AddCallback(callback);
					}
					else
					{
						this.callbacks.Add(callback);
					}
				}
				catch (Exception ex)
				{
					HLog.LogException(ex);
				}
			}

			public void RemoveCallback(AdsCallback callback)
			{
				if (this.adapter != null)
				{
					this.adapter.RemoveCallback(callback);
					return;
				}
				this.callbacks.Remove(callback);
			}

			public void SetAdapter(AdsRequestHelper.AdsAdapter adapter)
			{
				foreach (AdsCallback adsCallback in this.callbacks)
				{
					adapter.AddCallback(adsCallback);
				}
				this.callbacks.Clear();
				if (this.config != null)
				{
					adapter.UpdateConfig(this.config);
					this.config = null;
				}
				this.adapter = adapter;
			}

			private List<AdsCallback> callbacks = new List<AdsCallback>();

			private AdsRequestHelper.AdsAdapter adapter;

			private string config;
		}

		public enum AdsMediationType
		{
			ADMOB,
			ALMAX,
			WECHAT
		}

		public struct AdsSlot
		{
			public char code;

			public AdsRequestHelper.AdsMediationType mediation;

			public string slotId;
		}

		public struct AdsPlacement
		{
			public string name;

			public string slots;
		}

		public struct AdsConfiguration
		{
			public string admobAppId;

			public string almaxAppId;

			public string pangleAppId;

			public bool isChina;

			public AdsRequestHelper.AdsSlot[] interstitialSlots;

			public AdsRequestHelper.AdsSlot[] rewardedSlots;

			public AdsRequestHelper.AdsPlacement[] interstitial;

			public AdsRequestHelper.AdsPlacement[] rewarded;
		}

		[Serializable]
		public class AdsItemConfigData
		{
			public int request_interval_min = 5;

			public bool progressive_interval = true;

			public int request_interval_max = 60;

			public bool is_high;

			public int request_limit = -1;

			public bool enabled = true;
		}

		[Serializable]
		public class AdsConfigData
		{
			public AdsRequestHelper.AdsItemConfigData get_slot_config(char slotId)
			{
				if (this.slots == null)
				{
					this.slots = new Dictionary<char, AdsRequestHelper.AdsItemConfigData>();
				}
				if (!this.slots.ContainsKey(slotId))
				{
					this.slots.Add(slotId, new AdsRequestHelper.AdsItemConfigData());
				}
				return this.slots[slotId];
			}

			public int remote_log_level = 3;

			public Dictionary<char, AdsRequestHelper.AdsItemConfigData> slots = new Dictionary<char, AdsRequestHelper.AdsItemConfigData>
			{
				{
					'H',
					new AdsRequestHelper.AdsItemConfigData
					{
						request_interval_min = 30,
						progressive_interval = true,
						request_interval_max = 120,
						request_limit = -1,
						is_high = true,
						enabled = true
					}
				},
				{
					'J',
					new AdsRequestHelper.AdsItemConfigData
					{
						request_interval_min = 30,
						progressive_interval = true,
						request_interval_max = 120,
						request_limit = -1,
						is_high = true,
						enabled = true
					}
				},
				{
					'K',
					new AdsRequestHelper.AdsItemConfigData
					{
						request_interval_min = 30,
						progressive_interval = true,
						request_interval_max = 120,
						request_limit = -1,
						is_high = true,
						enabled = true
					}
				},
				{
					'M',
					new AdsRequestHelper.AdsItemConfigData
					{
						request_interval_min = 5,
						progressive_interval = true,
						request_interval_max = 60,
						request_limit = -1,
						is_high = false,
						enabled = true
					}
				},
				{
					'N',
					new AdsRequestHelper.AdsItemConfigData
					{
						request_interval_min = 5,
						progressive_interval = true,
						request_interval_max = 60,
						request_limit = -1,
						is_high = false,
						enabled = true
					}
				}
			};
		}
	}
}
