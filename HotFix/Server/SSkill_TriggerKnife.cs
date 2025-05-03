using System;

namespace Server
{
	public class SSkill_TriggerKnife : SSkillBase
	{
		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SSkill_TriggerKnife.Data>(parameters) : new SSkill_TriggerKnife.Data());
		}

		public override void DoEventNodes()
		{
			if (this.m_skillData.IsHaveAnimation)
			{
				if (this.m_skillData.m_animEventNodes.Count == 0)
				{
					HLog.LogError(string.Format("SSkillBase.DoEventNodes ..m_eventNodes is null!! skillID = {0}", this.m_skillData.m_id));
				}
				int count = this.m_skillData.m_animEventNodes.Count;
				for (int i = 0; i < this.m_skillData.m_animEventNodes.Count; i++)
				{
					SSkillData.EventNode eventNode = this.m_skillData.m_animEventNodes[i];
					if (eventNode.eventName.Equals("Fire"))
					{
						this.m_curFireIndex++;
						SMemberBase owner = this.m_owner;
						if (owner != null)
						{
							BaseBattleServerController controller = owner.m_controller;
							if (controller != null)
							{
								controller.AddCurFrame(eventNode.frame);
							}
						}
						this.curStartFrame = base.Owner.m_controller.CurFrame;
						this.FireBullet(this.m_curFireIndex, i == count - 2);
						base.AddSkillLegacyPower();
						base.AddSkillRecharge();
					}
					else if (eventNode.eventName.Equals("End") && this.isLastEventNodes)
					{
						this.curEndFrame = eventNode.frame;
						this.ReadyFinishSkill();
					}
				}
				return;
			}
			base.AddSkillLegacyPower();
			base.AddSkillRecharge();
			if (this.isLastEventNodes)
			{
				this.ReadyFinishSkill();
			}
		}

		private void ReadyFinishSkill()
		{
			this.TriggerBasicSkill();
			this.FinishSkill();
		}

		private void TriggerBasicSkill()
		{
			SkillTriggerData skillTriggerData = new SkillTriggerData();
			skillTriggerData.m_triggerType = (SkillTriggerType)this.m_data.triggerType;
			skillTriggerData.SetTriggerSkill(this);
			skillTriggerData.m_parameter = this.m_data;
			for (int i = 0; i < base.CurSelectTargetDatas.Count; i++)
			{
				skillTriggerData.m_iHitTargetList.Add(base.CurSelectTargetDatas[i].m_target);
			}
			this.m_skillFactory.CheckPlay(skillTriggerData);
		}

		public SSkill_TriggerKnife.Data m_data;

		public class Data
		{
			public int triggerType = 999;

			public int triggerCount = -1;

			public int forceKnifeType;
		}
	}
}
