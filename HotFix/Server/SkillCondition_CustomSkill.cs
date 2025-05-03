using System;

namespace Server
{
	public class SkillCondition_CustomSkill : ConditionBase
	{
		public override void OnInit()
		{
			this.Reset();
			SMemberBase owner = this.m_curSkill.Owner;
			owner.OnRoundStart = (Action)Delegate.Combine(owner.OnRoundStart, new Action(this.OnRoundStart));
			SSkillBase curSkill = this.m_curSkill;
			curSkill.m_onPlayStart = (Action<SSkillBase>)Delegate.Combine(curSkill.m_onPlayStart, new Action<SSkillBase>(this.OnPlayStart));
			SSkillFactory skillFactory = this.m_curSkill.skillFactory;
			skillFactory.m_onSkillCountChange = (Action<SSkillBase>)Delegate.Combine(skillFactory.m_onSkillCountChange, new Action<SSkillBase>(this.OnSkillCountChange));
		}

		public override void OnDeInit()
		{
			this.m_data = null;
			SMemberBase owner = this.m_curSkill.Owner;
			owner.OnRoundStart = (Action)Delegate.Remove(owner.OnRoundStart, new Action(this.OnRoundStart));
			SSkillBase curSkill = this.m_curSkill;
			curSkill.m_onPlayStart = (Action<SSkillBase>)Delegate.Remove(curSkill.m_onPlayStart, new Action<SSkillBase>(this.OnPlayStart));
			SSkillFactory skillFactory = this.m_curSkill.skillFactory;
			skillFactory.m_onSkillCountChange = (Action<SSkillBase>)Delegate.Remove(skillFactory.m_onSkillCountChange, new Action<SSkillBase>(this.OnSkillCountChange));
		}

		private void Reset()
		{
			this.m_isMatch = false;
		}

		public override void SetParameter(string parameter)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameter)) ? JsonManager.ToObject<SkillCondition_CustomSkill.Data>(parameter) : new SkillCondition_CustomSkill.Data());
		}

		public override bool IsMatchCondition()
		{
			return this.m_isMatch;
		}

		public override void OnRefresh()
		{
		}

		private void OnRoundStart()
		{
			this.Reset();
		}

		private void OnPlayStart(SSkillBase skill)
		{
			this.Reset();
		}

		private void OnSkillCountChange(SSkillBase skill)
		{
			int id = skill.skillData.m_id;
			if (this.m_data != null && this.m_data.skillIds != null && this.m_data.skillIds.Length != 0)
			{
				for (int i = 0; i < this.m_data.skillIds.Length; i++)
				{
					if (this.m_data.skillIds[i] == id)
					{
						this.m_isMatch = true;
						return;
					}
				}
			}
		}

		public SkillCondition_CustomSkill.Data m_data;

		private bool m_isMatch;

		[Serializable]
		public class Data
		{
			public int[] skillIds;
		}
	}
}
