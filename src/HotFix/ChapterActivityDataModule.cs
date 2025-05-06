using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class ChapterActivityDataModule : IDataModule
	{
		public ChapterActiveRankInfo RankReward { get; private set; }

		public int GetName()
		{
			return 151;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterActivity_RefreshScore, new HandlerEvent(this.OnEventRefreshScore));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterActivity_RankReward, new HandlerEvent(this.OnEventRefreshReward));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterActivity_RefreshScore, new HandlerEvent(this.OnEventRefreshScore));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterActivity_RankReward, new HandlerEvent(this.OnEventRefreshReward));
		}

		public void Reset()
		{
		}

		public void SetLoginData(RepeatedField<ActiveInfo> activeInfo, ChapterActiveRankInfo reward)
		{
			if (activeInfo == null)
			{
				return;
			}
			this.activityDic.Clear();
			this.rewardActivityList.Clear();
			this.ServerActiveInfoMap = activeInfo;
			this.RankReward = reward;
			for (int i = 0; i < this.ServerActiveInfoMap.Count; i++)
			{
				ChapterActivityData chapterActivityData = null;
				if (this.ServerActiveInfoMap[i].ActiveType == 2U)
				{
					chapterActivityData = new ChapterActivityRankData();
				}
				if (chapterActivityData != null)
				{
					chapterActivityData.Init(this.ServerActiveInfoMap[i]);
					this.activityDic.Add(chapterActivityData.RowId, chapterActivityData);
				}
			}
			if (this.RankReward != null)
			{
				this.AddRewardActivity(this.RankReward.RowId, (ChapterActivityKind)this.RankReward.ActiveType, (int)this.RankReward.ActiveId);
			}
		}

		private void OnEventRefreshScore(object sender, int type, BaseEventArgs eventArgs)
		{
			if (eventArgs != null)
			{
				EventArgsChapterActivityRefreshScore eventArgsChapterActivityRefreshScore = eventArgs as EventArgsChapterActivityRefreshScore;
				if (eventArgsChapterActivityRefreshScore != null && eventArgsChapterActivityRefreshScore.scoreMap.Count > 0)
				{
					int num = 0;
					foreach (ulong num2 in eventArgsChapterActivityRefreshScore.scoreMap.Keys)
					{
						ChapterActivityData chapterActivityData;
						if (this.activityDic.TryGetValue(num2, out chapterActivityData) && chapterActivityData.Kind == ChapterActivityKind.Rank)
						{
							uint num3 = eventArgsChapterActivityRefreshScore.scoreMap[num2];
							int currentScore = chapterActivityData.CurrentScore;
							int totalScore = (int)chapterActivityData.TotalScore;
							int currentProgressIndex = chapterActivityData.GetCurrentProgressIndex();
							chapterActivityData.SetScore(num3);
							int currentScore2 = chapterActivityData.CurrentScore;
							int currentProgressIndex2 = chapterActivityData.GetCurrentProgressIndex();
							Dictionary<int, ChapterActivity_ChapterObj> dictionary = new Dictionary<int, ChapterActivity_ChapterObj>();
							for (int i = currentProgressIndex; i <= currentProgressIndex2; i++)
							{
								ChapterActivity_ChapterObj chapterObj = chapterActivityData.GetChapterObj(i);
								if (chapterObj != null)
								{
									dictionary.Add(i, chapterObj);
								}
							}
							num++;
							EventArgsChapterActivityGetScoreAni eventArgsChapterActivityGetScoreAni = new EventArgsChapterActivityGetScoreAni();
							eventArgsChapterActivityGetScoreAni.SetData(chapterActivityData.RowId, currentScore, currentScore2, totalScore, (int)num3, currentProgressIndex, currentProgressIndex2, eventArgsChapterActivityRefreshScore.rewards, dictionary);
							GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterActivity_GetScoreAni, eventArgsChapterActivityGetScoreAni);
						}
					}
					if (num == 0)
					{
						this.ResumeHangUp();
						return;
					}
					return;
				}
			}
			if (this.aniCount <= 0)
			{
				this.ResumeHangUp();
			}
		}

		private void ResumeHangUp()
		{
			if (Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Sweep)
			{
				Singleton<GameEventController>.Instance.ResumeHangUp();
			}
		}

		public void AddAnimation()
		{
			this.aniCount++;
		}

		public void AnimationFinish()
		{
			this.aniCount--;
			if (this.aniCount <= 0)
			{
				this.aniCount = 0;
				if (Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Sweep)
				{
					Singleton<GameEventController>.Instance.ResumeHangUp();
				}
			}
		}

		private void OnEventRefreshReward(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsChapterActivityRankReward eventArgsChapterActivityRankReward = eventArgs as EventArgsChapterActivityRankReward;
			if (eventArgsChapterActivityRankReward != null)
			{
				if (this.RankReward != null)
				{
					for (int i = 0; i < this.rewardActivityList.Count; i++)
					{
						if (this.rewardActivityList[i].RowId == this.RankReward.RowId)
						{
							this.rewardActivityList.RemoveAt(i);
							break;
						}
					}
				}
				this.RankReward = eventArgsChapterActivityRankReward.RankReward;
				if (this.RankReward != null)
				{
					this.AddRewardActivity(this.RankReward.RowId, (ChapterActivityKind)this.RankReward.ActiveType, (int)this.RankReward.ActiveId);
				}
			}
		}

		private void AddRewardActivity(ulong rowId, ChapterActivityKind kind, int actId)
		{
			for (int i = 0; i < this.rewardActivityList.Count; i++)
			{
				if (this.rewardActivityList[i].RowId == rowId)
				{
					this.rewardActivityList.RemoveAt(i);
					break;
				}
			}
			ChapterActivityRewardData chapterActivityRewardData = new ChapterActivityRewardData(rowId, kind, actId);
			this.rewardActivityList.Add(chapterActivityRewardData);
		}

		public ChapterActivityData GetActivityData(ulong rowId)
		{
			ChapterActivityData chapterActivityData;
			if (this.activityDic.TryGetValue(rowId, out chapterActivityData))
			{
				return chapterActivityData;
			}
			return null;
		}

		public ChapterActivityData GetActiveActivityData(ChapterActivityKind kind)
		{
			foreach (ChapterActivityData chapterActivityData in this.activityDic.Values)
			{
				if (chapterActivityData.Kind == kind && chapterActivityData.IsInProgress())
				{
					return chapterActivityData;
				}
			}
			return null;
		}

		public static long ServerTime()
		{
			return DxxTools.Time.ServerTimestamp;
		}

		public ChapterActivityRewardData GetRewardActivity(ChapterActivityKind kind)
		{
			for (int i = 0; i < this.rewardActivityList.Count; i++)
			{
				if (this.rewardActivityList[i].Kind == kind)
				{
					return this.rewardActivityList[i];
				}
			}
			return null;
		}

		public void CollectReward(ulong rowId)
		{
			ChapterActivityData chapterActivityData;
			if (this.activityDic.TryGetValue(rowId, out chapterActivityData))
			{
				ChapterActivityRankData chapterActivityRankData = chapterActivityData as ChapterActivityRankData;
				if (chapterActivityRankData != null)
				{
					chapterActivityRankData.SetCollectedReward();
				}
			}
			for (int i = 0; i < this.rewardActivityList.Count; i++)
			{
				if (this.rewardActivityList[i].RowId == rowId)
				{
					this.rewardActivityList.RemoveAt(i);
					return;
				}
			}
		}

		public ChapterActivityRewardData CheckRewardActivity()
		{
			foreach (ChapterActivityData chapterActivityData in this.activityDic.Values)
			{
				if (chapterActivityData.IsHaveEndReward())
				{
					this.AddRewardActivity(chapterActivityData.RowId, chapterActivityData.Kind, (int)chapterActivityData.ActivityId);
				}
			}
			if (this.rewardActivityList.Count > 0)
			{
				return this.rewardActivityList[0];
			}
			return null;
		}

		public List<ChapterActivity_ChapterObj> OnGetAllRankRewardList(int groupId)
		{
			List<ChapterActivity_ChapterObj> list = new List<ChapterActivity_ChapterObj>();
			IList<ChapterActivity_ChapterObj> chapterActivity_ChapterObjElements = GameApp.Table.GetManager().GetChapterActivity_ChapterObjElements();
			for (int i = 0; i < chapterActivity_ChapterObjElements.Count; i++)
			{
				if (chapterActivity_ChapterObjElements[i].group == groupId)
				{
					list.Add(chapterActivity_ChapterObjElements[i]);
				}
			}
			return list;
		}

		public ChapterActivity_ChapterObj GetCurrentReward(ChapterActivityData activityData)
		{
			if (activityData != null)
			{
				ChapterActivity_RankActivity chapterActivity_RankActivity = GameApp.Table.GetManager().GetChapterActivity_RankActivity((int)activityData.ActivityId);
				if (chapterActivity_RankActivity != null)
				{
					List<ChapterActivity_ChapterObj> list = this.OnGetAllRankRewardList(chapterActivity_RankActivity.group);
					for (int i = 0; i < list.Count; i++)
					{
						if ((long)list[i].score > (long)((ulong)activityData.TotalScore))
						{
							return list[i];
						}
					}
				}
			}
			return null;
		}

		public List<ItemData> GetRankReward(int rank, int rankId)
		{
			List<ItemData> list = new List<ItemData>();
			if (rank <= 0)
			{
				return list;
			}
			IList<ChapterActivity_RankReward> allElements = GameApp.Table.GetManager().GetChapterActivity_RankRewardModelInstance().GetAllElements();
			string[] array = null;
			for (int i = 0; i < allElements.Count; i++)
			{
				if (rankId == allElements[i].randID && rank <= allElements[i].rank)
				{
					array = allElements[i].reward;
					break;
				}
			}
			if (array != null)
			{
				for (int j = 0; j < array.Length; j++)
				{
					List<int> listInt = array[j].GetListInt(',');
					if (listInt.Count >= 2)
					{
						ItemData itemData = new ItemData(listInt[0], (long)listInt[1]);
						list.Add(itemData);
					}
				}
			}
			return list;
		}

		private RepeatedField<ActiveInfo> ServerActiveInfoMap;

		private Dictionary<ulong, ChapterActivityData> activityDic = new Dictionary<ulong, ChapterActivityData>();

		private List<ChapterActivityRewardData> rewardActivityList = new List<ChapterActivityRewardData>();

		private int aniCount;
	}
}
