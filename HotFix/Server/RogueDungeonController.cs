using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using LocalModels;
using LocalModels.Bean;

namespace Server
{
	public static class RogueDungeonController
	{
		public static void GetRogueDungeonBattleEnemy(LocalModelManager tableMgr, uint stageId, uint passStage, RepeatedField<int> monsterSkills, List<int> monsterCfgIDs, out List<CardData> first, out List<List<CardData>> otherWave)
		{
			first = new List<CardData>();
			otherWave = new List<List<CardData>>();
			uint num = stageId / 1000U;
			RogueDungeon_rogueDungeon rogueDungeon_rogueDungeon = tableMgr.GetRogueDungeon_rogueDungeon((int)num);
			if (rogueDungeon_rogueDungeon == null)
			{
				HLog.LogError(string.Format("Table RogueDungeon_rogue not found id ={0}", num));
				return;
			}
			List<int> list = new List<int>();
			for (int i = 0; i < monsterSkills.Count; i++)
			{
				int num2 = monsterSkills[i];
				RogueDungeon_monsterEntry rogueDungeon_monsterEntry = tableMgr.GetRogueDungeon_monsterEntry(num2);
				if (rogueDungeon_monsterEntry != null && rogueDungeon_monsterEntry.actionType == 1)
				{
					List<int> listInt = rogueDungeon_monsterEntry.entryParam.GetListInt('|');
					list.AddRange(listInt);
				}
			}
			for (int j = 0; j < monsterCfgIDs.Count; j++)
			{
				int num3 = j;
				List<CardData> list2 = new List<CardData>();
				int num4 = monsterCfgIDs[j];
				MonsterCfg_monsterCfg monsterCfg_monsterCfg = tableMgr.GetMonsterCfg_monsterCfg(num4);
				if (monsterCfg_monsterCfg == null)
				{
					HLog.LogError(string.Format("Table RogueDungeon_monsterCfg not found id ={0}", num4));
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
					int num5 = 0;
					foreach (MemberPos memberPos in dictionary.Keys)
					{
						int num6 = dictionary[memberPos];
						GameMember_member elementById = tableMgr.GetGameMember_memberModelInstance().GetElementById(num6);
						if (elementById != null)
						{
							CardData cardData = new CardData();
							cardData.m_memberID = num6;
							cardData.m_instanceID = 200 + num3 * 10 + num5;
							cardData.m_posIndex = memberPos;
							cardData.m_camp = MemberCamp.Enemy;
							cardData.SetMemberRace(MemberRace.Hero);
							MemberAttributeData memberAttributeData = new MemberAttributeData();
							memberAttributeData.MergeAttributes(elementById.baseAttributes.GetMergeAttributeData(), false);
							memberAttributeData.ConvertBaseData();
							memberAttributeData.MergeAttributes(rogueDungeon_rogueDungeon.buffData.GetMergeAttributeData(), false);
							memberAttributeData.ConvertBaseData();
							memberAttributeData.MergeAttributes(RogueDungeonController.GetBattleTypeAttributeData(rogueDungeon_rogueDungeon, monsterCfg_monsterCfg.battleType), false);
							memberAttributeData.ConvertBaseData();
							GameConfig_Config gameConfig_Config = tableMgr.GetGameConfig_Config(3203);
							if (gameConfig_Config != null)
							{
								List<MergeAttributeData> mergeAttributeData = gameConfig_Config.Value.GetMergeAttributeData();
								int num7 = 0;
								while ((long)num7 < (long)((ulong)passStage))
								{
									memberAttributeData.MergeAttributes(mergeAttributeData, false);
									memberAttributeData.ConvertBaseData();
									num7++;
								}
							}
							memberAttributeData.MergeAttributes(RogueDungeonController.GetPassStageOtherAttributesUpgrade(tableMgr, passStage), false);
							cardData.m_memberAttributeData = memberAttributeData;
							cardData.AddSkill(elementById.skillIDs.GetListInt('|'));
							cardData.AddSkill(list);
							list2.Add(cardData);
							num5++;
						}
						else
						{
							HLog.LogError(string.Format("Table Member not found id ={0}", num6));
						}
					}
					if (list2.Count > 0)
					{
						if (first.Count == 0)
						{
							first = list2;
						}
						else
						{
							otherWave.Add(list2);
						}
					}
				}
			}
		}

		public static List<MergeAttributeData> GetBattleTypeAttributeData(RogueDungeon_rogueDungeon table, int battleType)
		{
			switch (battleType)
			{
			case 1:
				return table.normalBattleAttr.GetMergeAttributeData();
			case 2:
				return table.eliteBattleAttr.GetMergeAttributeData();
			case 3:
				return table.bossBattleAttr.GetMergeAttributeData();
			default:
				return new List<MergeAttributeData>();
			}
		}

		public static List<MergeAttributeData> GetPassStageOtherAttributesUpgrade(LocalModelManager tableMgr, uint passStage)
		{
			List<MergeAttributeData> mergeAttributeData = tableMgr.GetGameConfig_Config(3206).Value.GetMergeAttributeData();
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			int num = 0;
			while ((long)num < (long)((ulong)passStage))
			{
				list.AddRange(mergeAttributeData);
				num++;
			}
			return list;
		}
	}
}
