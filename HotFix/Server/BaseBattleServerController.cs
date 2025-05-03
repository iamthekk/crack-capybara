using System;
using System.Collections.Generic;

namespace Server
{
	public abstract class BaseBattleServerController : BaseServerController<InBattleData, OutBattleData>
	{
		public SMemberFactory memberFactory
		{
			get
			{
				return this.m_memberFactory;
			}
		}

		public bool IsGameOver
		{
			get
			{
				return this.m_isGameOver;
			}
		}

		public bool IsNeedReport
		{
			get
			{
				return !this.m_isGameOver && base.InData.m_isNeedReport;
			}
		}

		public override void Init()
		{
			this.CurWave = 1;
			this.MaxWave = base.InData.m_waveMax;
			this.MaxRound = base.InData.m_durationRound;
			this.m_random = new XRandom(base.InData.m_seed);
			this.m_outData = new OutBattleData();
			this.m_outData.m_battleReport = new BattleReport();
			this.m_outData.m_battleReport.SetEnable(base.InData.m_isNeedReport);
			this.m_memberFactory = new SMemberFactory(this);
			this.m_memberFactory.OnInit();
			base.Init();
		}

		public override void Update(float deltaTime)
		{
		}

		public override void DeInit()
		{
			this.m_memberFactory.OnDeInit();
			base.DeInit();
		}

		public void OnRoundBattle()
		{
			this.CurRoundCount++;
			ReportTool.AddRoundStart(this, this.CurRoundCount, this.MaxRound);
			this.OnMemberRoundStart();
			this.OnMemberRoundPlaying();
			this.OnMemberRoundEnd();
			ReportTool.AddRoundEnd(this, this.CurRoundCount, this.MaxRound);
			if (!this.IsGameOver && this.CurRoundCount >= this.MaxRound)
			{
				this.OnGameEnd(false);
			}
		}

		private void OnMemberRoundPlaying()
		{
			this.RefreshMemberRoundData();
			for (SMemberBase smemberBase = this.FindNextMember(); smemberBase != null; smemberBase = this.FindNextMember())
			{
				smemberBase.OnMemberRoundPlaying();
			}
		}

		private void Trigger(SkillTriggerType triggerType)
		{
			this.RefreshMemberRoundData();
			for (SMemberBase smemberBase = this.FindNextMember(); smemberBase != null; smemberBase = this.FindNextMember())
			{
				smemberBase.skillFactory.Trigger(triggerType);
			}
		}

		private SMemberBase FindNextMember()
		{
			if (this.m_isGameOver)
			{
				return null;
			}
			for (int i = 0; i < this.curMembers.Count; i++)
			{
				SMemberBase smemberBase = this.curMembers[i];
				if (!smemberBase.IsDeath && smemberBase.RoundState == MemberRoundState.RoundReady)
				{
					return smemberBase;
				}
			}
			return null;
		}

		protected void RefreshCurMembers()
		{
			this.curMembers = this.m_memberFactory.GetAllMemberList().SortOrderBattle();
		}

		private void OnMemberBattleStart()
		{
			this.RefreshCurMembers();
			for (int i = 0; i < this.curMembers.Count; i++)
			{
				SMemberBase smemberBase = this.curMembers[i];
				if (!smemberBase.IsDeath)
				{
					smemberBase.BattleStart();
				}
			}
			this.Trigger(SkillTriggerType.BattleStart);
		}

		private void OnMemberRoundStart()
		{
			for (int i = 0; i < this.curMembers.Count; i++)
			{
				SMemberBase smemberBase = this.curMembers[i];
				if (!smemberBase.IsDeath)
				{
					smemberBase.RoundStart();
				}
			}
			this.Trigger(SkillTriggerType.RoundStart);
		}

		private void OnMemberRoundEnd()
		{
			for (int i = 0; i < this.curMembers.Count; i++)
			{
				SMemberBase smemberBase = this.curMembers[i];
				if (!smemberBase.IsDeath)
				{
					smemberBase.RoundEnd();
				}
			}
			this.Trigger(SkillTriggerType.RoundEnd);
		}

		public virtual void OnBattleStart()
		{
			this.CurRoundCount = 0;
			this.OnGameStartReport();
			this.OnMemberBattleStart();
			this.DebugRoundReport();
		}

