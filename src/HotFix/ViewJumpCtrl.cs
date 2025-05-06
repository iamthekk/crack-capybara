using System;
using System.Threading.Tasks;
using Dxx.Guild;
using Framework;
using Framework.Logic.GameTestTools;
using Framework.ViewModule;

namespace HotFix
{
	public class ViewJumpCtrl : Singleton<ViewJumpCtrl>
	{
		public bool IsCanJumpTo(ViewJumpType jumpType, object openData = null, bool isShowTip = true)
		{
			string text = string.Empty;
			bool flag;
			switch (jumpType)
			{
			case ViewJumpType.MainBattle:
				flag = true;
				goto IL_0628;
			case ViewJumpType.MainEquip:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Equip, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(2);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.MainShop:
			case ViewJumpType.MainShop_GiftShop:
			case ViewJumpType.MainShop_EquipShop:
			case ViewJumpType.MainShop_DiamonShop:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Shop, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(1);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.MainActive:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainUI_DailyActivities, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(25);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.Tower:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_ChallengeTower, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(41);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.CrossArena:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_Activity_CrossArena, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(42);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.MainCityBox:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_City_Box, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(1051);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.MainCityShop:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.BlackMarket, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(18);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.MainCityGuild:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainUI_Guild, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(24);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.DragonLair:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_Activity_DragonsLair, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(43);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.AstralTree:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_Activity_AstralTree, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(44);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.BlackMarket:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.BlackMarket, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(18);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.MainChest:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Chest, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(5);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.MainEquip_PetDraw:
			case ViewJumpType.MainEquip_PetList:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Pet, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(51);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.Equip_Collection:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Collection, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(52);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.Dungeon:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainUI_DailyActivities, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(25);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.MainTalent:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Talent, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(4);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.Mount:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Mount, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(53);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.MainEquip_Artifact:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Artifact, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(54);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.HangUp:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.HangUp, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(202);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.Sweep:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Sweep, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(23);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.RogueDungeon:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.RogueDungeon, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(56);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.SwordIsland:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_Activity_SwordIsland, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(45);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.Mining:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Mining, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(55);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.Activity_Week:
			{
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_Week, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(102);
					goto IL_0628;
				}
				flag = false;
				ActivityWeekDataModule dataModule = GameApp.Data.GetDataModule(DataName.ActivityWeekDataModule);
				if (dataModule != null)
				{
					flag = dataModule.CanShow();
				}
				if (!flag)
				{
					text = Singleton<LanguageManager>.Instance.GetInfoByID("server_err_5001");
					goto IL_0628;
				}
				goto IL_0628;
			}
			case ViewJumpType.DeepSeaRuins:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Main_Activity_DeepSeaRuins, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(46);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.NewWorld:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.NewWorld, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(71);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.GuildShop:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.BlackMarket, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(18);
				}
				if (!flag)
				{
					goto IL_0628;
				}
				flag = GuildSDKManager.Instance.GuildInfo.HasGuild;
				if (!GuildSDKManager.Instance.GuildInfo.HasGuild)
				{
					text = Singleton<LanguageManager>.Instance.GetInfoByID("guild_not_join");
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.ManaCrystalShop:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.BlackMarket, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(18);
				}
				if (!flag)
				{
					goto IL_0628;
				}
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.ManaCrystalShop, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(203);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.GuildBoss:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainUI_Guild, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(24);
				}
				if (!flag)
				{
					goto IL_0628;
				}
				flag = GuildSDKManager.Instance.GuildInfo.HasGuild;
				if (!flag)
				{
					text = Singleton<LanguageManager>.Instance.GetInfoByID("guild_not_join");
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.WorldBoss:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.WorldBoss, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(114);
					goto IL_0628;
				}
				goto IL_0628;
			case ViewJumpType.SevenDayCarnival:
				flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_Carnival, false);
				if (!flag)
				{
					text = Singleton<GameFunctionController>.Instance.GetLockTips(103);
					goto IL_0628;
				}
				goto IL_0628;
			}
			throw new ArgumentOutOfRangeException("jumpType", jumpType, null);
			IL_0628:
			if (isShowTip && !string.IsNullOrEmpty(text))
			{
				GameApp.View.ShowStringTip(text);
			}
			return flag;
		}

		public async Task JumpTo(ViewJumpType jumpType, object openData = null)
		{
			switch (jumpType)
			{
			case ViewJumpType.MainBattle:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.MainEquip:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Equip, null);
				return;
			case ViewJumpType.MainShop:
			{
				MainShopType mainShopType = MainShopType.EquipShop;
				if (openData != null && openData is MainShopType)
				{
					mainShopType = (MainShopType)openData;
				}
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				MainViewModule viewModule = GameApp.View.GetViewModule(ViewName.MainViewModule);
				viewModule.GotoPage(UIMainPageName.Shop, null);
				UIMainShop uimainShop = viewModule.GetCurrentPage() as UIMainShop;
				if (uimainShop != null)
				{
					uimainShop.SwitchShop(mainShopType, null);
				}
				GameApp.View.CloseAllView(new UILayers[] { 1 }, null);
				return;
			}
			case ViewJumpType.MainActive:
				if (!GameApp.View.IsOpened(ViewName.DailyActivitiesViewModule))
				{
					GameApp.View.OpenView(ViewName.DailyActivitiesViewModule, null, 1, null, null);
					return;
				}
				return;
			case ViewJumpType.Tower:
				if (!GameApp.View.IsOpened(ViewName.TowerMainViewModule))
				{
					GameApp.View.OpenView(ViewName.TowerMainViewModule, null, 1, null, null);
					return;
				}
				return;
			case ViewJumpType.CrossArena:
			{
				if (GameApp.View.IsOpened(ViewName.CrossArenaViewModule))
				{
					return;
				}
				int num;
				int num2;
				if (GameApp.Data.GetDataModule(DataName.CrossArenaDataModule).IsInDayCloseTime(out num, out num2))
				{
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("arena_open_tips", new object[] { num, num2 });
					GameApp.View.ShowStringTip(infoByID);
					return;
				}
				await GameApp.View.OpenViewTask(ViewName.CrossArenaViewModule, null, 1, null, null);
				return;
			}
			case ViewJumpType.MainCityBox:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.MainCityBoxViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.MainCityBoxViewModule, null, 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.MainCityShop:
				GameApp.View.CloseAllView(new UILayers[] { 1 }, null);
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.GuildShopViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.GuildShopViewModule, new MainShopJumpTabData(MainShopType.BlackMarket), 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.MainCityGuild:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.MainGuildViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.MainGuildViewModule, null, 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.DragonLair:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.DailyActivitiesViewModule))
				{
					DailyActivitiesViewModule.OpenData openData2 = new DailyActivitiesViewModule.OpenData();
					openData2.openPageType = DailyActivitiesPageType.Dungeon;
					await GameApp.View.OpenViewTask(ViewName.DailyActivitiesViewModule, openData2, 1, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.DungeonViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.DungeonViewModule, DungeonID.DragonsLair, 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.AstralTree:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.DailyActivitiesViewModule))
				{
					DailyActivitiesViewModule.OpenData openData3 = new DailyActivitiesViewModule.OpenData();
					openData3.openPageType = DailyActivitiesPageType.Dungeon;
					await GameApp.View.OpenViewTask(ViewName.DailyActivitiesViewModule, openData3, 1, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.DungeonViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.DungeonViewModule, DungeonID.AstralTree, 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.BlackMarket:
				if (!GameApp.View.IsOpened(ViewName.GuildShopViewModule))
				{
					GuildProxy.UI.OpenBlackMarket();
					return;
				}
				return;
			case ViewJumpType.MainChest:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				GameApp.View.CloseAllView(new UILayers[] { 1 }, null);
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Chest, null);
				return;
			case ViewJumpType.MainShop_GiftShop:
				await Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.MainShop, MainShopType.GiftShop);
				return;
			case ViewJumpType.MainEquip_PetDraw:
				if (!GameApp.View.IsOpened(ViewName.PetViewModule))
				{
					PetViewModule.OpenData openData4 = new PetViewModule.OpenData();
					openData4.pageType = PetPageType.PetRanch;
					await GameApp.View.OpenViewTask(ViewName.PetViewModule, openData4, 1, null, null);
					return;
				}
				return;
			case ViewJumpType.MainEquip_PetList:
				if (!GameApp.View.IsOpened(ViewName.PetViewModule))
				{
					PetViewModule.OpenData openData5 = new PetViewModule.OpenData();
					openData5.pageType = PetPageType.PetList;
					await GameApp.View.OpenViewTask(ViewName.PetViewModule, openData5, 1, null, null);
					return;
				}
				return;
			case ViewJumpType.Equip_Collection:
				if (!GameApp.View.IsOpened(ViewName.CollectionViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.CollectionViewModule, null, 1, null, null);
					return;
				}
				return;
			case ViewJumpType.MainShop_EquipShop:
				await Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.MainShop, MainShopType.EquipShop);
				return;
			case ViewJumpType.Dungeon:
				if (!GameApp.View.IsOpened(ViewName.DailyActivitiesViewModule))
				{
					DailyActivitiesViewModule.OpenData openData6 = new DailyActivitiesViewModule.OpenData();
					openData6.openPageType = DailyActivitiesPageType.Dungeon;
					await GameApp.View.OpenViewTask(ViewName.DailyActivitiesViewModule, openData6, 1, null, null);
					return;
				}
				return;
			case ViewJumpType.MainTalent:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Talent, openData);
				return;
			case ViewJumpType.Mount:
				if (!GameApp.View.IsOpened(ViewName.MountViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.MountViewModule, null, 1, null, null);
					return;
				}
				return;
			case ViewJumpType.MainEquip_Artifact:
				if (!GameApp.View.IsOpened(ViewName.ArtifactViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.PetViewModule, null, 1, null, null);
					return;
				}
				return;
			case ViewJumpType.HangUp:
				if (!GameApp.View.IsOpened(ViewName.HangUpViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.HangUpViewModule, null, 1, null, null);
					return;
				}
				return;
			case ViewJumpType.Sweep:
				if (!GameApp.View.IsOpened(ViewName.ChapterSweepViewModule))
				{
					int previousChapterID = GameApp.Data.GetDataModule(DataName.ChapterDataModule).GetPreviousChapterID();
					ChapterSweepViewModule.OpenData openData7 = new ChapterSweepViewModule.OpenData();
					openData7.chapterId = previousChapterID;
					openData7.isRecord = false;
					await GameApp.View.OpenViewTask(ViewName.ChapterSweepViewModule, openData7, 1, null, null);
					return;
				}
				return;
			case ViewJumpType.RogueDungeon:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.DailyActivitiesViewModule))
				{
					new DailyActivitiesViewModule.OpenData().openPageType = DailyActivitiesPageType.Challenge;
					await GameApp.View.OpenViewTask(ViewName.DailyActivitiesViewModule, null, 1, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.RogueDungeonViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.RogueDungeonViewModule, null, 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.SwordIsland:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.DailyActivitiesViewModule))
				{
					DailyActivitiesViewModule.OpenData openData8 = new DailyActivitiesViewModule.OpenData();
					openData8.openPageType = DailyActivitiesPageType.Dungeon;
					await GameApp.View.OpenViewTask(ViewName.DailyActivitiesViewModule, openData8, 1, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.DungeonViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.DungeonViewModule, DungeonID.SwordIsland, 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.Mining:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.DailyActivitiesViewModule))
				{
					DailyActivitiesViewModule.OpenData openData9 = new DailyActivitiesViewModule.OpenData();
					openData9.openPageType = DailyActivitiesPageType.Challenge;
					await GameApp.View.OpenViewTask(ViewName.DailyActivitiesViewModule, openData9, 1, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.MiningViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.MiningViewModule, null, 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.Activity_Week:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.ActivityWeekEntryModule))
				{
					await GameApp.View.OpenViewTask(ViewName.ActivityWeekEntryModule, null, 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.MainShop_DiamonShop:
				await Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.MainShop, MainShopType.DiamondShop);
				return;
			case ViewJumpType.DeepSeaRuins:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.DailyActivitiesViewModule))
				{
					DailyActivitiesViewModule.OpenData openData10 = new DailyActivitiesViewModule.OpenData();
					openData10.openPageType = DailyActivitiesPageType.Dungeon;
					await GameApp.View.OpenViewTask(ViewName.DailyActivitiesViewModule, openData10, 1, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.DungeonViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.DungeonViewModule, DungeonID.DeepSeaRuins, 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.NewWorld:
			{
				NewWorldDataModule dataModule = GameApp.Data.GetDataModule(DataName.NewWorldDataModule);
				if (dataModule != null && !dataModule.IsGoNewWorldEnabled() && !GameApp.View.IsOpened(ViewName.NewWorldViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.NewWorldViewModule, null, 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			}
			case ViewJumpType.GuildShop:
				GameApp.View.CloseAllView(new UILayers[] { 1 }, null);
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.GuildShopViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.GuildShopViewModule, new MainShopJumpTabData(MainShopType.GuildShop), 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.ManaCrystalShop:
				GameApp.View.CloseAllView(new UILayers[] { 1 }, null);
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.GuildShopViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.GuildShopViewModule, new MainShopJumpTabData(MainShopType.ManaCrystalShop), 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.GuildBoss:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.MainGuildViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.MainGuildViewModule, null, 1, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.GuildBossViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.GuildBossViewModule, null, 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.WorldBoss:
				if (!GameApp.View.IsOpened(ViewName.MainViewModule))
				{
					GameApp.View.OpenView(ViewName.MainViewModule, null, 0, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.DailyActivitiesViewModule))
				{
					DailyActivitiesViewModule.OpenData openData11 = new DailyActivitiesViewModule.OpenData();
					openData11.openPageType = DailyActivitiesPageType.Challenge;
					await GameApp.View.OpenViewTask(ViewName.DailyActivitiesViewModule, openData11, 1, null, null);
				}
				if (!GameApp.View.IsOpened(ViewName.WorldBossViewModule))
				{
					await GameApp.View.OpenViewTask(ViewName.WorldBossViewModule, null, 1, null, null);
				}
				GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
				return;
			case ViewJumpType.SevenDayCarnival:
				if (!GameApp.View.IsOpened(ViewName.SevenDayCarnivalViewModule))
				{
					GameApp.View.OpenView(ViewName.SevenDayCarnivalViewModule, null, 1, null, null);
					return;
				}
				return;
			}
			throw new ArgumentOutOfRangeException("jumpType", jumpType, null);
		}

		[GameTestMethod("跳转", "跳转到主界面", "", 1)]
		private static void JumpToMainBattle()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.MainBattle);
		}

		[GameTestMethod("跳转", "跳转到装备界面", "", 2)]
		private static void JumpToMainEquip()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.MainEquip);
		}

		[GameTestMethod("跳转", "跳转到商店界面", "", 3)]
		private static void JumpToMainShop()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.MainShop);
		}

		[GameTestMethod("跳转", "跳转到活动界面", "", 4)]
		private static void JumpToMainActive()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.MainActive);
		}

		[GameTestMethod("跳转", "跳转到爬塔", "", 5)]
		private static void JumpToTower()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.Tower);
		}

		[GameTestMethod("跳转", "跳转到竞技场", "", 6)]
		private static void JumpToCrass()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.CrossArena);
		}

		[GameTestMethod("跳转", "跳转到升级界面", "", 7)]
		private static void JumpToMainHeroLevel()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.MainTalent);
		}

		[GameTestMethod("跳转", "跳转到主城宝箱界面", "", 8)]
		private static void JumpToMainCityBox()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.MainCityBox);
		}

		[GameTestMethod("跳转", "跳转到主城黑市", "", 9)]
		private static void JumpToMainCityShop()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.MainCityShop);
		}

		[GameTestMethod("跳转", "跳转到主城工会", "", 10)]
		private static void JumpToMainCityGuild()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.MainCityGuild);
		}

		[GameTestMethod("跳转", "跳转到商城-黑市", "", 20)]
		private static void JumpToMainShopBlackMarket()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.BlackMarket);
		}

		[GameTestMethod("跳转", "跳转到主页宝箱", "", 21)]
		private static void JumpToMainChest()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.MainChest);
		}

		[GameTestMethod("跳转", "跳转到主页商店-限时礼包", "", 22)]
		private static void JumpToMainShopGift()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.MainShop_GiftShop);
		}

		[GameTestMethod("跳转", "跳转到装备-宠物抽卡", "", 23)]
		private static void JumpToMainEquipPetDraw()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.MainEquip_PetDraw);
		}

		[GameTestMethod("跳转", "跳转到装备-宠物列表", "", 24)]
		private static void JumpToMainEquipPetList()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.MainEquip_PetList);
		}

		[GameTestMethod("跳转", "跳转到装备-藏品", "", 25)]
		private static void JumpToEquipCollection()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.Equip_Collection);
		}

		[GameTestMethod("跳转", "跳转到商城-装备", "", 26)]
		private static void JumpToMainShopEquipment()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.MainShop_EquipShop);
		}

		[GameTestMethod("跳转", "跳转到副本", "", 27)]
		private static void JumpToDungeon()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.Dungeon);
		}

		[GameTestMethod("跳转", "跳转到公会商店", "", 28)]
		private static void JumpToGuildShop()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.GuildShop);
		}

		[GameTestMethod("跳转", "跳转到魔晶商店", "", 29)]
		private static void JumpToManaCrystalShop()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.ManaCrystalShop);
		}

		[GameTestMethod("跳转", "跳转到公会boss", "", 30)]
		private static void JumpToGuildBoss()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.GuildBoss);
		}

		[GameTestMethod("跳转", "跳转到世界boss", "", 31)]
		private static void JumpToWorldBoss()
		{
			ViewJumpCtrl.JumpTest(ViewJumpType.WorldBoss);
		}

		private static void JumpTest(ViewJumpType jumpType)
		{
			if (!Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(jumpType, null, true))
			{
				return;
			}
			Singleton<ViewJumpCtrl>.Instance.JumpTo(jumpType, null);
		}
	}
}
