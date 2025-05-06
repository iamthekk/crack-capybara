using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using LocalModels;
using Proto.Battle;
using Proto.Common;
using Server;

namespace HotFix
{
	public class BattleController_Tower : BattleControllerBase
	{
		public BattleClientController_Tower BattleClient
		{
			get
			{
				return this.m_client;
			}
		}

		protected override async Task OnInit()
		{
			this.PlayServer(out this.m_inBattleData, out this.m_outBattleData);
			this.m_client = new BattleClientController_Tower();
			this.m_client.SetData(this.m_inBattleData, this.m_outBattleData);
			await this.m_client.Init();
		}

		protected override async Task OnDeInit()
		{
			BattleClientController_Tower client = this.m_client;
			if (client != null)
			{
				client.DeInit();
			}
			this.m_client = null;
			await Task.CompletedTask;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleClientController_Tower client = this.m_client;
			if (client == null)
			{
				return;
			}
			client.Update(deltaTime, unscaledDeltaTime);
		}

		public void PlayServer(out InBattleData inBattleData, out OutBattleData outBattleData)
		{
			inBattleData = this.GetInBattleDataByServer();
			BattleServer battleServer = new BattleServer();
			outBattleData = battleServer.DoBattle(inBattleData, GameApp.Table.GetManager());
		}

		private InBattleData GetInBattleDataByServer()
		{
			BattleTowerDataModule dataModule = GameApp.Data.GetDataModule(DataName.BattleTowerDataModule);
			RTowerCombatReq rtowerCombatReq = new RTowerCombatReq
			{
				Tower = (int)dataModule.m_towerChallengeResponse.ConfigId,
				UserInfo = dataModule.m_towerChallengeResponse.UserInfo,
				Seed = dataModule.m_towerChallengeResponse.Seed
			};
			LocalModelManager manager = GameApp.Table.GetManager();
			List<CardData> list = new List<CardData>();
			CardData cardData;
			rtowerCombatReq.GetTowerMainPlayer(manager, out cardData);
			list.Add(cardData);
			List<CardData> list2;
			rtowerCombatReq.UserInfo.Pets.ToList<PetDto>().PetFilter(manager, MemberCamp.Friendly, out list2);
			list.AddRange(list2);
			List<CardData> towerEnemyCardDatas = TowerController.GetTowerEnemyCardDatas(manager, rtowerCombatReq.Tower);
			list.AddRange(towerEnemyCardDatas);
			return new InBattleData
			{
				m_cardDatas = list,
				m_seed = rtowerCombatReq.Seed,
				m_durationRound = 15
			};
		}

		private InBattleData m_inBattleData;

		private OutBattleData m_outBattleData;

		private BattleClientController_Tower m_client;
	}
}
