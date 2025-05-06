using System;

namespace Server
{
	public abstract class HurtBase
	{
		private protected SMemberBase m_defender { protected get; private set; }

		private protected SMemberBase m_attacker { protected get; private set; }

		private protected SMemberBase m_attackerPet { protected get; private set; }

		private protected SSkillBase m_skill { protected get; private set; }

		private protected SBulletBase m_bullet { protected get; private set; }

		private protected SBuffBase m_buff { protected get; private set; }

		protected FP Attack { get; set; }

		protected FP ShieldAttack { get; set; }

		protected bool IsHit { get; set; }

		protected bool IsCrit { get; set; }

		protected bool IsSeckill { get; set; }

		protected bool IsVampire { get; set; }

		protected bool IsPetAttack { get; set; }

		protected int TargetSelectIndex { get; set; } = 1;

		protected FP m_hudValue { get; set; } = FP._0;

		public long GetAllAttack()
		{
			return this.Attack.FloorToLong();
		}

		public void SetMember(SMemberBase defender, SMemberBase attacker, SMemberBase attackerPet)
		{
			this.m_defender = defender;
			this.m_attacker = attacker;
			this.m_attackerPet = attackerPet;
		}

		public virtual void SetSkill(SSkillBase skill, SBulletBase bullet, HurtData hurtData)
		{
			this.m_skill = skill;
			this.m_bullet = bullet;
			this.SetHurt(this.GetHurtType(), hurtData);
		}

		public virtual void SetBuff(SBuffBase buff, HurtData hurtData)
		{
			this.m_buff = buff;
			this.SetHurt(this.GetHurtType(), hurtData);
		}

		public abstract HurtType GetHurtType();

		public void SetIsPetAttack(bool isPetAttack)
		{
			this.IsPetAttack = isPetAttack;
		}

		public void SetTargetSelectIndex(int selectIndex)
		{
			this.TargetSelectIndex = selectIndex;
		}

		protected virtual void SetHurt(HurtType hurtType, HurtData hurtData)
		{
			this.Attack = hurtData.m_attack;
			if (this.Attack <= FP._0)
			{
				this.Attack = FP._1;
			}
			if (this.m_skill != null)
			{
				bool flag = this.CalcIsSkillHit();
				this.SetIsHit(flag);
				return;
			}
			if (this.m_buff != null)
			{
				this.SetIsHit(true);
			}
		}

		protected void SetIsHit(bool isHit)
		{
			this.IsHit = isHit;
		}

		protected void SetIsVampire(bool isVampire)
		{
			this.IsVampire = isVampire;
		}

		public virtual void Run(out long allAttack, out bool isMiss, out bool isCrit)
		{
			if (!this.m_defender.memberData.attribute.IsInvincibled)
			{
				this.OnRunBefore();
				this.OnMathSkillBaseDamage();
				this.OnMathDamageCrit();
				this.OnMathSkillDamageFix();
				if (this.IsHit && this.Attack < FP._1)
				{
					this.Attack = FP._1;
				}
			}
			else
			{
				this.Attack = FP._0;
				this.m_defender.ReportTextTips("battle_text_Invincibled");
			}
			this.m_hudValue = (this.Attack = this.GetAllAttack());
			isMiss = !this.IsHit;
			isCrit = this.IsCrit;
			this.OnMathShield();
			this.OnMathVampire();
			this.OnMathState();
			this.OnMathResult();
			allAttack = this.GetAllAttack();
		}

		public FP GetDamage()
		{
			if (this.IsHit)
			{
				return this.m_hudValue;
			}
			return FP._0;
		}

		protected bool CalcIsSkillHit()
		{
			FP fp = this.m_defender.memberData.attribute.GetMissRate - this.m_attacker.memberData.attribute.GetIgnoreMiss;
			FP fp2 = this.m_attacker.m_controller.Random01();
			return fp < fp2;
		}

		protected virtual void OnRunBefore()
		{
		}

