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
	public class BattleController_Chapter : BattleControllerBase
	{
		public BattleClientController_Chapter BattleClient
		{
			get
			{
				return this.m_client;
			}
		}

		protected override async Task OnInit()
		{
			this.m_client = new BattleClientController_Chapter();
			await this.m_client.Init();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_ChapterBattle_Start, new HandlerEvent(this.OnEventBattleStart));
		}

		protected override async Task OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_ChapterBattle_Start, new HandlerEvent(this.OnEventBattleStart));
			BattleClientController_Chapter client = this.m_client;
			await ((client != null) ? client.DeInit() : null);
			this.m_client = null;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			BattleClientController_Chapter client = this.m_client;
			if (client == null)
			{
				return;
			}
			client.Update(deltaTime, unscaledDeltaTime);
		}

		public void OnEventBattleStart(object sender, int type, BaseEventArgs eventArgs)
		{
			List<List<CardData>> list = new List<List<CardData>>();
			EventArgChapterBattleStart eventArgChapterBattleStart = eventArgs as EventArgChapterBattleStart;
			if (eventArgChapterBattleStart != null)
			{
				list = eventArgChapterBattleStart.otherWaves;
			}
			this.PlayServer(list);
		}

		public void PlayServer(List<List<CardData>> otherWaves)
		{
			if (this.m_client == null)
			{
				return;
			}
			List<CardData> allMembersCardData = this.m_client.GetAllMembersCardData();
			this.PlayServer(out this.m_inBattleData, out this.m_outBattleData, allMembersCardData, otherWaves);
			this.m_client.SetData(this.m_inBattleData, this.m_outBattleData);
			Singleton<GameEventController>.Instance.SetReviveCount(this.m_outBattleData.m_resultData.m_revivedCount);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleStart, null);
		}

		public void PlayServer(out InBattleData inBattleData, out OutBattleData outBattleData, List<CardData> cards, List<List<CardData>> otherWaves)
		{
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			inBattleData = ((!dataModule.isServerBattle) ? this.GetInBattleDataByCard(cards, otherWaves) : this.GetInBattleDataByServer(dataModule));
			for (int i = 0; i < inBattleData.m_cardDatas.Count; i++)
			{
			}
			BattleServer battleServer = new BattleServer();
			outBattleData = battleServer.DoBattle(inBattleData, GameApp.Table.GetManager());
		}

		private InBattleData GetInBattleDataByCard(List<CardData> cards, List<List<CardData>> otherWaves)
		{
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			InBattleData inBattleData = new InBattleData();
			for (int i = 0; i < cards.Count; i++)
			{
				inBattleData.m_cardDatas.Add(cards[i]);
			}
			inBattleData.m_seed = dataModule.ChapterBattleSeed;
			inBattleData.m_durationRound = 30;
			inBattleData.m_revivedCount = Singleton<GameEventController>.Instance.GetHasReviveCount();
			inBattleData.m_waveMax = ((otherWaves != null && otherWaves.Count > 0) ? (otherWaves.Count + 1) : 1);
			inBattleData.m_otherWareDatas = otherWaves;
			return inBattleData;
		}

		private InBattleData GetInBattleDataByServer(ChapterDataModule chapterDataModule)
		{
			RChapterCombatReq chapterCombatReq = chapterDataModule.ChapterCombatReq;
			StringBuilder stringBuilder = new StringBuilder();
			if (chapterCombatReq == null)
			{
				HLog.LogError(HLog.ToColor("【主线战斗】服务器数据异常.", 3));
			}
			else
			{
				string text = "SLogID=" + chapterCombatReq.BattleServerLogId + "\n" + chapterCombatReq.BattleServerLogData;
				Singleton<BattleLog>.Instance.ClearSLog();
				Singleton<BattleLog>.Instance.AddSLog(text, chapterCombatReq.BattleServerLogId);
				string text2 = string.Format("[BattleCheck]:request chapterId={0} WaveIndex={1} seed={2} CurHp={3} ReviveCount={4} ", new object[] { chapterCombatReq.ChapterId, chapterCombatReq.WaveIndex, chapterCombatReq.Seed, chapterCombatReq.CurHp, chapterCombatReq.ReviveCount });
				string text3 = "UserInfo=" + JsonManager.SerializeObject(chapterCombatReq.UserInfo) + " ";
				stringBuilder.Append(text2);
				stringBuilder.Append(text3);
			}
			LocalModelManager manager = GameApp.Table.GetManager();
			List<CardData> list = new List<CardData>();
			CardData cardData;
			string text4;
			chapterCombatReq.GetChapterMainPlayer(manager, out cardData, out text4);
			list.Add(cardData);
			stringBuilder.Append(text4);
			List<CardData> list2;
			chapterCombatReq.UserInfo.Pets.ToList<PetDto>().PetFilter(manager, MemberCamp.Friendly, out list2);
			list.AddRange(list2);
			for (int i = 0; i < list2.Count; i++)
			{
				stringBuilder.Append("[PetData]:" + list2[i].Log() + " ");
			}
			List<CardData> list3;
			List<List<CardData>> list4;
			ChapterController.GetChapterBattleEnemy(manager, chapterCombatReq.ChapterId, chapterCombatReq.WaveIndex, chapterCombatReq.MonsterCfgId.ToList<int>(), chapterCombatReq.BattleTimes, out list3, out list4);
			list.AddRange(list3);
			for (int j = 0; j < list3.Count; j++)
			{
				CardData cardData2 = list3[j];
				stringBuilder.Append("[EnemyData]=" + cardData2.Log() + " ");
			}
			string text5 = string.Format("CLogID={0}\n{1}", chapterCombatReq.BattleServerLogId, stringBuilder);
			Singleton<BattleLog>.Instance.ClearCLog();
			Singleton<BattleLog>.Instance.AddCLog(text5);
			return new InBattleData
			{
				m_cardDatas = list,
				m_seed = chapterCombatReq.Seed,
				m_durationRound = 30,
				m_revivedCount = chapterCombatReq.ReviveCount,
				m_waveMax = ((list4 != null && list4.Count > 0) ? (list4.Count + 1) : 1),
				m_otherWareDatas = list4
			};
		}

		private InBattleData m_inBattleData;

		private OutBattleData m_outBattleData;

		private BattleClientController_Chapter m_client;
	}
}
