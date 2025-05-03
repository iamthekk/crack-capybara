using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Server
{
	public abstract class BaseSkillTrigger
	{
		public void SetSkill(SSkillBase skill)
		{
			this.m_skill = skill;
		}

		public virtual void InitCondition(string parameter)
		{
			if (parameter.Equals(string.Empty))
			{
				return;
			}
			List<int> listInt = parameter.GetListInt('&');
			for (int i = 0; i < listInt.Count; i++)
			{
				GameSkill_skillCondition elementById = this.m_skill.Owner.m_controller.Table.GetGameSkill_skillConditionModelInstance().GetElementById(listInt[i]);
				if (listInt[i] != 0)
				{
					ConditionBase conditionBase;
					switch (elementById.type)
					{
					case 1:
						conditionBase = new SkillCondition_Default();
						break;
					case 2:
						conditionBase = new SkillCondition_Anger();
						break;
					case 3:
						conditionBase = new SkillCondition_Value();
						break;
					case 4:
						conditionBase = new SkillCondition_Buff();
						break;
					case 5:
						conditionBase = new SkillCondition_Skill();
						break;
					case 6:
						conditionBase = new SkillCondition_BeAttack();
						break;
					case 7:
						conditionBase = new SkillCondition_Probability();
						break;
					case 8:
						conditionBase = new SkillCondition_BaiscSkill();
						break;
					case 9:
						conditionBase = new SkillCondition_ComboCount();
						break;
					case 10:
						conditionBase = new SkillCondition_NotFullHp();
						break;
					case 11:
						conditionBase = new SkillCondition_ShieldExist();
						break;
					case 12:
						conditionBase = new SkillCondition_TriggerKnife();
						break;
					case 13:
						conditionBase = new SkillCondition_TriggerFallingSword();
						break;
					case 14:
						conditionBase = new SkillCondition_LegacyPower();
						break;
					case 15:
						conditionBase = new SkillCondition_SkillKnifeCount();
						break;
					case 16:
						conditionBase = new SkillCondition_CustomSkill();
						break;
					case 17:
						conditionBase = new SkillCondition_SkillCastCount();
						break;
					default:
						conditionBase = new SkillCondition_Default();
						HLog.LogError(string.Format("contition is Error. table.type = {0}", elementById.type));
						break;
					}
					conditionBase.Init(this.m_skill);
					conditionBase.SetParameter(elementById.parameters);
					this.conditions.Add(conditionBase);
				}
			}
		}

		public void DeInitCondition()
		{
			for (int i = 0; i < this.conditions.Count; i++)
			{
				this.conditions[i].OnDeInit();
			}
			this.OnDeInit();
			this.m_skill = null;
			this.conditions.Clear();
		}

		public abstract int GetName();

		public abstract bool IsCanPlay(SkillTriggerData triggerData);

		public virtual void OnInit()
		{
		}

		public virtual void OnDeInit()
		{
		}

		public virtual void SetParameter(string parameter)
		{
		}

		public virtual void OnRefreshTriggers()
		{
		}

		public bool IsMatchComtitions()
		{
			if (this.conditions == null)
			{
				return false;
			}
			for (int i = 0; i < this.conditions.Count; i++)
			{
				if (!this.conditions[i].IsMatchCondition())
				{
					return false;
				}
			}
			return true;
		}

		public void OnRefreshContitions()
		{
			for (int i = 0; i < this.conditions.Count; i++)
			{
				this.conditions[i].OnRefresh();
			}
		}

		public SSkillBase m_skill;

		private List<ConditionBase> conditions = new List<ConditionBase>();
	}
}
