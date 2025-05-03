using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using LocalModels;
using LocalModels.Bean;
using Proto.Common;

namespace Server
{
	public class AddAttributeTalent : BaseAddAttribute
	{
		public AddAttributeTalent(LocalModelManager tableManager)
			: base(tableManager)
		{
		}

		public void SetData(TalentsInfo talentsInfo)
		{
			this.talentsInfo = talentsInfo;
		}

		public override AddAttributeData MathAll()
		{
			AddAttributeData addAttributeData = new AddAttributeData();
			if (this.talentsInfo == null)
			{
				return addAttributeData;
			}
			int step = (int)this.talentsInfo.Step;
			int expProcess = (int)this.talentsInfo.ExpProcess;
			MapField<string, uint> attributesMap = this.talentsInfo.AttributesMap;
			IList<TalentNew_talentEvolution> allElements = this.m_tableManager.GetTalentNew_talentEvolutionModelInstance().GetAllElements();
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			for (int i = 0; i < allElements.Count; i++)
			{
				TalentNew_talentEvolution talentNew_talentEvolution = allElements[i];
				if (step > talentNew_talentEvolution.id)
				{
					List<MergeAttributeData> mergeAttributeData = talentNew_talentEvolution.attributeGroup.GetMergeAttributeData();
					List<MergeAttributeData> mergeAttributeData2 = talentNew_talentEvolution.evolutionAttributes.GetMergeAttributeData();
					for (int j = 0; j < mergeAttributeData.Count; j++)
					{
						mergeAttributeData[j].Multiply(talentNew_talentEvolution.levelLimit);
					}
					list.AddRange(mergeAttributeData);
					list.AddRange(mergeAttributeData2);
				}
				else if (step == talentNew_talentEvolution.id)
				{
					List<MergeAttributeData> mergeAttributeData3 = talentNew_talentEvolution.attributeGroup.GetMergeAttributeData();
					for (int k = 0; k < mergeAttributeData3.Count; k++)
					{
						int num = (int)(attributesMap.ContainsKey(mergeAttributeData3[k].Header) ? attributesMap[mergeAttributeData3[k].Header] : 0U);
						mergeAttributeData3[k].Multiply(num);
					}
					list.AddRange(mergeAttributeData3);
					break;
				}
			}
			IList<TalentNew_talent> allElements2 = this.m_tableManager.GetTalentNew_talentModelInstance().GetAllElements();
			for (int l = 0; l < allElements2.Count; l++)
			{
				TalentNew_talent talentNew_talent = allElements2[l];
				if (expProcess < talentNew_talent.talentLevel)
				{
					break;
				}
				if (talentNew_talent.rewardType == 2 || talentNew_talent.rewardType == 3)
				{
					List<MergeAttributeData> mergeAttributeData4 = talentNew_talent.reward.GetMergeAttributeData();
					list.AddRange(mergeAttributeData4);
				}
			}
			addAttributeData.m_attributeDatas = list.Merge();
			return addAttributeData;
		}

		private TalentsInfo talentsInfo;
	}
}
