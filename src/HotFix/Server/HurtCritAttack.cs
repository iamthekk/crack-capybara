using System;

namespace Server
{
	public class HurtCritAttack : HurtAttack
	{
		public override HurtType GetHurtType()
		{
			return HurtType.CritAttack;
		}

		protected override void OnMathDamageCrit()
		{
			if (!base.IsHit)
			{
				return;
			}
			base.IsCrit = true;
			FP attackerCritValue = base.GetAttackerCritValue();
			base.Attack *= attackerCritValue;
		}
	}
}
