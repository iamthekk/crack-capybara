using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using HotFix.Client;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class BattleClientController_Test : BattleClientControllerBase
	{
		protected override Task OnInit()
		{
			BattleClientController_Test.<OnInit>d__0 <OnInit>d__;
			<OnInit>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnInit>d__.<>4__this = this;
			<OnInit>d__.<>1__state = -1;
			<OnInit>d__.<>t__builder.Start<BattleClientController_Test.<OnInit>d__0>(ref <OnInit>d__);
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

		protected override void OnGameOver(bool isWin)
		{
			ViewName viewName = (isWin ? ViewName.BattleTestWinViewModule : ViewName.BattleTestLoseViewModule);
			GameApp.View.OpenView(viewName, null, 1, null, null);
		}

		public void OnEventBattle(object sender, int type, BaseEventArgs eventArgs)
		{
			base.OnBattleStart();
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

		private async Task CreateMember(int GUID, int memberID, MemberCamp camp, bool isMainMember, MemberPos posIndex, Transform parent)
		{
			await this.CreateMember(GUID, memberID, camp, isMainMember, posIndex, parent, -1, -1);
		}

		private async Task CreateMember(int GUID, int memberID, MemberCamp camp, bool isMainMember, MemberPos posIndex, Transform parent, FP curHp, FP maxHp)
		{
			GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(memberID);
			if (elementById != null)
			{
				CMemberData cmemberData = new CMemberData();
				cmemberData.SetTableData(elementById);
				cmemberData.SetMemberData(GUID, camp, isMainMember, posIndex, curHp, maxHp);
				if (isMainMember)
				{
					cmemberData.cardData.SetMemberRace(MemberRace.Hero);
					List<MergeAttributeData> mergeAttributeData = Config.RoleAttributeAdd.GetMergeAttributeData();
					cmemberData.cardData.m_memberAttributeData.MergeAttributes(mergeAttributeData, false);
				}
				ClientPointData pointByIndex = this.m_pointController.GetPointByIndex(cmemberData.Camp, cmemberData.PosIndex);
				Vector3 position = pointByIndex.GetPosition();
				await this.m_memberFactory.CreateMember(position, cmemberData, pointByIndex, parent);
			}
		}

		private async Task CreateMember(CardData cardData)
		{
			GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(cardData.m_memberID);
			if (elementById != null)
			{
				CMemberData cmemberData = new CMemberData(cardData);
				cmemberData.SetTableData(elementById);
				ClientPointData pointByIndex = this.m_pointController.GetPointByIndex(cardData.m_camp, cardData.m_posIndex);
				Vector3 position = pointByIndex.GetPosition();
				await this.m_memberFactory.CreateMember(position, cmemberData, pointByIndex, null);
			}
		}

		public List<CardData> GetAllMembersCardData()
		{
			List<CardData> list = new List<CardData>();
			foreach (CMemberBase cmemberBase in this.m_memberFactory.GetMembers.Values)
			{
				list.Add(cmemberBase.m_memberData.cardData);
			}
			return list;
		}
	}
}
