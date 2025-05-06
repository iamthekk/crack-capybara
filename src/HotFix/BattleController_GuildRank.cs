using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Server;

namespace HotFix
{
	public class BattleController_GuildRank : BattleControllerBase
	{
		public BattleClientController_GuildRank BattleClient
		{
			get
			{
				return this.m_client;
			}
		}

		protected override async Task OnInit()
		{
			this.PlayServer(out this.m_inBattleData, out this.m_outBattleData);
			this.m_client = new BattleClientController_GuildRank();
			this.m_client.SetData(this.m_inBattleData, this.m_outBattleData);
			await this.m_client.Init();
		}

		protected override async Task OnDeInit()
		{
			BattleClientController_GuildRank client = this.m_client;
			await ((client != null) ? client.DeInit() : null);
			this.m_client = null;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleClientController_GuildRank client = this.m_client;
			if (client == null)
			{
				return;
			}
			client.Update(deltaTime, unscaledDeltaTime);
		}

		public void PlayServer(out InBattleData inBattleData, out OutBattleData outBattleData)
		{
			inBattleData = this.GetInBattleDataByModel();
			BattleServer battleServer = new BattleServer();
			outBattleData = battleServer.DoBattle(inBattleData, GameApp.Table.GetManager());
		}

		private InBattleData GetInBattleDataByModel()
		{
			InBattleData inBattleData = new InBattleData();
			BattleGuildRankDataModule dataModule = GameApp.Data.GetDataModule(DataName.BattleGuildRankDataModule);
			inBattleData.m_seed = dataModule.Record.Seed;
			inBattleData.m_durationRound = dataModule.Duration;
			List<CardData> list = dataModule.Record.OwnerUser.ToCardDatas(MemberCamp.Friendly, GameApp.Table.GetManager());
			List<CardData> list2 = dataModule.Record.OtherUser.ToCardDatas(MemberCamp.Enemy, GameApp.Table.GetManager());
			inBattleData.m_cardDatas.AddRange(list);
			inBattleData.m_cardDatas.AddRange(list2);
			return inBattleData;
		}

		private InBattleData m_inBattleData;

		private OutBattleData m_outBattleData;

		private BattleClientController_GuildRank m_client;
	}
}