		protected virtual void OnMathSkillBaseDamage()
		{
			if (this.m_skill != null)
			{
				if (this.IsComboDamageType())
				{
					this.Attack *= FP._1 + this.m_attacker.memberData.attribute.CombatBaseDamagePercent;
					return;
				}
				if (this.IsCounterDamageType())
				{
					this.Attack *= FP._1 + this.m_attacker.memberData.attribute.CounterBaseDamagePercent;
					return;
				}
				if (this.m_skill.skillData.m_freedType == SkillFreedType.Ordinary)
				{
					this.Attack *= FP._1 + this.m_attacker.memberData.attribute.OrdinarySkillBaseDamagePercent;
					return;
				}
				if (this.m_skill.skillData.m_freedType == SkillFreedType.Big)
				{
					this.Attack *= FP._1 + this.m_attacker.memberData.attribute.BigSkillBaseDamagePercent;
				}
			}
		}

		protected abstract void OnMathDamageCrit();

		protected abstract void OnMathSkillDamageFix();

		protected virtual void OnMathState()
		{
		}

		protected void OnMathVampire()
		{
			if (!this.IsVampire || !this.IsHit)
			{
				return;
			}
			if (this.m_attacker.memberData.attribute.IsNoHealing)
			{
				return;
			}
			if (this.Attack <= FP._0)
			{
				return;
			}
			FP attackerVampireRate = this.GetAttackerVampireRate();
			if (this.m_defender.m_controller.Random01() <= attackerVampireRate)
			{
				FP attackerVampirePercent = this.GetAttackerVampirePercent();
				FP fp = this.Attack * attackerVampirePercent;
				if (fp < FP._1)
				{
					fp = FP._1;
				}
				FP fp2 = -fp;
				this.m_attacker.memberData.ChangeHp(fp, true);
				ReportTool.AddHurt(this.m_attacker, this.m_defender, fp2, ChangeHPType.Vampire, null, null, null, 1);
			}
		}

		public void ClearShield()
		{
			this.m_defender.memberData.ChangeShield(-this.m_defender.memberData.CurShield);
		}

		public virtual void OnMathShield()
		{
			if (!this.IsHit)
			{
				return;
			}
			if (this.Attack > 0)
			{
				this.CheckDefenderHitBefore();
			}
			if (this.m_defender.memberData.CurShield.Equals(0))
			{
				this.ShieldAttack = 0;
				return;
			}
			FP fp = this.m_defender.memberData.CurShield - this.Attack;
			if (fp <= 0)
			{
				this.ShieldAttack = -this.m_defender.memberData.CurShield;
				this.m_defender.memberData.ChangeShield(this.ShieldAttack);
				this.Attack = -fp;
				return;
			}
			this.ShieldAttack = -this.Attack;
			this.m_defender.memberData.ChangeShield(this.ShieldAttack);
			this.Attack = 0;
		}

		protected virtual void OnMathResult()
		{
			if (this.IsHit)
			{
				this.m_defender.memberData.ChangeHp(-this.Attack, true);
				ReportTool.AddHurt(this.m_defender, this.m_attacker, this.m_hudValue, this.IsCrit ? ChangeHPType.Crit : ChangeHPType.NormalHurt, this.m_skill, this.m_bullet, this.m_buff, this.TargetSelectIndex);
				return;
			}
			ReportTool.AddHurt(this.m_defender, this.m_attacker, this.m_hudValue, ChangeHPType.Miss, this.m_skill, this.m_bullet, this.m_buff, 1);
		}

		protected void CheckDefenderHitBefore()
		{
			if (this.m_attacker == null || this.m_defender.IsDeath)
			{
				return;
			}
			if (this.m_attacker != this.m_defender)
			{
				SkillTriggerData skillTriggerData = new SkillTriggerData();
				skillTriggerData.m_triggerType = SkillTriggerType.HitByHurtedBefore;
				skillTriggerData.SetAttacker(this.m_attacker);
				skillTriggerData.m_parameter = 1;
				skillTriggerData.m_iHitTargetList.Add(this.m_defender);
				this.m_defender.skillFactory.CheckPlay(skillTriggerData);
			}
		}

