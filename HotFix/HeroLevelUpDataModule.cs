using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic.Modules;
using LocalModels.Bean;
using Proto.User;
using Server;
using UnityEngine;

namespace HotFix
{
	public class HeroLevelUpDataModule : IDataModule
	{
		public CardData MainPlayCardData
		{
			get
			{
				return this.m_mainPlayCardData;
			}
		}

		public int Level
		{
			get
			{
				return this.m_level;
			}
		}

		public int GetName()
		{
			return 122;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_GameLoginData_SetHeroLevelUpData, new HandlerEvent(this.OnEventSetHeroLevelUpData));
			manager.RegisterEvent(LocalMessageName.CC_HeroLevelUpDataModule_RefreshData, new HandlerEvent(this.OnEventRefreshData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_GameLoginData_SetHeroLevelUpData, new HandlerEvent(this.OnEventSetHeroLevelUpData));
			manager.UnRegisterEvent(LocalMessageName.CC_HeroLevelUpDataModule_RefreshData, new HandlerEvent(this.OnEventRefreshData));
		}

		public void Reset()
		{
		}

		private void SetHeroLevelUpData(UserLoginResponse loginResponse)
		{
			this.RefreshMainCardData();
			this.m_maxLevel = GameApp.Table.GetManager().GetHeroLevelup_HeroLevelupModelInstance().GetAllElements()
				.Count;
			if (loginResponse == null)
			{
				return;
			}
			if (loginResponse.Actor == null)
			{
				return;
			}
			this.RefreshHeroLevelUpData((int)loginResponse.Actor.Level);
		}

		private void RefreshHeroLevelUpData(int level)
		{
			this.RefreshMainCardData();
			this.m_level = level;
			this.m_addAttributeData.Clear();
			AddAttributeHeroLevelUp addAttributeHeroLevelUp = new AddAttributeHeroLevelUp(GameApp.Table.GetManager());
			addAttributeHeroLevelUp.SetData(this.m_level);
			this.m_addAttributeData.Merge(addAttributeHeroLevelUp.MathAll());
		}

		private void RefreshMainCardData()
		{
			HeroDataModule dataModule = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			this.m_mainPlayCardData = dataModule.MainCardData;
		}

		public HeroLevelup_HeroLevelup GetHeroLevelupTable(int level)
		{
			return GameApp.Table.GetManager().GetHeroLevelup_HeroLevelupModelInstance().GetElementById(level);
		}

		public HeroLevelup_HeroLevelup GetHeroLevelupTable()
		{
			return GameApp.Table.GetManager().GetHeroLevelup_HeroLevelupModelInstance().GetElementById(this.Level);
		}

		public bool IsGradeUp(int level)
		{
			HeroLevelup_HeroLevelup heroLevelupTable = this.GetHeroLevelupTable(level);
			return heroLevelupTable != null && heroLevelupTable.gradeUpCost != null && heroLevelupTable.gradeUpCost.Length != 0;
		}

		public bool IsGradeUp()
		{
			return this.IsGradeUp(this.m_level);
		}

		public List<ItemData> GetLevelUpCost(int level)
		{
			List<ItemData> list = new List<ItemData>();
			HeroLevelup_HeroLevelup heroLevelupTable = this.GetHeroLevelupTable(level);
			if (heroLevelupTable == null)
			{
				return list;
			}
			for (int i = 0; i < heroLevelupTable.levelUpCost.Length; i++)
			{
				string text = heroLevelupTable.levelUpCost[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					if (array.Length == 2)
					{
						list.Add(new ItemData(int.Parse(array[0]), (long)int.Parse(array[1])));
					}
				}
			}
			return list;
		}

		public List<ItemData> GetLevelUpCost()
		{
			return this.GetLevelUpCost(this.m_level);
		}

		public List<ItemData> GetGradeUpCost(int level)
		{
			List<ItemData> list = new List<ItemData>();
			HeroLevelup_HeroLevelup heroLevelupTable = this.GetHeroLevelupTable(level);
			if (heroLevelupTable == null)
			{
				return list;
			}
			for (int i = 0; i < heroLevelupTable.gradeUpCost.Length; i++)
			{
				string text = heroLevelupTable.gradeUpCost[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					if (array.Length == 2)
					{
						list.Add(new ItemData(int.Parse(array[0]), (long)int.Parse(array[1])));
					}
				}
			}
			return list;
		}

		public List<ItemData> GetGradeUpCost()
		{
			return this.GetGradeUpCost(this.m_level);
		}

