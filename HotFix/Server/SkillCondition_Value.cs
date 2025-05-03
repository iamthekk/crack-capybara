using System;
using System.Collections.Generic;

namespace Server
{
	public class SkillCondition_Value : ConditionBase
	{
		public override void OnInit()
		{
		}

		public override void OnDeInit()
		{
		}

		public override void SetParameter(string parameter)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameter)) ? JsonManager.ToObject<SkillCondition_Value.Data>(parameter) : new SkillCondition_Value.Data());
		}

		public override bool IsMatchCondition()
		{
			List<SMemberBase> list = new List<SMemberBase>();
			TargetType target = this.m_data.target;
			if (target != TargetType.Spellcasters)
			{
				if (target != TargetType.SkillSelect)
				{
					HLog.LogError("数值判断条件 m_data.target is error.");
				}
				else
				{
					this.m_curSkill.SetSelectTargetData();
					List<SkillSelectTargetData> curSelectTargetDatas = this.m_curSkill.CurSelectTargetDatas;
					if (curSelectTargetDatas != null)
					{
						for (int i = 0; i < curSelectTargetDatas.Count; i++)
						{
							list.Add(curSelectTargetDatas[i].m_target);
						}
					}
				}
			}
			else
			{
				list.Add(this.m_curSkill.Owner);
			}
			if (list.Count <= 0)
			{
				return false;
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (this.m_data.valueType == ValueType.Percent)
				{
					float num = list[j].memberData.CurHP.AsFloat() * 100f / list[j].memberData.attribute.HPMax.AsFloat();
					if (this.m_data.sign == SignType.LessOrEqual)
					{
						if (num <= this.m_data.value)
						{
							return true;
						}
					}
					else if (this.m_data.sign == SignType.Greater && num > this.m_data.value)
					{
						return true;
					}
				}
				else if (this.m_data.valueType == ValueType.Constant)
				{
					float num2 = list[j].memberData.CurHP.AsFloat();
					if (this.m_data.sign == SignType.LessOrEqual)
					{
						if (num2 <= this.m_data.value)
						{
							return true;
						}
					}
					else if (this.m_data.sign == SignType.Greater && num2 > this.m_data.value)
					{
						return true;
					}
				}
			}
			return false;
		}

		public SkillCondition_Value.Data m_data;

		public class Data
		{
			public TargetType target;

			public SignType sign;

			public ValueType valueType;

			public float value;
		}
	}
}
