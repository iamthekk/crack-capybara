using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Server
{
	public class SSkillFactory
	{
		private BaseBattleServerController m_controller { get; set; }

		public List<SSkillBase> allSkill
		{
			get
			{
				return this.m_skills;
			}
		}

		public SMemberBase Owner
		{
			get
			{
				return this.m_owner;
			}
		}

		public int CurComboCount
		{
			get
			{
				return this.m_curComboCount;
			}
		}

		public Action<SSkillBase> m_onSkillCountChange { get; set; }

		public void OnInit(BaseBattleServerController controller, SMemberBase owner)
		{
			this.m_controller = controller;
			this.m_owner = owner;
			SMemberBase owner2 = this.m_owner;
			owner2.OnKillTarget = (Action<SSkillBase>)Delegate.Combine(owner2.OnKillTarget, new Action<SSkillBase>(this.OnKillEnemyAfter));
			this.m_skills = new List<SSkillBase>();
		}

		public void OnDeInit()
		{
			this.m_skills.Clear();
			this.m_skills = null;
			SMemberBase owner = this.m_owner;
			owner.OnKillTarget = (Action<SSkillBase>)Delegate.Remove(owner.OnKillTarget, new Action<SSkillBase>(this.OnKillEnemyAfter));
			this.m_owner = null;
			this.m_controller = null;
		}

		private void OnPlayStart(SSkillBase skill)
		{
			Action<SSkillBase> onPlayStart = this.m_onPlayStart;
			if (onPlayStart == null)
			{
				return;
			}
			onPlayStart(skill);
		}

		private void OnSkillOneHurtAfter(SSkillBase skill, int targetIndex, int totalTargetCount)
		{
			Action<SSkillBase> onSkillOneHurtAfter = this.m_onSkillOneHurtAfter;
			if (onSkillOneHurtAfter != null)
			{
				onSkillOneHurtAfter(skill);
			}
			this.CheckPlay(SkillTriggerType.ComboPointAfter, skill);
			if (skill.skillType != SkillType.Default)
			{
				if (skill.skillType == SkillType.Thunder)
				{
					this.CheckPlay(SkillTriggerType.RoleToThunderHurtAfter, skill);
					return;
				}
				if (skill.skillType == SkillType.Icicle)
				{
					this.CheckPlay(SkillTriggerType.RoleToIcicleHurtAfter, skill);
					return;
				}
				if (skill.skillType == SkillType.Knife)
				{
					this.CheckPlay(SkillTriggerType.RoleToKnifeHurtAfter, skill);
					return;
				}
				if (skill.skillType == SkillType.Fire)
				{
					this.CheckPlay(SkillTriggerType.RoleToFireHurtAfter, skill);
					return;
				}
				if (skill.skillType == SkillType.Swordkee)
				{
					this.CheckPlay(SkillTriggerType.RoleToSwordkeeHurtAfter, skill);
					return;
				}
				if (skill.skillType == SkillType.FallingSword)
				{
					this.CheckPlay(SkillTriggerType.RoleToFallingSwordHurtAfter, skill);
				}
			}
		}

		public void OnSkillPlayAfter(SSkillBase skill)
		{
			if (skill.skillData.m_freedType == SkillFreedType.Ordinary)
			{
				this.CheckPlay(SkillTriggerType.NormalAttackAfter, skill);
				return;
			}
			if (skill.skillData.m_freedType == SkillFreedType.Small)
			{
				this.CheckPlay(SkillTriggerType.SmallSkillAfter, skill);
				return;
			}
			if (skill.skillData.m_freedType == SkillFreedType.Big)
			{
				this.CheckPlay(SkillTriggerType.BigSkillAfter, skill);
				return;
			}
			if (skill.skillData.m_freedType == SkillFreedType.Legacy)
			{
				this.CheckPlay(SkillTriggerType.LegacySkillAfter, skill);
			}
		}

		public void OnSkillCountChange(SSkillBase skill)
		{
			Action<SSkillBase> onSkillCountChange = this.m_onSkillCountChange;
			if (onSkillCountChange != null)
			{
				onSkillCountChange(skill);
			}
			switch (skill.skillType)
			{
			case SkillType.Thunder:
				this.CheckPlay(SkillTriggerType.OnceSkillThunder, skill);
				return;
			case SkillType.Icicle:
				this.CheckPlay(SkillTriggerType.OnceSkillIcicle, skill);
				return;
			case SkillType.Knife:
				this.CheckPlay(SkillTriggerType.OnceSkillKnife, skill);
				return;
			case SkillType.Fire:
				this.CheckPlay(SkillTriggerType.OnceSkillFire, skill);
				return;
			case SkillType.Swordkee:
				this.CheckPlay(SkillTriggerType.OnceSkillSwordkee, skill);
				return;
			case SkillType.FallingSword:
				this.CheckPlay(SkillTriggerType.OnceSkillFallingSword, skill);
				return;
			default:
				return;
			}
		}

		public void OnWeaponCritAfter(SSkillBase skill)
		{
			if (skill.skillData.m_freedType == SkillFreedType.Big || skill.skillData.m_freedType == SkillFreedType.Ordinary)
			{
				this.CheckPlay(SkillTriggerType.WeaponCritAfter, skill);
			}
			if (skill.skillData.m_freedType == SkillFreedType.Big || skill.skillData.m_freedType == SkillFreedType.Small)
			{
				this.CheckPlay(SkillTriggerType.SkillCritAfter, skill);
			}
		}

		public void OnKillEnemyAfter(SSkillBase skill)
		{
			this.CheckPlay(SkillTriggerType.KillEnemyAfter, skill);
		}

		public void OnNormalAttackKillAfter(SSkillBase skill)
		{
			if (skill.skillData.m_freedType == SkillFreedType.Ordinary)
			{
				this.CheckPlay(SkillTriggerType.NormalAttackKillAfter, skill);
			}
		}

		public void AddSkills(List<int> skillIds)
		{
			if (skillIds == null)
			{
				return;
			}
			for (int i = 0; i < skillIds.Count; i++)
			{
				this.AddSkill(skillIds[i]);
			}
		}

		public void AddSkill(int skillId)
		{
			GameSkill_skill elementById = this.m_controller.Table.GetGameSkill_skillModelInstance().GetElementById(skillId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("SkillFactory.AddSkill.[GameSkill_skill] is null.skillId = {0}", skillId));
				return;
			}
			GameSkill_skillType elementById2 = this.m_controller.Table.GetGameSkill_skillTypeModelInstance().GetElementById(elementById.typeID);
			if (elementById2 == null)
			{
				HLog.LogError(string.Format("SkillFactory.AddSkill.[GameSkill_skillType] is null.skillId = {0}", skillId));
				return;
			}
			SSkillBase sskillBase = Activator.CreateInstance(Type.GetType(elementById2.sClassName)) as SSkillBase;
			if (sskillBase == null)
			{
				HLog.LogError(string.Format("SkillFactory.AddSkill SSkillBase is null skillId = {0}", skillId));
				return;
			}
			SSkillData sskillData = new SSkillData();
			sskillData.SetMember(this.m_owner);
			sskillData.SetController(this.m_owner.m_controller);
			sskillData.SetTableData(elementById);
			sskillBase.OnInit(this, sskillData);
			SSkillBase sskillBase2 = sskillBase;
			sskillBase2.m_onPlayStart = (Action<SSkillBase>)Delegate.Combine(sskillBase2.m_onPlayStart, new Action<SSkillBase>(this.OnPlayStart));
			SSkillBase sskillBase3 = sskillBase;
			sskillBase3.m_onSkillOneHurtAfter = (Action<SSkillBase, int, int>)Delegate.Combine(sskillBase3.m_onSkillOneHurtAfter, new Action<SSkillBase, int, int>(this.OnSkillOneHurtAfter));
			SSkillBase sskillBase4 = sskillBase;
			sskillBase4.m_onSkillPlayAfter = (Action<SSkillBase>)Delegate.Combine(sskillBase4.m_onSkillPlayAfter, new Action<SSkillBase>(this.OnSkillPlayAfter));
			this.m_skills.Add(sskillBase);
		}

		public void RemoveSkills(List<int> skillIds)
		{
			for (int i = 0; i < skillIds.Count; i++)
			{
				this.RemoveSkill(skillIds[i]);
			}
		}

		public void RemoveSkill(int skillId)
		{
			for (int i = this.m_skills.Count - 1; i >= 0; i--)
			{
				SSkillBase sskillBase = this.m_skills[i];
				if (sskillBase.skillData.m_id == skillId)
				{
					SSkillBase sskillBase2 = sskillBase;
					sskillBase2.m_onPlayStart = (Action<SSkillBase>)Delegate.Remove(sskillBase2.m_onPlayStart, new Action<SSkillBase>(this.OnPlayStart));
					SSkillBase sskillBase3 = sskillBase;
					sskillBase3.m_onSkillOneHurtAfter = (Action<SSkillBase, int, int>)Delegate.Remove(sskillBase3.m_onSkillOneHurtAfter, new Action<SSkillBase, int, int>(this.OnSkillOneHurtAfter));
					SSkillBase sskillBase4 = sskillBase;
					sskillBase4.m_onSkillPlayAfter = (Action<SSkillBase>)Delegate.Remove(sskillBase4.m_onSkillPlayAfter, new Action<SSkillBase>(this.OnSkillPlayAfter));
					sskillBase.OnDeInit();
					this.m_skills.RemoveAt(i);
					return;
				}
			}
		}

		public void PlayCounter(SkillTriggerData triggerData, Action Complete)
		{
			if (!this.m_owner.IsBeControlled)
			{
				List<SSkillBase> list = this.GetSkillByFreedType(SkillFreedType.Ordinary).SortOrderPlaying();
				for (int i = 0; i < list.Count; i++)
				{
					SSkillBase sskillBase = list[i];
					FP fp = this.m_owner.m_controller.Random01();
					FP fp2 = ((triggerData.m_attacker != null) ? triggerData.m_attacker.memberData.attribute.IgnoreRevengeRate : FP._0);
					if (fp <= this.m_owner.memberData.attribute.GetRevengeRate() - fp2 && sskillBase.CheckCounterCanPlay(triggerData))
					{
						sskillBase.PlayCounter();
					}
				}
			}
			if (Complete != null)
			{
				Complete();
			}
		}

		public void Trigger(SkillTriggerType triggerType)
		{
			this.Owner.SetRoundState(MemberRoundState.RoundReady);
			SkillTriggerData skillTriggerData = new SkillTriggerData();
			skillTriggerData.m_triggerType = triggerType;
			List<SSkillBase> list = new List<SSkillBase>();
			for (SSkillBase sskillBase = this.GetTriggerPassiveSkill(list, this.allSkill, skillTriggerData); sskillBase != null; sskillBase = this.GetTriggerPassiveSkill(list, this.allSkill, skillTriggerData))
			{
				if (this.Owner.IsBeControlled)
				{
					list.Add(sskillBase);
					if (sskillBase.skillData.m_freedType == SkillFreedType.Passive)
					{
						sskillBase.Play();
					}
				}
				else if (this.Owner.IsSilence)
				{
					list.Add(sskillBase);
					if (sskillBase.skillData.m_freedType == SkillFreedType.Passive || sskillBase.skillData.m_freedType == SkillFreedType.Ordinary)
					{
						sskillBase.Play();
					}
				}
				else
				{
					if (sskillBase.skillData.m_freedType != SkillFreedType.Big)
					{
						list.Add(sskillBase);
					}
					sskillBase.Play();
				}
			}
			this.Owner.SetRoundState(MemberRoundState.RoundEnd);
		}

		public SSkillBase GetTriggerPassiveSkill(List<SSkillBase> ignoreList, List<SSkillBase> skills, SkillTriggerData triggerData)
		{
			skills = skills.SortOrderPlaying();
			for (int i = 0; i < skills.Count; i++)
			{
				SSkillBase sskillBase = skills[i];
				if (!ignoreList.Contains(sskillBase) && sskillBase.IsCanPlay(triggerData))
				{
					return sskillBase;
				}
			}
			return null;
		}

		public void CheckPlay(SkillTriggerType triggerType, SSkillBase skill)
		{
			SkillTriggerData skillTriggerData = new SkillTriggerData();
			skillTriggerData.m_triggerType = triggerType;
			skillTriggerData.SetTriggerSkill(skill);
			for (int i = 0; i < skill.CurSelectTargetDatas.Count; i++)
			{
				skillTriggerData.m_iHitTargetList.Add(skill.CurSelectTargetDatas[i].m_target);
			}
			this.CheckPlay(skillTriggerData);
		}

		public void CheckPlay(SkillTriggerType triggerType, SBuffBase buff)
		{
			SkillTriggerData skillTriggerData = new SkillTriggerData();
			skillTriggerData.m_triggerType = triggerType;
			skillTriggerData.SetTriggerBuff(buff);
			skillTriggerData.SetTarget(buff.m_attacker);
			this.CheckPlay(skillTriggerData);
		}

		public void CheckPlay(SkillTriggerType triggerType)
		{
			this.CheckPlay(new SkillTriggerData
			{
				m_triggerType = triggerType
			});
		}

		public void CheckPlay(SkillTriggerData triggerData)
		{
			if (this.Owner != null && this.Owner.IsDeath)
			{
				return;
			}
			List<SSkillBase> list = new List<SSkillBase>();
			for (SSkillBase sskillBase = this.GetTriggerSkill(list, this.allSkill, triggerData); sskillBase != null; sskillBase = this.GetTriggerSkill(list, this.allSkill, triggerData))
			{
				if (this.Owner != null && this.Owner.IsDeath)
				{
					return;
				}
				sskillBase.Play();
				list.Add(sskillBase);
			}
		}

		private SSkillBase GetTriggerSkill(List<SSkillBase> ignoreList, List<SSkillBase> skills, SkillTriggerData triggerData)
		{
			skills = skills.SortOrderPlaying();
			for (int i = 0; i < skills.Count; i++)
			{
				SSkillBase sskillBase = skills[i];
				if (!ignoreList.Contains(sskillBase) && sskillBase.IsCanPlay(triggerData))
				{
					return sskillBase;
				}
			}
			return null;
		}

		public void RefreshSkillState()
		{
			for (int i = 0; i < this.m_skills.Count; i++)
			{
				this.m_skills[i].RefreshCD(1);
			}
		}

		private List<SSkillBase> GetSkillByFreedType(SkillFreedType type)
		{
			List<SSkillBase> list = new List<SSkillBase>();
			for (int i = 0; i < this.m_skills.Count; i++)
			{
				SSkillBase sskillBase = this.m_skills[i];
				if (sskillBase.skillData.m_freedType == type)
				{
					list.Add(sskillBase);
				}
			}
			return list;
		}

		private List<SSkillBase> m_skills;

		private SMemberBase m_owner;

		private int m_curComboCount;

		public Action<SSkillBase> m_onPlayStart;

		public Action<SSkillBase> m_onSkillOneHurtAfter;
	}
}
