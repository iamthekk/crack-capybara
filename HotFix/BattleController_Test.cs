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
	public class BattleController_Test : BattleControllerBase
	{
		public BattleClientController_Test BattleClient
		{
			get
			{
				return this.m_client;
			}
		}

		protected override async Task OnInit()
		{
			this.m_client = new BattleClientController_Test();
			await this.m_client.Init();
			this.SetTestData();
			List<List<CardData>> list = this.CreateWaveData();
			this.PlayServer(out this.m_inBattleData, out this.m_outBattleData, this.m_client.GetAllMembersCardData(), list);
			this.m_client.SetData(this.m_inBattleData, this.m_outBattleData);
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleClientController_Test client = this.m_client;
			if (client == null)
			{
				return;
			}
			client.Update(deltaTime, unscaledDeltaTime);
		}

		protected override async Task OnDeInit()
		{
			BattleClientController_Test client = this.m_client;
			await ((client != null) ? client.DeInit() : null);
			this.m_client = null;
		}

		private List<List<CardData>> CreateWaveData()
		{
			if (Config.WaveList != null)
			{
				List<List<CardData>> list = new List<List<CardData>>();
				for (int i = 0; i < Config.WaveList.Count; i++)
				{
					int num = 2 + i;
					List<int> list2 = Config.WaveList[i];
					if (list2 != null && list2.Count > 0)
					{
						List<CardData> list3 = new List<CardData>();
						list.Add(list3);
						for (int j = 0; j < list2.Count; j++)
						{
							int num2 = 200 + (num - 1) * 10 + j;
							int num3 = list2[j];
							MemberCamp memberCamp = MemberCamp.Enemy;
							bool flag = false;
							MemberPos memberPos = (MemberPos)j;
							GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(num3);
							CardData cardData = new CardData(num2, num3, memberCamp, memberPos, flag);
							cardData.m_memberAttributeData.MergeAttributes(elementById.baseAttributes.GetMergeAttributeData(), false);
							List<int> listInt = elementById.skillIDs.GetListInt('|');
							cardData.AddSkill(listInt);
							list3.Add(cardData);
						}
					}
				}
				return list;
			}
			return null;
		}

		private void SetTestData()
		{
			CMemberBase mainMember = this.m_client.memberFactory.MainMember;
			AddAttributeDataModule dataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			mainMember.m_memberData.cardData.AddSkill(dataModule.SkillIDs);
			mainMember.skillFactory.AddSkills(dataModule.SkillIDs);
			mainMember.m_memberData.cardData.AddSkill(Config.MainRoleAddSkill);
			mainMember.skillFactory.AddSkills(Config.MainRoleAddSkill);
			foreach (CMemberBase cmemberBase in this.m_client.memberFactory.GetMembers.Values)
			{
				GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(cmemberBase.m_memberData.cardData.m_memberID);
				if (elementById != null)
				{
					if (!cmemberBase.m_memberData.cardData.IsPet)
					{
						cmemberBase.m_memberData.cardData.m_memberAttributeData.MergeAttributes(elementById.baseAttributes.GetMergeAttributeData(), false);
					}
					if (cmemberBase.m_memberData.Camp == MemberCamp.Enemy)
					{
						List<int> listInt = elementById.skillIDs.GetListInt('|');
						cmemberBase.m_memberData.cardData.AddSkill(listInt);
						cmemberBase.m_memberData.cardData.AddSkill(Config.EnemysAddSkill);
						cmemberBase.skillFactory.AddSkills(Config.MainRoleAddSkill);
					}
					else if (cmemberBase.m_memberData.Camp == MemberCamp.Friendly && cmemberBase.m_memberData.cardData.m_memberRace == MemberRace.Pet)
					{
						cmemberBase.m_memberData.cardData.AddSkill(Config.PetAddSkill);
						cmemberBase.skillFactory.AddSkills(Config.PetAddSkill);
					}
				}
			}
		}

		public void PlayServer(out InBattleData inBattleData, out OutBattleData outBattleData, List<CardData> cards, List<List<CardData>> otherWareDatas)
		{
			inBattleData = this.GetInBattleDataByCard(cards, otherWareDatas);
			BattleServer battleServer = new BattleServer();
			outBattleData = battleServer.DoBattle(inBattleData, GameApp.Table.GetManager());
		}

		private InBattleData GetInBattleDataByCard(List<CardData> cards, List<List<CardData>> otherWareDatas)
		{
			InBattleData inBattleData = new InBattleData
			{
				m_seed = Random.Range(1, 10000),
				m_durationRound = 30
			};
			for (int i = 0; i < cards.Count; i++)
			{
				inBattleData.m_cardDatas.Add(cards[i]);
			}
			return inBattleData;
		}

		private InBattleData m_inBattleData;

		private OutBattleData m_outBattleData;

		private BattleClientController_Test m_client;
	}
}
