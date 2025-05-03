﻿using System;

namespace Server
{
	public class SSkill_TriggerSkillRandomCount : SSkillBase
	{
		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SSkill_TriggerSkillRandomCount.Data>(parameters) : new SSkill_TriggerSkillRandomCount.Data());
		}

		public override void DoEventNodes()
		{
			if (this.m_skillData.IsHaveAnimation)
			{
				if (this.m_skillData.m_animEventNodes.Count == 0)
				{
					HLog.LogError(string.Format("SSkillBase.DoEventNodes ..m_eventNodes is null!! skillID = {0}", this.m_skillData.m_id));
				}
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
						this.FireBullet(this.m_curFireIndex, true);
						if (!base.skillData.IsHaveFirebulletData())
						{
							this.TriggerBasicSkill();
						}
						base.AddSkillLegacyPower();
						base.AddSkillRecharge();
					}
					else if (eventNode.eventName.Equals("End") && this.isLastEventNodes)
					{
						this.curEndFrame = eventNode.frame;
						this.FinishSkill();
					}
				}
				return;
			}
			this.FireBullet(0, true);
			if (!base.skillData.IsHaveFirebulletData())
			{
				this.TriggerBasicSkill();
			}
			base.AddSkillLegacyPower();
			base.AddSkillRecharge();
			if (this.isLastEventNodes)
			{
				this.FinishSkill();
			}
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
			if (this.m_owner.m_controller.Random01() * 100 > this.m_data.triggerRate)
			{
				return;
			}
			int num = this.m_owner.m_controller.RandomRange(this.m_data.triggerCountMin, this.m_data.triggerCountMax);
			if (triggerType == SkillTriggerType.Thunder)
			{
				this.TriggerThunder(num);
				return;
			}
			if (triggerType == SkillTriggerType.Icicle)
			{
				this.TriggerIcicle(num);
				return;
			}
			if (triggerType == SkillTriggerType.Fire)
			{
				this.TriggerFire(num);
				return;
			}
			if (triggerType == SkillTriggerType.Knife)
			{
				this.TriggerKnife(num);
				return;
			}
			if (triggerType == SkillTriggerType.KnifeTrigger || triggerType == SkillTriggerType.Swordkee)
			{
				this.m_skillFactory.CheckPlay(triggerType, this);
				return;
			}
			if (triggerType == SkillTriggerType.FallingSword)
			{
				this.m_skillFactory.CheckPlay(triggerType, this);
			}
		}

		private void TriggerThunder(int count)
		{
			if (base.Owner.memberData.attribute.ThunderCount > FP._0)
			{
				for (int i = 0; i < count; i++)
				{
					if (!i.Equals(0))
					{
						this.m_owner.m_controller.AddCurFrame(8);
					}
					this.m_skillFactory.CheckPlay((SkillTriggerType)this.m_data.triggerType, this);
				}
			}
		}

		private void TriggerIcicle(int count)
		{
			for (int i = 0; i < count; i++)
			{
				if (!i.Equals(0))
				{
					this.m_owner.m_controller.AddCurFrame(8);
				}
				this.m_skillFactory.CheckPlay((SkillTriggerType)this.m_data.triggerType, this);
			}
		}

		private void TriggerFire(int count)
		{
			for (int i = 0; i < count; i++)
			{
				if (!i.Equals(0))
				{
					this.m_owner.m_controller.AddCurFrame(8);
				}
				this.m_skillFactory.CheckPlay((SkillTriggerType)this.m_data.triggerType, this);
			}
		}

		private void TriggerKnife(int count)
		{
			SkillTriggerData skillTriggerData = new SkillTriggerData();
			skillTriggerData.m_triggerType = (SkillTriggerType)this.m_data.triggerType;
			skillTriggerData.SetTriggerSkill(this);
			skillTriggerData.m_parameter = count;
			for (int i = 0; i < base.CurSelectTargetDatas.Count; i++)
			{
				skillTriggerData.m_iHitTargetList.Add(base.CurSelectTargetDatas[i].m_target);
			}
			this.m_skillFactory.CheckPlay(skillTriggerData);
		}

		public SSkill_TriggerSkillRandomCount.Data m_data;

		public class Data
		{
			public int triggerType = 999;

			public int triggerCountMin = 1;

			public int triggerCountMax = 1;

			public int triggerRate = 100;
		}
	}
}
