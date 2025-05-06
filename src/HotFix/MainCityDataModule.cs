using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Proto.Common;
using Proto.User;
using UnityEngine;

namespace HotFix
{
	public class MainCityDataModule : IDataModule
	{
		public int GetName()
		{
			return 109;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_GameLoginData_SetMainCityData, new HandlerEvent(this.OnEventSetMainCityData));
			manager.RegisterEvent(LocalMessageName.CC_MainCityData_RefreshMainCityGoldData, new HandlerEvent(this.OnEventRefreshMainCityGoldData));
			manager.RegisterEvent(LocalMessageName.CC_MainCityData_RefreshMainCityBoxData, new HandlerEvent(this.OnEventRefreshMainCityBoxData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_GameLoginData_SetMainCityData, new HandlerEvent(this.OnEventSetMainCityData));
			manager.UnRegisterEvent(LocalMessageName.CC_MainCityData_RefreshMainCityGoldData, new HandlerEvent(this.OnEventRefreshMainCityGoldData));
			manager.UnRegisterEvent(LocalMessageName.CC_MainCityData_RefreshMainCityBoxData, new HandlerEvent(this.OnEventRefreshMainCityBoxData));
		}

		public void Reset()
		{
		}

		private void InitData(UserLoginResponse loginResponse)
		{
			this.m_cityData = loginResponse.City;
			this.m_goldLevel = this.m_cityData.GoldmineLevel;
			this.m_goldTimeSpan = (long)((this.m_cityData.LastGoldmineRewardTime == 0UL) ? this.m_cityData.InitGoldTime : this.m_cityData.LastGoldmineRewardTime);
			this.m_refreshChestAddRowIDs.Clear();
			this.m_chestDatas.Clear();
			for (int i = 0; i < loginResponse.City.CityChest.Count; i++)
			{
				CityChestDto cityChestDto = loginResponse.City.CityChest[i];
				if (cityChestDto != null)
				{
					this.m_chestDatas[cityChestDto.RowId] = new MainCityBoxData(cityChestDto.RowId, (int)cityChestDto.Quality);
				}
			}
			this.m_startChestHangeTime = loginResponse.City.StartChestHangTime;
			this.m_refreshChestHangTime = loginResponse.City.RefreshChestHangTime;
			this.m_chestIntegral = loginResponse.City.Score;
		}

		public Dictionary<int, ItemData> GetItemDatasForGold(out bool isFull, out bool isUpdate, out bool isHaveReward, out long span)
		{
			isFull = false;
			isUpdate = false;
			isHaveReward = false;
			span = 0L;
			Dictionary<int, ItemData> dictionary = new Dictionary<int, ItemData>();
			if (GameApp.Table.GetManager().GetMainLevelReward_AFKrewardModelInstance().GetElementById(this.m_goldLevel) == null)
			{
				return dictionary;
			}
			if (this.m_cityData == null)
			{
				return dictionary;
			}
			isFull = this.IsFullForGold(out span);
			isUpdate = this.IsCanLevelUpForGold();
			dictionary = this.GetSpanItemDatasForGold(this.m_goldLevel, (int)span);
			foreach (KeyValuePair<int, ItemData> keyValuePair in dictionary)
			{
				if (keyValuePair.Value != null && keyValuePair.Value.TotalCount > 0L)
				{
					isHaveReward = true;
					break;
				}
			}
			return dictionary;
		}

		public bool IsFullForGold(out long span)
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			span = dataModule.LocalUTC - this.m_goldTimeSpan;
			long goldMaxDuration = this.GetGoldMaxDuration();
			if (span > goldMaxDuration)
			{
				span = goldMaxDuration;
				return true;
			}
			return false;
		}

		public float GetProgressForGold()
		{
			if (GameApp.Table.GetManager().GetMainLevelReward_AFKrewardModelInstance().GetElementById(this.m_goldLevel) == null)
			{
				return 0f;
			}
			if (this.m_cityData == null)
			{
				return 0f;
			}
			long num = GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalUTC - this.m_goldTimeSpan;
			long goldMaxDuration = this.GetGoldMaxDuration();
			if (num > goldMaxDuration)
			{
				num = goldMaxDuration;
			}
			return (float)num / (float)goldMaxDuration;
		}

