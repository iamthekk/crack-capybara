using System;

namespace Server
{
	public enum SkillTriggerConditionType
	{
		Default = 1,
		Anger,
		Value,
		Buff,
		SkillTypeTriggerCount,
		BeAttackedCount,
		Probability,
		BasicSkillTypeTriggerCount,
		ComboCount,
		NotFullHp,
		ShieldExist,
		TriggerKnife,
		TriggerFallingSword,
		LegacyPower,
		KnifeCount,
		CustomSkill,
		SkillCastTriggerCount
	}
}
