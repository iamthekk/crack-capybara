using System;
using System.Collections.Generic;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public abstract class ChapterActivityData
	{
		public ulong RowId { get; private set; }

		public ChapterActivityKind Kind { get; private set; }

		public uint ActivityId { get; private set; }

		public long StartTime { get; private set; }

		public long EndTime { get; private set; }

		public uint TotalScore { get; private set; }

		public int CurrentScore { get; private set; }

		public int ScoreAtlasId { get; protected set; }

		public string ScoreIcon { get; protected set; }

		public string ScoreNameId { get; protected set; }

		public string ActivityTitleId { get; protected set; }

		public ChapterActivity_ChapterObj CurrentProgress { get; private set; }

		public void Init(ActiveInfo info)
		{
			this.RowId = info.RowId;
			this.Kind = (ChapterActivityKind)info.ActiveType;
			this.ActivityId = info.ActiveId;
			this.StartTime = (long)info.StartTime;
			this.EndTime = (long)info.EndTime;
			this.TotalScore = info.Socre;
			this.SetTable((int)this.ActivityId);
			this.CalcCurrentProgress();
		}

		protected abstract void SetTable(int actId);

		public void SetScore(uint score)
		{
			this.TotalScore = score;
			this.CalcCurrentProgress();
		}

		public bool IsStart()
		{
			long num = ChapterActivityDataModule.ServerTime();
			return this.StartTime <= num;
		}

		public bool IsEnd()
		{
			return ChapterActivityDataModule.ServerTime() >= this.EndTime;
		}

		public bool IsFunctionOpen()
		{
			return this.Kind == ChapterActivityKind.Rank && Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_Ranking, false);
		}

		public bool IsInProgress()
		{
			return this.IsFunctionOpen() && this.IsStart() && !this.IsEnd();
		}

		private void CalcCurrentProgress()
		{
			this.CurrentScore = (int)this.TotalScore;
			List<ChapterActivity_ChapterObj> allProgress = this.GetAllProgress();
			for (int i = 0; i < allProgress.Count; i++)
			{
				ChapterActivity_ChapterObj chapterActivity_ChapterObj = allProgress[i];
				if (this.CurrentScore < chapterActivity_ChapterObj.num)
				{
					this.CurrentProgress = chapterActivity_ChapterObj;
					return;
				}
				this.CurrentScore -= chapterActivity_ChapterObj.num;
			}
			if (allProgress.Count > 0)
			{
				this.CurrentProgress = allProgress[allProgress.Count - 1];
			}
		}

		public ChapterActivity_ChapterObj GetNextProgress()
		{
			List<ChapterActivity_ChapterObj> allProgress = this.GetAllProgress();
			int num = allProgress.IndexOf(this.CurrentProgress);
			if (num + 1 < allProgress.Count)
			{
				return allProgress[num + 1];
			}
			return allProgress[allProgress.Count - 1];
		}

		public List<ItemData> GetProgressRewards()
		{
			List<ItemData> list = new List<ItemData>();
			for (int i = 0; i < this.CurrentProgress.reward.Length; i++)
			{
				List<int> listInt = this.CurrentProgress.reward[i].GetListInt(',');
				if (listInt.Count >= 2)
				{
					ItemData itemData = new ItemData(listInt[0], (long)listInt[1]);
					list.Add(itemData);
				}
			}
			return list;
		}

		public List<ItemData> GetProgressRewards(int index)
		{
			List<ItemData> list = new List<ItemData>();
			List<ChapterActivity_ChapterObj> allProgress = this.GetAllProgress();
			if (index < allProgress.Count)
			{
				for (int i = 0; i < allProgress[index].reward.Length; i++)
				{
					List<int> listInt = allProgress[index].reward[i].GetListInt(',');
					if (listInt.Count >= 2)
					{
						ItemData itemData = new ItemData(listInt[0], (long)listInt[1]);
						list.Add(itemData);
					}
				}
			}
			return list;
		}

		public int GetCurrentProgressIndex()
		{
			return this.GetAllProgress().IndexOf(this.CurrentProgress);
		}

		public ChapterActivity_ChapterObj GetChapterObj(int index)
		{
			List<ChapterActivity_ChapterObj> allProgress = this.GetAllProgress();
			if (index < allProgress.Count)
			{
				return allProgress[index];
			}
			return null;
		}

		public abstract List<ChapterActivity_ChapterObj> GetAllProgress();

		public abstract List<ItemData> GetTotalShowRewards();

		public abstract bool IsHaveEndReward();

		public int AllProgressCount()
		{
			return this.GetAllProgress().Count;
		}

		public int CollectedCount()
		{
			List<ChapterActivity_ChapterObj> allProgress = this.GetAllProgress();
			if (this.IsFinish())
			{
				return allProgress.Count;
			}
			int num = allProgress.IndexOf(this.CurrentProgress);
			if (num >= 0)
			{
				return num;
			}
			return 0;
		}

		public bool IsFinish()
		{
			int num = 0;
			List<ChapterActivity_ChapterObj> allProgress = this.GetAllProgress();
			for (int i = 0; i < allProgress.Count; i++)
			{
				num += allProgress[i].num;
			}
			return (ulong)this.TotalScore >= (ulong)((long)num);
		}
	}
}
