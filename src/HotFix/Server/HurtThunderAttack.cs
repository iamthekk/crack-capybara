using System;

namespace Server
{
	public class HurtThunderAttack : HurtBase
	{
		public override HurtType GetHurtType()
		{
			return HurtType.ThunderAttack;
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
			FP electricDamageAdd = base.GetElectricDamageAdd();
			FP skillTypeDamageAdd = base.GetSkillTypeDamageAdd();
			FP fp = FP._1 + damageAdd + otherDamageAdd + stateDamageAdd + petDamageAdd + electricDamageAdd + skillTypeDamageAdd;
			fp = MathTools.Clamp(fp, SBattleConst.DamageFixPercentMin, SBattleConst.DamageFixPercentMax);
			base.Attack *= fp;
			base.Attack *= base.GetGlobalDamageAddPercent() * base.GetGlobalDamageReductionPercent();
			FP thunderCumulativeDamageAddPercent = base.m_attacker.memberData.attribute.ThunderCumulativeDamageAddPercent;
			if (thunderCumulativeDamageAddPercent > FP._0)
			{
				FP thunderHitByCount = base.m_defender.memberData.attribute.ThunderHitByCount;
				if (thunderHitByCount > FP._0)
				{
					base.Attack *= FP._1 + thunderCumulativeDamageAddPercent * thunderHitByCount;
				}
			}
			FP blockDamageReductionRate = base.GetBlockDamageReductionRate();
			base.Attack *= FP._1 - blockDamageReductionRate;
		}

		protected override void OnMathResult()
		{
			if (base.IsHit)
			{
				base.m_defender.memberData.ChangeHp(-base.Attack, true);
				ReportTool.AddHurt(base.m_defender, base.m_attacker, base.m_hudValue, base.IsCrit ? ChangeHPType.Crit : ChangeHPType.ThunderHurt, base.m_skill, base.m_bullet, base.m_buff, 1);
				return;
			}
			ReportTool.AddHurt(base.m_defender, base.m_attacker, base.m_hudValue, ChangeHPType.Miss, base.m_skill, base.m_bullet, base.m_buff, 1);
		}
	}
}
