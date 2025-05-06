using System;
using Dxx.Guild.GuildUIDatas;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Platfrom;
using HotFix;

namespace Dxx.Guild
{
	public class GuildUIDataModule : IGuildModule
	{
		public int ModuleName
		{
			get
			{
				return 1001;
			}
		}

		public bool Init(GuildInitConfig config)
		{
			this.ApplyJoin.Init();
			this.LevelUP.Init();
			this.Race.Init();
			this.RaceDanChange.Init();
			this.RaceBattleMatch.Init();
			this.RaceViewOpenData.Init();
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			if (@event != null)
			{
				@event.RegisterEvent(10, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.RegisterEvent(16, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.RegisterEvent(17, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.RegisterEvent(18, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.RegisterEvent(14, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.RegisterEvent(15, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.RegisterEvent(12, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.RegisterEvent(20, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.RegisterEvent(23, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.RegisterEvent(22, new GuildHandlerEvent(this.OnGuildSetBeKickOutFlag));
				@event.RegisterEvent(25, new GuildHandlerEvent(this.OnMyPositionChange));
			}
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Guild_Leave, new HandlerEvent(this.OnLeaveGuild));
			return true;
		}

		public void UnInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			if (@event != null)
			{
				@event.UnRegisterEvent(10, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.UnRegisterEvent(16, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.UnRegisterEvent(17, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.UnRegisterEvent(18, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.UnRegisterEvent(14, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.UnRegisterEvent(15, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.UnRegisterEvent(12, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.UnRegisterEvent(20, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.UnRegisterEvent(23, new GuildHandlerEvent(this.OnGuildDataChangeRefreshUI));
				@event.UnRegisterEvent(22, new GuildHandlerEvent(this.OnGuildSetBeKickOutFlag));
				@event.UnRegisterEvent(25, new GuildHandlerEvent(this.OnMyPositionChange));
			}
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Guild_Leave, new HandlerEvent(this.OnLeaveGuild));
			this.ApplyJoin.DeInit();
			this.LevelUP.DeInit();
			this.Race.DeInit();
			this.RaceDanChange.DeInit();
			this.RaceBattleMatch.DeInit();
			this.RaceViewOpenData.DeInit();
		}

		public void OnGuildDataChangeRefreshUI(int type, GuildBaseEvent eventArgs)
		{
			this.OnRefreshUI();
		}

		private async void OnRefreshUI()
		{
			if (!this.mPrepareDispatchRefreshUI)
			{
				this.mPrepareDispatchRefreshUI = true;
				await TaskExpand.Delay(this.mRefreshDelayDispatch);
				this.mPrepareDispatchRefreshUI = false;
				GuildEvent_RefreshUI guildEvent_RefreshUI = new GuildEvent_RefreshUI();
				GuildSDKManager.Instance.Event.DispatchNow(8, guildEvent_RefreshUI);
			}
		}

		private void OnGuildSetBeKickOutFlag(int type, GuildBaseEvent eventArgs)
		{
		}

		private void OnLeaveGuild(object sender, int type, BaseEventArgs eventArgs)
		{
			int num = 501;
			ViewName viewName = ViewName.GuildViewEnd;
			for (int i = num; i <= (int)viewName; i++)
			{
				if (i != 515 && GameApp.View.GetViewModuleData(i) != null && GameApp.View.IsOpened(i))
				{
					GameApp.View.CloseView(i, null);
				}
			}
			RedPointController.Instance.ReCalc("Guild", true);
		}

		[GameTestMethod]
		private static void OnTest()
		{
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_Guild_Leave, null);
		}

		private void OnMyPositionChange(int type, GuildBaseEvent eventArgs)
		{
			if (eventArgs is GuildEvent_MyPositionChange)
			{
				GuildProxy.RedPoint.CalcRedPoint("Guild.Hall.ApplyJoin", true);
			}
		}

		public GuildUIData_ApplyJoin ApplyJoin = new GuildUIData_ApplyJoin();

		public GuildUIData_GuildLevelUP LevelUP = new GuildUIData_GuildLevelUP();

		public GuildUIData_Race Race = new GuildUIData_Race();

		public GuildUIData_RaceDanChange RaceDanChange = new GuildUIData_RaceDanChange();

		public GuildUIData_RaceBattleMatch RaceBattleMatch = new GuildUIData_RaceBattleMatch();

		public GuildUIData_RaceViewOpenData RaceViewOpenData = new GuildUIData_RaceViewOpenData();

		private int mRefreshDelayDispatch = 50;

		private bool mPrepareDispatchRefreshUI;
	}
}
