using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using HotFix.Client;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class BattleMemberController : MapMemberControllerBase
	{
		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.isNewWaveEnter)
			{
				this.NewWaveEnterMove(deltaTime);
			}
		}

		public virtual async Task WaveChange(int waveIndex, List<CardData> cardDatas, ClientPointController pointCtrl, CMemberFactory cMemberFactory, TaskOutValue<List<CMemberBase>> outMembers)
		{
			this.waveEnemyList.Clear();
			if (pointCtrl == null || cMemberFactory == null)
			{
				outMembers.SetValue(this.waveEnemyList);
			}
			else
			{
				for (int i = 0; i < cardDatas.Count; i++)
				{
					CardData cardData = cardDatas[i];
					GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(cardData.m_memberID);
					if (elementById == null)
					{
						return;
					}
					CMemberData cmemberData = new CMemberData(cardData);
					cmemberData.SetTableData(elementById);
					ClientPointData pointByIndex = pointCtrl.GetPointByIndex(cardData.m_camp, cardData.m_posIndex);
					Vector3 position = pointByIndex.GetPosition();
					await cMemberFactory.CreateMember(position, cmemberData, pointByIndex, null);
					CMemberBase member = cMemberFactory.GetMember(cardData.m_instanceID);
					if (member != null)
					{
						this.waveEnemyList.Add(member);
					}
					cardData = null;
				}
				outMembers.SetValue(this.waveEnemyList);
			}
		}

		public virtual void WaveEnter(int remainEnterFrameCount)
		{
			this.startPoints.Clear();
			this.endPoints.Clear();
			this.newWaveMoveTime = Config.GetTimeByFrame(remainEnterFrameCount);
			for (int i = 0; i < this.waveEnemyList.Count; i++)
			{
				CMemberBase cmemberBase = this.waveEnemyList[i];
				cmemberBase.InitStartPosition();
				Vector3 startPosition = cmemberBase.GetStartPosition();
				cmemberBase.m_gameObject.transform.position = startPosition + Vector3.right * GameConfig.GameBattle_WavePoint_Distance;
				this.startPoints.Add(cmemberBase.m_gameObject.transform.position);
				this.endPoints.Add(startPosition);
				cmemberBase.PlayAnimation("Run");
			}
			this.elapsedTime = 0f;
			this.isNewWaveEnter = true;
		}

		protected virtual void NewWaveEnterMove(float deltaTime)
		{
			this.elapsedTime += deltaTime;
			float num = this.elapsedTime / this.newWaveMoveTime;
			for (int i = 0; i < this.waveEnemyList.Count; i++)
			{
				this.waveEnemyList[i].m_gameObject.transform.position = Vector3.Lerp(this.startPoints[i], this.endPoints[i], num);
			}
			if (num >= 1f)
			{
				this.isNewWaveEnter = false;
				for (int j = 0; j < this.waveEnemyList.Count; j++)
				{
					this.waveEnemyList[j].m_gameObject.transform.position = this.endPoints[j];
				}
				this.NewWaveArrive();
			}
		}

		protected virtual void NewWaveArrive()
		{
			for (int i = 0; i < this.waveEnemyList.Count; i++)
			{
				this.waveEnemyList[i].PlayAnimation("Idle");
			}
			this.waveEnemyList.Clear();
		}

		public async Task CreatePlayer(ClientPointController pointCtrl, CMemberFactory memberFac, Transform parent, List<int> skillIds)
		{
			TaskOutValue<CMemberBase> outMember = new TaskOutValue<CMemberBase>();
			await this.CreateMember(pointCtrl, memberFac, 100, Singleton<GameEventController>.Instance.PlayerData.PlayerMemberId, MemberCamp.Friendly, true, MemberPos.One, parent, -1, -1, outMember, skillIds);
			if (outMember.Value != null)
			{
				ModelUtils.CreateBattlePlayerMountModel(outMember.Value);
			}
		}

		public async Task CreatePets(ClientPointController pointCtrl, CMemberFactory memberFac, Transform parent)
		{
			List<CardData> petCards = Singleton<GameEventController>.Instance.PlayerData.PetCards;
			for (int i = 0; i < petCards.Count; i++)
			{
				CardData card = petCards[i];
				TaskOutValue<CMemberBase> outPet = new TaskOutValue<CMemberBase>();
				await this.CreateMember(pointCtrl, memberFac, card.m_instanceID, card.m_memberID, card.m_camp, card.m_isMainMember, card.m_posIndex, parent, -1, -1, outPet, null);
				if (outPet.Value != null)
				{
					outPet.Value.m_memberData.cardData.CloneFrom(card);
				}
				card = null;
				outPet = null;
			}
		}

		public async Task CreateMember(ClientPointController pointCtrl, CMemberFactory memberFac, int GUID, int memberID, MemberCamp camp, bool isMainMember, MemberPos posIndex, Transform parent, FP curHp, FP maxHp, TaskOutValue<CMemberBase> outMember, List<int> skillIds)
		{
			if (pointCtrl != null && memberFac != null)
			{
				GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(memberID);
				if (elementById == null)
				{
					HLog.LogError(string.Format("Not found member id={0}", memberID));
				}
				else
				{
					CMemberData cmemberData = new CMemberData();
					cmemberData.SetTableData(elementById);
					cmemberData.SetMemberData(GUID, camp, isMainMember, posIndex, curHp, maxHp);
					if (camp == MemberCamp.Friendly)
					{
						cmemberData.cardData.SetMemberRace(isMainMember ? MemberRace.Hero : MemberRace.Pet);
					}
					else
					{
						cmemberData.cardData.SetMemberRace(MemberRace.Hero);
					}
					ClientPointData pointByIndex = pointCtrl.GetPointByIndex(cmemberData.Camp, cmemberData.PosIndex);
					Vector3 position = pointByIndex.GetPosition();
					await memberFac.CreateMember(position, cmemberData, pointByIndex, parent);
					CMemberBase member = memberFac.GetMember(GUID);
					if (outMember != null && member != null)
					{
						outMember.SetValue(member);
					}
				}
			}
		}

		public async Task CreateMember(ClientPointController pointCtrl, CMemberFactory memberFac, CardData cardData, Transform parent, TaskOutValue<CMemberBase> outMember)
		{
			if (pointCtrl != null && memberFac != null)
			{
				if (cardData != null)
				{
					GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(cardData.m_memberID);
					if (elementById != null)
					{
						CMemberData cmemberData = new CMemberData(cardData);
						cmemberData.SetTableData(elementById);
						if (cardData.m_camp == MemberCamp.Friendly)
						{
							cmemberData.cardData.SetMemberRace(cardData.m_isMainMember ? MemberRace.Hero : MemberRace.Pet);
						}
						else
						{
							cmemberData.cardData.SetMemberRace(MemberRace.Hero);
						}
						ClientPointData pointByIndex = pointCtrl.GetPointByIndex(cardData.m_camp, cardData.m_posIndex);
						Vector3 position = pointByIndex.GetPosition();
						await memberFac.CreateMember(position, cmemberData, pointByIndex, parent);
						CMemberBase member = memberFac.GetMember(cardData.m_instanceID);
						if (outMember != null && member != null)
						{
							outMember.SetValue(member);
						}
					}
				}
			}
		}

		protected float newWaveMoveTime;

		protected float elapsedTime;

		protected bool isNewWaveEnter;

		private List<CMemberBase> waveEnemyList = new List<CMemberBase>();

		private List<Vector3> startPoints = new List<Vector3>();

		private List<Vector3> endPoints = new List<Vector3>();
	}
}
