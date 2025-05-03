using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using HotFix.Client;

namespace HotFix
{
	public class BattleClientController_RogueDungeon : BattleClientControllerBase
	{
		protected override Task OnInit()
		{
			BattleClientController_RogueDungeon.<OnInit>d__1 <OnInit>d__;
			<OnInit>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnInit>d__.<>4__this = this;
			<OnInit>d__.<>1__state = -1;
			<OnInit>d__.<>t__builder.Start<BattleClientController_RogueDungeon.<OnInit>d__1>(ref <OnInit>d__);
			return <OnInit>d__.<>t__builder.Task;
		}

		protected override async Task OnDeInit()
		{
			await this.m_GameCameraController.OnDeInit();
			this.m_GameCameraController = null;
			await this.m_pointController.OnDeInit();
			this.m_pointController = null;
			await this.m_sceneMapController.OnDeInit();
			this.m_sceneMapController = null;
			await this.m_eventMemberController.OnDeInit();
			this.m_eventMemberController = null;
			await this.m_memberFactory.OnDeInit();
			this.m_memberFactory = null;
			Singleton<GameManager>.Instance.ActiveGameSpeed(false);
		}

		protected override void RegisterEvents()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleStart, new HandlerEvent(this.OnEventBattle));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleJump, new HandlerEvent(this.OnEventJump));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleBack, new HandlerEvent(this.OnEventBack));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Battle_Pause, new HandlerEvent(this.OnEventBattlePause));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleAddSkill, new HandlerEvent(this.OnEventAddSkill));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleRemoveSkill, new HandlerEvent(this.OnEventRemoveSkill));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ChangeMap, new HandlerEvent(this.OnEventRefreshSceneMap));
		}

		protected override void UnRegisterEvents()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleStart, new HandlerEvent(this.OnEventBattle));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleJump, new HandlerEvent(this.OnEventJump));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleBack, new HandlerEvent(this.OnEventBack));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Battle_Pause, new HandlerEvent(this.OnEventBattlePause));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleAddSkill, new HandlerEvent(this.OnEventAddSkill));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleRemoveSkill, new HandlerEvent(this.OnEventRemoveSkill));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ChangeMap, new HandlerEvent(this.OnEventRefreshSceneMap));
		}

		public void OnEventBattle(object sender, int type, BaseEventArgs eventArgs)
		{
			base.OnBattleStart();
		}

		protected override void OnGameOver(bool isWin)
		{
			RogueDungeonDataModule dataModule = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule);
			bool flag = dataModule.BattleResult > 0;
			string empty = string.Empty;
			if (isWin != flag)
			{
				string text = string.Format("{0}", Singleton<BattleLog>.Instance.SLog);
				string text2 = string.Format("{0}", Singleton<BattleLog>.Instance.CLog);
				HLog.LogError(HLog.ToColor("(服务器和本地)战斗结果不一致.", 3), HLog.ToColor(text, 9), HLog.ToColor(text2, 5));
				flag = false;
			}
			Singleton<GameEventController>.Instance.PlayerData.SetReviveCount(dataModule.CombatReq.ReviveCount);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_GameOverRefreshAttr, null);
			EventArgsGameEnd instance = Singleton<EventArgsGameEnd>.Instance;
			instance.SetData(flag ? GameOverType.Win : GameOverType.Failure);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_GameOver, instance);
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

		public void OnEventBattlePause(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsBool eventArgsBool = eventArgs as EventArgsBool;
			if (eventArgsBool != null)
			{
				base.SetPause(eventArgsBool.Value);
				EventMemberController eventMemberController = this.m_eventMemberController;
				if (eventMemberController != null)
				{
					eventMemberController.OnPause(eventArgsBool.Value);
				}
				GameCameraController gameCameraController = this.m_GameCameraController;
				if (gameCameraController != null)
				{
					gameCameraController.OnPause(eventArgsBool.Value);
				}
				SceneMapController sceneMapController = this.m_sceneMapController;
				if (sceneMapController == null)
				{
					return;
				}
				sceneMapController.OnPause(eventArgsBool.Value);
			}
		}

		private async void OnEventAddSkill(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgAddSkills eventArgAddSkills = eventArgs as EventArgAddSkills;
			if (eventArgAddSkills != null)
			{
				await this.AddSkill(eventArgAddSkills.skills);
			}
		}

		private void OnEventRemoveSkill(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgRemoveSkills eventArgRemoveSkills = eventArgs as EventArgRemoveSkills;
			if (eventArgRemoveSkills != null)
			{
				this.RemoveSkill(eventArgRemoveSkills.skills);
			}
		}

		private async void OnEventRefreshSceneMap(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgChangeMap eventArgChangeMap = eventArgs as EventArgChangeMap;
			if (eventArgChangeMap != null)
			{
				await this.m_sceneMapController.RefreshSceneMap(eventArgChangeMap.mapId);
			}
		}

		private async Task AddSkill(List<int> ids)
		{
			await this.m_memberFactory.MainMember.skillFactory.AddSkills(ids);
		}

		private void RemoveSkill(List<int> ids)
		{
			this.m_memberFactory.MainMember.skillFactory.RemoveAllSkill(ids);
		}

		public EventMemberController m_eventMemberController;
	}
}
