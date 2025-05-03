using System;
using System.Collections.Generic;
using LocalModels;
using LocalModels.Bean;

namespace Server
{
	public static class TowerController
	{
		public static List<CardData> GetTowerEnemyCardDatas(LocalModelManager table, int towerLevelID)
		{
			List<CardData> list = new List<CardData>();
			TowerChallenge_TowerLevel elementById = table.GetTowerChallenge_TowerLevelModelInstance().GetElementById(towerLevelID);
			if (elementById == null)
			{
				return list;
			}
			for (int i = 0; i < elementById.MemberData.Length; i++)
			{
				string text = elementById.MemberData[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					if (array.Length >= 1)
					{
						int num = int.Parse(array[0]);
						CardData cardData = new CardData(200 + i, num, MemberCamp.Enemy);
						cardData.m_posIndex = ((elementById.MemberData.Length == 1) ? MemberPos.One : ((i == 0) ? MemberPos.Two : MemberPos.Three));
						cardData.SetMemberRace(MemberRace.Hero);
						GameMember_member elementById2 = table.GetGameMember_memberModelInstance().GetElementById(cardData.m_memberID);
						if (elementById2 != null)
						{
							List<MergeAttributeData> list2 = new List<MergeAttributeData>();
							list2.AddRange(elementById2.baseAttributes.GetMergeAttributeData());
							list2.AddRange(elementById.BuffData.GetMergeAttributeData());
							cardData.UpdateAttribute(list2);
							cardData.UpdateSkills(elementById2.skillIDs.GetListInt('|'));
							cardData.m_curHp = cardData.m_memberAttributeData.GetHpMax();
							list.Add(cardData);
						}
					}
				}
			}
			return list;
		}
	}
}
