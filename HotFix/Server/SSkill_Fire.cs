using System;

namespace Server
{
	public class SSkill_Fire : SSkillBase
	{
		public override void OnInit(SSkillFactory skillFactory, SSkillData skillData)
		{
			this.m_skillType = SkillType.Fire;
			base.m_onSkillOneHurtAfter = (Action<SSkillBase, int, int>)Delegate.Combine(base.m_onSkillOneHurtAfter, new Action<SSkillBase, int, int>(this.TriggerBuff));
			base.OnInit(skillFactory, skillData);
		}

		public override void OnDeInit()
		{
			base.m_onSkillOneHurtAfter = (Action<SSkillBase, int, int>)Delegate.Remove(base.m_onSkillOneHurtAfter, new Action<SSkillBase, int, int>(this.TriggerBuff));
			base.OnDeInit();
		}

		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SSkill_Fire.Data>(parameters) : new SSkill_Fire.Data());
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

		private void TriggerBuff(SSkillBase skill, int targetSelectIndex, int totalTargetCount)
		{
			this.AddFireBuff(targetSelectIndex);
			this.AddDamageBuff(targetSelectIndex);
		}

		private void AddFireBuff(int targetSelectIndex)
		{
			FP fp = this.m_owner.memberData.attribute.TriggerFireBuffRate + this.m_owner.memberData.attribute.TriggerFireBuffAddRate;
			if (fp > FP._0 && this.m_owner.m_controller.Random01() <= fp && !this.m_data.addFireBuffID.Equals(0) && base.CurSelectTargetDatas != null && base.CurSelectTargetDatas.Count >= targetSelectIndex)
			{
				SMemberBase target = base.CurSelectTargetDatas[targetSelectIndex].m_target;
				if (target.IsDeath)
				{
					return;
				}
				int num = this.m_owner.memberData.attribute.FireAddBuffLayerAdd.FloorToInt();
				for (int i = 0; i < num; i++)
				{
					target.buffFactory.AddBuff(base.Owner, this.m_data.addFireBuffID);
				}
			}
		}

		private void AddDamageBuff(int targetSelectIndex)
		{
			FP fireTriggerDamageBuffRate = this.m_owner.memberData.attribute.FireTriggerDamageBuffRate;
			if (fireTriggerDamageBuffRate > FP._0 && this.m_owner.m_controller.Random01() <= fireTriggerDamageBuffRate && !this.m_data.addDamageBuffID.Equals(0) && base.CurSelectTargetDatas != null && base.CurSelectTargetDatas.Count > targetSelectIndex)
			{
				SMemberBase target = base.CurSelectTargetDatas[targetSelectIndex].m_target;
				if (target.IsDeath)
				{
					return;
				}
				target.buffFactory.AddBuff(base.Owner, this.m_data.addDamageBuffID);
			}
		}

		protected override void AutoChangeSkillTriggerCount()
		{
		}

		public SSkill_Fire.Data m_data;

		public class Data
		{
			public int addFireBuffID;

			public int addDamageBuffID;
		}
	}
}
