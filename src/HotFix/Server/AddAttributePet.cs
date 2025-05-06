using System;
using System.Collections.Generic;
using LocalModels;
using LocalModels.Bean;
using Proto.Common;

namespace Server
{
	public class AddAttributePet : BaseAddAttribute
	{
		public AddAttributePet(LocalModelManager tableManager)
			: base(tableManager)
		{
		}

		public void SetPetCollectionIds(List<uint> petCollectionIds)
		{
			this.m_petCollectionIds = petCollectionIds;
			if (this.m_petCollectionIds == null)
			{
				this.m_petCollectionIds = new List<uint>();
			}
		}

		public void SetData(List<PetDto> petDtos, List<uint> petCollectionIds)
		{
			this.m_petCollectionIds = petCollectionIds;
			if (this.m_petCollectionIds == null)
			{
				this.m_petCollectionIds = new List<uint>();
			}
			this.m_petDtos = petDtos;
			if (this.m_petDtos == null)
			{
				this.m_petDtos = new List<PetDto>();
			}
		}

		public override AddAttributeData MathAll()
		{
			AddAttributeData addAttributeData = new AddAttributeData();
			for (int i = 0; i < this.m_petDtos.Count; i++)
			{
				PetDto petDto = this.m_petDtos[i];
				if (petDto != null && petDto.PetType == 1U)
				{
					Pet_pet elementById = this.m_tableManager.GetPet_petModelInstance().GetElementById((int)petDto.ConfigId);
					uint num = petDto.PetLv / 5U * 5U;
					if (num > 0U)
					{
						int petLevelEffectId = elementById.GetPetLevelEffectId((int)num);
						Pet_petLevelEffect elementById2 = this.m_tableManager.GetPet_petLevelEffectModelInstance().GetElementById(petLevelEffectId);
						if (elementById2 != null)
						{
							List<MergeAttributeData> mergeAttributeData = elementById2.playerAttr.GetMergeAttributeData();
							addAttributeData.m_attributeDatas.AddRange(mergeAttributeData);
						}
					}
				}
			}
			if (this.m_petDtos != null && this.m_petDtos.Count > 0)
			{
				for (int j = 0; j < this.m_petDtos.Count; j++)
				{
					PetDto petDto2 = this.m_petDtos[j];
					if (petDto2.FormationPos > 0U)
					{
						List<MergeAttributeData> petPassiveForPlayerMergeAttributeDatas = petDto2.GetPetPassiveForPlayerMergeAttributeDatas(this.m_tableManager);
						addAttributeData.m_attributeDatas.AddRange(petPassiveForPlayerMergeAttributeDatas);
					}
				}
			}
			if (this.m_petDtos != null && this.m_petDtos.Count > 0)
			{
				for (int k = 0; k < this.m_petDtos.Count; k++)
				{
					PetDto petDto3 = this.m_petDtos[k];
					if (petDto3.FormationPos > 0U)
					{
						AddAttributeData addAttributeData2 = petDto3.MathPetAttributeData((EPetFormationType)petDto3.FormationPos, this.m_tableManager);
						addAttributeData.m_attributeDatas.AddRange(addAttributeData2.m_attributeDatas);
					}
				}
			}
			return addAttributeData;
		}

		private List<uint> m_petCollectionIds = new List<uint>();

		private List<PetDto> m_petDtos = new List<PetDto>();
	}
}
