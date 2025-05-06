using System;
using System.Collections.Generic;

namespace Server
{
	public class MemberAttributeData
	{
		public BattleMode CurBattleMode { get; set; }

		public bool IsNoHealing
		{
			get
			{
				return this.NoHealing > FP._0;
			}
		}

		public void CopyFrom(MemberAttributeData attributeData)
		{
			if (attributeData == null)
			{
				return;
			}
			this.GlobalAttackPercent = attributeData.GlobalAttackPercent;
			this.GlobalHPMaxPercent = attributeData.GlobalHPMaxPercent;
			this.GlobalDefencePercent = attributeData.GlobalDefencePercent;
			this.GlobalDamageAddPercent = attributeData.GlobalDamageAddPercent;
			this.GlobalDamageReductionPercent = attributeData.GlobalDamageReductionPercent;
			this.Attack = attributeData.Attack;
			this.AttackPercent = attributeData.AttackPercent;
			this.HPMax = attributeData.HPMax;
			this.HPMaxPercent = attributeData.HPMaxPercent;
			this.Defence = attributeData.Defence;
			this.DefencePercent = attributeData.DefencePercent;
			this.IgnoreDefencePercent = attributeData.IgnoreDefencePercent;
			this.LegacyPowerAddPercent = attributeData.LegacyPowerAddPercent;
			this.Miss = attributeData.Miss;
			this.MissDoublePercent = attributeData.MissDoublePercent;
			this.IgnoreMiss = attributeData.IgnoreMiss;
			this.RechargeMax = attributeData.RechargeMax;
			this.NoEnergyRecovery = attributeData.NoEnergyRecovery;
			this.NormalComboRate = attributeData.NormalComboRate;
			this.NormalComboCount = attributeData.NormalComboCount;
			this.NormalComboDamageAddPercent = attributeData.NormalComboDamageAddPercent;
			this.NormalComboCritAddPercent = attributeData.NormalComboCritAddPercent;
			this.DefenseReductionCritValue = attributeData.DefenseReductionCritValue;
			this.CritRate = attributeData.CritRate;
			this.PetCritRate = attributeData.PetCritRate;
			this.CritValue = attributeData.CritValue;
			this.CritValueReduction = attributeData.CritValueReduction;
			this.NormalCritRate = attributeData.NormalCritRate;
			this.SkillCritRate = attributeData.SkillCritRate;
			this.IsNormalCritAddValue = attributeData.IsNormalCritAddValue;
			this.IsSkillCritAddValue = attributeData.IsSkillCritAddValue;
			this.WeaponCritRate = attributeData.WeaponCritRate;
			this.DamageAddPercent = attributeData.DamageAddPercent;
			this.DamageReductionPercent = attributeData.DamageReductionPercent;
			this.Greater60HpDamageAddPercent = attributeData.Greater60HpDamageAddPercent;
			this.IsTargetHpHigherDamageHigher = attributeData.IsTargetHpHigherDamageHigher;
			this.IsInvincible = attributeData.IsInvincible;
			this.SkillDamageAddPercent = attributeData.SkillDamageAddPercent;
			this.SkillDamageReductionPercent = attributeData.SkillDamageReductionPercent;
			this.ComboDamageAddPercent = attributeData.ComboDamageAddPercent;
			this.ComboDamageReductionPercent = attributeData.ComboDamageReductionPercent;
			this.RevengeDamageAddPercent = attributeData.RevengeDamageAddPercent;
			this.RevengeDamageReductionPercent = attributeData.RevengeDamageReductionPercent;
			this.PetDamageAddPercent = attributeData.PetDamageAddPercent;
			this.PetDamageReductionPercent = attributeData.PetDamageReductionPercent;
			this.BigSkillDamageAddByGreaterHpPercent = attributeData.BigSkillDamageAddByGreaterHpPercent;
			this.GreaterHpPercentBigSkillDamageAddPercent = attributeData.GreaterHpPercentBigSkillDamageAddPercent;
			this.BigSkillDamageAddByLessHpPercent = attributeData.BigSkillDamageAddByLessHpPercent;
			this.LessHpPercentBigSkillDamageAddPercent = attributeData.LessHpPercentBigSkillDamageAddPercent;
			this.ToBossDamageAddPercent = attributeData.ToBossDamageAddPercent;
			this.ShieldDamageReductionPercent = attributeData.ShieldDamageReductionPercent;
			this.PhysicalAddPercent = attributeData.PhysicalAddPercent;
			this.PhysicalReductionPercent = attributeData.PhysicalReductionPercent;
			this.FireAddPercent = attributeData.FireAddPercent;
			this.FireReductionPercent = attributeData.FireReductionPercent;
			this.ElectricAddPercent = attributeData.ElectricAddPercent;
			this.ElectricReductionPercent = attributeData.ElectricReductionPercent;
			this.IceAddPercent = attributeData.IceAddPercent;
			this.IceReductionPercent = attributeData.IceReductionPercent;
			this.TrueAddPercent = attributeData.TrueAddPercent;
			this.TrueReductionPercent = attributeData.TrueReductionPercent;
			this.KnifeAddPercent = attributeData.KnifeAddPercent;
			this.KnifeReductionPercent = attributeData.KnifeReductionPercent;
			this.FallingSwordAddPercent = attributeData.FallingSwordAddPercent;
			this.FallingSwordReductionPercent = attributeData.FallingSwordReductionPercent;
			this.SwordkeeAddPercent = attributeData.SwordkeeAddPercent;
			this.SwordkeeReductionPercent = attributeData.SwordkeeReductionPercent;
			this.ThunderAddPercent = attributeData.ThunderAddPercent;
			this.ThunderReductionPercent = attributeData.ThunderReductionPercent;
			this.FireWaveAddPercent = attributeData.FireWaveAddPercent;
			this.FireWaveReductionPercent = attributeData.FireWaveReductionPercent;
			this.BurnAddPercent = attributeData.BurnAddPercent;
			this.BurnReductionPercent = attributeData.BurnReductionPercent;
			this.IcicleAddPercent = attributeData.IcicleAddPercent;
			this.IcicleReductionPercent = attributeData.IcicleReductionPercent;
			this.FireBuffAddPercent = attributeData.FireBuffAddPercent;
			this.FireBuffReductionPercent = attributeData.FireBuffReductionPercent;
			this.PoisonAddPercent = attributeData.PoisonAddPercent;
			this.PoisonReductionPercent = attributeData.PoisonReductionPercent;
			this.CombatBaseDamagePercent = attributeData.CombatBaseDamagePercent;
			this.CounterBaseDamagePercent = attributeData.CounterBaseDamagePercent;
			this.OrdinarySkillBaseDamagePercent = attributeData.OrdinarySkillBaseDamagePercent;
			this.BigSkillBaseDamagePercent = attributeData.BigSkillBaseDamagePercent;
			this.NormalDamageAddPercent = attributeData.NormalDamageAddPercent;
			this.NormalDamageReductionPercent = attributeData.NormalDamageReductionPercent;
			this.BigSkillDamageAddPercent = attributeData.BigSkillDamageAddPercent;
			this.BigSkillDamageReductionPercent = attributeData.BigSkillDamageReductionPercent;
			this.IcicleAttackAddPercent = attributeData.IcicleAttackAddPercent;
			this.BurnAttackAddPercent = attributeData.BurnAttackAddPercent;
			this.PoisonAttackAddPercent = attributeData.PoisonAttackAddPercent;
			this.StunAttackAddPercent = attributeData.StunAttackAddPercent;
			this.ToVerdictAddPercent = attributeData.ToVerdictAddPercent;
			this.ToVerdictComboRate = attributeData.ToVerdictComboRate;
			this.IcicleOrStunDamageAddPercent = attributeData.IcicleOrStunDamageAddPercent;
			this.VulnerabilityDamageAddPercent = attributeData.VulnerabilityDamageAddPercent;
			this.BlockValue = attributeData.BlockValue;
			this.BlockTriggerPercent = attributeData.BlockTriggerPercent;
			this.RevengeRate = attributeData.RevengeRate;
			this.RevengeCount = attributeData.RevengeCount;
			this.ChainComboCount = attributeData.ChainComboCount;
			this.RecoverAddPercent = attributeData.RecoverAddPercent;
			this.RecoverReductionPercent = attributeData.RecoverReductionPercent;
			this.NoHealing = attributeData.NoHealing;
			this.VampireRate = attributeData.VampireRate;
			this.VampireAddPercent = attributeData.VampireAddPercent;
			this.CounterVampireRate = attributeData.CounterVampireRate;
			this.ReviveCount = attributeData.ReviveCount;
			this.ReviveHpPercent = attributeData.ReviveHpPercent;
			this.ThunderBaseAttackPercent = attributeData.ThunderBaseAttackPercent;
			this.ThunderCount = attributeData.ThunderCount;
			this.ThunderBuffRate = attributeData.ThunderBuffRate;
			this.ThunderSuperRate = attributeData.ThunderSuperRate;
			this.ThunderDeathRate = attributeData.ThunderDeathRate;
			this.ThunderSuperAttackPercent = attributeData.ThunderSuperAttackPercent;
			this.ThunderDeathAttackPercent = attributeData.ThunderDeathAttackPercent;
			this.ThunderDeathAttackAddPercent = attributeData.ThunderDeathAttackAddPercent;
			this.ThunderDeathAttackAddMaxCount = attributeData.ThunderDeathAttackAddMaxCount;
			this.ThunderHitByCount = attributeData.ThunderHitByCount;
			this.ThunderCumulativeDamageAddPercent = attributeData.ThunderCumulativeDamageAddPercent;
			this.ThunderShield = attributeData.ThunderShield;
			this.DurianShield = attributeData.DurianShield;
			this.IcicleSeckillRate = attributeData.IcicleSeckillRate;
			this.FrozenRate = attributeData.FrozenRate;
			this.KnifeBaseAttackPercent = attributeData.KnifeBaseAttackPercent;
			this.IsTriggerKnift = attributeData.IsTriggerKnift;
			this.KnifeCount = attributeData.KnifeCount;
			this.IsBattleStartKnife = attributeData.IsBattleStartKnife;
			this.BattleStartKnifeCount = attributeData.BattleStartKnifeCount;
			this.IsRoundStartKnife = attributeData.IsRoundStartKnife;
			this.RoundStartKnifeCount = attributeData.RoundStartKnifeCount;
			this.KnifeComboRate = attributeData.KnifeComboRate;
			this.KnifeComboCount = attributeData.KnifeComboCount;
			this.KnifeThunderRate = attributeData.KnifeThunderRate;
			this.KnifeIcicleRate = attributeData.KnifeIcicleRate;
			this.KnifeFireRate = attributeData.KnifeFireRate;
			this.KnifeSuperRate = attributeData.KnifeSuperRate;
			this.KnifeAngelRate = attributeData.KnifeAngelRate;
			this.KnifeSuperAttackPercent = attributeData.KnifeSuperAttackPercent;
			this.KnifeAngelAttackPercent = attributeData.KnifeAngelAttackPercent;
			this.KnifePoisonRate = attributeData.KnifePoisonRate;
			this.KnifeRevertRate = attributeData.KnifeRevertRate;
			this.KnifeRageRate = attributeData.KnifeRageRate;
			this.TriggerFireBuffRate = attributeData.TriggerFireBuffRate;
			this.TriggerFireBuffAddRate = attributeData.TriggerFireBuffAddRate;
			this.FireBuffAddRound = attributeData.FireBuffAddRound;
			this.FireBuffReductionRound = attributeData.FireBuffReductionRound;
			this.FireBuffMaxLayerAdd = attributeData.FireBuffMaxLayerAdd;
			this.FireAddBuffLayerAdd = attributeData.FireAddBuffLayerAdd;
			this.FireTriggerDamageBuffRate = attributeData.FireTriggerDamageBuffRate;
			this.IsFireBuffPerLayerDamageAdd = attributeData.IsFireBuffPerLayerDamageAdd;
			this.FireBuffPerLayerDamageAddPercent = attributeData.FireBuffPerLayerDamageAddPercent;
			this.FallingSwordCount = attributeData.FallingSwordCount;
			this.IsBattleStartFallingSword = attributeData.IsBattleStartFallingSword;
			this.BattleStartFallingSwordCount = attributeData.BattleStartFallingSwordCount;
			this.IsRoundStartFallingSword = attributeData.IsRoundStartFallingSword;
			this.RoundStartFallingSwordCount = attributeData.RoundStartFallingSwordCount;
			this.IsBigSkillAfterFallingSword = attributeData.IsBigSkillAfterFallingSword;
			this.BigSkillAfterFallingSwordCount = attributeData.BigSkillAfterFallingSwordCount;
			this.FallingSwordSuperRate = attributeData.FallingSwordSuperRate;
			this.FallingSwordSuperAttackPercent = attributeData.FallingSwordSuperAttackPercent;
			this.FallingSwordFireRate = attributeData.FallingSwordFireRate;
			this.ControlImmunityRate = attributeData.ControlImmunityRate;
			this.IgnoreControlImmunityRate = attributeData.IgnoreControlImmunityRate;
			this.IgnoreNormalComboRate = attributeData.IgnoreNormalComboRate;
			this.IgnoreRevengeRate = attributeData.IgnoreRevengeRate;
			this.IgnoreCritRate = attributeData.IgnoreCritRate;
			this.SwordkeeRecoverRate = attributeData.SwordkeeRecoverRate;
			this.SwordkeeSuperRate = attributeData.SwordkeeSuperRate;
			this.SwordkeeSuperAttackPercent = attributeData.SwordkeeSuperAttackPercent;
			this.SwordkeeTriggerDamageBuffRate = attributeData.SwordkeeTriggerDamageBuffRate;
			this.PurplePetDamageAddPercent = attributeData.PurplePetDamageAddPercent;
			this.OrangePetDamageAddPercent = attributeData.OrangePetDamageAddPercent;
			this.RedPetDamageAddPercent = attributeData.RedPetDamageAddPercent;
			this.RecoveryRate = attributeData.RecoveryRate;
			this.BeginShell = attributeData.BeginShell;
			this.UpgradeRegeneRate = attributeData.UpgradeRegeneRate;
			this.GetSkillCountByBegin = attributeData.GetSkillCountByBegin;
			this.GetSelectSkillCountByUpgrade = attributeData.GetSelectSkillCountByUpgrade;
			this.AttackBattleEnd = attributeData.AttackBattleEnd;
			this.DefenseBattleEnd = attributeData.DefenseBattleEnd;
			this.EffectiveTimes = attributeData.EffectiveTimes;
			this.ExpAddRate = attributeData.ExpAddRate;
			this.EventCoinAddRate = attributeData.EventCoinAddRate;
			this.BattleCoinAddRate = attributeData.BattleCoinAddRate;
			this.SlotCoinAddRate = attributeData.SlotCoinAddRate;
			this.FlippingCoinAddRate = attributeData.FlippingCoinAddRate;
			this.StaminaMax = attributeData.StaminaMax;
			this.StaminaRecoveryRate = attributeData.StaminaRecoveryRate;
			this.PrivilegeGemRate = attributeData.PrivilegeGemRate;
			this.IdleResAddRate = attributeData.IdleResAddRate;
			this.AxeRecoverRate = attributeData.AxeRecoverRate;
			this.HangTimeMax = attributeData.HangTimeMax;
			this.SmallLuckyRate = attributeData.SmallLuckyRate;
			this.BigLuckyRate = attributeData.BigLuckyRate;
			this.OrangeSkillRate = attributeData.OrangeSkillRate;
			this.RedSkillRate = attributeData.RedSkillRate;
		}

