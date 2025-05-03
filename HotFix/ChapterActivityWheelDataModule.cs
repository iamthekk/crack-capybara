using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic;
using LocalModels.Bean;
using Proto.Chapter;
using Proto.Common;

namespace HotFix
{
	public class ChapterActivityWheelDataModule : IDataModule
	{
		public ChapterActiveWheelInfo WheelInfo { get; private set; }

		public List<CommonFundUIData> uiDataList { get; private set; } = new List<CommonFundUIData>();

		public int GetName()
		{
			return 168;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
		}

		public void SetLoginData(ChapterActiveWheelInfo info)
		{
			this.WheelInfo = info;
			this.CreateRewardData();
			this.RegisterChapterWheelRefresh();
		}

		public void UpdateInfo(ChapterActiveWheelInfo info)
		{
			bool flag = info != null && this.WheelInfo != null && info.RowId != this.WheelInfo.RowId;
			this.WheelInfo = info;
			if (flag)
			{
				this.CreateRewardData();
			}
			this.RegisterChapterWheelRefresh();
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterActivityWheel_RefreshUI, null);
		}

		private void CreateRewardData()
		{
			this.uiDataList.Clear();
			if (this.WheelInfo == null)
			{
				return;
			}
			List<ChapterActivity_ActvTurntableReward> allRewards = this.GetAllRewards();
			for (int i = 0; i < allRewards.Count; i++)
			{
				ChapterActivity_ActvTurntableReward chapterActivity_ActvTurntableReward = allRewards[i];
				CommonFundUIData commonFundUIData = new CommonFundUIData();
				commonFundUIData.ConfigId = chapterActivity_ActvTurntableReward.id;
				commonFundUIData.Score = chapterActivity_ActvTurntableReward.score;
				commonFundUIData.IsLoopReward = false;
				commonFundUIData.LoopRewardLimit = 0;
				commonFundUIData.FreeRewards = chapterActivity_ActvTurntableReward.freeReward.ToItemDataList();
				this.uiDataList.Add(commonFundUIData);
			}
			this.uiDataList.Sort(new Comparison<CommonFundUIData>(CommonFundUIData.Sort));
			for (int j = 0; j < this.uiDataList.Count; j++)
			{
				CommonFundUIData commonFundUIData2 = this.uiDataList[j];
				if (j > 0)
				{
					commonFundUIData2.PreviousScore = this.uiDataList[j - 1].Score;
				}
				else
				{
					commonFundUIData2.PreviousScore = 0;
				}
				if (j + 1 >= this.uiDataList.Count)
				{
					commonFundUIData2.NextScore = 0;
				}
				else
				{
					commonFundUIData2.NextScore = this.uiDataList[j + 1].Score;
				}
			}
			RedPointController.Instance.ReCalc("Main.ChapterWheel", true);
		}

