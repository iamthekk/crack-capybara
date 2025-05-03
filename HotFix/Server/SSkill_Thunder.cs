using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
	public class SSkill_Thunder : SSkillBase
	{
		public override void OnInit(SSkillFactory skillFactory, SSkillData skillData)
		{
			this.forceThunderType = SSkill_Thunder.ThunderType.None;
			this.m_thunderUseFullCount = 0;
			this.m_skillType = SkillType.Thunder;
			base.m_onSkillOneHurtAfter = (Action<SSkillBase, int, int>)Delegate.Combine(base.m_onSkillOneHurtAfter, new Action<SSkillBase, int, int>(this.TriggerThunderBuff));
			base.OnInit(skillFactory, skillData);
		}

		public override void OnDeInit()
		{
			this.forceThunderType = SSkill_Thunder.ThunderType.None;
			this.m_thunderUseFullCount = 0;
			base.m_onSkillOneHurtAfter = (Action<SSkillBase, int, int>)Delegate.Remove(base.m_onSkillOneHurtAfter, new Action<SSkillBase, int, int>(this.TriggerThunderBuff));
			base.OnDeInit();
		}

		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SSkill_Thunder.Data>(parameters) : new SSkill_Thunder.Data());
		}

		protected override void OnPlaying()
		{
			this.forceThunderType = (SSkill_Thunder.ThunderType)this.GetForceThunderType();
			bool flag = false;
			int thunderCount = this.GetThunderCount();
			for (int i = 0; i < thunderCount; i++)
			{
				this.CheckThunderChange();
				this.isLastEventNodes = i == thunderCount - FP._1;
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
				base.CheckSpecialSkill();
				this.DoEventNodes();
			}
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

		private void TriggerThunderBuff(SSkillBase skill, int targetSelectIndex, int totalTargetCount)
		{
			if (this.m_owner.m_controller.Random01() <= this.m_owner.memberData.attribute.ThunderBuffRate && this.m_data != null && this.m_data.PoisonBuffIDs != null && this.m_data.PoisonBuffIDs.Length != 0 && base.CurSelectTargetDatas != null && base.CurSelectTargetDatas.Count > targetSelectIndex)
			{
				SMemberBase target = base.CurSelectTargetDatas[targetSelectIndex].m_target;
				if (target.IsDeath)
				{
					return;
				}
				target.buffFactory.AddBuffs(base.Owner, this.m_data.PoisonBuffIDs.ToList<int>());
			}
		}

		private void CheckThunderChange()
		{
			FP fp = this.m_owner.m_controller.Random01();
			if (this.forceThunderType == SSkill_Thunder.ThunderType.Death)
			{
				base.skillData.ChangeFireBullets(this.m_data.ThunderDeathFireBulletID);
				this.ChangeThunderToOtherAttack(false, true);
				this.m_thunderUseFullCount += FP._1;
				return;
			}
			if (this.forceThunderType == SSkill_Thunder.ThunderType.Super)
			{
				base.skillData.ChangeFireBullets(this.m_data.ThunderSuperFireBulletID);
				this.ChangeThunderToOtherAttack(true, false);
				return;
			}
			if (this.forceThunderType == SSkill_Thunder.ThunderType.Default)
			{
				base.skillData.InitFireBullets();
				this.ChangeThunderToOtherAttack(false, false);
				return;
			}
			if (fp <= this.m_owner.memberData.attribute.ThunderDeathRate)
			{
				base.skillData.ChangeFireBullets(this.m_data.ThunderDeathFireBulletID);
				this.ChangeThunderToOtherAttack(false, true);
				this.m_thunderUseFullCount += FP._1;
				return;
			}
			if (fp <= this.m_owner.memberData.attribute.ThunderSuperRate)
			{
				base.skillData.ChangeFireBullets(this.m_data.ThunderSuperFireBulletID);
				this.ChangeThunderToOtherAttack(true, false);
				return;
			}
			base.skillData.InitFireBullets();
			this.ChangeThunderToOtherAttack(false, false);
		}

		public void ChangeThunderToOtherAttack(bool isThunderSuper, bool isThunderDeath)
		{
			if (isThunderDeath)
			{
				FP thunderDeathAttackAddMaxCount = base.Owner.memberData.attribute.ThunderDeathAttackAddMaxCount;
				FP thunderDeathAttackPercent = base.Owner.memberData.attribute.ThunderDeathAttackPercent;
				FP thunderDeathAttackAddPercent = base.Owner.memberData.attribute.ThunderDeathAttackAddPercent;
				FP fp = MathTools.Clamp(this.m_thunderUseFullCount, FP._0, thunderDeathAttackAddMaxCount);
				List<string> listString = base.skillData.m_hurtAttributes.GetListString('|');
				for (int i = 0; i < listString.Count; i++)
				{
					string text = listString[i] + string.Format("*{0}*{1}", thunderDeathAttackPercent, FP._1 + thunderDeathAttackAddPercent * fp);
					listString[i] = text;
				}
				base.skillData.m_hurtAttributeDatas = listString;
			}
			else if (isThunderSuper)
			{
				FP thunderBaseAttackPercent = base.Owner.memberData.attribute.ThunderBaseAttackPercent;
				FP thunderSuperAttackPercent = base.Owner.memberData.attribute.ThunderSuperAttackPercent;
				List<string> listString2 = base.skillData.m_hurtAttributes.GetListString('|');
				for (int j = 0; j < listString2.Count; j++)
				{
					string text2 = listString2[j] + string.Format("*{0}*{1}", thunderBaseAttackPercent, FP._1 + thunderSuperAttackPercent);
					listString2[j] = text2;
				}
				base.skillData.m_hurtAttributeDatas = listString2;
			}
			else
			{
				FP thunderBaseAttackPercent2 = base.Owner.memberData.attribute.ThunderBaseAttackPercent;
				List<string> listString3 = base.skillData.m_hurtAttributes.GetListString('|');
				for (int k = 0; k < listString3.Count; k++)
				{
					string text3 = listString3[k] + string.Format("*{0}", thunderBaseAttackPercent2);
					listString3[k] = text3;
				}
				base.skillData.m_hurtAttributeDatas = listString3;
			}
			this.m_isThunderSuper = isThunderSuper;
			this.m_isThunderDeath = isThunderDeath;
		}

		private int GetForceThunderType()
		{
			if (base.m_curSkillTriggerData != null)
			{
				SSkill_TriggerThunder.Data data = base.m_curSkillTriggerData.m_parameter as SSkill_TriggerThunder.Data;
				if (data != null)
				{
					return data.thunderType;
				}
			}
			return 0;
		}

		private int GetThunderCount()
		{
			SSkill_TriggerThunder.Data data = ((base.m_curSkillTriggerData != null && base.m_curSkillTriggerData.m_parameter != null) ? (base.m_curSkillTriggerData.m_parameter as SSkill_TriggerThunder.Data) : null);
			if (data != null && data.triggerCount > 0)
			{
				return data.triggerCount;
			}
			return base.Owner.memberData.attribute.ThunderCount.FloorToInt();
		}

		protected override void AutoChangeSkillTriggerCount()
		{
		}

		public SSkill_Thunder.Data m_data;

		public FP m_thunderUseFullCount = FP._0;

		private SSkill_Thunder.ThunderType forceThunderType;

		private bool m_isThunderDeath;

		private bool m_isThunderSuper;

		public enum ThunderType
		{
			None,
			Default,
			Super,
			Death
		}

		public class Data
		{
			public int ThunderSuperFireBulletID = 1011;

			public int ThunderDeathFireBulletID = 1012;

			public int[] PoisonBuffIDs;
		}
	}
}