		public void ConvertBaseData()
		{
			this.Attack = this.GetAttack();
			this.AttackPercent = FP._0;
			this.GlobalAttackPercent = FP._0;
			this.HPMax = this.GetHpMax();
			this.HPMaxPercent = FP._0;
			this.GlobalHPMaxPercent = FP._0;
			this.Defence = this.GetDefence();
			this.DefencePercent = FP._0;
			this.GlobalDefencePercent = FP._0;
		}

		public void MergeAttributes(List<MergeAttributeData> datas, bool isReverse = false)
		{
			if (datas == null)
			{
				return;
			}
			for (int i = 0; i < datas.Count; i++)
			{
				this.MergeAttribute(datas[i], isReverse);
			}
		}

		public void MergeAttribute(MergeAttributeData data, bool isReverse = false)
		{
			if (data == null)
			{
				return;
			}
			string header = data.Header;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(header);
			if (num <= 2276992146U)
			{
				if (num <= 902974996U)
				{
					if (num <= 484614851U)
					{
						if (num <= 324772136U)
						{
							if (num <= 233388682U)
							{
								if (num <= 119853533U)
								{
									if (num != 50686908U)
									{
										if (num != 89668488U)
										{
											if (num == 119853533U)
											{
												if (header == "BigSkillDamageAdd%")
												{
													FP fp = data.Value / 100;
													this.BigSkillDamageAddPercent += ((!isReverse) ? fp : (-fp));
													return;
												}
											}
										}
										else if (header == "StaminaMax")
										{
											this.StaminaMax += ((!isReverse) ? data.Value : (-data.Value));
											return;
										}
									}
									else if (header == "FireWaveReduction%")
									{
										FP fp2 = data.Value / 100;
										this.FireWaveReductionPercent += ((!isReverse) ? fp2 : (-fp2));
										return;
									}
								}
								else if (num != 132455196U)
								{
									if (num != 215219850U)
									{
										if (num == 233388682U)
										{
											if (header == "NormalComboCritAdd%")
											{
												FP fp3 = data.Value / 100;
												this.NormalComboCritAddPercent += ((!isReverse) ? fp3 : (-fp3));
												return;
											}
										}
									}
									else if (header == "EventCoinAddRate%")
									{
										FP fp4 = data.Value / 100;
										this.EventCoinAddRate += ((!isReverse) ? fp4 : (-fp4));
										return;
									}
								}
								else if (header == "ThunderDeathAttack%")
								{
									FP fp5 = data.Value / 100;
									this.ThunderDeathAttackPercent += ((!isReverse) ? fp5 : (-fp5));
									return;
								}
							}
							else if (num <= 295522859U)
							{
								if (num != 238890685U)
								{
									if (num != 292982194U)
									{
										if (num == 295522859U)
										{
											if (header == "VampireAdd%")
											{
												FP fp6 = data.Value / 100;
												this.VampireAddPercent += ((!isReverse) ? fp6 : (-fp6));
												return;
											}
										}
									}
									else if (header == "PhysicalAdd%")
									{
										FP fp7 = data.Value / 100;
										this.PhysicalAddPercent += ((!isReverse) ? fp7 : (-fp7));
										return;
									}
								}
								else if (header == "ReviveHpRate%")
								{
									FP fp8 = data.Value / 100;
									this.ReviveHpPercent += ((!isReverse) ? fp8 : (-fp8));
									return;
								}
							}
							else if (num != 300985230U)
							{
								if (num != 314884380U)
								{
									if (num == 324772136U)
									{
										if (header == "Attack%")
										{
											FP fp9 = data.Value / 100;
											this.AttackPercent += ((!isReverse) ? fp9 : (-fp9));
											return;
										}
									}
								}
								else if (header == "IceReduction%")
								{
									FP fp10 = data.Value / 100;
									this.IceReductionPercent += ((!isReverse) ? fp10 : (-fp10));
									return;
								}
							}
							else if (header == "KnifeBaseAttack%")
							{
								FP fp11 = data.Value / 100;
								this.KnifeBaseAttackPercent += ((!isReverse) ? fp11 : (-fp11));
								return;
							}
						}
						else if (num <= 416793316U)
						{
							if (num <= 379785653U)
							{
								if (num != 367905890U)
								{
									if (num != 377638745U)
									{
										if (num == 379785653U)
										{
											if (header == "CritValue%")
											{
												FP fp12 = data.Value / 100;
												this.CritValue += ((!isReverse) ? fp12 : (-fp12));
												return;
											}
										}
									}
									else if (header == "BattleStartKnifeCount")
									{
										FP value = data.Value;
										this.BattleStartKnifeCount += ((!isReverse) ? value : (-value));
										return;
									}
								}
								else if (header == "RevengeDamageAdd%")
								{
									FP fp13 = data.Value / 100;
									this.RevengeDamageAddPercent += ((!isReverse) ? fp13 : (-fp13));
									return;
								}
							}
							else if (num != 391021011U)
							{
								if (num != 394635115U)
								{
									if (num == 416793316U)
									{
										if (header == "FireBuffReduction%")
										{
											FP fp14 = data.Value / 100;
											this.FireBuffReductionPercent += ((!isReverse) ? fp14 : (-fp14));
											return;
										}
									}
								}
								else if (header == "BlockValue")
								{
									FP value2 = data.Value;
									this.BlockValue += ((!isReverse) ? value2 : (-value2));
									return;
								}
							}
							else if (header == "BigSkillAfterFallingSwordCount")
							{
								FP value3 = data.Value;
								this.BigSkillAfterFallingSwordCount += ((!isReverse) ? value3 : (-value3));
								return;
							}
						}
						else if (num <= 425664211U)
						{
							if (num != 419922387U)
							{
								if (num != 423632796U)
								{
									if (num == 425664211U)
									{
										if (header == "NormalComboCount")
										{
											FP value4 = data.Value;
											this.NormalComboCount += ((!isReverse) ? value4 : (-value4));
											return;
										}
									}
								}
								else if (header == "UpgradeRegenerate%")
								{
									FP fp15 = data.Value / 100;
									this.UpgradeRegeneRate += ((!isReverse) ? fp15 : (-fp15));
									return;
								}
							}
							else if (header == "ThunderSuperAttack%")
							{
								FP fp16 = data.Value / 100;
								this.ThunderSuperAttackPercent += ((!isReverse) ? fp16 : (-fp16));
								return;
							}
						}
						else if (num != 436237490U)
						{
							if (num != 475898920U)
							{
								if (num == 484614851U)
								{
									if (header == "Defence")
									{
										this.Defence += ((!isReverse) ? data.Value : (-data.Value));
										return;
									}
								}
							}
							else if (header == "ThunderCount")
							{
								this.ThunderCount += ((!isReverse) ? data.Value : (-data.Value));
								return;
							}
						}
						else if (header == "IsNormalCritAddValue")
						{
							FP value5 = data.Value;
							this.IsNormalCritAddValue += ((!isReverse) ? value5 : (-value5));
							return;
						}
					}
					else if (num <= 699163305U)
					{
						if (num <= 572422636U)
						{
							if (num <= 518729377U)
							{
								if (num != 491629968U)
								{
									if (num != 495698666U)
									{
										if (num == 518729377U)
										{
											if (header == "IgnoreControlImmunity%")
											{
												FP fp17 = data.Value / 100;
												this.IgnoreControlImmunityRate += ((!isReverse) ? fp17 : (-fp17));
												return;
											}
										}
									}
									else if (header == "BurnAdd%")
									{
										FP fp18 = data.Value / 100;
										this.BurnAddPercent += ((!isReverse) ? fp18 : (-fp18));
										return;
									}
								}
								else if (header == "Revenge%")
								{
									FP fp19 = data.Value / 100;
									this.RevengeRate += ((!isReverse) ? fp19 : (-fp19));
									return;
								}
							}
							else if (num != 539726319U)
							{
								if (num != 564899428U)
								{
									if (num == 572422636U)
									{
										if (header == "SlotCoinAddRate%")
										{
											FP fp20 = data.Value / 100;
											this.SlotCoinAddRate += ((!isReverse) ? fp20 : (-fp20));
											return;
										}
									}
								}
								else if (header == "KnifeSuper%")
								{
									FP fp21 = data.Value / 100;
									this.KnifeSuperRate += ((!isReverse) ? fp21 : (-fp21));
									return;
								}
							}
							else if (header == "SwordkeeSuper%")
							{
								FP fp22 = data.Value / 100;
								this.SwordkeeSuperRate += ((!isReverse) ? fp22 : (-fp22));
								return;
							}
						}
						else if (num <= 603148018U)
						{
							if (num != 593219697U)
							{
								if (num != 602720057U)
								{
									if (num == 603148018U)
									{
										if (header == "Frozen%")
										{
											FP fp23 = data.Value / 100;
											this.FrozenRate += ((!isReverse) ? fp23 : (-fp23));
											return;
										}
									}
								}
								else if (header == "DefenseBattleEnd%")
								{
									this.DefenseBattleEnd += ((!isReverse) ? data.Value : (-data.Value));
									return;
								}
							}
							else if (header == "FireReduction%")
							{
								FP fp24 = data.Value / 100;
								this.FireReductionPercent += ((!isReverse) ? fp24 : (-fp24));
								return;
							}
						}
						else if (num != 625009628U)
						{
							if (num != 635836579U)
							{
								if (num == 699163305U)
								{
									if (header == "FallingSwordReduction%")
									{
										FP fp25 = data.Value / 100;
										this.FallingSwordReductionPercent += ((!isReverse) ? fp25 : (-fp25));
										return;
									}
								}
							}
							else if (header == "ToVerdictAdd%")
							{
								FP fp26 = data.Value / 100;
								this.ToVerdictAddPercent += ((!isReverse) ? fp26 : (-fp26));
								return;
							}
						}
						else if (header == "DamageReduction%")
						{
							FP fp27 = data.Value / 100;
							this.DamageReductionPercent += ((!isReverse) ? fp27 : (-fp27));
							return;
						}
					}
					else if (num <= 796554613U)
					{
						if (num <= 745524681U)
						{
							if (num != 699525643U)
							{
								if (num != 726220194U)
								{
									if (num == 745524681U)
									{
										if (header == "SkillDamageAdd%")
										{
											FP fp28 = data.Value / 100;
											this.SkillDamageAddPercent += ((!isReverse) ? fp28 : (-fp28));
											return;
										}
									}
								}
								else if (header == "ElectricReduction%")
								{
									FP fp29 = data.Value / 100;
									this.ElectricReductionPercent += ((!isReverse) ? fp29 : (-fp29));
									return;
								}
							}
							else if (header == "AttackBattleEnd%")
							{
								this.AttackBattleEnd += ((!isReverse) ? data.Value : (-data.Value));
								return;
							}
						}
						else if (num != 746483589U)
						{
							if (num != 767334412U)
							{
								if (num == 796554613U)
								{
									if (header == "TrueReduction%")
									{
										FP fp30 = data.Value / 100;
										this.TrueReductionPercent += ((!isReverse) ? fp30 : (-fp30));
										return;
									}
								}
							}
							else if (header == "ExpAddRate%")
							{
								FP fp31 = data.Value / 100;
								this.ExpAddRate += ((!isReverse) ? fp31 : (-fp31));
								return;
							}
						}
						else if (header == "IgnoreNormalCombo%")
						{
							FP fp32 = data.Value / 100;
							this.IgnoreNormalComboRate += ((!isReverse) ? fp32 : (-fp32));
							return;
						}
					}
					else if (num <= 830651559U)
					{
						if (num != 805076035U)
						{
							if (num != 815484847U)
							{
								if (num == 830651559U)
								{
									if (header == "ThunderBaseAttack%")
									{
										FP fp33 = data.Value / 100;
										this.ThunderBaseAttackPercent += ((!isReverse) ? fp33 : (-fp33));
										return;
									}
								}
							}
							else if (header == "NormalDamageReduction%")
							{
								FP fp34 = data.Value / 100;
								this.NormalDamageReductionPercent += ((!isReverse) ? fp34 : (-fp34));
								return;
							}
						}
						else if (header == "RecoverReduction%")
						{
							FP fp35 = data.Value / 100;
							this.RecoverReductionPercent += ((!isReverse) ? fp35 : (-fp35));
							return;
						}
					}
					else if (num <= 843334290U)
					{
						if (num != 832593793U)
						{
							if (num == 843334290U)
							{
								if (header == "PrivilegeGem%")
								{
									FP fp36 = data.Value / 100;
									this.PrivilegeGemRate += ((!isReverse) ? fp36 : (-fp36));
									return;
								}
							}
						}
						else if (header == "PetDamageAdd%")
						{
							FP fp37 = data.Value / 100;
							this.PetDamageAddPercent += ((!isReverse) ? fp37 : (-fp37));
							return;
						}
					}
					else if (num != 877229825U)
					{
						if (num == 902974996U)
						{
							if (header == "AxeRecover%")
							{
								FP fp38 = data.Value / 100;
								this.AxeRecoverRate += ((!isReverse) ? fp38 : (-fp38));
								return;
							}
						}
					}
					else if (header == "SmallLuckyRate%")
					{
						FP fp39 = data.Value / 100;
						this.SmallLuckyRate += ((!isReverse) ? fp39 : (-fp39));
						return;
					}
				}
				else if (num <= 1715523827U)
				{
					if (num <= 1479435889U)
					{
						if (num <= 1143770545U)
						{
							if (num <= 1009172047U)
							{
								if (num != 906096703U)
								{
									if (num != 955487728U)
									{
										if (num == 1009172047U)
										{
											if (header == "KnifeThunder%")
											{
												FP fp40 = data.Value / 100;
												this.KnifeThunderRate += ((!isReverse) ? fp40 : (-fp40));
												return;
											}
										}
									}
									else if (header == "BeginShell")
									{
										this.BeginShell += ((!isReverse) ? data.Value : (-data.Value));
										return;
									}
								}
								else if (header == "StunAttackAdd%")
								{
									FP fp41 = data.Value / 100;
									this.StunAttackAddPercent += ((!isReverse) ? fp41 : (-fp41));
									return;
								}
							}
							else if (num != 1069607449U)
							{
								if (num != 1093169364U)
								{
									if (num == 1143770545U)
									{
										if (header == "RecoverAdd%")
										{
											FP fp42 = data.Value / 100;
											this.RecoverAddPercent += ((!isReverse) ? fp42 : (-fp42));
											return;
										}
									}
								}
								else if (header == "IgnoreDefence%")
								{
									FP fp43 = data.Value / 100;
									this.IgnoreDefencePercent += ((!isReverse) ? fp43 : (-fp43));
									return;
								}
							}
							else if (header == "HPMax")
							{
								FP hpMax = this.GetHpMax();
								this.HPMax += ((!isReverse) ? data.Value : (-data.Value));
								Action<FP> onHpMaxUpdate = this.m_onHpMaxUpdate;
								if (onHpMaxUpdate == null)
								{
									return;
								}
								onHpMaxUpdate(hpMax);
								return;
							}
						}
						else if (num <= 1296369593U)
						{
							if (num != 1172188297U)
							{
								if (num != 1271254876U)
								{
									if (num == 1296369593U)
									{
										if (header == "ThunderReduction%")
										{
											FP fp44 = data.Value / 100;
											this.ThunderReductionPercent += ((!isReverse) ? fp44 : (-fp44));
											return;
										}
									}
								}
								else if (header == "IcicleAdd%")
								{
									FP fp45 = data.Value / 100;
									this.IcicleAddPercent += ((!isReverse) ? fp45 : (-fp45));
									return;
								}
							}
							else if (header == "LessHpPercentBigSkillDamageAdd%")
							{
								FP fp46 = data.Value / 100;
								this.LessHpPercentBigSkillDamageAddPercent += ((!isReverse) ? fp46 : (-fp46));
								return;
							}
						}
						else if (num != 1436798390U)
						{
							if (num != 1442762743U)
							{
								if (num == 1479435889U)
								{
									if (header == "ThunderCumulativeDamageAdd%")
									{
										FP fp47 = data.Value / 100;
										this.ThunderCumulativeDamageAddPercent += ((!isReverse) ? fp47 : (-fp47));
										return;
									}
								}
							}
							else if (header == "ThunderAdd%")
							{
								FP fp48 = data.Value / 100;
								this.ThunderAddPercent += ((!isReverse) ? fp48 : (-fp48));
								return;
							}
						}
						else if (header == "IgnoreRevenge%")
						{
							FP fp49 = data.Value / 100;
							this.IgnoreRevengeRate += ((!isReverse) ? fp49 : (-fp49));
							return;
						}
					}
					else if (num <= 1590063122U)
					{
						if (num <= 1520265518U)
						{
							if (num != 1500242430U)
							{
								if (num != 1515394258U)
								{
									if (num == 1520265518U)
									{
										if (header == "CounterVampire%")
										{
											FP fp50 = data.Value / 100;
											this.CounterVampireRate += ((!isReverse) ? fp50 : (-fp50));
											return;
										}
									}
								}
								else if (header == "GreaterHpPercentBigSkillDamageAdd%")
								{
									FP fp51 = data.Value / 100;
									this.GreaterHpPercentBigSkillDamageAddPercent += ((!isReverse) ? fp51 : (-fp51));
									return;
								}
							}
							else if (header == "GetSelectSkillCountByUpgrade")
							{
								this.GetSelectSkillCountByUpgrade += ((!isReverse) ? data.Value : (-data.Value));
								return;
							}
						}
						else if (num != 1544985852U)
						{
							if (num != 1555836984U)
							{
								if (num == 1590063122U)
								{
									if (header == "Defence%")
									{
										FP fp52 = data.Value / 100;
										this.DefencePercent += ((!isReverse) ? fp52 : (-fp52));
										return;
									}
								}
							}
							else if (header == "CombatBaseDamage%")
							{
								FP fp53 = data.Value / 100;
								this.CombatBaseDamagePercent += ((!isReverse) ? fp53 : (-fp53));
								return;
							}
						}
						else if (header == "NoHealing")
						{
							FP value6 = data.Value;
							this.NoHealing += ((!isReverse) ? value6 : (-value6));
							return;
						}
					}
					else if (num <= 1693780359U)
					{
						if (num != 1604840396U)
						{
							if (num != 1668353200U)
							{
								if (num == 1693780359U)
								{
									if (header == "ThunderSuper%")
									{
										FP fp54 = data.Value / 100;
										this.ThunderSuperRate += ((!isReverse) ? fp54 : (-fp54));
										return;
									}
								}
							}
							else if (header == "BattleStartFallingSwordCount")
							{
								FP value7 = data.Value;
								this.BattleStartFallingSwordCount += ((!isReverse) ? value7 : (-value7));
								return;
							}
						}
						else if (header == "RevengeDamageReduction%")
						{
							FP fp55 = data.Value / 100;
							this.RevengeDamageReductionPercent += ((!isReverse) ? fp55 : (-fp55));
							return;
						}
					}
					else if (num != 1697685538U)
					{
						if (num != 1708397026U)
						{
							if (num == 1715523827U)
							{
								if (header == "PetDamageReduction%")
								{
									FP fp56 = data.Value / 100;
									this.PetDamageReductionPercent += ((!isReverse) ? fp56 : (-fp56));
									return;
								}
							}
						}
						else if (header == "IsBattleStartKnife")
						{
							FP value8 = data.Value;
							this.IsBattleStartKnife += ((!isReverse) ? value8 : (-value8));
							return;
						}
					}
					else if (header == "WeaponCritRate%")
					{
						FP fp57 = data.Value / 100;
						this.WeaponCritRate += ((!isReverse) ? fp57 : (-fp57));
						return;
					}
				}
				else if (num <= 1984268305U)
				{
					if (num <= 1830125735U)
					{
						if (num <= 1774190526U)
						{
							if (num != 1736316382U)
							{
								if (num != 1738070863U)
								{
									if (num == 1774190526U)
									{
										if (header == "HangTimeMax")
										{
											this.HangTimeMax += ((!isReverse) ? data.Value : (-data.Value));
											return;
										}
									}
								}
								else if (header == "LegacyPower")
								{
									Action<FP> onLegacyPowerUpdate = this.m_onLegacyPowerUpdate;
									if (onLegacyPowerUpdate == null)
									{
										return;
									}
									onLegacyPowerUpdate((!isReverse) ? data.Value : (-data.Value));
									return;
								}
							}
							else if (header == "CritValueReduction%")
							{
								FP fp58 = data.Value / 100;
								this.CritValueReduction += ((!isReverse) ? fp58 : (-fp58));
								return;
							}
						}
						else if (num != 1788877211U)
						{
							if (num != 1811507796U)
							{
								if (num == 1830125735U)
								{
									if (header == "FireBuffReductionRound")
									{
										FP value9 = data.Value;
										this.FireBuffReductionRound += ((!isReverse) ? value9 : (-value9));
										return;
									}
								}
							}
							else if (header == "Miss%")
							{
								FP fp59 = data.Value / 100;
								this.Miss += ((!isReverse) ? fp59 : (-fp59));
								return;
							}
						}
						else if (header == "UseFireBuffPerLayerDamageAdd")
						{
							FP value10 = data.Value;
							this.IsFireBuffPerLayerDamageAdd += ((!isReverse) ? value10 : (-value10));
							return;
						}
					}
					else if (num <= 1883942262U)
					{
						if (num != 1851613821U)
						{
							if (num != 1856590237U)
							{
								if (num == 1883942262U)
								{
									if (header == "ComboDamageAdd%")
									{
										FP fp60 = data.Value / 100;
										this.ComboDamageAddPercent += ((!isReverse) ? fp60 : (-fp60));
										return;
									}
								}
							}
							else if (header == "OrangeSkillRate%")
							{
								FP fp61 = data.Value / 100;
								this.OrangeSkillRate += ((!isReverse) ? fp61 : (-fp61));
								return;
							}
						}
						else if (header == "GlobalDefence%")
						{
							FP fp62 = data.Value / 100;
							this.GlobalDefencePercent += ((!isReverse) ? fp62 : (-fp62));
							return;
						}
					}
					else if (num != 1903957402U)
					{
						if (num != 1940855490U)
						{
							if (num == 1984268305U)
							{
								if (header == "BigSkillBaseDamage%")
								{
									FP fp63 = data.Value / 100;
									this.BigSkillBaseDamagePercent += ((!isReverse) ? fp63 : (-fp63));
									return;
								}
							}
						}
						else if (header == "ToVerdictComboRate%")
						{
							FP fp64 = data.Value / 100;
							this.ToVerdictComboRate += ((!isReverse) ? fp64 : (-fp64));
							return;
						}
					}
					else if (header == "IgnoreCrit%")
					{
						FP fp65 = data.Value / 100;
						this.IgnoreCritRate += ((!isReverse) ? fp65 : (-fp65));
						return;
					}
				}
				else if (num <= 2092906184U)
				{
					if (num <= 2041165527U)
					{
						if (num != 2026422945U)
						{
							if (num != 2039097040U)
							{
								if (num == 2041165527U)
								{
									if (header == "ThunderDeathAttackAdd%")
									{
										FP fp66 = data.Value / 100;
										this.ThunderDeathAttackAddPercent += ((!isReverse) ? fp66 : (-fp66));
										return;
									}
								}
							}
							else if (header == "Shield")
							{
								FP fp67 = ((!isReverse) ? data.Value : (-data.Value));
								Action<FP> onShieldValueUpdate = this.m_onShieldValueUpdate;
								if (onShieldValueUpdate == null)
								{
									return;
								}
								onShieldValueUpdate(fp67);
								return;
							}
						}
						else if (header == "FireBuffAddRound")
						{
							FP value11 = data.Value;
							this.FireBuffAddRound += ((!isReverse) ? value11 : (-value11));
							return;
						}
					}
					else if (num != 2041777376U)
					{
						if (num != 2073327634U)
						{
							if (num == 2092906184U)
							{
								if (header == "ElectricAdd%")
								{
									FP fp68 = data.Value / 100;
									this.ElectricAddPercent += ((!isReverse) ? fp68 : (-fp68));
									return;
								}
							}
						}
						else if (header == "RedPetDamageAdd%")
						{
							FP fp69 = data.Value / 100;
							this.RedPetDamageAddPercent += ((!isReverse) ? fp69 : (-fp69));
							return;
						}
					}
					else if (header == "ComboDamageReduction%")
					{
						FP fp70 = data.Value / 100;
						this.ComboDamageReductionPercent += ((!isReverse) ? fp70 : (-fp70));
						return;
					}
				}
				else if (num <= 2152693963U)
				{
					if (num != 2100989274U)
					{
						if (num != 2112414679U)
						{
							if (num == 2152693963U)
							{
								if (header == "SwordkeeSuperAttack%")
								{
									FP fp71 = data.Value / 100;
									this.SwordkeeSuperAttackPercent += ((!isReverse) ? fp71 : (-fp71));
									return;
								}
							}
						}
						else if (header == "FallingSwordSuper%")
						{
							FP fp72 = data.Value / 100;
							this.FallingSwordSuperRate += ((!isReverse) ? fp72 : (-fp72));
							return;
						}
					}
					else if (header == "SwordkeeTriggerDamageBuff%")
					{
						FP value12 = data.Value;
						this.SwordkeeTriggerDamageBuffRate += ((!isReverse) ? value12 : (-value12));
						return;
					}
				}
				else if (num <= 2168401206U)
				{
					if (num != 2159173842U)
					{
						if (num == 2168401206U)
						{
							if (header == "KnifeReduction%")
							{
								FP fp73 = data.Value / 100;
								this.KnifeReductionPercent += ((!isReverse) ? fp73 : (-fp73));
								return;
							}
						}
					}
					else if (header == "DamageAdd%")
					{
						FP fp74 = data.Value / 100;
						this.DamageAddPercent += ((!isReverse) ? fp74 : (-fp74));
						return;
					}
				}
				else if (num != 2234284006U)
				{
					if (num == 2276992146U)
					{
						if (header == "KnifeRage%")
						{
							FP fp75 = data.Value / 100;
							this.KnifeRageRate += ((!isReverse) ? fp75 : (-fp75));
							return;
						}
					}
				}
				else if (header == "IcicleReduction%")
				{
					FP fp76 = data.Value / 100;
					this.IcicleReductionPercent += ((!isReverse) ? fp76 : (-fp76));
					return;
				}
			}
			else if (num <= 2943015215U)
			{
				if (num <= 2661224905U)
				{
					if (num <= 2484935950U)
					{
						if (num <= 2352767799U)
						{
							if (num <= 2339590805U)
							{
								if (num != 2289848740U)
								{
									if (num != 2337141094U)
									{
										if (num == 2339590805U)
										{
											if (header == "KnifePoison%")
											{
												FP fp77 = data.Value / 100;
												this.KnifePoisonRate += ((!isReverse) ? fp77 : (-fp77));
												return;
											}
										}
									}
									else if (header == "SwordkeeRecover%")
									{
										FP fp78 = data.Value / 100;
										this.SwordkeeRecoverRate += ((!isReverse) ? fp78 : (-fp78));
										return;
									}
								}
								else if (header == "ThunderShield")
								{
									FP value13 = data.Value;
									this.ThunderShield += ((!isReverse) ? value13 : (-value13));
									return;
								}
							}
							else if (num != 2343121693U)
							{
								if (num != 2343816346U)
								{
									if (num == 2352767799U)
									{
										if (header == "FireBuffPerLayerDamageAdd%")
										{
											FP fp79 = data.Value / 100;
											this.FireBuffPerLayerDamageAddPercent += ((!isReverse) ? fp79 : (-fp79));
											return;
										}
									}
								}
								else if (header == "ToBossDamageAdd%")
								{
									FP fp80 = data.Value / 100;
									this.ToBossDamageAddPercent += ((!isReverse) ? fp80 : (-fp80));
									return;
								}
							}
							else if (header == "Attack")
							{
								this.Attack += ((!isReverse) ? data.Value : (-data.Value));
								return;
							}
						}
						else if (num <= 2400834834U)
						{
							if (num != 2377789678U)
							{
								if (num != 2398566297U)
								{
									if (num == 2400834834U)
									{
										if (header == "IceAdd%")
										{
											FP fp81 = data.Value / 100;
											this.IceAddPercent += ((!isReverse) ? fp81 : (-fp81));
											return;
										}
									}
								}
								else if (header == "PoisonReduction%")
								{
									FP fp82 = data.Value / 100;
									this.PoisonReductionPercent += ((!isReverse) ? fp82 : (-fp82));
									return;
								}
							}
							else if (header == "FireBuffMaxLayerAdd")
							{
								FP value14 = data.Value;
								this.FireBuffMaxLayerAdd += ((!isReverse) ? value14 : (-value14));
								return;
							}
						}
						else if (num != 2429542940U)
						{
							if (num != 2463276959U)
							{
								if (num == 2484935950U)
								{
									if (header == "BurnAttackAdd%")
									{
										FP fp83 = data.Value / 100;
										this.BurnAttackAddPercent += ((!isReverse) ? fp83 : (-fp83));
										return;
									}
								}
							}
							else if (header == "OrdinarySkillBaseDamage%")
							{
								FP fp84 = data.Value / 100;
								this.OrdinarySkillBaseDamagePercent += ((!isReverse) ? fp84 : (-fp84));
								return;
							}
						}
						else if (header == "KnifeSuperAttack%")
						{
							FP fp85 = data.Value / 100;
							this.KnifeSuperAttackPercent += ((!isReverse) ? fp85 : (-fp85));
							return;
						}
					}
					else if (num <= 2556341719U)
					{
						if (num <= 2528763676U)
						{
							if (num != 2498656317U)
							{
								if (num != 2504656766U)
								{
									if (num == 2528763676U)
									{
										if (header == "IsSkillCritRateCritAddValue")
										{
											FP value15 = data.Value;
											this.IsSkillCritAddValue += ((!isReverse) ? value15 : (-value15));
											return;
										}
									}
								}
								else if (header == "VampireRate%")
								{
									FP fp86 = data.Value / 100;
									this.VampireRate += ((!isReverse) ? fp86 : (-fp86));
									return;
								}
							}
							else if (header == "LegacyPowerAdd%")
							{
								FP fp87 = data.Value / 100;
								this.LegacyPowerAddPercent += ((!isReverse) ? fp87 : (-fp87));
								return;
							}
						}
						else if (num != 2543168442U)
						{
							if (num != 2556304615U)
							{
								if (num == 2556341719U)
								{
									if (header == "PoisonAdd%")
									{
										FP fp88 = data.Value / 100;
										this.PoisonAddPercent += ((!isReverse) ? fp88 : (-fp88));
										return;
									}
								}
							}
							else if (header == "GlobalHPMax%")
							{
								FP fp89 = data.Value / 100;
								this.GlobalHPMaxPercent += ((!isReverse) ? fp89 : (-fp89));
								return;
							}
						}
						else if (header == "CounterBaseDamage%")
						{
							FP fp90 = data.Value / 100;
							this.CounterBaseDamagePercent += ((!isReverse) ? fp90 : (-fp90));
							return;
						}
					}
					else if (num <= 2566720253U)
					{
						if (num != 2561719412U)
						{
							if (num != 2561956782U)
							{
								if (num == 2566720253U)
								{
									if (header == "NoEnergyRecovery")
									{
										FP value16 = data.Value;
										this.NoEnergyRecovery += ((!isReverse) ? value16 : (-value16));
										return;
									}
								}
							}
							else if (header == "IsRoundStartKnife")
							{
								FP value17 = data.Value;
								this.IsRoundStartKnife += ((!isReverse) ? value17 : (-value17));
								return;
							}
						}
						else if (header == "HPMax%")
						{
							FP hpMax2 = this.GetHpMax();
							FP fp91 = data.Value / 100;
							this.HPMaxPercent += ((!isReverse) ? fp91 : (-fp91));
							Action<FP> onHpMaxUpdate2 = this.m_onHpMaxUpdate;
							if (onHpMaxUpdate2 == null)
							{
								return;
							}
							onHpMaxUpdate2(hpMax2);
							return;
						}
					}
					else if (num != 2599398015U)
					{
						if (num != 2630197791U)
						{
							if (num == 2661224905U)
							{
								if (header == "DurianShield")
								{
									FP value18 = data.Value;
									this.DurianShield += ((!isReverse) ? value18 : (-value18));
									return;
								}
							}
						}
						else if (header == "ReviveCount")
						{
							FP value19 = data.Value;
							this.ReviveCount += ((!isReverse) ? value19 : (-value19));
							return;
						}
					}
					else if (header == "FireAdd%")
					{
						FP fp92 = data.Value / 100;
						this.FireAddPercent += ((!isReverse) ? fp92 : (-fp92));
						return;
					}
				}
				else if (num <= 2815638266U)
				{
					if (num <= 2750999576U)
					{
						if (num <= 2694673755U)
						{
							if (num != 2667876646U)
							{
								if (num != 2691652096U)
								{
									if (num == 2694673755U)
									{
										if (header == "KnifeRevert%")
										{
											FP fp93 = data.Value / 100;
											this.KnifeRevertRate += ((!isReverse) ? fp93 : (-fp93));
											return;
										}
									}
								}
								else if (header == "EffectiveTimes")
								{
									this.EffectiveTimes += ((!isReverse) ? data.Value : (-data.Value));
									return;
								}
							}
							else if (header == "VulnerabilityDamageAdd%")
							{
								FP fp94 = data.Value / 100;
								this.VulnerabilityDamageAddPercent += ((!isReverse) ? fp94 : (-fp94));
								return;
							}
						}
						else if (num != 2729249265U)
						{
							if (num != 2733999587U)
							{
								if (num == 2750999576U)
								{
									if (header == "FallingSwordCount")
									{
										FP value20 = data.Value;
										this.FallingSwordCount += ((!isReverse) ? value20 : (-value20));
										return;
									}
								}
							}
							else if (header == "IsTriggerKnift")
							{
								FP value21 = data.Value;
								this.IsTriggerKnift += ((!isReverse) ? value21 : (-value21));
								return;
							}
						}
						else if (header == "SwordkeeReduction%")
						{
							FP fp95 = data.Value / 100;
							this.SwordkeeReductionPercent += ((!isReverse) ? fp95 : (-fp95));
							return;
						}
					}
					else if (num <= 2781259547U)
					{
						if (num != 2768259929U)
						{
							if (num != 2773623445U)
							{
								if (num == 2781259547U)
								{
									if (header == "NormalCritRate%")
									{
										FP fp96 = data.Value / 100;
										this.NormalCritRate += ((!isReverse) ? fp96 : (-fp96));
										return;
									}
								}
							}
							else if (header == "TriggerFireBuff%")
							{
								FP fp97 = data.Value / 100;
								this.TriggerFireBuffRate += ((!isReverse) ? fp97 : (-fp97));
								return;
							}
						}
						else if (header == "IsBattleStartFallingSword")
						{
							FP value22 = data.Value;
							this.IsBattleStartFallingSword += ((!isReverse) ? value22 : (-value22));
							return;
						}
					}
					else if (num != 2783683546U)
					{
						if (num != 2799255944U)
						{
							if (num == 2815638266U)
							{
								if (header == "IcicleSeckill%")
								{
									FP fp98 = data.Value / 100;
									this.IcicleSeckillRate += ((!isReverse) ? fp98 : (-fp98));
									return;
								}
							}
						}
						else if (header == "BigLuckyRate%")
						{
							FP fp99 = data.Value / 100;
							this.BigLuckyRate += ((!isReverse) ? fp99 : (-fp99));
							return;
						}
					}
					else if (header == "FireTriggerDamageBuff%")
					{
						FP value23 = data.Value;
						this.FireTriggerDamageBuffRate += ((!isReverse) ? value23 : (-value23));
						return;
					}
				}
				else if (num <= 2858588092U)
				{
					if (num <= 2842144906U)
					{
						if (num != 2821276129U)
						{
							if (num != 2825727063U)
							{
								if (num == 2842144906U)
								{
									if (header == "FireBuffAdd%")
									{
										FP fp100 = data.Value / 100;
										this.FireBuffAddPercent += ((!isReverse) ? fp100 : (-fp100));
										return;
									}
								}
							}
							else if (header == "SkillCritRate%")
							{
								FP fp101 = data.Value / 100;
								this.SkillCritRate += ((!isReverse) ? fp101 : (-fp101));
								return;
							}
						}
						else if (header == "KnifeFire%")
						{
							FP fp102 = data.Value / 100;
							this.KnifeFireRate += ((!isReverse) ? fp102 : (-fp102));
							return;
						}
					}
					else if (num != 2844848360U)
					{
						if (num != 2849879225U)
						{
							if (num == 2858588092U)
							{
								if (header == "PhysicalReduction%")
								{
									FP fp103 = data.Value / 100;
									this.PhysicalReductionPercent += ((!isReverse) ? fp103 : (-fp103));
									return;
								}
							}
						}
						else if (header == "Recovery%")
						{
							FP fp104 = data.Value / 100;
							this.RecoveryRate += ((!isReverse) ? fp104 : (-fp104));
							return;
						}
					}
					else if (header == "KnifeIcicle%")
					{
						FP fp105 = data.Value / 100;
						this.KnifeIcicleRate += ((!isReverse) ? fp105 : (-fp105));
						return;
					}
				}
				else if (num <= 2913575652U)
				{
					if (num != 2882090959U)
					{
						if (num != 2907078795U)
						{
							if (num == 2913575652U)
							{
								if (header == "TriggerFireBuffAdd%")
								{
									FP fp106 = data.Value / 100;
									this.TriggerFireBuffAddRate += ((!isReverse) ? fp106 : (-fp106));
									return;
								}
							}
						}
						else if (header == "NormalCombo%")
						{
							FP fp107 = data.Value / 100;
							this.NormalComboRate += ((!isReverse) ? fp107 : (-fp107));
							return;
						}
					}
					else if (header == "PoisonAttackAdd%")
					{
						FP fp108 = data.Value / 100;
						this.PoisonAttackAddPercent += ((!isReverse) ? fp108 : (-fp108));
						return;
					}
				}
				else if (num <= 2932534896U)
				{
					if (num != 2922930795U)
					{
						if (num == 2932534896U)
						{
							if (header == "IcicleAttackAdd%")
							{
								FP fp109 = data.Value / 100;
								this.IcicleAttackAddPercent += ((!isReverse) ? fp109 : (-fp109));
								return;
							}
						}
					}
					else if (header == "NormalComboDamageAdd%")
					{
						FP fp110 = data.Value / 100;
						this.NormalComboDamageAddPercent += ((!isReverse) ? fp110 : (-fp110));
						return;
					}
				}
				else if (num != 2938320739U)
				{
					if (num == 2943015215U)
					{
						if (header == "ThunderBuff%")
						{
							FP fp111 = data.Value / 100;
							this.ThunderBuffRate += ((!isReverse) ? fp111 : (-fp111));
							return;
						}
					}
				}
				else if (header == "FallingSwordSuperAttack%")
				{
					FP fp112 = data.Value / 100;
					this.FallingSwordSuperAttackPercent += ((!isReverse) ? fp112 : (-fp112));
					return;
				}
			}
			else if (num <= 3559936305U)
			{
				if (num <= 3203008697U)
				{
					if (num <= 3134448683U)
					{
						if (num <= 3039952241U)
						{
							if (num != 2960134143U)
							{
								if (num != 3039554674U)
								{
									if (num == 3039952241U)
									{
										if (header == "GlobalDamageAdd%")
										{
											FP fp113 = data.Value / 100;
											this.GlobalDamageAddPercent += ((!isReverse) ? fp113 : (-fp113));
											return;
										}
									}
								}
								else if (header == "RedSkillRate%")
								{
									FP fp114 = data.Value / 100;
									this.RedSkillRate += ((!isReverse) ? fp114 : (-fp114));
									return;
								}
							}
							else if (header == "SwordkeeAdd%")
							{
								FP fp115 = data.Value / 100;
								this.SwordkeeAddPercent += ((!isReverse) ? fp115 : (-fp115));
								return;
							}
						}
						else if (num != 3054515783U)
						{
							if (num != 3067500950U)
							{
								if (num == 3134448683U)
								{
									if (header == "KnifeComboCount")
									{
										FP value24 = data.Value;
										this.KnifeComboCount += ((!isReverse) ? value24 : (-value24));
										return;
									}
								}
							}
							else if (header == "IgnoreMiss%")
							{
								FP fp116 = data.Value / 100;
								this.IgnoreMiss += ((!isReverse) ? fp116 : (-fp116));
								return;
							}
						}
						else if (header == "FallingSwordAdd%")
						{
							FP fp117 = data.Value / 100;
							this.FallingSwordAddPercent += ((!isReverse) ? fp117 : (-fp117));
							return;
						}
					}
					else if (num <= 3168947780U)
					{
						if (num != 3155132838U)
						{
							if (num != 3162734590U)
							{
								if (num == 3168947780U)
								{
									if (header == "BurnReduction%")
									{
										FP fp118 = data.Value / 100;
										this.BurnReductionPercent += ((!isReverse) ? fp118 : (-fp118));
										return;
									}
								}
							}
							else if (header == "KnifeAngelAttack%")
							{
								FP fp119 = data.Value / 100;
								this.KnifeAngelAttackPercent += ((!isReverse) ? fp119 : (-fp119));
								return;
							}
						}
						else if (header == "KnifeAngel%")
						{
							FP fp120 = data.Value / 100;
							this.KnifeAngelRate += ((!isReverse) ? fp120 : (-fp120));
							return;
						}
					}
					else if (num != 3169739017U)
					{
						if (num != 3170106929U)
						{
							if (num == 3203008697U)
							{
								if (header == "IdleResAdd%")
								{
									FP fp121 = data.Value / 100;
									this.IdleResAddRate += ((!isReverse) ? fp121 : (-fp121));
									return;
								}
							}
						}
						else if (header == "BigSkillDamageAddByLessHp%")
						{
							FP fp122 = data.Value / 100;
							this.BigSkillDamageAddByLessHpPercent += ((!isReverse) ? fp122 : (-fp122));
							return;
						}
					}
					else if (header == "ShieldDamageReduction%")
					{
						FP fp123 = data.Value / 100;
						this.ShieldDamageReductionPercent += ((!isReverse) ? fp123 : (-fp123));
						return;
					}
				}
				else if (num <= 3385725185U)
				{
					if (num <= 3255034125U)
					{
						if (num != 3223872271U)
						{
							if (num != 3237332911U)
							{
								if (num == 3255034125U)
								{
									if (header == "NormalDamageAdd%")
									{
										FP fp124 = data.Value / 100;
										this.NormalDamageAddPercent += ((!isReverse) ? fp124 : (-fp124));
										return;
									}
								}
							}
							else if (header == "FlippingCoinAddRate%")
							{
								FP fp125 = data.Value / 100;
								this.FlippingCoinAddRate += ((!isReverse) ? fp125 : (-fp125));
								return;
							}
						}
						else if (header == "OrangePetDamageAdd%")
						{
							FP fp126 = data.Value / 100;
							this.OrangePetDamageAddPercent += ((!isReverse) ? fp126 : (-fp126));
							return;
						}
					}
					else if (num != 3312255410U)
					{
						if (num != 3350337052U)
						{
							if (num == 3385725185U)
							{
								if (header == "FireAddBuffLayerAdd")
								{
									FP value25 = data.Value;
									this.FireAddBuffLayerAdd += ((!isReverse) ? value25 : (-value25));
									return;
								}
							}
						}
						else if (header == "IcicleOrStunDamageAdd%")
						{
							FP fp127 = data.Value / 100;
							this.IcicleOrStunDamageAddPercent += ((!isReverse) ? fp127 : (-fp127));
							return;
						}
					}
					else if (header == "FireWaveAdd%")
					{
						FP fp128 = data.Value / 100;
						this.FireWaveAddPercent += ((!isReverse) ? fp128 : (-fp128));
						return;
					}
				}
				else if (num <= 3472471011U)
				{
					if (num != 3412204288U)
					{
						if (num != 3414355812U)
						{
							if (num == 3472471011U)
							{
								if (header == "KnifeCombo%")
								{
									FP fp129 = data.Value / 100;
									this.KnifeComboRate += ((!isReverse) ? fp129 : (-fp129));
									return;
								}
							}
						}
						else if (header == "ThunderDeath%")
						{
							FP fp130 = data.Value / 100;
							this.ThunderDeathRate += ((!isReverse) ? fp130 : (-fp130));
							return;
						}
					}
					else if (header == "FallingSwordFire%")
					{
						FP fp131 = data.Value / 100;
						this.FallingSwordFireRate += ((!isReverse) ? fp131 : (-fp131));
						return;
					}
				}
				else if (num != 3508387468U)
				{
					if (num != 3522760579U)
					{
						if (num == 3559936305U)
						{
							if (header == "KnifeCount")
							{
								FP value26 = data.Value;
								this.KnifeCount += ((!isReverse) ? value26 : (-value26));
								return;
							}
						}
					}
					else if (header == "PurplePetDamageAdd%")
					{
						FP fp132 = data.Value / 100;
						this.PurplePetDamageAddPercent += ((!isReverse) ? fp132 : (-fp132));
						return;
					}
				}
				else if (header == "KnifeAdd%")
				{
					FP fp133 = data.Value / 100;
					this.KnifeAddPercent += ((!isReverse) ? fp133 : (-fp133));
					return;
				}
			}
			else if (num <= 3968980422U)
			{
				if (num <= 3811421533U)
				{
					if (num <= 3702082652U)
					{
						if (num != 3597303253U)
						{
							if (num != 3692514014U)
							{
								if (num == 3702082652U)
								{
									if (header == "IsInvincible")
									{
										FP value27 = data.Value;
										this.IsInvincible += ((!isReverse) ? value27 : (-value27));
										return;
									}
								}
							}
							else if (header == "CritRate%")
							{
								FP fp134 = data.Value / 100;
								this.CritRate += ((!isReverse) ? fp134 : (-fp134));
								return;
							}
						}
						else if (header == "StaminaRecover%")
						{
							FP fp135 = data.Value / 100;
							this.StaminaRecoveryRate += ((!isReverse) ? fp135 : (-fp135));
							return;
						}
					}
					else if (num != 3742135179U)
					{
						if (num != 3766927427U)
						{
							if (num == 3811421533U)
							{
								if (header == "GlobalAttack%")
								{
									FP fp136 = data.Value / 100;
									this.GlobalAttackPercent += ((!isReverse) ? fp136 : (-fp136));
									return;
								}
							}
						}
						else if (header == "MissDouble%")
						{
							FP fp137 = data.Value / 100;
							this.MissDoublePercent += ((!isReverse) ? fp137 : (-fp137));
							return;
						}
					}
					else if (header == "SkillDamageReduction%")
					{
						FP fp138 = data.Value / 100;
						this.SkillDamageReductionPercent += ((!isReverse) ? fp138 : (-fp138));
						return;
					}
				}
				else if (num <= 3860364124U)
				{
					if (num != 3819354180U)
					{
						if (num != 3850662367U)
						{
							if (num == 3860364124U)
							{
								if (header == "IsBigSkillAfterFallingSword")
								{
									FP value28 = data.Value;
									this.IsBigSkillAfterFallingSword += ((!isReverse) ? value28 : (-value28));
									return;
								}
							}
						}
						else if (header == "BigSkillDamageReduction%")
						{
							FP fp139 = data.Value / 100;
							this.BigSkillDamageReductionPercent += ((!isReverse) ? fp139 : (-fp139));
							return;
						}
					}
					else if (header == "RevengeCount")
					{
						this.RevengeCount += ((!isReverse) ? data.Value : (-data.Value));
						return;
					}
				}
				else if (num != 3907572195U)
				{
					if (num != 3966067349U)
					{
						if (num == 3968980422U)
						{
							if (header == "BigSkillDamageAddByGreaterHp%")
							{
								FP fp140 = data.Value / 100;
								this.BigSkillDamageAddByGreaterHpPercent += ((!isReverse) ? fp140 : (-fp140));
								return;
							}
						}
					}
					else if (header == "IsTargetHpHigherDamageHigher")
					{
						FP value29 = data.Value;
						this.IsTargetHpHigherDamageHigher += ((!isReverse) ? value29 : (-value29));
						return;
					}
				}
				else if (header == "TrueAdd%")
				{
					FP fp141 = data.Value / 100;
					this.TrueAddPercent += ((!isReverse) ? fp141 : (-fp141));
					return;
				}
			}
			else if (num <= 4080100530U)
			{
				if (num <= 4067587819U)
				{
					if (num != 4026747765U)
					{
						if (num != 4028531508U)
						{
							if (num == 4067587819U)
							{
								if (header == "ControlImmunity%")
								{
									FP fp142 = data.Value / 100;
									this.ControlImmunityRate += ((!isReverse) ? fp142 : (-fp142));
									return;
								}
							}
						}
						else if (header == "RoundStartFallingSwordCount")
						{
							FP value30 = data.Value;
							this.RoundStartFallingSwordCount += ((!isReverse) ? value30 : (-value30));
							return;
						}
					}
					else if (header == "IsRoundStartFallingSword")
					{
						FP value31 = data.Value;
						this.IsRoundStartFallingSword += ((!isReverse) ? value31 : (-value31));
						return;
					}
				}
				else if (num != 4068778659U)
				{
					if (num != 4078340181U)
					{
						if (num == 4080100530U)
						{
							if (header == "BattleCoinAddRate%")
							{
								FP fp143 = data.Value / 100;
								this.BattleCoinAddRate += ((!isReverse) ? fp143 : (-fp143));
								return;
							}
						}
					}
					else if (header == "ThunderDeathAttackAddMaxCount")
					{
						this.ThunderDeathAttackAddMaxCount += ((!isReverse) ? data.Value : (-data.Value));
						return;
					}
				}
				else if (header == "GetSkillCountByBegin")
				{
					this.GetSkillCountByBegin += ((!isReverse) ? data.Value : (-data.Value));
					return;
				}
			}
			else if (num <= 4136069348U)
			{
				if (num != 4103137980U)
				{
					if (num != 4117397215U)
					{
						if (num == 4136069348U)
						{
							if (header == "DefenseReductionCritValue%")
							{
								FP fp144 = data.Value / 100;
								this.DefenseReductionCritValue += ((!isReverse) ? fp144 : (-fp144));
								return;
							}
						}
					}
					else if (header == "PetCritRate%")
					{
						FP fp145 = data.Value / 100;
						this.PetCritRate += ((!isReverse) ? fp145 : (-fp145));
						return;
					}
				}
				else if (header == "Recharge")
				{
					Action<FP> onRechargeUpdate = this.m_onRechargeUpdate;
					if (onRechargeUpdate == null)
					{
						return;
					}
					onRechargeUpdate((!isReverse) ? data.Value : (-data.Value));
					return;
				}
			}
			else if (num <= 4156043856U)
			{
				if (num != 4155996931U)
				{
					if (num == 4156043856U)
					{
						if (header == "Greater60HpDamageAdd%")
						{
							FP fp146 = data.Value / 100;
							this.Greater60HpDamageAddPercent += ((!isReverse) ? fp146 : (-fp146));
							return;
						}
					}
				}
				else if (header == "GlobalDamageReduction%")
				{
					FP fp147 = data.Value / 100;
					this.GlobalDamageReductionPercent += ((!isReverse) ? fp147 : (-fp147));
					return;
				}
			}
			else if (num != 4195276125U)
			{
				if (num == 4243823599U)
				{
					if (header == "ChainComboCount")
					{
						FP value32 = data.Value;
						this.ChainComboCount += ((!isReverse) ? value32 : (-value32));
						return;
					}
				}
			}
			else if (header == "RoundStartKnifeCount")
			{
				FP value33 = data.Value;
				this.RoundStartKnifeCount += ((!isReverse) ? value33 : (-value33));
				return;
			}
			HLog.LogError("属性配置表错误. MergeAttribute is failure. 没有type属性类型. type = " + header);
		}

