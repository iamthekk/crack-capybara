using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Proto.Common;
using Proto.Tower;

namespace HotFix
{
	public class TowerDataModule : IDataModule
	{
		public bool IsAllFinish
		{
			get
			{
				TowerChallenge_TowerLevel towerChallenge_TowerLevel = GameApp.Table.GetManager().GetTowerChallenge_TowerLevelElements().Last<TowerChallenge_TowerLevel>();
				return towerChallenge_TowerLevel != null && this.CurTowerLevelId == towerChallenge_TowerLevel.id && this.CompleteTowerLevelId == towerChallenge_TowerLevel.id;
			}
		}

		public int CurTowerLevelId { get; private set; }

		public int FightLevelId { get; private set; }

		public int NextFightLevelId
		{
			get
			{
				if (this.CurTowerConfig != null)
				{
					int num = this.CurTowerConfig.level.ToList<int>().IndexOf(this.FightLevelId);
					if (num + 1 < this.CurTowerConfig.level.Length)
					{
						return this.CurTowerConfig.level[num + 1];
					}
				}
				return -1;
			}
		}

		public int FightLevelIndex
		{
			get
			{
				if (this.CurTowerConfig != null)
				{
					return this.CurTowerConfig.level.ToList<int>().IndexOf(this.FightLevelId);
				}
				return 0;
			}
		}

		public int CompleteTowerLevelId
		{
			get
			{
				return this.completeTowerLevelIdValue;
			}
			private set
			{
				this.completeTowerLevelIdValue = value;
				this.UpdateCurTowerLevelId(this.completeTowerLevelIdValue);
			}
		}

		private int ClaimedRewardTowerId { get; set; }

		public TowerChallenge_Tower CurTowerConfig
		{
			get
			{
				return this.GetTowerConfigByLevelId(this.CurTowerLevelId);
			}
		}

		public int MaxLevelCount
		{
			get
			{
				return this.CurTowerConfig.level.Length;
			}
		}

		public int CurTowerLevelIndex
		{
			get
			{
				return this.GetLevelIndexByLevelId(this.CurTowerLevelId);
			}
		}

		public int CurTowerLevelNum
		{
			get
			{
				return this.GetLevelNumByLevelId(this.CurTowerLevelId);
			}
		}

		public List<TowerRankDto> RankDtoList { get; } = new List<TowerRankDto>();

		public int CurTowerRank { get; private set; }

		public int MoveLevelLastLevelId
		{
			get
			{
				if (this.GetTowerConfigByLevelId(this.moveLevelLastLevelId) == this.CurTowerConfig)
				{
					return this.moveLevelLastLevelId;
				}
				return this.CurTowerLevelId;
			}
			private set
			{
				this.moveLevelLastLevelId = value;
			}
		}

		public bool IsPlayLevelMoveAnim { get; private set; }

		public int GetName()
		{
			return 133;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_TowerDataMoudule_SetCurTowerLevelIdData, new HandlerEvent(this.OnEventRefreshCurTowerLevelId));
			manager.RegisterEvent(LocalMessageName.CC_TowerDataMoudule_SetCurTowerRankData, new HandlerEvent(this.OnEventRefreshCurTowerRank));
			manager.RegisterEvent(LocalMessageName.CC_TowerDataMoudule_TowerChallengeEnd, new HandlerEvent(this.OnEventTowerChallengeEnd));
			manager.RegisterEvent(LocalMessageName.CC_TowerDataMoudule_SetBattleReadyData, new HandlerEvent(this.OnEventSetBattleReadyData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_TowerDataMoudule_SetCurTowerLevelIdData, new HandlerEvent(this.OnEventRefreshCurTowerLevelId));
			manager.UnRegisterEvent(LocalMessageName.CC_TowerDataMoudule_SetCurTowerRankData, new HandlerEvent(this.OnEventRefreshCurTowerRank));
			manager.UnRegisterEvent(LocalMessageName.CC_TowerDataMoudule_TowerChallengeEnd, new HandlerEvent(this.OnEventTowerChallengeEnd));
			manager.UnRegisterEvent(LocalMessageName.CC_TowerDataMoudule_SetBattleReadyData, new HandlerEvent(this.OnEventSetBattleReadyData));
		}

