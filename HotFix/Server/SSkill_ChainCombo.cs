using System;

namespace Server
{
	public class SSkill_ChainCombo : SSkillBase
	{
		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SSkill_ChainCombo.Data>(parameters) : new SSkill_ChainCombo.Data());
		}

		protected override void OnPlaying()
		{
			bool flag = false;
			int num = base.Owner.memberData.attribute.ChainComboCount.FloorToInt() + 1;
			for (int i = 0; i < num; i++)
			{
				if (base.CurSelectTargetDatas == null || base.CurSelectTargetDatas.Count <= 0 || base.CurSelectTargetDatas[0] == null || base.CurSelectTargetDatas[0].m_target.memberData.IsDeath)
				{
					this.FinishSkill();
					return;
				}
				if (i != 0)
				{
					this.m_owner.m_controller.AddCurFrame(8);
				}
				base.ReportPlay(0);
				if (!flag)
				{
					base.SkillAddBuffs(SkillTriggerBuffState.Start);
					flag = true;
				}
				this.DoEventNodes();
			}
		}

		public override void DoEventNodes()
		{
			this.FireBullet(0, true);
			this.TriggerBasicSkill();
			base.AddSkillLegacyPower();
			base.AddSkillRecharge();
			if (this.isLastEventNodes)
			{
				this.FinishSkill();
				return;
			}
			base.SkillAddBuffs(SkillTriggerBuffState.End);
		}

		protected override void OnDamageAfter()
		{
			this.TriggerBasicSkill();
		}

		private void TriggerBasicSkill()
		{
			SkillTriggerType triggerType = (SkillTriggerType)this.m_data.triggerType;
			if (triggerType == SkillTriggerType.Undo)
			{
				return;
			}
			if (triggerType == SkillTriggerType.Thunder)
			{
				this.TriggerThunder();
				return;
			}
			if (triggerType == SkillTriggerType.Icicle || triggerType == SkillTriggerType.Fire || triggerType == SkillTriggerType.Knife)
			{
				this.TriggerDefault();
			}
		}

		private void TriggerThunder()
		{
			if (base.Owner.memberData.attribute.ThunderCount > FP._0)
			{
				for (int i = 0; i < this.m_data.triggerCount; i++)
				{
					if (!i.Equals(0))
					{
						this.m_owner.m_controller.AddCurFrame(8);
					}
					this.m_skillFactory.CheckPlay((SkillTriggerType)this.m_data.triggerType, this);
				}
			}
		}

		private void TriggerDefault()
		{
			this.m_skillFactory.CheckPlay((SkillTriggerType)this.m_data.triggerType, this);
		}

		public SSkill_ChainCombo.Data m_data;

		public class Data
		{
			public int triggerType = 999;

			public int triggerCount = 1;
		}
	}
}
