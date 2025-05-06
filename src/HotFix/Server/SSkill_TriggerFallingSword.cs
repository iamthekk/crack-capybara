using System;

namespace Server
{
	public class SSkill_TriggerFallingSword : SSkillBase
	{
		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SSkill_TriggerFallingSword.Data>(parameters) : new SSkill_TriggerFallingSword.Data());
		}

		public override void OnInit(SSkillFactory skillFactory, SSkillData skillData)
		{
			base.OnInit(skillFactory, skillData);
		}

		public override void DoEventNodes()
		{
			base.AddSkillLegacyPower();
			if (!this.m_skillData.IsHaveAnimation)
			{
				this.TriggerBasicSkill();
			}
			this.FinishSkill();
		}

		private void TriggerBasicSkill()
		{
			SkillTriggerData skillTriggerData = new SkillTriggerData();
			skillTriggerData.m_triggerType = (SkillTriggerType)this.m_data.triggerType;
			skillTriggerData.SetTriggerSkill(this);
			skillTriggerData.m_parameter = this.m_data.triggerCount;
			for (int i = 0; i < base.CurSelectTargetDatas.Count; i++)
			{
				skillTriggerData.m_iHitTargetList.Add(base.CurSelectTargetDatas[i].m_target);
			}
			this.m_skillFactory.CheckPlay(skillTriggerData);
		}

		public SSkill_TriggerFallingSword.Data m_data;

		public class Data
		{
			public int triggerType = 999;

			public int triggerCount = -1;
		}
	}
}
