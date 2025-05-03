using System;

namespace Server
{
	public class HurtRecoverHP : HurtBase
	{
		public override HurtType GetHurtType()
		{
			return HurtType.RecoverHP;
		}

		protected override void SetHurt(HurtType hurtType, HurtData hurtData)
		{
			base.Attack = hurtData.m_attack;
			if (base.Attack >= FP._0)
			{
				base.Attack = -FP._1;
			}
			base.SetIsHit(true);
		}

		public override void Run(out long allAttack, out bool isMiss, out bool isCrit)
		{
			isMiss = false;
			isCrit = false;
			this.OnMathSkillDamageFix();
			base.m_hudValue = base.GetAllAttack();
			this.OnMathResult();
			allAttack = base.GetAllAttack();
		}

		protected override void OnMathDamageCrit()
		{
		}

		protected override void OnMathSkillDamageFix()
		{
			if (base.m_defender.memberData.attribute.IsNoHealing)
			{
				base.Attack = 0;
				return;
			}
			FP targetRecoverHPAddPercent = base.GetTargetRecoverHPAddPercent();
			base.Attack *= targetRecoverHPAddPercent;
			base.Attack = -MathTools.Clamp(base.Attack.Abs(), FP._0, FP.MaxValue);
		}

		protected override void OnMathResult()
		{
			FP fp = -base.GetAllAttack();
			base.m_defender.memberData.ChangeHp(fp, true);
			ReportTool.AddHurt(base.m_defender, base.m_attacker, base.m_hudValue, ChangeHPType.Recover, base.m_skill, base.m_bullet, base.m_buff, 1);
		}
	}
}
