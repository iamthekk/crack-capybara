using System;

namespace Server
{
	public class SkillCondition_BeAttack : ConditionBase
	{
		public override void OnInit()
		{
			SMemberBase owner = this.m_curSkill.Owner;
			owner.OnBattleStart = (Action)Delegate.Combine(owner.OnBattleStart, new Action(this.OnBattleStart));
			SMemberBase owner2 = this.m_curSkill.Owner;
			owner2.OnBeAttacked = (Action<SSkillBase>)Delegate.Combine(owner2.OnBeAttacked, new Action<SSkillBase>(this.OnBeAttacked));
			SSkillBase curSkill = this.m_curSkill;
			curSkill.m_onPlayStart = (Action<SSkillBase>)Delegate.Combine(curSkill.m_onPlayStart, new Action<SSkillBase>(this.OnPlayStart));
		}

		public override void OnDeInit()
		{
			SMemberBase owner = this.m_curSkill.Owner;
			owner.OnBattleStart = (Action)Delegate.Remove(owner.OnBattleStart, new Action(this.OnBattleStart));
			SMemberBase owner2 = this.m_curSkill.Owner;
			owner2.OnBeAttacked = (Action<SSkillBase>)Delegate.Remove(owner2.OnBeAttacked, new Action<SSkillBase>(this.OnBeAttacked));
			SSkillBase curSkill = this.m_curSkill;
			curSkill.m_onPlayStart = (Action<SSkillBase>)Delegate.Remove(curSkill.m_onPlayStart, new Action<SSkillBase>(this.OnPlayStart));
		}

		public override void SetParameter(string parameter)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameter)) ? JsonManager.ToObject<SkillCondition_BeAttack.Data>(parameter) : new SkillCondition_BeAttack.Data());
		}

		public override bool IsMatchCondition()
		{
			return this.m_curBeAttackedCount >= this.m_data.MaxCount;
		}

		private void Reset()
		{
			this.m_curBeAttackedCount = 0;
		}

		private void OnBattleStart()
		{
			this.Reset();
		}

		private void OnBeAttacked(SSkillBase skill)
		{
			for (int i = 0; i < this.m_data.skillTypes.Length; i++)
			{
				if (skill.skillData.m_freedType == this.m_data.skillTypes[i])
				{
					this.m_curBeAttackedCount++;
					return;
				}
			}
		}

		private void OnPlayStart(SSkillBase skill)
		{
			this.Reset();
		}

		public SkillCondition_BeAttack.Data m_data;

		private int m_curBeAttackedCount;

		[Serializable]
		public class Data
		{
			public SkillFreedType[] skillTypes;

			public int MaxCount;
		}
	}
}
