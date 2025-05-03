using System;

namespace Server
{
	public enum BuffTriggerTags
	{
		None,
		Default,
		RoundStart,
		RoundEnd,
		MemberRoundStart,
		MemberRoundEnd,
		BeforeTargetCountHurt,
		AfterTargetCountHurt,
		BeforeBeNormalAttack,
		BeforeBeBigSkillAttack,
		BeCured,
		TeamMemberAttacked,
		TeamMemberCured,
		Killed,
		Revive,
		NearDeath,
		Death,
		EnemyRoundEnd,
		BuffRemove
	}
}
