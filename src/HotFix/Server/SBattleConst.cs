using System;

namespace Server
{
	public static class SBattleConst
	{
		public static FP CombatSuppressionMin { get; private set; } = -0.2f;

		public static FP CombatSuppressionMax { get; private set; } = FP.MaxValue;

		public static FP CritRateMin { get; private set; } = FP._0;

		public static FP CritRateMax { get; private set; } = FP._1;

		public static FP CritDamageMin { get; private set; } = 1.25f;

		public static FP CritDamageMax { get; private set; } = 5;

		public static FP DamageFixPercentMin { get; private set; } = 0.25f;

		public static FP DamageFixPercentMax { get; private set; } = 5;

		public static FP VampireProbabilityMin { get; private set; } = FP._0;

		public static FP VampireProbabilityMax { get; private set; } = FP._1;

		public static FP VampireConstRate { get; private set; } = 0.1f;

		public static FP GlobalDamageReductionPercentMin { get; private set; } = 0.1f;

		public static FP GlobalDamageReductionPercentMax { get; private set; } = FP._1;

		public static FP DamageAddLegacyPower4Percent { get; private set; } = FP._1;

		public static FP DamageAddLegacyPower4Value { get; private set; } = FP._5;
	}
}