		public void ChangeReviveCount(FP changeValue)
		{
			this.ReviveCount += changeValue;
		}

		public void ResetThunderHitCount()
		{
			this.ThunderHitByCount = FP._0;
		}

		public void AddThunderHitByCount(int count = 1)
		{
			this.ThunderHitByCount += count;
		}

		public FP GetAttack()
		{
			return this.Attack * (FP._1 + this.AttackPercent) * (FP._1 + this.GlobalAttackPercent);
		}

		public FP GetHpMax()
		{
			FP fp = this.HPMax * (FP._1 + this.HPMaxPercent) * (FP._1 + this.GlobalHPMaxPercent);
			if (this.CurBattleMode == BattleMode.PVP)
			{
				fp *= Config.PVPMaxHpRate;
			}
			return fp.AsLong();
		}

		public FP GetDefence()
		{
			return this.Defence * (FP._1 + this.DefencePercent) * (FP._1 + this.GlobalDefencePercent);
		}

		public FP GetFinalDefence(FP ignoreDefencePercent)
		{
			FP fp = MathTools.Clamp(ignoreDefencePercent, FP._0, FP._1);
			return this.GetDefence() * (FP._1 - fp);
		}

		public FP GetRecoverAddPercent()
		{
			return FP._1 + this.RecoverAddPercent - this.RecoverReductionPercent;
		}

