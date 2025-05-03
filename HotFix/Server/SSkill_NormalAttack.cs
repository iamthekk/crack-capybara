using System;
using System.Collections.Generic;

namespace Server
{
	public class SSkill_NormalAttack : SSkillBase
	{
		protected override void SetParameters(string parameters)
		{
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
						base.AddSkillLegacyPower();
						base.AddSkillRecharge();
					}
					else if (eventNode.eventName.Equals("End"))
					{
						this.OnAnimEventEnd();
						if (this.isLastEventNodes)
						{
							this.curEndFrame = eventNode.frame;
							this.FinishOneAttack();
						}
					}
				}
			}
		}

		private void FinishOneAttack()
		{
			if (base.CastType == SkillCastType.Counter)
			{
				this.m_skillFactory.CheckPlay(SkillTriggerType.CounterAfter, this);
				this.FinishSkill();
				return;
			}
			if (base.IsSkillHit)
			{
				base.ReadyCounter();
			}
			if (!(this.curComboCount < base.Owner.memberData.attribute.NormalComboCount))
			{
				this.FinishCombo();
				return;
			}
			this.m_CastType = SkillCastType.Attack;
			if (base.Owner.IsDeath)
			{
				this.FinishCombo();
				return;
			}
			this.curComboCount++;
			FP fp = this.m_owner.m_controller.Random01();
			if (base.CurSelectTargetDatas.Count > 0 && !base.CurSelectTargetDatas[0].m_target.IsDeath)
			{
				SMemberBase target = base.CurSelectTargetDatas[0].m_target;
				FP fp2 = base.Owner.memberData.attribute.NormalComboRate - target.memberData.attribute.IgnoreNormalComboRate;
				if (target.buffFactory.IsVerdict)
				{
					fp2 += base.Owner.memberData.attribute.ToVerdictComboRate;
				}
				if (fp <= fp2)
				{
					this.m_CastType = SkillCastType.Combo;
					this.CheckComboSkill(true);
					base.SkillAfterCompensation();
					this.OnPlayBefore();
					this.OnPlaying();
					this.CheckComboSkill(false);
					return;
				}
				this.FinishCombo();
				return;
			}
			else
			{
				if (!base.CheckCanPlay(null))
				{
					this.FinishCombo();
					return;
				}
				SMemberBase target2 = base.CurSelectTargetDatas[0].m_target;
				FP fp3 = base.Owner.memberData.attribute.NormalComboRate - target2.memberData.attribute.IgnoreNormalComboRate;
				if (target2.buffFactory.IsVerdict)
				{
					fp3 += base.Owner.memberData.attribute.ToVerdictComboRate;
				}
				if (fp <= fp3)
				{
					this.m_CastType = SkillCastType.Combo;
					this.CheckComboSkill(true);
					base.SkillAfterCompensation();
					base.Move(false);
					this.OnPlayBefore();
					this.OnPlaying();
					this.CheckComboSkill(false);
					return;
				}
				this.FinishCombo();
				return;
			}
		}

		private void FinishCombo()
		{
			this.curComboCount = 0;
			base.IsFinishCounter = true;
			this.FinishSkill();
		}

		private void CheckComboSkill(bool isCombo)
		{
			if (isCombo)
			{
				List<string> listString = base.skillData.m_hurtAttributes.GetListString('|');
				for (int i = 0; i < listString.Count; i++)
				{
					List<string> list = listString;
					int num = i;
					list[num] += string.Format("*{0}", FP._1 + this.m_owner.memberData.attribute.NormalComboDamageAddPercent);
				}
				base.skillData.m_hurtAttributeDatas = listString;
				return;
			}
			base.skillData.m_hurtAttributeDatas = base.skillData.m_hurtAttributes.GetListString('|');
		}

		private void OnAnimEventEnd()
		{
			this.ActiveChangeSkillTriggerCount();
			if (base.CastType == SkillCastType.Combo)
			{
				this.OnComboAfter();
			}
		}

		private void OnComboAfter()
		{
			this.m_skillFactory.CheckPlay(SkillTriggerType.ComboAfter, this);
		}

		protected override void AutoChangeSkillTriggerCount()
		{
		}

		private int curComboCount;
	}
}
