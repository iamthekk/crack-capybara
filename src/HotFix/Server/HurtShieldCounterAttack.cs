using System;

namespace Server
{
	public class HurtShieldCounterAttack : HurtBase
	{
		public override HurtType GetHurtType()
		{
			return HurtType.ShieldCounterAttack;
		}

		protected override void OnRunBefore()
		{
			bool flag = false;
			base.SetIsVampire(flag);
		}

		protected override void OnMathDamageCrit()
		{
			if (!base.IsHit)
			{
				return;
			}
			base.IsCrit = false;
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