		public virtual void OnGameStartReport()
		{
			if (this.IsNeedReport)
			{
				BattleReportData_GameStart battleReportData_GameStart = this.CreateReport<BattleReportData_GameStart>();
				foreach (SMemberBase smemberBase in this.m_memberFactory.GetAllMember().Values)
				{
					GameStartMemberData gameStartMemberData = new GameStartMemberData();
					gameStartMemberData.m_instanceId = smemberBase.m_instanceId;
					gameStartMemberData.m_curHp = smemberBase.memberData.CurHP;
					gameStartMemberData.m_maxHp = smemberBase.memberData.attribute.GetHpMax();
					gameStartMemberData.m_curRecharge = smemberBase.memberData.CurRecharge;
					gameStartMemberData.m_maxRecharge = smemberBase.memberData.attribute.RechargeMax;
					gameStartMemberData.m_skillIds.Clear();
					if (smemberBase.memberData.m_skillIDs != null)
					{
						for (int i = 0; i < smemberBase.memberData.m_skillIDs.Count; i++)
						{
							gameStartMemberData.m_skillIds.Add(smemberBase.memberData.m_skillIDs[i]);
						}
					}
					gameStartMemberData.m_curLegacyPower.Clear();
					foreach (KeyValuePair<int, FP> keyValuePair in smemberBase.memberData.CurLegacyPowerDict)
					{
						gameStartMemberData.m_curLegacyPower.Add(keyValuePair.Key, keyValuePair.Value);
					}
					gameStartMemberData.m_maxLegacyPower.Clear();
					foreach (KeyValuePair<int, FP> keyValuePair2 in smemberBase.memberData.MaxLegacyPowerDict)
					{
						gameStartMemberData.m_maxLegacyPower.Add(keyValuePair2.Key, keyValuePair2.Value);
					}
					gameStartMemberData.m_attack = smemberBase.memberData.attribute.GetAttack();
					gameStartMemberData.m_defense = smemberBase.memberData.attribute.GetDefence();
					gameStartMemberData.m_isUsedRevive = smemberBase.memberData.IsReviveUsed;
					battleReportData_GameStart.m_members.Add(gameStartMemberData);
				}
				this.AddReport(battleReportData_GameStart, 0, true);
			}
		}

		public virtual void TryCreateNewWave(bool isWin, out bool isEnd)
		{
			isEnd = true;
		}

		public virtual void OnGameEnd(bool isWin)
		{
		}

		protected void OnGameOver(OutResultData result)
		{
			if (this.IsNeedReport)
			{
				BattleReportData_GameOver battleReportData_GameOver = this.CreateReport<BattleReportData_GameOver>();
				battleReportData_GameOver.m_resultData = result;
				this.AddReport(battleReportData_GameOver, 5, true);
				this.m_isGameOver = true;
			}
		}

		private void RefreshMemberRoundData()
		{
			for (int i = 0; i < this.curMembers.Count; i++)
			{
				this.curMembers[i].RefreshRoundState();
			}
		}

		public void AddReport(BaseBattleReportData data, int addFrame = 0, bool isAddCurFrame = true)
		{
			if (!this.IsNeedReport)
			{
				return;
			}
			if (this.m_outData == null)
			{
				return;
			}
			if (this.m_outData.m_battleReport == null)
			{
				return;
			}
			if (data == null)
			{
				return;
			}
			if (isAddCurFrame)
			{
				data.m_frame = (this.m_curFrame += addFrame);
			}
			else
			{
				data.m_frame = this.m_curFrame + addFrame;
			}
			this.m_outData.m_battleReport.AddReport(data);
		}

		public void AddCurFrame(int addFrame = 0)
		{
			this.m_curFrame += addFrame;
		}

		public void DebugRoundReport()
		{
			this.m_outData.m_battleReport.DebugReportByRound(this.CurRoundCount);
		}

		public T CreateReport<T>() where T : BaseBattleReportData, new()
		{
			T t = new T();
			t.m_round = this.CurRoundCount;
			return t;
		}

		public FP Random01()
		{
			return new FP((long)this.m_random.Range(1, 10000), 10000L);
		}

		public int RandomInt()
		{
			return this.m_random.NextInt();
		}

		public int RandomRange(int min, int max)
		{
			return this.m_random.Range(min, max);
		}

		public bool IsMatchProbability(int probability)
		{
			int num = this.m_random.Range(0, 10000);
			return probability * 100 >= num;
		}

		public bool IsMatchProbability(float probability)
		{
			int num = this.m_random.Range(0, 10000);
			return probability * 100f >= (float)num;
		}

		public bool IsMatchProbability(long probability)
		{
			int num = this.m_random.Range(0, 10000);
			return probability * 100L >= (long)num;
		}

		public void PlayCounter(SSkillBase skill, SMemberBase attacker, List<SkillSelectTargetData> targets, Action complete = null)
		{
			int completeCount = 0;
			if (targets != null)
			{
				Action action = delegate
				{
					int completeCount2 = completeCount;
					completeCount = completeCount2 + 1;
					if (completeCount >= targets.Count)
					{
						Action complete2 = complete;
						if (complete2 == null)
						{
							return;
						}
						complete2();
					}
				};
				for (int i = 0; i < targets.Count; i++)
				{
					SMemberBase target = targets[i].m_target;
					if (target.IsDeath || target.IsBeControlled || attacker.memberData.cardData.IsPet)
					{
						action();
					}
					else
					{
						SkillTriggerData skillTriggerData = new SkillTriggerData();
						skillTriggerData.m_triggerType = SkillTriggerType.RoleCounter;
						skillTriggerData.SetTarget(attacker);
						skillTriggerData.SetTriggerSkill(skill);
						target.PlayCounter(skillTriggerData, action);
					}
				}
				return;
			}
			HLog.LogError("BaseBattleServerController PlayCounter targets is null");
		}

		protected XRandom m_random;

		private SMemberFactory m_memberFactory;

		private bool m_isGameOver;

		private int CurRoundCount;

		private int MaxRound = 99;

		protected int CurWave = 1;

		protected int MaxWave = 1;

		protected List<SMemberBase> curMembers;
	}
}
