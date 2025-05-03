using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;

namespace HotFix
{
	public class BattleClientController_Conquer : BattleClientControllerBase
	{
		protected override Task OnInit()
		{
			BattleClientController_Conquer.<OnInit>d__0 <OnInit>d__;
			<OnInit>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnInit>d__.<>4__this = this;
			<OnInit>d__.<>1__state = -1;
			<OnInit>d__.<>t__builder.Start<BattleClientController_Conquer.<OnInit>d__0>(ref <OnInit>d__);
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
		}

		protected override void OnGameOver(bool isWin)
		{
			if (GameApp.Data.GetDataModule(DataName.BattleConquerDataModule).IsWin())
			{
				GameApp.View.OpenView(ViewName.BattleConquerWinViewModule, null, 1, null, null);
				return;
			}
			GameApp.View.OpenView(ViewName.BattleConquerLoseViewModule, null, 1, null, null);
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
