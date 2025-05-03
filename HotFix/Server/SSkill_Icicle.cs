using System;

namespace Server
{
	public class SSkill_Icicle : SSkillBase
	{
		public override void OnInit(SSkillFactory skillFactory, SSkillData skillData)
		{
			this.m_skillType = SkillType.Icicle;
			base.m_onSkillOneHurtAfter = (Action<SSkillBase, int, int>)Delegate.Combine(base.m_onSkillOneHurtAfter, new Action<SSkillBase, int, int>(this.TriggerIcicleBuff));
			base.OnInit(skillFactory, skillData);
		}

		public override void OnDeInit()
		{
			base.m_onSkillOneHurtAfter = (Action<SSkillBase, int, int>)Delegate.Remove(base.m_onSkillOneHurtAfter, new Action<SSkillBase, int, int>(this.TriggerIcicleBuff));
			base.OnDeInit();
		}

		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SSkill_Icicle.Data>(parameters) : new SSkill_Icicle.Data());
		}

		public override void DoEventNodes()
		{
			this.FireBullet(0, true);
			this.ActiveChangeSkillTriggerCount();
			base.AddSkillLegacyPower();
			base.AddSkillRecharge();
			if (this.isLastEventNodes)
			{
				this.FinishSkill();
			}
		}

		private void TriggerIcicleBuff(SSkillBase skill, int targetSelectIndex, int totalTargetCount)
		{
			if (this.m_owner.m_controller.Random01() <= this.m_owner.memberData.attribute.FrozenRate && !this.m_data.addBuffID.Equals(0) && base.CurSelectTargetDatas != null && base.CurSelectTargetDatas.Count > targetSelectIndex)
			{
				SMemberBase target = base.CurSelectTargetDatas[targetSelectIndex].m_target;
				if (target.IsDeath)
				{
					return;
				}
				target.buffFactory.AddBuff(base.Owner, this.m_data.addBuffID);
			}
		}

		protected override void AutoChangeSkillTriggerCount()
		{
		}

		public SSkill_Icicle.Data m_data;

		public int castFinishCount;

		public class Data
		{
			public int addBuffID;
		}
	}
}
