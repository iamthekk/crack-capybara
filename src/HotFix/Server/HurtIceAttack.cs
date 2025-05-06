using System;

namespace Server
{
	public class HurtIceAttack : HurtBase
	{
		public override HurtType GetHurtType()
		{
			return HurtType.IceAttack;
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

		public override void Run(out long allAttack, out bool isMiss, out bool isCrit)
		{
			if (base.m_defender.memberData.attribute.IsInvincibled)
			{
				base.Attack = FP._0;
				base.m_defender.ReportTextTips("battle_text_Invincibled");
			}
			else
			{
				if (base.m_defender.memberData.m_RoleType == MemberRoleType.NormalMonster)
				{
					base.IsSeckill = base.m_attacker.m_controller.Random01() <= base.m_attacker.memberData.attribute.IcicleSeckillRate;
				}
				if (base.IsHit && base.IsSeckill)
				{
					base.ClearShield();
					base.Attack = base.m_defender.memberData.CurHP;
				}
				else
				{
					this.OnMathSkillBaseDamage();
					this.OnMathDamageCrit();
					this.OnMathSkillDamageFix();
					if (base.IsHit && base.Attack < FP._0)
					{
						base.Attack = FP._1;
					}
					base.m_hudValue = base.Attack;
					this.OnMathShield();
					base.OnMathVampire();
				}
			}
			this.OnMathState();
			this.OnMathResult();
			allAttack = base.GetAllAttack();
			isMiss = !base.IsHit;
			isCrit = base.IsCrit;
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
			FP iceDamageAdd = base.GetIceDamageAdd();
			FP skillTypeDamageAdd = base.GetSkillTypeDamageAdd();
			FP fp = FP._1 + damageAdd + otherDamageAdd + stateDamageAdd + petDamageAdd + iceDamageAdd + skillTypeDamageAdd;
			fp = MathTools.Clamp(fp, SBattleConst.DamageFixPercentMin, SBattleConst.DamageFixPercentMax);
			base.Attack *= fp;
			base.Attack *= base.GetGlobalDamageAddPercent() * base.GetGlobalDamageReductionPercent();
			FP blockDamageReductionRate = base.GetBlockDamageReductionRate();
			base.Attack *= FP._1 - blockDamageReductionRate;
		}

		protected override void OnMathResult()
		{
			if (base.IsSeckill)
			{
				base.m_defender.memberData.ChangeHp(-base.Attack, true);
				ReportTool.AddHurt(base.m_defender, base.m_attacker, base.m_hudValue, ChangeHPType.Seckill, base.m_skill, base.m_bullet, base.m_buff, 1);
				return;
			}
			if (base.IsHit)
			{
				base.m_defender.memberData.ChangeHp(-base.Attack, true);
				ReportTool.AddHurt(base.m_defender, base.m_attacker, base.m_hudValue, base.IsCrit ? ChangeHPType.Crit : ChangeHPType.IceHurt, base.m_skill, base.m_bullet, base.m_buff, 1);
				return;
			}
			ReportTool.AddHurt(base.m_defender, base.m_attacker, base.m_hudValue, ChangeHPType.Miss, base.m_skill, base.m_bullet, base.m_buff, 1);
		}
	}
}
