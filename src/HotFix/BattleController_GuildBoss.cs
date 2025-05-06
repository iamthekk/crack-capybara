using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using LocalModels;
using Proto.Battle;
using Proto.Common;
using Server;

namespace HotFix
{
	public class BattleController_GuildBoss : BattleControllerBase
	{
		public BattleClientController_GuildBoss BattleClient
		{
			get
			{
				return this.m_client;
			}
		}

		protected override async Task OnInit()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_GuildBossBattle_Start, new HandlerEvent(this.OnEventGuildBossBattleStart));
			this.m_client = new BattleClientController_GuildBoss();
			await this.m_client.Init();
		}

		protected override async Task OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_GuildBossBattle_Start, new HandlerEvent(this.OnEventGuildBossBattleStart));
			BattleClientController_GuildBoss client = this.m_client;
			if (client != null)
			{
				client.DeInit();
			}
			this.m_client = null;
			await Task.CompletedTask;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleClientController_GuildBoss client = this.m_client;
			if (client == null)
			{
				return;
			}
			client.Update(deltaTime, unscaledDeltaTime);
		}

		public void OnEventGuildBossBattleStart(object sender, int type, BaseEventArgs eventArgs)
		{
			this.BattleStart();
		}

		public void BattleStart()
		{
			if (this.m_client == null)
			{
				return;
			}
			this.PlayServer(out this.m_inBattleData, out this.m_outBattleData);
			this.m_client.SetData(this.m_inBattleData, this.m_outBattleData);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_GuildBossBattle_ShowBoss, null);
		}

		public void PlayServer(out InBattleData inBattleData, out OutBattleData outBattleData)
		{
			inBattleData = this.GetInBattleDataByServer();
			BattleServer battleServer = new BattleServer();
			outBattleData = battleServer.DoBattle(inBattleData, GameApp.Table.GetManager());
		}

		private InBattleData GetInBattleDataByServer()
		{
			BattleGuildBossDataModule dataModule = GameApp.Data.GetDataModule(DataName.BattleGuildBossDataModule);
			RGuildBossCombatReq rguildBossCombatReq = new RGuildBossCombatReq
			{
				ConfigId = dataModule.guildBossBattleResponse.ConfigId,
				UserInfo = dataModule.guildBossBattleResponse.UserInfo,
				Seed = dataModule.guildBossBattleResponse.Seed,
				BossHp = dataModule.guildBossBattleResponse.BeforeHp
			};
			List<CardData> list = new List<CardData>();
			LocalModelManager manager = GameApp.Table.GetManager();
			CardData cardData;
			string text;
			rguildBossCombatReq.GetGuildBossMainPlayer(manager, out cardData, out text);
			list.Add(cardData);
			List<CardData> list2;
			rguildBossCombatReq.UserInfo.Pets.ToList<PetDto>().PetFilter(manager, MemberCamp.Friendly, out list2);
			list.AddRange(list2);
			List<CardData> guildBossEnemyCardDatas = GuildController.GetGuildBossEnemyCardDatas(manager, rguildBossCombatReq.ConfigId, rguildBossCombatReq.BossHp);
			list.AddRange(guildBossEnemyCardDatas);
			return new InBattleData
			{
				m_cardDatas = list,
				m_seed = rguildBossCombatReq.Seed,
				m_durationRound = 15,
				m_waveMax = 1
			};
		}

		private InBattleData m_inBattleData;

		private OutBattleData m_outBattleData;

		private BattleClientController_GuildBoss m_client;
	}
}
