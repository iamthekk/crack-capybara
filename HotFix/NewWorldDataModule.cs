using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.LeaderBoard;
using Proto.NewWorld;

namespace HotFix
{
	public class NewWorldDataModule : IDataModule
	{
		public RepeatedField<int> LikeInfo { get; private set; }

		public long LikeRefreshTime { get; private set; }

		public int EnterNewWorldPlayerCount { get; private set; }

		public RepeatedField<int> PlayerLikeCount { get; private set; }

		public long NewWorldOpenTime { get; private set; }

		public int EnterNewWorldSign { get; private set; }

		public bool IsEnterNewWorld
		{
			get
			{
				return this.EnterNewWorldSign > 0;
			}
		}

		public RankUserDto SelfRank { get; private set; }

		public RepeatedField<RankUserDto> Top3User { get; private set; }

		public RankUserDto FirstUser { get; private set; }

		public RepeatedField<int> FinishTasks { get; private set; }

		public int GetName()
		{
			return 166;
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

		public void SetLoginData(int sign)
		{
			this.EnterNewWorldSign = sign;
			this.taskDic.Clear();
			List<NewWorld_newWorldTask> list = GameApp.Table.GetManager().GetNewWorld_newWorldTaskElements().ToList<NewWorld_newWorldTask>();
			list.Sort((NewWorld_newWorldTask a, NewWorld_newWorldTask b) => a.id.CompareTo(b.id));
			for (int i = 0; i < list.Count; i++)
			{
				NewWorld_newWorldTask newWorld_newWorldTask = list[i];
				List<NewWorld_newWorldTask> list2;
				if (this.taskDic.TryGetValue(newWorld_newWorldTask.group, out list2))
				{
					list2.Add(newWorld_newWorldTask);
				}
				else
				{
					this.taskDic.Add(newWorld_newWorldTask.group, new List<NewWorld_newWorldTask> { newWorld_newWorldTask });
				}
			}
		}

		public void UpdateInfo(NewWorldInfoResponse resp)
		{
			if (resp == null)
			{
				return;
			}
			this.LikeInfo = resp.LikeInfo;
			this.LikeRefreshTime = resp.LikeRefreshtime;
			this.EnterNewWorldPlayerCount = resp.EnterNewWrldCount;
			this.PlayerLikeCount = resp.LikeCount;
			this.NewWorldOpenTime = resp.NewWorldOpenTime;
			this.EnterNewWorldSign = resp.IsEnterNewWorld;
			this.FirstUser = resp.Top1;
			this.FinishTasks = resp.RewardTasks;
			if (this.FirstUser != null)
			{
				EventArgsRefreshTopPlayer eventArgsRefreshTopPlayer = new EventArgsRefreshTopPlayer();
				eventArgsRefreshTopPlayer.SetData(this.FirstUser);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_NewWorld_RefreshTopPlayer, eventArgsRefreshTopPlayer);
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_MainCity_Refresh, null);
			RedPointController.Instance.ReCalc("Main.NewWorld", true);
		}

		public void UpdateLikeInfo(LikeResponse resp)
		{
			if (resp == null)
			{
				return;
			}
			this.LikeInfo = resp.LikeInfo;
			this.LikeRefreshTime = resp.LikeRefreshtime;
			this.PlayerLikeCount = resp.LikeCount;
		}

		public void UpdateRankData(LeaderBoardResponse resp)
		{
			if (resp.Top3 != null)
			{
				this.Top3User = resp.Top3;
				if (this.Top3User.Count > 0)
				{
					this.FirstUser = this.Top3User[0];
				}
				if (this.FirstUser != null)
				{
					EventArgsRefreshTopPlayer eventArgsRefreshTopPlayer = new EventArgsRefreshTopPlayer();
					eventArgsRefreshTopPlayer.SetData(this.FirstUser);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_NewWorld_RefreshTopPlayer, eventArgsRefreshTopPlayer);
				}
			}
			if (resp.Self != null)
			{
				this.SelfRank = resp.Self;
			}
		}

		public void UpdateTaskData(NewWorldTaskRewardResponse resp)
		{
			if (resp == null || resp.RewardTasks == null)
			{
				return;
			}
			this.FinishTasks = resp.RewardTasks;
			RedPointController.Instance.ReCalc("Main.NewWorld", true);
		}

