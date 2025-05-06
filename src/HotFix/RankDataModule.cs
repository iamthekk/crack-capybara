using System;
using System.Collections.Generic;
using System.Linq;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.LeaderBoard;

namespace HotFix
{
	public class RankDataModule : IDataModule
	{
		public int GetName()
		{
			return 143;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
			this.rankLoadedUtcDic.Clear();
		}

		public long GetLastLoadedUtc(RankType rankType)
		{
			long num;
			if (this.rankLoadedUtcDic.TryGetValue(rankType, out num))
			{
				return num;
			}
			return 0L;
		}

		public void ClearLastLoadedUtc(RankType rankType)
		{
			if (this.rankLoadedUtcDic.ContainsKey(rankType))
			{
				this.rankLoadedUtcDic.Remove(rankType);
			}
		}

		public void SetLastLoadedUtc(RankType rankType, long timestamp)
		{
			this.rankLoadedUtcDic[rankType] = timestamp;
		}

		public List<BaseRankData> GetLastRankData(RankType rankType)
		{
			List<BaseRankData> list;
			if (this.rankDataDic.TryGetValue(rankType, out list))
			{
				return list;
			}
			return new List<BaseRankData>();
		}

		public void ClearLastRankData(RankType rankType)
		{
			if (this.rankDataDic.ContainsKey(rankType))
			{
				this.rankDataDic.Remove(rankType);
			}
		}

		public void SetLastRankData(RankType rankType, List<BaseRankData> tempList)
		{
			this.rankDataDic[rankType] = tempList;
		}

		public void SetLaseRankDataAdd(RankType rankType, List<BaseRankData> tempList)
		{
			if (this.rankDataDic.ContainsKey(rankType))
			{
				this.rankDataDic[rankType].AddRange(tempList);
				return;
			}
			this.rankDataDic[rankType] = tempList;
		}

		public void SetLastTop3(RankType rankType, RepeatedField<RankUserDto> users)
		{
			this.top3Dic[rankType] = users;
		}

		public List<RankUserDto> GetLastTop3(RankType rankType)
		{
			RepeatedField<RankUserDto> repeatedField;
			if (this.top3Dic.TryGetValue(rankType, out repeatedField))
			{
				return repeatedField.ToList<RankUserDto>();
			}
			return new List<RankUserDto>();
		}

		private Dictionary<RankType, long> rankLoadedUtcDic = new Dictionary<RankType, long>();

		private Dictionary<RankType, List<BaseRankData>> rankDataDic = new Dictionary<RankType, List<BaseRankData>>();

		private Dictionary<RankType, RepeatedField<RankUserDto>> top3Dic = new Dictionary<RankType, RepeatedField<RankUserDto>>();
	}
}
