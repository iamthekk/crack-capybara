using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Server
{
	public abstract class SSkillBase
	{
		public SMemberBase Owner
		{
			get
			{
				return this.m_owner;
			}
		}

		public SSkillData skillData
		{
			get
			{
				return this.m_skillData;
			}
		}

		public SSkillFactory skillFactory
		{
			get
			{
				return this.m_skillFactory;
			}
		}

		public SBulletFactory bulletFactory
		{
			get
			{
				return this.m_bulletFactory;
			}
		}

		public SkillType skillType
		{
			get
			{
				return this.m_skillType;
			}
		}

		public SkillCastType CastType
		{
			get
			{
				return this.m_CastType;
			}
		}

		public List<SkillSelectTargetData> CurSelectTargetDatas
		{
			get
			{
				return this.m_targetDatas;
			}
		}

		public SkillTriggerData m_curSkillTriggerData { get; private set; }

		public Action<SSkillBase> m_onPlayStart { get; set; }

		public Action<SSkillBase, int, int> m_onSkillOneHurtAfter { get; set; }

		public Action<SSkillBase> m_onSkillPlayAfter { get; set; }

		protected Action<SSkillBase> m_onSkillCountChange { get; set; }

		public virtual void OnInit(SSkillFactory skillFactory, SSkillData skillData)
		{
			this.m_skillFactory = skillFactory;
			this.m_owner = this.m_skillFactory.Owner;
			this.m_skillData = skillData;
			this.m_bulletFactory = new SBulletFactory();
			this.m_bulletFactory.SetSkill(this);
			this.m_bulletFactory.OnInit();
			this.Init_CD();
			this.Init_Select();
			this.Init_Trigger();
			this.Init_Attribute();
			this.SetParameters(this.m_skillData.m_parameters);
		}

		public virtual void OnDeInit()
		{
			SSkillData skillData = this.m_skillData;
			if (skillData != null)
			{
				skillData.OnDispose();
			}
			this.DeInit_Select();
			this.DeInit_Trigger();
			this.DeInit_Attribute();
			this.DeInit_CD();
		}

		protected virtual void OnPlayBefore()
		{
			this.m_curFireIndex = -1;
			this.isLastEventNodes = true;
			this.curStartFrame = 0;
			this.curEndFrame = 0;
			this.m_isAddSkillRecharge = false;
			this.m_isAddSkillLegacyPower = false;
		}

		protected virtual void OnPlaying()
		{
			this.ReportPlay(0);
			this.SkillAddBuffs(SkillTriggerBuffState.Start);
			this.OnFunction();
			this.CheckSpecialSkill();
			this.DoEventNodes();
		}

		public bool IsFinishCounter { get; set; }

		protected virtual void FinishSkill()
		{
			this.AutoChangeSkillTriggerCount();
			this.SkillAddBuffs(SkillTriggerBuffState.End);
			this.SkillAfterCompensation();
			this.OnSkillPlayAfter();
			this.SkillFinishing();
			if (!this.IsFinishCounter)
			{
				this.ReadyCounter();
			}
			this.Move(true);
			this.ReportPlaySkillComplete(0, true);
		}

		protected virtual void ActiveChangeSkillTriggerCount()
		{
			Action<SSkillBase> onSkillCountChange = this.m_onSkillCountChange;
			if (onSkillCountChange != null)
			{
				onSkillCountChange(this);
			}
			this.skillFactory.OnSkillCountChange(this);
		}

		protected virtual void AutoChangeSkillTriggerCount()
		{
			this.skillFactory.OnSkillCountChange(this);
		}

		protected int SkillAfterCompensation()
		{
			int num = 0;
			if (this.skillData.IsHaveAnimation)
			{
				num = this.curEndFrame - (this.Owner.m_controller.CurFrame - this.curStartFrame);
				if (num >= 0)
				{
					this.m_owner.m_controller.AddCurFrame(num);
				}
				else
				{
					this.m_owner.m_controller.AddCurFrame(3);
				}
			}
			return num;
		}

		protected void OnSkillPlayAfter()
		{
			Action<SSkillBase> onSkillPlayAfter = this.m_onSkillPlayAfter;
			if (onSkillPlayAfter == null)
			{
				return;
			}
			onSkillPlayAfter(this);
		}

		protected void SkillFinishing()
		{
			this.OnSkillFinish();
		}

		protected virtual void OnSkillFinish()
		{
		}

		protected virtual void OnDamageAfter()
		{
		}

		protected virtual void OnFunction()
		{
		}

		protected abstract void SetParameters(string parameters);

		public void SetSelectTargetData(SkillTriggerData triggerData)
		{
			this.GetSelectTargetDatas(out this.m_targetDatas, triggerData);
		}

		public void SetSelectTargetData()
		{
			this.GetSelectTargetDatas(out this.m_targetDatas, this.m_curSkillTriggerData);
		}

		public bool CheckCanPlay(SkillTriggerData triggerData)
		{
			this.SetSelectTargetData(triggerData);
			return this.CurSelectTargetDatas != null && this.CurSelectTargetDatas.Count > 0;
		}

		public bool CheckCounterCanPlay(SkillTriggerData triggerData)
		{
			if (triggerData.m_triggerType == SkillTriggerType.RoleCounter)
			{
				this.m_targetDatas = new List<SkillSelectTargetData>();
				SkillSelectTargetData skillSelectTargetData = new SkillSelectTargetData();
				skillSelectTargetData.m_target = triggerData.m_iHitTargetList[0];
				this.m_targetDatas.Add(skillSelectTargetData);
			}
			return this.m_targetDatas != null && this.m_targetDatas.Count > 0 && !this.m_targetDatas[0].m_target.IsDeath;
		}

		public virtual void Play()
		{
			this.PlayNormal();
		}

		private void PlayNormal()
		{
			this.m_CastType = SkillCastType.Attack;
			Action<SSkillBase> onPlayStart = this.m_onPlayStart;
			if (onPlayStart != null)
			{
				onPlayStart(this);
			}
			this.ResetMaxCD();
			this.Move(false);
			this.OnPlayBefore();
			this.OnPlaying();
		}

		public void PlayCounter()
		{
			this.m_CastType = SkillCastType.Counter;
			this.Move(false);
			this.OnPlayBefore();
			this.OnPlaying();
		}

		protected void ReadyCounter()
		{
			if ((this.skillData.m_freedType == SkillFreedType.Ordinary || this.skillData.m_freedType == SkillFreedType.Big) && (this.CastType == SkillCastType.Attack || this.CastType == SkillCastType.Combo))
			{
				this.Owner.m_controller.PlayCounter(this, this.Owner, this.m_targetDatas, null);
			}
		}

		public virtual void DoEventNodes()
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
						this.curStartFrame = this.Owner.m_controller.CurFrame;
						this.FireBullet(this.m_curFireIndex, i == count - 2);
						this.AddSkillLegacyPower();
						this.AddSkillRecharge();
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
			this.AddSkillLegacyPower();
			this.AddSkillRecharge();
			if (this.isLastEventNodes)
			{
				this.FinishSkill();
			}
		}

		private protected bool m_isAddSkillRecharge { protected get; private set; }

		protected void AddSkillRecharge()
		{
			if (!this.m_isAddSkillRecharge && this.m_CastType == SkillCastType.Attack)
			{
				int recharge = this.m_skillData.m_recharge;
				if (recharge > 0)
				{
					this.Owner.memberData.ChangeRecharge(recharge);
				}
				this.m_isAddSkillRecharge = true;
			}
		}

		private protected bool m_isAddSkillLegacyPower { protected get; private set; }

		protected void AddSkillLegacyPower()
		{
			if (this.m_CastType == SkillCastType.Attack)
			{
				int legacyPower = this.m_skillData.m_legacyPower;
				if (legacyPower > 0)
				{
					if (this.Owner.memberData.cardData.IsPet)
					{
						SMemberBase mainMember = this.Owner.memberFactory.GetMainMember(this.Owner.memberData.Camp);
						if (mainMember != null)
						{
							mainMember.memberData.ChangeAllLegacyPower(legacyPower);
							return;
						}
					}
					else
					{
						this.Owner.memberData.ChangeAllLegacyPower(legacyPower);
					}
				}
			}
		}

		protected void Move(bool isMoveBack)
		{
			if (this.m_skillData.isSkillMove)
			{
				this.ReportMove(isMoveBack);
			}
		}

		private void Init_Attribute()
		{
			this.OnAddBaseAttributes(false);
		}

		private void DeInit_Attribute()
		{
			this.OnAddBaseAttributes(true);
		}

		protected virtual void OnAddBaseAttributes(bool isReverse = false)
		{
			this.MathMemberAttributes(isReverse);
		}

		private void MathMemberAttributes(bool isReverse = false)
		{
			List<MergeAttributeData> mergeAttributeData = this.skillData.m_baseAttributes.GetMergeAttributeData();
			this.Owner.memberData.attribute.MergeAttributes(mergeAttributeData, isReverse);
		}

		public bool IsFullBloodAddDamage { get; private set; }

		public bool IsLessThanBloodAddDamage { get; private set; }

		public void OnFullBloodAddDamage(bool isFullBloodAddDamage)
		{
			this.IsFullBloodAddDamage = isFullBloodAddDamage;
		}

		public void OnLessThanBloodAddDamage(bool isLessThanBloodAddDamage)
		{
			this.IsLessThanBloodAddDamage = isLessThanBloodAddDamage;
		}

		protected void CheckSpecialSkill()
		{
			if (this.skillData.m_freedType == SkillFreedType.Big)
			{
				FP fp = -this.Owner.memberData.CurRecharge;
				this.Owner.memberData.ChangeRecharge(fp);
				ReportTool.AddChangeAttribute(this.Owner, "Recharge", 0, fp, null);
				return;
			}
			if (this.skillData.m_freedType == SkillFreedType.Legacy)
			{
				FP fp2 = -this.Owner.memberData.GetCurLegacyPower(this.skillData.m_id);
				this.Owner.memberData.ChangeLegacyPower(this.skillData.m_id, fp2);
				ReportTool.AddChangeAttribute(this.Owner, "LegacyPower", 0, fp2, string.Format("{0}", this.skillData.m_id));
			}
		}

		protected void SkillAddBuffs(SkillTriggerBuffState state)
		{
			if (state == SkillTriggerBuffState.Start)
			{
				this.OnSkillStartOwnerAddBuffs();
				this.OnSkillStartTargetAddBuffs();
				this.OnSkillStartFriendAddBuffs();
				return;
			}
			if (state == SkillTriggerBuffState.End)
			{
				this.OnSkillEndOwnerAddBuffs();
			}
		}

		private void OnSkillStartOwnerAddBuffs()
		{
			List<int> skillStartOwnerAddBuffs = this.m_skillData.GetSkillStartOwnerAddBuffs();
			this.OnSkillOwnerAddBuffs(skillStartOwnerAddBuffs);
		}

		private void OnSkillEndOwnerAddBuffs()
		{
			List<int> skillEndOwnerAddBuffs = this.m_skillData.GetSkillEndOwnerAddBuffs();
			this.OnSkillOwnerAddBuffs(skillEndOwnerAddBuffs);
		}

		private void OnSkillOwnerAddBuffs(List<int> buffs)
		{
			if (buffs.Count > 0)
			{
				this.Owner.AddBuffs(this.Owner, buffs);
			}
		}

		private void OnSkillStartTargetAddBuffs()
		{
			List<SkillSelectTargetData> curSelectTargetDatas = this.CurSelectTargetDatas;
			if (curSelectTargetDatas == null)
			{
				return;
			}
			for (int i = 0; i < curSelectTargetDatas.Count; i++)
			{
				List<int> skillStartTargetAddBuffs = this.m_skillData.GetSkillStartTargetAddBuffs();
				if (skillStartTargetAddBuffs.Count > 0)
				{
					curSelectTargetDatas[i].m_target.AddBuffs(this.Owner, skillStartTargetAddBuffs);
				}
			}
		}

		private void OnSkillStartFriendAddBuffs()
		{
			List<int> skillStartFriendAddBuffs = this.skillData.GetSkillStartFriendAddBuffs();
			this.OnSkillFriendAddBuffs(skillStartFriendAddBuffs);
		}

		private void OnSkillEndFriendAddBuffs()
		{
			List<int> skillEndFriendAddBuffs = this.skillData.GetSkillEndFriendAddBuffs();
			this.OnSkillFriendAddBuffs(skillEndFriendAddBuffs);
		}

		private void OnSkillFriendAddBuffs(List<int> buffIDs)
		{
			if (buffIDs.Count > 0)
			{
				List<SMemberBase> membersByCamp = this.Owner.memberFactory.GetMembersByCamp(this.Owner.memberData.Camp);
				for (int i = 0; i < membersByCamp.Count; i++)
				{
					SMemberBase smemberBase = membersByCamp[i];
					if (!smemberBase.IsDeath && smemberBase.memberData.cardData.m_memberRace != MemberRace.Pet)
					{
						smemberBase.AddBuffs(this.Owner, buffIDs);
					}
				}
			}
		}

		protected virtual void FireBullet(int fireBulletIndex = 0, bool isLastFire = true)
		{
			SkillFireBulletData fireBulletData = this.m_skillData.GetFireBulletData(fireBulletIndex);
			if (fireBulletData == null)
			{
				return;
			}
			this.CreateBullet(new CreateBulletData
			{
				m_bulletID = fireBulletData.m_bulletID,
				m_fireBulletID = fireBulletData.m_fireBulletID,
				m_isLastBullet = isLastFire,
				m_skill = this,
				m_selectTargets = this.CurSelectTargetDatas
			});
		}

		protected SBulletBase CreateBullet(CreateBulletData createBulletData)
		{
			SBulletFactory bulletFactory = this.m_bulletFactory;
			if (bulletFactory == null)
			{
				return null;
			}
			return bulletFactory.CreateBullet(createBulletData);
		}

		public int CurCD
		{
			get
			{
				return this.m_curCD;
			}
			set
			{
				this.m_curCD = value;
			}
		}

		protected void Init_CD()
		{
			this.ResetInitCD();
		}

		protected void DeInit_CD()
		{
		}

		protected void SubCD(int cdCount = 1)
		{
			if (this.m_curCD > 0)
			{
				this.m_curCD -= cdCount;
			}
		}

		protected void ResetMaxCD()
		{
			this.m_curCD = this.m_skillData.m_maxCD;
		}

		public void ResetInitCD()
		{
			this.m_curCD = this.m_skillData.m_initCD;
		}

		public void ResetCDToZero()
		{
			int num = 9999;
			if (this.m_skillData.m_maxCD < num)
			{
				this.ResetInitCD();
			}
		}

		public void RefreshCD(int roundCount = 1)
		{
			this.SubCD(roundCount);
		}

		public bool IsSkillHit { get; private set; }

		public void OnHitted(List<SMemberBase> members, SBulletBase bullet)
		{
			this.IsSkillHit = false;
			bool flag = false;
			string curHurtAttributeData = this.GetCurHurtAttributeData(this.m_curFireIndex);
			if (this.m_skillData.m_hurtAttributeDatas.Count > 0)
			{
				int count = members.Count;
				for (int i = 0; i < members.Count; i++)
				{
					int num = i;
					SMemberBase smemberBase = members[i];
					if (smemberBase != null && !smemberBase.IsDeath)
					{
						Dictionary<HurtType, HurtData> hurt = this.m_owner.GetHurt(curHurtAttributeData, smemberBase);
						bool isPet = this.m_owner.memberData.cardData.IsPet;
						SMemberBase smemberBase2 = (isPet ? this.m_owner : null);
						SMemberBase smemberBase3 = (isPet ? this.m_owner.memberFactory.GetMainMember(this.m_owner.memberData.Camp) : this.m_owner);
						FP fp;
						bool flag2;
						smemberBase.ToHittedBySkill(smemberBase3, this, bullet, smemberBase2, hurt, isPet, i + 1, out fp, out flag2);
						if (this.skillData.m_effectType == SkillEffectType.Hurt)
						{
							if (!flag2)
							{
								this.IsSkillHit = true;
								flag = true;
								this.AddHittedBuffs(bullet, smemberBase);
								Action<SSkillBase, int, int> onSkillOneHurtAfter = this.m_onSkillOneHurtAfter;
								if (onSkillOneHurtAfter != null)
								{
									onSkillOneHurtAfter(this, num, count);
								}
							}
						}
						else
						{
							this.IsSkillHit = true;
							flag = true;
							this.AddHittedBuffs(bullet, smemberBase);
							Action<SSkillBase, int, int> onSkillOneHurtAfter2 = this.m_onSkillOneHurtAfter;
							if (onSkillOneHurtAfter2 != null)
							{
								onSkillOneHurtAfter2(this, num, count);
							}
						}
					}
				}
			}
			else
			{
				this.AddHittedBuffs(bullet, members);
				this.OnSkillEndFriendAddBuffs();
			}
			if (flag)
			{
				if (this.skillData.m_effectType == SkillEffectType.Hurt)
				{
					this.OnDamageAfter();
					this.skillFactory.CheckPlay(SkillTriggerType.RoleHurtHitAfter, this);
				}
				this.OnSkillEndFriendAddBuffs();
			}
		}

		protected void AddHittedBuffs(SBulletBase bullet, SMemberBase member)
		{
			if (!member.IsDeath)
			{
				member.AddBuffs(bullet.m_skill.Owner, bullet.m_skill.m_skillData.GetSkillEndTargetAddBuffs());
				member.AddBuffs(bullet.m_skill.Owner, bullet.m_bulletData.GetHitAddBuffs());
			}
		}

		protected void AddHittedBuffs(SBulletBase bullet, List<SMemberBase> members)
		{
			for (int i = 0; i < members.Count; i++)
			{
				SMemberBase smemberBase = members[i];
				if (smemberBase != null && !smemberBase.IsDeath)
				{
					this.AddHittedBuffs(bullet, smemberBase);
				}
			}
		}

		private string GetCurHurtAttributeData(int fireIndex = 0)
		{
			string text = string.Empty;
			List<string> hurtAttributeDatas = this.skillData.m_hurtAttributeDatas;
			if (!hurtAttributeDatas.Count.Equals(0))
			{
				if (fireIndex >= 0)
				{
					if (fireIndex < hurtAttributeDatas.Count)
					{
						text = hurtAttributeDatas[fireIndex];
					}
					else
					{
						text = hurtAttributeDatas[hurtAttributeDatas.Count - 1];
					}
				}
				else
				{
					text = hurtAttributeDatas[0];
				}
			}
			return text;
		}

		protected void ReportPlay(int frame = 0)
		{
			BattleReportData_PlaySkill battleReportData_PlaySkill = this.m_owner.m_controller.CreateReport<BattleReportData_PlaySkill>();
			battleReportData_PlaySkill.m_memberInstanceID = this.m_owner.m_instanceId;
			battleReportData_PlaySkill.m_skillId = this.m_skillData.m_id;
			battleReportData_PlaySkill.m_targetList = new List<int>();
			battleReportData_PlaySkill.m_curCD = this.m_curCD;
			battleReportData_PlaySkill.m_maxCD = this.m_skillData.m_maxCD;
			battleReportData_PlaySkill.m_castType = this.CastType;
			this.m_owner.m_controller.AddReport(battleReportData_PlaySkill, frame, false);
		}

		protected void ReportPlaySkillComplete(int frame = 0, bool isAddCurFrame = true)
		{
			BattleReportData_PlaySkillComplete battleReportData_PlaySkillComplete = this.m_owner.m_controller.CreateReport<BattleReportData_PlaySkillComplete>();
			battleReportData_PlaySkillComplete.m_memberInstanceID = this.m_owner.m_instanceId;
			battleReportData_PlaySkillComplete.m_skillId = this.m_skillData.m_id;
			if (this.skillData.m_freedType == SkillFreedType.Legacy)
			{
				battleReportData_PlaySkillComplete.m_displayFrame = 20;
				this.m_owner.m_controller.AddReport(battleReportData_PlaySkillComplete, frame, isAddCurFrame);
				this.m_owner.m_controller.AddCurFrame(20);
				return;
			}
			this.m_owner.m_controller.AddReport(battleReportData_PlaySkillComplete, frame, isAddCurFrame);
		}

		protected void ReportMove(bool isMoveBack)
		{
			BattleReportData_Move battleReportData_Move = this.m_owner.m_controller.CreateReport<BattleReportData_Move>();
			battleReportData_Move.m_memberInstanceID = this.m_owner.m_instanceId;
			battleReportData_Move.m_skillId = this.m_skillData.m_id;
			int num = ((this.CurSelectTargetDatas.Count > 0) ? this.CurSelectTargetDatas[0].m_target.m_instanceId : 0);
			battleReportData_Move.m_targetInstanceID = num;
			battleReportData_Move.m_isMoveBack = isMoveBack;
			this.m_owner.m_controller.AddReport(battleReportData_Move, 0, false);
			this.m_owner.m_controller.AddCurFrame(battleReportData_Move.m_moveFrame);
		}

		protected void ReportChangeAttribute()
		{
			List<BattleReportData_AttributeData> list = new List<BattleReportData_AttributeData>();
			BattleReportData_AttributeData battleReportData_AttributeData = new BattleReportData_AttributeData("Attack", this.Owner.memberData.attribute.GetAttack(), FP._0, null);
			list.Add(battleReportData_AttributeData);
			BattleReportData_AttributeData battleReportData_AttributeData2 = new BattleReportData_AttributeData("Defence", this.Owner.memberData.attribute.GetDefence(), FP._0, null);
			list.Add(battleReportData_AttributeData2);
			ReportTool.AddChangeAttribute(this.Owner, list);
		}

		public void Init_Select()
		{
			this.m_skillSelects = new List<ISkillSelect>();
			for (int i = 0; i < this.skillData.m_selectIDs.Length; i++)
			{
				int num = this.skillData.m_selectIDs[i];
				if (num > 0)
				{
					GameSkill_skillSelect elementById = this.Owner.m_controller.Table.GetGameSkill_skillSelectModelInstance().GetElementById(num);
					Type type = Type.GetType(elementById.sClassName);
					if (type == null)
					{
						HLog.LogError(string.Format("BaseSkill.Init_Select ..string to classType is error!! className =  {0} ,ISkillSelectID = {1} ", elementById.sClassName, num));
						return;
					}
					ISkillSelect skillSelect = Activator.CreateInstance(type) as ISkillSelect;
					this.m_skillSelects.Add(skillSelect);
				}
			}
		}

		public void DeInit_Select()
		{
			this.m_skillSelects.Clear();
		}

		private bool MathSelectTarget(out List<SkillSelectTargetData> selectTargetDatas, SkillTriggerData triggerData, int count = 1)
		{
			selectTargetDatas = new List<SkillSelectTargetData>();
			if (this.m_skillSelects == null)
			{
				return false;
			}
			if (this.Owner == null)
			{
				return false;
			}
			if (this.Owner.m_controller == null)
			{
				return false;
			}
			if (this.Owner.m_controller.memberFactory == null)
			{
				return false;
			}
			List<SMemberBase> list = new List<SMemberBase>();
			for (int i = 0; i < this.m_skillSelects.Count; i++)
			{
				ISkillSelect skillSelect = this.m_skillSelects[i];
				if (skillSelect != null)
				{
					skillSelect.MathSelectTarget(triggerData, this.Owner.m_controller.memberFactory.allMember, this.Owner, this, ref list);
				}
			}
			int num = 0;
			int num2 = 0;
			while (num2 < list.Count && num < count)
			{
				SMemberBase smemberBase = list[num2];
				if (smemberBase != null)
				{
					SkillSelectTargetData skillSelectTargetData = new SkillSelectTargetData();
					skillSelectTargetData.m_target = smemberBase;
					selectTargetDatas.Add(skillSelectTargetData);
					num++;
				}
				num2++;
			}
			return num != 0;
		}

		private bool GetSelectTargetDatas(out List<SkillSelectTargetData> targets, SkillTriggerData triggerData)
		{
			int num = ((this.m_skillData.m_rangeType == SkillRangeType.Single) ? 1 : ((this.m_skillData.m_groupSelectMaxCount != 0) ? this.m_skillData.m_groupSelectMaxCount : 999));
			return this.MathSelectTarget(out targets, triggerData, num);
		}

		public void Init_Trigger()
		{
			this.Init_TriggerContition();
		}

		public void DeInit_Trigger()
		{
			foreach (KeyValuePair<int, BaseSkillTrigger> keyValuePair in this.m_skillTriggers)
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.DeInitCondition();
				}
			}
			this.m_skillTriggers.Clear();
		}

		private void Init_TriggerContition()
		{
			List<string> listString = this.skillData.m_triggerContitions.GetListString('|');
			for (int i = 0; i < listString.Count; i++)
			{
				List<string> listString2 = listString[i].GetListString('!');
				int num = 0;
				string text = string.Empty;
				if (listString2.Count == 1)
				{
					num = int.Parse(listString2[0]);
				}
				else if (listString2.Count == 2)
				{
					num = int.Parse(listString2[0]);
					text = listString2[1];
				}
				BaseSkillTrigger baseSkillTrigger = this.InstanceTrigger(num);
				baseSkillTrigger.SetSkill(this);
				baseSkillTrigger.InitCondition(text);
				baseSkillTrigger.OnInit();
				this.m_skillTriggers[baseSkillTrigger.GetName()] = baseSkillTrigger;
			}
		}

		private BaseSkillTrigger InstanceTrigger(int id)
		{
			if (id <= 90)
			{
				switch (id)
				{
				case 0:
					return new SkillTrigger_RoleRoundFighting();
				case 1:
					return new SkillTrigger_BattleStart();
				case 2:
					return new SkillTrigger_RoundStart();
				case 3:
					return new SkillTrigger_RoundEnd();
				case 4:
					return new SkillTrigger_RoleRoundStart();
				case 5:
					return new SkillTrigger_RoleRoundEnd();
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 21:
				case 22:
				case 23:
				case 24:
				case 25:
				case 26:
				case 27:
				case 28:
				case 29:
				case 36:
				case 37:
				case 38:
				case 39:
				case 44:
				case 45:
				case 46:
				case 47:
				case 48:
				case 49:
				case 57:
				case 58:
				case 59:
					break;
				case 11:
					return new SkillTrigger_RoleHurtHitAfter();
				case 12:
					return new SkillTrigger_NormalAttackAfter();
				case 13:
					return new SkillTrigger_BigSkillAfter();
				case 14:
					return new SkillTrigger_WeaponCritAfter();
				case 15:
					return new SkillTrigger_SmallSkillAfter();
				case 16:
					return new SkillTrigger_SkillCritAfter();
				case 17:
					return new SkillTrigger_RoleOneHurtAfter();
				case 18:
					return new SkillTrigger_RoleSkillAfter();
				case 19:
					return new SkillTrigger_CustomSkillAfter();
				case 20:
					return new SkillTrigger_LegacySkillAfter();
				case 30:
					return new SkillTrigger_RoleToThunderHurtAfter();
				case 31:
					return new SkillTrigger_RoleToIcicleHurtAfter();
				case 32:
					return new SkillTrigger_RoleToKnifeHurtAfter();
				case 33:
					return new SkillTrigger_RoleToFireHurtAfter();
				case 34:
					return new SkillTrigger_RoleToSwordkeeHurtAfter();
				case 35:
					return new SkillTrigger_RoleToFallingSwordHurtAfter();
				case 40:
					return new SkillTrigger_KillEnemyAfter();
				case 41:
					return new SkillTrigger_NormalAttackKillAfter();
				case 42:
					return new SkillTrigger_ComboPointAfter();
				case 43:
					return new SkillTrigger_EnemyFreezeAfter();
				case 50:
					return new SkillTrigger_HitByHurtedAfter();
				case 51:
					return new SkillTrigger_HitByNormalAttackAfter();
				case 52:
					return new SkillTrigger_HitByBigSkillAfter();
				case 53:
					return new SkillTrigger_CounterAfter();
				case 54:
					return new SkillTrigger_MissAfter();
				case 55:
					return new SkillTrigger_ComboAfter();
				case 56:
					return new SkillTrigger_ShieldAfter();
				case 60:
					return new SkillTrigger_HitByHurtedBefore();
				default:
					if (id == 90)
					{
						return new SkillTrigger_ReviveAfter();
					}
					break;
				}
			}
			else
			{
				switch (id)
				{
				case 100:
					return new SkillTrigger_Thunder();
				case 101:
					return new SkillTrigger_Icicle();
				case 102:
					return new SkillTrigger_Knife();
				case 103:
					return new SkillTrigger_Fire();
				case 104:
					return new SkillTrigger_Swordkee();
				case 105:
					return new SkillTrigger_FallingSword();
				case 106:
				case 107:
				case 108:
				case 109:
				case 110:
				case 111:
					break;
				case 112:
					return new SkillTrigger_KnifeTrigger();
				case 113:
					return new SkillTrigger_FallingSwordTrigger();
				default:
					switch (id)
					{
					case 200:
						return new SkillTrigger_OnceSkillThunder();
					case 201:
						return new SkillTrigger_OnceSkillIcicle();
					case 202:
						return new SkillTrigger_OnceSkillKnife();
					case 203:
						return new SkillTrigger_OnceSkillFire();
					case 204:
						return new SkillTrigger_OnceSkillSwordkee();
					case 205:
						return new SkillTrigger_OnceSkillFallingSword();
					default:
						if (id == 999)
						{
							return new SkillTrigger_Undo();
						}
						break;
					}
					break;
				}
			}
			HLog.LogError(string.Format("SkillTrigger is Error. SkillTriggerType = {0}", id));
			return new SkillTrigger_Undo();
		}

		public bool IsCanPlay(SkillTriggerData triggerData)
		{
			if (triggerData == null)
			{
				return false;
			}
			if (this.m_skillTriggers == null)
			{
				return false;
			}
			this.m_curSkillTriggerData = triggerData;
			bool flag = false;
			foreach (BaseSkillTrigger baseSkillTrigger in this.m_skillTriggers.Values)
			{
				if (triggerData.m_triggerType == SkillTriggerType.Undo)
				{
					break;
				}
				if (triggerData.m_triggerType == (SkillTriggerType)baseSkillTrigger.GetName() && baseSkillTrigger != null)
				{
					flag = baseSkillTrigger.IsCanPlay(triggerData);
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				flag = this.CheckCanPlay(triggerData);
				if (flag)
				{
					this.OnRefreshTrigger();
				}
			}
			return flag;
		}

		private void OnRefreshTrigger()
		{
			foreach (BaseSkillTrigger baseSkillTrigger in this.m_skillTriggers.Values)
			{
				baseSkillTrigger.OnRefreshContitions();
			}
		}

		protected SMemberBase m_owner;

		protected SSkillData m_skillData;

		protected SSkillFactory m_skillFactory;

		protected SBulletFactory m_bulletFactory;

		protected SkillType m_skillType;

		protected SkillCastType m_CastType = SkillCastType.Attack;

		protected List<SkillSelectTargetData> m_targetDatas;

		protected int m_curFireIndex = -1;

		protected bool isLastEventNodes;

		protected int curStartFrame;

		protected int curEndFrame;

		protected int m_curCD;

		public SSkillTypeData_TargetShieldAddDamage targetShieldAddDamage;

		public AttackerHpLessAddDamage attackerHpLessAddDamage;

		public TargetHpLessAddDamage targetHpLessAddDamage;

		protected List<ISkillSelect> m_skillSelects;

		private Dictionary<int, BaseSkillTrigger> m_skillTriggers = new Dictionary<int, BaseSkillTrigger>();
	}
}