		public FP GetRevengeRate()
		{
			return this.RevengeRate;
		}

		public FP GetMissRate
		{
			get
			{
				return this.Miss * (FP._1 + this.MissDoublePercent);
			}
		}

		public FP GetIgnoreMiss
		{
			get
			{
				return this.IgnoreMiss;
			}
		}

		public bool IsThunderShield()
		{
			return this.ThunderShield > FP._0;
		}

		public bool IsDurianShield()
		{
			return this.DurianShield > FP._0;
		}

		public bool IsNormalCritRateAddCritValue
		{
			get
			{
				return this.IsNormalCritAddValue > FP._0;
			}
		}

		public bool IsSkillCritRateAddCritValue
		{
			get
			{
				return this.IsSkillCritAddValue > FP._0;
			}
		}

		public bool IsHpHigherDamageHigher
		{
			get
			{
				return this.IsTargetHpHigherDamageHigher > FP._0;
			}
		}

		public bool IsInvincibled
		{
			get
			{
				return this.IsInvincible > FP._0;
			}
		}

		public long GetBasicAttributeValue(string attribute)
		{
			long num = 0L;
			if (!(attribute == "Attack"))
			{
				if (!(attribute == "Attack%"))
				{
					if (!(attribute == "Defence"))
					{
						if (!(attribute == "Defence%"))
						{
							if (!(attribute == "HPMax"))
							{
								if (attribute == "HPMax%")
								{
									num = this.HPMaxPercent.AsLong();
								}
							}
							else
							{
								num = this.HPMax.AsLong();
							}
						}
						else
						{
							num = this.DefencePercent.AsLong();
						}
					}
					else
					{
						num = this.Defence.AsLong();
					}
				}
				else
				{
					num = this.HPMax.AsLong();
				}
			}
			else
			{
				num = this.Attack.AsLong();
			}
			return num;
		}

