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
	public class BattleController_RogueDungeon : BattleControllerBase
	{
		public BattleClientController_RogueDungeon BattleClient
		{
			get
			{
				return this.m_client;
			}
		}

		protected override async Task OnInit()
		{
			this.m_client = new BattleClientController_RogueDungeon();
			await this.m_client.Init();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_ChapterBattle_Start, new HandlerEvent(this.OnEventBattleStart));
		}

		protected override async Task OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_ChapterBattle_Start, new HandlerEvent(this.OnEventBattleStart));
			BattleClientController_RogueDungeon client = this.m_client;
			await ((client != null) ? client.DeInit() : null);
			this.m_client = null;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleClientController_RogueDungeon client = this.m_client;
			if (client == null)
			{
				return;
			}
			client.Update(deltaTime, unscaledDeltaTime);
		}

		public void OnEventBattleStart(object sender, int type, BaseEventArgs eventArgs)
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
			Singleton<GameEventController>.Instance.SetReviveCount(this.m_outBattleData.m_resultData.m_revivedCount);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleStart, null);
			EventArgsBool eventArgsBool = new EventArgsBool();
			eventArgsBool.SetData(true);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIBattleRogueDungeon_ShowHideRound, eventArgsBool);
		}

		public void PlayServer(out InBattleData inBattleData, out OutBattleData outBattleData)
		{
			inBattleData = this.GetInBattleDataByServer();
			for (int i = 0; i < inBattleData.m_cardDatas.Count; i++)
			{
			}
			BattleServer battleServer = new BattleServer();
			outBattleData = battleServer.DoBattle(inBattleData, GameApp.Table.GetManager());
		}

		private InBattleData GetInBattleDataByServer()
		{
			RogueDungeonDataModule dataModule = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule);
			StringBuilder stringBuilder = new StringBuilder();
			RHellTowerCombatReq combatReq = dataModule.CombatReq;
			if (combatReq == null)
			{
				HLog.LogError(HLog.ToColor("【地牢战斗】服务器数据异常.", 3));
				return new InBattleData();
			}
			string text = "SLogID=" + combatReq.BattleServerLogId + "\n" + combatReq.BattleServerLogData;
			Singleton<BattleLog>.Instance.ClearSLog();
			Singleton<BattleLog>.Instance.AddSLog(text, combatReq.BattleServerLogId);
			string text2 = string.Format("[BattleCheck]:request stageId={0} passStage={1} seed={2} CurHp={3} ReviveCount={4} ", new object[] { combatReq.StageId, combatReq.PassStage, combatReq.Seed, combatReq.CurHp, combatReq.ReviveCount });
			string text3 = "UserInfo=" + JsonManager.SerializeObject(combatReq.UserInfo) + " ";
			stringBuilder.Append(text2);
			stringBuilder.Append(text3);
			LocalModelManager manager = GameApp.Table.GetManager();
			combatReq.CurHp = Singleton<GameEventController>.Instance.PlayerData.CurrentHp.AsLong();
			List<CardData> list = new List<CardData>();
			CardData cardData;
			string text4;
			combatReq.GetRogueDungeonMainPlayer(manager, out cardData, out text4);
			list.Add(cardData);
			stringBuilder.Append(text4);
			List<CardData> list2;
			combatReq.UserInfo.Pets.ToList<PetDto>().PetFilter(manager, MemberCamp.Friendly, out list2);
			list.AddRange(list2);
			for (int i = 0; i < list2.Count; i++)
			{
				stringBuilder.Append("[PetData]:" + list2[i].Log() + " ");
			}
			List<CardData> list3;
			List<List<CardData>> list4;
			RogueDungeonController.GetRogueDungeonBattleEnemy(manager, combatReq.StageId, combatReq.PassStage, combatReq.MonsterSkillList, combatReq.MonsterCfgId.ToList<int>(), out list3, out list4);
			list.AddRange(list3);
			for (int j = 0; j < list3.Count; j++)
			{
				CardData cardData2 = list3[j];
				stringBuilder.Append("[EnemyData]=" + cardData2.Log() + " ");
			}
			string text5 = string.Format("CLogID={0}\n{1}", combatReq.BattleServerLogId, stringBuilder);
			Singleton<BattleLog>.Instance.ClearCLog();
			Singleton<BattleLog>.Instance.AddCLog(text5);
			return new InBattleData
			{
				m_cardDatas = list,
				m_seed = combatReq.Seed,
				m_durationRound = 15,
				m_revivedCount = Singleton<GameEventController>.Instance.PlayerData.ReviveCount,
				m_waveMax = ((list4 != null && list4.Count > 0) ? (list4.Count + 1) : 1),
				m_otherWareDatas = list4
			};
		}

		private InBattleData m_inBattleData;

		private OutBattleData m_outBattleData;

		private BattleClientController_RogueDungeon m_client;
	}
}