		public void EnterNewWorld()
		{
			int enterNewWorldSign = this.EnterNewWorldSign;
			this.EnterNewWorldSign = enterNewWorldSign + 1;
		}

		public int GetLikeCount(int rank)
		{
			int num = rank - 1;
			if (this.PlayerLikeCount != null && num < this.PlayerLikeCount.Count)
			{
				return this.PlayerLikeCount[num];
			}
			return 0;
		}

		public int GetLikeState(int rank)
		{
			if (!this.LikeInfo.Contains(rank))
			{
				return 0;
			}
			return 1;
		}

		public bool IsShowNewWorldScene()
		{
			return !this.IsEnterNewWorld && Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.NewWorld, false) && GameApp.Data.GetDataModule(DataName.ChapterDataModule).CurrentChapter.id > 80;
		}

		public bool IsCheckRank()
		{
			if (this.SelfRank == null || this.SelfRank.Rank <= 0)
			{
				return true;
			}
			if (this.Top3User == null || this.Top3User.Count != 3)
			{
				return true;
			}
			for (int i = 0; i < this.Top3User.Count; i++)
			{
				RankUserDto rankUserDto = this.Top3User[i];
				if (rankUserDto == null || rankUserDto.Rank <= 0)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsGoNewWorldEnabled()
		{
			return !this.IsEnterNewWorld && Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.NewWorld, false) && this.NewWorldOpenTime <= DxxTools.Time.ServerTimestamp && this.IsAllTaskFinish();
		}

		public NewWorld_newWorldTask GetCurrentTask(int group)
		{
			List<NewWorld_newWorldTask> list;
			if (this.taskDic.TryGetValue(group, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					NewWorld_newWorldTask newWorld_newWorldTask = list[i];
					if (this.FinishTasks != null && !this.FinishTasks.Contains(newWorld_newWorldTask.id))
					{
						return newWorld_newWorldTask;
					}
				}
				if (list.Count > 0)
				{
					List<NewWorld_newWorldTask> list2 = list;
					return list2[list2.Count - 1];
				}
			}
			return null;
		}

		public int GetAllTaskCount()
		{
			int num = 0;
			foreach (List<NewWorld_newWorldTask> list in this.taskDic.Values)
			{
				num += list.Count;
			}
			return num;
		}

		public int GetFinishTaskCount()
		{
			RepeatedField<int> finishTasks = this.FinishTasks;
			if (finishTasks == null)
			{
				return 0;
			}
			return finishTasks.Count;
		}

		public bool IsGetTaskReward(int id)
		{
			return this.FinishTasks != null && this.FinishTasks.Contains(id);
		}

		public bool IsAllTaskFinish()
		{
			int allTaskCount = this.GetAllTaskCount();
			return this.GetFinishTaskCount() >= allTaskCount;
		}

		public bool IsRedPoint()
		{
			if (this.EnterNewWorldSign > 0)
			{
				return false;
			}
			if (this.IsAllTaskFinish())
			{
				return false;
			}
			NewWorld_newWorldTask currentTask = this.GetCurrentTask(1);
			if (currentTask != null)
			{
				bool flag = this.IsGetTaskReward(currentTask.id);
				int num = GameApp.Data.GetDataModule(DataName.ChapterDataModule).CurrentChapter.id - 1;
				int num2 = currentTask.num;
				if (num >= num2 && !flag)
				{
					return true;
				}
			}
			NewWorld_newWorldTask currentTask2 = this.GetCurrentTask(2);
			if (currentTask2 != null)
			{
				bool flag2 = this.IsGetTaskReward(currentTask2.id);
				int talentExp = GameApp.Data.GetDataModule(DataName.TalentDataModule).TalentExp;
				int num3 = currentTask2.num;
				if (talentExp >= num3 && !flag2)
				{
					return true;
				}
			}
			NewWorld_newWorldTask currentTask3 = this.GetCurrentTask(3);
			if (currentTask3 != null)
			{
				bool flag3 = this.IsGetTaskReward(currentTask3.id);
				int completeTowerLevelId = GameApp.Data.GetDataModule(DataName.TowerDataModule).CompleteTowerLevelId;
				int num4 = currentTask3.num;
				if (completeTowerLevelId >= num4 && !flag3)
				{
					return true;
				}
			}
			return false;
		}

		private Dictionary<int, List<NewWorld_newWorldTask>> taskDic = new Dictionary<int, List<NewWorld_newWorldTask>>();
	}
}