		public long GetAttack4UI()
		{
			return this.GetAttack().AsLong();
		}

		public long GetHpMax4UI()
		{
			return this.GetHpMax().AsLong();
		}

		public long GetDefence4UI()
		{
			return this.GetDefence().AsLong();
		}

		public void Log()
		{
			JsonManager.SerializeObject(this);
		}

		[MemberAttributeInfo("GlobalAttack%", "全局攻击力 累加百分比", 0)]
		public FP GlobalAttackPercent = new FP(0);

		[MemberAttributeInfo("GlobalHPMax%", "全局最大血量 累加百分比", 0)]
		public FP GlobalHPMaxPercent = new FP(0);

		[MemberAttributeInfo("GlobalDefence%", "全局防御力 累加百分比", 0)]
		public FP GlobalDefencePercent = new FP(0);

		[MemberAttributeInfo("GlobalDamageAdd%", "全局伤害 累加百分比", 0)]
		public FP GlobalDamageAddPercent = new FP(0);

		[MemberAttributeInfo("GlobalDamageReduction%", "全局伤害 减免百分比", 0)]
		public FP GlobalDamageReductionPercent = new FP(0);

		[MemberAttributeInfo("PhysicalAdd%", "物理伤害", 0)]
		public FP PhysicalAddPercent = new FP(0);

