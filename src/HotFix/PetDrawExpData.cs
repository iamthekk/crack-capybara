using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class PetDrawExpData
	{
		public int DrawLevel { get; private set; }

		public int LevelCurExp { get; private set; }

		public int LevelMaxExp { get; private set; }

		public bool IsMaxLevel { get; private set; }

		public void Clear()
		{
			this.DrawLevel = 1;
			this.LevelCurExp = 0;
			this.LevelMaxExp = 1;
			this.IsMaxLevel = false;
		}

		public void SetData(int level, int exp)
		{
			if (level <= 0)
			{
				level = 1;
			}
			int drawLevel = this.DrawLevel;
			this.DrawLevel = level;
			this.LevelCurExp = exp;
			Pet_petSummon elementById = GameApp.Table.GetManager().GetPet_petSummonModelInstance().GetElementById(level);
			bool elementById2 = GameApp.Table.GetManager().GetPet_petSummonModelInstance().GetElementById(level + 1) != null;
			this.LevelMaxExp = elementById.exp;
			if (!elementById2)
			{
				this.IsMaxLevel = true;
			}
			if (this.DrawLevel > drawLevel)
			{
				Singleton<GameFunctionController>.Instance.CheckNewFunctionOpenSpecial(FunctionUnlockType.PetDrawLevel, true);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_PetDataModule_UpdatePetDrawData, null);
			}
		}
	}
}
