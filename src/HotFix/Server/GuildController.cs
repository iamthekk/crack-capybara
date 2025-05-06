using System;
using System.Collections.Generic;
using LocalModels;
using LocalModels.Bean;

namespace Server
{
	public static class GuildController
	{
		public static List<CardData> GetGuildBossEnemyCardDatas(LocalModelManager table, int guildBossStepID, long bossHp)
		{
			List<CardData> list = new List<CardData>();
			GuildBOSS_guildBossStep guildBOSS_guildBossStep = table.GetGuildBOSS_guildBossStep(guildBossStepID);
			if (guildBOSS_guildBossStep == null)
			{
				return list;
			}
			GuildBOSS_guildBoss guildBOSS_guildBoss = table.GetGuildBOSS_guildBoss(guildBOSS_guildBossStep.BossId);
			if (guildBOSS_guildBoss == null)
			{
				return list;
			}
			CardData cardData = new CardData(200, guildBOSS_guildBoss.BossId, MemberCamp.Enemy);
			cardData.m_posIndex = MemberPos.One;
			cardData.SetMemberRace(MemberRace.Hero);
			GameMember_member elementById = table.GetGameMember_memberModelInstance().GetElementById(cardData.m_memberID);
			if (elementById == null)
			{
				return list;
			}
			MemberAttributeData memberAttributeData = new MemberAttributeData();
			memberAttributeData.MergeAttributes(elementById.baseAttributes.GetMergeAttributeData(), false);
			memberAttributeData.ConvertBaseData();
			memberAttributeData.MergeAttributes(guildBOSS_guildBossStep.BossAttributes.GetMergeAttributeData(), false);
			memberAttributeData.ConvertBaseData();
			new List<MergeAttributeData>().AddRange(elementById.baseAttributes.GetMergeAttributeData());
			cardData.m_memberAttributeData = memberAttributeData;
			cardData.AddSkill(elementById.skillIDs.GetListInt('|'));
			cardData.m_curHp = ((bossHp == -1L) ? cardData.m_memberAttributeData.GetHpMax() : bossHp);
			list.Add(cardData);
			return list;
		}
	}
}