		[MemberAttributeInfo("PhysicalReduction%", "物理减伤", 0)]
		public FP PhysicalReductionPercent = new FP(0);

		[MemberAttributeInfo("FireAdd%", "火焰伤害", 0)]
		public FP FireAddPercent = new FP(0);

		[MemberAttributeInfo("FireReduction%", "火焰减伤", 0)]
		public FP FireReductionPercent = new FP(0);

		[MemberAttributeInfo("ElectricAdd%", "电系伤害", 0)]
		public FP ElectricAddPercent = new FP(0);

		[MemberAttributeInfo("ElectricReduction%", "电系减伤", 0)]
		public FP ElectricReductionPercent = new FP(0);

		[MemberAttributeInfo("IceAdd%", "冰系伤害", 0)]
		public FP IceAddPercent = new FP(0);

		[MemberAttributeInfo("IceReduction%", "冰系减伤", 0)]
		public FP IceReductionPercent = new FP(0);

		[MemberAttributeInfo("TrueAdd%", "真实增伤", 0)]
		public FP TrueAddPercent = new FP(0);

		[MemberAttributeInfo("TrueReduction%", "真实减伤", 0)]
		public FP TrueReductionPercent = new FP(0);

		[MemberAttributeInfo("Attack", "攻击力", 0)]
		public FP Attack = FP._0;

		[MemberAttributeInfo("Attack%", "攻击力 累加百分比", 0)]
		public FP AttackPercent = new FP(0);

		[MemberAttributeInfo("HPMax", "最大血量", 0)]
		public FP HPMax = FP._0;

		[MemberAttributeInfo("HPMax%", "最大生命累加百分比", 0)]
		public FP HPMaxPercent = new FP(0);

		[MemberAttributeInfo("Defence", "防御力", 0)]
		public FP Defence = FP._0;

		[MemberAttributeInfo("Defence%", "防御力累加百分比", 0)]
		public FP DefencePercent = new FP(0);

		[MemberAttributeInfo("IgnoreDefence%", "忽略防御百分比", 0)]
		public FP IgnoreDefencePercent = new FP(0);

		[MemberAttributeInfo("LegacyPowerAdd%", "传承能量增加百分比", 0)]
		public FP LegacyPowerAddPercent = FP._0;

		[MemberAttributeInfo("Miss%", "闪避率", 0)]
		public FP Miss = new FP(0);

		[MemberAttributeInfo("MissDouble%", "闪避翻倍", 0)]
		public FP MissDoublePercent = new FP(0);

		[MemberAttributeInfo("IgnoreMiss%", "忽视闪避", 0)]
		public FP IgnoreMiss = new FP(0);

		public FP RechargeMax = new FP(100);

		[MemberAttributeInfo("NoEnergyRecovery", "禁止能量回复", 0)]
		public FP NoEnergyRecovery = FP._0;

		[MemberAttributeInfo("NormalCombo%", "普攻连击率", 0)]
		public FP NormalComboRate = new FP(0);

		[MemberAttributeInfo("NormalComboCount", "普攻连击次数", 0)]
		public FP NormalComboCount = FP._0;

		[MemberAttributeInfo("NormalComboDamageAdd%", "普攻连击伤害加成百分比", 0)]
		public FP NormalComboDamageAddPercent = FP._0;

		[MemberAttributeInfo("NormalComboCritAdd%", "普攻连击暴击伤害增加百分比", 0)]
		public FP NormalComboCritAddPercent = FP._0;

		[MemberAttributeInfo("DefenseReductionCritValue%", "减防目标暴击伤害增加百分比", 0)]
		public FP DefenseReductionCritValue = FP._0;

