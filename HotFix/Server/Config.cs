using System;
using System.Collections.Generic;

namespace Server
{
	public static class Config
	{
		public static float GetTimeByFrame(int frame)
		{
			return (float)frame / 30f;
		}

		public static string BsvVersion { get; private set; } = "-1";

		public static void SetBsvVersion(string version)
		{
			Config.BsvVersion = version;
		}

		public const bool VisibleLegacyPowerProgress = false;

		public const int LogicFrame = 30;

		public const int ChapterMaxRound = 30;

		public const int ArenaMaxRound = 15;

		public const int TowerMaxRound = 15;

		public const int DungeonMaxRound = 15;

		public const int GuildBossMaxRound = 15;

		public const int BattleArenaMaxRound = 30;

		public static FP PVPMaxHpRate = 5;

		public const int RogueDungeonMaxRound = 15;

		public const int WorldBossMaxRound = 10;

		public const int ShieldMaxBetLimit = 1;

		public const int FrameCost_LegacyAnimationCast = 18;

		public const int FrameCost_LegacySkillSummonRevert = 20;

		public const int FrameCost_Revive = 46;

		public const int MoveFrame = 3;

		public const int ThunderHurtInterval = 8;

		public const int IcicleHurtInterval = 8;

		public const int FireHurtInterval = 8;

		public const int KnifeHurtInterval = 8;

		public const int RoleRoundEndInterval = 3;

		public const int RoleBeHitedInterval = 3;

		public const int chainComboInterval = 8;

		public const int BuffHurtWaitTime = 15;

		public const int WaveReadyWaitTime = 30;

		public const int WaveCreateAndEnterMoveTime = 20;

		public const string Animator_HitName_Miss = "Dodge";

		public const int CirtShakeID = 4;

		public const int FriendlyInsID = 100;

		public const int FriendlyPetInsID = 150;

		public const int FriendlyMountInsID = 180;

		public const int EnemyInsID = 200;

		public const int EnemyPetInsID = 250;

		public const int TrueDamageAttackLimit = 10;

		public const string BattleVersion = "1.0.2";

		public const string VersionTime = "2024-06-17 23:18:14";

		public const string HUD_Text_Combo = "battle_text_combo";

		public const string HUD_Text_Counter = "battle_text_counter";

		public const string HUD_Text_Stun = "battle_text_stun";

		public const string HUD_Text_Frozen = "battle_text_frozen";

		public const string HUD_Text_Miss = "battle_text_miss";

		public const string HUD_Text_IgnoreStun = "battle_text_IgnoreStun";

		public const string HUD_Text_IgnoreFrozen = "battle_text_IgnoreFrozen";

		public const string HUD_Text_Invincibled = "battle_text_Invincibled";

		public const int TestBattleMaxRound = 30;

		public static List<int> RoleMembers = new List<int> { 1 };

		public static List<int> EnemyMmbers = new List<int> { 3 };

		public static List<List<int>> WaveList = new List<List<int>>
		{
			new List<int> { 3 },
			new List<int> { 3 }
		};

		public static List<int> InspectorEnemyMmbers = new List<int>();

		public static List<int> MainRoleAddSkill = new List<int>();

		public static string RoleAttributeAdd = "";

		public static List<int> PetAddSkill = new List<int>();

		public static List<int> EnemysAddSkill = new List<int>();

		public static float GameEvent_Power_AddAttack = 1.015f;

		public static float GameEvent_Power_AddHP = 1.045f;
	}
}
