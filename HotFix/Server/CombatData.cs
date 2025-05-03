using System;
using System.Collections.Generic;
using LocalModels;
using LocalModels.Bean;

namespace Server
{
	public class CombatData
	{
		public double CurComba
		{
			get
			{
				return this.m_comba;
			}
			set
			{
				this.m_comba = value;
			}
		}

		public CombatData()
		{
			this.Reset();
		}

		public void Reset()
		{
			this.m_comba = 0.0;
		}

		public void MathCombat(LocalModelManager localModelManager, MemberAttributeData data, List<int> skills)
		{
			if (data == null)
			{
				return;
			}
			IList<Attribute_AttrText> allElements = localModelManager.GetAttribute_AttrTextModelInstance().GetAllElements();
			this.MathAttrCombat(allElements, data);
			this.MathSkillCombat(localModelManager, skills);
		}

		public void MathAttrCombat(IList<Attribute_AttrText> attributeAbilities, MemberAttributeData data)
		{
			for (int i = 0; i < attributeAbilities.Count; i++)
			{
				Attribute_AttrText attribute_AttrText = attributeAbilities[i];
				if (attribute_AttrText.IsPower >= 1)
				{
					string id = attribute_AttrText.ID;
					uint num = <PrivateImplementationDetails>.ComputeStringHash(id);
					if (num <= 1811507796U)
					{
						if (num <= 593219697U)
						{
							if (num <= 367905890U)
							{
								if (num <= 292982194U)
								{
									if (num != 119853533U)
									{
										if (num != 233388682U)
										{
											if (num == 292982194U)
											{
												if (id == "PhysicalAdd%")
												{
													this.m_comba += data.PhysicalAddPercent.AsDouble() * (double)attribute_AttrText.Value;
												}
											}
										}
										else if (id == "NormalComboCritAdd%")
										{
											this.m_comba += data.NormalComboCritAddPercent.AsDouble() * (double)attribute_AttrText.Value;
										}
									}
									else if (id == "BigSkillDamageAdd%")
									{
										this.m_comba += data.BigSkillDamageAddPercent.AsDouble() * (double)attribute_AttrText.Value;
									}
								}
								else if (num != 295522859U)
								{
									if (num != 314884380U)
									{
										if (num == 367905890U)
										{
											if (id == "RevengeDamageAdd%")
											{
												this.m_comba += data.RevengeDamageAddPercent.AsDouble() * (double)attribute_AttrText.Value;
											}
										}
									}
									else if (id == "IceReduction%")
									{
										this.m_comba += data.IceReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
									}
								}
								else if (id == "VampireAdd%")
								{
									this.m_comba += data.VampireAddPercent.AsDouble() * (double)attribute_AttrText.Value;
								}
							}
							else if (num <= 425664211U)
							{
								if (num != 379785653U)
								{
									if (num != 394635115U)
									{
										if (num == 425664211U)
										{
											if (id == "NormalComboCount")
											{
												this.m_comba += data.NormalComboCount.AsDouble() * (double)attribute_AttrText.Value;
											}
										}
									}
									else if (id == "BlockValue")
									{
										this.m_comba += data.BlockValue.AsDouble() * (double)attribute_AttrText.Value;
									}
								}
								else if (id == "CritValue%")
								{
									this.m_comba += data.CritValue.AsDouble() * (double)attribute_AttrText.Value;
								}
							}
							else if (num <= 491629968U)
							{
								if (num != 484614851U)
								{
									if (num == 491629968U)
									{
										if (id == "Revenge%")
										{
											this.m_comba += data.RevengeRate.AsDouble() * (double)attribute_AttrText.Value;
										}
									}
								}
								else if (id == "Defence")
								{
									this.m_comba += data.GetDefence().AsDouble() * (double)attribute_AttrText.Value;
								}
							}
							else if (num != 495698666U)
							{
								if (num == 593219697U)
								{
									if (id == "FireReduction%")
									{
										this.m_comba += data.FireReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
									}
								}
							}
							else if (id == "BurnAdd%")
							{
								this.m_comba += data.FireAddPercent.AsDouble() * (double)attribute_AttrText.Value;
							}
						}
						else if (num <= 1069607449U)
						{
							if (num <= 745524681U)
							{
								if (num != 625009628U)
								{
									if (num != 726220194U)
									{
										if (num == 745524681U)
										{
											if (id == "SkillDamageAdd%")
											{
												this.m_comba += data.SkillDamageAddPercent.AsDouble() * (double)attribute_AttrText.Value;
											}
										}
									}
									else if (id == "ElectricReduction%")
									{
										this.m_comba += data.ElectricReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
									}
								}
								else if (id == "DamageReduction%")
								{
									this.m_comba += data.DamageReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
								}
							}
							else if (num != 796554613U)
							{
								if (num != 815484847U)
								{
									if (num == 1069607449U)
									{
										if (id == "HPMax")
										{
											this.m_comba += data.GetHpMax().AsDouble() * (double)attribute_AttrText.Value;
										}
									}
								}
								else if (id == "NormalDamageReduction%")
								{
									this.m_comba += data.NormalDamageReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
								}
							}
							else if (id == "TrueReduction%")
							{
								this.m_comba += data.TrueReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
							}
						}
						else if (num <= 1296369593U)
						{
							if (num != 1093169364U)
							{
								if (num != 1271254876U)
								{
									if (num == 1296369593U)
									{
										if (id == "ThunderReduction%")
										{
											this.m_comba += data.ThunderReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
										}
									}
								}
								else if (id == "IcicleAdd%")
								{
									this.m_comba += data.IcicleAddPercent.AsDouble() * (double)attribute_AttrText.Value;
								}
							}
							else if (id == "IgnoreDefence%")
							{
								this.m_comba += data.IgnoreDefencePercent.AsDouble() * (double)attribute_AttrText.Value;
							}
						}
						else if (num <= 1604840396U)
						{
							if (num != 1442762743U)
							{
								if (num == 1604840396U)
								{
									if (id == "RevengeDamageReduction%")
									{
										this.m_comba += data.RevengeDamageReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
									}
								}
							}
							else if (id == "ThunderAdd%")
							{
								this.m_comba += data.ThunderAddPercent.AsDouble() * (double)attribute_AttrText.Value;
							}
						}
						else if (num != 1736316382U)
						{
							if (num == 1811507796U)
							{
								if (id == "Miss%")
								{
									this.m_comba += data.Miss.AsDouble() * (double)attribute_AttrText.Value;
								}
							}
						}
						else if (id == "CritValueReduction%")
						{
							this.m_comba += data.CritValueReduction.AsDouble() * (double)attribute_AttrText.Value;
						}
					}
					else if (num <= 2825727063U)
					{
						if (num <= 2343121693U)
						{
							if (num <= 2092906184U)
							{
								if (num != 1883942262U)
								{
									if (num != 2041777376U)
									{
										if (num == 2092906184U)
										{
											if (id == "ElectricAdd%")
											{
												this.m_comba += data.ElectricAddPercent.AsDouble() * (double)attribute_AttrText.Value;
											}
										}
									}
									else if (id == "ComboDamageReduction%")
									{
										this.m_comba += data.ComboDamageReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
									}
								}
								else if (id == "ComboDamageAdd%")
								{
									this.m_comba += data.ComboDamageAddPercent.AsDouble() * (double)attribute_AttrText.Value;
								}
							}
							else if (num != 2159173842U)
							{
								if (num != 2234284006U)
								{
									if (num == 2343121693U)
									{
										if (id == "Attack")
										{
											this.m_comba += data.GetAttack().AsDouble() * (double)attribute_AttrText.Value;
										}
									}
								}
								else if (id == "IcicleReduction%")
								{
									this.m_comba += data.IceReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
								}
							}
							else if (id == "DamageAdd%")
							{
								this.m_comba += data.DamageAddPercent.AsDouble() * (double)attribute_AttrText.Value;
							}
						}
						else if (num <= 2504656766U)
						{
							if (num != 2398566297U)
							{
								if (num != 2400834834U)
								{
									if (num == 2504656766U)
									{
										if (id == "VampireRate%")
										{
											this.m_comba += data.VampireRate.AsDouble() * (double)attribute_AttrText.Value;
										}
									}
								}
								else if (id == "IceAdd%")
								{
									this.m_comba += data.IceAddPercent.AsDouble() * (double)attribute_AttrText.Value;
								}
							}
							else if (id == "PoisonReduction%")
							{
								this.m_comba += data.PoisonReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
							}
						}
						else if (num <= 2599398015U)
						{
							if (num != 2556341719U)
							{
								if (num == 2599398015U)
								{
									if (id == "FireAdd%")
									{
										this.m_comba += data.FireAddPercent.AsDouble() * (double)attribute_AttrText.Value;
									}
								}
							}
							else if (id == "PoisonAdd%")
							{
								this.m_comba += data.PoisonAddPercent.AsDouble() * (double)attribute_AttrText.Value;
							}
						}
						else if (num != 2781259547U)
						{
							if (num == 2825727063U)
							{
								if (id == "SkillCritRate%")
								{
									this.m_comba += data.SkillCritRate.AsDouble() * (double)attribute_AttrText.Value;
								}
							}
						}
						else if (id == "NormalCritRate%")
						{
							this.m_comba += data.NormalCritRate.AsDouble() * (double)attribute_AttrText.Value;
						}
					}
					else if (num <= 3255034125U)
					{
						if (num <= 2922930795U)
						{
							if (num != 2858588092U)
							{
								if (num != 2907078795U)
								{
									if (num == 2922930795U)
									{
										if (id == "NormalComboDamageAdd%")
										{
											this.m_comba += data.NormalComboDamageAddPercent.AsDouble() * (double)attribute_AttrText.Value;
										}
									}
								}
								else if (id == "NormalCombo%")
								{
									this.m_comba += data.NormalComboRate.AsDouble() * (double)attribute_AttrText.Value;
								}
							}
							else if (id == "PhysicalReduction%")
							{
								this.m_comba += data.PhysicalReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
							}
						}
						else if (num != 3067500950U)
						{
							if (num != 3168947780U)
							{
								if (num == 3255034125U)
								{
									if (id == "NormalDamageAdd%")
									{
										this.m_comba += data.NormalDamageAddPercent.AsDouble() * (double)attribute_AttrText.Value;
									}
								}
							}
							else if (id == "BurnReduction%")
							{
								this.m_comba += data.FireReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
							}
						}
						else if (id == "IgnoreMiss%")
						{
							this.m_comba += data.IgnoreMiss.AsDouble() * (double)attribute_AttrText.Value;
						}
					}
					else if (num <= 3819354180U)
					{
						if (num != 3692514014U)
						{
							if (num != 3742135179U)
							{
								if (num == 3819354180U)
								{
									if (id == "RevengeCount")
									{
										this.m_comba += data.RevengeCount.AsDouble() * (double)attribute_AttrText.Value;
									}
								}
							}
							else if (id == "SkillDamageReduction%")
							{
								this.m_comba += data.SkillDamageReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
							}
						}
						else if (id == "CritRate%")
						{
							this.m_comba += data.CritRate.AsDouble() * (double)attribute_AttrText.Value;
						}
					}
					else if (num <= 3907572195U)
					{
						if (num != 3850662367U)
						{
							if (num == 3907572195U)
							{
								if (id == "TrueAdd%")
								{
									this.m_comba += data.TrueAddPercent.AsDouble() * (double)attribute_AttrText.Value;
								}
							}
						}
						else if (id == "BigSkillDamageReduction%")
						{
							this.m_comba += data.BigSkillDamageReductionPercent.AsDouble() * (double)attribute_AttrText.Value;
						}
					}
					else if (num != 4136069348U)
					{
						if (num == 4243823599U)
						{
							if (id == "ChainComboCount")
							{
								this.m_comba += data.ChainComboCount.AsDouble() * (double)attribute_AttrText.Value;
							}
						}
					}
					else if (id == "DefenseReductionCritValue%")
					{
						this.m_comba += data.DefenseReductionCritValue.AsDouble() * (double)attribute_AttrText.Value;
					}
				}
			}
		}

		public void MathSkillCombat(LocalModelManager localModelManager, List<int> skills)
		{
			if (skills != null)
			{
				for (int i = 0; i < skills.Count; i++)
				{
					int num = skills[i];
					GameSkill_skill elementById = localModelManager.GetGameSkill_skillModelInstance().GetElementById(num);
					if (elementById != null)
					{
						this.m_comba += (double)elementById.combat;
					}
				}
			}
		}

		private double m_comba;
	}
}
