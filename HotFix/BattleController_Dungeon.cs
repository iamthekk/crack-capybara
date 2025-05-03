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
	public class BattleController_Dungeon : BattleControllerBase
	{
		public BattleClientController_Dungeon BattleClient
		{
			get
			{
				return this.m_client;
			}
		}

		protected override async Task OnInit()
		{
			this.PlayServer(out this.m_inBattleData, out this.m_outBattleData);
			switch (this.dungeonID)
			{
			case DungeonID.DragonsLair:
				this.m_client = new BattleClientController_DungeonDragonLair();
				break;
			case DungeonID.AstralTree:
				this.m_client = new BattleClientController_DungeonAstralTree();
				break;
			case DungeonID.SwordIsland:
				this.m_client = new BattleClientController_DungeonSwordIsland();
				break;
			case DungeonID.DeepSeaRuins:
				this.m_client = new BattleClientController_DungeonDeepSeaRuins();
				break;
			default:
				HLog.LogError(string.Format("未定义的副本类型{0}", this.dungeonID));
				return;
			}
			this.m_client.SetData(this.m_inBattleData, this.m_outBattleData);
			await this.m_client.Init();
		}

		protected override async Task OnDeInit()
		{
			BattleClientController_Dungeon client = this.m_client;
			if (client != null)
			{
				client.DeInit();
			}
			this.m_client = null;
			await Task.CompletedTask;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleClientController_Dungeon client = this.m_client;
			if (client == null)
			{
				return;
			}
			client.Update(deltaTime, unscaledDeltaTime);
		}

		public void SetData(DungeonID id)
		{
			this.dungeonID = id;
		}

		public void PlayServer(out InBattleData inBattleData, out OutBattleData outBattleData)
		{
			inBattleData = this.GetInBattleDataByServer();
			BattleServer battleServer = new BattleServer();
			outBattleData = battleServer.DoBattle(inBattleData, GameApp.Table.GetManager());
		}

		private InBattleData GetInBattleDataByServer()
		{
			DungeonDataModule dataModule = GameApp.Data.GetDataModule(DataName.DungeonDataModule);
			RDungeonCombatReq rdungeonCombatReq = new RDungeonCombatReq
			{
				DungeonId = dataModule.DungeonResponse.DungeonId,
				LevelId = dataModule.DungeonResponse.LevelId,
				UserInfo = dataModule.DungeonResponse.UserInfo,
				Seed = dataModule.DungeonResponse.Seed
			};
			LocalModelManager manager = GameApp.Table.GetManager();
			List<CardData> list = new List<CardData>();
			CardData cardData;
			rdungeonCombatReq.GetDungeonMainPlayer(manager, out cardData);
			list.Add(cardData);
			List<CardData> list2;
			rdungeonCombatReq.UserInfo.Pets.ToList<PetDto>().PetFilter(manager, MemberCamp.Friendly, out list2);
			list.AddRange(list2);
			List<CardData> dungeonEnemyCardDatas = DungeonController.GetDungeonEnemyCardDatas(manager, rdungeonCombatReq.DungeonId, rdungeonCombatReq.LevelId);
			list.AddRange(dungeonEnemyCardDatas);
			return new InBattleData
			{
				m_cardDatas = list,
				m_seed = rdungeonCombatReq.Seed,
				m_durationRound = 15
			};
		}

		private InBattleData m_inBattleData;

		private OutBattleData m_outBattleData;

		private BattleClientController_Dungeon m_client;

		private DungeonID dungeonID;
	}
}
