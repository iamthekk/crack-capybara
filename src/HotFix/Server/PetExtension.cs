using System;
using System.Collections.Generic;
using LocalModels;
using LocalModels.Bean;
using Proto.Common;

namespace Server
{
	public static class PetExtension
	{
		public static CardData PetDto2CardData(this PetDto petDto, EPetFormationType formationType, MemberCamp camp, LocalModelManager manager)
		{
			int petConfigId = petDto.GetPetConfigId(manager);
			Pet_pet elementById = manager.GetPet_petModelInstance().GetElementById(petConfigId);
			AddAttributeData addAttributeData = petDto.MathPetAttributeData(formationType, manager);
			MemberAttributeData memberAttributeData = new MemberAttributeData();
			memberAttributeData.MergeAttributes(addAttributeData.m_attributeDatas, false);
			CardData cardData = new CardData();
			cardData.m_rowID = (int)petDto.RowId;
			cardData.m_memberID = elementById.memberId;
			cardData.m_camp = camp;
			cardData.m_memberAttributeData = memberAttributeData;
			cardData.SetMemberRace(MemberRace.Pet);
			cardData.AddSkill(addAttributeData.m_skillIDs);
			cardData.m_petData = new SPetData().Init(petDto, manager);
			return cardData;
		}

		public static void PetFilter(this List<PetDto> petDto, LocalModelManager Table, MemberCamp camp, out List<CardData> petFightCardDataList)
		{
			petFightCardDataList = new List<CardData>();
			int num = 0;
			int i;
			int j;
			for (i = 1; i <= 3; i = j + 1)
			{
				PetDto petDto2 = petDto.Find((PetDto p) => (ulong)p.FormationPos == (ulong)((long)i));
				if (petDto2 != null)
				{
					num++;
					CardData cardData = petDto2.PetDto2CardData((EPetFormationType)i, camp, Table);
					cardData.SetMemberRace(MemberRace.Pet);
					cardData.m_instanceID = ((camp == MemberCamp.Friendly) ? (150 + num) : (250 + num));
					cardData.m_posIndex = (MemberPos)num;
					petFightCardDataList.Add(cardData);
					BattleLogHelper.LogCardData(cardData, "[PetData]");
				}
				j = i;
			}
		}

		public static int GetPetConfigId(this PetDto petDto, LocalModelManager manager)
		{
			Item_Item elementById = manager.GetItem_ItemModelInstance().GetElementById((int)petDto.ConfigId);
			if (elementById == null)
			{
				return 0;
			}
			int num;
			if (petDto.PetType == 2U)
			{
				num = int.Parse(elementById.itemTypeParam[0]);
			}
			else
			{
				num = int.Parse(elementById.itemTypeParam[0]);
			}
			return num;
		}

		public static int GetPetLevelEffectId(this Pet_pet petPet, int level)
		{
			int num = level / 5 * 5;
			if (num == 0)
			{
				return 0;
			}
			return petPet.levelEffectID * 1000 + num;
		}

		public static int GetPetLevelId(this Pet_pet petPet, int level)
		{
			return petPet.quality * 10000 + level;
		}

		public static int GetPetStarId(this Pet_pet petPet, int petStar)
		{
			return petPet.quality * 10000 + petStar;
		}

		public static List<int> GetPetBattleSkillIds(this Pet_pet petPet, int level, LocalModelManager manager)
		{
			List<int> list = new List<int>();
			int battleSkill = petPet.battleSkill;
			int petLevelEffectId = petPet.GetPetLevelEffectId(level);
			Pet_petLevelEffect pet_petLevelEffect = ((petLevelEffectId > 0) ? manager.GetPet_petLevelEffectModelInstance().GetElementById(petLevelEffectId) : null);
			Pet_petSkill elementById = manager.GetPet_petSkillModelInstance().GetElementById(battleSkill);
			int num = ((pet_petLevelEffect == null) ? 1 : pet_petLevelEffect.petSkillLevel);
			int num2 = 0;
			if (elementById != null && elementById.level.Length != 0)
			{
				for (int i = elementById.level.Length - 1; i >= 0; i--)
				{
					if (num - 1 >= i)
					{
						num2 = elementById.level[i];
						break;
					}
				}
			}
			if (num2 > 0)
			{
				list.Add(num2);
			}
			return list;
		}

		public static List<MergeAttributeData> GetPetLevelMergeAttributeData(this Pet_pet petPet, int level, LocalModelManager manager)
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			int petLevelId = petPet.GetPetLevelId(level);
			Pet_petLevel elementById = manager.GetPet_petLevelModelInstance().GetElementById(petLevelId);
			if (elementById != null)
			{
				list.AddRange(elementById.upgradeAttributes.GetMergeAttributeData());
			}
			return list;
		}

