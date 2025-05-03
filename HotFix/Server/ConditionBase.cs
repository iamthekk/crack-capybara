using System;

namespace Server
{
	public abstract class ConditionBase
	{
		public void Init(SSkillBase curSkill)
		{
			this.m_curSkill = curSkill;
			this.OnInit();
		}

		public abstract void OnInit();

		public abstract void OnDeInit();

		public abstract void SetParameter(string parameter);

		public abstract bool IsMatchCondition();

		public virtual void OnRefresh()
		{
		}

		protected SSkillBase m_curSkill;
	}
}
