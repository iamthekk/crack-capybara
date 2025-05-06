using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Server
{
	public abstract class SMemberBase
	{
		public BaseBattleServerController m_controller { get; private set; }

		public SMemberData memberData
		{
			get
			{
				return this.m_memberData;
			}
		}

		public SMemberFactory memberFactory
		{
			get
			{
				return this.m_memberFactory;
			}
		}

		public SSkillFactory skillFactory
		{
			get
			{
				return this.m_skillFactory;
			}
		}

		public SBuffFactory buffFactory
		{
			get
			{
				return this.m_buffFactory;
			}
		}

		public bool IsDeath
		{
			get
			{
				return this.memberData.CurHP <= FP._0;
			}
		}

		public bool IsBeControlled
		{
			get
			{
				return this.IsStun || this.IsFrozen;
			}
		}

		public bool IsStun
		{
			get
			{
				return this.m_stun != null && this.m_stun.IsControlled;
			}
		}

		public bool IsFrozen
		{
			get
			{
				return this.m_frozen != null && this.m_frozen.IsControlled;
			}
		}

		public bool IsSilence
		{
			get
			{
				return this.m_Silence != null && this.m_Silence.IsControlled;
			}
		}

		public void Init()
		{
			this.m_instanceId = this.m_memberData.cardData.m_instanceID;
			this.Init_Attribute();
			this.Init_Skill();
			this.Init_Buff();
			this.Init_MemberSubmodule();
			this.OnInit();
			this.memberData.LogAttributes();
		}

		public void DeInit()
		{
			this.OnDeInit();
			this.memberData.Clear();
			this.DeInit_Skill();
			this.DeInit_Buff();
			this.DeInit_Attribute();
			this.DeInit_MemberSubmodule();
		}

		public void SetMemberData(CardData cardData, GameMember_member member)
		{
			this.m_memberData = new SMemberData();
			this.m_memberData.SetCardData(cardData);
			this.m_memberData.SetTableData(member);
			this.m_memberData.SetSMmemberBase(this);
		}

		public void SetController(BaseBattleServerController controller)
		{
			this.m_controller = controller;
		}

		public void SetMemberFactory(SMemberFactory factory)
		{
			this.m_memberFactory = factory;
		}

		public virtual void OnInit()
		{
		}

		public virtual void OnDeInit()
		{
		}

		public abstract void SetParameters(string parameters);

		public Dictionary<HurtType, HurtData> GetHurt(string attribute, SMemberBase target)
		{
			if (target == null)
			{
				return new Dictionary<HurtType, HurtData>();
			}
			HurtAttributeParameterMethods hurtAttributeParameterMethods = new HurtAttributeParameterMethods();
			SMemberBase smemberBase = ((!this.memberData.cardData.IsPet) ? this : this.memberFactory.GetMainMember(this.memberData.Camp));
			hurtAttributeParameterMethods.OnRefreshByAttacker(smemberBase, target, null);
			return attribute.GetAddMergeAttributeData(hurtAttributeParameterMethods.m_fields, hurtAttributeParameterMethods.m_valuse, ',').ToHurtDatas(this.m_controller.Table);
		}

		public void RefreshMemberRoundState()
		{
			SSkillFactory skillFactory = this.m_skillFactory;
			if (skillFactory == null)
			{
				return;
			}
			skillFactory.RefreshSkillState();
		}

		private void Init_Attribute()
		{
			this.SetCurValue();
			SMemberData memberData = this.m_memberData;
			memberData.onAttributeChanged = (Action<string, FP, FP, string>)Delegate.Combine(memberData.onAttributeChanged, new Action<string, FP, FP, string>(this.OnAttributeChanged));
			SMemberData memberData2 = this.m_memberData;
			memberData2.onReviveCallback = (Action<FP>)Delegate.Combine(memberData2.onReviveCallback, new Action<FP>(this.OnRevive));
		}

		private void DeInit_Attribute()
		{
			SMemberData memberData = this.m_memberData;
			memberData.onAttributeChanged = (Action<string, FP, FP, string>)Delegate.Remove(memberData.onAttributeChanged, new Action<string, FP, FP, string>(this.OnAttributeChanged));
			SMemberData memberData2 = this.m_memberData;
			memberData2.onReviveCallback = (Action<FP>)Delegate.Remove(memberData2.onReviveCallback, new Action<FP>(this.OnRevive));
		}

		private void SetCurValue()
		{
			if (this.m_memberData.cardData.m_curHp < FP._0)
			{
				this.memberData.CurHP = this.memberData.attribute.GetHpMax();
			}
			else
			{
				this.memberData.CurHP = this.m_memberData.cardData.m_curHp;
			}
			if (this.m_memberData.cardData.m_curEnergy < FP._0)
			{
				this.memberData.CurRecharge = FP._0;
			}
			else
			{
				this.memberData.CurRecharge = this.m_memberData.cardData.m_curEnergy;
			}
			foreach (KeyValuePair<int, FP> keyValuePair in this.m_memberData.CurLegacyPowerDict)
			{
				this.memberData.CurLegacyPowerDict[keyValuePair.Key] = keyValuePair.Value;
			}
			this.memberData.IsReviveUsed = this.memberData.cardData.m_reviveUsed;
		}

		private void OnAttributeChanged(string key, FP curValue, FP changeValue, string param)
		{
			if (key.Equals("Shield") || key.Equals("ShieldThunder") || key.Equals("ShieldDurian") || key.Equals("Recharge") || key.Equals("LegacyPower"))
			{
				ReportTool.AddChangeAttribute(this, key, curValue, changeValue, param);
			}
		}

		private void OnRevive(FP reviveHp)
		{
			ReportTool.AddReviveReport(this, reviveHp);
			this.skillFactory.CheckPlay(SkillTriggerType.ReviveAfter);
		}

		public void CheckMainDeath()
		{
			if (this.IsDeath && this.m_memberData.cardData.m_isMainMember)
			{
				this.memberFactory.OnGameOver();
			}
		}

		private void Init_Buff()
		{
			this.m_buffFactory = new SBuffFactory(this);
			this.m_buffFactory.OnInit();
		}

		private void DeInit_Buff()
		{
			this.m_buffFactory.OnDeInit();
			this.m_buffFactory = null;
		}

		private void TriggerMemberBuffs(BuffTriggerTags tag)
		{
			if (this.IsDeath)
			{
				return;
			}
			SBuffFactory buffFactory = this.buffFactory;
			if (buffFactory == null)
			{
				return;
			}
			buffFactory.TriggerBuffs(tag);
		}

		public void AddBuffs(SMemberBase attacker, List<int> buffIDs)
		{
			SBuffFactory buffFactory = this.m_buffFactory;
			if (buffFactory == null)
			{
				return;
			}
			buffFactory.AddBuffs(attacker, buffIDs);
		}

		public void RefreshCD_Buffs()
		{
			SBuffFactory buffFactory = this.m_buffFactory;
			if (buffFactory == null)
			{
				return;
			}
			buffFactory.RefreshAllRoundCD();
		}

		public void RemoveAllBuffs()
		{
			SBuffFactory buffFactory = this.m_buffFactory;
			if (buffFactory == null)
			{
				return;
			}
			buffFactory.RemoveAllBuffs();
		}

		private void Init_MemberSubmodule()
		{
			this.m_stun = new SMemberSubmodule_Stun(this);
			this.m_stun.Init();
			this.m_frozen = new SMemberSubmodule_Frozen(this);
			this.m_frozen.Init();
			this.m_Silence = new SMemberSubmodule_Silence(this);
			this.m_Silence.Init();
		}

		private void DeInit_MemberSubmodule()
		{
			SMemberSubmodule_Stun stun = this.m_stun;
			if (stun != null)
			{
				stun.DeInit();
			}
			SMemberSubmodule_Frozen frozen = this.m_frozen;
			if (frozen != null)
			{
				frozen.DeInit();
			}
			SMemberSubmodule_Silence silence = this.m_Silence;
			if (silence == null)
			{
				return;
			}
			silence.DeInit();
		}

		public void OnRoleRoundEnd_MemberSubmodule()
		{
			SMemberSubmodule_Stun stun = this.m_stun;
			if (stun != null)
			{
				stun.OnRoleRoundEnd(1);
			}
			SMemberSubmodule_Frozen frozen = this.m_frozen;
			if (frozen != null)
			{
				frozen.OnRoleRoundEnd(1);
			}
			SMemberSubmodule_Silence silence = this.m_Silence;
			if (silence == null)
			{
				return;
			}
			silence.OnRoleRoundEnd(1);
		}

		protected void ReportWaitRoundCount(int waitCount)
		{
			BattleReportData_WaitRoundCount battleReportData_WaitRoundCount = this.m_controller.CreateReport<BattleReportData_WaitRoundCount>();
			battleReportData_WaitRoundCount.TargetInstanceID = this.m_instanceId;
			battleReportData_WaitRoundCount.WaitRoundCount = waitCount;
			this.m_controller.AddReport(battleReportData_WaitRoundCount, 0, false);
		}

		public void ReportTextTips(string textID)
		{
			BattleReportData_TextTips battleReportData_TextTips = this.m_controller.CreateReport<BattleReportData_TextTips>();
			battleReportData_TextTips.TargetInstanceID = this.m_instanceId;
			battleReportData_TextTips.TextID = textID;
			this.m_controller.AddReport(battleReportData_TextTips, 0, false);
		}

		private void Init_Skill()
		{
			this.m_skillFactory = new SSkillFactory();
			this.m_skillFactory.OnInit(this.m_controller, this);
			this.m_skillFactory.AddSkills(this.m_memberData.cardData.skillIDs);
			this.m_memberData.UpdateLegacyPowerMax();
		}

		private void DeInit_Skill()
		{
			this.m_skillFactory.OnDeInit();
			this.m_skillFactory = null;
		}

		public void PlayCounter(SkillTriggerData triggerData, Action Complete)
		{
			SSkillFactory skillFactory = this.m_skillFactory;
			if (skillFactory == null)
			{
				return;
			}
			skillFactory.PlayCounter(triggerData, Complete);
		}

		public MemberRoundState RoundState
		{
			get
			{
				return this.m_roundState;
			}
			protected set
			{
				this.m_roundState = value;
			}
		}

		public void RefreshRoundState()
		{
			this.m_roundState = MemberRoundState.RoundReady;
		}

		public void SetRoundState(MemberRoundState state)
		{
			this.m_roundState = state;
		}

		public void BattleStart()
		{
			Action onBattleStart = this.OnBattleStart;
			if (onBattleStart != null)
			{
				onBattleStart();
			}
			this.memberData.CurComboCount = 0;
			this.memberData.attribute.ResetThunderHitCount();
		}

		public void RoundStart()
		{
			Action onRoundStart = this.OnRoundStart;
			if (onRoundStart != null)
			{
				onRoundStart();
			}
			this.TriggerMemberBuffs(BuffTriggerTags.RoundStart);
		}

		public void RoundEnd()
		{
			this.RefreshMemberRoundState();
			this.memberData.attribute.ResetThunderHitCount();
			this.TriggerMemberBuffs(BuffTriggerTags.RoundEnd);
			Action onRoundEnd = this.OnRoundEnd;
			if (onRoundEnd == null)
			{
				return;
			}
			onRoundEnd();
		}

		private void RoleRoundStart()
		{
			this.TriggerMemberBuffs(BuffTriggerTags.MemberRoundStart);
			Action onRoleRoundStart = this.OnRoleRoundStart;
			if (onRoleRoundStart != null)
			{
				onRoleRoundStart();
			}
			this.skillFactory.Trigger(SkillTriggerType.RoleRoundStart);
		}

		private void RoleRoundEnd()
		{
			this.memberData.CurComboCount = 0;
			this.skillFactory.Trigger(SkillTriggerType.RoleRoundEnd);
			this.TriggerMemberBuffs(BuffTriggerTags.MemberRoundEnd);
			this.RefreshCD_Buffs();
			this.OnRoleRoundEnd_MemberSubmodule();
			Action onRoleRoundEnd = this.OnRoleRoundEnd;
			if (onRoleRoundEnd != null)
			{
				onRoleRoundEnd();
			}
			this.m_controller.AddCurFrame(3);
		}

		public void OnMemberRoundPlaying()
		{
			this.RoleRoundStart();
			this.RoleRoundFighting();
			this.RoleRoundEnd();
			this.memberFactory.OnGameOver();
		}

		protected virtual void RoleRoundFighting()
		{
			this.skillFactory.Trigger(SkillTriggerType.RoleRoundFighting);
		}

		public void ToHittedBySkill(SMemberBase attacker, SSkillBase skill, SBulletBase bullet, SMemberBase attackerPet, Dictionary<HurtType, HurtData> hurtDatas, bool isPetAttack, int targetSelectIndex, out FP allAttack, out bool isMiss)
		{
			allAttack = 0;
			isMiss = false;
			foreach (KeyValuePair<HurtType, HurtData> keyValuePair in hurtDatas)
			{
				HurtType key = keyValuePair.Key;
				HurtBase hurtInstance = this.GetHurtInstance(key);
				hurtInstance.SetMember(this, attacker, attackerPet);
				hurtInstance.SetSkill(skill, bullet, keyValuePair.Value);
				hurtInstance.SetIsPetAttack(isPetAttack);
				hurtInstance.SetTargetSelectIndex(targetSelectIndex);
				long num;
				bool flag;
				hurtInstance.Run(out num, out isMiss, out flag);
				if (attacker.memberData.Camp == MemberCamp.Friendly)
				{
					this.memberFactory.MainRoleDamage(hurtInstance.GetDamage());
				}
				allAttack += num;
				if (flag)
				{
					attacker.skillFactory.OnWeaponCritAfter(skill);
				}
			}
			if (!isMiss)
			{
				Action<SSkillBase> onBeAttacked = this.OnBeAttacked;
				if (onBeAttacked != null)
				{
					onBeAttacked(skill);
				}
				this.OnBulletHitted(attacker, skill, bullet, null, allAttack);
				this.m_skillFactory.CheckPlay(SkillTriggerType.HitByHurtedAfter, skill);
				if (skill.skillData.m_freedType == SkillFreedType.Ordinary)
				{
					this.m_skillFactory.CheckPlay(SkillTriggerType.HitByNormalAttackAfter, skill);
				}
				else if (skill.skillData.m_freedType == SkillFreedType.Big)
				{
					this.m_skillFactory.CheckPlay(SkillTriggerType.HitByBigSkillAfter, skill);
				}
			}
			else
			{
				this.m_skillFactory.CheckPlay(SkillTriggerType.MissAfter, skill);
			}
			if (this.IsDeath)
			{
				attacker.skillFactory.OnNormalAttackKillAfter(skill);
				Action<SSkillBase> onKillTarget = attacker.OnKillTarget;
				if (onKillTarget != null)
				{
					onKillTarget(skill);
				}
				this.CheckMainDeath();
			}
		}

		public void ToHittedByBuff(SMemberBase attacker, SBuffBase buff, SMemberBase attackerPet, bool isPetAttack, Dictionary<HurtType, HurtData> hurtDatas, long energy, out long allAttack)
		{
			allAttack = 0L;
			foreach (KeyValuePair<HurtType, HurtData> keyValuePair in hurtDatas)
			{
				HurtType key = keyValuePair.Key;
				HurtBase hurtInstance = this.GetHurtInstance(key);
				hurtInstance.SetMember(this, attacker, attackerPet);
				hurtInstance.SetBuff(buff, keyValuePair.Value);
				hurtInstance.SetIsPetAttack(isPetAttack);
				long num;
				bool flag;
				bool flag2;
				hurtInstance.Run(out num, out flag, out flag2);
				if (attacker.memberData.Camp == MemberCamp.Friendly)
				{
					this.memberFactory.MainRoleDamage(hurtInstance.GetDamage());
				}
				allAttack += num;
			}
			this.OnBulletHitted(attacker, null, null, buff, allAttack);
			if (allAttack > 0L)
			{
				this.m_skillFactory.CheckPlay(SkillTriggerType.HitByHurtedAfter, buff);
			}
		}

		private HurtBase GetHurtInstance(HurtType hurtType)
		{
			switch (hurtType)
			{
			case HurtType.Attack:
				return new HurtAttack();
			case HurtType.CritAttack:
				return new HurtCritAttack();
			case HurtType.TrueAttack:
				return new HurtTrueAttack();
			case HurtType.IceAttack:
				return new HurtIceAttack();
			case HurtType.FireAttack:
				return new HurtFireAttack();
			case HurtType.FireBuffAttack:
				return new HurtFireBuffAttack();
			case HurtType.ThunderAttack:
				return new HurtThunderAttack();
			case HurtType.PoisonBuffAttack:
				return new HurtPoisonBuffAttack();
			case HurtType.RecoverHP:
				return new HurtRecoverHP();
			case HurtType.PhysicalAttack:
				return new HurtPhysicalAttack();
			case HurtType.ShieldCounterAttack:
				return new HurtShieldCounterAttack();
			default:
				HLog.LogError(string.Format("HurtType is Error. hurtType = {0}", hurtType));
				return new HurtAttack();
			}
		}

		private void OnBulletHitted(SMemberBase attacker, SSkillBase skill, SBulletBase bullet, SBuffBase buff, FP value)
		{
			if (skill != null && skill.skillType == SkillType.Thunder)
			{
				this.memberData.attribute.AddThunderHitByCount(1);
			}
		}

		public int m_instanceId;

		private SMemberData m_memberData;

		private SMemberFactory m_memberFactory;

		private SSkillFactory m_skillFactory;

		private SBuffFactory m_buffFactory;

		public Action OnBattleStart;

		public Action OnRoundStart;

		public Action OnRoleRoundStart;

		public Action OnRoleRoundEnd;

		public Action OnRoundEnd;

		public Action OnBattleEnd;

		public Action<SSkillBase> OnBeAttacked;

		public Action<SSkillBase> OnKillTarget;

		public SMemberSubmodule_Stun m_stun;

		public SMemberSubmodule_Frozen m_frozen;

		public SMemberSubmodule_Silence m_Silence;

		private MemberRoundState m_roundState;
	}
}
