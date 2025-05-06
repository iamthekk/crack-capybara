using System;
using System.Collections.Generic;
using System.Linq;
using LocalModels.Bean;

namespace Server
{
	public class SMemberData
	{
		public CardData cardData
		{
			get
			{
				return this.m_cardData;
			}
		}

		public int m_ID
		{
			get
			{
				return this.cardData.m_memberID;
			}
		}

		public int CurComboCount
		{
			get
			{
				return this.m_curComboCount;
			}
			set
			{
				this.m_curComboCount = value;
			}
		}

		public FP CurHP
		{
			get
			{
				return this.m_curHP;
			}
			set
			{
				this.m_curHP = FPMath.Clamp(value, FP._0, this.attribute.GetHpMax());
			}
		}

		public FP HPPercent
		{
			get
			{
				return this.CurHP / this.attribute.GetHpMax();
			}
		}

		public FP CurLossHp
		{
			get
			{
				return this.attribute.GetHpMax() - this.CurHP;
			}
		}

		public bool IsDeath
		{
			get
			{
				return this.CurHP <= FP._0;
			}
		}

		public FP CurRecharge
		{
			get
			{
				return this.m_curRecharge;
			}
			set
			{
				this.m_curRecharge = FPMath.Clamp(value, FP._0, this.attribute.RechargeMax);
			}
		}

		public FP GetCurLegacyPower(int skillId)
		{
			FP fp;
			if (!this.CurLegacyPowerDict.TryGetValue(skillId, out fp))
			{
				return FP._0;
			}
			return fp;
		}

		public FP GetMaxLegacyPower(int skillId)
		{
			FP fp;
			if (!this.MaxLegacyPowerDict.TryGetValue(skillId, out fp))
			{
				return FP._0;
			}
			return fp;
		}

		public FP SetCurLegacyPower(int skillId, FP changeValue)
		{
			if (this.CurLegacyPowerDict.ContainsKey(skillId))
			{
				FP fp = this.CurLegacyPowerDict[skillId] + changeValue;
				this.CurLegacyPowerDict[skillId] = FPMath.Clamp(fp, FP._0, this.GetMaxLegacyPower(skillId));
				return this.CurLegacyPowerDict[skillId];
			}
			return FP._0;
		}

		public void SetMaxLegacyPower(int skillId, FP value)
		{
			this.MaxLegacyPowerDict[skillId] = value;
		}

		public FP CurShield
		{
			get
			{
				return this.m_curShield;
			}
			set
			{
				this.m_curShield = FPMath.Clamp(value, FP._0, 1 * this.attribute.GetHpMax());
			}
		}

		public FP CurCombat
		{
			get
			{
				return this.m_curCombat;
			}
			set
			{
				this.m_curCombat = FPMath.Max(value, FP._1);
			}
		}

		public bool IsHpGreater60Percent
		{
			get
			{
				return this.HPPercent > FP._060;
			}
		}

		public MemberCamp Camp
		{
			get
			{
				return this.cardData.m_camp;
			}
		}

		public MemberPos PosIndex
		{
			get
			{
				return this.cardData.m_posIndex;
			}
		}

		public MemberMeatType MeatType
		{
			get
			{
				return this.m_meatType;
			}
			set
			{
				this.m_meatType = value;
			}
		}

		public SMemberBase sMemberBase
		{
			get
			{
				return this.m_sMemberBase;
			}
			private set
			{
				this.m_sMemberBase = value;
			}
		}

		public MemberAttributeData attribute
		{
			get
			{
				return this.m_attribute;
			}
		}

		public bool IsReviveUsed
		{
			get
			{
				return this.m_isReviveUsed;
			}
			set
			{
				this.m_isReviveUsed = value;
			}
		}

		public SMemberData()
		{
			this.m_attribute = new MemberAttributeData();
		}

		public void SetCardData(CardData cardData)
		{
			this.m_cardData = cardData;
			this.m_attribute.CopyFrom(cardData.m_memberAttributeData);
			if (this.m_attribute.HPMax <= FP._0)
			{
				HLog.LogError("cardData is Error. HpMax <= 0");
			}
			if (this.m_attribute.Attack <= FP._0)
			{
				HLog.LogError("cardData is Error. Attack <= 0");
			}
		}

