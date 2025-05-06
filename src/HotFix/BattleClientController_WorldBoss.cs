using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using HotFix.Client;
using LocalModels.Bean;
using Proto.Common;
using Server;
using UnityEngine;

namespace HotFix
{
	public class BattleClientController_WorldBoss : BattleClientControllerBase
	{
		protected override Task OnInit()
		{
			BattleClientController_WorldBoss.<OnInit>d__3 <OnInit>d__;
			<OnInit>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnInit>d__.<>4__this = this;
			<OnInit>d__.<>1__state = -1;
			<OnInit>d__.<>t__builder.Start<BattleClientController_WorldBoss.<OnInit>d__3>(ref <OnInit>d__);
			return <OnInit>d__.<>t__builder.Task;
		}

		protected override async Task OnDeInit()
		{
			await this.m_GameCameraController.OnDeInit();
			await this.m_pointController.OnDeInit();
			await this.m_memberFactory.OnDeInit();
			await this.m_worldBossSceneMapController.OnDeInit();
		}

		protected override void RegisterEvents()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleStart, new HandlerEvent(this.OnEventBattle));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleJump, new HandlerEvent(this.OnEventJump));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleBack, new HandlerEvent(this.OnEventBack));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleAddSkill, new HandlerEvent(this.OnEventAddSkill));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleRemoveSkill, new HandlerEvent(this.OnEventRemoveSkill));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_WorldBossBattle_ShowBoss, new HandlerEvent(this.OnEventShowBoss));
		}

		protected override void UnRegisterEvents()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleStart, new HandlerEvent(this.OnEventBattle));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleJump, new HandlerEvent(this.OnEventJump));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleBack, new HandlerEvent(this.OnEventBack));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleAddSkill, new HandlerEvent(this.OnEventAddSkill));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleRemoveSkill, new HandlerEvent(this.OnEventRemoveSkill));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_WorldBossBattle_ShowBoss, new HandlerEvent(this.OnEventShowBoss));
		}

		private void OnEventShowBoss(object sender, int type, BaseEventArgs eventArgs)
		{
			this.ShowBoss();
		}

		private void OnEventBattle(object sender, int type, BaseEventArgs eventArgs)
		{
			base.OnBattleStart();
			Singleton<GameManager>.Instance.ActivePvESpeed(true);
		}

		protected override void OnGameOver(bool isWin)
		{
			WorldBossDataModule dataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
			bool flag = dataModule.mEndWorldBossResponse.Result.Equals(1U);
			if (isWin != flag)
			{
				HLog.LogError(HLog.ToColor("(服务器和本地)战斗结果不一致.", 3));
			}
			BattleResultWorldBossViewModule.OpenData openData = new BattleResultWorldBossViewModule.OpenData();
			openData.IsSuccess = true;
			openData.CurrentDamage = (long)dataModule.mEndWorldBossResponse.Damage;
			openData.TotalDamage = (long)dataModule.mEndWorldBossResponse.TotalDamage;
			openData.BestDamage = dataModule.BestDamage;
			RepeatedField<RewardDto> reward = dataModule.mEndWorldBossResponse.CommonData.Reward;
			int count = reward.Count;
			openData.Rewards = new PropData[count];
			for (int i = 0; i < count; i++)
			{
				openData.Rewards[i] = reward[i].ToPropData();
			}
			GameApp.View.OpenView(ViewName.BattleResultViewWorldBossModule, openData, 1, null, null);
			Singleton<GameManager>.Instance.ActivePvESpeed(false);
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

		private async Task AddSkill(List<int> ids)
		{
			await this.m_memberFactory.MainMember.skillFactory.AddSkills(ids);
		}

		private void RemoveSkill(List<int> ids)
		{
			this.m_memberFactory.MainMember.skillFactory.RemoveAllSkill(ids);
		}

		private async void ShowBoss()
		{
			this.enemyParent.SetActiveSafe(false);
			List<Task> list = new List<Task>();
			List<OutMemberData> members = this.m_outBattleData.m_createData.GetMembers(this.curWave);
			for (int i = 0; i < members.Count; i++)
			{
				OutMemberData outMemberData = members[i];
				if (outMemberData.m_camp == MemberCamp.Enemy)
				{
					GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(outMemberData.m_memberID);
					CMemberData cmemberData = new CMemberData();
					cmemberData.SetTableData(elementById);
					cmemberData.SetMemberData(outMemberData.m_memberInstanceID, outMemberData.m_camp, outMemberData.m_isMainMember, outMemberData.m_posIndex, outMemberData.m_curHp, outMemberData.m_maxHp);
					cmemberData.cardData.SetMemberRace(outMemberData.m_memberRace);
					ClientPointData pointByIndex = this.m_pointController.GetPointByIndex(outMemberData.m_camp, outMemberData.m_posIndex);
					Vector3 position = pointByIndex.GetPosition();
					Task task = this.m_memberFactory.CreateMember(position, cmemberData, pointByIndex, this.enemyParent.transform);
					list.Add(task);
				}
			}
			await Task.WhenAll(list);
			float num = 0f;
			foreach (CMemberBase cmemberBase in this.m_memberFactory.GetMembers.Values)
			{
				if (cmemberBase.m_memberData.Camp == MemberCamp.Enemy)
				{
					cmemberBase.SetAlpha(1f);
					cmemberBase.PlayAnimation("Appear");
					cmemberBase.AddAnimation("Idle");
					float animationDuration = cmemberBase.GetAnimationDuration("Appear");
					if (num < animationDuration)
					{
						num = animationDuration;
					}
				}
			}
			this.enemyParent.SetActiveSafe(true);
			DelayCall.Instance.CallOnce((int)(num * 1000f), delegate
			{
				BattleFightViewModule.OpenData openData = new BattleFightViewModule.OpenData();
				openData.aniFinish = delegate
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleStart, null);
				};
				openData.spinOffsetY = 0f;
				GameApp.View.OpenView(ViewName.BattleFightViewModule, openData, 1, null, null);
			});
		}

		protected override void DoReport_Report_Hurt(BaseBattleReportData data)
		{
			base.DoReport_Report_Hurt(data);
			ValueTuple<FP, FP> hurtReportData = base.GetHurtReportData(data);
			if (hurtReportData.Item1 <= 0)
			{
				return;
			}
			EventArgBattleDamageUpdate instance = Singleton<EventArgBattleDamageUpdate>.Instance;
			instance.CurDamage = hurtReportData.Item1.RoundToLong();
			instance.CurHP = hurtReportData.Item2.RoundToLong();
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_BattleDamageUpdate, instance);
		}

		private WorldBossMapController m_worldBossSceneMapController;

		private GameObject playerParent;

		private GameObject enemyParent;
	}
}
