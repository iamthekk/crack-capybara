using System;

namespace Server
{
	public class SkillCondition_Anger : ConditionBase
	{
		public override void OnInit()
		{
		}

		public override void OnDeInit()
		{
			this.m_data = null;
		}

		public override void SetParameter(string parameter)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameter)) ? JsonManager.ToObject<SkillCondition_Anger.Data>(parameter) : new SkillCondition_Anger.Data());
		}

		public override bool IsMatchCondition()
		{
			float num;
			if (this.m_data.valueType == ValueType.Constant)
			{
				num = this.m_curSkill.Owner.memberData.CurRecharge.AsFloat();
			}
			else
			{
				num = this.m_curSkill.Owner.memberData.CurRecharge.AsFloat() / this.m_curSkill.Owner.memberData.attribute.RechargeMax.AsFloat() * 100f;
			}
			return num >= (float)this.m_data.value;
		}

		public SkillCondition_Anger.Data m_data;

		public class Data
		{
			public ValueType valueType;

			public int value;
		}
	}
}
