using System;
using System.Collections.Generic;

namespace Server
{
	public class HurtAttributeParameterMethods
	{
		public void OnRefreshByAttacker(SMemberBase attacker, SMemberBase defender, Dictionary<HurtType, HurtData> hurtDatas)
		{
			this.m_fields.Add("AttackerAttack");
			this.m_valuse.Add(attacker.memberData.attribute.GetAttack().AsLong().ToString());
			this.m_fields.Add("AttackerDefense");
			this.m_valuse.Add(attacker.memberData.attribute.GetDefence().AsLong().ToString());
			this.m_fields.Add("AttackerHPMax");
			this.m_valuse.Add(attacker.memberData.attribute.GetHpMax().AsLong().ToString());
			this.m_fields.Add("AttackerCurHP");
			this.m_valuse.Add(attacker.memberData.CurHP.AsLong().ToString());
			this.m_fields.Add("AttackerLossHP");
			this.m_valuse.Add(attacker.memberData.CurLossHp.AsLong().ToString());
			this.m_fields.Add("FinalDefense");
			FP finalDefence = defender.memberData.attribute.GetFinalDefence(attacker.memberData.attribute.IgnoreDefencePercent);
			this.m_valuse.Add(finalDefence.AsLong().ToString());
			this.m_fields.Add("DefenderAttack");
			this.m_valuse.Add(defender.memberData.attribute.GetAttack().AsLong().ToString());
			this.m_fields.Add("DefenderDefense");
			this.m_valuse.Add(defender.memberData.attribute.GetDefence().AsLong().ToString());
			this.m_fields.Add("DefenderHPMax");
			this.m_valuse.Add(defender.memberData.attribute.GetHpMax().AsLong().ToString());
			this.m_fields.Add("DefenderCurHP");
			this.m_valuse.Add(defender.memberData.CurHP.AsLong().ToString());
			this.m_fields.Add("DefenderLossHP");
			this.m_valuse.Add(defender.memberData.CurLossHp.AsLong().ToString());
		}

		public void AddParameter(string field, string value)
		{
			if (field.Equals(string.Empty) || value.Equals(string.Empty))
			{
				return;
			}
			this.m_fields.Add(field);
			this.m_valuse.Add(value);
		}

		public List<string> m_fields = new List<string>();

		public List<string> m_valuse = new List<string>();
	}
}