		public bool IsCanLevelUpForGold()
		{
			MainLevelReward_AFKreward elementById = GameApp.Table.GetManager().GetMainLevelReward_AFKrewardModelInstance().GetElementById(this.m_goldLevel);
			if (elementById == null)
			{
				return false;
			}
			bool flag = false;
			long num = (long)GameApp.Data.GetDataModule(DataName.MainDataModule).ChapterMaxProcess;
			if (elementById.RequiredLevel != 0 && num >= (long)elementById.RequiredLevel)
			{
				flag = true;
			}
			return flag;
		}

		public string GetRequiredLevel()
		{
			MainLevelReward_AFKreward elementById = GameApp.Table.GetManager().GetMainLevelReward_AFKrewardModelInstance().GetElementById(this.m_goldLevel);
			if (elementById == null)
			{
				return string.Empty;
			}
			return string.Format("{0}-{1}", elementById.RequiredLevel / 1000, elementById.RequiredLevel % 1000);
		}

		public Dictionary<int, ItemData> GetSpanItemDatasForGold(int level, int span)
		{
			Dictionary<int, ItemData> dictionary = new Dictionary<int, ItemData>();
			MainLevelReward_AFKreward elementById = GameApp.Table.GetManager().GetMainLevelReward_AFKrewardModelInstance().GetElementById(level);
			if (elementById == null)
			{
				return dictionary;
			}
			dictionary.Add(1, new ItemData(1, (long)(span / Singleton<GameConfig>.Instance.Hangup_Gold_Duration * elementById.HangGold)));
			dictionary.Add(3, new ItemData(3, (long)(span / Singleton<GameConfig>.Instance.Hangup_Dust_Duration * elementById.HangDust)));
			dictionary.Add(4, new ItemData(4, (long)(span / Singleton<GameConfig>.Instance.Hangup_HeroExp_Duration * elementById.HangHeroExp)));
			return dictionary;
		}

		public long GetGoldMaxDuration()
		{
			return Singleton<GameConfig>.Instance.Hangup_MaxDuration;
		}

		public bool IsFullLevelForGold()
		{
			IList<MainLevelReward_AFKreward> allElements = GameApp.Table.GetManager().GetMainLevelReward_AFKrewardModelInstance().GetAllElements();
			return this.m_goldLevel >= allElements.Count;
		}

		public bool IsInShowTime(long span)
		{
			return span >= Singleton<GameConfig>.Instance.Hangup_MinShowDuration;
		}

		public bool IsBoxLoop(out List<int> durations, out int index, out long time)
		{
			long num = GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalUTC - (long)this.m_startChestHangeTime;
			durations = Singleton<GameConfig>.Instance.MainCity_BoxDurations;
			long num2 = 0L;
			bool flag = true;
			time = 0L;
			index = -1;
			for (int i = 0; i < durations.Count; i++)
			{
				num2 += (long)durations[i];
				if (num2 >= num)
				{
					index = i;
					time = num2 - num;
					flag = false;
					break;
				}
			}
			return flag;
		}

		public int GetBoxLoopIndex(out List<int> durations, out long time)
		{
			long num = GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalUTC - (long)this.m_refreshChestHangTime;
			durations = Singleton<GameConfig>.Instance.MainCity_BoxLoopDurations;
			int num2 = 0;
			time = 0L;
			int num3 = 0;
			for (int i = 0; i < durations.Count; i++)
			{
				num3 += durations[i];
			}
			long num4 = num % (long)num3;
			for (int j = 0; j < durations.Count; j++)
			{
				num4 -= (long)durations[j];
				if (num4 < 0L)
				{
					num2 = j;
					time = -num4;
					break;
				}
			}
			return num2;
		}

		public int GetBoxMaxIntegralCount()
		{
			return Singleton<GameConfig>.Instance.MainCity_BoxMaxIntegralCount;
		}

		public int GetBoxMaxCount()
		{
			return Singleton<GameConfig>.Instance.MainCity_BoxMaxCount;
		}

		public bool IsBoxFull()
		{
			return this.m_chestDatas.Count >= this.GetBoxMaxCount();
		}

		public int GetBoxCount()
		{
			return this.m_chestDatas.Count;
		}

		public bool IsHaveBox()
		{
			return this.m_chestDatas.Count > 0;
		}

		public bool IsCanReceiveBoxIntegral()
		{
			return this.m_chestIntegral >= this.GetBoxMaxIntegralCount();
		}

