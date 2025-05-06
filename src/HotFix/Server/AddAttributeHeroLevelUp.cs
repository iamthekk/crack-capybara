using System;
using System.Collections.Generic;
using LocalModels;
using LocalModels.Bean;

namespace Server
{
	public class AddAttributeHeroLevelUp : BaseAddAttribute
	{
		public AddAttributeHeroLevelUp(LocalModelManager tableManager)
			: base(tableManager)
		{
		}

		public void SetData(int level)
		{
			this.m_level = level;
		}

		public override AddAttributeData MathAll()
		{
			AddAttributeData addAttributeData = new AddAttributeData();
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			IList<HeroLevelup_HeroLevelup> allElements = this.m_tableManager.GetHeroLevelup_HeroLevelupModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				HeroLevelup_HeroLevelup heroLevelup_HeroLevelup = allElements[i];
				if (heroLevelup_HeroLevelup.ID == this.m_level)
				{
					break;
				}
				for (int j = 0; j < heroLevelup_HeroLevelup.levelUpRewards.Length; j++)
				{
					string text = heroLevelup_HeroLevelup.levelUpRewards[j];
					if (text != null)
					{
						MergeAttributeData mergeAttributeData = new MergeAttributeData(text, null, null);
						list.Add(mergeAttributeData);
					}
				}
			}
			addAttributeData.m_attributeDatas = list.Merge();
			return addAttributeData;
		}

		public int m_level;
	}
}