		protected bool IsCounterDamageType()
		{
			return this.m_skill != null && this.m_skill.CastType == SkillCastType.Counter;
		}

		protected bool IsComboDamageType()
		{
			return this.m_skill != null && this.m_skill.CastType == SkillCastType.Combo;
		}

		protected FP GetCombatSuppressionValue()
		{
			return FP._0;
		}

		protected FP GetDamageAdd()
		{
			FP fp = this.m_attacker.memberData.attribute.DamageAddPercent - this.m_defender.memberData.attribute.DamageReductionPercent;
			if (this.m_defender.memberData.IsHpGreater60Percent)
			{
				fp += this.m_attacker.memberData.attribute.Greater60HpDamageAddPercent;
			}
			if (this.m_attacker.memberData.attribute.IsHpHigherDamageHigher)
			{
				fp += this.m_defender.memberData.HPPercent * FP._030;
			}
			if (this.m_defender.memberData.m_RoleType == MemberRoleType.Boss)
			{
				fp += this.m_attacker.memberData.attribute.ToBossDamageAddPercent;
			}
			return fp;
		}

		protected virtual FP GetOtherDamageAdd()
		{
			FP fp = FP._0;
			if (this.m_skill != null)
			{
				MemberAttributeData attribute = this.m_attacker.memberData.attribute;
				MemberAttributeData attribute2 = this.m_defender.memberData.attribute;
				if (this.m_skill.targetShieldAddDamage != null && this.m_skill.targetShieldAddDamage.isOpen > 0 && this.m_defender.memberData.CurShield > FP._0)
				{
					fp += this.m_skill.targetShieldAddDamage.damageAddPercent * 0.01f;
				}
				if (this.m_skill.attackerHpLessAddDamage != null && this.m_skill.attackerHpLessAddDamage.isOpen > 0)
				{
					FP fp2 = 100 - this.m_attacker.memberData.CurHP * FP._100 / this.m_attacker.memberData.attribute.GetHpMax();
					if (fp2 > 0)
					{
						fp += (fp2 / this.m_skill.attackerHpLessAddDamage.hpPercent).FloorToLong() * this.m_skill.attackerHpLessAddDamage.damageAddPercent * 0.01f;
					}
				}
				if (this.m_skill.targetHpLessAddDamage != null && this.m_skill.targetHpLessAddDamage.isOpen > 0)
				{
					FP fp3 = 100 - this.m_attacker.memberData.CurHP * FP._100 / this.m_attacker.memberData.attribute.GetHpMax();
					if (fp3 > 0)
					{
						fp += (fp3 / this.m_skill.targetHpLessAddDamage.hpPercent).FloorToLong() * this.m_skill.targetHpLessAddDamage.damageAddPercent * 0.01f;
					}
				}
				if (this.m_skill.skillData.m_freedType == SkillFreedType.Big)
				{
					SSkillBase skill = this.m_skill;
					if (this.m_skill.IsFullBloodAddDamage && this.m_defender.memberData.HPPercent >= this.m_attacker.memberData.attribute.BigSkillDamageAddByGreaterHpPercent)
					{
						fp += this.m_attacker.memberData.attribute.GreaterHpPercentBigSkillDamageAddPercent;
					}
					if (this.m_skill.IsLessThanBloodAddDamage && this.m_defender.memberData.HPPercent <= this.m_attacker.memberData.attribute.BigSkillDamageAddByLessHpPercent)
					{
						fp += this.m_attacker.memberData.attribute.LessHpPercentBigSkillDamageAddPercent;
					}
				}
				if (this.m_skill.skillData.m_freedType == SkillFreedType.Big || this.m_skill.skillData.m_freedType == SkillFreedType.Small || this.m_skill.skillData.m_freedType == SkillFreedType.Legacy)
				{
					fp += attribute.SkillDamageAddPercent - attribute2.SkillDamageReductionPercent;
				}
				if (this.m_skill.CastType == SkillCastType.Combo)
				{
					fp += attribute.ComboDamageAddPercent - attribute2.ComboDamageReductionPercent;
				}
				else if (this.m_skill.CastType == SkillCastType.Counter)
				{
					fp += attribute.RevengeDamageAddPercent - attribute2.RevengeDamageReductionPercent;
				}
			}
			return fp;
		}

