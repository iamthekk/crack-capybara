using System;

namespace Server
{
	public class HurtTrueAttack : HurtBase
	{
		public override HurtType GetHurtType()
		{
			return HurtType.TrueAttack;
		}

		protected override void OnRunBefore()
		{
			bool flag = base.m_skill != null && (base.m_skill.skillData.m_freedType == SkillFreedType.Ordinary || base.m_skill.skillData.m_freedType == SkillFreedType.Big);
			base.SetIsVampire(flag);
		}

		protected override void OnMathDamageCrit()
		{
		}

		protected override void OnMathSkillDamageFix()
		{
			if (!base.IsHit)
			{
				return;
			}
			FP fp = base.m_attacker.memberData.attribute.GetAttack() * 10;
			if (base.Attack > fp)
			{
				base.Attack = fp;
			}
		}

		protected override void OnMathResult()
		{
			if (base.IsHit)
			{
				base.m_defender.memberData.ChangeHp(-base.Attack, true);
				ReportTool.AddHurt(base.m_defender, base.m_attacker, base.m_hudValue, base.IsCrit ? ChangeHPType.Crit : ChangeHPType.TrueDamage, base.m_skill, base.m_bullet, base.m_buff, 1);
				return;
			}
			ReportTool.AddHurt(base.m_defender, base.m_attacker, base.m_hudValue, ChangeHPType.Miss, base.m_skill, base.m_bullet, base.m_buff, 1);
		}
	}
}
