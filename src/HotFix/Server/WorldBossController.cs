using System;
using System.Collections.Generic;
using LocalModels;
using LocalModels.Bean;

namespace Server
{
	public static class WorldBossController
	{
		public static List<CardData> GetWorldBossBattleEnemy(LocalModelManager tableMgr, int configId, int monsterCfgId)
		{
			List<CardData> list = new List<CardData>();
			WorldBoss_WorldBoss worldBoss_WorldBoss = tableMgr.GetWorldBoss_WorldBoss(configId);
			if (worldBoss_WorldBoss == null)
			{
				return list;
			}
			MonsterCfg_monsterCfg monsterCfg_monsterCfg = tableMgr.GetMonsterCfg_monsterCfg(monsterCfgId);
			if (monsterCfg_monsterCfg == null)
			{
				HLog.LogError(string.Format("Table monsterCfg not found id ={0}", monsterCfgId));
				return list;
			}
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
			int num = 0;
			foreach (MemberPos memberPos in dictionary.Keys)
			{
				int num2 = dictionary[memberPos];
				GameMember_member elementById = tableMgr.GetGameMember_memberModelInstance().GetElementById(num2);
				if (elementById != null)
				{
					CardData cardData = new CardData();
					cardData.m_memberID = num2;
					cardData.m_instanceID = 200 + num;
					cardData.m_posIndex = memberPos;
					cardData.m_camp = MemberCamp.Enemy;
					cardData.SetMemberRace(MemberRace.Hero);
					MemberAttributeData memberAttributeData = new MemberAttributeData();
					memberAttributeData.MergeAttributes(elementById.baseAttributes.GetMergeAttributeData(), false);
					memberAttributeData.ConvertBaseData();
					memberAttributeData.MergeAttributes(worldBoss_WorldBoss.buffData.GetMergeAttributeData(), false);
					memberAttributeData.ConvertBaseData();
					cardData.m_memberAttributeData = memberAttributeData;
					cardData.AddSkill(elementById.skillIDs.GetListInt('|'));
					list.Add(cardData);
					num++;
				}
				else
				{
					HLog.LogError(string.Format("Table Member not found id ={0}", num2));
				}
			}
			return list;
		}
	}
}
