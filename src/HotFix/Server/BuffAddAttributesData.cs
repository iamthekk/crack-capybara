using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Server
{
	public class BuffAddAttributesData
	{
		public void AddAttributes(GameBuff_buff buff, SMemberBase owner, SMemberBase attacker)
		{
			string[] array;
			string[] array2;
			this.GetAttributeArgs(owner, attacker, out array, out array2);
			List<MergeAttributeData> addMergeAttributeData = buff.GetAddMergeAttributeData(array, array2);
			owner.memberData.attribute.MergeAttributes(addMergeAttributeData, false);
			for (int i = 0; i < addMergeAttributeData.Count; i++)
			{
				this.m_datas.Add(addMergeAttributeData[i]);
			}
		}

		public void RemoveAttributes(GameBuff_buff buff, SMemberBase owner, SMemberBase attacker)
		{
			owner.memberData.attribute.MergeAttributes(this.m_datas, true);
			this.m_datas.Clear();
		}

		public void AddAttributesOnce(GameBuff_buff buff, SMemberBase owner, SMemberBase attacker)
		{
			string[] array;
			string[] array2;
			this.GetAttributeArgs(owner, attacker, out array, out array2);
			List<MergeAttributeData> addMergeAttributeOnceData = buff.GetAddMergeAttributeOnceData(array, array2);
			owner.memberData.attribute.MergeAttributes(addMergeAttributeOnceData, false);
		}

		private void GetAttributeArgs(SMemberBase owner, SMemberBase attacker, out string[] argsName, out string[] args)
		{
			argsName = new string[] { "DefenderAttack", "DefenderDefense", "DefenderHPMax", "DefenderCurHP", "DefenderLossHP", "AttackerAttack", "AttackerDefense", "AttackerHPMax", "AttackerHP", "AttackerLossHP" };
			args = new string[]
			{
				owner.memberData.attribute.GetAttack().AsLong().ToString(),
				owner.memberData.attribute.GetDefence().AsLong().ToString(),
				owner.memberData.attribute.GetHpMax().AsLong().ToString(),
				owner.memberData.CurHP.AsLong().ToString(),
				owner.memberData.CurLossHp.AsLong().ToString(),
				attacker.memberData.attribute.GetAttack().AsLong().ToString(),
				attacker.memberData.attribute.GetDefence().AsLong().ToString(),
				attacker.memberData.attribute.GetHpMax().AsLong().ToString(),
				attacker.memberData.CurHP.AsLong().ToString(),
				attacker.memberData.CurLossHp.AsLong().ToString()
			};
		}

		private List<MergeAttributeData> m_datas = new List<MergeAttributeData>();
	}
}
