using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.Platfrom;
using HotFix.Client;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public abstract class BattleClientControllerBase
	{
		public OutBattleData OutBattleData
		{
			get
			{
				return this.m_outBattleData;
			}
		}

		public bool IsPause
		{
			get
			{
				return this.m_isPause;
			}
		}

		public CMemberFactory memberFactory
		{
			get
			{
				return this.m_memberFactory;
			}
		}

		public Camera BattleCamera
		{
			get
			{
				GameCameraController gameCameraController = this.m_GameCameraController;
				if (gameCameraController == null)
				{
					return null;
				}
				return gameCameraController.MainCamera;
			}
		}

		public void SetData(InBattleData inBattleData, OutBattleData outBattleData)
		{
			this.m_inBattleData = inBattleData;
			this.m_outBattleData = outBattleData;
		}

		public async Task Init()
		{
			this.m_taskManager = new TaskManager();
			this.m_taskManager.OnInit();
			await this.OnInit();
			this.RegisterEvents();
		}

		protected abstract Task OnInit();

		protected abstract Task OnDeInit();

		protected abstract void RegisterEvents();

		protected abstract void UnRegisterEvents();

		protected abstract void OnGameOver(bool isWin);

		public async Task DeInit()
		{
			this.UnRegisterEvents();
			await this.OnDeInit();
		}

		public void Update(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_isPause || this.m_isInCreateWaveMember)
			{
				return;
			}
			if (this.m_isPlayingTask && this.m_taskManager != null)
			{
				this.m_taskManager.OnUpdate(deltaTime);
			}
			GameCameraController gameCameraController = this.m_GameCameraController;
			if (gameCameraController != null)
			{
				gameCameraController.OnUpdate(deltaTime, 0f);
			}
			CMemberFactory memberFactory = this.m_memberFactory;
			if (memberFactory != null)
			{
				memberFactory.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			SceneMapController sceneMapController = this.m_sceneMapController;
			if (sceneMapController != null)
			{
				sceneMapController.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			BattleMemberController battleMemberController = this.m_battleMemberController;
			if (battleMemberController != null)
			{
				battleMemberController.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (!this.m_isAnalysisReport)
			{
				return;
			}
			this.m_curTime += deltaTime;
			int frame = this.GetFrame();
			List<BaseBattleReportData> reportDatas = this.m_outBattleData.m_battleReport.GetReportDatas(this.m_index, frame);
			for (int i = 0; i < reportDatas.Count; i++)
			{
				BaseBattleReportData baseBattleReportData = reportDatas[i];
				if (baseBattleReportData != null)
				{
					switch (baseBattleReportData.m_type)
					{
					case BattleReportType.CreateMember:
						break;
					case BattleReportType.GameStart:
						this.DoReport_GameStart(baseBattleReportData);
						break;
					case BattleReportType.GameOver:
						this.DoReport_GameOver(baseBattleReportData);
						break;
					case BattleReportType.RoundStart:
						this.DoReport_RoundStart(baseBattleReportData);
						break;
					case BattleReportType.RoundEnd:
						this.DoReport_RoundEnd(baseBattleReportData);
						break;
					case BattleReportType.PlaySkill:
						this.DoReport_PlaySkill(baseBattleReportData);
						break;
					case BattleReportType.Hurt:
						this.DoReport_Report_Hurt(baseBattleReportData);
						break;
					case BattleReportType.PlaySkillComplete:
						this.DoReport_PlaySkillComplete(baseBattleReportData);
						break;
					case BattleReportType.CreateBullet:
						this.DoReport_CreateBullet(baseBattleReportData);
						break;
					case BattleReportType.BuffAdd:
						this.DoReport_BuffAdd(baseBattleReportData);
						break;
					case BattleReportType.BuffUpdate:
						this.DoReport_BuffUpdate(baseBattleReportData);
						break;
					case BattleReportType.BuffRemove:
						this.DoReport_BuffRemove(baseBattleReportData);
						break;
					case BattleReportType.ChangeAttributes:
						this.DoReport_ChangeAttribute(baseBattleReportData);
						break;
					case BattleReportType.Move:
						this.DoReport_Move(baseBattleReportData);
						break;
					case BattleReportType.Revive:
						this.DoReport_Revive(baseBattleReportData);
						break;
					case BattleReportType.WaitRoundCount:
						this.DoReport_WaitRoundCount(baseBattleReportData);
						break;
					case BattleReportType.WaveChange:
						this.DoReport_WaveChange(baseBattleReportData);
						break;
					case BattleReportType.TextTips:
						this.DoReport_TextTips(baseBattleReportData);
						break;
					case BattleReportType.LegacySkillSummonDisplay:
						this.DoReport_LegacySkillSummonDisplay(baseBattleReportData);
						break;
					default:
						HLog.LogError(HLog.ToColor("BattleReportType is error.", 3));
						break;
					}
				}
			}
			this.m_index += reportDatas.Count;
			this.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected void BattleReset()
		{
			this.m_curTime = 0f;
			this.m_index = 0;
		}

		protected virtual void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public void OnIsPlayingTask(bool isPlayingTask)
		{
			this.m_isPlayingTask = isPlayingTask;
		}

		public void OnIsPlaying(bool isPlaying)
		{
			this.m_isAnalysisReport = isPlaying;
		}

		public void OnBattleStart()
		{
			TaskManager taskManager = this.m_taskManager;
			if (taskManager != null)
			{
				taskManager.OnPlay();
			}
			if (!this.m_isJump)
			{
				this.BattleReset();
				this.m_isAnalysisReport = true;
			}
		}

		private int GetFrame()
		{
			return (int)(this.m_curTime * 30f);
		}

		public void SetPause(bool isPause = true)
		{
			this.m_isPause = isPause;
			this.m_memberFactory.OnPause(isPause);
		}

		public void Jump()
		{
			this.m_isJump = true;
			if (this.m_outBattleData.m_endWave <= 1)
			{
				this.JumpWave1Battle();
			}
			else
			{
				this.JumpWaveNBattle();
			}
			this.OnGameOver(this.m_outBattleData.m_resultData.m_isWin);
		}

		private void JumpWave1Battle()
		{
			for (int i = 0; i < this.m_outBattleData.m_resultData.m_members.Count; i++)
			{
				OutResultMemberData outResultMemberData = this.m_outBattleData.m_resultData.m_members[i];
				CMemberBase member = this.m_memberFactory.GetMember(outResultMemberData.m_memberInstanceID);
				if (member != null)
				{
					member.m_memberData.SetMaxHp(outResultMemberData.m_maxHp);
					member.m_memberData.SetCurHp(outResultMemberData.m_curHp);
					member.m_memberData.SetAttack(outResultMemberData.m_attack);
					member.m_memberData.SetDefense(outResultMemberData.m_defense);
					if (outResultMemberData.m_curHp <= FP._0)
					{
						member.AI.SwitchState(MemberState.Death);
					}
					else
					{
						member.AI.SwitchState(MemberState.Idle);
					}
					member.RefreshHpHUD();
					member.RefreshPlayerAttrUI();
					member.BattleJump();
				}
			}
		}

		private void JumpWaveNBattle()
		{
		}

		public async Task PreloaderOtherBattleRes(InBattleData inBattleData, int delay)
		{
			if (inBattleData != null && inBattleData.m_otherWareDatas != null)
			{
				HLog.LogError(HLog.ToColor(string.Format("[BattleClientController_Chapter.OnInit1] curFrame:{0}", Time.frameCount), 8));
				await TaskExpand.Delay(delay);
				HLog.LogError(HLog.ToColor(string.Format("[BattleClientController_Chapter.OnInit2] curFrame:{0}", Time.frameCount), 8));
				for (int i = 0; i < inBattleData.m_otherWareDatas.Count; i++)
				{
					await this.LoadArtMemberModel(inBattleData.m_otherWareDatas[i]);
					HLog.LogError(HLog.ToColor(string.Format("[BattleClientController_Chapter.OnInit3] curFrame:{0}", Time.frameCount), 8));
				}
			}
		}

		private async Task LoadArtMemberModel(List<CardData> cardDatas)
		{
			if (cardDatas != null && cardDatas.Count != 0)
			{
				List<Task> list = new List<Task>();
				for (int i = 0; i < cardDatas.Count; i++)
				{
					CardData cardData = cardDatas[i];
					ArtMember_member elementById = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(cardData.m_memberID);
					if (elementById == null)
					{
						HLog.LogError(string.Format("CreateMember .. GameArt_memberModel is Error ..modelID is {0}", cardData.m_memberID));
						return;
					}
					string path = elementById.path;
					list.Add(PoolManager.Instance.CheckPrefab(path));
				}
				await Task.WhenAll(list);
			}
		}

		protected virtual void RegisterEvent()
		{
		}

		protected virtual void UnRegisterEvent()
		{
		}

		private void DoReport_BuffAdd(BaseBattleReportData data)
		{
			BattleReportData_BuffAdd battleReportData_BuffAdd = data as BattleReportData_BuffAdd;
			if (battleReportData_BuffAdd == null)
			{
				return;
			}
			CMemberBase member = this.m_memberFactory.GetMember(battleReportData_BuffAdd.AttackerInstanceID);
			CMemberBase member2 = this.m_memberFactory.GetMember(battleReportData_BuffAdd.TargetInstanceID);
			if (member == null)
			{
				return;
			}
			if (member2 == null)
			{
				return;
			}
			member2.AddBuff(battleReportData_BuffAdd.Guid, battleReportData_BuffAdd.BuffId, member);
			member2.m_memberData.SetAttack(battleReportData_BuffAdd.Attack);
			member2.m_memberData.SetMaxHp(battleReportData_BuffAdd.MaxHp);
			member2.m_memberData.SetDefense(battleReportData_BuffAdd.Defense);
			member2.RefreshPlayerAttrUI();
			EventArgsAddSign eventArgsAddSign = new EventArgsAddSign();
			eventArgsAddSign.SetData(member2.InstanceID, battleReportData_BuffAdd.BuffId, battleReportData_BuffAdd.BuffLayer, battleReportData_BuffAdd.BuffRound);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameView_SignAdd, eventArgsAddSign);
		}

		private void DoReport_BuffUpdate(BaseBattleReportData data)
		{
			BattleReportData_BuffUpdate battleReportData_BuffUpdate = data as BattleReportData_BuffUpdate;
			if (battleReportData_BuffUpdate == null)
			{
				return;
			}
			bool member = this.m_memberFactory.GetMember(battleReportData_BuffUpdate.AttackerInstanceID) != null;
			CMemberBase member2 = this.m_memberFactory.GetMember(battleReportData_BuffUpdate.TargetInstanceID);
			if (!member)
			{
				return;
			}
			if (member2 == null)
			{
				return;
			}
			CBuffFactory buffFactory = member2.buffFactory;
			if (buffFactory != null)
			{
				buffFactory.OnTrigger(battleReportData_BuffUpdate.BuffId, battleReportData_BuffUpdate.BuffLayer);
			}
			EventArgsAddSign eventArgsAddSign = new EventArgsAddSign();
			eventArgsAddSign.SetData(member2.InstanceID, battleReportData_BuffUpdate.BuffId, battleReportData_BuffUpdate.BuffLayer, battleReportData_BuffUpdate.BuffRound);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameView_SignAdd, eventArgsAddSign);
		}

		private void DoReport_BuffRemove(BaseBattleReportData data)
		{
			BattleReportData_BuffRemove battleReportData_BuffRemove = data as BattleReportData_BuffRemove;
			if (battleReportData_BuffRemove == null)
			{
				return;
			}
			CMemberBase member = this.m_memberFactory.GetMember(battleReportData_BuffRemove.AttackerInstanceID);
			CMemberBase member2 = this.m_memberFactory.GetMember(battleReportData_BuffRemove.TargetInstanceID);
			if (member == null)
			{
				return;
			}
			if (member2 == null)
			{
				return;
			}
			member2.RemoveBuff(battleReportData_BuffRemove.Guid, battleReportData_BuffRemove.BuffId, member);
			member2.m_memberData.SetAttack(battleReportData_BuffRemove.Attack);
			member2.m_memberData.SetMaxHp(battleReportData_BuffRemove.MaxHp);
			member2.m_memberData.SetDefense(battleReportData_BuffRemove.Defense);
			member2.RefreshPlayerAttrUI();
			EventArgsRemoveSign eventArgsRemoveSign = new EventArgsRemoveSign();
			eventArgsRemoveSign.SetData(member2.InstanceID, battleReportData_BuffRemove.BuffId);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameView_SignRemove, eventArgsRemoveSign);
		}

		private void DoReport_ChangeAttribute(BaseBattleReportData data)
		{
			BattleReportData_ChangeAttribute battleReportData_ChangeAttribute = data as BattleReportData_ChangeAttribute;
			if (battleReportData_ChangeAttribute == null)
			{
				return;
			}
			CMemberBase member = this.m_memberFactory.GetMember(battleReportData_ChangeAttribute.TargetInstanceID);
			if (member == null)
			{
				return;
			}
			if (battleReportData_ChangeAttribute.Attributes != null)
			{
				bool flag = false;
				foreach (KeyValuePair<string, BattleReportData_AttributeData> keyValuePair in battleReportData_ChangeAttribute.Attributes)
				{
					string key = keyValuePair.Key;
					uint num = <PrivateImplementationDetails>.ComputeStringHash(key);
					if (num <= 1738070863U)
					{
						if (num != 403056577U)
						{
							if (num != 484614851U)
							{
								if (num == 1738070863U)
								{
									if (key == "LegacyPower")
									{
										int num2;
										if (int.TryParse(keyValuePair.Value.param, out num2))
										{
											member.m_memberData.SetCurLegacyPower(num2, keyValuePair.Value.curValue);
											member.RefreshLegacyPowerHUD(num2);
										}
									}
								}
							}
							else if (key == "Defence")
							{
								flag = true;
								member.m_memberData.SetDefense(keyValuePair.Value.curValue);
							}
						}
						else if (key == "ShieldDurian")
						{
							member.m_memberData.SetCurShield(keyValuePair.Value.curValue, false, true);
						}
					}
					else if (num <= 2111287722U)
					{
						if (num != 2039097040U)
						{
							if (num == 2111287722U)
							{
								if (key == "ShieldThunder")
								{
									member.m_memberData.SetCurShield(keyValuePair.Value.curValue, true, false);
								}
							}
						}
						else if (key == "Shield")
						{
							member.m_memberData.SetCurShield(keyValuePair.Value.curValue, false, false);
						}
					}
					else if (num != 2343121693U)
					{
						if (num == 4103137980U)
						{
							if (key == "Recharge")
							{
								member.m_memberData.SetCurRecharge(keyValuePair.Value.curValue);
								member.RefreshRechargeHUD();
							}
						}
					}
					else if (key == "Attack")
					{
						flag = true;
						member.m_memberData.SetAttack(keyValuePair.Value.curValue);
					}
				}
				if (flag)
				{
					member.RefreshPlayerAttrUI();
				}
			}
			BattleLogHelper.LogReport_ChangeAttribute(battleReportData_ChangeAttribute);
		}

		private void DoReport_CreateBullet(BaseBattleReportData data)
		{
			BattleReportData_CreateBullet battleReportData_CreateBullet = data as BattleReportData_CreateBullet;
			BattleLogHelper.LogReport_CreateBullet(battleReportData_CreateBullet);
			GameSkill_fireBullet elementById = GameApp.Table.GetManager().GetGameSkill_fireBulletModelInstance().GetElementById(battleReportData_CreateBullet.FireBulletID);
			if (elementById == null)
			{
				HLog.LogError(string.Format("GameSkill_fireBullet is Error. FireBulletID = {0}", battleReportData_CreateBullet.FireBulletID));
			}
			CMemberBase member = this.m_memberFactory.GetMember(battleReportData_CreateBullet.AttackerInstanceID);
			if (member == null)
			{
				HLog.LogError(string.Format("Attacker is Error. AttackerInstanceID = {0}", battleReportData_CreateBullet.AttackerInstanceID));
				return;
			}
			CMemberBase member2 = this.m_memberFactory.GetMember(battleReportData_CreateBullet.TargetInstanceID);
			if (member2 == null)
			{
				HLog.LogError(string.Format("Target is Error. TargetInstanceID = {0}", battleReportData_CreateBullet.TargetInstanceID));
				return;
			}
			CFireBulletData cfireBulletData = new CFireBulletData();
			cfireBulletData.SetBulletID(battleReportData_CreateBullet.BulletID);
			Transform transform = member.m_body.GetTransform(elementById.bulletStartPosID);
			Transform transform2 = member2.m_body.GetTransform(elementById.bulletEndPosID);
			cfireBulletData.SetPosPoint(transform, transform2);
			cfireBulletData.SetSoundData(elementById.bulletStartSoundID, elementById.bulletHitSoundID);
			cfireBulletData.IsMainTarget = battleReportData_CreateBullet.IsMainTarget;
			cfireBulletData.IsShowBullet = elementById.showTpye.Equals(0);
			member.FireBullet(cfireBulletData);
		}

		private void DoReport_GameOver(BaseBattleReportData data)
		{
			BattleReportData_GameOver battleReportData_GameOver = data as BattleReportData_GameOver;
			this.m_isAnalysisReport = false;
			if (this.m_memberFactory != null)
			{
				this.m_memberFactory.OnGameOver(battleReportData_GameOver);
			}
			GameTGATools.Ins.StageClickTempBattleResult = (battleReportData_GameOver.m_resultData.m_isWin ? 1 : 0);
			GameTGATools.Ins.StageClickTempBattleRound = battleReportData_GameOver.m_round;
			this.OnGameOver(battleReportData_GameOver.m_resultData.m_isWin);
		}

		private void DoReport_GameStart(BaseBattleReportData data)
		{
			BattleReportData_GameStart battleReportData_GameStart = data as BattleReportData_GameStart;
			BattleLogHelper.LogReport_GameStart(battleReportData_GameStart);
			if (this.m_memberFactory != null)
			{
				this.m_memberFactory.OnGameStart(battleReportData_GameStart);
				this.m_memberFactory.RefreshBattleAllHp();
			}
			EventArgsBattleStart eventArgsBattleStart = new EventArgsBattleStart();
			eventArgsBattleStart.SetData(battleReportData_GameStart);
			GameApp.Event.DispatchNow(this, 299, eventArgsBattleStart);
		}

		protected virtual void DoReport_Report_Hurt(BaseBattleReportData data)
		{
			BattleReportData_Hurt battleReportData_Hurt = data as BattleReportData_Hurt;
			BattleLogHelper.LogReport_Hurt(battleReportData_Hurt);
			List<BattleReportData_HurtOneData> list = battleReportData_Hurt.GetList();
			if (this.m_memberFactory != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					BattleReportData_HurtOneData battleReportData_HurtOneData = list[i];
					CMemberBase member = this.m_memberFactory.GetMember(battleReportData_HurtOneData.m_hitInstanceID);
					if (member != null)
					{
						member.Hurt(battleReportData_HurtOneData);
					}
				}
				this.m_memberFactory.RefreshBattleAllHp();
			}
		}

		[return: TupleElementNames(new string[] { "damage", "hp" })]
		protected ValueTuple<FP, FP> GetHurtReportData(BaseBattleReportData data)
		{
			FP fp = FP._0;
			FP fp2 = -FP._1;
			BattleReportData_Hurt battleReportData_Hurt = data as BattleReportData_Hurt;
			if (battleReportData_Hurt != null)
			{
				List<BattleReportData_HurtOneData> list = battleReportData_Hurt.GetList();
				if (this.m_memberFactory != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						BattleReportData_HurtOneData battleReportData_HurtOneData = list[i];
						CMemberBase member = this.m_memberFactory.GetMember(battleReportData_HurtOneData.m_attackerInstanceID);
						if (member != null && member.m_memberData.Camp == MemberCamp.Friendly && battleReportData_HurtOneData.m_changeHPData.m_type != ChangeHPType.Miss && battleReportData_HurtOneData.m_changeHPData.m_type != ChangeHPType.Recover && battleReportData_HurtOneData.m_changeHPData.m_type != ChangeHPType.Vampire)
						{
							fp += battleReportData_HurtOneData.m_changeHPData.m_hpUpdate;
						}
					}
					if (list.Count > 0)
					{
						List<BattleReportData_HurtOneData> list2 = list;
						fp2 = list2[list2.Count - 1].m_curHp;
					}
				}
			}
			return new ValueTuple<FP, FP>(fp, fp2);
		}

		private void DoReport_Revive(BaseBattleReportData data)
		{
			BattleReportData_Revive battleReportData_Revive = data as BattleReportData_Revive;
			if (this.m_memberFactory != null)
			{
				CMemberBase member = this.m_memberFactory.GetMember(battleReportData_Revive.TargetInstanceID);
				if (member != null)
				{
					member.m_memberData.SetIsUsedRevive(true);
					member.RefreshPlayerAttrUI();
					member.InsertPlayHitEffect(new TaskOutValue<HitBase>(), 3002, 0, PointRotationDirection.Target, HitTargetType.Owner, null, null);
					member.ShowTextHUD(EHoverTextType.Revive);
				}
			}
		}

		protected virtual void DoReport_LegacySkillSummonDisplay(BaseBattleReportData data)
		{
			BattleReportData_LegacySkillSummonDisplay battleReportData_LegacySkillSummonDisplay = data as BattleReportData_LegacySkillSummonDisplay;
			BattleLogHelper.LogReport_LegacySkillSummonDisplay(battleReportData_LegacySkillSummonDisplay);
			if (this.m_memberFactory != null)
			{
				CMemberBase member = this.m_memberFactory.GetMember(battleReportData_LegacySkillSummonDisplay.m_memberInstanceID);
				if (member != null)
				{
					member.LegacySkillSummonDisplay(battleReportData_LegacySkillSummonDisplay);
				}
			}
		}

		private void DoReport_Move(BaseBattleReportData data)
		{
			BattleReportData_Move battleReportData_Move = data as BattleReportData_Move;
			BattleLogHelper.LogReport_Move(battleReportData_Move);
			if (this.m_memberFactory != null)
			{
				CMemberBase member = this.m_memberFactory.GetMember(battleReportData_Move.m_memberInstanceID);
				if (member != null)
				{
					member.Move(battleReportData_Move);
				}
			}
		}

		private void DoReport_PlaySkill(BaseBattleReportData data)
		{
			BattleReportData_PlaySkill battleReportData_PlaySkill = data as BattleReportData_PlaySkill;
			BattleLogHelper.LogReport_PlaySkill(battleReportData_PlaySkill);
			if (this.m_memberFactory != null)
			{
				CMemberBase member = this.m_memberFactory.GetMember(battleReportData_PlaySkill.m_memberInstanceID);
				if (member != null)
				{
					member.PlaySkill(battleReportData_PlaySkill);
				}
			}
		}

		private void DoReport_PlaySkillComplete(BaseBattleReportData data)
		{
			BattleReportData_PlaySkillComplete battleReportData_PlaySkillComplete = data as BattleReportData_PlaySkillComplete;
			BattleLogHelper.LogReport_PlaySkillComplete(battleReportData_PlaySkillComplete);
			if (this.m_memberFactory != null)
			{
				CMemberBase member = this.m_memberFactory.GetMember(battleReportData_PlaySkillComplete.m_memberInstanceID);
				if (member != null)
				{
					member.skillFactory.PlaySkillComplete(battleReportData_PlaySkillComplete);
				}
			}
		}

		private void DoReport_RoundStart(BaseBattleReportData data)
		{
			BattleReportData_RoundStart battleReportData_RoundStart = data as BattleReportData_RoundStart;
			if (battleReportData_RoundStart == null)
			{
				return;
			}
			BattleLogHelper.LogReport_RoundStart(battleReportData_RoundStart);
			EventArgsRoundStart instance = Singleton<EventArgsRoundStart>.Instance;
			instance.SetData(battleReportData_RoundStart.CurRound, battleReportData_RoundStart.MaxRound);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameView_RoundStart, instance);
		}

		private void DoReport_RoundEnd(BaseBattleReportData data)
		{
			BattleReportData_RoundEnd battleReportData_RoundEnd = data as BattleReportData_RoundEnd;
			if (battleReportData_RoundEnd == null)
			{
				return;
			}
			BattleLogHelper.LogReport_RoundEnd(battleReportData_RoundEnd);
		}

		private async void DoReport_WaveChange(BaseBattleReportData data)
		{
			this.m_isInCreateWaveMember = true;
			BattleReportData_WaveChange reportData = data as BattleReportData_WaveChange;
			if (reportData != null)
			{
				BattleLogHelper.LogReport_WaveChange(reportData);
				List<CardData> cardList = this.m_inBattleData.GetWaveData(reportData.CurWave);
				if (this.m_battleMemberController != null)
				{
					TaskOutValue<List<CMemberBase>> outMembers = new TaskOutValue<List<CMemberBase>>();
					await this.m_battleMemberController.WaveChange(reportData.CurWave, cardList, this.m_pointController, this.m_memberFactory, outMembers);
					if (outMembers.Value != null)
					{
						for (int i = 0; i < outMembers.Value.Count; i++)
						{
							CMemberBase cmemberBase = outMembers.Value[i];
							BattleReportData_WaveChange.WaveChangeMemberData waveChangeMemberData;
							if (reportData.m_members.TryGetValue(cmemberBase.InstanceID, out waveChangeMemberData))
							{
								this.RefreshMemberData(cmemberBase, waveChangeMemberData);
							}
						}
					}
					this.m_isInCreateWaveMember = false;
					int num = 20;
					this.m_battleMemberController.WaveEnter(num);
					outMembers = null;
				}
				else if (this.m_memberFactory != null)
				{
					List<Task> list = new List<Task>();
					for (int j = 0; j < cardList.Count; j++)
					{
						list.Add(this.CreateMember(reportData, cardList[j]));
					}
					await Task.WhenAll(list);
					this.m_isInCreateWaveMember = false;
					int num2 = 20;
					for (int k = 0; k < cardList.Count; k++)
					{
						CardData cardData = cardList[k];
						this.m_memberFactory.GetMember(cardData.m_instanceID).MoveEnter(num2);
					}
				}
				else
				{
					this.m_isInCreateWaveMember = false;
				}
			}
		}

		private async Task CreateMember(BattleReportData_WaveChange report, CardData cardData)
		{
			GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(cardData.m_memberID);
			if (elementById != null)
			{
				CMemberData cMemberData = new CMemberData(cardData);
				cMemberData.SetTableData(elementById);
				ClientPointData pointByIndex = this.m_pointController.GetPointByIndex(cardData.m_camp, cardData.m_posIndex);
				Vector3 position = pointByIndex.GetPosition();
				await this.m_memberFactory.CreateMember(position, cMemberData, pointByIndex, null);
				BattleReportData_WaveChange.WaveChangeMemberData waveChangeMemberData;
				if (report.m_members.TryGetValue(cMemberData.InstanceID, out waveChangeMemberData))
				{
					CMemberBase member = this.m_memberFactory.GetMember(cMemberData.InstanceID);
					this.RefreshMemberData(member, waveChangeMemberData);
					member.InitStartPosition();
				}
			}
		}

		private void RefreshMemberData(CMemberBase member, BattleReportData_WaveChange.WaveChangeMemberData waveChangeMemberData)
		{
			member.m_memberData.SetMaxHp(waveChangeMemberData.m_maxHp);
			member.m_memberData.SetCurHp(waveChangeMemberData.m_curHp);
			member.m_memberData.SetAttack(waveChangeMemberData.m_attack);
			member.m_memberData.SetDefense(waveChangeMemberData.m_defense);
			member.m_memberData.SetMaxRecharge(waveChangeMemberData.m_maxRecharge);
			member.m_memberData.SetCurRecharge(waveChangeMemberData.m_curRecharge);
			foreach (KeyValuePair<int, FP> keyValuePair in waveChangeMemberData.m_maxLegacyPower)
			{
				member.m_memberData.SetMaxLegacyPower(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (KeyValuePair<int, FP> keyValuePair2 in waveChangeMemberData.m_curLegacyPower)
			{
				member.m_memberData.SetCurLegacyPower(keyValuePair2.Key, keyValuePair2.Value);
			}
			member.m_memberData.SetCurShield(FP._0, false, false);
			member.m_memberData.SetIsUsedRevive(waveChangeMemberData.m_isUsedRevive);
			member.RefreshHpHUD();
			member.RefreshShieldHUD();
			member.RefreshRechargeHUD();
			member.RefreshLegacyPowerHUD(0);
			member.ShowHpHUD(true);
			member.RefreshPlayerAttrUI();
		}

		private void DoReport_TextTips(BaseBattleReportData data)
		{
			BattleReportData_TextTips battleReportData_TextTips = data as BattleReportData_TextTips;
			if (this.m_memberFactory != null)
			{
				CMemberBase member = this.m_memberFactory.GetMember(battleReportData_TextTips.TargetInstanceID);
				if (member != null)
				{
					member.ShowTextHUD(battleReportData_TextTips.TextID);
				}
			}
		}

		private void DoReport_WaitRoundCount(BaseBattleReportData data)
		{
			BattleReportData_WaitRoundCount battleReportData_WaitRoundCount = data as BattleReportData_WaitRoundCount;
			if (this.m_memberFactory != null)
			{
				CMemberBase member = this.m_memberFactory.GetMember(battleReportData_WaitRoundCount.TargetInstanceID);
				if (member != null)
				{
					member.ShowWaitRoundCount(battleReportData_WaitRoundCount.WaitRoundCount);
				}
			}
		}

		public int curWave = 1;

		protected InBattleData m_inBattleData;

		protected OutBattleData m_outBattleData;

		protected bool m_isPause;

		protected bool m_isInCreateWaveMember;

		protected SceneMapController m_sceneMapController;

		protected GameCameraController m_GameCameraController;

		protected ClientPointController m_pointController;

		protected BattleMemberController m_battleMemberController;

		protected CMemberFactory m_memberFactory;

		private TaskManager m_taskManager;

		protected bool m_isPlayingTask = true;

		protected bool m_isAnalysisReport;

		protected bool m_isJump;

		protected float m_curTime;

		protected int m_index;
	}
}
