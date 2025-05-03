using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
	public class BattleServerController : BaseBattleServerController
	{
		protected override void OnInit()
		{
			base.OutData.m_createData = new OutCreateData();
			base.memberFactory.RevivedCount = base.InData.m_revivedCount;
			Dictionary<int, SMemberBase> allMember = base.memberFactory.GetAllMember();
			this.AddCreateData(base.OutData.m_createData, allMember.Values.ToList<SMemberBase>());
		}

		public override void TryCreateNewWave(bool isWin, out bool isBattleEnd)
		{
			if (isWin && this.CurWave < this.MaxWave)
			{
				isBattleEnd = false;
			}
			else
			{
				isBattleEnd = true;
			}
			if (!isBattleEnd)
			{
				this.CurWave++;
				List<CardData> list = base.InData.m_otherWareDatas[this.CurWave - 2];
				List<SMemberBase> list2 = base.memberFactory.CreateWaveMember(list);
				if (list2 != null)
				{
					this.AddCreateData(base.OutData.m_createData, list2);
				}
				ReportTool.AddWaveChangeReport(this, this.CurWave, this.MaxWave);
				base.RefreshCurMembers();
				return;
			}
			this.OnGameEnd(isWin);
		}

		protected override void OnUpdate(float deltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public override void OnGameEnd(bool isWin)
		{
			OutResultData outResultData = new OutResultData();
			Dictionary<int, SMemberBase> allMember = base.memberFactory.GetAllMember();
			List<OutResultMemberData> list = new List<OutResultMemberData>();
			foreach (SMemberBase smemberBase in allMember.Values)
			{
				if (smemberBase != null)
				{
					Action onBattleEnd = smemberBase.OnBattleEnd;
					if (onBattleEnd != null)
					{
						onBattleEnd();
					}
					list.Add(new OutResultMemberData
					{
						m_rowID = smemberBase.memberData.cardData.m_rowID,
						m_memberInstanceID = smemberBase.m_instanceId,
						m_camp = smemberBase.memberData.Camp,
						m_isMainMember = smemberBase.memberData.cardData.m_isMainMember,
						m_curHp = smemberBase.memberData.CurHP,
						m_maxHp = smemberBase.memberData.attribute.GetHpMax(),
						m_attack = smemberBase.memberData.attribute.GetAttack(),
						m_defense = smemberBase.memberData.attribute.GetDefence()
					});
				}
			}
			outResultData.m_members = list;
			outResultData.m_isWin = isWin;
			outResultData.m_revivedCount = base.memberFactory.RevivedCount;
			outResultData.m_MainTotalDamage = base.memberFactory.MainTotalDamage.AsLong();
			outResultData.m_sbMainTotalDamage = base.memberFactory.sbMainTotalDamage;
			base.OutData.m_resultData = outResultData;
			base.OutData.m_endWave = this.CurWave;
			base.OnGameOver(outResultData);
		}

		protected void AddCreateData(OutCreateData createData, List<SMemberBase> members)
		{
			List<OutMemberData> list = new List<OutMemberData>();
			foreach (SMemberBase smemberBase in members)
			{
				if (smemberBase != null)
				{
					OutMemberData outMemberData = new OutMemberData();
					outMemberData.m_waveID = this.CurWave;
					outMemberData.m_rowID = smemberBase.memberData.cardData.m_rowID;
					outMemberData.m_memberID = smemberBase.memberData.cardData.m_memberID;
					outMemberData.m_memberInstanceID = smemberBase.m_instanceId;
					outMemberData.m_isMainMember = smemberBase.memberData.cardData.m_isMainMember;
					outMemberData.m_posIndex = smemberBase.memberData.cardData.m_posIndex;
					outMemberData.m_memberRace = smemberBase.memberData.cardData.m_memberRace;
					outMemberData.m_camp = smemberBase.memberData.cardData.m_camp;
					outMemberData.m_curHp = smemberBase.memberData.CurHP;
					outMemberData.m_maxHp = smemberBase.memberData.attribute.GetHpMax();
					outMemberData.isEnemyPlayer = smemberBase.memberData.cardData.IsEnemyPlayer;
					outMemberData.m_skills = new List<OutSkillData>();
					for (int i = 0; i < smemberBase.skillFactory.allSkill.Count; i++)
					{
						SSkillBase sskillBase = smemberBase.skillFactory.allSkill[i];
						if (sskillBase != null)
						{
							OutSkillData outSkillData = new OutSkillData();
							outSkillData.m_skillID = sskillBase.skillData.m_id;
							outMemberData.m_skills.Add(outSkillData);
						}
					}
					list.Add(outMemberData);
				}
			}
			createData.AddMembers(list);
			this.LogCreateDataMembers(list);
		}

		private void LogCreateDataMembers(List<OutMemberData> outMemberDatas)
		{
			try
			{
				BattleLogHelper.LogCreateDataMembers(outMemberDatas, "[BattleServerController_CreateData]");
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}
	}
}
