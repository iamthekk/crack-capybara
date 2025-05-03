using System;
using System.Collections.Generic;
using System.Linq;
using LocalModels;
using LocalModels.Bean;
using Proto.Common;

namespace Server
{
	public class AddAttributeTalentLegacy : BaseAddAttribute
	{
		public AddAttributeTalentLegacy(LocalModelManager tableManager)
			: base(tableManager)
		{
		}

		public void SetData(TalentLegacyInfoDto data)
		{
			this.talentLegacyInfo = data;
		}

		public override AddAttributeData MathAll()
		{
			AddAttributeData addAttributeData = new AddAttributeData();
			if (this.talentLegacyInfo == null)
			{
				return addAttributeData;
			}
			if (this.talentLegacyInfo.CareerList != null)
			{
				List<int> list = new List<int>();
				List<MergeAttributeData> list2 = new List<MergeAttributeData>();
				List<int> list3 = new List<int>();
				for (int i = 0; i < this.talentLegacyInfo.CareerList.Count; i++)
				{
					TalentLegacyCareerDto talentLegacyCareerDto = this.talentLegacyInfo.CareerList[i];
					if (talentLegacyCareerDto != null)
					{
						bool flag = talentLegacyCareerDto.CareerId == this.talentLegacyInfo.SelectCareer;
						if (flag)
						{
							list.AddRange(talentLegacyCareerDto.AssemblyTalentLegacyId);
						}
						if (talentLegacyCareerDto.TalentLegacys != null)
						{
							for (int j = 0; j < talentLegacyCareerDto.TalentLegacys.Count; j++)
							{
								TalentLegacyDto talentLegacyDto = talentLegacyCareerDto.TalentLegacys[j];
								if (talentLegacyDto != null && talentLegacyDto.Level > 0)
								{
									int num = talentLegacyDto.TalentLegacyId * 100 + talentLegacyDto.Level;
									TalentLegacy_talentLegacyNode elementById = this.m_tableManager.GetTalentLegacy_talentLegacyNodeModelInstance().GetElementById(talentLegacyDto.TalentLegacyId);
									TalentLegacy_talentLegacyEffect elementById2 = this.m_tableManager.GetTalentLegacy_talentLegacyEffectModelInstance().GetElementById(num);
									if (elementById2 != null)
									{
										if (flag && elementById != null && elementById.type == 4 && elementById2.skills.Length != 0)
										{
											list3.AddRange(elementById2.skills.ToList<int>());
										}
										List<MergeAttributeData> mergeAttributeData = elementById2.attributes.GetMergeAttributeData();
										if (mergeAttributeData != null && mergeAttributeData.Count > 0)
										{
											list2.AddRange(mergeAttributeData);
										}
									}
								}
							}
						}
					}
				}
				addAttributeData.m_attributeDatas = list2;
				addAttributeData.m_skillIDs.Clear();
				for (int k = 0; k < list.Count; k++)
				{
					int num2 = list[k];
					if (num2 > 0)
					{
						int legacyNodeLevel = this.GetLegacyNodeLevel(this.talentLegacyInfo.SelectCareer, num2);
						if (legacyNodeLevel > 0)
						{
							int num3 = num2 * 100 + legacyNodeLevel;
							TalentLegacy_talentLegacyEffect elementById3 = this.m_tableManager.GetTalentLegacy_talentLegacyEffectModelInstance().GetElementById(num3);
							if (elementById3 != null)
							{
								int[] skills = elementById3.skills;
								addAttributeData.m_skillIDs.AddRange(skills.ToList<int>());
							}
						}
					}
				}
				addAttributeData.m_skillIDs.AddRange(list3);
				addAttributeData.m_attributeDatas.Merge();
			}
			return addAttributeData;
		}

		private int GetLegacyNodeLevel(int selectCareer, int legacyNodeId)
		{
			if (this.talentLegacyInfo != null && this.talentLegacyInfo.CareerList != null)
			{
				for (int i = 0; i < this.talentLegacyInfo.CareerList.Count; i++)
				{
					TalentLegacyCareerDto talentLegacyCareerDto = this.talentLegacyInfo.CareerList[i];
					if (talentLegacyCareerDto != null && talentLegacyCareerDto.TalentLegacys != null)
					{
						for (int j = 0; j < talentLegacyCareerDto.TalentLegacys.Count; j++)
						{
							TalentLegacyDto talentLegacyDto = talentLegacyCareerDto.TalentLegacys[j];
							if (talentLegacyDto != null && talentLegacyDto.TalentLegacyId == legacyNodeId)
							{
								return talentLegacyDto.Level;
							}
						}
					}
				}
			}
			return 0;
		}

		private TalentLegacyInfoDto talentLegacyInfo;
	}
}
