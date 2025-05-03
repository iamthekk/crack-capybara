using System;

namespace Server
{
	public class HurtAttack : HurtBase
	{
		public override HurtType GetHurtType()
		{
			return HurtType.Attack;
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
			FP skillTypeDamageAdd = base.GetSkillTypeDamageAdd();
			FP fp = FP._1 + damageAdd + otherDamageAdd + stateDamageAdd + petDamageAdd + skillTypeDamageAdd;
			fp = MathTools.Clamp(fp, SBattleConst.DamageFixPercentMin, SBattleConst.DamageFixPercentMax);
			base.Attack *= fp;
			base.Attack *= base.GetGlobalDamageAddPercent() * base.GetGlobalDamageReductionPercent();
			FP blockDamageReductionRate = base.GetBlockDamageReductionRate();
			base.Attack *= FP._1 - blockDamageReductionRate;
		}
	}
}
