using System;
using System.Collections.Generic;
using Framework;
using Proto.Pet;
using Server;

namespace HotFix
{
	public static class PetUtil
	{
		public static void GetFightPetRowIds(ulong removeTargetRowId, out List<ulong> fightPetRowIds)
		{
			PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			fightPetRowIds = dataModule.GetFightPetRowIds();
			for (int i = 0; i < fightPetRowIds.Count; i++)
			{
				if (fightPetRowIds[i] == removeTargetRowId)
				{
					fightPetRowIds[i] = 0UL;
				}
			}
		}

		public static List<ulong> GenerateNewFormationData(ulong rowId, EPetFormationType formationType, out bool isChange)
		{
			isChange = true;
			int num = 0;
			if (formationType == EPetFormationType.Fight1)
			{
				num = 0;
			}
			else if (formationType == EPetFormationType.Fight2)
			{
				num = 1;
			}
			else if (formationType == EPetFormationType.Fight3)
			{
				num = 2;
			}
			List<ulong> list = new List<ulong>(GameApp.Data.GetDataModule(DataName.PetDataModule).FormationRowIds);
			if (list[num] == rowId)
			{
				isChange = false;
				return list;
			}
			if (rowId > 0UL)
			{
				int num2 = list.IndexOf(rowId);
				if (num2 >= 0 && num != num2)
				{
					ulong num3 = list[num];
					list[num2] = num3;
					list[num] = rowId;
				}
				else
				{
					list[num] = rowId;
				}
			}
			else
			{
				list[num] = rowId;
			}
			return list;
		}

		public static string GetSortName(EPetSortType sortType)
		{
			if (sortType == EPetSortType.Quality)
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID("pet_sort_quality");
			}
			if (sortType != EPetSortType.Combat)
			{
				HLog.LogError("Unknown EPetSortType");
				return "unknown";
			}
			return Singleton<LanguageManager>.Instance.GetInfoByID("pet_sort_combat");
		}

		public static EPetSortType GetNextSortType(EPetSortType sortType)
		{
			int length = Enum.GetValues(typeof(EPetSortType)).Length;
			int num = (int)(sortType + 1);
			if (num >= length)
			{
				num = 0;
			}
			return (EPetSortType)num;
		}

		public static void PetFormationChange(ulong rowId, EPetFormationType formationType, Action<bool> callback)
		{
			bool flag;
			List<ulong> list = PetUtil.GenerateNewFormationData(rowId, formationType, out flag);
			if (!flag)
			{
				return;
			}
			NetworkUtils.Pet.PetFormatPosRequest(list, delegate(bool isOk, PetFormationResponse res)
			{
				Action<bool> callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2(isOk);
			});
		}
	}
}
