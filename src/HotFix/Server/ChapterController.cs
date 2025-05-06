using System;
using System.Collections.Generic;
using LocalModels;
using LocalModels.Bean;

namespace Server
{
	public static class ChapterController
	{
		public static void GetChapterBattleEnemy(LocalModelManager tableMgr, int chapterID, int waveIndex, List<int> monsterCfgIDs, int battleTimes, out List<CardData> first, out List<List<CardData>> otherWave)
		{
			first = new List<CardData>();
			otherWave = new List<List<CardData>>();
			for (int i = 0; i < monsterCfgIDs.Count; i++)
			{
				int num = i;
				List<CardData> list = new List<CardData>();
				int num2 = monsterCfgIDs[i];
				MonsterCfg_monsterCfg monsterCfg_monsterCfg = tableMgr.GetMonsterCfg_monsterCfg(num2);
				Chapter_chapter elementById = tableMgr.GetChapter_chapterModelInstance().GetElementById(chapterID);
				if (monsterCfg_monsterCfg == null)
				{
					HLog.LogError(string.Format("Table Chapter_monsterCfg not found id ={0}", num2));
				}
				else if (elementById == null)
				{
					HLog.LogError(string.Format("Table Chapter_chapter not found id ={0}", chapterID));
				}
				else
				{
					Dictionary<MemberPos, int> dictionary = new Dictionary<MemberPos, int>();
					if (monsterCfg_monsterCfg.pos1 != 0)
					{
						dictionary.Add(MemberPos.One, monsterCfg_monsterCfg.pos1);
					}
					if (monsterCfg_monsterCfg.pos2 != 0)
					{
						dictionary.Add(MemberPos.Two, monsterCfg_monsterCfg.pos2);
					}
					if (monsterCfg_monsterCfg.pos3 != 0)
					{
						dictionary.Add(MemberPos.Three, monsterCfg_monsterCfg.pos3);
					}
					if (monsterCfg_monsterCfg.pos4 != 0)
					{
						dictionary.Add(MemberPos.Four, monsterCfg_monsterCfg.pos4);
					}
					if (monsterCfg_monsterCfg.pos5 != 0)
					{
						dictionary.Add(MemberPos.Five, monsterCfg_monsterCfg.pos5);
					}
					int num3 = 0;
					foreach (MemberPos memberPos in dictionary.Keys)
					{
						int num4 = dictionary[memberPos];
						GameMember_member elementById2 = tableMgr.GetGameMember_memberModelInstance().GetElementById(num4);
						if (elementById2 != null)
						{
							CardData cardData = new CardData();
							cardData.m_memberID = num4;
							cardData.m_instanceID = 200 + num * 10 + num3;
							cardData.m_posIndex = memberPos;
							cardData.m_camp = MemberCamp.Enemy;
							cardData.SetMemberRace(MemberRace.Hero);
							MemberAttributeData memberAttributeData = new MemberAttributeData();
							memberAttributeData.MergeAttributes(elementById2.baseAttributes.GetMergeAttributeData(), false);
							memberAttributeData.ConvertBaseData();
							memberAttributeData.MergeAttributes(elementById.attributes.GetMergeAttributeData(), false);
							memberAttributeData.ConvertBaseData();
							memberAttributeData.MergeAttributes(ChapterController.GetBattleTypeAttributeData(elementById, monsterCfg_monsterCfg.battleType), false);
							memberAttributeData.ConvertBaseData();
							memberAttributeData.MergeAttributes(ChapterController.GetStageAttributesUpgrade(tableMgr, waveIndex), false);
							memberAttributeData.ConvertBaseData();
							string value = tableMgr.GetGameConfig_Config(111).Value;
							if (!string.IsNullOrEmpty(value))
							{
								string[] array = value.Split('|', StringSplitOptions.None);
								int num5;
								int num6;
								if (array != null && chapterID - 1 < array.Length && int.TryParse(array[chapterID - 1], out num5) && int.TryParse(tableMgr.GetGameConfig_Config(112).Value, out num6))
								{
									List<MergeAttributeData> mergeAttributeData = tableMgr.GetGameConfig_Config(110).Value.GetMergeAttributeData();
									if (num5 > 0 && battleTimes >= num5)
									{
										int num7 = battleTimes - num5 + 1;
										if (num7 > num6)
										{
											num7 = num6;
										}
										for (int j = 0; j < num7; j++)
										{
											memberAttributeData.MergeAttributes(mergeAttributeData, false);
										}
									}
								}
							}
							cardData.m_memberAttributeData = memberAttributeData;
							cardData.AddSkill(elementById2.skillIDs.GetListInt('|'));
							list.Add(cardData);
							num3++;
						}
						else
						{
							HLog.LogError(string.Format("Table Member not found id ={0}", num4));
						}
					}
					if (list.Count > 0)
					{
						if (first.Count == 0)
						{
							first = list;
						}
						else
						{
							otherWave.Add(list);
						}
					}
				}
			}
		}

		public static List<MergeAttributeData> GetBattleTypeAttributeData(Chapter_chapter chapterTable, int battleType)
		{
			switch (battleType)
			{
			case 1:
				return chapterTable.normalBattleAttr.GetMergeAttributeData();
			case 2:
				return chapterTable.eliteBattleAttr.GetMergeAttributeData();
			case 3:
				return chapterTable.bossBattleAttr.GetMergeAttributeData();
			case 4:
				return chapterTable.npcBattleAttr.GetMergeAttributeData();
			default:
				return new List<MergeAttributeData>();
			}
		}

		public static List<MergeAttributeData> GetStageAttributesUpgrade(LocalModelManager tableMgr, int stage)
		{
			IList<Chapter_stageUpgrade> allElements = tableMgr.GetChapter_stageUpgradeModelInstance().GetAllElements();
			int num;
			int num2;
			if (stage == 0)
			{
				num = 0;
				num2 = 0;
			}
			else if (stage < allElements.Count)
			{
				Chapter_stageUpgrade chapter_stageUpgrade = allElements[stage - 1];
				num = chapter_stageUpgrade.attackUpgrade;
				num2 = chapter_stageUpgrade.hpUpgrade;
			}
			else
			{
				Chapter_stageUpgrade chapter_stageUpgrade2 = allElements[allElements.Count - 1];
				int num3 = stage - allElements.Count;
				num = (int)((double)chapter_stageUpgrade2.attackUpgrade * MathTools.Pow((double)Config.GameEvent_Power_AddAttack, (double)num3));
				num2 = (int)((double)chapter_stageUpgrade2.hpUpgrade * MathTools.Pow((double)Config.GameEvent_Power_AddHP, (double)num3));
			}
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			MergeAttributeData mergeAttributeData = new MergeAttributeData("Attack%=" + num.ToString(), null, null);
			MergeAttributeData mergeAttributeData2 = new MergeAttributeData("HPMax%=" + num2.ToString(), null, null);
			list.Add(mergeAttributeData);
			list.Add(mergeAttributeData2);
			return list;
		}
	}
}
