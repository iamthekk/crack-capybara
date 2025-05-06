using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using Proto.Common;
using Server;

namespace HotFix
{
	public class BattleController_CrossArena : BattleControllerBase
	{
		public BattleClientController_CrossArena BattleClient
		{
			get
			{
				return this.m_client;
			}
		}

		protected override async Task OnInit()
		{
			this.PlayServer(out this.m_inBattleData, out this.m_outBattleData);
			this.m_client = new BattleClientController_CrossArena();
			this.m_client.SetData(this.m_inBattleData, this.m_outBattleData);
			await this.m_client.Init();
		}

		protected override async Task OnDeInit()
		{
			BattleClientController_CrossArena client = this.m_client;
			await ((client != null) ? client.DeInit() : null);
			this.m_client = null;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleClientController_CrossArena client = this.m_client;
			if (client == null)
			{
				return;
			}
			client.Update(deltaTime, unscaledDeltaTime);
		}

		public void PlayServer(out InBattleData inBattleData, out OutBattleData outBattleData)
		{
			inBattleData = this.GetInBattleData();
			BattleServer battleServer = new BattleServer();
			outBattleData = battleServer.DoBattle(inBattleData, GameApp.Table.GetManager());
		}

		private InBattleData GetInBattleData()
		{
			InBattleData inBattleData = new InBattleData();
			BattleCrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.BattleCrossArenaDataModule);
			CardData mainCardData = dataModule.Record.OwnerUser.GetMainCardData(MemberCamp.Friendly, GameApp.Table.GetManager());
			if (mainCardData != null)
			{
				inBattleData.m_cardDatas.Add(mainCardData);
			}
			else
			{
				HLog.LogError("BattleController_CrossArena. 服务器友方主角数据错误.");
			}
			List<CardData> list;
			dataModule.Record.OwnerUser.Pets.ToList<PetDto>().PetFilter(GameApp.Table.GetManager(), MemberCamp.Friendly, out list);
			inBattleData.m_cardDatas.AddRange(list);
			CardData mainCardData2 = dataModule.Record.OtherUser.GetMainCardData(MemberCamp.Enemy, GameApp.Table.GetManager());
			if (mainCardData2 != null)
			{
				mainCardData2.IsEnemyPlayer = true;
				inBattleData.m_cardDatas.Add(mainCardData2);
			}
			else
			{
				HLog.LogError("BattleController_CrossArena. 服务器敌方主角数据错误.");
			}
			List<CardData> list2;
			dataModule.Record.OtherUser.Pets.ToList<PetDto>().PetFilter(GameApp.Table.GetManager(), MemberCamp.Enemy, out list2);
			inBattleData.m_cardDatas.AddRange(list2);
			inBattleData.m_seed = dataModule.Record.Seed;
			inBattleData.m_durationRound = dataModule.Duration;
			inBattleData.m_battleMode = BattleMode.PVP;
			return inBattleData;
		}

		private long GetBattlePower(CardData cardData, List<CardData> petFightCardDatas)
		{
			CombatData combatData = new CombatData();
			combatData.MathCombat(GameApp.Table.GetManager(), cardData.m_memberAttributeData, cardData.skillIDs);
			for (int i = 0; i < petFightCardDatas.Count; i++)
			{
				CardData cardData2 = petFightCardDatas[i];
				combatData.MathSkillCombat(GameApp.Table.GetManager(), cardData2.skillIDs);
			}
			return (long)combatData.CurComba;
		}

		protected InBattleData m_inBattleData;

		protected OutBattleData m_outBattleData;

		protected BattleClientController_CrossArena m_client;
	}
}