		public void Reset()
		{
			this.lastLoadRankDataTime = 0L;
		}

		private void OnEventRefreshCurTowerLevelId(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsSetCurTowerLevelIdData eventArgsSetCurTowerLevelIdData = eventArgs as EventArgsSetCurTowerLevelIdData;
			if (eventArgsSetCurTowerLevelIdData == null)
			{
				return;
			}
			this.ClaimedRewardTowerId = eventArgsSetCurTowerLevelIdData.ClaimedRewardTowerId;
			this.CompleteTowerLevelId = eventArgsSetCurTowerLevelIdData.CompleteTowerLevelId;
		}

		private void OnEventRefreshCurTowerRank(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsSetCurTowerRankData eventArgsSetCurTowerRankData = eventArgs as EventArgsSetCurTowerRankData;
			if (eventArgsSetCurTowerRankData == null)
			{
				return;
			}
			this.CurTowerRank = eventArgsSetCurTowerRankData.TowerRank;
		}

		private void OnEventTowerChallengeEnd(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsTowerChallengeEnd eventArgsTowerChallengeEnd = eventArgs as EventArgsTowerChallengeEnd;
			if (eventArgsTowerChallengeEnd == null)
			{
				return;
			}
			this.FightLevelId = this.CurTowerLevelId;
			if (eventArgsTowerChallengeEnd.Result == 1)
			{
				this.CompleteTowerLevelId = eventArgsTowerChallengeEnd.LevelId;
			}
		}

		private void OnEventSetBattleReadyData(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsSetTowerBattleReadyData eventArgsSetTowerBattleReadyData = eventArgs as EventArgsSetTowerBattleReadyData;
			if (eventArgsSetTowerBattleReadyData == null)
			{
				return;
			}
			if (eventArgsSetTowerBattleReadyData.IsReset)
			{
				this.IsPlayLevelMoveAnim = false;
				this.MoveLevelLastLevelId = this.CurTowerLevelId;
				return;
			}
			this.IsPlayLevelMoveAnim = eventArgsSetTowerBattleReadyData.BattleResult == 1;
			this.MoveLevelLastLevelId = eventArgsSetTowerBattleReadyData.BattleLevelId;
		}

		private void UpdateCurTowerLevelId(int completeTowerLevelId)
		{
			this.CurTowerLevelId = this.CalculateNextLevelId(completeTowerLevelId, true);
		}

		private int CalculateNextLevelId(int completeTowerLevelId, bool isCheckReward)
		{
			TowerChallenge_Tower towerConfig = this.GetTowerConfigByLevelId(completeTowerLevelId);
			if (towerConfig == null)
			{
				return GameApp.Table.GetManager().GetTowerChallenge_TowerModelInstance().GetAllElements()[0].level[0];
			}
			int levelIndexByLevelId = this.GetLevelIndexByLevelId(completeTowerLevelId);
			if (levelIndexByLevelId < towerConfig.level.Length - 1)
			{
				return towerConfig.level[levelIndexByLevelId + 1];
			}
			if (isCheckReward && !this.CheckTowerRewardIsClaimed(towerConfig))
			{
				return completeTowerLevelId;
			}
			List<TowerChallenge_Tower> list = GameApp.Table.GetManager().GetTowerChallenge_TowerModelInstance().GetAllElements()
				.ToList<TowerChallenge_Tower>();
			int num = list.FindIndex((TowerChallenge_Tower item) => item.id == towerConfig.id);
			if (num + 1 >= list.Count)
			{
				return completeTowerLevelId;
			}
			return list[num + 1].level[0];
		}

