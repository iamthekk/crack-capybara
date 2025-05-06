using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using LocalModels;
using Proto.Battle;
using Proto.Common;
using Server;

namespace HotFix
{
	public class BattleController_WorldBoss : BattleControllerBase
	{
		public BattleClientController_WorldBoss BattleClient
		{
			get
			{
				return this.m_client;
			}
		}

		protected override async Task OnInit()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_WorldBossBattle_Start, new HandlerEvent(this.OnEventWorldBossBattleStart));
			this.m_client = new BattleClientController_WorldBoss();
			await this.m_client.Init();
		}

		protected override async Task OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_WorldBossBattle_Start, new HandlerEvent(this.OnEventWorldBossBattleStart));
			BattleClientController_WorldBoss client = this.m_client;
			if (client != null)
			{
				client.DeInit();
			}
			this.m_client = null;
			await Task.CompletedTask;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleClientController_WorldBoss client = this.m_client;
			if (client == null)
			{
				return;
			}
			client.Update(deltaTime, unscaledDeltaTime);
		}

		public void OnEventWorldBossBattleStart(object sender, int type, BaseEventArgs eventArgs)
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
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_WorldBossBattle_ShowBoss, null);
		}

		public void PlayServer(out InBattleData inBattleData, out OutBattleData outBattleData)
		{
			inBattleData = this.GetInBattleDataByServer();
			BattleServer battleServer = new BattleServer();
			outBattleData = battleServer.DoBattle(inBattleData, GameApp.Table.GetManager());
		}

		private InBattleData GetInBattleDataByServer()
		{
			WorldBossDataModule dataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
			StringBuilder stringBuilder = new StringBuilder();
			if (dataModule.mEndWorldBossResponse == null)
			{
				HLog.LogError(HLog.ToColor("【世界BOSS战斗】服务器数据异常.", 3));
				return new InBattleData();
			}
			RWorldBossCombatReq rworldBossCombatReq = new RWorldBossCombatReq
			{
				ConfigId = dataModule.mEndWorldBossResponse.WorldBossInfo.MissionId,
				UserInfo = dataModule.mEndWorldBossResponse.UserInfo,
				Seed = dataModule.mEndWorldBossResponse.Seed,
				MonsterCfgId = dataModule.mEndWorldBossResponse.ConfigId
			};
			string text = "SLogID=" + dataModule.mEndWorldBossResponse.BattleServerLogId + "\n" + dataModule.mEndWorldBossResponse.BattleServerLogData;
			Singleton<BattleLog>.Instance.ClearSLog();
			Singleton<BattleLog>.Instance.AddSLog(text, dataModule.mEndWorldBossResponse.BattleServerLogId);
			string text2 = string.Format("[BattleCheck]:request configId={0} seed={1} ", rworldBossCombatReq.ConfigId, rworldBossCombatReq.Seed);
			string text3 = "UserInfo=" + JsonManager.SerializeObject(rworldBossCombatReq.UserInfo) + " ";
			stringBuilder.Append(text2);
			stringBuilder.Append(text3);
			LocalModelManager manager = GameApp.Table.GetManager();
			List<CardData> list = new List<CardData>();
			CardData cardData;
			string text4;
			rworldBossCombatReq.GetWorldBossMainPlayer(manager, out cardData, out text4);
			list.Add(cardData);
			List<CardData> list2;
			rworldBossCombatReq.UserInfo.Pets.ToList<PetDto>().PetFilter(manager, MemberCamp.Friendly, out list2);
			list.AddRange(list2);
			List<CardData> worldBossBattleEnemy = WorldBossController.GetWorldBossBattleEnemy(manager, rworldBossCombatReq.ConfigId, rworldBossCombatReq.MonsterCfgId);
			list.AddRange(worldBossBattleEnemy);
			return new InBattleData
			{
				m_cardDatas = list,
				m_seed = rworldBossCombatReq.Seed,
				m_durationRound = 10
			};
		}

		private InBattleData m_inBattleData;

		private OutBattleData m_outBattleData;

		private BattleClientController_WorldBoss m_client;
	}
}
