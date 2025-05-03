using System;

namespace Server
{
	public enum BattleReportType
	{
		CreateMember = 1,
		GameStart,
		GameOver,
		RoundStart,
		RoundEnd,
		PlaySkill,
		Hurt,
		PlaySkillComplete,
		CreateBullet,
		BuffAdd,
		BuffUpdate,
		BuffRemove,
		ChangeAttributes,
		Move,
		Revive,
		WaitRoundCount,
		WaveChange,
		TextTips,
		LegacySkillSummonDisplay
	}
}
