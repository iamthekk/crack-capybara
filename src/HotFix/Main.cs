using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Dxx.Chat;
using Framework;
using Framework.DataModule;
using Framework.DxxGuild;
using Framework.EventSystem;
using Framework.Logic.Modules;
using Framework.RunTimeManager;
using Framework.SDKManager;
using Framework.SocketNet;
using Framework.State;
using Framework.ViewModule;
using HotFix.GuildUI;
using LocalModels;
using Server;
using UnityEngine;
using UnityEngine.Scripting;

namespace HotFix
{
	public class Main
	{
		public void OnStarUp()
		{
			Input.multiTouchEnabled = false;
			Screen.sleepTimeout = -1;
			CultureInfo cultureInfo = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
			CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
			this.m_message.Register(GameApp.Event);
			this.m_dataModule.Register(GameApp.Data);
			this.m_viewModlue.RegisterViews(GameApp.View);
			this.m_state.Register(GameApp.State);
			Singleton<QualityManager>.Instance.InitQuality();
			this.RegisterNetwork();
			this.RegisterTable();
			this.RegisterMail();
			this.SocketNetInit();
			this.GuildSDKInit(GameApp.GuildConfig);
			this.ChatManagerInit();
			HotfixTestTool.RegistGameTest();
			this.mLastInFocusTime = DateTime.Now;
			GameServer.Init(GameApp.Table.GetManager());
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Logout_ClearData, new HandlerEvent(this.OnClearUserData));
		}

		[Preserve]
		public void OnUpdate()
		{
		}

		[Preserve]
		public void OnFixedUpdate()
		{
		}

