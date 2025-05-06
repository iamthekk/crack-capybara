using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class PetSkillData
	{
		public PetSkillData Init(int skillGroupId, int petId, int petLv)
		{
			if (this.isInit && this.petLevel.Equals(petLv))
			{
				return this;
			}
			this.Reset();
			this.petId = petId;
			this.isLock = true;
			this.skillGroupId = skillGroupId;
			this.petLevel = petLv;
			Pet_petSkill elementById = GameApp.Table.GetManager().GetPet_petSkillModelInstance().GetElementById(skillGroupId);
			int[] unlockLevel = elementById.unlockLevel;
			if (unlockLevel != null && unlockLevel.Length != 0)
			{
				for (int i = unlockLevel.Length - 1; i >= 0; i--)
				{
					int num = unlockLevel[i];
					if (this.petLevel >= num)
					{
						this.curIndex = i;
						this.skillLevel = i + 1;
						this.isLock = false;
						this.curSkillId = elementById.level[i];
						break;
					}
				}
			}
			this.isInit = true;
			return this;
		}

		private void Reset()
		{
			this.curIndex = 0;
			this.skillGroupId = 0;
			this.curSkillId = 0;
			this.petLevel = 0;
			this.skillLevel = 0;
			this.isLock = true;
			this.isInit = false;
		}

		public int petId;

		public int skillGroupId;

		public int curSkillId;

		public int skillLevel;

		public int curIndex;

		public int petLevel;

		public bool isLock = true;

		private bool isInit;
	}
}
