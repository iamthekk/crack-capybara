using System;
using Framework.Coroutine;
using Framework.DataModule;
using Framework.DeepLink;
using Framework.DxxGuild;
using Framework.EventSystem;
using Framework.GameAppConfig;
using Framework.Http;
using Framework.Logic;
using Framework.Logic.Modules;
using Framework.Logic.UI;
using Framework.MailManager;
using Framework.NetWork;
using Framework.Platfrom;
using Framework.PurchaseManager;
using Framework.ResourcesModule;
using Framework.RunTimeManager;
using Framework.SceneModule;
using Framework.SDKManager;
using Framework.SocketNet;
using Framework.SoundModule;
using Framework.State;
using Framework.TableModule;
using Framework.UnityGlobalManager;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
	public class GameApp : MonoBehaviour
	{
		public static GameAppConfigManager Config { get; private set; }

		public static EventSystemManager Event { get; private set; }

		public static DataModuleManager Data { get; private set; }

		public static CoroutineManager CoroutineSystem { get; private set; }

		public static ViewModuleManager View { get; private set; }

		public static StateManager State { get; private set; }

		public static TableManager Table { get; private set; }

		public static ResourcesManager Resources { get; private set; }

		public static SceneManager Scene { get; private set; }

		public static AudioManager Sound { get; set; }

		public static HttpManager Http { get; set; }

		public static NetWorkManager NetWork { get; set; }

		public static RunTimeManager RunTime { get; private set; }

		public static SDKManager SDK { get; private set; }

		public static UnityGlobalManager UnityGlobal { get; private set; }

		public static GuildConfig GuildConfig { get; private set; }

		public static MailManager Mail { get; private set; }

		public static ISocketNet SocketNet { get; private set; }

		public static PurchaseManager Purchase { get; private set; }

		public static DeepLinkMain DeepLink { get; private set; }

		public static bool IsPause
		{
			get
			{
				return GameApp.m_pauseCount > 0;
			}
		}

		public static float TimeScale
		{
			get
			{
				return GameApp.m_timeScale;
			}
			private set
			{
				GameApp.m_timeScale = value;
			}
		}

		public void OnStarUp()
		{
			GameApp.Config = this.m_config;
			GameApp.Event = this.m_event;
			GameApp.Data = this.m_data;
			GameApp.CoroutineSystem = this.m_coroutine;
			GameApp.View = this.m_view;
			GameApp.State = this.m_state;
			GameApp.Table = this.m_table;
			GameApp.Resources = this.m_resources;
			GameApp.Scene = this.m_scene;
			GameApp.Sound = this.m_sound;
			GameApp.Sound.OnInit();
			GameApp.Http = this.m_http;
			GameApp.NetWork = this.m_netWork;
			GameApp.GuildConfig = this.m_guild;
			GameApp.Mail = this.m_mail;
			GameApp.SocketNet = new SocketNet_HabbyImAdapter();
			GameApp.RunTime = this.m_runTime;
			GameApp.SDK = this.m_sdk;
			this.m_sdk.Init();
			GameApp.UnityGlobal = this.m_unityGlobal;
			this.m_unityGlobal.OnInit();
			Utility.Vibration.Init();
			GameApp.Purchase = this.m_purchase;
			GameApp.DeepLink = this.m_deepLink;
			GameApp.Event.Init();
			GameApp.Data.Init();
			GameApp.View.Init();
			GameApp.State.Init();
			GameApp.RegisterAllMessage(GameApp.Event);
			GameApp.RegisterAllDataModules(GameApp.Data);
			GameApp.RegisterAllViewModules(GameApp.View);
			GameApp.RegisterAllStates(GameApp.State);
			this.OnStarupSetting();
		}

		public void OnUpdate()
		{
			GameApp.Event.OnUpdate(Time.deltaTime, Time.unscaledDeltaTime);
			GameApp.View.OnUpdate(Time.deltaTime, Time.unscaledDeltaTime);
			GameApp.State.OnUpdate(Time.deltaTime, Time.unscaledDeltaTime);
			GameApp.SDK.OnUpdate(Time.deltaTime, Time.unscaledDeltaTime);
		}

		public void OnLateUpdate()
		{
			GameApp.State.OnLateUpdate(Time.deltaTime, Time.unscaledDeltaTime);
		}

		public void OnFixedUpdate()
		{
			GameApp.RunTime.OnFixedUpdate();
		}

		public void OnShutdown()
		{
			this.m_view.CloseAllView(null);
			this.m_runTime.OnShutDown();
			this.m_data.UnRegisterAllDataModule(Array.Empty<int>());
			this.m_event.UnRegisterAllEvent();
			this.m_sound.OnDeInit();
			this.m_coroutine.RemoveAllTask();
			this.m_state.UnRegisterAllState(Array.Empty<int>());
			this.m_table.SetITableManager(null);
			this.m_unityGlobal.OnDeInit();
			if (GameApp.SocketNet != null)
			{
				GameApp.SocketNet.DeInit();
			}
			GameApp.SocketNet = null;
		}

		public async void OnRestart()
		{
			GameApp.View.Pool.m_checkAssetsUI.gameObject.SetActive(true);
			await TaskExpand.Delay(300);
			SceneManager.LoadSceneAsync("CheckAssets").completed += delegate(AsyncOperation s)
			{
				this.OnShutdown();
				this.OnStarUp();
			};
		}

		public void OnStarupSetting()
		{
			Singleton<LanguageManager>.Instance.CheckLanguage();
			CustomTextTool.SetCustomTextExtension();
			if (GameLauncher.Builder.m_isReadyLanguage)
			{
				EventArgLanguageType instance = Singleton<EventArgLanguageType>.Instance;
				instance.SetData(GameLauncher.Builder.m_languageType);
				GameApp.Event.DispatchNow(this, 1, instance);
			}
			bool @bool = GameApp.Config.GetBool("IsReleaseServer");
			GameApp.NetWork.m_netWorkUsingType = (@bool ? NetWorkUsingType.Release : NetWorkUsingType.Debug);
			GameApp.SDK.IsRelease = @bool;
			GameApp.Mail.m_serverType = (@bool ? MailServerType.Release : MailServerType.Debug);
			GameApp.State.ActiveState(StateName.CheckAssetsState);
		}

		private static void RegisterAllMessage(EventSystemManager events)
		{
		}

		private static void RegisterAllDataModules(DataModuleManager datas)
		{
			datas.RegisterDataModule(new LanguageDataModule());
		}

		private static void RegisterAllViewModules(ViewModuleManager views)
		{
			views.RegisterViewModule(new ViewModuleData(1, GameApp.View.Pool.m_checkAssetsUI, DestoryType.Dont));
			views.RegisterViewModule(new ViewModuleData(2, GameApp.View.Pool.m_netloadingUI, DestoryType.Dont));
		}

		private static void RegisterAllStates(StateManager states)
		{
			states.RegisterState(new CheckAssetsState());
		}

		public static void SetTimeScale(float value)
		{
			GameApp.m_timeScale = value;
			if (!GameApp.IsPause)
			{
				Time.timeScale = GameApp.m_timeScale;
			}
		}

		public static void SetPause(bool value)
		{
			GameApp.m_pauseCount += (value ? 1 : (-1));
			if (GameApp.m_pauseCount == 1 && value)
			{
				Time.timeScale = 0f;
				return;
			}
			if (GameApp.m_pauseCount == 0 && !value)
			{
				Time.timeScale = GameApp.m_timeScale;
			}
		}

		public static void Quit()
		{
			Application.Quit();
		}

		[SerializeField]
		public GameAppConfigManager m_config;

		[SerializeField]
		public EventSystemManager m_event;

		[SerializeField]
		private DataModuleManager m_data;

		[SerializeField]
		public CoroutineManager m_coroutine;

		[SerializeField]
		public ViewModuleManager m_view;

		[SerializeField]
		public StateManager m_state;

		[SerializeField]
		public TableManager m_table;

		[SerializeField]
		public ResourcesManager m_resources;

		[SerializeField]
		public SceneManager m_scene;

		[SerializeField]
		public AudioManager m_sound;

		[SerializeField]
		public HttpManager m_http;

		[SerializeField]
		public NetWorkManager m_netWork;

		[SerializeField]
		public RunTimeManager m_runTime;

		[SerializeField]
		public SDKManager m_sdk;

		[SerializeField]
		public UnityGlobalManager m_unityGlobal;

		[SerializeField]
		public GuildConfig m_guild;

		[SerializeField]
		public MailManager m_mail;

		[SerializeField]
		public PurchaseManager m_purchase;

		[SerializeField]
		public DeepLinkMain m_deepLink;

		private static int m_pauseCount = 0;

		private static float m_timeScale = 1f;
	}
}
