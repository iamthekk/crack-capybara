using System;

namespace Server
{
	public class HurtPhysicalAttack : HurtBase
	{
		public override HurtType GetHurtType()
		{
			return HurtType.PhysicalAttack;
		}

		protected override void OnRunBefore()
		{
			bool flag = base.m_skill != null && (base.m_skill.skillData.m_freedType == SkillFreedType.Ordinary || base.m_skill.skillData.m_freedType == SkillFreedType.Big);
			base.SetIsVampire(flag);
		}

		protected override void OnMathDamageCrit()
		{
			if (!base.IsHit)
			{
				return;
			}
			base.IsCrit = false;
			FP attackerCritRate = base.GetAttackerCritRate();
			if (base.m_attacker.m_controller.Random01() <= attackerCritRate)
			{
				base.IsCrit = true;
				FP attackerCritValue = base.GetAttackerCritValue();
				base.Attack *= attackerCritValue;
			}
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
			FP petDamageAdd = base.GetPetDamageAdd();
			FP physicalDamageAdd = base.GetPhysicalDamageAdd();
			FP skillTypeDamageAdd = base.GetSkillTypeDamageAdd();
			FP fp = FP._1 + damageAdd + otherDamageAdd + stateDamageAdd + petDamageAdd + physicalDamageAdd + skillTypeDamageAdd;
			fp = MathTools.Clamp(fp, SBattleConst.DamageFixPercentMin, SBattleConst.DamageFixPercentMax);
			base.Attack *= fp;
			base.Attack *= base.GetGlobalDamageAddPercent() * base.GetGlobalDamageReductionPercent();
			FP blockDamageReductionRate = base.GetBlockDamageReductionRate();
			base.Attack *= FP._1 - blockDamageReductionRate;
		}

		protected void TrySwordkeeRecover()
		{
			if (base.m_skill == null || base.m_skill.skillData == null || base.m_skill.skillType != SkillType.Swordkee)
			{
				return;
			}
			FP swordkeeRecoverRate = base.m_attacker.memberData.attribute.SwordkeeRecoverRate;
			if (swordkeeRecoverRate <= FP._0)
			{
				return;
			}
			FP fp = swordkeeRecoverRate * base.m_attacker.memberData.attribute.GetHpMax();
			base.m_attacker.memberData.ChangeHp(fp, true);
			ReportTool.AddHurt(base.m_attacker, base.m_defender, fp, ChangeHPType.Recover, null, null, null, 1);
		}

		protected override void OnMathResult()
		{
			if (base.IsHit)
			{
				base.m_defender.memberData.ChangeHp(-base.Attack, true);
				ReportTool.AddHurt(base.m_defender, base.m_attacker, base.m_hudValue, base.IsCrit ? ChangeHPType.Crit : ChangeHPType.PhysicalHurt, base.m_skill, base.m_bullet, base.m_buff, 1);
				this.TrySwordkeeRecover();
				return;
			}
			ReportTool.AddHurt(base.m_defender, base.m_attacker, base.m_hudValue, ChangeHPType.Miss, base.m_skill, base.m_bullet, base.m_buff, 1);
		}
	}
}
