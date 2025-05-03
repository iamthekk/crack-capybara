using System;
using System.Collections.Generic;

namespace Server
{
	public class SSkill_FallingSword : SSkillBase
	{
		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SSkill_FallingSword.Data>(parameters) : new SSkill_FallingSword.Data());
		}

		public override void OnInit(SSkillFactory skillFactory, SSkillData skillData)
		{
			this.m_skillType = SkillType.FallingSword;
			base.m_onSkillOneHurtAfter = (Action<SSkillBase, int, int>)Delegate.Combine(base.m_onSkillOneHurtAfter, new Action<SSkillBase, int, int>(this.TriggerFire));
			base.OnInit(skillFactory, skillData);
		}

		public override void OnDeInit()
		{
			base.m_onSkillOneHurtAfter = (Action<SSkillBase, int, int>)Delegate.Remove(base.m_onSkillOneHurtAfter, new Action<SSkillBase, int, int>(this.TriggerFire));
			base.OnDeInit();
		}

		public override void Play()
		{
			this.CheackFallingSwordSuper();
			base.Play();
		}

		private int GetFireCount()
		{
			object parameter = base.m_curSkillTriggerData.m_parameter;
			if (parameter is int)
			{
				int num = (int)parameter;
				if (num > 0)
				{
					return num;
				}
			}
			SkillTriggerType triggerType = base.m_curSkillTriggerData.m_iHitTargetSkill.m_curSkillTriggerData.m_triggerType;
			int num2;
			if (triggerType == SkillTriggerType.BattleStart)
			{
				num2 = base.Owner.memberData.attribute.BattleStartFallingSwordCount.RoundToInt();
			}
			else if (triggerType == SkillTriggerType.RoleRoundStart)
			{
				num2 = base.Owner.memberData.attribute.RoundStartFallingSwordCount.RoundToInt();
			}
			else if (triggerType == SkillTriggerType.BigSkillAfter)
			{
				num2 = base.Owner.memberData.attribute.BigSkillAfterFallingSwordCount.RoundToInt();
			}
			else
			{
				num2 = base.Owner.memberData.attribute.FallingSwordCount.RoundToInt();
			}
			return num2;
		}

		public override void DoEventNodes()
		{
			int fireCount = this.GetFireCount();
			for (int i = 0; i < fireCount; i++)
			{
				if (!base.CurSelectTargetDatas[0].m_target.IsDeath)
				{
					this.FireBullet(0, true);
					base.AddSkillLegacyPower();
				}
				else
				{
					if (!base.CheckCanPlay(null))
					{
						break;
					}
					this.FireBullet(0, true);
					base.AddSkillLegacyPower();
				}
				this.ActiveChangeSkillTriggerCount();
			}
			base.AddSkillRecharge();
			this.FinishSkill();
		}

		private void CheackFallingSwordSuper()
		{
			if (this.m_owner.m_controller.Random01() <= this.m_owner.memberData.attribute.FallingSwordSuperRate)
			{
				base.skillData.ChangeFireBullets(1041);
				this.ChangeFallingSwordSuperAttack(true);
				return;
			}
			base.skillData.InitFireBullets();
			this.ChangeFallingSwordSuperAttack(false);
		}

		public void ChangeFallingSwordSuperAttack(bool isSuper)
		{
			if (isSuper.Equals(this.m_isFallingSword))
			{
				return;
			}
			if (isSuper)
			{
				List<string> listString = base.skillData.m_hurtAttributes.GetListString('|');
				for (int i = 0; i < listString.Count; i++)
				{
					List<string> list = listString;
					int num = i;
					list[num] += string.Format("*{0}", FP._1 + this.m_owner.memberData.attribute.FallingSwordSuperAttackPercent);
				}
				base.skillData.m_hurtAttributeDatas = listString;
			}
			else
			{
				base.skillData.m_hurtAttributeDatas = base.skillData.m_hurtAttributes.GetListString('|');
			}
			this.m_isFallingSword = isSuper;
		}

		private void TriggerFire(SSkillBase skill, int targetSelectIndex, int totalTargetCount)
		{
			if (this.m_owner.m_controller.Random01() <= this.m_owner.memberData.attribute.FallingSwordFireRate)
			{
				this.m_skillFactory.CheckPlay(SkillTriggerType.Fire, this);
			}
		}

		protected override void AutoChangeSkillTriggerCount()
		{
		}

		public SSkill_FallingSword.Data m_data;

		private bool m_isFallingSword;

		public class Data
		{
		}
	}
}
