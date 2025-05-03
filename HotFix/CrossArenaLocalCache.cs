using System;
using Framework;
using Framework.Logic;
using Framework.Logic.GameTestTools;

namespace HotFix
{
	public class CrossArenaLocalCache
	{
		public bool HasLastDanCache
		{
			get
			{
				return this.LastDan != 0 && this.LastDanSeasonEndTime != 0L;
			}
		}

		public bool NeedShowDanChange
		{
			get
			{
				if (!this.HasLastDanCache)
				{
					return false;
				}
				CrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
				return dataModule.Dan != 0 && dataModule.CurSeasonId != 0 && this.LastSeasonId != dataModule.CurSeasonId;
			}
		}

		public void ReadLocalData()
		{
			string userString = Utility.PlayerPrefs.GetUserString("CrossArena_LastDanCache", "");
			if (!string.IsNullOrEmpty(userString) && userString.Length > 0)
			{
				string[] array = userString.Split('|', StringSplitOptions.None);
				if (array.Length != 0 && !string.IsNullOrEmpty(array[0]) && !int.TryParse(array[0], out this.LastDan))
				{
					this.LastDan = 0;
				}
				if (array.Length > 1 && !string.IsNullOrEmpty(array[1]) && !long.TryParse(array[1], out this.LastDanSeasonEndTime))
				{
					this.LastDanSeasonEndTime = 0L;
				}
				if (array.Length > 2 && !string.IsNullOrEmpty(array[2]) && !int.TryParse(array[2], out this.LastSeasonId))
				{
					this.LastSeasonId = 0;
				}
			}
			if (this.LastDan == 0 || this.LastDanSeasonEndTime == 0L)
			{
				this.LastDan = 0;
				this.LastDanSeasonEndTime = 0L;
			}
		}

		public void Save(int dan, long time, int seasonId)
		{
			this.LastDan = dan;
			this.LastDanSeasonEndTime = time;
			this.LastSeasonId = seasonId;
			string text = string.Format("{0}|{1}|{2}", dan, time, seasonId);
			Utility.PlayerPrefs.SetUserString("CrossArena_LastDanCache", text);
		}

		[GameTestMethod("竞技场", "清除赛季变更标记", "", 0)]
		private static void OnTest()
		{
			CrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
			dataModule.LocalCache.LastDan = 1;
			CrossArenaProgress crossArenaProgress = new CrossArenaProgress();
			crossArenaProgress.BuildProgress();
			dataModule.LocalCache.LastDanSeasonEndTime = (crossArenaProgress.SeasonOpenTime -= 604800L);
			dataModule.CurSeasonId = 0;
			dataModule.LocalCache.Save(dataModule.LocalCache.LastDan, dataModule.LocalCache.LastDanSeasonEndTime, dataModule.LocalCache.LastSeasonId);
		}

		public int LastDan;

		public long LastDanSeasonEndTime;

		public int LastSeasonId;
	}
}
