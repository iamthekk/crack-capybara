using System;

namespace Server
{
	public class HurtFireBuffAttack : HurtBase
	{
		public override HurtType GetHurtType()
		{
			return HurtType.FireBuffAttack;
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
			FP damageAdd = base.GetDamageAdd();
			FP otherDamageAdd = this.GetOtherDamageAdd();
			FP stateDamageAdd = base.GetStateDamageAdd();
			FP burnDamageAdd = base.GetBurnDamageAdd();
			FP fireBuffDamageAdd = base.GetFireBuffDamageAdd();
			FP petDamageAdd = base.GetPetDamageAdd();
			FP fp = FP._1 + damageAdd + otherDamageAdd + stateDamageAdd + fireBuffDamageAdd + petDamageAdd + burnDamageAdd;
			fp = MathTools.Clamp(fp, SBattleConst.DamageFixPercentMin, SBattleConst.DamageFixPercentMax);
			base.Attack *= fp;
		}

		protected override void OnMathResult()
		{
			if (base.IsHit)
			{
				base.m_defender.memberData.ChangeHp(-base.Attack, true);
				ReportTool.AddHurt(base.m_defender, base.m_attacker, base.m_hudValue, base.IsCrit ? ChangeHPType.Crit : ChangeHPType.FireHurt, base.m_skill, base.m_bullet, base.m_buff, 1);
			}
		}
	}
}
