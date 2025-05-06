using System;
using System.Collections.Generic;

namespace Server
{
	public class ReportTool
	{
		public static void AddRoundStart(BaseBattleServerController controller, int curRound, int maxRound)
		{
			BattleReportData_RoundStart battleReportData_RoundStart = controller.CreateReport<BattleReportData_RoundStart>();
			battleReportData_RoundStart.SetData(curRound, maxRound);
			controller.AddReport(battleReportData_RoundStart, 0, true);
		}

		public static void AddRoundEnd(BaseBattleServerController controller, int curRound, int maxRound)
		{
			BattleReportData_RoundEnd battleReportData_RoundEnd = controller.CreateReport<BattleReportData_RoundEnd>();
			battleReportData_RoundEnd.SetData(curRound, maxRound);
			controller.AddReport(battleReportData_RoundEnd, 0, true);
		}

		public static void AddHurt(SMemberBase target, SMemberBase attacker, FP hudValue, ChangeHPType changeHpType, SSkillBase skill, SBulletBase bullet, SBuffBase buff, int targetSelectIndex = 1)
		{
			if (target == null)
			{
				return;
			}
			BaseBattleServerController controller = target.m_controller;
			BattleReportData_Hurt battleReportData_Hurt = controller.CreateReport<BattleReportData_Hurt>();
			BattleReportData_HurtOneData battleReportData_HurtOneData = new BattleReportData_HurtOneData();
			battleReportData_HurtOneData.m_hitInstanceID = target.m_instanceId;
			battleReportData_HurtOneData.m_hitMemberId = target.memberData.m_ID;
			if (attacker != null)
			{
				battleReportData_HurtOneData.m_attackerInstanceID = attacker.m_instanceId;
			}
			if (skill != null)
			{
				battleReportData_HurtOneData.m_skillId = skill.skillData.m_id;
				battleReportData_HurtOneData.m_bulletId = bullet.m_bulletData.m_bulletId;
			}
			if (bullet != null)
			{
				battleReportData_HurtOneData.m_fireBulletID = bullet.m_bulletData.m_fireBulletID;
			}
			if (buff != null)
			{
				battleReportData_HurtOneData.m_buffId = buff.m_buffData.m_id;
			}
			ChangeHPData changeHPData = new ChangeHPData
			{
				m_type = changeHpType,
				m_hpUpdate = hudValue
			};
			if ((battleReportData_HurtOneData.IsShowCombo = attacker.memberData.cardData.m_isMainMember) && changeHpType != ChangeHPType.Miss && changeHpType != ChangeHPType.Recover && changeHpType != ChangeHPType.PoisonHurt && changeHpType != ChangeHPType.Vampire)
			{
				battleReportData_HurtOneData.CurComboCount = ++attacker.memberData.CurComboCount;
			}
			battleReportData_HurtOneData.m_maxHp = target.memberData.attribute.GetHpMax();
			battleReportData_HurtOneData.m_curHp = target.memberData.CurHP;
			battleReportData_HurtOneData.m_attack = target.memberData.attribute.GetAttack();
			battleReportData_HurtOneData.m_defense = target.memberData.attribute.GetDefence();
			battleReportData_HurtOneData.m_changeHPData = changeHPData;
			battleReportData_Hurt.AddData(battleReportData_HurtOneData);
			if (bullet == null || !bullet.m_isLastBullet)
			{
				controller.AddReport(battleReportData_Hurt, 0, false);
				return;
			}
			if (targetSelectIndex > 1)
			{
				controller.AddReport(battleReportData_Hurt, bullet.m_bulletData.frame, false);
				return;
			}
			controller.AddReport(battleReportData_Hurt, bullet.m_bulletData.frame, true);
		}

		public static void AddChangeAttribute(SMemberBase target, string key, FP curValue, FP changeValue, string param)
		{
			if (target == null)
			{
				return;
			}
			BaseBattleServerController controller = target.m_controller;
			BattleReportData_ChangeAttribute battleReportData_ChangeAttribute = controller.CreateReport<BattleReportData_ChangeAttribute>();
			battleReportData_ChangeAttribute.SetData(target.m_instanceId);
			battleReportData_ChangeAttribute.AddData(new BattleReportData_AttributeData(key, curValue, changeValue, param));
			controller.AddReport(battleReportData_ChangeAttribute, 0, false);
		}

		public static void AddChangeAttribute(SMemberBase target, List<BattleReportData_AttributeData> attrs)
		{
			if (target == null)
			{
				return;
			}
			BaseBattleServerController controller = target.m_controller;
			BattleReportData_ChangeAttribute battleReportData_ChangeAttribute = controller.CreateReport<BattleReportData_ChangeAttribute>();
			battleReportData_ChangeAttribute.SetData(target.m_instanceId);
			for (int i = 0; i < attrs.Count; i++)
			{
				BattleReportData_AttributeData battleReportData_AttributeData = attrs[i];
				battleReportData_ChangeAttribute.AddData(new BattleReportData_AttributeData(battleReportData_AttributeData.attributeKey, battleReportData_AttributeData.curValue, battleReportData_AttributeData.changeValue, battleReportData_AttributeData.param));
			}
			controller.AddReport(battleReportData_ChangeAttribute, 0, false);
		}

		public static void AddReviveReport(SMemberBase target, FP reviveHp)
		{
			if (target == null)
			{
				return;
			}
			BaseBattleServerController controller = target.m_controller;
			BattleReportData_Revive battleReportData_Revive = controller.CreateReport<BattleReportData_Revive>();
			battleReportData_Revive.SetData(target.m_instanceId, reviveHp);
			controller.AddReport(battleReportData_Revive, 0, false);
		}

		public static void AddWaveChangeReport(BaseBattleServerController controller, int waveId, int maxWaveId)
		{
			BattleReportData_WaveChange battleReportData_WaveChange = new BattleReportData_WaveChange();
			battleReportData_WaveChange.SetData(controller, waveId, maxWaveId);
			controller.AddReport(battleReportData_WaveChange, 30, true);
			controller.AddCurFrame(20);
		}

		public static void AddLegacySkillSummonDisplay(BaseBattleServerController controller, int skillId, int instanceId, int legacyAppearFrame)
		{
			BattleReportData_LegacySkillSummonDisplay battleReportData_LegacySkillSummonDisplay = controller.CreateReport<BattleReportData_LegacySkillSummonDisplay>();
			battleReportData_LegacySkillSummonDisplay.SetData(instanceId, skillId, legacyAppearFrame);
			controller.AddReport(battleReportData_LegacySkillSummonDisplay, 0, false);
			controller.AddCurFrame(battleReportData_LegacySkillSummonDisplay.m_displayFrame);
		}
	}
}
