using System;

namespace Server
{
	public class HurtPoisonBuffAttack : HurtBase
	{
		public override HurtType GetHurtType()
		{
			return HurtType.PoisonBuffAttack;
		}

		protected override void OnMathDamageCrit()
		{
		}

		protected override void OnMathSkillDamageFix()
		{
			FP damageAdd = base.GetDamageAdd();
			FP otherDamageAdd = this.GetOtherDamageAdd();
			FP stateDamageAdd = base.GetStateDamageAdd();
			FP poisonDamageAdd = base.GetPoisonDamageAdd();
			FP petDamageAdd = base.GetPetDamageAdd();
			FP fp = FP._1 + damageAdd + otherDamageAdd + stateDamageAdd + poisonDamageAdd + petDamageAdd;
			fp = MathTools.Clamp(fp, FP._0, FP.MaxValue);
			base.Attack *= fp;
			base.Attack *= base.GetGlobalDamageAddPercent() * base.GetGlobalDamageReductionPercent();
		}

		protected override void OnMathResult()
		{
			if (base.IsHit)
			{
				base.m_defender.memberData.ChangeHp(-base.Attack, true);
				ReportTool.AddHurt(base.m_defender, base.m_attacker, base.m_hudValue, ChangeHPType.PoisonHurt, base.m_skill, base.m_bullet, base.m_buff, 1);
			}
		}
	}
}