		public void UpdateScore(long rowId, int score)
		{
			if (this.WheelInfo != null && this.WheelInfo.RowId == rowId)
			{
				int score2 = this.WheelInfo.Score;
				this.WheelInfo.Score = score;
				EventArgsChapterWheelGetScore eventArgsChapterWheelGetScore = new EventArgsChapterWheelGetScore();
				eventArgsChapterWheelGetScore.SetData(score2, score);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterActivityWheel_GetScore, eventArgsChapterWheelGetScore);
				RedPointController.Instance.ReCalc("Main.ChapterWheel", true);
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterActivityWheel_RefreshUI, null);
		}

		public void UpdateSpinInfo(ChapterWheelSpineResponse resp)
		{
			if (resp == null)
			{
				return;
			}
			this.WheelInfo.Score = resp.Score;
			this.WheelInfo.RewardScore = resp.RewardScore;
			this.WheelInfo.FreeNum = resp.FreeNum;
			this.WheelInfo.FreeRate = resp.FreeRate;
			this.WheelInfo.PlayTimes = resp.PlayTimes;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterActivityWheel_RefreshUI, null);
			RedPointController.Instance.ReCalc("Main.ChapterWheel", true);
		}

		public bool IsActivityOpen()
		{
			if (this.WheelInfo == null)
			{
				return false;
			}
			if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.ChapterActivityWheel, false))
			{
				return false;
			}
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			return serverTimestamp >= this.WheelInfo.StartTime && serverTimestamp < this.WheelInfo.EndTime;
		}

		public bool IsActivityOpen(long rowId)
		{
			return this.WheelInfo != null && this.WheelInfo.RowId == rowId && this.IsActivityOpen();
		}

		public List<ChapterActivity_ActvTurntableReward> GetAllRewards()
		{
			List<ChapterActivity_ActvTurntableReward> list = new List<ChapterActivity_ActvTurntableReward>();
			if (this.WheelInfo != null)
			{
				ChapterActivity_ActvTurntableBase chapterActivity_ActvTurntableBase = GameApp.Table.GetManager().GetChapterActivity_ActvTurntableBase(this.WheelInfo.ActiveId);
				if (chapterActivity_ActvTurntableBase != null)
				{
					IList<ChapterActivity_ActvTurntableReward> chapterActivity_ActvTurntableRewardElements = GameApp.Table.GetManager().GetChapterActivity_ActvTurntableRewardElements();
					for (int i = 0; i < chapterActivity_ActvTurntableRewardElements.Count; i++)
					{
						if (chapterActivity_ActvTurntableRewardElements[i].group == chapterActivity_ActvTurntableBase.group)
						{
							list.Add(chapterActivity_ActvTurntableRewardElements[i]);
						}
					}
				}
				else
				{
					HLog.LogError(string.Format("未找到幸运星活动id={0}", this.WheelInfo.ActiveId));
				}
			}
			return list;
		}

		public ChapterActivity_ActvTurntableReward GetCurrentReward()
		{
			if (this.WheelInfo != null)
			{
				List<ChapterActivity_ActvTurntableReward> allRewards = this.GetAllRewards();
				for (int i = 0; i < allRewards.Count; i++)
				{
					if (allRewards[i].score > this.WheelInfo.RewardScore)
					{
						return allRewards[i];
					}
				}
			}
			return null;
		}

		[return: TupleElementNames(new string[] { "current", "next" })]
		public ValueTuple<int, int> GetRewardProgress()
		{
			int num = 0;
			int num2 = 0;
			if (this.WheelInfo != null)
			{
				CommonFundUIData stageData = CommonFundTools.GetStageData(this.uiDataList, this.WheelInfo.RewardScore);
				if (stageData != null)
				{
					num = CommonFundTools.GetStageScore(this.uiDataList, this.WheelInfo.RewardScore, 0);
					num = ((num < 0) ? 0 : num);
					num2 = stageData.Score - stageData.PreviousScore;
				}
			}
			return new ValueTuple<int, int>(num, num2);
		}

		public bool IsAllFinish()
		{
			if (this.WheelInfo != null)
			{
				List<ChapterActivity_ActvTurntableReward> allRewards = this.GetAllRewards();
				if (allRewards.Count > 0)
				{
					int rewardScore = this.WheelInfo.RewardScore;
					List<ChapterActivity_ActvTurntableReward> list = allRewards;
					if (rewardScore >= list[list.Count - 1].score)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsRedPoint()
		{
			if (!this.IsActivityOpen())
			{
				return false;
			}
			if (this.IsAllFinish())
			{
				return false;
			}
			if (this.WheelInfo.FreeNum > 0)
			{
				return true;
			}
			ChapterActivity_ActvTurntableBase chapterActivity_ActvTurntableBase = GameApp.Table.GetManager().GetChapterActivity_ActvTurntableBase(this.WheelInfo.ActiveId);
			return chapterActivity_ActvTurntableBase != null && this.WheelInfo.Score >= chapterActivity_ActvTurntableBase.cost;
		}

		private void RegisterChapterWheelRefresh()
		{
			DxxTools.UI.RemoveServerTimeClockCallback("ChapterWheelRefreshKey");
			if (this.IsActivityOpen())
			{
				long serverTimestamp = DxxTools.Time.ServerTimestamp;
				if (this.WheelInfo.EndTime > 0L && this.WheelInfo.EndTime > serverTimestamp)
				{
					DxxTools.UI.AddServerTimeCallback("ChapterWheelRefreshKey", new Action(this.DoChapterWheelRefreshRequest), this.WheelInfo.EndTime, 0);
				}
			}
		}

		private void DoChapterWheelRefreshRequest()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterActivityWheel_ActChange, null);
			RedPointController.Instance.ReCalc("Main.ChapterWheel", true);
			float num = Utility.Math.Random(2f, 10f);
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.AppendInterval(sequence, num);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				NetworkUtils.Chapter.DoChapterWheelInfoRequest(null);
			});
		}

		public readonly int ScoreAtlas = 120;

		public readonly string ScoreIcon = "icon_zhuanpan";

		public readonly string ScoreNameId = "activity_wheel_score_name";

		private const string ChapterWheelRefreshKey = "ChapterWheelRefreshKey";
	}
}
