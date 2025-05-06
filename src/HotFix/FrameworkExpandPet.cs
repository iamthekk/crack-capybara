using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public static class FrameworkExpandPet
	{
		public static bool IsFullMaxLevel(this Pet_pet petPet, int level)
		{
			int petLevelId = petPet.GetPetLevelId(level);
			Pet_petLevel elementById = GameApp.Table.GetManager().GetPet_petLevelModelInstance().GetElementById(petLevelId);
			return elementById == null || elementById.nextID <= 0;
		}

		public static List<ItemData> GetLevelUpCosts(this Pet_pet petPet, int level)
		{
			int num = int.Parse(petPet.toFragment.Split(",", StringSplitOptions.None)[0]);
			List<ItemData> list = new List<ItemData>();
			int petLevelId = petPet.GetPetLevelId(level);
			Pet_petLevel elementById = GameApp.Table.GetManager().GetPet_petLevelModelInstance().GetElementById(petLevelId);
			if (elementById == null)
			{
				return list;
			}
			if (elementById.levelupFragment > 0)
			{
				ItemData itemData = new ItemData(num, (long)elementById.levelupFragment);
				list.Add(itemData);
			}
			for (int i = 0; i < elementById.levelupCost.Length; i++)
			{
				string text = elementById.levelupCost[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					int num2;
					int num3;
					if (array.Length == 2 && int.TryParse(array[0], out num2) && int.TryParse(array[1], out num3))
					{
						ItemData itemData2 = new ItemData(num2, (long)num3);
						list.Add(itemData2);
					}
				}
			}
			return list;
		}

		public static List<PetData> SortByType(this List<PetData> list, EPetSortType sortType)
		{
			return GameApp.Data.GetDataModule(DataName.PetDataModule).SortPetList(list, sortType);
		}
	}
}
