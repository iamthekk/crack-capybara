using System;
using System.Collections.Generic;
using LocalModels;
using LocalModels.Bean;

namespace Server
{
	public static class DungeonController
	{
		public static List<CardData> GetDungeonEnemyCardDatas(LocalModelManager table, int dungeonID, int levelID)
		{
			List<CardData> list = new List<CardData>();
			Dungeon_DungeonLevel dungeonLevel = table.GetDungeon_DungeonLevelModelInstance().GetAllElements().GetDungeonLevel(dungeonID, levelID);
			if (dungeonLevel == null)
			{
				return list;
			}
			for (int i = 0; i < dungeonLevel.MemberData.Length; i++)
			{
				string text = dungeonLevel.MemberData[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					if (array.Length >= 1)
					{
						int num = int.Parse(array[0]);
						CardData cardData = new CardData(200 + i, num, MemberCamp.Enemy);
						cardData.m_posIndex = ((dungeonLevel.MemberData.Length == 1) ? MemberPos.One : ((i == 0) ? MemberPos.Two : MemberPos.Three));
						cardData.SetMemberRace(MemberRace.Hero);
						GameMember_member elementById = table.GetGameMember_memberModelInstance().GetElementById(cardData.m_memberID);
						if (elementById != null)
						{
							List<MergeAttributeData> list2 = new List<MergeAttributeData>();
							list2.AddRange(elementById.baseAttributes.GetMergeAttributeData());
							list2.AddRange(dungeonLevel.MemberAttribute.GetMergeAttributeData());
							cardData.UpdateAttribute(list2);
							cardData.UpdateSkills(elementById.skillIDs.GetListInt('|'));
							list.Add(cardData);
						}
					}
				}
			}
			return list;
		}
	}
}