		[Preserve]
		public void OnShutDown()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Logout_ClearData, new HandlerEvent(this.OnClearUserData));
			if (GameApp.NetWork != null)
			{
				GameApp.NetWork.SetNetworkManager(null);
			}
			if (GameApp.Table != null && GameApp.Table.GetManager() != null)
			{
				GameApp.Table.GetManager().DeInitialiseLocalModels();
			}
			if (GameApp.Mail != null)
			{
				GameApp.Mail.OnDeInit();
			}
			if (GameApp.Purchase != null)
			{
				GameApp.Purchase.OnDeInit();
			}
			if (GameApp.SocketNet != null)
			{
				this.SocketNetDeInit();
			}
			if (GameApp.UnityGlobal != null)
			{
				GameApp.UnityGlobal.UnLoad(null);
			}
			if (this.mGuildModule != null)
			{
				this.mGuildModule.Destroy();
				this.mGuildModule = null;
			}
			if (this.mChatManager != null)
			{
				this.mChatManager.Destroy();
				this.mChatManager = null;
			}
			if (RedPointController.Instance != null)
			{
				RedPointController.Instance.OnDelect();
			}
			if (GuideController.Instance != null)
			{
				GuideController.Instance.OnDelect();
			}
			if (GlobalUpdater.Instance != null)
			{
				GlobalUpdater.Instance.Dispose();
			}
			UIViewPlayerCamera.DestroyAll();
			GameAFTools.DestroyAF();
			GameTGATools.DestroyTGA();
			HotfixTestTool.UnRegistGameTest();
		}

		private void SocketNetDeInit()
		{
			GameApp.SocketNet.SetSocketGameHandler(null);
		}

		[Preserve]
		public void OnApplicationFocus(bool hasFocus)
		{
		}

		[Preserve]
		public void OnApplicationPause(bool pauseStatus)
		{
			if (!pauseStatus)
			{
				SDKManager sdk = GameApp.SDK;
				if (sdk == null)
				{
					return;
				}
				SDKManager.SDKTGA analyze = sdk.Analyze;
				if (analyze == null)
				{
					return;
				}
				analyze.Track_AppStart();
				return;
			}
			else
			{
				SDKManager sdk2 = GameApp.SDK;
				if (sdk2 == null)
				{
					return;
				}
				SDKManager.SDKTGA analyze2 = sdk2.Analyze;
				if (analyze2 == null)
				{
					return;
				}
				analyze2.Track_AppEnd();
				return;
			}
		}

		[Preserve]
		public void OnApplicationQuit()
		{
			SDKManager sdk = GameApp.SDK;
			if (sdk == null)
			{
				return;
			}
			SDKManager.SDKTGA analyze = sdk.Analyze;
			if (analyze == null)
			{
				return;
			}
			analyze.Track_AppEnd();
		}

		private void RegisterNetwork()
		{
			GameApp.NetWork.SetNetworkManager(new ProtoNetWorkManager());
		}

		private async void RegisterTable()
		{
			this.mLoadCount = 0;
			LocalModelManager localModelManager = new LocalModelManager();
			GameApp.Table.SetITableManager(localModelManager);
			localModelManager.InitialiseLocalModelsAsync();
			localModelManager.LoadAll(new Action(this.OnLoadTableAndGlobal));
			this.RegisterUnityGlobal();
		}

		[Conditional("ENABLE_PROFILER")]
		private void LoadingTable()
		{
			this.tableLoadStep = LoadingTime.MainStepIndex;
		}

		private void RegisterUnityGlobal()
		{
			GameApp.UnityGlobal.SetManager(new UserUnityGlobalManager());
			GameApp.UnityGlobal.Load(new Action(this.OnLoadTableAndGlobal));
		}

		private void RegisterMail()
		{
			GameApp.Mail.SetManager(new HabbyMailManager());
			GameApp.Mail.OnInit();
		}

		private void RegsterPurchase()
		{
			UnityPurchaseManager unityPurchaseManager = new UnityPurchaseManager();
			UnityPurchaseCaches unityPurchaseCaches = new UnityPurchaseCaches();
			GameApp.Purchase.SetCaches(unityPurchaseCaches);
			GameApp.Purchase.SetManager(unityPurchaseManager);
			GameApp.Purchase.OnInit();
		}

		private void OnLoadTableAndGlobal()
		{
			this.mLoadCount++;
			if (this.mLoadCount >= 2)
			{
				this.OnLoadFinished();
			}
		}

		private void OnLoadFinished()
		{
			GameApp.SDK.Analyze.TrackLogin("加载资源完成", null);
			Singleton<GameConfig>.Instance.InitTableData();
			this.RegsterPurchase();
			GlobalUpdater.OnCreate();
			RedPointController.OnCreate();
			GameTGATools.CreateTGA();
			GameAFTools.CreateAF();
			GameApp.State.ActiveState(101);
		}

		private void SocketNetInit()
		{
			if (GameApp.SocketNet is ISocketNetGame)
			{
				SocketNet_GameProxy socketNet_GameProxy = new SocketNet_GameProxy();
				GameApp.SocketNet.SetSocketGameHandler(socketNet_GameProxy);
				return;
			}
			HabbyIm_GameProxy habbyIm_GameProxy = new HabbyIm_GameProxy();
			GameApp.SocketNet.SetSocketGameHandler(habbyIm_GameProxy);
		}

		private void GuildSDKInit(GuildConfig config)
		{
			this.mGuildModule = new GameGuildModule();
			this.mGuildModule.Init(config);
		}

		private void ChatManagerInit()
		{
			this.mChatManager = Singleton<ChatManager>.Instance;
			this.mChatManager.Init();
		}

		private void OnClearUserData(object sender, int type, BaseEventArgs eventargs)
		{
			this.mGuildModule.Destroy();
			Singleton<ChatManager>.Instance.Clear();
			this.SocketNetInit();
			Singleton<ChatManager>.Instance.Init();
			this.GuildSDKInit(GameApp.GuildConfig);
		}

		public string GetLanguageInfoByID(LanguageType languageType, int id)
		{
			return this.GetLanguageInfoByID(languageType, id.ToString());
		}

		public string GetLanguageInfoByID(LanguageType languageType, string id)
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID(languageType, id);
		}

		private Main.DataModule m_dataModule = new Main.DataModule();

		private Main.ViewModule m_viewModlue = new Main.ViewModule();

		private Main.State m_state = new Main.State();

		private Main.Message m_message = new Main.Message();

		private GameGuildModule mGuildModule;

		private ChatManager mChatManager;

		public DateTime mLastInFocusTime;

		private int tableLoadStep = -1;

		private int mLoadCount;

		private class DataModule
		{
			private void OnRegister(DataModuleManager manager, IDataModule dataModule)
			{
				manager.RegisterDataModule(dataModule);
				GameApp.RunTime.AddIDConnecter(new RunTimeIDConnecterData(1, dataModule, dataModule.GetType()));
			}

			public void Register(DataModuleManager manager)
			{
				SettingDataModule settingDataModule = new SettingDataModule();
				this.OnRegister(manager, settingDataModule);
				settingDataModule.InitValue();
				this.OnRegister(manager, new LoginDataModule());
				this.OnRegister(manager, new CommonDataModule());
				this.OnRegister(manager, new GameDataModule());
				this.OnRegister(manager, new PropDataModule());
				this.OnRegister(manager, new HeroDataModule());
				this.OnRegister(manager, new EquipDataModule());
				this.OnRegister(manager, new PetDataModule());
				this.OnRegister(manager, new ChapterDataModule());
				this.OnRegister(manager, new BattleCrossArenaDataModule());
				this.OnRegister(manager, new BattleConquerDataModule());
				this.OnRegister(manager, new BattleGuildRankDataModule());
				this.OnRegister(manager, new BattleGuildBossDataModule());
				this.OnRegister(manager, new BattleTowerDataModule());
				this.OnRegister(manager, new HeroLevelUpDataModule());
				this.OnRegister(manager, new RelicDataModule());
				this.OnRegister(manager, new MainDataModule());
				this.OnRegister(manager, new MainCityDataModule());
				this.OnRegister(manager, new AddAttributeDataModule());
				this.OnRegister(manager, new RedPointDataModule());
				this.OnRegister(manager, new FunctionDataModule());
				this.OnRegister(manager, new BattleMainDataModule());
				this.OnRegister(manager, new SocialityDataModule());
				this.OnRegister(manager, new OtherMainCityDataModule());
				this.OnRegister(manager, new PlayerInformationDataModule());
				this.OnRegister(manager, new ConquerDataModule());
				this.OnRegister(manager, new ReportConquerDataModule());
				this.OnRegister(manager, new IAPDataModule());
				this.OnRegister(manager, new VIPDataModule());
				this.OnRegister(manager, new ShopDataModule());
				this.OnRegister(manager, new SignDataModule());
				this.OnRegister(manager, new TVRewardDataModule());
				this.OnRegister(manager, new MailDataModule());
				this.OnRegister(manager, new CrossArenaDataModule());
				this.OnRegister(manager, new TowerDataModule());
				this.OnRegister(manager, new ActivityCommonDataModule());
				this.OnRegister(manager, new ActivityWeekDataModule());
				this.OnRegister(manager, new ActTimeRankDataModule());
				this.OnRegister(manager, new ActivitySlotTrainDataModule());
				this.OnRegister(manager, new TaskDataModule());
				this.OnRegister(manager, new SevenDayCarnivalDataModule());
				this.OnRegister(manager, new WorldBossDataModule());
				this.OnRegister(manager, new RankDataModule());
				this.OnRegister(manager, new TicketDataModule());
				this.OnRegister(manager, new TicketDailyExchangeDataModule());
				this.OnRegister(manager, new TalentDataModule());
				this.OnRegister(manager, new CollectionDataModule());
				this.OnRegister(manager, new GuideDataModule());
				this.OnRegister(manager, new ChapterActivityDataModule());
				this.OnRegister(manager, new ChapterSweepDataModule());
				this.OnRegister(manager, new ChestDataModule());
				this.OnRegister(manager, new DungeonDataModule());
				this.OnRegister(manager, new AdDataModule());
				this.OnRegister(manager, new MountDataModule());
				this.OnRegister(manager, new ArtifactDataModule());
				this.OnRegister(manager, new RefreshDataModule());
				this.OnRegister(manager, new HangUpDataModule());
				this.OnRegister(manager, new PushGiftDataModule());
				this.OnRegister(manager, new ChainPacksDataModule());
				this.OnRegister(manager, new MiningDataModule());
				this.OnRegister(manager, new ClothesDataModule());
				this.OnRegister(manager, new SelectServerDataModule());
				this.OnRegister(manager, new RogueDungeonDataModule());
				this.OnRegister(manager, new ChapterBattlePassDataModule());
				this.OnRegister(manager, new TalentLegacyDataModule());
				this.OnRegister(manager, new NewWorldDataModule());
				this.OnRegister(manager, new ChapterActivityWheelDataModule());
				this.OnRegister(manager, new ChainPacksPushDataModule());
				this.OnRegister(manager, new DeepLinkDataModule());
			}
		}

		private class Message
		{
			public void Register(EventSystemManager events)
			{
			}
		}

		private class State
		{
			public void Register(StateManager states)
			{
				states.RegisterState(new LoginState());
				states.RegisterState(new FirstEnterWorldState());
				states.RegisterState(new MainState());
				states.RegisterState(new BattleChapterState());
				states.RegisterState(new BattleCrossArenaState());
				states.RegisterState(new BattleTowerState());
				states.RegisterState(new BattleGuildBossState());
				states.RegisterState(new BattleConquerState());
				states.RegisterState(new BattleGuildRankState());
				states.RegisterState(new BattleDungeonState());
				states.RegisterState(new BattleRogueDungeonState());
				states.RegisterState(new BattleWorldBossState());
				states.RegisterState(new BattleTestState());
			}
		}

		public class ViewModule
		{
			private string OnGetViewName(int viewName)
			{
				ViewName viewName2 = (ViewName)viewName;
				return viewName2.ToString();
			}

			public void RegisterViews(ViewModuleManager views)
			{
				views.m_funcAnalysisViewName = new Func<int, string>(this.OnGetViewName);
				views.RegisterViewModule(new ViewModuleData(101, null, "Assets/_Resources/Prefab/UI/Loading/UI_LoadingNew.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(104, null, "Assets/_Resources/Prefab/UI/Loading/UI_LoadingMain.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(105, null, "Assets/_Resources/Prefab/UI/Loading/UI_LoadingBattle.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(102, null, "Assets/_Resources/Prefab/UI/Tip/UI_Tip.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(103, null, "Assets/_Resources/Prefab/UI/Login/UI_Login.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(201, new MainViewModuleLoader(), "Assets/_Resources/Prefab/UI/Main/UI_Main.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(202, new CurrencyViewModuleLoader(), "Assets/_Resources/Prefab/UI/Currency/UI_Currency.prefab", 0));
				views.RegisterViewModule(new ViewModuleData(106, null, "Assets/_Resources/Prefab/UI/UI_FlyItem/UI_FlyItem.prefab", 0));
				views.RegisterViewModule(new ViewModuleData(203, null, "Assets/_Resources/Prefab/UI/InfoCommon/UI_InfoCommon.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(205, null, "Assets/_Resources/Prefab/UI/PopCommon/UI_PopCommon.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(210, null, "Assets/_Resources/Prefab/UI/PopCommon/UI_PopCommon2.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(204, new RewardCommonViewModuleLoader(), "Assets/_Resources/Prefab/UI/RewardCommon/UI_RewardCommon.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(207, null, "Assets/_Resources/Prefab/UI/Setting/UI_Setting.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(206, null, "Assets/_Resources/Prefab/UI/LanguageChoose/UI_LanguageChoose.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(212, null, "Assets/_Resources/Prefab/UI/UI_ItemInfo/UI_ItemInfo.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(213, new FunctionOpenViewModuleLoader(), "Assets/_Resources/Prefab/UI/UI_FunctionOpen/UI_FunctionOpen.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(300, new PlayerAvatarClothesViewModuleLoader(), "Assets/_Resources/Prefab/UI/PlayerAvatarSkin/UI_PlayerAvatarClothes.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(301, null, "Assets/_Resources/Prefab/UI/PlayerName/UI_PlayerName.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(802, new PlayerInformationViewModuleLoader(), "Assets/_Resources/Prefab/UI/PlayerInformation/UI_PlayerInformation.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(803, null, "Assets/_Resources/Prefab/UI/SelfInformation/UI_SelfInformation.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(211, new BagViewModuleLoader(), "Assets/_Resources/Prefab/UI/UI_Bag/UI_Bag.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(811, new SignViewModuleLoader(), "Assets/_Resources/Prefab/UI/UI_Sign/UI_Sign.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(812, null, "Assets/_Resources/Prefab/UI/TVReward/TVRewardViewModule.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(214, new BattleHUDViewModuleLoader(), "Assets/_Resources/Prefab/UI/Battle/UI_BattleHUD.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(215, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleFight.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(234, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleTest.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(235, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleTestWin.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(236, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleTestLose.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(222, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleCrossArena.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(225, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleGuildRank.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(228, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleTower.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(223, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleCrossArenaWin.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(224, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleCrossArenaLose.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(226, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleGuildRankWin.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(227, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleGuildRankLose.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(229, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleTowerWin.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(230, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleTowerLose.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(232, null, "Assets/_Resources/Prefab/UI/BattleGuildBoss/UI_BattleGuildBoss.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(233, null, "Assets/_Resources/Prefab/UI/BattleGuildBoss/GuildBoss_Rank.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(231, null, "Assets/_Resources/Prefab/UI/BattleGuildBoss/UI_BattleGuildBossFinish.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(237, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleWin.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(238, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleLose.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(818, null, "Assets/_Resources/Prefab/UI/Purchase/CommonPurchaseTip.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(821, new IAPShopViewModuleLoader(), "Assets/_Resources/Prefab/UI/IAPShop/UI_IAPShopView.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(380, new EquipDetailsViewModuleLoader(), "Assets/_Resources/Prefab/UI/EquipDetails/UI_EquipDetails.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(381, null, "Assets/_Resources/Prefab/UI/EquipMerge/UI_EquipMerge.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(382, null, "Assets/_Resources/Prefab/UI/EquipMergeFinished/UI_EquipMergeFinished.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(383, null, "Assets/_Resources/Prefab/UI/EquipSelector/UI_EquipSelector.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(385, null, "Assets/_Resources/Prefab/UI/EquipMergeAnimation/UI_EquipMergeAnimation.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(386, null, "Assets/_Resources/Prefab/UI/EquipReset/UI_EquipReset.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(320, null, "Assets/_Resources/Prefab/UI/AttributeDetailed/UI_AttributeDetailed.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1018, null, "Assets/_Resources/Prefab/UI/AttributeShow/UI_AttributeShow.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(400, null, "Assets/_Resources/Prefab/UI/CrossArena/UI_CrossArena.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(401, null, "Assets/_Resources/Prefab/UI/CrossArenaRewards/UI_CrossArenaRewards.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(402, null, "Assets/_Resources/Prefab/UI/CrossArenaChallenge/UI_CrossArenaChallenge.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(403, null, "Assets/_Resources/Prefab/UI/CrossArenaRecord/UI_CrossArenaRecord.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(404, null, "Assets/_Resources/Prefab/UI/CrossArena/UI_CrossArenaDanChange.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(814, null, "Assets/_Resources/Prefab/UI/UI_Mail/UI_Mail.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(815, null, "Assets/_Resources/Prefab/UI/UI_MailInfo/UI_MailInfo.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(816, null, "Assets/_Resources/Prefab/UI/Tower/UI_MainTower.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(817, null, "Assets/_Resources/Prefab/UI/Tower/UI_TowerRank.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(809, null, "Assets/_Resources/Prefab/UI/Shop/UI_MainShop.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(209, null, "Assets/_Resources/Prefab/UI/ShopBuyConfirm/UI_ShopBuyConfirm.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1037, null, "Assets/_Resources/Prefab/UI/MainShop/UI_ShopActivitySUpPoolPreview.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1038, null, "Assets/_Resources/Prefab/UI/MainShop/UI_ShopActivitySUpBigRewardSelect.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(502, new MainGuildViewModuleLoader(), "Assets/_Resources/Prefab/UI/Guild/UI_MainGuildInfo.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(515, new MainGuildViewModuleLoader(), "Assets/_Resources/Prefab/UI/Guild/UIMainGuild.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(510, null, "Assets/_Resources/Prefab/UI/Guild/UI_GuildCheckPop.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(504, null, "Assets/_Resources/Prefab/UI/Guild/UI_GuildInfoPop.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(506, null, "Assets/_Resources/Prefab/UI/Guild/UI_GuildDetailInfo.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(508, null, "Assets/_Resources/Prefab/UI/Guild/UI_GuildAnnouncementModify.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(509, null, "Assets/_Resources/Prefab/UI/Guild/UI_GuildApplyJoin.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(526, null, "Assets/_Resources/Prefab/UI/Guild/UI_GuildManageMember.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(517, null, "Assets/_Resources/Prefab/UI/Guild/UI_GuildChat.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(525, null, "Assets/_Resources/Prefab/UI/Guild/UI_GuildActivity.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(513, null, "Assets/_Resources/Prefab/UI/Guild/GuildBOSS/UI_GuildBoss.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(501, null, "Assets/_Resources/Prefab/UI/Guild/UI_GuildIconSet.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(806, new GuildContributeViewModuleLoader(), "Assets/_Resources/Prefab/UI/Guild/UIGuildContribute.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(527, new GuildCheckSimplePopViewModuleLoader(), "Assets/_Resources/Prefab/UI/Guild/UI_GuildCheckSimplePop.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(804, new GuildPlayerInformationViewModuleLoader(), "Assets/_Resources/Prefab/UI/Guild/GuildCommon/UI_GuildPlayerInformation.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(805, new GuildShopViewModuleLoader(), "Assets/_Resources/Prefab/UI/Guild/GuildUIMainShop.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(532, null, "Assets/_Resources/Prefab/UI/Guild/UI_GuildLog.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(512, null, "Assets/_Resources/Prefab/UI/Guild/UI_GuildLevelUp.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(507, null, "Assets/_Resources/Prefab/UI/Guild/UI_GuildList.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(518, null, "Assets/_Resources/Prefab/UI/Guild/GuildRace/UI_GuildRaceMain.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(520, null, "Assets/_Resources/Prefab/UI/Guild/GuildRace/UI_GuildRaceRecordBattle.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(519, null, "Assets/_Resources/Prefab/UI/Guild/GuildRace/UI_GuildRaceRecordMain.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(521, null, "Assets/_Resources/Prefab/UI/Guild/GuildRace/UI_GuildRaceRewardsShow.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(522, null, "Assets/_Resources/Prefab/UI/Guild/GuildRace/UI_GuildRaceRank.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(523, null, "Assets/_Resources/Prefab/UI/Guild/GuildRace/UI_GuildRaceSeasonSel.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(524, null, "Assets/_Resources/Prefab/UI/Guild/GuildRace/UI_GuildRacePositionChange.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(528, null, "Assets/_Resources/Prefab/UI/Guild/GuildRace/UI_GuildRaceDanChange.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(529, null, "Assets/_Resources/Prefab/UI/Guild/GuildRace/UI_GuildRaceBattleMatch.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(530, null, "Assets/_Resources/Prefab/UI/Guild/GuildBOSS/UI_GuildBossUserRank.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(531, null, "Assets/_Resources/Prefab/UI/Guild/GuildBOSS/UI_GuildBossRankPanel.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(534, null, "Assets/_Resources/Prefab/UI/Guild/GuildBOSS/UI_GuildBossTask.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(535, null, "Assets/_Resources/Prefab/UI/Guild/GuildBOSS/UI_GuildBossUpDanPanel.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(571, null, "Assets/_Resources/Prefab/UI/Chat/UI_ChatShareItem.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(574, null, "Assets/_Resources/Prefab/UI/Chat/UI_ChatDonation.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(572, null, "Assets/_Resources/Prefab/UI/Chat/UI_ChatDonationItem.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(573, null, "Assets/_Resources/Prefab/UI/Chat/UI_ChatDonationRecord.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(901, new ActivityViewModuleLoader(), "Assets/_Resources/Prefab/UI/ActivityWeek/UI_ActivityWeekEntry.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(900, new ActivityViewModuleLoader(), "Assets/_Resources/Prefab/UI/ActivityWeek/UI_ActivityWeekViewModule.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(902, new ActivityShopViewModuleLoader(), "Assets/_Resources/Prefab/UI/ActivityWeek/UI_ActivityShop.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(903, new ActivitySlotTrainViewModuleLoader(), "Assets/_Resources/Prefab/UI/ActivitySlotTrain/UIActivitySlotTrain.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(904, null, "Assets/_Resources/Prefab/UI/ActivitySlotTrain/UIActivitySlotTrainBigSelect.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(905, null, "Assets/_Resources/Prefab/UI/ActivitySlotTrain/UIActivitySlotTrainProbability.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(906, null, "Assets/_Resources/Prefab/UI/ActivitySlotTrain/UIActivitySlotTrainTask.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(913, null, "Assets/_Resources/Prefab/UI/UI_Task/UI_Task.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(914, null, "Assets/_Resources/Prefab/UI/SevenDayCarnival/SevenDayCarnivalPanel.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(915, null, "Assets/_Resources/Prefab/UI/UI_BoxInfo/ChapterBoxInfoPanel.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(916, null, "Assets/_Resources/Prefab/UI/UI_TipsBubble/TipsBubble.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(907, null, "Assets/_Resources/Prefab/UI/WorldBoss/UI_WorldBossPanel.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(908, null, "Assets/_Resources/Prefab/UI/WorldBoss/WorldBossRankAnimPanel.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(909, null, "Assets/_Resources/Prefab/UI/WorldBoss/WorldBossDamageView.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(910, null, "Assets/_Resources/Prefab/UI/WorldBoss/UI_WorldBossRankPanel.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(911, null, "Assets/_Resources/Prefab/UI/WorldBoss/UI_BattleResultWorldBoss.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(912, null, "Assets/_Resources/Prefab/UI/BattleWorldBoss/UI_BattleWorldBoss.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(917, null, "Assets/_Resources/Prefab/UI/UI_UpdateApp/UI_UpdateApp.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(918, null, "Assets/_Resources/Prefab/UI/UI_UpdateResources/UI_UpdateResources.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(819, null, "Assets/_Resources/Prefab/UI/Ticket/UI_CommonTicketBuyTip.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(820, null, "Assets/_Resources/Prefab/UI/Ticket/UI_CommonTicketDailyExchangeTip.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(925, new GameEventViewModuleLoader(), "Assets/_Resources/Prefab/UI/GameEvent/UIGameEvent.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(926, null, "Assets/_Resources/Prefab/UI/GameEventSkill/UISelectSkill.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(927, null, "Assets/_Resources/Prefab/UI/GameEventSkill/UIGetSkill.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(946, null, "Assets/_Resources/Prefab/UI/GameEventSkill/UILearnedSkills.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(928, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattlePause.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(105, null, "Assets/_Resources/Prefab/UI/Loading/UI_LoadingMap.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(930, null, "Assets/_Resources/Prefab/UI/GameEventFinish/UIGameEventFinish.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(931, null, "Assets/_Resources/Prefab/UI/GameEventSkill/UIUnlockSkill.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(932, null, "Assets/_Resources/Prefab/UI/InfoTip/UIInfoTip.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(933, null, "Assets/_Resources/Prefab/UI/InfoTip/UI_ItemInfoTip.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(936, null, "Assets/_Resources/Prefab/UI/Chapter/UIChapterReward.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(937, null, "Assets/_Resources/Prefab/UI/GameEventBox/UIGameEventBox.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(938, null, "Assets/_Resources/Prefab/UI/GameEventDemon/UIGameEventDemon.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(939, null, "Assets/_Resources/Prefab/UI/GameEventAngel/UIGameEventAngel.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(940, null, "Assets/_Resources/Prefab/UI/GameEventAdventurer/UIGameEventAdventurer.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(947, null, "Assets/_Resources/Prefab/UI/SlotTrain/UIGameEventSlotTrain.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(948, null, "Assets/_Resources/Prefab/UI/SlotTrain/UIGameEventSlotTrainReward.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(941, null, "Assets/_Resources/Prefab/UI/Chapter/UI_ChapterPowerfulEnemy.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(945, null, "Assets/_Resources/Prefab/UI/Talent/UI_TalentRewardTip.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(942, null, "Assets/_Resources/Prefab/UI/Talent/UI_TalentEvolutionPreview.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(943, null, "Assets/_Resources/Prefab/UI/Talent/UI_TalentStateUpgradeResult.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(944, null, "Assets/_Resources/Prefab/UI/Talent/UI_TalentSkillReward.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(949, new PetViewModuleLoader(), "Assets/_Resources/Prefab/UI/Pet/UI_Pet.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(950, null, "Assets/_Resources/Prefab/UI/Pet/UI_PetStarUpgrade.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(952, null, "Assets/_Resources/Prefab/UI/Pet/UI_PetCollection.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(953, null, "Assets/_Resources/Prefab/UI/Tip/UI_PetSkillTip.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(954, null, "Assets/_Resources/Prefab/UI/Pet/UI_PetOpenEgg.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(955, null, "Assets/_Resources/Prefab/UI/Pet/UI_PetProbabilityTip.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(951, null, "Assets/_Resources/Prefab/UI/Pet/UI_PetQuickStarUpgradeResult.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(956, null, "Assets/_Resources/Prefab/UI/Pet/UI_PetTraining.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(957, null, "Assets/_Resources/Prefab/UI/Pet/UI_PetTrainingProbabilityTip.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(958, null, "Assets/_Resources/Prefab/UI/Pet/UI_PetLevelEffectTip.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(959, null, "Assets/_Resources/Prefab/UI/Pet/UI_PetSkillEffectTip.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(960, null, "Assets/_Resources/Prefab/UI/Pet/UI_PetItemTipInfo.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(961, new FortuneWheelViewModuleLoader(), "Assets/_Resources/Prefab/UI/FortuneWheel/UI_FortuneWheel.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(962, null, "Assets/_Resources/Prefab/UI/GameEventSlot/UIGameEventSlotReward.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(963, null, "Assets/_Resources/Prefab/UI/GameEventSlot/UIGameEventSlotSkill.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(964, null, "Assets/_Resources/Prefab/UI/FortuneWheel/UI_RewardAttribute.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(965, new CardFlippingViewModuleLoader(), "Assets/_Resources/Prefab/UI/CardFlipping/UI_CardFlipping.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(966, new SlotMachineViewModuleLoader(), "Assets/_Resources/Prefab/UI/SlotMachine/UI_SlotMachine.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(968, new CollectionViewModuleLoader(), "Assets/_Resources/Prefab/UI/Collection/UI_Collection.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(969, new CollectionDetailViewModuleLoader(), "Assets/_Resources/Prefab/UI/Collection/UI_CollectionDetail.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(970, null, "Assets/_Resources/Prefab/UI/Collection/UI_CollectionSuitShow.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(972, null, "Assets/_Resources/Prefab/UI/Guide/UIGuide.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(973, new ChapterActivityNormalViewModuleLoader(), "Assets/_Resources/Prefab/UI/ChapterActivity/UIChapterActivityNormal.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(974, new ChapterActivityRankViewModuleLoader(), "Assets/_Resources/Prefab/UI/ChapterActivity/UIChapterActivityRank.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(975, null, "Assets/_Resources/Prefab/UI/ChapterActivity/UIChapterActivityRankPreview.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(976, new ChapterSweepViewModuleLoader(), "Assets/_Resources/Prefab/UI/ChapterSweep/UIChapterSweep.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(977, null, "Assets/_Resources/Prefab/UI/ChapterSweepFinish/UIChapterSweepFinish.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(978, null, "Assets/_Resources/Prefab/UI/More/UI_MoreExtension.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(979, null, "Assets/_Resources/Prefab/UI/IAPShop/UI_OpenEquipBox.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(980, null, "Assets/_Resources/Prefab/UI/UI_OpenChestShow/UI_OpenChestShow.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(981, null, "Assets/_Resources/Prefab/UI/UI_RememberTipCommon/UI_RememberTipCommon.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(982, null, "Assets/_Resources/Prefab/UI/EquipShopProbability/EquipShopProbabilityViewModule.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(984, null, "Assets/_Resources/Prefab/UI/DailyActivities/UIDailyActivities.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(983, new DungeonViewModuleLoader(), "Assets/_Resources/Prefab/UI/Dungeon/UIDungeon.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(985, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleDungeon.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(986, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleDungeonWin.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(987, null, "Assets/_Resources/Prefab/UI/Battle/UI_BattleDungeonLose.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(988, new MountViewModuleLoader(), "Assets/_Resources/Prefab/UI/Mount/UIMount.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(989, new ArtifactViewModuleLoader(), "Assets/_Resources/Prefab/UI/Artifact/UIArtifact.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(990, null, "Assets/_Resources/Prefab/UI/SkillUpgradePreview/UISkillUpgradePreview.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(991, null, "Assets/_Resources/Prefab/UI/IAPRechargeGift/UI_IAPRechargeGift.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(992, null, "Assets/_Resources/Prefab/UI/IAPPrivilegeCard/UI_IAPPrivilegeCard.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(993, new IAPFundViewModuleLoader(), "Assets/_Resources/Prefab/UI/IAPFund/UI_IAPFund.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(822, null, "Assets/_Resources/Prefab/UI/IAPBattlePass/UI_IAPBattlePassBuy.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(823, null, "Assets/_Resources/Prefab/UI/IAPFund/UI_IAPLevelFundBuy.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(924, null, "Assets/_Resources/Prefab/UI/IAPEnergyGift/UI_EnergyGift.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(998, null, "Assets/_Resources/Prefab/UI/WatchAD/UIWatchAD.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(999, new HangUpViewModuleLoader(), "Assets/_Resources/Prefab/UI/HangUp/UIHangUp.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1000, null, "Assets/_Resources/Prefab/UI/SingleSlot/UISingleSlot.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(1001, null, "Assets/_Resources/Prefab/UI/PaySlot/UIPaySlot.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(1003, new PushGiftViewModuleLoader(), "Assets/_Resources/Prefab/UI/PushGift/Pop/UI_PopPushGift.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1002, new PushGiftViewModuleLoader(), "Assets/_Resources/Prefab/UI/PushGift/Panel/UI_PushGiftViewModule.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1004, new MiningViewModuleLoader(), "Assets/_Resources/Prefab/UI/Mining/UIMining.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1005, new MiningBonusViewModuleLoader(), "Assets/_Resources/Prefab/UI/Mining/UIMiningBonus.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1006, null, "Assets/_Resources/Prefab/UI/Mining/UIMiningBonusRate.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1007, null, "Assets/_Resources/Prefab/UI/Mining/UIMiningBuyCard.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1008, null, "Assets/_Resources/Prefab/UI/Mining/UIMiningTreasureUpgrade.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1009, null, "Assets/_Resources/Prefab/UI/UI_SelectServer/UI_SelectServer.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(1010, null, "Assets/_Resources/Prefab/UI/RogueDungeon/UIRogueDungeon.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1011, null, "Assets/_Resources/Prefab/UI/RogueDungeon/UIRogueDungeonRank.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1012, new BattleRogueDungeonViewModuleLoader(), "Assets/_Resources/Prefab/UI/BattleRogueDungeon/UIBattleRogueDungeon.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1013, null, "Assets/_Resources/Prefab/UI/SpecialChallenges/UISpecialChallenges.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1014, null, "Assets/_Resources/Prefab/UI/RoundSelectSkill/UIRoundSelectSkill.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1015, null, "Assets/_Resources/Prefab/UI/BattleRogueDungeon/UIBattleRogueDungeonResult.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1016, null, "Assets/_Resources/Prefab/UI/SettingsMinigame/UI_MinigameConstract.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(1017, null, "Assets/_Resources/Prefab/UI/OfficialAccount/UI_OfficialAccount.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(971, new ItemResourcesViewModuleLoader(), "Assets/_Resources/Prefab/UI/ItemResources/UI_ItemResources.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1019, null, "Assets/_Resources/Prefab/UI/UI_Rate/UI_Rate.prefab", 1));
				views.RegisterViewModule(new ViewModuleData(1020, null, "Assets/_Resources/Prefab/UI/IAPMeetingGift/UIMeetingGift.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1021, null, "Assets/_Resources/Prefab/UI/ChapterBattlePass/UIChapterBattlePass.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1022, null, "Assets/_Resources/Prefab/UI/BoxUpgrade/UIBoxUpgrade.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1023, null, "Assets/_Resources/Prefab/UI/UI_Notice/UI_Notice.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1027, null, "Assets/_Resources/Prefab/UI/TalentLegacy/TalentLegacyStudy.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1028, null, "Assets/_Resources/Prefab/UI/Ticket/CommonUseTip.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1029, null, "Assets/_Resources/Prefab/UI/TalentLegacy/TalentLegacySkill.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1030, null, "Assets/_Resources/Prefab/UI/TalentLegacy/TalentLegacyStudyFinish.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1031, null, "Assets/_Resources/Prefab/UI/TalentLegacy/TalentLegacyBigStudyFinish.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1032, null, "Assets/_Resources/Prefab/UI/TalentLegacy/TalentLegacySkillSelect.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1024, null, "Assets/_Resources/Prefab/UI/NewWorld/UINewWorld.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1025, null, "Assets/_Resources/Prefab/UI/NewWorld/UINewWorldRank.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1026, null, "Assets/_Resources/Prefab/UI/CloudLoading/UICloudLoading.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1033, null, "Assets/_Resources/Prefab/UI/BuyMonthCard/UIBuyMonthCard.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1034, null, "Assets/_Resources/Prefab/UI/ChainPacks/UI_ChainPacks.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1035, null, "Assets/_Resources/Prefab/UI/ChapterActivityWheel/UIChapterActivityWheel.prefab", 2));
				views.RegisterViewModule(new ViewModuleData(1036, null, "Assets/_Resources/Prefab/UI/ChapterActivityWheel/UIChapterActivityWheelPreview.prefab", 2));
			}
		}

		public static class ABTestKey
		{
			public const string Version1 = "capybara_ab_1";
		}

		public static class ABTest
		{
		}
	}
}