		[MemberAttributeInfo("CritRate%", "基础暴击率", 0)]
		public FP CritRate = new FP(0);

		[MemberAttributeInfo("PetCritRate%", "宠物暴击率", 0)]
		public FP PetCritRate = new FP(0);

		[MemberAttributeInfo("CritValue%", "基础暴击伤害加成", 0)]
		public FP CritValue = FP._2;

		[MemberAttributeInfo("CritValueReduction%", "基础暴击伤害减免百分比", 0)]
		public FP CritValueReduction = new FP(0);

		[MemberAttributeInfo("NormalCritRate%", "普攻暴击率", 0)]
		public FP NormalCritRate = new FP(0);

		[MemberAttributeInfo("SkillCritRate%", "技能暴击率", 0)]
		public FP SkillCritRate = new FP(0);

		[MemberAttributeInfo("WeaponCritRate%", "武器暴击率", 0)]
		public FP WeaponCritRate = new FP(0);

		[MemberAttributeInfo("IsNormalCritAddValue", "普攻暴击率增加普攻暴击伤害", 0)]
		public FP IsNormalCritAddValue = FP._0;

		[MemberAttributeInfo("IsSkillCritRateCritAddValue", "技能暴击率增加技能暴击伤害", 0)]
		public FP IsSkillCritAddValue = FP._0;

		[MemberAttributeInfo("DamageAdd%", "伤害增加百分比", 0)]
		public FP DamageAddPercent = new FP(0);

		[MemberAttributeInfo("DamageReduction%", "伤害减免百分比", 0)]
		public FP DamageReductionPercent = new FP(0);

		[MemberAttributeInfo("Greater60HpDamageAdd%", "大于60%血量伤害增加百分比", 0)]
		public FP Greater60HpDamageAddPercent = new FP(0);

		[MemberAttributeInfo("IsTargetHpHigherDamageHigher", "目标血量越高造成伤害越高", 0)]
		private FP IsTargetHpHigherDamageHigher = FP._0;

		[MemberAttributeInfo("IsInvincible", "是否无敌", 0)]
		private FP IsInvincible = FP._0;

		[MemberAttributeInfo("CombatBaseDamage%", "连击基础伤害系数", 0)]
		public FP CombatBaseDamagePercent = new FP(0);

		[MemberAttributeInfo("CounterBaseDamage%", "反击基础伤害系数", 0)]
		public FP CounterBaseDamagePercent = new FP(0);

		[MemberAttributeInfo("OrdinarySkillBaseDamage%", "普攻基础伤害系数", 0)]
		public FP OrdinarySkillBaseDamagePercent = new FP(0);

		[MemberAttributeInfo("BigSkillBaseDamage%", "大招基础伤害系数", 0)]
		public FP BigSkillBaseDamagePercent = new FP(0);

		[MemberAttributeInfo("NormalDamageAdd%", "普通伤害增伤百分比", 0)]
		public FP NormalDamageAddPercent = new FP(0);

		[MemberAttributeInfo("NormalDamageReduction%", "普通伤害减伤百分比", 0)]
		public FP NormalDamageReductionPercent = new FP(0);

		[MemberAttributeInfo("BigSkillDamageAdd%", "大招伤害增伤百分比", 0)]
		public FP BigSkillDamageAddPercent = new FP(0);

		[MemberAttributeInfo("BigSkillDamageReduction%", "大招伤害减伤百分比", 0)]
		public FP BigSkillDamageReductionPercent = new FP(0);

		[MemberAttributeInfo("SkillDamageAdd%", "技能伤害(大招、小招)增伤百分比", 0)]
		public FP SkillDamageAddPercent = new FP(0);

		[MemberAttributeInfo("SkillDamageReduction%", "技能伤害减伤百分比", 0)]
		public FP SkillDamageReductionPercent = new FP(0);

		[MemberAttributeInfo("ComboDamageAdd%", "连击伤害增伤百分比", 0)]
		public FP ComboDamageAddPercent = new FP(0);

		[MemberAttributeInfo("ComboDamageReduction%", "连击伤害减伤百分比", 0)]
		public FP ComboDamageReductionPercent = new FP(0);

		[MemberAttributeInfo("RevengeDamageAdd%", "反击伤害增伤百分比", 0)]
		public FP RevengeDamageAddPercent = new FP(0);

		[MemberAttributeInfo("RevengeDamageReduction%", "反击伤害减伤百分比", 0)]
		public FP RevengeDamageReductionPercent = new FP(0);

		[MemberAttributeInfo("PetDamageAdd%", "宠物伤害增伤百分比", 0)]
		public FP PetDamageAddPercent = new FP(0);

		[MemberAttributeInfo("PetDamageReduction%", "宠物伤害减伤百分比", 0)]
		public FP PetDamageReductionPercent = new FP(0);

		[MemberAttributeInfo("BigSkillDamageAddByGreaterHp%", "大招增伤条件(生命大于xx目标生效)", 0)]
		public FP BigSkillDamageAddByGreaterHpPercent = new FP(0);

		[MemberAttributeInfo("GreaterHpPercentBigSkillDamageAdd%", "大招增伤百分比(生命大于xx目标生效)", 0)]
		public FP GreaterHpPercentBigSkillDamageAddPercent = new FP(0);

		[MemberAttributeInfo("BigSkillDamageAddByLessHp%", "大招增伤条件(生命小于xx的目标生效)", 0)]
		public FP BigSkillDamageAddByLessHpPercent = new FP(0);

		[MemberAttributeInfo("LessHpPercentBigSkillDamageAdd%", "大招增伤百分比(生命小于xx的目标生效)", 0)]
		public FP LessHpPercentBigSkillDamageAddPercent = new FP(0);

		[MemberAttributeInfo("ToBossDamageAdd%", "攻击Boss伤害增加百分比", 0)]
		public FP ToBossDamageAddPercent = new FP(0);

		[MemberAttributeInfo("ShieldDamageReduction%", "护盾伤害减免百分比", 0)]
		public FP ShieldDamageReductionPercent = new FP(0);

		[MemberAttributeInfo("IcicleAdd%", "冰增伤", 0)]
		public FP IcicleAddPercent = new FP(0);

		[MemberAttributeInfo("IcicleReduction%", "冰减伤", 0)]
		public FP IcicleReductionPercent = new FP(0);

		[MemberAttributeInfo("BurnAdd%", "火焰增伤", 0)]
		public FP BurnAddPercent = new FP(0);

		[MemberAttributeInfo("BurnReduction%", "火焰减伤", 0)]
		public FP BurnReductionPercent = new FP(0);

		[MemberAttributeInfo("FireWaveAdd%", "火焰波伤害增加百分比", 0)]
		public FP FireWaveAddPercent = new FP(0);

		[MemberAttributeInfo("FireWaveReduction%", "火焰波伤害减免百分比", 0)]
		public FP FireWaveReductionPercent = new FP(0);

		[MemberAttributeInfo("FireBuffAdd%", "火焰(BUFF)增伤", 0)]
		public FP FireBuffAddPercent = new FP(0);

		[MemberAttributeInfo("FireBuffReduction%", "火焰(BUFF)减伤", 0)]
		public FP FireBuffReductionPercent = new FP(0);

		[MemberAttributeInfo("ThunderAdd%", "闪电增伤", 0)]
		public FP ThunderAddPercent = new FP(0);

		[MemberAttributeInfo("ThunderReduction%", "闪电减伤", 0)]
		public FP ThunderReductionPercent = new FP(0);

		[MemberAttributeInfo("PoisonAdd%", "毒增伤", 0)]
		public FP PoisonAddPercent = new FP(0);

		[MemberAttributeInfo("PoisonReduction%", "毒减伤", 0)]
		public FP PoisonReductionPercent = new FP(0);

		[MemberAttributeInfo("KnifeAdd%", "飞刀增伤", 0)]
		public FP KnifeAddPercent = new FP(0);

		[MemberAttributeInfo("KnifeReduction%", "飞刀减伤", 0)]
		public FP KnifeReductionPercent = new FP(0);

		[MemberAttributeInfo("IcicleAttackAdd%", "对冰冻目标造成额外伤害%", 0)]
		public FP IcicleAttackAddPercent = new FP(0);

		[MemberAttributeInfo("BurnAttackAdd%", "对燃烧目标造成额外伤害%", 0)]
		public FP BurnAttackAddPercent = new FP(0);

		[MemberAttributeInfo("PoisonAttackAdd%", "对中毒目标造成额外伤害%", 0)]
		public FP PoisonAttackAddPercent = new FP(0);

		[MemberAttributeInfo("StunAttackAdd%", "对眩晕目标造成额外伤害%", 0)]
		public FP StunAttackAddPercent = new FP(0);

		[MemberAttributeInfo("ToVerdictAdd%", "对制裁目标造成额外伤害%", 0)]
		public FP ToVerdictAddPercent = new FP(0);

		[MemberAttributeInfo("ToVerdictComboRate%", "对裁决连击率增加%", 0)]
		public FP ToVerdictComboRate = new FP(0);

		[MemberAttributeInfo("IcicleOrStunDamageAdd%", "对于冰冻或眩晕额外伤害增加%", 0)]
		public FP IcicleOrStunDamageAddPercent = new FP(0);

		[MemberAttributeInfo("VulnerabilityDamageAdd%", "对于易伤额外伤害增加%", 0)]
		public FP VulnerabilityDamageAddPercent = new FP(0);

		[MemberAttributeInfo("BlockValue", "格挡值", 0)]
		public FP BlockValue = FP._0;

		public FP BlockTriggerPercent = FP._030;

		[MemberAttributeInfo("Revenge%", "反击概率", 0)]
		public FP RevengeRate = new FP(0);

		[MemberAttributeInfo("RevengeCount", "反击次数", 0)]
		public FP RevengeCount = FP._1;

		[MemberAttributeInfo("ChainComboCount", "Combo 次数技能 连击次数", 0)]
		public FP ChainComboCount = FP._0;

		[MemberAttributeInfo("RecoverAdd%", "血量恢复增加百分比", 0)]
		public FP RecoverAddPercent = new FP(0);

		[MemberAttributeInfo("RecoverReduction%", "血量回复减少百分比", 0)]
		public FP RecoverReductionPercent = new FP(0);

		[MemberAttributeInfo("NoHealing", "禁疗", 0)]
		private FP NoHealing = FP._0;

		[MemberAttributeInfo("VampireRate%", "触发吸血概率", 0)]
		public FP VampireRate = new FP(0);

		[MemberAttributeInfo("VampireAdd%", "吸血增加百分比", 0)]
		public FP VampireAddPercent = new FP(0);

		[MemberAttributeInfo("CounterVampire%", "反击吸血率", 0)]
		public FP CounterVampireRate = new FP(0);

		[MemberAttributeInfo("ReviveCount", "复活次数", 0)]
		public FP ReviveCount = FP._0;

		[MemberAttributeInfo("ReviveHpRate%", "复活血量百分比", 0)]
		public FP ReviveHpPercent = new FP(0);

		[MemberAttributeInfo("ThunderBaseAttack%", "基础闪电伤害系数", 0)]
		public FP ThunderBaseAttackPercent = new FP(0);

		[MemberAttributeInfo("ThunderCount", "闪电伤害次数", 0)]
		public FP ThunderCount = FP._0;

		[MemberAttributeInfo("ThunderBuff%", "闪电触发概率(闪电命中时有概率使敌人受到持续伤害)", 0)]
		public FP ThunderBuffRate = new FP(0);

		[MemberAttributeInfo("ThunderSuper%", "变为超级雷电的几率%", 0)]
		public FP ThunderSuperRate = new FP(0);

		[MemberAttributeInfo("ThunderDeath%", "变为死亡雷电的几率%", 0)]
		public FP ThunderDeathRate = new FP(0);

		[MemberAttributeInfo("ThunderSuperAttack%", "超级雷电伤害%", 0)]
		public FP ThunderSuperAttackPercent = new FP(0);

		[MemberAttributeInfo("ThunderDeathAttack%", "死亡雷电伤害%", 0)]
		public FP ThunderDeathAttackPercent = new FP(0);

		[MemberAttributeInfo("ThunderDeathAttackAdd%", "死亡闪电伤害增加百分比", 0)]
		public FP ThunderDeathAttackAddPercent = new FP(0);

		[MemberAttributeInfo("ThunderDeathAttackAddMaxCount", "死亡闪电伤害增加次数上限", 0)]
		public FP ThunderDeathAttackAddMaxCount = FP._0;

		public FP ThunderHitByCount = FP._0;

		[MemberAttributeInfo("ThunderCumulativeDamageAdd%", "闪电命中次数累计增加伤害百分比", 0)]
		public FP ThunderCumulativeDamageAddPercent = new FP(0);

