using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using LocalModels.Bean;
using Proto.Common;
using Server;

namespace HotFix
{
	public class PetData
	{
		public PetData UpdateData(PetDto petDto)
		{
			this.petId = petDto.GetPetConfigId(GameApp.Table.GetManager());
			this.configId = petDto.ConfigId;
			if (petDto.PetType == 1U)
			{
				this.PetItemType = ((petDto != null) ? EPetItemType.Pet : EPetItemType.Fragment);
				this.petRowId = petDto.RowId;
				this.level = (int)petDto.PetLv;
				this.isShow = (int)petDto.IsShow;
				this.formationType = (EPetFormationType)((petDto != null) ? petDto.FormationPos : 0U);
				this.petCount = (int)petDto.PetCount;
				this.trainingAttributeIds = petDto.TrainingAttributeIds.ToList<int>();
				this.trainingAttributeValues = petDto.TrainingAttributeValues.ToList<int>();
				this.trainingAttributeIdsTemp = petDto.TrainingAttributeIdsTemp.ToList<int>();
				this.trainingAttributeValuesTemp = petDto.TrainingAttributeValuesTemp.ToList<int>();
				this.trainingAttributeLockIds = petDto.TrainLock.ToList<int>();
			}
			else
			{
				this.PetItemType = ((this.petRowId > 0UL) ? EPetItemType.Pet : EPetItemType.Fragment);
				this.fragmentRowId = petDto.RowId;
				this.fragmentCount = (int)petDto.PetCount;
			}
			return this;
		}

		public PetData AddData(PetDto petDto)
		{
			this.petId = petDto.GetPetConfigId(GameApp.Table.GetManager());
			if (petDto.PetType == 1U)
			{
				this.UpdateData(petDto);
			}
			else if (this.fragmentRowId > 0UL)
			{
				this.UpdateData(petDto);
			}
			else
			{
				this.fragmentCount = (int)petDto.PetCount;
			}
			return this;
		}

		public PetData UpdateLocalData()
		{
			Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(this.petId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("configId: {0} can't find in Pet Config, please check!", this.petId));
				return this;
			}
			this.memberId = elementById.memberId;
			this.nameId = elementById.nameID;
			this.quality = elementById.quality;
			this.levelEffectID = elementById.levelEffectID;
			int num = elementById.battleSkill;
			PetSkillData petSkillData = new PetSkillData().Init(num, this.petId, this.level);
			this.battleSkill = petSkillData;
			PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this.combat = dataModule.MathCombatData(this);
			return this;
		}

		public Pet_pet CfgPetData
		{
			get
			{
				return GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(this.petId);
			}
		}

		public bool IsCanLevelUp(int petLevel)
		{
			Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(this.petId);
			if (elementById.IsFullMaxLevel(petLevel))
			{
				return false;
			}
			Pet_petLevel elementById2 = GameApp.Table.GetManager().GetPet_petLevelModelInstance().GetElementById(elementById.GetPetLevelId(this.level));
			return GameApp.Data.GetDataModule(DataName.TalentDataModule).TalentStage >= elementById2.talentNeed;
		}

		public bool HasEnoughLevelUpCost()
		{
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			bool flag = true;
			List<ItemData> levelUpCosts = this.GetLevelUpCosts(this.petId, this.level);
			for (int i = 0; i < levelUpCosts.Count; i++)
			{
				ItemData itemData = levelUpCosts[i];
				if (itemData != null)
				{
					long itemDataCountByid = dataModule.GetItemDataCountByid((ulong)((long)itemData.ID));
					if (itemData.TotalCount > itemDataCountByid)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		public List<ItemData> GetLevelUpCosts(int petConfigId, int level)
		{
			new List<ItemData>();
			return GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(petConfigId)
				.GetLevelUpCosts(level);
		}

		public bool ConditionLevelUpOk()
		{
			return this.IsCanLevelUp(this.level) && this.HasEnoughLevelUpCost();
		}

		public bool ConditionInDeployOk()
		{
			PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			return this.petRowId > 0UL && dataModule.IsDeploy(this.petRowId);
		}

		public int GetPetEntryId(int index, bool isTemp)
		{
			if (isTemp)
			{
				if (index < 0 || index >= this.trainingAttributeIdsTemp.Count)
				{
					return 0;
				}
				return this.trainingAttributeIdsTemp[index];
			}
			else
			{
				if (index < 0 || index >= this.trainingAttributeIds.Count)
				{
					return 0;
				}
				return this.trainingAttributeIds[index];
			}
		}

		public int GetPetEntryValue(int index, bool isTemp)
		{
			if (isTemp)
			{
				if (index < 0 || index >= this.trainingAttributeValuesTemp.Count)
				{
					return 0;
				}
				return this.trainingAttributeValuesTemp[index];
			}
			else
			{
				if (index < 0 || index >= this.trainingAttributeValues.Count)
				{
					return 0;
				}
				return this.trainingAttributeValues[index];
			}
		}

		public int GetPetLevelEffectId(int level)
		{
			int num = level / 5 * 5;
			return this.levelEffectID * 1000 + num;
		}

		public int GetPetLevelId(int lv)
		{
			return this.quality * 10000 + lv;
		}

		public MemberAttributeData GetMemberAttributeData()
		{
			AddAttributeData addAttributeData = this.ToPetDto().MathPetAttributeData(this.formationType, GameApp.Table.GetManager());
			MemberAttributeData memberAttributeData = new MemberAttributeData();
			memberAttributeData.MergeAttributes(addAttributeData.m_attributeDatas, false);
			return memberAttributeData;
		}

		public PetDto ToPetDto()
		{
			PetDto petDto = new PetDto();
			petDto.ConfigId = (uint)this.petId;
			petDto.RowId = this.petRowId;
			petDto.PetType = 1U;
			petDto.PetCount = 1U;
			petDto.PetLv = (uint)this.level;
			petDto.FormationPos = (uint)this.formationType;
			petDto.IsShow = (uint)this.isShow;
			petDto.TrainingAttributeIds.AddRange(this.trainingAttributeIds);
			petDto.TrainingAttributeValues.AddRange(this.trainingAttributeValues);
			petDto.TrainingAttributeIdsTemp.AddRange(this.trainingAttributeIdsTemp);
			petDto.TrainingAttributeValuesTemp.AddRange(this.trainingAttributeValuesTemp);
			return petDto;
		}

		public static PetData CreateFakeData(int configId, int lv, int petStar)
		{
			Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(configId);
			return new PetData
			{
				level = lv,
				petId = configId,
				formationType = EPetFormationType.Idle,
				quality = elementById.quality,
				nameId = elementById.nameID
			};
		}

		public bool HaveNewPassive
		{
			get
			{
				return this.trainingAttributeIdsTemp.Count > 0;
			}
		}

		public ulong petRowId;

		public ulong fragmentRowId;

		public EPetItemType PetItemType;

		public int level = 1;

		public EPetFormationType formationType;

		public int isShow;

		public int petCount;

		public int fragmentCount;

		public string nameId;

		public int memberId;

		public int quality;

		public PetSkillData battleSkill = new PetSkillData();

		public double combat;

		public int petId;

		public uint configId;

		public int levelEffectID;

		public List<int> trainingAttributeIds = new List<int>();

		public List<int> trainingAttributeValues = new List<int>();

		public List<int> trainingAttributeIdsTemp = new List<int>();

		public List<int> trainingAttributeValuesTemp = new List<int>();

		public List<int> trainingAttributeLockIds = new List<int>();
	}
}
