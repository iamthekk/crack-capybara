using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.Logic;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class GameEventSkillBuildFactory
	{
		public void Init(int randomSeed)
		{
			this.initBuildList.Clear();
			this.buildDic.Clear();
			this.unlockSkills.Clear();
			this.showedUnlockSkills.Clear();
			this.levelUpSkillSeed.Clear();
			this.refreshSkillSeed.Clear();
			this.adRefreshSkillSeed.Clear();
			IList<GameSkillBuild_skillBuild> allElements = GameApp.Table.GetManager().GetGameSkillBuild_skillBuildModelInstance().GetAllElements();
			this.chapterModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			XRandom xrandom = new XRandom(randomSeed);
			Chapter_skillExp chapter_skillExp = GameApp.Table.GetManager().GetChapter_skillExpModelInstance().GetAllElements()
				.Last<Chapter_skillExp>();
			for (int i = 0; i < chapter_skillExp.id; i++)
			{
				int num = xrandom.NextInt();
				this.levelUpSkillSeed.Add(num);
			}
			for (int j = 0; j < 100; j++)
			{
				int num2 = xrandom.NextInt();
				int num3 = xrandom.NextInt();
				this.refreshSkillSeed.Add(num2);
				this.adRefreshSkillSeed.Add(num3);
			}
			List<int> passChapterIds = this.chapterModule.GetPassChapterIds();
			for (int k = 0; k < allElements.Count; k++)
			{
				GameSkillBuild_skillBuild gameSkillBuild_skillBuild = allElements[k];
				if (!gameSkillBuild_skillBuild.source.Contains(0) && (gameSkillBuild_skillBuild.unlockChapter <= 0 || passChapterIds.Contains(gameSkillBuild_skillBuild.unlockChapter)))
				{
					GameEventSkillBuildData gameEventSkillBuildData = new GameEventSkillBuildData();
					gameEventSkillBuildData.SetTable(gameSkillBuild_skillBuild);
					this.initBuildList.Add(gameEventSkillBuildData);
					if (gameEventSkillBuildData.level == 1 && gameEventSkillBuildData.composeSkillList.Count == 0 && (gameEventSkillBuildData.config.needGroup == null || gameEventSkillBuildData.config.needGroup.Length == 0))
					{
						for (int l = 0; l < gameSkillBuild_skillBuild.source.Length; l++)
						{
							SkillBuildSourceType skillBuildSourceType = (SkillBuildSourceType)gameSkillBuild_skillBuild.source[l];
							List<GameEventSkillBuildData> list;
							if (this.buildDic.TryGetValue(skillBuildSourceType, out list))
							{
								list.Add(gameEventSkillBuildData);
							}
							else
							{
								this.buildDic.Add(skillBuildSourceType, new List<GameEventSkillBuildData> { gameEventSkillBuildData });
							}
						}
					}
				}
			}
			if (!this.buildDic.ContainsKey(SkillBuildSourceType.EventUpgradeSkill))
			{
				this.buildDic.Add(SkillBuildSourceType.EventUpgradeSkill, new List<GameEventSkillBuildData>());
			}
		}

		public List<GameEventSkillBuildData> GetRandomList(SkillBuildSourceType sourceType, int randomNum, int seed)
		{
			SkillBuildQuality skillBuildQuality = this.RandomQuality(sourceType, seed);
			return this.GetRandomList(sourceType, randomNum, seed, skillBuildQuality);
		}

		private List<GameEventSkillBuildData> GetRandomList(SkillBuildSourceType sourceType, int randomNum, int seed, SkillBuildQuality quality)
		{
			List<GameEventSkillBuildData> list = new List<GameEventSkillBuildData>();
			List<GameEventSkillBuildData> list2 = new List<GameEventSkillBuildData>();
			List<GameEventSkillBuildData> list3;
			if (this.buildDic.TryGetValue(sourceType, out list3))
			{
				if (Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Chapter && this.chapterModule.CurrentChapter.id == 1)
				{
					int num = (PlayerPrefsKeys.GetChapterFightTime(this.chapterModule.CurrentChapter.id) + 1) * 1000 + (Singleton<GameEventController>.Instance.PlayerData.ExpLevel.mVariable - 1);
					GameSkillBuild_first elementById = GameApp.Table.GetManager().GetGameSkillBuild_firstModelInstance().GetElementById(num);
					if (elementById != null)
					{
						for (int i = 0; i < list3.Count; i++)
						{
							if (elementById.skills.Contains(list3[i].id) && (quality == SkillBuildQuality.All || list3[i].quality == quality))
							{
								list2.Add(list3[i]);
							}
						}
					}
					if (list2.Count == 0)
					{
						for (int j = 0; j < list3.Count; j++)
						{
							if (quality == SkillBuildQuality.All || list3[j].quality == quality)
							{
								list2.Add(list3[j]);
							}
						}
					}
				}
				else
				{
					for (int k = 0; k < list3.Count; k++)
					{
						if (quality == SkillBuildQuality.All || list3[k].quality == quality)
						{
							list2.Add(list3[k]);
						}
					}
				}
				if (list2.Count == 0)
				{
					list2.AddRange(list3);
				}
			}
			int num2 = 0;
			while (num2 < randomNum && list2.Count != 0)
			{
				GameEventSkillBuildData gameEventSkillBuildData = this.RandomSkill(list2, seed);
				if (gameEventSkillBuildData != null)
				{
					list2.Remove(gameEventSkillBuildData);
					list.Add(gameEventSkillBuildData);
				}
				num2++;
			}
			if ((sourceType == SkillBuildSourceType.BoxGold || sourceType == SkillBuildSourceType.BoxSilver || sourceType == SkillBuildSourceType.BoxBronze) && list.Count < randomNum)
			{
				int num3 = randomNum - list.Count;
				List<GameEventSkillBuildData> randomList = this.GetRandomList(SkillBuildSourceType.Normal, num3, seed, SkillBuildQuality.Gray);
				list.AddRange(randomList);
			}
			if (list.Count < randomNum)
			{
				while (list.Count < randomNum)
				{
					GameEventSkillBuildData gameEventSkillBuildData2 = this.CreateFillingData();
					list.Add(gameEventSkillBuildData2);
				}
				list = this.RandomSort(list);
			}
			return list;
		}

		public void SelectSkill(GameEventSkillBuildData data, bool checkUnlock = true)
		{
			if (data == null)
			{
				return;
			}
			foreach (SkillBuildSourceType skillBuildSourceType in this.buildDic.Keys)
			{
				List<GameEventSkillBuildData> list = this.buildDic[skillBuildSourceType];
				list.Remove(data);
				GameEventSkillBuildData nextLevelData = this.GetNextLevelData(data, skillBuildSourceType);
				if (nextLevelData != null)
				{
					list.Add(nextLevelData);
				}
				List<GameEventSkillBuildData> activeOtherSkills = this.GetActiveOtherSkills(data, skillBuildSourceType);
				for (int i = 0; i < activeOtherSkills.Count; i++)
				{
					if (!list.Contains(activeOtherSkills[i]))
					{
						list.Add(activeOtherSkills[i]);
					}
				}
				List<GameEventSkillBuildData> activeComposeSkills = this.GetActiveComposeSkills(data, skillBuildSourceType);
				if (activeComposeSkills.Count > 0)
				{
					list.AddRange(activeComposeSkills);
				}
			}
			if (checkUnlock)
			{
				this.CheckGetComposeSkill(data);
			}
		}

		private GameEventSkillBuildData GetNextLevelData(GameEventSkillBuildData data, SkillBuildSourceType sourceType)
		{
			for (int i = 0; i < this.initBuildList.Count; i++)
			{
				GameEventSkillBuildData gameEventSkillBuildData = this.initBuildList[i];
				if (gameEventSkillBuildData.config.source.Contains((int)sourceType) && gameEventSkillBuildData.id != data.id && gameEventSkillBuildData.groupId == data.groupId && gameEventSkillBuildData.level - 1 == data.level)
				{
					return gameEventSkillBuildData;
				}
			}
			return null;
		}

		private List<GameEventSkillBuildData> GetActiveOtherSkills(GameEventSkillBuildData data, SkillBuildSourceType sourceType)
		{
			List<GameEventSkillBuildData> list = new List<GameEventSkillBuildData>();
			for (int i = 0; i < this.initBuildList.Count; i++)
			{
				GameEventSkillBuildData gameEventSkillBuildData = this.initBuildList[i];
				if (!gameEventSkillBuildData.IsComposeSkill && !Singleton<GameEventController>.Instance.IsHaveSkillBuild(gameEventSkillBuildData.id) && gameEventSkillBuildData.config.source.Contains((int)sourceType) && gameEventSkillBuildData.config.needGroup.Contains(data.groupId) && gameEventSkillBuildData.level == 1)
				{
					list.Add(gameEventSkillBuildData);
				}
			}
			return list;
		}

		private List<GameEventSkillBuildData> GetActiveComposeSkills(GameEventSkillBuildData data, SkillBuildSourceType sourceType)
		{
			List<GameEventSkillBuildData> list = new List<GameEventSkillBuildData>();
			List<GameEventSkillBuildData> list2 = new List<GameEventSkillBuildData>();
			for (int i = 0; i < this.initBuildList.Count; i++)
			{
				if (this.initBuildList[i].IsComposeSkill && this.initBuildList[i].config.source.Contains((int)sourceType) && this.initBuildList[i].level == 1 && this.initBuildList[i].IsHaveComposeSkill(data.id))
				{
					list2.Add(this.initBuildList[i]);
				}
			}
			for (int j = 0; j < list2.Count; j++)
			{
				if (!Singleton<GameEventController>.Instance.IsHaveSkillBuild(list2[j].id))
				{
					bool flag = false;
					int num = 0;
					List<int> composeSkillList = list2[j].composeSkillList;
					for (int k = 0; k < composeSkillList.Count; k++)
					{
						if (Singleton<GameEventController>.Instance.IsHaveSkillBuild(composeSkillList[k]))
						{
							num++;
						}
					}
					if (num == composeSkillList.Count)
					{
						flag = true;
					}
					if (flag)
					{
						list.Add(list2[j]);
					}
				}
			}
			return list;
		}

		private GameEventSkillBuildData RandomSkill(List<GameEventSkillBuildData> randomList, int seed)
		{
			int num = 0;
			for (int i = 0; i < randomList.Count; i++)
			{
				num += randomList[i].weight;
			}
			int num2 = new XRandom(seed).Range(0, num);
			int num3 = 0;
			for (int j = 0; j < randomList.Count; j++)
			{
				num3 += randomList[j].weight;
				if (num2 < num3)
				{
					return randomList[j];
				}
			}
			return null;
		}

		private SkillBuildQuality RandomQuality(SkillBuildSourceType sourceType, int seed)
		{
			if (sourceType > SkillBuildSourceType.None)
			{
				List<int> list = new List<int>();
				AddAttributeDataModule dataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
				GameSkillBuild_skillRandom elementById = GameApp.Table.GetManager().GetGameSkillBuild_skillRandomModelInstance().GetElementById((int)sourceType);
				if (elementById != null)
				{
					int num = 0;
					for (int i = 0; i < elementById.weight.Length; i++)
					{
						float num2 = 0f;
						if (i == 1)
						{
							num2 = dataModule.MemberAttributeData.OrangeSkillRate.AsFloat();
						}
						else if (i == 2)
						{
							num2 = dataModule.MemberAttributeData.RedSkillRate.AsFloat();
						}
						int num3 = (int)((float)elementById.weight[i] * (1f + num2));
						list.Add(num3);
						num += num3;
					}
					int num4 = new XRandom(seed).Range(0, num);
					int num5 = 0;
					for (int j = 0; j < list.Count; j++)
					{
						num5 += list[j];
						if (num4 < num5)
						{
							return (SkillBuildQuality)j;
						}
					}
				}
			}
			return SkillBuildQuality.All;
		}

		private GameEventSkillBuildData CreateFillingData()
		{
			GameEventSkillBuildData gameEventSkillBuildData = new GameEventSkillBuildData();
			GameSkillBuild_skillBuild elementById = GameApp.Table.GetManager().GetGameSkillBuild_skillBuildModelInstance().GetElementById(100000);
			gameEventSkillBuildData.SetTable(elementById);
			return gameEventSkillBuildData;
		}

		private List<GameEventSkillBuildData> RandomSort(List<GameEventSkillBuildData> list)
		{
			List<GameEventSkillBuildData> list2 = new List<GameEventSkillBuildData>();
			while (list.Count > 0)
			{
				int num = Utility.Math.Random(0, list.Count);
				list2.Add(list[num]);
				list.RemoveAt(num);
			}
			return list2;
		}

		public int GetSkillBuildGroupMaxLevel(int groupId)
		{
			int num = 0;
			for (int i = 0; i < this.initBuildList.Count; i++)
			{
				GameEventSkillBuildData gameEventSkillBuildData = this.initBuildList[i];
				if (gameEventSkillBuildData.groupId == groupId && num < gameEventSkillBuildData.level)
				{
					num = gameEventSkillBuildData.level;
				}
			}
			return num;
		}

		public bool IsSkillPoolEmpty(SkillBuildSourceType sourceType)
		{
			List<GameEventSkillBuildData> list;
			return !this.buildDic.TryGetValue(sourceType, out list) || list.Count == 0;
		}

		private void CheckUnlockSkills(int selectBuildId)
		{
			List<GameEventSkillBuildData> composeSkills = this.GetComposeSkills();
			for (int i = 0; i < composeSkills.Count; i++)
			{
				GameEventSkillBuildData gameEventSkillBuildData = composeSkills[i];
				if (!Singleton<GameEventController>.Instance.IsHaveSkillBuild(gameEventSkillBuildData.id) && !this.showedUnlockSkills.Contains(gameEventSkillBuildData.groupId))
				{
					int skillBuildGroupMaxLevel = this.GetSkillBuildGroupMaxLevel(gameEventSkillBuildData.groupId);
					if (gameEventSkillBuildData.level == skillBuildGroupMaxLevel)
					{
						int num = 0;
						for (int j = 0; j < gameEventSkillBuildData.composeSkillList.Count; j++)
						{
							int num2 = gameEventSkillBuildData.composeSkillList[j];
							GameSkillBuild_skillBuild elementById = GameApp.Table.GetManager().GetGameSkillBuild_skillBuildModelInstance().GetElementById(num2);
							if (elementById == null)
							{
								HLog.LogError(string.Format("Table [GameSkillBuild] not found id = {0}", num2));
							}
							else if (Singleton<GameEventController>.Instance.IsHaveSkillGroupId(elementById.groupId))
							{
								num++;
							}
						}
						if (num == gameEventSkillBuildData.composeSkillList.Count && !this.unlockSkills.Contains(gameEventSkillBuildData))
						{
							this.unlockSkills.Add(gameEventSkillBuildData);
							this.showedUnlockSkills.Add(gameEventSkillBuildData.groupId);
						}
					}
				}
			}
		}

		private void CheckGetComposeSkill(GameEventSkillBuildData data)
		{
			if (data == null || !data.IsComposeSkill || data.level != 1)
			{
				return;
			}
			if (this.showedUnlockSkills.Contains(data.groupId))
			{
				return;
			}
			if (!this.unlockSkills.Contains(data))
			{
				this.unlockSkills.Add(data);
				this.showedUnlockSkills.Add(data.groupId);
			}
		}

		private List<GameEventSkillBuildData> GetComposeSkills()
		{
			List<GameEventSkillBuildData> list = new List<GameEventSkillBuildData>();
			for (int i = 0; i < this.initBuildList.Count; i++)
			{
				if (this.initBuildList[i].IsComposeSkill)
				{
					list.Add(this.initBuildList[i]);
				}
			}
			return list;
		}

		public List<GameEventSkillBuildData> GetUnlockSkills()
		{
			return this.unlockSkills;
		}

		public void RemoveUnlockSkill(int index)
		{
			if (index < this.unlockSkills.Count)
			{
				this.unlockSkills.RemoveAt(index);
			}
		}

		public GameEventSkillBuildData GetSpecifiedSkill(int buildId)
		{
			if (Singleton<GameEventController>.Instance.IsHaveSkillBuild(buildId))
			{
				GameEventSkillBuildData skillBuild = Singleton<GameEventController>.Instance.GetSkillBuild(buildId);
				int skillBuildGroupMaxLevel = this.GetSkillBuildGroupMaxLevel(skillBuild.groupId);
				if (skillBuild.level == skillBuildGroupMaxLevel)
				{
					HLog.LogError(string.Format("指定获得的技能已满级，buildId={0}", buildId));
					return skillBuild;
				}
				for (int i = 0; i < this.initBuildList.Count; i++)
				{
					GameEventSkillBuildData gameEventSkillBuildData = this.initBuildList[i];
					if (gameEventSkillBuildData.id != skillBuild.id && gameEventSkillBuildData.groupId == skillBuild.groupId && gameEventSkillBuildData.level - 1 == skillBuild.level)
					{
						return gameEventSkillBuildData;
					}
				}
			}
			else
			{
				for (int j = 0; j < this.initBuildList.Count; j++)
				{
					if (this.initBuildList[j].id == buildId && this.initBuildList[j].level == 1)
					{
						return this.initBuildList[j];
					}
				}
			}
			return null;
		}

		public GameEventSkillBuildData RandomLostSkill(int tag, int seed)
		{
			List<GameEventSkillBuildData> playerSkillBuildList = Singleton<GameEventController>.Instance.PlayerData.GetPlayerSkillBuildList();
			List<GameEventSkillBuildData> list = new List<GameEventSkillBuildData>();
			for (int i = 0; i < playerSkillBuildList.Count; i++)
			{
				if (tag == 0)
				{
					list.Add(playerSkillBuildList[i]);
				}
				else if (playerSkillBuildList[i].tag == tag)
				{
					list.Add(playerSkillBuildList[i]);
				}
			}
			if (list.Count > 0)
			{
				int num = new XRandom(seed).Range(0, list.Count);
				return list[num];
			}
			return null;
		}

		public void LostSkill(GameEventSkillBuildData skillBuild)
		{
			GameEventSkillBuildData gameEventSkillBuildData = null;
			if (skillBuild.level == 1)
			{
				gameEventSkillBuildData = skillBuild;
			}
			else
			{
				for (int i = 0; i < this.initBuildList.Count; i++)
				{
					if (this.initBuildList[i].groupId == skillBuild.groupId && this.initBuildList[i].level == 1)
					{
						gameEventSkillBuildData = skillBuild;
					}
				}
			}
			for (int j = 0; j < skillBuild.config.source.Length; j++)
			{
				if (skillBuild.composeSkillList.Count > 0)
				{
					bool flag = false;
					for (int k = 0; k < skillBuild.composeSkillList.Count; k++)
					{
						int num = skillBuild.composeSkillList[k];
						if (!Singleton<GameEventController>.Instance.IsHaveSkillBuild(num))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						return;
					}
				}
				SkillBuildSourceType skillBuildSourceType = (SkillBuildSourceType)skillBuild.config.source[j];
				List<GameEventSkillBuildData> list;
				if (this.buildDic.TryGetValue(skillBuildSourceType, out list))
				{
					for (int l = 0; l < list.Count; l++)
					{
						GameEventSkillBuildData gameEventSkillBuildData2 = list[l];
						if (gameEventSkillBuildData2.groupId == skillBuild.groupId)
						{
							list.Remove(gameEventSkillBuildData2);
							break;
						}
					}
					if (gameEventSkillBuildData != null)
					{
						list.Add(gameEventSkillBuildData);
					}
				}
			}
			List<GameEventSkillBuildData> list2;
			if (this.buildDic.TryGetValue(SkillBuildSourceType.EventUpgradeSkill, out list2))
			{
				for (int m = 0; m < list2.Count; m++)
				{
					GameEventSkillBuildData gameEventSkillBuildData3 = list2[m];
					if (gameEventSkillBuildData3.groupId == skillBuild.groupId)
					{
						list2.Remove(gameEventSkillBuildData3);
						return;
					}
				}
			}
		}

		public GameEventSkillBuildData GetSkillByID(int skillBuildId)
		{
			for (int i = 0; i < this.initBuildList.Count; i++)
			{
				if (this.initBuildList[i].skillId == skillBuildId)
				{
					return this.initBuildList[i];
				}
			}
			return null;
		}

		public List<GameEventSkillBuildData> GetInitSkillBuildList()
		{
			return this.initBuildList;
		}

		public void RecordRemoveLowLevelSkill(GameEventSkillBuildData highLevelSkillBuild)
		{
			foreach (SkillBuildSourceType skillBuildSourceType in this.buildDic.Keys)
			{
				List<GameEventSkillBuildData> list = this.buildDic[skillBuildSourceType];
				GameEventSkillBuildData gameEventSkillBuildData = null;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].groupId == highLevelSkillBuild.groupId && list[i].level < highLevelSkillBuild.level)
					{
						gameEventSkillBuildData = list[i];
						break;
					}
				}
				if (gameEventSkillBuildData != null)
				{
					list.Remove(gameEventSkillBuildData);
				}
			}
		}

		public int GetLevelUpSkillSeed(int lv)
		{
			int num = lv - 1;
			if (num < this.levelUpSkillSeed.Count)
			{
				return this.levelUpSkillSeed[num];
			}
			List<int> list = this.levelUpSkillSeed;
			return list[list.Count - 1];
		}

		public int GetRefreshSkillSeed(int refreshCount, bool isAd)
		{
			if (isAd)
			{
				if (refreshCount < this.adRefreshSkillSeed.Count)
				{
					return this.adRefreshSkillSeed[refreshCount];
				}
				List<int> list = this.adRefreshSkillSeed;
				return list[list.Count - 1];
			}
			else
			{
				if (refreshCount < this.refreshSkillSeed.Count)
				{
					return this.refreshSkillSeed[refreshCount];
				}
				List<int> list2 = this.refreshSkillSeed;
				return list2[list2.Count - 1];
			}
		}

		public List<GameEventSkillBuildData> GetSkillPool(SkillBuildSourceType sourceType)
		{
			List<GameEventSkillBuildData> list;
			if (this.buildDic.TryGetValue(sourceType, out list))
			{
				return list;
			}
			GameEventSkillBuildData gameEventSkillBuildData = this.CreateFillingData();
			return new List<GameEventSkillBuildData> { gameEventSkillBuildData };
		}

		private List<GameEventSkillBuildData> initBuildList = new List<GameEventSkillBuildData>();

		private Dictionary<SkillBuildSourceType, List<GameEventSkillBuildData>> buildDic = new Dictionary<SkillBuildSourceType, List<GameEventSkillBuildData>>();

		private List<int> showedUnlockSkills = new List<int>();

		private List<GameEventSkillBuildData> unlockSkills = new List<GameEventSkillBuildData>();

		private List<int> levelUpSkillSeed = new List<int>();

		private List<int> refreshSkillSeed = new List<int>();

		private List<int> adRefreshSkillSeed = new List<int>();

		private ChapterDataModule chapterModule;

		public const int FILLING_SKILL = 100000;
	}
}
