using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using LocalModels;
using LocalModels.Bean;
using Proto.Common;

namespace Server
{
	public class AddAttributeEquip : BaseAddAttribute
	{
		public AddAttributeEquip(LocalModelManager tableManager)
			: base(tableManager)
		{
		}

		public void SetData(RepeatedField<EquipmentDto> equipmentDtos)
		{
			this.m_equipmentDtos = equipmentDtos;
		}

		public override AddAttributeData MathAll()
		{
			AddAttributeData addAttributeData = new AddAttributeData();
			if (this.m_equipmentDtos == null)
			{
				return addAttributeData;
			}
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < this.m_equipmentDtos.Count; i++)
			{
				EquipmentDto equipmentDto = this.m_equipmentDtos[i];
				if (equipmentDto != null)
				{
					int equipId = (int)equipmentDto.EquipId;
					int level = (int)equipmentDto.Level;
					int evolution = (int)equipmentDto.Evolution;
					Equip_equip elementById = this.m_tableManager.GetEquip_equipModelInstance().GetElementById(equipId);
					Equip_equipSkill elementById2 = this.m_tableManager.GetEquip_equipSkillModelInstance().GetElementById(elementById.tagID);
					list.AddRange(elementById.GetMergeAttributeData(this.m_tableManager, level, evolution));
					int composeId = elementById.composeId;
					int[] qualitySkill = elementById2.qualitySkill;
					int[] qualityUnlock = elementById2.qualityUnlock;
					for (int j = qualityUnlock.Length - 1; j >= 0; j--)
					{
						int num = qualityUnlock[j];
						if (qualitySkill.Length >= j && composeId >= num)
						{
							int num2 = qualitySkill[j];
							Equip_skill elementById3 = this.m_tableManager.GetEquip_skillModelInstance().GetElementById(num2);
							if (elementById3 != null)
							{
								string action = elementById3.action;
								EquipAction equipAction;
								if (string.IsNullOrEmpty(action))
								{
									equipAction = new EquipAction();
								}
								else
								{
									equipAction = JsonManager.ToObject<EquipAction>(action);
								}
								if (equipAction.rageSkill > 0)
								{
									list2.Add(equipAction.rageSkill);
								}
								if (equipAction.skillIds != null && equipAction.skillIds.Length != 0)
								{
									list2.AddRange(equipAction.skillIds);
								}
								list.AddRange(equipAction.GetMergeAttributeData());
								break;
							}
							HLog.LogError(string.Format("skillId:{0} is not exist, please check Equip.xls skill sheet", num2));
						}
					}
				}
			}
			addAttributeData.m_attributeDatas = list.Merge();
			addAttributeData.m_skillIDs = list2;
			return addAttributeData;
		}

		public RepeatedField<EquipmentDto> m_equipmentDtos;
	}
}