		public static List<int> GetPetPassiveSkillIds(this PetDto petDto, LocalModelManager manager)
		{
			List<int> list = new List<int>();
			if (petDto != null)
			{
				for (int i = 0; i < petDto.TrainingAttributeIds.Count; i++)
				{
					int num = petDto.TrainingAttributeIds[i];
					int num2 = petDto.TrainingAttributeValues[i];
					Pet_PetEntry elementById = manager.GetPet_PetEntryModelInstance().GetElementById(num);
					if (elementById.entryType == 2)
					{
						int num3 = elementById.attrRange[1];
					}
					if (elementById.actionType == 2)
					{
						list.Add(int.Parse(elementById.action));
					}
				}
			}
			return list;
		}

		public static List<MergeAttributeData> GetPetPassiveForPlayerMergeAttributeDatas(this PetDto petDto, LocalModelManager manager)
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			if (petDto != null)
			{
				for (int i = 0; i < petDto.TrainingAttributeIds.Count; i++)
				{
					int num = petDto.TrainingAttributeIds[i];
					int num2 = petDto.TrainingAttributeValues[i];
					Pet_PetEntry elementById = manager.GetPet_PetEntryModelInstance().GetElementById(num);
					if (elementById.entryType == 2 || elementById.entryType == 3)
					{
						num2 = elementById.attrRange[1];
					}
					if (elementById.actionType == 1)
					{
						MergeAttributeData mergeAttribute = elementById.action.GetMergeAttribute();
						mergeAttribute.Multiply((float)num2 * 1f / (float)elementById.attrRange[1]);
						list.Add(mergeAttribute);
					}
				}
			}
			return list;
		}

		public static AddAttributeData MathPetAttributeData(this PetDto petDto, EPetFormationType formationType, LocalModelManager manager)
		{
			AddAttributeData addAttributeData = new AddAttributeData();
			if (petDto == null)
			{
				return addAttributeData;
			}
			int petLv = (int)petDto.PetLv;
			Pet_pet elementById = manager.GetPet_petModelInstance().GetElementById(petDto.GetPetConfigId(manager));
			List<MergeAttributeData> petLevelMergeAttributeData = elementById.GetPetLevelMergeAttributeData(petLv, manager);
			FP fp = 1f;
			List<int> list = new List<int>();
			if (petDto.TrainingAttributeIds != null)
			{
				for (int i = 0; i < petDto.TrainingAttributeIds.Count; i++)
				{
					int num = petDto.TrainingAttributeIds[i];
					int num2 = petDto.TrainingAttributeValues[i];
					Pet_PetEntry elementById2 = manager.GetPet_PetEntryModelInstance().GetElementById(num);
					if (elementById2.entryType == 2 || elementById2.entryType == 3)
					{
						num2 = elementById2.attrRange[1];
					}
					if (elementById2.actionType == 3)
					{
						fp += 0.01f * (float)int.Parse(elementById2.action) * (float)num2 / (float)elementById2.attrRange[1];
					}
					else if (elementById2.actionType == 2)
					{
						list.Add(int.Parse(elementById2.action));
					}
				}
			}
			MemberAttributeData memberAttributeData = new MemberAttributeData();
			memberAttributeData.MergeAttributes(petLevelMergeAttributeData, false);
			for (int j = petLevelMergeAttributeData.Count - 1; j >= 0; j--)
			{
				string header = petLevelMergeAttributeData[j].Header;
				if (header.Equals("Attack") || header.Equals("Attack%") || header.Equals("GlobalAttack%") || header.Equals("Defence") || header.Equals("Defence%") || header.Equals("GlobalDefence%") || header.Equals("HPMax") || header.Equals("HPMax%") || header.Equals("GlobalHPMax%"))
				{
					petLevelMergeAttributeData.RemoveAt(j);
				}
			}
			petLevelMergeAttributeData.Add(new MergeAttributeData(string.Format("{0}={1}", "Attack", memberAttributeData.GetAttack() * fp), null, null));
			petLevelMergeAttributeData.Add(new MergeAttributeData(string.Format("{0}={1}", "Defence", memberAttributeData.GetDefence() * fp), null, null));
			petLevelMergeAttributeData.Add(new MergeAttributeData(string.Format("{0}={1}", "HPMax", memberAttributeData.GetHpMax() * fp), null, null));
			List<int> list2 = new List<int>();
			list2 = elementById.GetPetBattleSkillIds((int)petDto.PetLv, manager);
			list2.AddRange(list);
			addAttributeData.m_attributeDatas = petLevelMergeAttributeData.Merge();
			addAttributeData.m_skillIDs = list2;
			return addAttributeData;
		}
	}
}