		protected FP GetStateDamageAdd()
		{
			FP fp = FP._0;
			if (this.m_defender.IsFrozen)
			{
				fp += this.m_attacker.memberData.attribute.IcicleAttackAddPercent;
			}
			if (this.m_defender.IsStun)
			{
				fp += this.m_attacker.memberData.attribute.StunAttackAddPercent;
			}
			if (this.m_defender.IsFrozen || this.m_defender.IsStun)
			{
				fp += this.m_attacker.memberData.attribute.IcicleOrStunDamageAddPercent;
			}
			if (this.m_defender.buffFactory.IsHasPoison)
			{
				fp += this.m_attacker.memberData.attribute.PoisonAttackAddPercent;
			}
			if (this.m_defender.buffFactory.IsHasBurn)
			{
				fp += this.m_attacker.memberData.attribute.BurnAttackAddPercent;
				bool flag = false;
				FP fp2 = FP._0;
				if (this.IsPetAttack && this.m_attackerPet != null)
				{
					flag = this.m_attackerPet.memberData.attribute.IsFireBuffPerLayerDamageAdd > 0;
					fp2 = this.m_attackerPet.memberData.attribute.FireBuffPerLayerDamageAddPercent;
				}
				else if (!this.IsPetAttack && this.m_attacker != null)
				{
					flag = this.m_attacker.memberData.attribute.IsFireBuffPerLayerDamageAdd > 0;
					fp2 = this.m_attacker.memberData.attribute.FireBuffPerLayerDamageAddPercent;
				}
				if (flag)
				{
					FP burnLayerCount = this.m_defender.buffFactory.BurnLayerCount;
					fp += burnLayerCount * fp2;
				}
			}
			if (this.m_defender.buffFactory.IsVerdict)
			{
				fp += this.m_attacker.memberData.attribute.ToVerdictAddPercent;
			}
			if (this.m_defender.buffFactory.IsVulnerability && this.IsCrit)
			{
				fp += this.m_attacker.memberData.attribute.VulnerabilityDamageAddPercent;
			}
			if (this.m_defender.memberData.CurShield > FP._0)
			{
				fp -= this.m_defender.memberData.attribute.ShieldDamageReductionPercent;
			}
			return fp;
		}