		public void UpdateLegacyPowerMax()
		{
			List<SSkillBase> list = this.sMemberBase.skillFactory.allSkill.FindAll((SSkillBase s) => s.skillData.m_freedType == SkillFreedType.Legacy);
			if (list != null && list.Count > 0)
			{
				foreach (SSkillBase sskillBase in list)
				{
					this.CurLegacyPowerDict[sskillBase.skillData.m_id] = FP._0;
					this.MaxLegacyPowerDict[sskillBase.skillData.m_id] = sskillBase.skillData.m_legacyPowerMax;
				}
			}
		}

		public void SetTableData(GameMember_member member)
		{
			this.RegisterEvents();
			this.m_RoleType = (MemberRoleType)member.roleType;
		}

		public void SetSMmemberBase(SMemberBase sMemberBase)
		{
			this.m_sMemberBase = sMemberBase;
			this.attribute.CurBattleMode = sMemberBase.m_controller.InData.m_battleMode;
		}

		public void Clear()
		{
			this.UnRegisterEvents();
		}

		private void RegisterEvents()
		{
			MemberAttributeData attribute = this.m_attribute;
			attribute.m_onRechargeUpdate = (Action<FP>)Delegate.Combine(attribute.m_onRechargeUpdate, new Action<FP>(this.ChangeRecharge));
			MemberAttributeData attribute2 = this.m_attribute;
			attribute2.m_onLegacyPowerUpdate = (Action<FP>)Delegate.Combine(attribute2.m_onLegacyPowerUpdate, new Action<FP>(this.ChangeAllLegacyPower));
			MemberAttributeData attribute3 = this.m_attribute;
			attribute3.m_onShieldValueUpdate = (Action<FP>)Delegate.Combine(attribute3.m_onShieldValueUpdate, new Action<FP>(this.ChangeShield));
			MemberAttributeData attribute4 = this.m_attribute;
			attribute4.m_onHpMaxUpdate = (Action<FP>)Delegate.Combine(attribute4.m_onHpMaxUpdate, new Action<FP>(this.ChangeMaxHp));
		}

		private void UnRegisterEvents()
		{
			MemberAttributeData attribute = this.m_attribute;
			attribute.m_onRechargeUpdate = (Action<FP>)Delegate.Remove(attribute.m_onRechargeUpdate, new Action<FP>(this.ChangeRecharge));
			MemberAttributeData attribute2 = this.m_attribute;
			attribute2.m_onLegacyPowerUpdate = (Action<FP>)Delegate.Remove(attribute2.m_onLegacyPowerUpdate, new Action<FP>(this.ChangeAllLegacyPower));
			MemberAttributeData attribute3 = this.m_attribute;
			attribute3.m_onShieldValueUpdate = (Action<FP>)Delegate.Remove(attribute3.m_onShieldValueUpdate, new Action<FP>(this.ChangeShield));
			MemberAttributeData attribute4 = this.m_attribute;
			attribute4.m_onHpMaxUpdate = (Action<FP>)Delegate.Remove(attribute4.m_onHpMaxUpdate, new Action<FP>(this.ChangeMaxHp));
		}

		private void MathMemberAttributes()
		{
			List<MergeAttributeData> mergeAttributeData = this.m_baseAttributes.GetMergeAttributeData();
			this.m_attribute.MergeAttributes(mergeAttributeData, false);
		}

		public void ChangeHp(FP changeValue, bool isBattleHurt)
		{
			this.CurHP = MathTools.Clamp(this.CurHP + changeValue, FP._0, this.attribute.GetHpMax());
			if (this.CurHP <= FP._0 && this.m_attribute.ReviveCount > this.sMemberBase.memberFactory.RevivedCount)
			{
				this.IsReviveUsed = true;
				this.sMemberBase.memberFactory.RevivedCount++;
				FP fp = (this.CurHP = this.m_attribute.HPMax * this.m_attribute.ReviveHpPercent);
				Action<FP> action = this.onReviveCallback;
				if (action != null)
				{
					action(fp);
				}
			}
			if (changeValue != FP._0)
			{
				Action<FP> onChangeHP = this.m_onChangeHP;
				if (onChangeHP != null)
				{
					onChangeHP(changeValue);
				}
			}
			if (changeValue < FP._0 && this.CurHP > FP._0 && isBattleHurt)
			{
				this.ChangeAllLegacyPower(((-changeValue / this.attribute.GetHpMax() * FP._100 / SBattleConst.DamageAddLegacyPower4Percent).AsLong() * SBattleConst.DamageAddLegacyPower4Value).AsLong());
			}
		}