		public int CalculateShouldChallengeLevelID(int completeTowerLevelId)
		{
			return this.CalculateNextLevelId(completeTowerLevelId, false);
		}

		public bool CheckTowerComplete(int towerId)
		{
			TowerChallenge_Tower elementById = GameApp.Table.GetManager().GetTowerChallenge_TowerModelInstance().GetElementById(towerId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("[TowerChallenge_Tower] not found id={0}", towerId));
				return true;
			}
			TowerChallenge_Tower curTowerConfig = this.CurTowerConfig;
			int towerConfigIndex = this.GetTowerConfigIndex(elementById);
			int towerConfigIndex2 = this.GetTowerConfigIndex(curTowerConfig);
			return towerConfigIndex2 > towerConfigIndex || (towerConfigIndex2 >= towerConfigIndex && (curTowerConfig.level.Length == 0 || curTowerConfig.level[curTowerConfig.level.Length - 1] == this.CompleteTowerLevelId));
		}

		public TowerChallenge_TowerLevel GetLevelConfigByLevelId(int levelID)
		{
			return GameApp.Table.GetManager().GetTowerChallenge_TowerLevelModelInstance().GetElementById(levelID);
		}

		public TowerChallenge_Tower GetTowerConfigByLevelId(int levelID)
		{
			return GameApp.Table.GetManager().GetTowerChallenge_TowerModelInstance().GetAllElements()
				.ToList<TowerChallenge_Tower>()
				.Find((TowerChallenge_Tower item) => item.level.Contains(levelID));
		}

		public int GetTowerConfigIndex(TowerChallenge_Tower towerConfig)
		{
			return GameApp.Table.GetManager().GetTowerChallenge_TowerModelInstance().GetAllElements()
				.ToList<TowerChallenge_Tower>()
				.IndexOf(towerConfig);
		}

		public int GetTowerConfigNum(TowerChallenge_Tower towerConfig)
		{
			return this.GetTowerConfigIndex(towerConfig) + 1;
		}

		public bool CheckCanChallengeByLevelId(int levelId)
		{
			return levelId == this.CurTowerLevelId && !this.IsShowTowerTopReward(levelId);
		}

		public bool IsShowTowerTopReward(int levelId)
		{
			if (this.GetLevelIndexByLevelId(levelId) != this.MaxLevelCount - 1 || levelId != this.CompleteTowerLevelId)
			{
				return false;
			}
			List<TowerChallenge_Tower> list = GameApp.Table.GetManager().GetTowerChallenge_TowerModelInstance().GetAllElements()
				.ToList<TowerChallenge_Tower>();
			if (list.Count <= 0)
			{
				return false;
			}
			TowerChallenge_Tower towerChallenge_Tower = list[list.Count - 1];
			TowerChallenge_Tower curTowerConfig = this.CurTowerConfig;
			return curTowerConfig.id == towerChallenge_Tower.id || !this.CheckTowerRewardIsClaimed(curTowerConfig);
		}

		public int GetLevelIndexByLevelId(int levelID)
		{
			TowerChallenge_Tower towerConfigByLevelId = this.GetTowerConfigByLevelId(levelID);
			if (towerConfigByLevelId == null)
			{
				HLog.LogError(string.Format("TowerChallenge_Tower not found id = {0}", levelID));
				return 0;
			}
			return towerConfigByLevelId.level.ToList<int>().IndexOf(levelID);
		}

		public int GetLevelNumByLevelId(int levelID)
		{
			return this.GetLevelIndexByLevelId(levelID) + 1;
		}

		public List<TowerEnemyData> GetLevelMemberData(TowerChallenge_TowerLevel levelData)
		{
			List<TowerEnemyData> list = new List<TowerEnemyData>();
			for (int i = 0; i < levelData.MemberData.Length; i++)
			{
				string[] array = levelData.MemberData[i].Split(',', StringSplitOptions.None);
				int num = int.Parse(array[0]);
				int num2 = int.Parse(array[1]);
				long num3 = long.Parse(array[2]);
				TowerEnemyData towerEnemyData = new TowerEnemyData(num, num2, num3);
				list.Add(towerEnemyData);
			}
			return list;
		}