		protected FP GetSkillTypeDamageAdd()
		{
			FP fp = FP._0;
			if (this.m_skill != null && this.m_skill.skillData != null && this.m_skill.skillData.skillTypeDamageAddParam != null && this.m_skill.skillData.skillTypeDamageAddParam.Length != 0)
			{
				foreach (SkillDamageType skillDamageType in this.m_skill.skillData.skillTypeDamageAddParam)
				{
					if (skillDamageType <= SkillDamageType.Burn)
					{
						if (skillDamageType <= SkillDamageType.BigSkillAttack)
						{
							if (skillDamageType != SkillDamageType.OrdinaryAttack)
							{
								if (skillDamageType == SkillDamageType.BigSkillAttack)
								{
									fp += this.GetBigSkillDamageAdd();
								}
							}
							else
							{
								fp += this.GetOrdinaryDamageAdd();
							}
						}
						else
						{
							switch (skillDamageType)
							{
							case SkillDamageType.FlyingKnife:
								fp += this.GetKnifeDamageAdd();
								break;
							case SkillDamageType.FallingSword:
								fp += this.GetFallingSwordDamageAdd();
								break;
							case SkillDamageType.Swordkee:
								fp += this.GetSwordkeeDamageAdd();
								break;
							default:
								if (skillDamageType != SkillDamageType.FireWave)
								{
									if (skillDamageType == SkillDamageType.Burn)
									{
										fp += this.GetBurnDamageAdd();
									}
								}
								else
								{
									fp += this.GetFireWaveDamageAdd();
								}
								break;
							}
						}
					}
					else if (skillDamageType <= SkillDamageType.Icicle)
					{
						if (skillDamageType != SkillDamageType.Thunder)
						{
							if (skillDamageType == SkillDamageType.Icicle)
							{
								fp += this.GetIcicleDamageAdd();
							}
						}
						else
						{
							fp += this.GetThunderDamageAdd();
						}
					}
					else if (skillDamageType != SkillDamageType.ComboAttack)
					{
						if (skillDamageType != SkillDamageType.CounterAttack)
						{
							if (skillDamageType == SkillDamageType.PetAttack)
							{
								if (!this.IsPetAttack)
								{
									fp += this.GetPetDamageAdd();
								}
							}
						}
						else
						{
							fp += this.GetCounterDamageAdd();
						}
					}
					else
					{
						fp += this.GetComboDamageAdd();
					}
				}
			}
			return fp;
		}

		protected FP GetBlockDamageReductionRate()
		{
			FP fp = FP._0;
			if (this.m_attacker.memberData.m_RoleType == MemberRoleType.NormalMonster || this.m_attacker.memberData.m_RoleType == MemberRoleType.EliteMonster || this.m_attacker.memberData.m_RoleType == MemberRoleType.Boss || this.m_defender.memberData.m_RoleType == MemberRoleType.NormalMonster || this.m_defender.memberData.m_RoleType == MemberRoleType.EliteMonster || this.m_defender.memberData.m_RoleType == MemberRoleType.Boss)
			{
				return fp;
			}
			FP fp2 = this.m_defender.memberData.attribute.BlockValue - this.m_attacker.memberData.attribute.BlockValue;
			if (fp2 > 0)
			{
				FP fp3 = this.m_defender.m_controller.Random01();
				if (this.m_defender.memberData.attribute.BlockTriggerPercent >= fp3)
				{
					fp = MathTools.Clamp(fp2, FP._0, 9000);
					fp *= 0.0001f;
				}
			}
			return fp;
		}

		protected FP GetAttackerCritRate()
		{
			MemberAttributeData attribute = this.m_attacker.memberData.attribute;
			MemberAttributeData attribute2 = this.m_defender.memberData.attribute;
			FP fp = attribute.CritRate - attribute2.IgnoreCritRate;
			if (this.IsPetAttack)
			{
				fp += attribute.PetCritRate;
			}
			if (this.m_skill != null)
			{
				if (this.m_skill.skillData.m_freedType == SkillFreedType.Ordinary)
				{
					fp += attribute.NormalCritRate;
				}
				else if (this.m_skill.skillData.m_freedType == SkillFreedType.Big || this.m_skill.skillData.m_freedType == SkillFreedType.Small)
				{
					fp += attribute.SkillCritRate;
				}
				if (this.m_skill.skillData.m_freedType == SkillFreedType.Ordinary || this.m_skill.skillData.m_freedType == SkillFreedType.Big)
				{
					fp += attribute.WeaponCritRate;
				}
			}
			MathTools.Clamp(fp, SBattleConst.CritRateMin, SBattleConst.CritRateMax);
			return fp;
		}