		public void ChangeMaxHp(FP beforeHpMax)
		{
			FP fp = this.attribute.GetHpMax() - beforeHpMax;
			if (fp > 0)
			{
				this.ChangeHp(fp, false);
				return;
			}
			this.ChangeHp(0, false);
		}

		public void ChangeRecharge(FP changeValue)
		{
			if (!(this.m_attribute.NoEnergyRecovery > FP._0))
			{
				this.CurRecharge += changeValue;
				if (!changeValue.Equals(FP._0))
				{
					Action<string, FP, FP, string> action = this.onAttributeChanged;
					if (action == null)
					{
						return;
					}
					action("Recharge", this.CurRecharge, changeValue, null);
				}
			}
		}

		public void ChangeLegacyPower(int skillId, FP changeValue)
		{
			if (this.CurLegacyPowerDict.ContainsKey(skillId))
			{
				FP fp = ((changeValue > FP._0) ? (changeValue * (FP._1 + this.attribute.LegacyPowerAddPercent)) : changeValue);
				FP fp2 = this.SetCurLegacyPower(skillId, fp);
				if (!fp.Equals(FP._0))
				{
					Action<string, FP, FP, string> action = this.onAttributeChanged;
					if (action == null)
					{
						return;
					}
					action("LegacyPower", fp2, fp, string.Format("{0}", skillId));
				}
			}
		}

		public void ChangeAllLegacyPower(FP changeValue)
		{
			if (!changeValue.Equals(FP._0))
			{
				foreach (int num in this.CurLegacyPowerDict.Keys.ToList<int>())
				{
					this.ChangeLegacyPower(num, changeValue);
				}
			}
		}

		public void ChangeShield(FP changeValue)
		{
			FP curShield = this.CurShield;
			this.CurShield += changeValue;
			if (!changeValue.Equals(FP._0))
			{
				bool flag = this.m_attribute.IsDurianShield();
				bool flag2 = this.m_attribute.IsThunderShield();
				if (flag)
				{
					Action<string, FP, FP, string> action = this.onAttributeChanged;
					if (action == null)
					{
						return;
					}
					action("ShieldDurian", this.CurShield, changeValue, null);
					return;
				}
				else if (flag2)
				{
					Action<string, FP, FP, string> action2 = this.onAttributeChanged;
					if (action2 == null)
					{
						return;
					}
					action2("ShieldThunder", this.CurShield, changeValue, null);
					return;
				}
				else
				{
					Action<string, FP, FP, string> action3 = this.onAttributeChanged;
					if (action3 == null)
					{
						return;
					}
					action3("Shield", this.CurShield, changeValue, null);
				}
			}
		}

		public void LogAttributes()
		{
			BattleLogHelper.LogSMemberAttributes(this, "[SMemberData] ");
		}

		private CardData m_cardData;

		private int m_curComboCount;

		public List<int> m_skillIDs;

		public string m_baseAttributes = string.Empty;

		public MemberRoleType m_RoleType;

		private FP m_curHP = FP._0;

		private FP m_curRecharge = FP._0;

		public Dictionary<int, FP> CurLegacyPowerDict = new Dictionary<int, FP>();

		public Dictionary<int, FP> MaxLegacyPowerDict = new Dictionary<int, FP>();

		private FP m_curShield = FP._0;

		private FP m_curCombat = FP._1;

		private MemberMeatType m_meatType;

		private SMemberBase m_sMemberBase;

		private MemberAttributeData m_attribute;

		private bool m_isReviveUsed;

		public Action<string, FP, FP, string> onAttributeChanged;

		public Action<FP> onReviveCallback;

		public Action<FP> m_onChangeHP;
	}
}