		[MemberAttributeInfo("ThunderShield", "是否闪电盾", 0)]
		private FP ThunderShield = FP._0;

		[MemberAttributeInfo("DurianShield", "是否榴莲盾", 0)]
		private FP DurianShield = FP._0;

		[MemberAttributeInfo("IcicleSeckill%", "冰刺伤害秒杀概率", 0)]
		public FP IcicleSeckillRate = new FP(0);

		[MemberAttributeInfo("Frozen%", "冰冻概率", 0)]
		public FP FrozenRate = new FP(0);

		[MemberAttributeInfo("KnifeBaseAttack%", "飞刀基础伤害系数%", 0)]
		public FP KnifeBaseAttackPercent = new FP(0);

		[MemberAttributeInfo("IsTriggerKnift", "是否触发飞刀", 0)]
		public FP IsTriggerKnift = FP._0;

		[MemberAttributeInfo("KnifeCount", "飞刀次数", 0)]
		public FP KnifeCount = FP._0;

		[MemberAttributeInfo("IsBattleStartKnife", "是否触发战斗开始飞刀", 0)]
		public FP IsBattleStartKnife = FP._0;

		[MemberAttributeInfo("BattleStartKnifeCount", "战斗开始飞刀次数", 0)]
		public FP BattleStartKnifeCount = FP._0;

		[MemberAttributeInfo("IsRoundStartKnife", "是否触发角色回合开始飞刀", 0)]
		public FP IsRoundStartKnife = FP._0;

		[MemberAttributeInfo("RoundStartKnifeCount", "回合开始飞刀次数", 0)]
		public FP RoundStartKnifeCount = FP._0;

		[MemberAttributeInfo("KnifeCombo%", "飞刀连击率", 0)]
		public FP KnifeComboRate = new FP(0);

		[MemberAttributeInfo("KnifeComboCount", "飞刀连击次数", 0)]
		public FP KnifeComboCount = FP._0;

		[MemberAttributeInfo("KnifeThunder%", "飞刀触发闪电概率", 0)]
		public FP KnifeThunderRate = new FP(0);

		[MemberAttributeInfo("KnifeIcicle%", "飞刀触发冰刺概率", 0)]
		public FP KnifeIcicleRate = new FP(0);

		[MemberAttributeInfo("KnifeFire%", "飞刀触发火焰概率", 0)]
		public FP KnifeFireRate = new FP(0);

		[MemberAttributeInfo("KnifeSuper%", "变为超级飞刀的几率%", 0)]
		public FP KnifeSuperRate = new FP(0);

		[MemberAttributeInfo("KnifeAngel%", "变为天使飞刀的几率%", 0)]
		public FP KnifeAngelRate = new FP(0);

		[MemberAttributeInfo("KnifeSuperAttack%", "超级飞刀伤害加成%", 0)]
		public FP KnifeSuperAttackPercent = new FP(0);

		[MemberAttributeInfo("KnifeAngelAttack%", "天使飞刀伤害系数%", 0)]
		public FP KnifeAngelAttackPercent = new FP(0);

		public FP KnifeBombRate = new FP(0);

		[MemberAttributeInfo("KnifePoison%", "飞刀带毒概率", 0)]
		public FP KnifePoisonRate = new FP(0);

		[MemberAttributeInfo("KnifeRevert%", "飞刀回复概率", 0)]
		public FP KnifeRevertRate = new FP(0);

		[MemberAttributeInfo("KnifeRage%", "飞刀回怒概率", 0)]
		public FP KnifeRageRate = new FP(0);

		[MemberAttributeInfo("TriggerFireBuff%", "触发燃烧Buff概率", 0)]
		public FP TriggerFireBuffRate = new FP(0);

		[MemberAttributeInfo("TriggerFireBuffAdd%", "触发燃烧Buff概率增加", 0)]
		public FP TriggerFireBuffAddRate = new FP(0);

		[MemberAttributeInfo("FireBuffAddRound", "燃烧持续回合+1", 0)]
		public FP FireBuffAddRound = FP._0;

		[MemberAttributeInfo("FireBuffReductionRound", "燃烧持续回合-1", 0)]
		public FP FireBuffReductionRound = FP._0;

		[MemberAttributeInfo("FireBuffMaxLayerAdd", "燃烧Buff额外增加层数", 0)]
		public FP FireBuffMaxLayerAdd = FP._0;

		[MemberAttributeInfo("FireAddBuffLayerAdd", "火焰每次加N层燃烧Buff", 0)]
		public FP FireAddBuffLayerAdd = FP._1;

		[MemberAttributeInfo("FireTriggerDamageBuff%", "火焰波触发伤害Buff概率", 0)]
		public FP FireTriggerDamageBuffRate = new FP(0);

		[MemberAttributeInfo("UseFireBuffPerLayerDamageAdd", "是否开启燃烧Buff层数伤害增加", 0)]
		public FP IsFireBuffPerLayerDamageAdd = FP._0;

		[MemberAttributeInfo("FireBuffPerLayerDamageAdd%", "燃烧Buff每层伤害增加百分比", 0)]
		public FP FireBuffPerLayerDamageAddPercent = new FP(0);

		[MemberAttributeInfo("FallingSwordCount", "落剑数量", 0)]
		public FP FallingSwordCount = FP._0;

		[MemberAttributeInfo("IsBattleStartFallingSword", "战斗开始是否触发落剑", 0)]
		public FP IsBattleStartFallingSword = FP._0;

		[MemberAttributeInfo("BattleStartFallingSwordCount", "战斗开始落剑数量", 0)]
		public FP BattleStartFallingSwordCount = FP._0;

		[MemberAttributeInfo("IsRoundStartFallingSword", "回合结束是否触发落剑", 0)]
		public FP IsRoundStartFallingSword = FP._0;

		[MemberAttributeInfo("RoundStartFallingSwordCount", "角色回合开始落剑数量", 0)]
		public FP RoundStartFallingSwordCount = FP._0;

		[MemberAttributeInfo("IsBigSkillAfterFallingSword", "大招后是否触发落剑", 0)]
		public FP IsBigSkillAfterFallingSword = FP._0;

		[MemberAttributeInfo("BigSkillAfterFallingSwordCount", "角色回合开始落剑数量", 0)]
		public FP BigSkillAfterFallingSwordCount = FP._0;

		[MemberAttributeInfo("FallingSwordSuper%", "变为超级落剑的几率%", 0)]
		public FP FallingSwordSuperRate = new FP(0);

		[MemberAttributeInfo("FallingSwordSuperAttack%", "超级落剑伤害%", 0)]
		public FP FallingSwordSuperAttackPercent = new FP(0);

		[MemberAttributeInfo("FallingSwordAdd%", "落剑伤害增加百分比", 0)]
		public FP FallingSwordAddPercent = new FP(0);

		[MemberAttributeInfo("FallingSwordReduction%", "落剑伤害减免百分比", 0)]
		public FP FallingSwordReductionPercent = new FP(0);

		[MemberAttributeInfo("FallingSwordFire%", "落剑触发火焰波概率", 0)]
		public FP FallingSwordFireRate = new FP(0);

		[MemberAttributeInfo("ControlImmunity%", "免控率", 0)]
		public FP ControlImmunityRate = new FP(0);

		[MemberAttributeInfo("IgnoreControlImmunity%", "忽视免控率", 0)]
		public FP IgnoreControlImmunityRate = new FP(0);

		[MemberAttributeInfo("IgnoreNormalCombo%", "忽视连击率", 0)]
		public FP IgnoreNormalComboRate = new FP(0);

		[MemberAttributeInfo("IgnoreRevenge%", "忽视反击率", 0)]
		public FP IgnoreRevengeRate = new FP(0);

		[MemberAttributeInfo("IgnoreCrit%", "忽视暴击率", 0)]
		public FP IgnoreCritRate = new FP(0);

		[MemberAttributeInfo("SwordkeeRecover%", "剑气回复最大生命百分比", 0)]
		public FP SwordkeeRecoverRate = new FP(0);

		[MemberAttributeInfo("SwordkeeSuper%", "变为超级剑气的几率%", 0)]
		public FP SwordkeeSuperRate = new FP(0);

		[MemberAttributeInfo("SwordkeeSuperAttack%", "超级剑气伤害%", 0)]
		public FP SwordkeeSuperAttackPercent = new FP(0);

		[MemberAttributeInfo("SwordkeeAdd%", "剑气伤害增加百分比", 0)]
		public FP SwordkeeAddPercent = new FP(0);

		[MemberAttributeInfo("SwordkeeReduction%", "剑气伤害减免百分比", 0)]
		public FP SwordkeeReductionPercent = new FP(0);

		[MemberAttributeInfo("SwordkeeTriggerDamageBuff%", "剑气触发伤害Buff概率", 0)]
		public FP SwordkeeTriggerDamageBuffRate = new FP(0);

		[MemberAttributeInfo("PurplePetDamageAdd%", "史诗宠物伤害增加%", 0)]
		public FP PurplePetDamageAddPercent = new FP(0);

		[MemberAttributeInfo("OrangePetDamageAdd%", "传说宠物伤害增加%", 0)]
		public FP OrangePetDamageAddPercent = new FP(0);

		[MemberAttributeInfo("RedPetDamageAdd%", "神话宠物伤害增加%", 0)]
		public FP RedPetDamageAddPercent = new FP(0);

		[MemberAttributeInfo("Recovery%", "提升营地回复效果%", 0)]
		public FP RecoveryRate = new FP(0);

		public FP BeginShell = FP._0;

		[MemberAttributeInfo("UpgradeRegenerate%", "升级回复生命%", 0)]
		public FP UpgradeRegeneRate = new FP(0);

		public FP GetSkillCountByBegin = FP._0;

		[MemberAttributeInfo("GetSelectSkillCountByUpgrade", "升级时可以重新选择n次技能", 0)]
		public FP GetSelectSkillCountByUpgrade = FP._0;

		public FP EffectiveTimes = FP._0;

		[MemberAttributeInfo("AttackBattleEnd%", "战斗结束后攻击力", 0)]
		public FP AttackBattleEnd = new FP(0);

		[MemberAttributeInfo("DefenseBattleEnd%", "战斗结束后防御力", 0)]
		public FP DefenseBattleEnd = new FP(0);

		[MemberAttributeInfo("ExpAddRate%", "获得经验增加%", 0)]
		public FP ExpAddRate = new FP(0);

		[MemberAttributeInfo("EventCoinAddRate%", "事件金币倍率%", 0)]
		public FP EventCoinAddRate = new FP(0);

		[MemberAttributeInfo("BattleCoinAddRate%", "战斗金币倍率%", 0)]
		public FP BattleCoinAddRate = new FP(0);

		[MemberAttributeInfo("SlotCoinAddRate%", "老虎机金币倍率(%)", 0)]
		public FP SlotCoinAddRate = new FP(0);

		[MemberAttributeInfo("FlippingCoinAddRate%", "翻牌子金币倍率%", 0)]
		public FP FlippingCoinAddRate = new FP(0);

		[MemberAttributeInfo("StaminaMax", "体力上限", 0)]
		public FP StaminaMax = FP._0;

		[MemberAttributeInfo("StaminaRecover%", "体力恢复速度%", 0)]
		public FP StaminaRecoveryRate = new FP(0);

		[MemberAttributeInfo("PrivilegeGem%", "特权卡每日宝石收益(%)", 0)]
		public FP PrivilegeGemRate = FP._0;

		[MemberAttributeInfo("IdleResAdd%", "挂机资源奖励提升", 0)]
		public FP IdleResAddRate = FP._0;

		[MemberAttributeInfo("AxeRecover%", "镐头恢复速度%", 0)]
		public FP AxeRecoverRate = new FP(0);

		[MemberAttributeInfo("HangTimeMax", "挂机时间上限", 0)]
		public FP HangTimeMax = new FP(0);

		[MemberAttributeInfo("SmallLuckyRate%", "小吉概率", 0)]
		public FP SmallLuckyRate = new FP(0);

		[MemberAttributeInfo("BigLuckyRate%", "大吉概率", 0)]
		public FP BigLuckyRate = new FP(0);

		[MemberAttributeInfo("OrangeSkillRate%", "传说技能概率", 0)]
		public FP OrangeSkillRate = new FP(0);

		[MemberAttributeInfo("RedSkillRate%", "传奇技能概率", 0)]
		public FP RedSkillRate = new FP(0);

		public Action<FP> m_onHpMaxUpdate;

		public Action<FP> m_onRechargeUpdate;

		public Action<FP> m_onLegacyPowerUpdate;

		public Action<FP> m_onShieldValueUpdate;
	}
}