		protected FP GetAttackerCritValue()
		{
			FP fp = this.m_attacker.memberData.attribute.CritValue - this.m_defender.memberData.attribute.CritValueReduction;
			if (this.m_skill != null)
			{
				if (this.m_skill.CastType == SkillCastType.Combo)
				{
					fp += this.m_attacker.memberData.attribute.NormalComboCritAddPercent;
				}
				if (this.m_skill.skillData.m_freedType == SkillFreedType.Ordinary)
				{
					if (this.m_attacker.memberData.attribute.IsNormalCritRateAddCritValue)
					{
						fp += this.m_attacker.memberData.attribute.NormalCritRate * this.m_attacker.memberData.attribute.IsNormalCritAddValue;
					}
				}
				else if ((this.m_skill.skillData.m_freedType == SkillFreedType.Big || this.m_skill.skillData.m_freedType == SkillFreedType.Small) && this.m_attacker.memberData.attribute.IsSkillCritRateAddCritValue)
				{
					fp += this.m_attacker.memberData.attribute.SkillCritRate * this.m_attacker.memberData.attribute.IsSkillCritAddValue;
				}
			}
			if (this.m_defender.buffFactory.IsDefenseReduction)
			{
				fp += this.m_attacker.memberData.attribute.DefenseReductionCritValue;
			}
			return MathTools.Clamp(fp, SBattleConst.CritDamageMin, SBattleConst.CritDamageMax);
		}

		protected FP GetTargetRecoverHPAddPercent()
		{
			return MathTools.Clamp(this.m_defender.memberData.attribute.GetRecoverAddPercent(), FP._0, FP.MaxValue);
		}

		protected FP GetAttackerVampireRate()
		{
			FP fp = this.m_attacker.memberData.attribute.VampireRate;
			if (this.m_skill != null && this.m_skill.CastType == SkillCastType.Counter)
			{
				fp += this.m_attacker.memberData.attribute.CounterVampireRate;
			}
			return MathTools.Clamp(fp, SBattleConst.VampireProbabilityMin, SBattleConst.VampireProbabilityMax);
		}

		protected FP GetAttackerVampirePercent()
		{
			return MathTools.Clamp((FP._1 + this.m_attacker.memberData.attribute.VampireAddPercent) * SBattleConst.VampireConstRate * this.m_attacker.memberData.attribute.GetRecoverAddPercent(), FP._0, FP.MaxValue);
		}

		protected FP GetPetDamageAdd()
		{
			FP fp = FP._0;
			if (this.IsPetAttack)
			{
				fp = this.m_attacker.memberData.attribute.PetDamageAddPercent - this.m_defender.memberData.attribute.PetDamageReductionPercent;
				if (this.m_attackerPet != null)
				{
					if (this.m_attackerPet.memberData.cardData.m_petData.petQuality == EPetQuality.Purple)
					{
						fp += this.m_attacker.memberData.attribute.PurplePetDamageAddPercent;
					}
					else if (this.m_attackerPet.memberData.cardData.m_petData.petQuality == EPetQuality.Orange)
					{
						fp += this.m_attacker.memberData.attribute.OrangePetDamageAddPercent;
					}
					else if (this.m_attackerPet.memberData.cardData.m_petData.petQuality == EPetQuality.Red)
					{
						fp += this.m_attacker.memberData.attribute.RedPetDamageAddPercent;
					}
					fp += this.m_attackerPet.memberData.attribute.DamageAddPercent;
				}
			}
			return fp;
		}

		protected FP GetFireBuffDamageAdd()
		{
			return this.m_attacker.memberData.attribute.FireBuffAddPercent - this.m_defender.memberData.attribute.FireBuffReductionPercent;
		}

		protected FP GetGlobalDamageAddPercent()
		{
			return FP._1 + this.m_attacker.memberData.attribute.GlobalDamageAddPercent;
		}

		protected FP GetGlobalDamageReductionPercent()
		{
			return MathTools.Clamp(FP._1 - this.m_defender.memberData.attribute.GlobalDamageReductionPercent, SBattleConst.GlobalDamageReductionPercentMin, SBattleConst.GlobalDamageReductionPercentMax);
		}

