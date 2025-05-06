using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class MemberAnimationName
	{
		public static bool IsLoop(string aniName)
		{
			return aniName.Equals("Idle") || aniName.Equals("Run") || aniName.Equals("FastRun") || aniName.Equals("Walk") || aniName.Equals("Win") || aniName.Equals("Idle") || aniName.Equals("Idle") || aniName.Equals("Open_Idle") || aniName.Equals("Idle_Water") || aniName.Equals("Battle/Cast_Loop") || aniName.Equals("Other/Fish_Loop") || aniName.Equals("Idle01") || aniName.Equals("Idle02") || aniName.Equals("Idle03") || aniName.Equals("Store/Idle_Bow") || aniName.Equals("Store/Idle_Hammer") || aniName.Equals("Store/Idle_Magic") || aniName.Equals("Store/Idle_Sword") || aniName.Equals("Store/Idle_Spear") || aniName.Equals("Idle_Water");
		}

		public static string GetIdleAnimationName(int weaponId)
		{
			return MemberAnimationName.GetIdleAnimationName(false, weaponId);
		}

		public static string GetSelfIdleAnimationName()
		{
			return MemberAnimationName.GetIdleAnimationName(true, 0);
		}

		private static string GetIdleAnimationName(bool useLocalSelfWeapon, int weaponId)
		{
			string text = "Idle";
			if (useLocalSelfWeapon)
			{
				weaponId = GameApp.Data.GetDataModule(DataName.EquipDataModule).GetWeaponId();
			}
			if (weaponId > 0)
			{
				Equip_equip equip_equip = GameApp.Table.GetManager().GetEquip_equip(weaponId);
				if (equip_equip != null)
				{
					switch (equip_equip.subType)
					{
					case 101:
						text = "Store/Idle_Bow";
						break;
					case 102:
						text = "Store/Idle_Hammer";
						break;
					case 103:
						text = "Store/Idle_Magic";
						break;
					case 104:
						text = "Store/Idle_Sword";
						break;
					case 105:
						text = "Store/Idle_Spear";
						break;
					}
				}
			}
			return text;
		}

		public static string GetShowAnimationName(int weaponId)
		{
			return MemberAnimationName.GetShowAnimationName(weaponId, false);
		}

		public static string GetShowAnimationName(int weaponId, bool useLocalSelfWeapon)
		{
			string text = "";
			if (useLocalSelfWeapon)
			{
				weaponId = GameApp.Data.GetDataModule(DataName.EquipDataModule).GetWeaponId();
			}
			if (weaponId > 0)
			{
				Equip_equip equip_equip = GameApp.Table.GetManager().GetEquip_equip(weaponId);
				if (equip_equip != null)
				{
					switch (equip_equip.subType)
					{
					case 101:
						text = "Store/Show_Bow";
						break;
					case 102:
						text = "Store/Show_Hammer";
						break;
					case 103:
						text = "Store/Show_Magic";
						break;
					case 104:
						text = "Store/Show_Sword";
						break;
					case 105:
						text = "Store/Show_Spear";
						break;
					}
				}
			}
			return text;
		}

		public const string Idle = "Idle";

		public const string Idle_Bow = "Store/Idle_Bow";

		public const string Idle_Hammer = "Store/Idle_Hammer";

		public const string Idle_Magic = "Store/Idle_Magic";

		public const string Idle_Sword = "Store/Idle_Sword";

		public const string Idle_Spear = "Store/Idle_Spear";

		public const string Show_Bow = "Store/Show_Bow";

		public const string Show_Hammer = "Store/Show_Hammer";

		public const string Show_Magic = "Store/Show_Magic";

		public const string Show_Sword = "Store/Show_Sword";

		public const string Show_Spear = "Store/Show_Spear";

		public const string Idle01 = "Idle01";

		public const string Idle02 = "Idle02";

		public const string Idle03 = "Idle03";

		public const string Idle_Water = "Idle_Water";

		public const string Walk = "Walk";

		public const string Run = "Run";

		public const string FastRun = "FastRun";

		public const string Death = "Death";

		public const string IdleToRun = "Idle_to_Run";

		public const string RunToIdle = "Run_to_Idle";

		public const string Skill = "Skill";

		public const string Win = "Win";

		public const string Appear = "Appear";

		public const string Appear_1 = "Appear_1";

		public const string Appear_2 = "Appear_2";

		public const string Appear_3 = "Appear_3";

		public const string Leave = "Leave";

		public const string Rowing = "Battle/Cast_Loop";

		public const string Battle_Cast = "Battle/Cast";

		public const string Fishing_Begin = "Other/Fish_Begin";

		public const string Fishing_End_SmallFish = "Other/Fish_End01";

		public const string Fishing_End_BigFish = "Other/Fish_End02";

		public const string Fishing_Loop = "Other/Fish_Loop";

		public const string Attack_Begin = "Skill02_begin";

		public const string Attack_Loop = "Skill02_loop";

		public const string Attack_end = "Skill02_end";

		public const string Failure = "Idle";

		public const string Fitness = "Fitness";

		public const string BoxIdle = "Idle";

		public const string BoxEnter = "Appear";

		public const string BoxOpen = "Open";

		public const string BoxOpen2 = "Open2";

		public const string BoxOpenIdle = "Open_Idle";

		public const string BoxEndIdle = "end_idle";
	}
}
