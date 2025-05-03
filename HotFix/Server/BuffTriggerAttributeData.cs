using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Server
{
	public class BuffTriggerAttributeData
	{
		public void RefreshAttributes(GameBuff_buff buff, SMemberBase owner, SMemberBase attacker, int layer)
		{
			this.hurtDatas.Clear();
			HurtAttributeParameterMethods hurtAttributeParameterMethods = new HurtAttributeParameterMethods();
			SMemberBase smemberBase = (attacker.memberData.cardData.IsPet ? attacker.memberFactory.GetMainMember(attacker.memberData.Camp) : attacker);
			hurtAttributeParameterMethods.OnRefreshByAttacker(smemberBase, owner, null);
			hurtAttributeParameterMethods.AddParameter("Layer", layer.ToString());
			List<MergeAttributeData> mergeTriggerAttributeData = buff.GetMergeTriggerAttributeData(hurtAttributeParameterMethods.m_fields, hurtAttributeParameterMethods.m_valuse);
			this.MergeAttributes(mergeTriggerAttributeData, false);
		}

		public void MergeAttributes(List<MergeAttributeData> list, bool isReverse = false)
		{
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.MergeAttribute(list[i], isReverse);
			}
		}

		public void MergeAttribute(MergeAttributeData data, bool isReverse = false)
		{
			if (data == null)
			{
				return;
			}
			string header = data.Header;
			if (header == "Energy")
			{
				this.energy += ((!isReverse) ? data.Value : (-data.Value));
				return;
			}
			HurtType hurtType = GameExpand.AttributeToHurtType(header);
			HurtData hurtData;
			this.hurtDatas.TryGetValue(hurtType, out hurtData);
			if (hurtData == null)
			{
				hurtData = new HurtData(hurtType, default(FP));
			}
			hurtData.m_attack = data.Value;
			this.hurtDatas[hurtType] = hurtData;
		}

		public Dictionary<HurtType, HurtData> hurtDatas = new Dictionary<HurtType, HurtData>();

		public FP energy;

		public class AttributesField
		{
			public const string Energy = "Energy";
		}

		public class AttributesParameter
		{
			public const string Layer = "Layer";
		}
	}
}
