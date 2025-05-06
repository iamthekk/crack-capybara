using System;
using System.Collections.Generic;

namespace Server
{
	public class SkillCondition_Buff : ConditionBase
	{
		public override void OnInit()
		{
		}

		public override void OnDeInit()
		{
		}

		public override void SetParameter(string parameter)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameter)) ? JsonManager.ToObject<SkillCondition_Buff.Data>(parameter) : new SkillCondition_Buff.Data());
		}

		public override bool IsMatchCondition()
		{
			List<SMemberBase> list = new List<SMemberBase>();
			TargetType target = this.m_data.target;
			if (target != TargetType.Spellcasters)
			{
				if (target == TargetType.SkillSelect)
				{
					List<SkillSelectTargetData> curSelectTargetDatas = this.m_curSkill.CurSelectTargetDatas;
					for (int i = 0; i < curSelectTargetDatas.Count; i++)
					{
						list.Add(curSelectTargetDatas[i].m_target);
					}
				}
			}
			else
			{
				list.Add(this.m_curSkill.Owner);
			}
			if (list.Count > 0)
			{
				return false;
			}
			if (this.m_data.type == 1)
			{
				int num = 0;
				if (num < this.m_data.values.Length)
				{
					return this.m_curSkill.Owner.buffFactory.GetBuffsByOverlayType(this.m_data.values).Count > 0;
				}
			}
			else if (this.m_data.type == 2)
			{
				int num2 = 0;
				if (num2 < this.m_data.values.Length)
				{
					return this.m_curSkill.Owner.buffFactory.GetBuffs(this.m_data.values).Count > 0;
				}
			}
			return false;
		}

		public SkillCondition_Buff.Data m_data;

		[Serializable]
		public class Data
		{
			public TargetType target;

			public int type;

			public int[] values;
		}
	}
}