		public List<ItemData> GetTowerReward(TowerChallenge_Tower towerConfig)
		{
			int[] array;
			if (LoginDataModule.IsTestB())
			{
				array = towerConfig.ChestRewardB;
			}
			else
			{
				array = towerConfig.ChestReward;
			}
			List<ItemData> list = new List<ItemData>();
			for (int i = 0; i < array.Length - 1; i += 2)
			{
				int num = array[i];
				int num2 = array[i + 1];
				list.Add(new ItemData(num, (long)num2));
			}
			return list;
		}

		public bool CheckTowerRewardIsClaimed(TowerChallenge_Tower towerConfig)
		{
			TowerChallenge_Tower elementById = GameApp.Table.GetManager().GetTowerChallenge_TowerModelInstance().GetElementById(this.ClaimedRewardTowerId);
			if (elementById == null)
			{
				return false;
			}
			int towerConfigIndex = this.GetTowerConfigIndex(elementById);
			return this.GetTowerConfigIndex(towerConfig) <= towerConfigIndex;
		}

		public void CheckLoadRankDataList(Action onLoading, Action<bool> onLoadEnd)
		{
			if (GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalUTC - this.lastLoadRankDataTime >= this.rankDataCacheTime)
			{
				this.curRankPage = 1;
				this.LoadRankDataList(false, onLoading, delegate(bool res, bool isUpdateData)
				{
					Action<bool> onLoadEnd3 = onLoadEnd;
					if (onLoadEnd3 == null)
					{
						return;
					}
					onLoadEnd3(res);
				});
				return;
			}
			Action<bool> onLoadEnd2 = onLoadEnd;
			if (onLoadEnd2 == null)
			{
				return;
			}
			onLoadEnd2(true);
		}

		public void LoadRankDataList(bool isNextPage, Action onLoading, Action<bool, bool> onLoadEnd)
		{
			if (this.curRankPage < Singleton<GameConfig>.Instance.Tower_RankMaxPage)
			{
				if (onLoading != null)
				{
					onLoading();
				}
				int nextPage = ((isNextPage && this.curRankPage < Singleton<GameConfig>.Instance.Tower_RankMaxPage && this.RankDtoList.Count >= Singleton<GameConfig>.Instance.Tower_RankSingleCount) ? (this.curRankPage + 1) : this.curRankPage);
				NetworkUtils.Tower.TowerRankRequest(nextPage, false, delegate(bool isOk, TowerRankResponse resp)
				{
					if (!isOk)
					{
						Action<bool, bool> onLoadEnd3 = onLoadEnd;
						if (onLoadEnd3 == null)
						{
							return;
						}
						onLoadEnd3(false, false);
						return;
					}
					else
					{
						bool flag = false;
						if (nextPage == 1)
						{
							this.RankDtoList.Clear();
						}
						this.lastLoadRankDataTime = GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalUTC;
						if (resp.Rank != null && resp.Rank.Count > 0)
						{
							flag = this.curRankPage != nextPage;
							this.RankDtoList.AddRange(resp.Rank);
							this.curRankPage = nextPage;
						}
						Action<bool, bool> onLoadEnd4 = onLoadEnd;
						if (onLoadEnd4 == null)
						{
							return;
						}
						onLoadEnd4(true, flag);
						return;
					}
				});
				return;
			}
			Action<bool, bool> onLoadEnd2 = onLoadEnd;
			if (onLoadEnd2 == null)
			{
				return;
			}
			onLoadEnd2(true, false);
		}

		private int completeTowerLevelIdValue;

		private readonly long rankDataCacheTime = 20L;

		private long lastLoadRankDataTime;

		private int curRankPage;

		private int moveLevelLastLevelId;
	}
}
