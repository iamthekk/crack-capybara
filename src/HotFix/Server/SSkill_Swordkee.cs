using System;
using System.Collections.Generic;

namespace Server
{
	public class SSkill_Swordkee : SSkillBase
	{
		public override void OnInit(SSkillFactory skillFactory, SSkillData skillData)
		{
			this.m_skillType = SkillType.Swordkee;
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
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SSkill_Swordkee.Data>(parameters) : new SSkill_Swordkee.Data());
		}

		protected override void OnPlaying()
		{
			this.CheackSwordkeeSuper();
			base.OnPlaying();
		}

		private void CheackSwordkeeSuper()
		{
			if (this.m_owner.m_controller.Random01() <= this.m_owner.memberData.attribute.SwordkeeSuperRate)
			{
				base.skillData.ChangeFireBullets(this.m_data.SwordkeeSuperFireBulletID);
				this.ChangeSwordkeeSuperAttack(true);
				return;
			}
			base.skillData.InitFireBullets();
			this.ChangeSwordkeeSuperAttack(false);
		}

		public void ChangeSwordkeeSuperAttack(bool isSuper)
		{
			if (isSuper.Equals(this.m_isSwordkeeSuper))
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
					list[num] += string.Format("*{0}", FP._1 + this.m_owner.memberData.attribute.SwordkeeSuperAttackPercent);
				}
				base.skillData.m_hurtAttributeDatas = listString;
			}
			else
			{
				base.skillData.m_hurtAttributeDatas = base.skillData.m_hurtAttributes.GetListString('|');
			}
			this.m_isSwordkeeSuper = isSuper;
		}

		private void TriggerBuff(SSkillBase skill, int targetSelectIndex, int totalTargetCount)
		{
			this.AddDamageBuff(targetSelectIndex);
		}

		private void AddDamageBuff(int targetSelectIndex)
		{
			FP swordkeeTriggerDamageBuffRate = this.m_owner.memberData.attribute.SwordkeeTriggerDamageBuffRate;
			if (swordkeeTriggerDamageBuffRate > FP._0 && this.m_owner.m_controller.Random01() <= swordkeeTriggerDamageBuffRate && !this.m_data.addDamageBuffID.Equals(0) && base.CurSelectTargetDatas != null && base.CurSelectTargetDatas.Count > targetSelectIndex)
			{
				SMemberBase target = base.CurSelectTargetDatas[targetSelectIndex].m_target;
				if (!target.IsDeath)
				{
					target.buffFactory.AddBuff(base.Owner, this.m_data.addDamageBuffID);
				}
			}
		}

		protected override void AutoChangeSkillTriggerCount()
		{
			base.AutoChangeSkillTriggerCount();
		}

		public SSkill_Swordkee.Data m_data;

		private bool m_isSwordkeeSuper;

		public class Data
		{
			public int SwordkeeSuperFireBulletID = 1031;

			public int addDamageBuffID;
		}
	}
}
