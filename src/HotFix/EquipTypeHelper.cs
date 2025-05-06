using System;
using Server;

namespace HotFix
{
	public static class EquipTypeHelper
	{
		public static int GetEquipTypeStartIndex(EquipType equipType)
		{
			switch (equipType)
			{
			case EquipType.Weapon:
				return 0;
			case EquipType.Clothes:
				return 1;
			case EquipType.Ring:
				return 2;
			case EquipType.Accessory:
				return 4;
			default:
				HLog.LogError(string.Format("equipType:{0} not found", equipType));
				return -1;
			}
		}

		public static int GetEquipTypeMaxCount(EquipType equipType)
		{
			switch (equipType)
			{
			case EquipType.Weapon:
				return 1;
			case EquipType.Clothes:
				return 1;
			case EquipType.Ring:
				return 2;
			case EquipType.Accessory:
				return 2;
			default:
				HLog.LogError(string.Format("equipType:{0} not found", equipType));
				return -1;
			}
		}

		public const int WeaponIndex0 = 0;

		public const int ClothIndex0 = 1;

		public const int RingIndex0 = 2;

		public const int RingIndex1 = 3;

		public const int AccessoryIndex0 = 4;

		public const int AccessoryIndex1 = 5;

		public const int WeaponCount = 1;

		public const int ClothesCount = 1;

		public const int RingCount = 2;

		public const int AccessoryCount = 2;

		public const int TotalCount = 6;
	}
}
