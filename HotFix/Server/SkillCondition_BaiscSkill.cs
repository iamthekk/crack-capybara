using System;

namespace Server
{
	public class SkillCondition_BaiscSkill : ConditionBase
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
			this.m_curSkillPlayCount = 0;
		}

		public override void SetParameter(string parameter)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameter)) ? JsonManager.ToObject<SkillCondition_BaiscSkill.Data>(parameter) : new SkillCondition_BaiscSkill.Data());
		}

		public override bool IsMatchCondition()
		{
			return this.m_curSkillPlayCount >= this.m_data.MaxCount;
		}

		public override void OnRefresh()
		{
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
			if (this.m_data.basicSkillTypes == null)
			{
				HLog.LogError(string.Format("技能触发条件错误. SkillID = {0}", skill.skillData.m_id));
				return;
			}
			if (skill == null)
			{
				return;
			}
			for (int i = 0; i < this.m_data.basicSkillTypes.Length; i++)
			{
				if (skill.skillType == this.m_data.basicSkillTypes[i])
				{
					this.m_curSkillPlayCount++;
					return;
				}
			}
		}

		public SkillCondition_BaiscSkill.Data m_data;

		private int m_curSkillPlayCount;

		[Serializable]
		public class Data
		{
			public SkillType[] basicSkillTypes;

			public int MaxCount;

			public int IsRoundRefresh;
		}
	}
}
