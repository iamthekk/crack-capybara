using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Chapter;
using Proto.Common;

namespace HotFix
{
	public class ChapterSweepDataModule : IDataModule
	{
		public int SweepChapterId { get; private set; }

		public int SweepRate { get; private set; }

		public int RandomSeed { get; private set; }

		public MapField<uint, EventDetail> ServerEventMap { get; private set; }

		public MapField<uint, EventDetail> AddActivityMap { get; private set; }

		public int GetName()
		{
			return 152;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterSweep_Start, new HandlerEvent(this.OnEventSweepStart));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterSweep_End, new HandlerEvent(this.OnEventSweepEnd));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterSweep_Start, new HandlerEvent(this.OnEventSweepStart));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterSweep_End, new HandlerEvent(this.OnEventSweepEnd));
		}

		public void Reset()
		{
		}

		private void OnEventSweepStart(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgSweepStart eventArgSweepStart = eventArgs as EventArgSweepStart;
			if (eventArgSweepStart != null)
			{
				this.SweepChapterId = eventArgSweepStart.response.ChapterId;
				this.SweepRate = (int)eventArgSweepStart.response.Rate;
				this.RandomSeed = eventArgSweepStart.response.ChapterSeed;
				this.ServerEventMap = eventArgSweepStart.response.EventMap;
				this.AddActivityMap = eventArgSweepStart.response.ActiveMap;
			}
		}

		private void OnEventSweepEnd(object sender, int type, BaseEventArgs eventArgs)
		{
			this.SendEndMsg();
		}

		private void SendEndMsg()
		{
			Chapter_chapter table = GameApp.Table.GetManager().GetChapter_chapterModelInstance().GetElementById(this.SweepChapterId);
			if (table == null)
			{
				return;
			}
			string key = "ChapterSweep";
			List<BattleChapterDropData> dropDataListExclude = Singleton<GameEventController>.Instance.GetDropDataListExclude(ChapterDropSource.Battle);
			List<BattleChapterDropData> dropDataList = Singleton<GameEventController>.Instance.GetDropDataList(ChapterDropSource.Battle);
			List<RewardDto> list = BattleChapterDropData.ToServerData(dropDataListExclude);
			List<RewardDto> list2 = BattleChapterDropData.ToServerData(dropDataList);
			NetworkUtils.Chapter.DoEndChapterSweepRequest(this.SweepChapterId, this.SweepRate, table.journeyStage, list, list2, delegate(bool result, EndChapterSweepResponse response)
			{
				if (!result)
				{
					if (response != null && response.Code < -100)
					{
						long serverTimestamp = DxxTools.Time.ServerTimestamp;
						DxxTools.UI.RemoveServerTimeClockCallback(key);
						DxxTools.UI.AddServerTimeCallback(key, new Action(this.SendEndMsg), serverTimestamp + 10L, 0);
						HLog.LogError("游历结算网络连接异常...");
					}
					return;
				}
				DxxTools.UI.RemoveServerTimeClockCallback(key);
				if (Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Idle)
				{
					return;
				}
				ChapterSweepFinishViewModule.EventEndData eventEndData = new ChapterSweepFinishViewModule.EventEndData();
				eventEndData.chapterId = this.SweepChapterId;
				eventEndData.endStage = table.journeyStage;
				if (response != null)
				{
					eventEndData.rewardList = response.CommonData.Reward.ToItemDataList();
				}
				else
				{
					eventEndData.rewardList = new List<ItemData>();
				}
				if (!GameApp.View.IsOpened(ViewName.ChapterSweepFinishViewModule))
				{
					GameApp.View.OpenView(ViewName.ChapterSweepFinishViewModule, eventEndData, 1, null, null);
				}
				Singleton<GameEventController>.Instance.EnterEventMode(GameEventStateName.Idle);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterSweep_ResetSweep, null);
			});
		}

		public void UseSweepRecord()
		{
			SweepRecordData sweepRecord = Singleton<EventRecordController>.Instance.SweepRecord;
			this.SweepChapterId = sweepRecord.chapterId;
			this.SweepRate = sweepRecord.rate;
			this.RandomSeed = sweepRecord.seed;
		}

		public List<int> GetRateList()
		{
			if (GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.IsActivation(IAPMonthCardData.CardType.Month))
			{
				return GameConfig.Sweep_MonthCard_Rates;
			}
			return GameConfig.Sweep_Free_Rates;
		}
	}
}