		public List<MergeAttributeData> GetLevelUpRewards(HeroLevelup_HeroLevelup level)
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			for (int i = 0; i < level.levelUpRewards.Length; i++)
			{
				MergeAttributeData mergeAttributeData = new MergeAttributeData(level.levelUpRewards[i], null, null);
				list.Add(mergeAttributeData);
			}
			return list;
		}

		public List<MergeAttributeData> GetLevelUpRewards()
		{
			HeroLevelup_HeroLevelup heroLevelupTable = this.GetHeroLevelupTable(this.m_level);
			if (heroLevelupTable == null)
			{
				return new List<MergeAttributeData>();
			}
			return this.GetLevelUpRewards(heroLevelupTable);
		}

		public List<MergeAttributeData> GetLevelUpRewards(int level)
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			HeroLevelup_HeroLevelup heroLevelupTable = this.GetHeroLevelupTable(level);
			if (heroLevelupTable == null)
			{
				return list;
			}
			for (int i = 0; i < heroLevelupTable.levelUpRewards.Length; i++)
			{
				MergeAttributeData mergeAttributeData = new MergeAttributeData(heroLevelupTable.levelUpRewards[i], null, null);
				list.Add(mergeAttributeData);
			}
			return list;
		}

		public string GetTrainingTitleName(int level, Color color)
		{
			HeroLevelup_HeroLevelup heroLevelupTable = this.GetHeroLevelupTable(level);
			if (heroLevelupTable == null)
			{
				return string.Empty;
			}
			return this.GetTrainingTitleName(heroLevelupTable, color);
		}

		public string GetTrainingTitleName(HeroLevelup_HeroLevelup table, Color color)
		{
			if (table == null)
			{
				return string.Empty;
			}
			int num = table.ID % 10;
			num = ((num == 0) ? 10 : num);
			return string.Format("{0}<color=#{1}>lv{2}</color>", Singleton<LanguageManager>.Instance.GetInfoByID(table.titleName), FrameworkExpand.ToHex(color), num);
		}

		public string GetTrainingTitleName(Color color)
		{
			return this.GetTrainingTitleName(this.m_level, color);
		}

		public int GetShowFgCount()
		{
			return (this.Level - 1) % 10;
		}

		public bool IsHaveLevelUpCost()
		{
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			bool flag = true;
			List<ItemData> levelUpCost = this.GetLevelUpCost(this.Level);
			for (int i = 0; i < levelUpCost.Count; i++)
			{
				ItemData itemData = levelUpCost[i];
				if (itemData != null)
				{
					long itemDataCountByid = dataModule.GetItemDataCountByid((ulong)((long)itemData.ID));
					if (itemData.TotalCount > itemDataCountByid)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		public bool IsHaveGradeUpCost()
		{
			return this.IsHaveGradeUpCost(this.m_level);
		}

		public bool IsHaveGradeUpCost(int level)
		{
			List<ItemData> gradeUpCost = this.GetGradeUpCost(level);
			return this.IsHaveGradeUpCost(gradeUpCost);
		}

		public bool IsHaveGradeUpCost(List<ItemData> costDatas)
		{
			if (costDatas == null)
			{
				return true;
			}
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			bool flag = true;
			for (int i = 0; i < costDatas.Count; i++)
			{
				ItemData itemData = costDatas[i];
				if (itemData != null)
				{
					long itemDataCountByid = dataModule.GetItemDataCountByid((ulong)((long)itemData.ID));
					if (itemData.TotalCount > itemDataCountByid)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		public bool IsLevelFull()
		{
			return this.Level >= this.m_maxLevel;
		}

		private void OnEventSetHeroLevelUpData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsSetHeroLevelUpData eventArgsSetHeroLevelUpData = eventargs as EventArgsSetHeroLevelUpData;
			if (eventArgsSetHeroLevelUpData == null)
			{
				return;
			}
			if (eventArgsSetHeroLevelUpData.m_userLoginResponse == null)
			{
				return;
			}
			this.SetHeroLevelUpData(eventArgsSetHeroLevelUpData.m_userLoginResponse);
		}

		private void OnEventRefreshData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsRefreshHeroLevelUpData eventArgsRefreshHeroLevelUpData = eventargs as EventArgsRefreshHeroLevelUpData;
			if (eventArgsRefreshHeroLevelUpData == null)
			{
				return;
			}
			this.RefreshHeroLevelUpData(eventArgsRefreshHeroLevelUpData.m_level);
		}

		private CardData m_mainPlayCardData;

		private int m_level;

		private int m_maxLevel;

		public int MaxLevel;

		public AddAttributeData m_addAttributeData = new AddAttributeData();
	}
}
