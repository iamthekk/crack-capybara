using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix.RedPoint.Calculators
{
	public class RPTalent
	{
		public class Chest : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Chest, false))
				{
					return 0;
				}
				int num = 0;
				if (GameApp.Data.GetDataModule(DataName.ChestDataModule).HasRewardCanGet())
				{
					num++;
				}
				PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
				IList<ChestList_ChestReward> allElements = GameApp.Table.GetManager().GetChestList_ChestRewardModelInstance().GetAllElements();
				for (int i = 0; i < allElements.Count; i++)
				{
					int itemId = allElements[i].itemId;
					long itemDataCountByid = dataModule.GetItemDataCountByid((ulong)((long)itemId));
					if (itemDataCountByid > 0L)
					{
						num += (int)itemDataCountByid;
					}
				}
				return num;
			}
		}

		public class Talent : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Talent, false))
				{
					return 0;
				}
				TalentNew_talentEvolution talentNew_talentEvolution = GameApp.Table.GetManager().GetTalentNew_talentEvolution(74);
				TalentDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
				if (dataModule.talentData != null)
				{
					if (dataModule.talentData.TalentExp >= talentNew_talentEvolution.exp)
					{
						return 0;
					}
					PropDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.PropDataModule);
					foreach (TalentAttributeLevelUpData talentAttributeLevelUpData in dataModule.talentData.AttributeMap.Values)
					{
						if (talentAttributeLevelUpData.curLevel < talentAttributeLevelUpData.maxLevel)
						{
							long[] levelUpCost = talentAttributeLevelUpData.levelUpCost;
							if (levelUpCost != null && levelUpCost.Length == 2 && dataModule2.GetItemDataCountByid((ulong)levelUpCost[0]) >= levelUpCost[1])
							{
								return 1;
							}
						}
					}
					return 0;
				}
				return 0;
			}
		}

		public class TalentLegacy : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				GameApp.Data.GetDataModule(DataName.TalentDataModule);
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TalentLegacy, false))
				{
					return 0;
				}
				TalentLegacyDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
				TalentLegacyDataModule.TalentLegacyInfo talentLegacyInfo = dataModule.OnGetTalentLegacyInfo();
				IList<TalentLegacy_career> talentLegacy_careerElements = GameApp.Table.GetManager().GetTalentLegacy_careerElements();
				for (int i = 0; i < talentLegacy_careerElements.Count; i++)
				{
					int num = dataModule.OnGetTalentLegacyCareerRed(talentLegacy_careerElements[i].id, -1, false);
					if (num == 0 && talentLegacyInfo != null && talentLegacyInfo.SelectCareerId <= 0)
					{
						return 1;
					}
					if (num == 1)
					{
						return num;
					}
				}
				return 0;
			}
		}

		public class TalentLegacySkill : IRedPointRecordCalculator
		{
			public int CalcRedPoint(RedPointDataRecord record)
			{
				GameApp.Data.GetDataModule(DataName.TalentDataModule);
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TalentLegacy, false))
				{
					return 0;
				}
				TalentLegacyDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
				TalentLegacyDataModule.TalentLegacyInfo talentLegacyInfo = dataModule.OnGetTalentLegacyInfo();
				if (talentLegacyInfo != null)
				{
					int num = int.Parse(GameApp.Table.GetManager().GetGameConfig_Config(8503).Value);
					for (int i = 0; i < num; i++)
					{
						if (talentLegacyInfo.AssemblySlotCount > i && talentLegacyInfo.SelectCareerId > 0)
						{
							bool flag = false;
							IList<TalentLegacy_career> talentLegacy_careerElements = GameApp.Table.GetManager().GetTalentLegacy_careerElements();
							for (int j = 0; j < talentLegacy_careerElements.Count; j++)
							{
								if (dataModule.OnGetTalentLegacySkillRed(talentLegacyInfo.SelectCareerId, talentLegacy_careerElements[j].id, i) == 1)
								{
									flag = true;
									break;
								}
							}
							TalentLegacyDataModule.TalentLegacyCareerInfo talentLegacyCareerInfo = dataModule.OnGetTalentLegacyCareerInfo(talentLegacyInfo.SelectCareerId);
							if (flag && talentLegacyCareerInfo.AssemblyTalentLegacySkillIdList.Count > i && talentLegacyCareerInfo.AssemblyTalentLegacySkillIdList[i] == 0)
							{
								return 1;
							}
						}
					}
				}
				return 0;
			}
		}
	}
}