		public List<ulong> GetBoxRowIDs()
		{
			List<ulong> list = new List<ulong>();
			foreach (KeyValuePair<ulong, MainCityBoxData> keyValuePair in this.m_chestDatas)
			{
				if (keyValuePair.Value != null)
				{
					list.Add(keyValuePair.Value.m_rowID);
				}
			}
			return list;
		}

		public List<ulong> GetRefreshChestAddRowIDs()
		{
			return this.m_refreshChestAddRowIDs;
		}

		private void OnEventSetMainCityData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsSetMainCityData eventArgsSetMainCityData = eventargs as EventArgsSetMainCityData;
			if (eventArgsSetMainCityData == null)
			{
				return;
			}
			this.InitData(eventArgsSetMainCityData.m_userLoginResponse);
		}

		private void OnEventRefreshMainCityGoldData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsRefreshMainCityGoldData eventArgsRefreshMainCityGoldData = eventargs as EventArgsRefreshMainCityGoldData;
			if (eventArgsRefreshMainCityGoldData == null)
			{
				return;
			}
			if (eventArgsRefreshMainCityGoldData.m_level > 0)
			{
				this.m_goldLevel = eventArgsRefreshMainCityGoldData.m_level;
			}
			if (eventArgsRefreshMainCityGoldData.m_timeSpan > 0L)
			{
				this.m_goldTimeSpan = eventArgsRefreshMainCityGoldData.m_timeSpan;
			}
		}

		private void OnEventRefreshMainCityBoxData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsRefreshMainCityBoxData eventArgsRefreshMainCityBoxData = eventargs as EventArgsRefreshMainCityBoxData;
			if (eventArgsRefreshMainCityBoxData == null)
			{
				return;
			}
			if (eventArgsRefreshMainCityBoxData.m_chestDatas != null)
			{
				List<ulong> list = new List<ulong>();
				foreach (KeyValuePair<ulong, MainCityBoxData> keyValuePair in this.m_chestDatas)
				{
					list.Add(keyValuePair.Key);
				}
				this.m_chestDatas.Clear();
				this.m_refreshChestAddRowIDs.Clear();
				for (int i = 0; i < eventArgsRefreshMainCityBoxData.m_chestDatas.Count; i++)
				{
					CityChestDto cityChestDto = eventArgsRefreshMainCityBoxData.m_chestDatas[i];
					if (cityChestDto != null)
					{
						this.m_chestDatas[cityChestDto.RowId] = new MainCityBoxData(cityChestDto.RowId, (int)cityChestDto.Quality);
						if (!list.Contains(cityChestDto.RowId))
						{
							this.m_refreshChestAddRowIDs.Add(cityChestDto.RowId);
						}
					}
				}
			}
			if (eventArgsRefreshMainCityBoxData.m_startChestHangeTime != 0UL)
			{
				this.m_startChestHangeTime = eventArgsRefreshMainCityBoxData.m_startChestHangeTime;
			}
			if (eventArgsRefreshMainCityBoxData.m_refreshChestHangTime != 0UL)
			{
				this.m_refreshChestHangTime = eventArgsRefreshMainCityBoxData.m_refreshChestHangTime;
			}
			if (eventArgsRefreshMainCityBoxData.m_chestIntegral >= 0)
			{
				this.m_chestIntegral = eventArgsRefreshMainCityBoxData.m_chestIntegral;
			}
			if (eventArgsRefreshMainCityBoxData.m_removeRowIDs != null)
			{
				for (int j = 0; j < eventArgsRefreshMainCityBoxData.m_removeRowIDs.Count; j++)
				{
					ulong num = eventArgsRefreshMainCityBoxData.m_removeRowIDs[j];
					this.m_chestDatas.Remove(num);
				}
			}
		}

		public CityDto m_cityData;

		public int m_goldLevel;

		public long m_goldTimeSpan;

		public Dictionary<ulong, MainCityBoxData> m_chestDatas = new Dictionary<ulong, MainCityBoxData>();

		public ulong m_startChestHangeTime;

		public ulong m_refreshChestHangTime;

		public int m_chestIntegral;

		[SerializeField]
		private List<ulong> m_refreshChestAddRowIDs = new List<ulong>();

		public bool IsSkipAni;
	}
}
