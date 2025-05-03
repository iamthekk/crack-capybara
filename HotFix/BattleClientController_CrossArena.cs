using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;

namespace HotFix
{
	public class BattleClientController_CrossArena : BattleClientControllerBase
	{
		protected override Task OnInit()
		{
			BattleClientController_CrossArena.<OnInit>d__0 <OnInit>d__;
			<OnInit>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnInit>d__.<>4__this = this;
			<OnInit>d__.<>1__state = -1;
			<OnInit>d__.<>t__builder.Start<BattleClientController_CrossArena.<OnInit>d__0>(ref <OnInit>d__);
			return <OnInit>d__.<>t__builder.Task;
		}

		protected override async Task OnDeInit()
		{
			await this.m_GameCameraController.OnDeInit();
			await this.m_pointController.OnDeInit();
			await this.m_memberFactory.OnDeInit();
		}

		protected override void RegisterEvents()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleStart, new HandlerEvent(this.OnEventBattle));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleJump, new HandlerEvent(this.OnEventJump));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleBack, new HandlerEvent(this.OnEventBack));
		}

		protected override void UnRegisterEvents()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleStart, new HandlerEvent(this.OnEventBattle));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleJump, new HandlerEvent(this.OnEventJump));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleBack, new HandlerEvent(this.OnEventBack));
		}

		public void OnEventBattle(object sender, int type, BaseEventArgs eventArgs)
		{
			base.OnBattleStart();
			Singleton<GameManager>.Instance.ActivePvPSpeed(true);
		}

		protected override void OnGameOver(bool isWin)
		{
			BattleCrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.BattleCrossArenaDataModule);
			bool flag = dataModule.IsWin();
			if (dataModule.CheckAttackerIsMe() && isWin != flag)
			{
				HLog.LogError(HLog.ToColor("(服务器和本地)战斗结果不一致.", 3));
			}
			if (flag)
			{
				GameApp.View.OpenView(ViewName.BattleCrossArenaWinViewModule, null, 1, null, null);
			}
			else
			{
				GameApp.View.OpenView(ViewName.BattleCrossArenaLoseViewModule, null, 1, null, null);
			}
			Singleton<GameManager>.Instance.ActivePvPSpeed(false);
		}

		public void OnEventJump(object sender, int type, BaseEventArgs eventArgs)
		{
			base.OnIsPlaying(false);
			base.OnIsPlayingTask(false);
			base.Jump();
		}

		public void OnEventBack(object sender, int type, BaseEventArgs eventArgs)
		{
			GameApp.State.ActiveState(StateName.LoginState);
		}
	}
}
