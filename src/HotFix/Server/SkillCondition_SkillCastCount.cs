using System;

namespace Server
{
	public class SkillCondition_SkillCastCount : ConditionBase
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
			this.Reset();
			SMemberBase owner = this.m_curSkill.Owner;
			owner.OnRoundStart = (Action)Delegate.Remove(owner.OnRoundStart, new Action(this.OnRoundStart));
			SSkillBase curSkill = this.m_curSkill;
			curSkill.m_onPlayStart = (Action<SSkillBase>)Delegate.Remove(curSkill.m_onPlayStart, new Action<SSkillBase>(this.OnPlayStart));
			SSkillFactory skillFactory = this.m_curSkill.skillFactory;
			skillFactory.m_onSkillCountChange = (Action<SSkillBase>)Delegate.Remove(skillFactory.m_onSkillCountChange, new Action<SSkillBase>(this.OnSkillCountChange));
			this.m_data = null;
		}

		private void Reset()
		{
			this.skillCastCount = 0;
		}

		public override void SetParameter(string parameter)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameter)) ? JsonManager.ToObject<SkillCondition_SkillCastCount.Data>(parameter) : new SkillCondition_SkillCastCount.Data());
		}

		public override bool IsMatchCondition()
		{
			return this.skillCastCount >= this.m_data.MaxCount;
		}

		private void OnRoundStart()
		{
			if (this.m_data.IsRoundRefresh > 0)
			{
				this.Reset();
			}
		}

		private void OnPlayStart(SSkillBase skill)
		{
			this.Reset();
		}

		private void OnSkillCountChange(SSkillBase skill)
		{
			if (skill == null || this.m_data == null || this.m_data.skillCastTypes == null || this.m_data.skillCastTypes.Length == 0)
			{
				return;
			}
			for (int i = 0; i < this.m_data.skillCastTypes.Length; i++)
			{
				if (skill.CastType == (SkillCastType)this.m_data.skillCastTypes[i])
				{
					this.skillCastCount++;
					return;
				}
			}
		}

		public SkillCondition_SkillCastCount.Data m_data;

		private int skillCastCount;

		[Serializable]
		public class Data
		{
			public int[] skillCastTypes;

			public int MaxCount;

			public int IsRoundRefresh;
		}
	}
}