		protected FP GetPhysicalDamageAdd()
		{
			return this.m_attacker.memberData.attribute.PhysicalAddPercent - this.m_defender.memberData.attribute.PhysicalReductionPercent;
		}

		protected FP GetFireDamageAdd()
		{
			return this.m_attacker.memberData.attribute.FireAddPercent - this.m_defender.memberData.attribute.FireReductionPercent;
		}

		protected FP GetElectricDamageAdd()
		{
			return this.m_attacker.memberData.attribute.ElectricAddPercent - this.m_defender.memberData.attribute.ElectricReductionPercent;
		}

		protected FP GetIceDamageAdd()
		{
			return this.m_attacker.memberData.attribute.IceAddPercent - this.m_defender.memberData.attribute.IceReductionPercent;
		}

		protected FP GetTrueDamageAdd()
		{
			return this.m_attacker.memberData.attribute.TrueAddPercent - this.m_defender.memberData.attribute.TrueReductionPercent;
		}

		protected FP GetRecoverAdd()
		{
			return this.m_attacker.memberData.attribute.RecoverAddPercent - this.m_defender.memberData.attribute.RecoverReductionPercent;
		}

		protected FP GetOrdinaryDamageAdd()
		{
			return this.m_attacker.memberData.attribute.NormalDamageAddPercent - this.m_defender.memberData.attribute.NormalDamageReductionPercent;
		}

		protected FP GetBigSkillDamageAdd()
		{
			return this.m_attacker.memberData.attribute.BigSkillDamageAddPercent - this.m_defender.memberData.attribute.BigSkillDamageReductionPercent;
		}

		protected FP GetComboDamageAdd()
		{
			if (this.m_skill != null && this.m_skill.CastType == SkillCastType.Combo)
			{
				return FP._0;
			}
			return this.m_attacker.memberData.attribute.ComboDamageAddPercent - this.m_defender.memberData.attribute.ComboDamageReductionPercent;
		}

		protected FP GetCounterDamageAdd()
		{
			if (this.m_skill != null && this.m_skill.CastType == SkillCastType.Counter)
			{
				return FP._0;
			}
			return this.m_attacker.memberData.attribute.RevengeDamageAddPercent - this.m_defender.memberData.attribute.RevengeDamageReductionPercent;
		}

		protected FP GetKnifeDamageAdd()
		{
			return this.m_attacker.memberData.attribute.KnifeAddPercent - this.m_defender.memberData.attribute.KnifeReductionPercent;
		}

		protected FP GetFallingSwordDamageAdd()
		{
			return this.m_attacker.memberData.attribute.FallingSwordAddPercent - this.m_defender.memberData.attribute.FallingSwordReductionPercent;
		}

		protected FP GetSwordkeeDamageAdd()
		{
			return this.m_attacker.memberData.attribute.SwordkeeAddPercent - this.m_defender.memberData.attribute.SwordkeeReductionPercent;
		}

		protected FP GetFireWaveDamageAdd()
		{
			return this.m_attacker.memberData.attribute.FireWaveAddPercent - this.m_defender.memberData.attribute.FireWaveReductionPercent;
		}

		protected FP GetBurnDamageAdd()
		{
			return this.m_attacker.memberData.attribute.BurnAddPercent - this.m_defender.memberData.attribute.BurnReductionPercent;
		}

		protected FP GetIcicleDamageAdd()
		{
			return this.m_attacker.memberData.attribute.IcicleAddPercent - this.m_defender.memberData.attribute.IcicleReductionPercent;
		}

		protected FP GetThunderDamageAdd()
		{
			return this.m_attacker.memberData.attribute.ThunderAddPercent - this.m_defender.memberData.attribute.ThunderReductionPercent;
		}

		protected FP GetPoisonDamageAdd()
		{
			return this.m_attacker.memberData.attribute.PoisonAddPercent - this.m_defender.memberData.attribute.PoisonReductionPercent;
		}
	}
}
