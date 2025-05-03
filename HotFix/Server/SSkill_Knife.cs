using System;
using System.Collections.Generic;

namespace Server
{
	public class SSkill_Knife : SSkillBase
	{
		protected override void SetParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<SSkill_Knife.Data>(parameters) : new SSkill_Knife.Data());
		}

		public override void OnInit(SSkillFactory skillFactory, SSkillData skillData)
		{
			this.forceKnifeType = SSkill_Knife.KnifeType.None;
			this.m_skillType = SkillType.Knife;
			base.m_onSkillOneHurtAfter = (Action<SSkillBase, int, int>)Delegate.Combine(base.m_onSkillOneHurtAfter, new Action<SSkillBase, int, int>(this.TriggerBuff));
			base.m_onSkillCountChange = (Action<SSkillBase>)Delegate.Combine(base.m_onSkillCountChange, new Action<SSkillBase>(this.TriggerSkill));
			base.OnInit(skillFactory, skillData);
		}

		public override void OnDeInit()
		{
			this.forceKnifeType = SSkill_Knife.KnifeType.None;
			base.m_onSkillOneHurtAfter = (Action<SSkillBase, int, int>)Delegate.Remove(base.m_onSkillOneHurtAfter, new Action<SSkillBase, int, int>(this.TriggerBuff));
			base.m_onSkillCountChange = (Action<SSkillBase>)Delegate.Remove(base.m_onSkillCountChange, new Action<SSkillBase>(this.TriggerSkill));
			base.OnDeInit();
		}

		protected override void OnPlaying()
		{
			this.fireCount = this.GetFireCount();
			base.ReportPlay(0);
			base.SkillAddBuffs(SkillTriggerBuffState.Start);
			this.OnFunction();
			base.CheckSpecialSkill();
			this.DoEventNodes();
		}

		public override void DoEventNodes()
		{
			this.forceKnifeType = (SSkill_Knife.KnifeType)this.GetForceKnifeType();
			this.CheckBomb();
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
						int num = 0;
						while (num < base.Owner.memberData.attribute.KnifeCount)
						{
							if (!base.CurSelectTargetDatas[0].m_target.IsDeath)
							{
								this.FireBullet(this.m_curFireIndex, true);
								this.ActiveChangeSkillTriggerCount();
								base.AddSkillLegacyPower();
							}
							else
							{
								if (!base.CheckCanPlay(null))
								{
									break;
								}
								this.FireBullet(this.m_curFireIndex, true);
								this.ActiveChangeSkillTriggerCount();
								base.AddSkillLegacyPower();
							}
							num++;
						}
						base.AddSkillRecharge();
					}
					else if (eventNode.eventName.Equals("End") && this.isLastEventNodes)
					{
						this.curEndFrame = eventNode.frame;
						this.FinishOneAttack();
					}
				}
				return;
			}
			this.m_curFireIndex++;
			for (int j = 0; j < this.fireCount; j++)
			{
				if (!base.CurSelectTargetDatas[0].m_target.IsDeath)
				{
					this.FireBullet(this.m_curFireIndex, true);
					this.ActiveChangeSkillTriggerCount();
					base.AddSkillLegacyPower();
				}
				else
				{
					if (!base.CheckCanPlay(null))
					{
						break;
					}
					this.FireBullet(this.m_curFireIndex, true);
					this.ActiveChangeSkillTriggerCount();
					base.AddSkillLegacyPower();
				}
			}
			base.AddSkillRecharge();
			this.FinishOneAttack();
		}

		private int GetForceKnifeType()
		{
			SSkill_TriggerKnife.Data data = ((base.m_curSkillTriggerData != null && base.m_curSkillTriggerData.m_parameter != null) ? (base.m_curSkillTriggerData.m_parameter as SSkill_TriggerKnife.Data) : null);
			if (data != null)
			{
				return data.forceKnifeType;
			}
			return 0;
		}

		private int GetFireCount()
		{
			SSkill_TriggerKnife.Data data = ((base.m_curSkillTriggerData != null && base.m_curSkillTriggerData.m_parameter != null) ? (base.m_curSkillTriggerData.m_parameter as SSkill_TriggerKnife.Data) : null);
			int num;
			if (data != null && data.triggerCount > 0)
			{
				num = data.triggerCount;
			}
			else
			{
				SkillTriggerType triggerType = base.m_curSkillTriggerData.m_iHitTargetSkill.m_curSkillTriggerData.m_triggerType;
				if (triggerType == SkillTriggerType.BattleStart)
				{
					num = base.Owner.memberData.attribute.BattleStartKnifeCount.RoundToInt();
				}
				else if (triggerType == SkillTriggerType.RoleRoundStart)
				{
					num = base.Owner.memberData.attribute.RoundStartKnifeCount.RoundToInt();
				}
				else
				{
					num = base.Owner.memberData.attribute.KnifeCount.RoundToInt();
				}
			}
			return num;
		}

		private void FinishOneAttack()
		{
			if (base.CastType == SkillCastType.Counter)
			{
				this.FinishSkill();
				return;
			}
			this.m_CastType = SkillCastType.Attack;
			if (!(this.curComboCount < base.Owner.memberData.attribute.KnifeComboCount))
			{
				this.FinishCombo();
				return;
			}
			this.curComboCount++;
			if (!(base.Owner.m_controller.Random01() <= base.Owner.memberData.attribute.KnifeComboRate))
			{
				this.FinishCombo();
				return;
			}
			this.m_CastType = SkillCastType.Combo;
			if (base.CurSelectTargetDatas.Count > 0 && !base.CurSelectTargetDatas[0].m_target.IsDeath)
			{
				base.SkillAfterCompensation();
				this.OnPlayBefore();
				this.DoEventNodes();
				return;
			}
			if (base.CheckCanPlay(null))
			{
				base.SkillAfterCompensation();
				base.Move(false);
				this.OnPlayBefore();
				this.DoEventNodes();
				return;
			}
			this.FinishCombo();
		}

		private void FinishCombo()
		{
			this.curComboCount = 0;
			this.FinishSkill();
		}

		private void ChangeKnifeAttack(bool isShitKnife, bool isAngelKnife)
		{
			if (isAngelKnife)
			{
				List<string> listString = base.skillData.m_hurtAttributes.GetListString('|');
				for (int i = 0; i < listString.Count; i++)
				{
					FP knifeAngelAttackPercent = this.m_owner.memberData.attribute.KnifeAngelAttackPercent;
					string text = listString[i] + string.Format("*{0}", knifeAngelAttackPercent);
					listString[i] = text;
				}
				base.skillData.m_hurtAttributeDatas = listString;
			}
			else if (isShitKnife)
			{
				List<string> listString2 = base.skillData.m_hurtAttributes.GetListString('|');
				for (int j = 0; j < listString2.Count; j++)
				{
					FP knifeBaseAttackPercent = this.m_owner.memberData.attribute.KnifeBaseAttackPercent;
					FP knifeSuperAttackPercent = this.m_owner.memberData.attribute.KnifeSuperAttackPercent;
					string text2 = listString2[j] + string.Format("*{0}*{1}", knifeBaseAttackPercent, FP._1 + knifeSuperAttackPercent);
					listString2[j] = text2;
				}
				base.skillData.m_hurtAttributeDatas = listString2;
			}
			else
			{
				List<string> listString3 = base.skillData.m_hurtAttributes.GetListString('|');
				for (int k = 0; k < listString3.Count; k++)
				{
					FP knifeBaseAttackPercent2 = this.m_owner.memberData.attribute.KnifeBaseAttackPercent;
					string text3 = listString3[k] + string.Format("*{0}", knifeBaseAttackPercent2);
					listString3[k] = text3;
				}
				base.skillData.m_hurtAttributeDatas = listString3;
			}
			this.m_isShitKnife = isShitKnife;
			this.m_isAngelKnife = isAngelKnife;
		}

		private void CheckBomb()
		{
			bool flag = this.m_owner.m_controller.Random01() <= this.m_owner.memberData.attribute.KnifeSuperRate;
			bool flag2 = this.m_owner.m_controller.Random01() <= this.m_owner.memberData.attribute.KnifeAngelRate;
			if (this.forceKnifeType == SSkill_Knife.KnifeType.Angel)
			{
				this.knifeType = SSkill_Knife.KnifeType.Angel;
				base.skillData.ChangeFireBullets(this.m_data.AngelFireBulletID);
				base.skillData.m_rangeType = SkillRangeType.Single;
				this.ChangeKnifeAttack(false, true);
			}
			else if (this.forceKnifeType == SSkill_Knife.KnifeType.Shit)
			{
				this.knifeType = SSkill_Knife.KnifeType.Shit;
				base.skillData.ChangeFireBullets(this.m_data.BombFireBulletID);
				base.skillData.m_rangeType = SkillRangeType.Group;
				this.ChangeKnifeAttack(true, false);
			}
			else if (this.forceKnifeType == SSkill_Knife.KnifeType.Default)
			{
				this.knifeType = SSkill_Knife.KnifeType.Default;
				base.skillData.InitFireBullets();
				base.skillData.m_rangeType = SkillRangeType.Single;
				this.ChangeKnifeAttack(false, false);
			}
			else if (flag2)
			{
				this.knifeType = SSkill_Knife.KnifeType.Angel;
				base.skillData.ChangeFireBullets(this.m_data.AngelFireBulletID);
				base.skillData.m_rangeType = SkillRangeType.Single;
				this.ChangeKnifeAttack(false, true);
			}
			else if (flag)
			{
				this.knifeType = SSkill_Knife.KnifeType.Shit;
				base.skillData.ChangeFireBullets(this.m_data.BombFireBulletID);
				base.skillData.m_rangeType = SkillRangeType.Group;
				this.ChangeKnifeAttack(true, false);
			}
			else
			{
				this.knifeType = SSkill_Knife.KnifeType.Default;
				base.skillData.InitFireBullets();
				base.skillData.m_rangeType = SkillRangeType.Single;
				this.ChangeKnifeAttack(false, false);
			}
			base.SetSelectTargetData();
			this.isCanTriggerBasic = !flag;
		}

		private void TriggerBuff(SSkillBase skill, int targetSelectIndex, int totalTargetCount)
		{
			if (this.knifeType == SSkill_Knife.KnifeType.Angel)
			{
				this.AddKnifeAngelBuff(targetSelectIndex, totalTargetCount);
			}
			if (this.isCanTriggerBasic)
			{
				this.TriggerPoisonBuff(targetSelectIndex, totalTargetCount);
				this.TriggerRevertBuff(targetSelectIndex, totalTargetCount);
				this.TriggerRageBuff(targetSelectIndex, totalTargetCount);
			}
		}

		private void AddKnifeAngelBuff(int targetSelectIndex, int totalTargetCount)
		{
			if (this.m_data.KnifeAngelBuffsOwner != null && this.m_data.KnifeAngelBuffsOwner.Length != 0)
			{
				for (int i = 0; i < this.m_data.KnifeAngelBuffsOwner.Length; i++)
				{
					this.m_owner.buffFactory.AddBuff(base.Owner, this.m_data.KnifeAngelBuffsOwner[i]);
				}
			}
			if (this.m_data.KnifeAngelBuffsTarget != null && this.m_data.KnifeAngelBuffsTarget.Length != 0 && base.CurSelectTargetDatas != null && base.CurSelectTargetDatas.Count > targetSelectIndex)
			{
				SMemberBase target = base.CurSelectTargetDatas[targetSelectIndex].m_target;
				if (target.IsDeath)
				{
					return;
				}
				for (int j = 0; j < this.m_data.KnifeAngelBuffsTarget.Length; j++)
				{
					target.buffFactory.AddBuff(base.Owner, this.m_data.KnifeAngelBuffsTarget[j]);
				}
			}
		}

		private void TriggerSkill(SSkillBase skill)
		{
			if (this.isCanTriggerBasic)
			{
				this.TriggerThunder();
				this.TriggerIcicle();
				this.TriggerFire();
			}
		}

		private void TriggerThunder()
		{
			if (this.m_owner.m_controller.Random01() <= this.m_owner.memberData.attribute.KnifeThunderRate)
			{
				this.m_skillFactory.CheckPlay(SkillTriggerType.Thunder, this);
			}
		}

		private void TriggerIcicle()
		{
			if (this.m_owner.m_controller.Random01() <= this.m_owner.memberData.attribute.KnifeIcicleRate)
			{
				this.m_skillFactory.CheckPlay(SkillTriggerType.Icicle, this);
			}
		}

		private void TriggerFire()
		{
			if (this.m_owner.m_controller.Random01() <= this.m_owner.memberData.attribute.KnifeFireRate)
			{
				this.m_skillFactory.CheckPlay(SkillTriggerType.Fire, this);
			}
		}

		private void TriggerPoisonBuff(int targetSelectIndex, int totalTargetCount)
		{
			if (this.m_owner.m_controller.Random01() <= this.m_owner.memberData.attribute.KnifePoisonRate && !this.m_data.PoisonBuffID.Equals(0) && base.CurSelectTargetDatas != null && base.CurSelectTargetDatas.Count > targetSelectIndex)
			{
				SMemberBase target = base.CurSelectTargetDatas[targetSelectIndex].m_target;
				if (target.IsDeath)
				{
					return;
				}
				target.buffFactory.AddBuff(base.Owner, this.m_data.PoisonBuffID);
			}
		}

		private void TriggerRevertBuff(int targetSelectIndex, int totalTargetCount)
		{
			if (this.m_owner.m_controller.Random01() <= this.m_owner.memberData.attribute.KnifeRevertRate && !this.m_data.RevertBuffID.Equals(0))
			{
				base.Owner.buffFactory.AddBuff(base.Owner, this.m_data.RevertBuffID);
			}
		}

		private void TriggerRageBuff(int targetSelectIndex, int totalTargetCount)
		{
			if (this.m_owner.m_controller.Random01() <= this.m_owner.memberData.attribute.KnifeRageRate && !this.m_data.RageBuffID.Equals(0))
			{
				base.Owner.buffFactory.AddBuff(base.Owner, this.m_data.RageBuffID);
			}
		}

		protected override void AutoChangeSkillTriggerCount()
		{
		}

		public SSkill_Knife.Data m_data;

		public SSkill_Knife.KnifeType knifeType = SSkill_Knife.KnifeType.Default;

		private SSkill_Knife.KnifeType forceKnifeType;

		private int curComboCount;

		private bool isCanTriggerBasic;

		private int fireCount;

		private bool m_isShitKnife;

		private bool m_isAngelKnife;

		public enum KnifeType
		{
			None,
			Default,
			Shit,
			Angel
		}

		public class Data
		{
			public int BombFireBulletID = 1021;

			public int AngelFireBulletID = 1022;

			public int PoisonBuffID;

			public int RevertBuffID;

			public int RageBuffID;

			public int[] KnifeAngelBuffsOwner;

			public int[] KnifeAngelBuffsTarget;
		}
	}
}
