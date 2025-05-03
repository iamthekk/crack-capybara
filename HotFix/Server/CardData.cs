using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
	public class CardData
	{
		public List<int> skillIDs
		{
			get
			{
				return this.m_skillIDs;
			}
		}

		public bool IsEnemyPlayer { get; set; }

		public SPetData m_petData { get; set; }

		public CardData()
		{
		}

		public CardData(int instanceID, int memberID, MemberCamp camp)
		{
			this.m_instanceID = instanceID;
			this.m_memberID = memberID;
			this.m_camp = camp;
		}

		public CardData(int instanceID, int memberID, MemberCamp camp, MemberPos posIndex, bool isMainMember = false)
		{
			this.m_instanceID = instanceID;
			this.m_memberID = memberID;
			this.m_camp = camp;
			this.m_posIndex = posIndex;
			this.m_isMainMember = isMainMember;
		}

		public CardData(int rowID, int instanceID, int memberID, MemberCamp camp, bool isMainMember = false)
		{
			this.m_rowID = rowID;
			this.m_instanceID = instanceID;
			this.m_memberID = memberID;
			this.m_camp = camp;
			this.m_isMainMember = isMainMember;
		}

		public CardData(int rowID, int instanceID, int memberID, MemberCamp camp, MemberPos posIndex, bool isMainMember = false)
		{
			this.m_rowID = rowID;
			this.m_instanceID = instanceID;
			this.m_memberID = memberID;
			this.m_camp = camp;
			this.m_posIndex = posIndex;
			this.m_isMainMember = isMainMember;
		}

		public void SetMemberRace(MemberRace race)
		{
			this.m_memberRace = race;
		}

		public bool IsPet
		{
			get
			{
				return this.m_memberRace == MemberRace.Pet;
			}
		}

		public void AddSkill(int skillId)
		{
			if (this.m_skillIDs == null)
			{
				this.m_skillIDs = new List<int>();
			}
			this.m_skillIDs.Add(skillId);
		}

		public void AddSkill(List<int> skillIds)
		{
			if (this.m_skillIDs == null)
			{
				this.m_skillIDs = new List<int>();
			}
			this.m_skillIDs.AddRange(skillIds);
		}

		public void UpdateSkills(List<int> skillIds)
		{
			if (skillIds != null)
			{
				if (this.m_skillIDs != null)
				{
					this.m_skillIDs.Clear();
				}
				else
				{
					this.m_skillIDs = new List<int>();
				}
				this.m_skillIDs.AddRange(skillIds);
			}
		}

		public void CloneFrom(CardData cardData)
		{
			if (cardData == null)
			{
				return;
			}
			this.m_rowID = cardData.m_rowID;
			this.m_memberID = cardData.m_memberID;
			this.m_instanceID = cardData.m_instanceID;
			this.m_camp = cardData.m_camp;
			this.m_posIndex = cardData.m_posIndex;
			this.m_memberRace = cardData.m_memberRace;
			this.m_isMainMember = cardData.m_isMainMember;
			this.m_petData = cardData.m_petData;
			this.IsEnemyPlayer = cardData.IsEnemyPlayer;
			this.RefreshCardData(cardData.m_skillIDs, cardData.m_memberAttributeData, cardData.m_curHp, cardData.m_curEnergy, cardData.m_curLegacyPower, cardData.m_reviveUsed);
		}

		public void RefreshCardData(List<int> addSkillIDs, MemberAttributeData attrData, FP curHp, FP curRecharge, Dictionary<int, FP> curLegacyPower, bool isUsedRevive)
		{
			this.m_memberAttributeData.CopyFrom(attrData);
			this.UpdateSkills(addSkillIDs);
			this.m_curHp = curHp;
			this.m_curEnergy = curRecharge;
			this.m_reviveUsed = isUsedRevive;
		}

		public void ConvertBaseData()
		{
			this.m_memberAttributeData.ConvertBaseData();
		}

		public void AttributeCopy(MemberAttributeData attrData)
		{
			this.m_memberAttributeData.CopyFrom(attrData);
		}

		public void UpdateAttribute(List<MergeAttributeData> mergeAttributeDatas)
		{
			if (mergeAttributeDatas == null)
			{
				return;
			}
			this.m_memberAttributeData = new MemberAttributeData();
			this.m_memberAttributeData.MergeAttributes(mergeAttributeDatas, false);
		}

		public void AddAttributes(List<MergeAttributeData> mergeAttributeDatas)
		{
			MemberAttributeData memberAttributeData = this.m_memberAttributeData;
			if (memberAttributeData == null)
			{
				return;
			}
			memberAttributeData.MergeAttributes(mergeAttributeDatas, false);
		}

		public string Log()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("m_rowID={0},", this.m_rowID));
			stringBuilder.Append(string.Format("m_instanceID={0},", this.m_instanceID));
			stringBuilder.Append(string.Format("m_memberID={0},", this.m_memberID));
			stringBuilder.Append(string.Format("m_camp={0},", this.m_camp));
			stringBuilder.Append(string.Format("m_index={0},", this.m_posIndex));
			stringBuilder.Append(string.Format("m_isMainMember={0},", this.m_isMainMember));
			stringBuilder.Append("\n");
			stringBuilder.Append(string.Format("SkillCount={0},", this.m_skillIDs.Count));
			stringBuilder.Append("SkillIDs:");
			for (int i = 0; i < this.m_skillIDs.Count; i++)
			{
				if (!i.Equals(0))
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(string.Format("{0}", this.m_skillIDs[i]));
			}
			stringBuilder.Append(" ");
			stringBuilder.Append("\n");
			stringBuilder.AppendLine(string.Format("AttackBasic={0},({1})", this.m_memberAttributeData.Attack, this.m_memberAttributeData.Attack.ToString()));
			stringBuilder.AppendLine(string.Format("AttackPercent={0},({1})", this.m_memberAttributeData.AttackPercent, this.m_memberAttributeData.AttackPercent.ToString()));
			stringBuilder.AppendLine(string.Format("Attack={0},({1})", this.m_memberAttributeData.GetAttack(), this.m_memberAttributeData.GetAttack().ToString()));
			stringBuilder.AppendLine(string.Format("HPMaxBasic={0},({1})", this.m_memberAttributeData.HPMax, this.m_memberAttributeData.HPMax.ToString()));
			stringBuilder.AppendLine(string.Format("HPMaxPercent={0},({1})", this.m_memberAttributeData.HPMaxPercent, this.m_memberAttributeData.HPMaxPercent.ToString()));
			stringBuilder.AppendLine(string.Format("HPMax={0},({1})", this.m_memberAttributeData.GetHpMax(), this.m_memberAttributeData.GetHpMax().ToString()));
			stringBuilder.AppendLine(string.Format("DefenceBasic={0},({1})", this.m_memberAttributeData.Defence, this.m_memberAttributeData.Defence.ToString()));
			stringBuilder.AppendLine(string.Format("DefencePercent={0},({1})", this.m_memberAttributeData.DefencePercent, this.m_memberAttributeData.DefencePercent.ToString()));
			stringBuilder.AppendLine(string.Format("Defence={0},({1})", this.m_memberAttributeData.GetDefence(), this.m_memberAttributeData.GetDefence().ToString()));
			stringBuilder.AppendLine(string.Format("CritRate={0},", this.m_memberAttributeData.CritRate));
			stringBuilder.AppendLine(string.Format("CritValue={0},", this.m_memberAttributeData.CritValue));
			stringBuilder.AppendLine(string.Format("CritValueReduction={0},", this.m_memberAttributeData.CritValueReduction));
			stringBuilder.AppendLine(string.Format("NormalCritRate={0},", this.m_memberAttributeData.NormalCritRate));
			stringBuilder.AppendLine(string.Format("SkillCritRate={0},", this.m_memberAttributeData.SkillCritRate));
			stringBuilder.AppendLine(string.Format("DamageAddPercent={0},", this.m_memberAttributeData.DamageAddPercent));
			stringBuilder.AppendLine(string.Format("DamageReductionPercent={0},", this.m_memberAttributeData.DamageReductionPercent));
			stringBuilder.AppendLine(string.Format("Miss={0},", this.m_memberAttributeData.Miss));
			stringBuilder.AppendLine(string.Format("MissDoublePercent={0},", this.m_memberAttributeData.MissDoublePercent));
			stringBuilder.AppendLine(string.Format("IgnoreMiss={0},", this.m_memberAttributeData.IgnoreMiss));
			stringBuilder.AppendLine(string.Format("NormalComboRate={0},", this.m_memberAttributeData.NormalComboRate));
			stringBuilder.AppendLine(string.Format("NormalComboCount={0},", this.m_memberAttributeData.NormalComboCount));
			stringBuilder.AppendLine(string.Format("NormalComboDamageAddPercent={0},", this.m_memberAttributeData.NormalComboDamageAddPercent));
			stringBuilder.AppendLine(string.Format("NormalComboCritAddPercent={0},", this.m_memberAttributeData.NormalComboCritAddPercent));
			stringBuilder.AppendLine(string.Format("DefenseReductionCritValue={0},", this.m_memberAttributeData.DefenseReductionCritValue));
			stringBuilder.AppendLine(string.Format("DamageAddPercent={0},", this.m_memberAttributeData.DamageAddPercent));
			stringBuilder.AppendLine(string.Format("DamageReductionPercent={0},", this.m_memberAttributeData.DamageReductionPercent));
			stringBuilder.AppendLine(string.Format("RevengeRate={0},", this.m_memberAttributeData.RevengeRate));
			stringBuilder.AppendLine(string.Format("RevengeCount={0},", this.m_memberAttributeData.RevengeCount));
			stringBuilder.AppendLine(string.Format("VampireRate={0},", this.m_memberAttributeData.VampireRate));
			stringBuilder.AppendLine(string.Format("VampireAddPercent={0},", this.m_memberAttributeData.VampireAddPercent));
			stringBuilder.AppendLine(string.Format("CounterVampireRate={0},", this.m_memberAttributeData.CounterVampireRate));
			stringBuilder.AppendLine(string.Format("ReviveCount={0},", this.m_memberAttributeData.ReviveCount));
			stringBuilder.AppendLine(string.Format("ReviveHpPercent={0},", this.m_memberAttributeData.ReviveHpPercent));
			return stringBuilder.ToString();
		}

		public int m_rowID;

		public int m_memberID;

		public int m_instanceID;

		public MemberCamp m_camp;

		public MemberPos m_posIndex;

		public MemberRace m_memberRace;

		public bool m_isMainMember;

		public FP m_curHp = FP._1 * -1;

		public FP m_curEnergy = FP._1 * -1;

		public Dictionary<int, FP> m_curLegacyPower = new Dictionary<int, FP>();

		public bool m_reviveUsed;

		private List<int> m_skillIDs = new List<int>();

		public MemberAttributeData m_memberAttributeData = new MemberAttributeData();
	}
}
