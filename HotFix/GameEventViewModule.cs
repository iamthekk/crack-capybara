using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using HotFix.Client;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class GameEventViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.chapterModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			this.isInBattle = false;
			this.isShowColor = false;
			this.isFlyAni = false;
			this.isGameOver = false;
			this.isMoveToNpc = false;
			this.befourBattleAtk = 0L;
			this.befourBattleDef = 0L;
			this.btnSpeedTrans = this.buttonSpeed.GetComponent<RectTransform>();
			if (this.chapterTaskController != null)
			{
				this.chapterTaskController.gameObject.SetActiveSafe(false);
			}
		}

		public override void OnOpen(object data)
		{
			this.SetFlyEndNode();
			this.buttonPause.onClick.AddListener(new UnityAction(this.OnClickPause));
			this.buttonSpeed.SetData(UISpeedButtonCtrl.SpeedType.Main);
			this.chapterStateController.Init();
			this.chapterNameController.Init();
			this.attributeController.Init();
			this.eventController.Init();
			this.weatherController.Init();
			this.sickController.Init();
			this.fishingController.Init();
			this.eventItemController.Init();
			this.progressCtrl.Init();
			this.playerCoinCtrl.Init();
			this.chapterBattlePassCtrl.SetChapterId(this.chapterModule.ChapterID, false);
			this.chapterBattlePassCtrl.Init();
			this.chapterBattlePassCtrl.PlayAni(false);
			this.rankEnterCtrl.Init();
			this.rankEnterCtrl.Hide();
			ChapterActivityData activeActivityData = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule).GetActiveActivityData(ChapterActivityKind.Rank);
			if (activeActivityData != null)
			{
				this.rankEnterCtrl.SetData(activeActivityData, null, true);
				this.rankEnterCtrl.Hide();
			}
			this.wheelEnterCtrl.Init();
			this.wheelEnterCtrl.SetData(null, true);
			this.wheelEnterCtrl.Hide();
			this.saveStageList.Clear();
			this.saveStageList.AddRange(this.chapterModule.CurrentChapter.Config.rewardStage.ToList<int>());
			this.skillIconItemObj.SetActiveSafe(false);
		}

		public override void OnClose()
		{
			this.isChapterPass = false;
			this.saveStageList.Clear();
			this.buttonPause.onClick.RemoveListener(new UnityAction(this.OnClickPause));
			this.chapterStateController.DeInit();
			this.chapterNameController.DeInit();
			this.attributeController.DeInit();
			this.eventController.DeInit();
			this.weatherController.DeInit();
			this.sickController.DeInit();
			this.fishingController.DeInit();
			this.eventItemController.DeInit();
			this.progressCtrl.DeInit();
			this.playerCoinCtrl.DeInit();
			this.chapterBattlePassCtrl.DeInit();
			this.rankEnterCtrl.DeInit();
			this.wheelEnterCtrl.DeInit();
		}

		public override void OnDelete()
		{
			this.skillIconItemList.Clear();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.attributeController.OnUpdate(deltaTime, unscaledDeltaTime);
			this.eventController.OnUpdate(deltaTime, unscaledDeltaTime);
			this.playerCoinCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			this.chapterBattlePassCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			this.rankEnterCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			this.wheelEnterCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		private void EventStart()
		{
			this.RefreshInfo(false);
			this.RefreshExpInfo();
			this.RefreshStageName(true, this.chapterModule.CurrentChapter.Config.mapId);
			this.RefreshTask();
			this.eventController.EventStart();
			GameEventPoolDataFactory eventPoolDataFactory = Singleton<GameEventController>.Instance.GetEventPoolDataFactory();
			this.progressCtrl.SetData(eventPoolDataFactory.GetProgressDataList());
			this.chapterBattlePassCtrl.SetData(true);
			this.RefreshStage();
			this.ShowEvent();
		}

		private void FinishSave(int result)
		{
			if (this.isChapterPass)
			{
				this.endStage = Singleton<GameEventController>.Instance.GetCurrentStage();
			}
			else
			{
				this.endStage = Singleton<GameEventController>.Instance.GetCurrentStage() - 1;
			}
			this.endStage = ((this.endStage < 0) ? 0 : this.endStage);
			this.chapterModule.FinishEvent(this.endStage, result, this.isChapterPass);
			this.isChapterPass = false;
		}

		private void RefreshInfo(bool useAni = true)
		{
			BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
			this.attributeController.SetHP(playerData.CurrentHp.GetValue(), playerData.HpMax.GetValue(), useAni);
			this.attributeController.SetAttack(playerData.Attack.GetValue());
			this.attributeController.SetDefence(playerData.Defence.GetValue());
			int num = (int)(playerData.CurrentHp.AsDouble() / playerData.HpMax.AsDouble() * 100.0);
			if (playerData.CurrentHp.GetValue() <= 0L)
			{
				num = 0;
			}
			else
			{
				num = Mathf.Clamp(num, 1, 100);
			}
			this.attributeController.SetHpPercent((long)num);
			if (this.isShowColor)
			{
				if (playerData.Attack.GetValue() > this.befourBattleAtk)
				{
					this.attributeController.SetAttackColor(UIAttributeController.AttributeColorType.Green);
				}
				else if (playerData.Attack.GetValue() < this.befourBattleAtk)
				{
					this.attributeController.SetAttackColor(UIAttributeController.AttributeColorType.Red);
				}
				else
				{
					this.attributeController.SetAttackColor(UIAttributeController.AttributeColorType.Normal);
				}
				if (playerData.Defence.GetValue() > this.befourBattleDef)
				{
					this.attributeController.SetDefenceColor(UIAttributeController.AttributeColorType.Green);
				}
				else if (playerData.Defence.GetValue() < this.befourBattleDef)
				{
					this.attributeController.SetDefenceColor(UIAttributeController.AttributeColorType.Red);
				}
				else
				{
					this.attributeController.SetDefenceColor(UIAttributeController.AttributeColorType.Normal);
				}
			}
			else
			{
				this.attributeController.SetAttackColor(UIAttributeController.AttributeColorType.Normal);
				this.attributeController.SetDefenceColor(UIAttributeController.AttributeColorType.Normal);
			}
			this.attributeController.SetGold(playerData.Food);
			this.eventController.SetNextDayButtonInfo(playerData.Food, playerData.BeginFood, playerData.CurrentHp.GetValue(), playerData.HpMax.GetValue());
		}

		private void RefreshExpInfo()
		{
			this.attributeController.SetExp(Singleton<GameEventController>.Instance.PlayerData.Exp.mVariable, Singleton<GameEventController>.Instance.PlayerData.NextExp, Singleton<GameEventController>.Instance.PlayerData.ExpLevel.mVariable);
		}

		private void RefreshStageName(bool isAni, int mapId)
		{
			Map_map elementById = GameApp.Table.GetManager().GetMap_mapModelInstance().GetElementById(mapId);
			this.RefreshStage();
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameId);
			this.chapterStateController.SetChapterName(infoByID);
			this.chapterNameController.SetName(infoByID);
			if (isAni)
			{
				this.chapterNameController.Show(true, delegate
				{
					if (!this.isInBattle)
					{
						this.chapterStateController.Show(false, true);
					}
				});
			}
		}

		private void RefreshStage()
		{
			ChapterData currentChapter = this.chapterModule.CurrentChapter;
			this.chapterStateController.SetDay(Singleton<GameEventController>.Instance.GetCurrentStage(), currentChapter.TotalStage);
			this.attributeController.SetDay(Singleton<GameEventController>.Instance.GetCurrentStage());
			this.progressCtrl.SetDay(Singleton<GameEventController>.Instance.GetCurrentStage());
		}

		private void RefreshTask()
		{
		}

		private void PushEvent(GameEventPushType pushType, object param = null)
		{
			Singleton<GameEventController>.Instance.PushEvent(pushType, param);
		}

		private void RefreshBattle()
		{
			List<GameEventSkillBuildData> playerSkillBuildList = Singleton<GameEventController>.Instance.PlayerData.GetPlayerSkillBuildList();
			for (int i = 0; i < playerSkillBuildList.Count; i++)
			{
				UIGameEventSkillIconItem uigameEventSkillIconItem;
				if (i < this.skillIconItemList.Count)
				{
					uigameEventSkillIconItem = this.skillIconItemList[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.skillIconItemObj);
					gameObject.transform.SetParentNormal(this.skillLayout.transform, false);
					uigameEventSkillIconItem = gameObject.GetComponent<UIGameEventSkillIconItem>();
					uigameEventSkillIconItem.Init();
					this.skillIconItemList.Add(uigameEventSkillIconItem);
				}
				uigameEventSkillIconItem.gameObject.SetActiveSafe(true);
				uigameEventSkillIconItem.Refresh(playerSkillBuildList[i]);
			}
		}

		private void ShowEvent()
		{
			this.eventsObj.SetActiveSafe(true);
			this.specialStateObj.SetActiveSafe(true);
			this.chapterStateController.Show(false, true);
			this.eventItemController.SetShow(true);
			this.progressCtrl.SetShow(true);
			this.textRound.text = "";
			this.ShowBattleAni(false);
		}

		private void ShowBattle()
		{
			this.eventsObj.SetActiveSafe(true);
			this.specialStateObj.SetActiveSafe(false);
			this.chapterStateController.Show(false, true);
			this.eventItemController.SetShow(false);
			this.progressCtrl.SetShow(false);
			this.ShowBattleAni(true);
		}

		private void AddEvent(GameEventUIData uiData)
		{
			this.eventController.AddEvent(uiData);
			this.RefreshStage();
		}

		private void EnterBattle()
		{
			this.befourBattleAtk = Singleton<GameEventController>.Instance.PlayerData.Attack.GetValue();
			this.befourBattleDef = Singleton<GameEventController>.Instance.PlayerData.Defence.GetValue();
			this.isInBattle = true;
			this.isShowColor = true;
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.eventsObj.transform, this.EventY, 0.7f, false), 27), delegate
			{
				Vector3 localPosition = this.eventsObj.transform.localPosition;
				this.eventsObj.transform.localPosition = new Vector3(localPosition.x, this.EventY, localPosition.z);
			});
		}

		private void BattleStart()
		{
			this.ShowBattle();
		}

		private void EndBattle(GameOverType gameOverType)
		{
			this.isShowColor = false;
			if (gameOverType != GameOverType.Win)
			{
				GameApp.SDK.Analyze.Track_StagetClickTest(null);
				this.isChapterPass = false;
				this.OpenResult(-1);
				return;
			}
			EventArgFlyDrop instance = Singleton<EventArgFlyDrop>.Instance;
			instance.SetData(Vector3.zero);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_FlyDrop, instance);
			GameApp.Sound.PlayClip(52, 1f);
			int currentStage = Singleton<GameEventController>.Instance.GetCurrentStage();
			int chapterTotalStage = this.GetChapterTotalStage();
			if (currentStage >= chapterTotalStage)
			{
				this.isChapterPass = true;
				GameApp.SDK.Analyze.Track_StagetClickTest(null);
				DelayCall.Instance.CallOnce(1000, delegate
				{
					this.OpenResult(1);
				});
				return;
			}
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.eventsObj.transform, 0f, 0.7f, false), 27), delegate
			{
				this.ShowEvent();
				Vector3 localPosition = this.eventsObj.transform.localPosition;
				this.eventsObj.transform.localPosition = new Vector3(localPosition.x, 0f, localPosition.z);
				this.PushEvent(GameEventPushType.EndBattle, null);
				DelayCall.Instance.CallOnce(300, delegate
				{
					this.isInBattle = false;
				});
			});
		}

		private void OnClickPause()
		{
			if (this.isChapterPass)
			{
				return;
			}
			if (this.attributeController.IsLevelUpAni)
			{
				return;
			}
			EventArgsBool instance = Singleton<EventArgsBool>.Instance;
			instance.SetData(true);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_Pause, instance);
			GameApp.View.OpenView(ViewName.BattlePauseViewModule, null, 1, null, null);
		}

		public bool IsClickEnabled()
		{
			return !this.isInBattle && !this.attributeController.IsLevelUpAni && !this.isShowSpecialUI && !this.isEventPuase && !this.isFlyAni && !this.isMoveToNpc && !this.isPlaySpecialAni && !this.fishingController.isFishing && Singleton<GameEventController>.Instance.IsCurrentEventDone();
		}

		public void OnClickScroll()
		{
			this.PushEvent(GameEventPushType.ClickScroll, null);
		}

		private void OnSkillAnimation(object sender, int type, BaseEventArgs args)
		{
			int value = (args as EventArgsInt).Value;
			for (int i = 0; i < this.skillIconItemList.Count; i++)
			{
				UIGameEventSkillIconItem uigameEventSkillIconItem = this.skillIconItemList[i];
				if (uigameEventSkillIconItem.ID.Equals(value))
				{
					uigameEventSkillIconItem.PlayAnimation();
					return;
				}
			}
		}

		private void OnShowLevelUp(object sender, int type, BaseEventArgs args)
		{
			this.isSelectSkill = true;
			if (args != null)
			{
				EventArgLevelupHp eventArgLevelupHp = args as EventArgLevelupHp;
				if (eventArgLevelupHp != null)
				{
					int level = eventArgLevelupHp.level;
					GameEventUIData gameEventUIData = GameEventUIDataCreator.Create(0, Singleton<GameEventController>.Instance.GetCurrentStage(), false, "UIGameEvent_38", new object[] { level }, null, null, null, null, null);
					this.AddEvent(gameEventUIData);
				}
				if (eventArgLevelupHp != null && eventArgLevelupHp.paramList != null && eventArgLevelupHp.paramList.Count > 0)
				{
					GameEventUIData gameEventUIData2 = GameEventUIDataCreator.Create(0, Singleton<GameEventController>.Instance.GetCurrentStage(), false, "UIGameEvent_71", null, eventArgLevelupHp.paramList, null, null, null, null);
					this.AddEvent(gameEventUIData2);
				}
			}
		}

		private void ShowSelectSkillEventUI()
		{
			List<GameEventSkillBuildData> lastLearnSkill = Singleton<GameEventController>.Instance.GetLastLearnSkill();
			if (lastLearnSkill == null || lastLearnSkill.Count == 0)
			{
				return;
			}
			for (int i = 0; i < lastLearnSkill.Count; i++)
			{
				GameEventUIData gameEventUIData = GameEventUIDataCreator.Create(0, Singleton<GameEventController>.Instance.GetCurrentStage(), false, "GameEventData_35", new object[] { lastLearnSkill[i].skillName }, null, null, null, null, null);
				this.AddEvent(gameEventUIData);
			}
			Singleton<GameEventController>.Instance.ClearLastLearnSkill();
		}

		public void SelectSkillByBegin(int delayTime)
		{
			if (this.beginSelectSkillNum > 0)
			{
				this.isSelectSkill = true;
				Sequence sequence = this.seqPool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, (float)delayTime);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					SelectSkillViewModule.OpenData openData = new SelectSkillViewModule.OpenData();
					openData.type = SelectSkillViewModule.SelectSkillType.BeginSkill;
					openData.sourceType = SkillBuildSourceType.Normal;
					openData.randomNum = 3;
					openData.selectNum = 1;
					openData.callBack = new Action(this.SelectSkillFinishByBegin);
					openData.randomSeed = Singleton<GameEventController>.Instance.GetLevelUpSkillSeed(1);
					GameApp.View.OpenView(ViewName.SelectSkillViewModule, openData, 1, null, null);
				});
			}
		}

		private void SelectSkillFinishByBegin()
		{
			this.isSelectSkill = false;
			this.beginSelectSkillNum--;
			if (this.beginSelectSkillNum > 0)
			{
				this.SelectSkillByBegin(200);
			}
		}

		private int GetChapterTotalStage()
		{
			return this.chapterModule.CurrentChapter.TotalStage;
		}

		private void OpenResult(int result)
		{
			this.isGameOver = true;
			if (result == 1)
			{
				EventArgsBool eventArgsBool = new EventArgsBool();
				eventArgsBool.SetData(false);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_IAP_BattleFail_ShowFirstRechargeUI, eventArgsBool);
			}
			else
			{
				EventArgsBool eventArgsBool2 = new EventArgsBool();
				eventArgsBool2.SetData(true);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_IAP_BattleFail_ShowFirstRechargeUI, eventArgsBool2);
			}
			this.FinishSave(result);
		}

		private void ShowEventInfoTip(string info)
		{
			this.eventController.ChangeEventInfo(info);
		}

		private void ShowBattleAni(bool isBattle)
		{
			this.buttonSpeed.SetClickDisabled(true);
			Sequence sequence = DOTween.Sequence();
			float num = 462f;
			float num2 = 630f;
			if (isBattle)
			{
				this.battleObj.SetActiveSafe(true);
				this.btnSpeedTrans.anchoredPosition = new Vector2(num2, this.btnSpeedTrans.anchoredPosition.y);
				TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.btnSpeedTrans, num - 20f, 0.2f, false)), ShortcutExtensions46.DOAnchorPosX(this.btnSpeedTrans, num, 0.1f, false));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.buttonSpeed.SetClickDisabled(false);
				});
				return;
			}
			this.btnSpeedTrans.anchoredPosition = new Vector2(num, this.btnSpeedTrans.anchoredPosition.y);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.btnSpeedTrans, num - 20f, 0.2f, false)), ShortcutExtensions46.DOAnchorPosX(this.btnSpeedTrans, num2, 0.1f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.battleObj.SetActiveSafe(false);
				this.buttonSpeed.SetClickDisabled(false);
			});
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_StartEvent, new HandlerEvent(this.OnEventStart));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_AddEvent, new HandlerEvent(this.OnAddEvent));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_UserSelected, new HandlerEvent(this.OnUseSelected));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ShakeButton, new HandlerEvent(this.OnShakeButton));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_RefreshAttribute, new HandlerEvent(this.OnRefreshAttribute));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_RefreshExp, new HandlerEvent(this.OnRefreshExp));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_AddEnemy, new HandlerEvent(this.OnEnterBattle));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Battle_GameOver, new HandlerEvent(this.OnEndBattle));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_CloseSelectSkill, new HandlerEvent(this.OnCloseSelectSkill));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_SkillAnimation, new HandlerEvent(this.OnSkillAnimation));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ShowLevelUp, new HandlerEvent(this.OnShowLevelUp));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_EventPause, new HandlerEvent(this.OnEventPause));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ShowBoxUI, new HandlerEvent(this.OnShowBoxUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_CloseBoxUI, new HandlerEvent(this.OnCloseBoxUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ShowSurpriseUI, new HandlerEvent(this.OnShowSurpriseUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_CloseSurpriseUI, new HandlerEvent(this.OnCloseSurpriseUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_OpenMiniGameUI, new HandlerEvent(this.OnOpenMiniGameUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_CloseMiniGameUI, new HandlerEvent(this.OnCloseMiniGameUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_DropInfo, new HandlerEvent(this.OnDropInfo));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_FlyDrop, new HandlerEvent(this.OnFlyDrop));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_NewScene_Pause, new HandlerEvent(this.OnNewScenePause));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_MoveToNpc, new HandlerEvent(this.OnEventMoveToNpc));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEven_ArriveNpc, new HandlerEvent(this.OnEventArriveNpc));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEven_ArriveEnemy, new HandlerEvent(this.OnEventArriveEnemy));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_RemoveNpc_Finish, new HandlerEvent(this.OnRemoveNpcFinish));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_SelectBeginSkill, new HandlerEvent(this.OnSelectBeginSkill));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_CheckUnlockSkillShow, new HandlerEvent(this.OnCheckUnlockSkillShow));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_SetSpecialAni, new HandlerEvent(this.OnEventSetSpecialAni));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_CloseFishingResult, new HandlerEvent(this.OnEventCloseFishingResult));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_MonsterGroupFight, new HandlerEvent(this.OnEventBattleNpcFight));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIFishing_Close, new HandlerEvent(this.OnEventCloseFishingGame));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_EnterNewStage, new HandlerEvent(this.OnEventNewStage));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_UIAnimationStart, new HandlerEvent(this.OnEventUIAnimationStart));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_UIAnimationFinish, new HandlerEvent(this.OnEventUIAnimationFinish));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_UIShowButton, new HandlerEvent(this.OnEventShowButton));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_ChapterBattle_Start, new HandlerEvent(this.OnEventBattleStart));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_NoMoreEvent, new HandlerEvent(this.OnEventNorMoreEvent));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ShowActivity, new HandlerEvent(this.OnEventShowActivity));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_FlyItems, new HandlerEvent(this.OnFlyItems));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnRoundRefresh));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterActivityWheel_GetScore, new HandlerEvent(this.OnEventGetWheelScore));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Battle_StartRun, new HandlerEvent(this.OnEventBattleStartRun));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_StartEvent, new HandlerEvent(this.OnEventStart));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_AddEvent, new HandlerEvent(this.OnAddEvent));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_UserSelected, new HandlerEvent(this.OnUseSelected));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ShakeButton, new HandlerEvent(this.OnShakeButton));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_RefreshAttribute, new HandlerEvent(this.OnRefreshAttribute));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_RefreshExp, new HandlerEvent(this.OnRefreshExp));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_AddEnemy, new HandlerEvent(this.OnEnterBattle));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Battle_GameOver, new HandlerEvent(this.OnEndBattle));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_CloseSelectSkill, new HandlerEvent(this.OnCloseSelectSkill));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_SkillAnimation, new HandlerEvent(this.OnSkillAnimation));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ShowLevelUp, new HandlerEvent(this.OnShowLevelUp));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_EventPause, new HandlerEvent(this.OnEventPause));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ShowBoxUI, new HandlerEvent(this.OnShowBoxUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_CloseBoxUI, new HandlerEvent(this.OnCloseBoxUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ShowSurpriseUI, new HandlerEvent(this.OnShowSurpriseUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_CloseSurpriseUI, new HandlerEvent(this.OnCloseSurpriseUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_OpenMiniGameUI, new HandlerEvent(this.OnOpenMiniGameUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_CloseMiniGameUI, new HandlerEvent(this.OnCloseMiniGameUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_DropInfo, new HandlerEvent(this.OnDropInfo));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_FlyDrop, new HandlerEvent(this.OnFlyDrop));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_NewScene_Pause, new HandlerEvent(this.OnNewScenePause));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_MoveToNpc, new HandlerEvent(this.OnEventMoveToNpc));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEven_ArriveNpc, new HandlerEvent(this.OnEventArriveNpc));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEven_ArriveEnemy, new HandlerEvent(this.OnEventArriveEnemy));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_RemoveNpc_Finish, new HandlerEvent(this.OnRemoveNpcFinish));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_SelectBeginSkill, new HandlerEvent(this.OnSelectBeginSkill));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_CheckUnlockSkillShow, new HandlerEvent(this.OnCheckUnlockSkillShow));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_SetSpecialAni, new HandlerEvent(this.OnEventSetSpecialAni));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_CloseFishingResult, new HandlerEvent(this.OnEventCloseFishingResult));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_MonsterGroupFight, new HandlerEvent(this.OnEventBattleNpcFight));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIFishing_Close, new HandlerEvent(this.OnEventCloseFishingGame));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_EnterNewStage, new HandlerEvent(this.OnEventNewStage));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_UIAnimationStart, new HandlerEvent(this.OnEventUIAnimationStart));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_UIAnimationFinish, new HandlerEvent(this.OnEventUIAnimationFinish));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_UIShowButton, new HandlerEvent(this.OnEventShowButton));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_ChapterBattle_Start, new HandlerEvent(this.OnEventBattleStart));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_NoMoreEvent, new HandlerEvent(this.OnEventNorMoreEvent));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ShowActivity, new HandlerEvent(this.OnEventShowActivity));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_FlyItems, new HandlerEvent(this.OnFlyItems));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnRoundRefresh));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterActivityWheel_GetScore, new HandlerEvent(this.OnEventGetWheelScore));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Battle_StartRun, new HandlerEvent(this.OnEventBattleStartRun));
		}

		private void OnEventPause(object sender, int type, BaseEventArgs args)
		{
			EventArgEventPause eventArgEventPause = args as EventArgEventPause;
			if (eventArgEventPause == null)
			{
				return;
			}
			this.isEventPuase = eventArgEventPause.isPause;
		}

		private void OnEventStart(object sender, int type, BaseEventArgs args)
		{
			this.EventStart();
		}

		private void OnAddEvent(object sender, int type, BaseEventArgs args)
		{
			EventArgAddEvent eventArgAddEvent = args as EventArgAddEvent;
			if (eventArgAddEvent != null)
			{
				this.AddEvent(eventArgAddEvent.uiData);
			}
		}

		private void OnUseSelected(object sender, int type, BaseEventArgs args)
		{
			EventArgUserSelected eventArgUserSelected = args as EventArgUserSelected;
			if (eventArgUserSelected != null && eventArgUserSelected.buttonData != null)
			{
				this.PushEvent(GameEventPushType.ClickButton, eventArgUserSelected.buttonData.index);
			}
		}

		private void OnShakeButton(object sender, int type, BaseEventArgs args)
		{
			this.eventController.ShakeButton();
		}

		private void OnRefreshAttribute(object sender, int type, BaseEventArgs args)
		{
			this.RefreshInfo(true);
		}

		private void OnRefreshExp(object sender, int type, BaseEventArgs args)
		{
			if (this.isChapterPass)
			{
				return;
			}
			this.RefreshExpInfo();
		}

		private void OnEnterBattle(object sender, int type, BaseEventArgs args)
		{
			this.EnterBattle();
		}

		private void OnEndBattle(object sender, int type, BaseEventArgs args)
		{
			EventArgsGameEnd eventArgsGameEnd = args as EventArgsGameEnd;
			if (eventArgsGameEnd != null)
			{
				this.EndBattle(eventArgsGameEnd.m_gameOverType);
			}
		}

		private void OnEventBattleNpcFight(object sender, int type, BaseEventArgs args)
		{
			this.EnterBattle();
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_130");
			this.ShowEventInfoTip(infoByID);
		}

		private void OnCloseSelectSkill(object sender, int type, BaseEventArgs args)
		{
			if (this.isSelectSkill)
			{
				this.isSelectSkill = false;
				this.ShowSelectSkillEventUI();
			}
			EventArgSelectSkillEnd eventArgSelectSkillEnd = args as EventArgSelectSkillEnd;
			this.PushEvent(GameEventPushType.CloseSelectSkill, (eventArgSelectSkillEnd != null) ? eventArgSelectSkillEnd.skills : null);
		}

		private void OnShowBoxUI(object sender, int type, BaseEventArgs args)
		{
			EventArgRandomBox eventArgRandomBox = args as EventArgRandomBox;
			if (eventArgRandomBox == null)
			{
				return;
			}
			this.isShowSpecialUI = true;
			GameApp.View.OpenView(ViewName.GameEventBoxViewModule, eventArgRandomBox.randomBoxData, 1, null, null);
		}

		private void OnCloseBoxUI(object sender, int type, BaseEventArgs args)
		{
			this.isShowSpecialUI = false;
			EventArgSelectSkillEnd eventArgSelectSkillEnd = args as EventArgSelectSkillEnd;
			this.PushEvent(GameEventPushType.CloseBox, (eventArgSelectSkillEnd != null) ? eventArgSelectSkillEnd.skills : null);
		}

		private void OnShowSurpriseUI(object sender, int type, BaseEventArgs args)
		{
			EventArgSurprise eventArgSurprise = args as EventArgSurprise;
			if (eventArgSurprise == null)
			{
				return;
			}
			this.isShowSpecialUI = true;
			Chapter_surpriseBuild elementById = GameApp.Table.GetManager().GetChapter_surpriseBuildModelInstance().GetElementById(eventArgSurprise.surpriseId);
			if (elementById != null)
			{
				switch (elementById.type)
				{
				case 1:
				{
					GameEventAngelViewModule.OpenData openData = new GameEventAngelViewModule.OpenData();
					openData.modelId = elementById.modelId;
					openData.memberId = elementById.memberId;
					openData.seed = eventArgSurprise.randomSeed;
					openData.sourceType = SkillBuildSourceType.Angel;
					GameApp.View.OpenView(ViewName.GameEventAngelViewModule, openData, 1, null, null);
					return;
				}
				case 2:
				{
					GameEventDemonViewModule.OpenData openData2 = new GameEventDemonViewModule.OpenData();
					openData2.modelId = elementById.modelId;
					openData2.memberId = elementById.memberId;
					openData2.seed = eventArgSurprise.randomSeed;
					openData2.sourceType = SkillBuildSourceType.Demon;
					GameApp.View.OpenView(ViewName.GameEventDemonViewModule, openData2, 1, null, null);
					return;
				}
				case 3:
				{
					GameEventAdventurerViewModule.OpenData openData3 = new GameEventAdventurerViewModule.OpenData();
					openData3.surpriseId = eventArgSurprise.surpriseId;
					openData3.seed = eventArgSurprise.randomSeed;
					GameApp.View.OpenView(ViewName.GameEventAdventurerViewModule, openData3, 1, null, null);
					return;
				}
				case 4:
					HLog.LogError("老虎机功能屏蔽了，不要配置老虎机！");
					return;
				case 5:
				{
					SlotTrainViewModule.OpenData openData4 = new SlotTrainViewModule.OpenData();
					openData4.seed = eventArgSurprise.randomSeed;
					openData4.sourceType = SkillBuildSourceType.SlotTrain;
					GameApp.View.OpenView(ViewName.SlotTrainViewModule, openData4, 1, null, null);
					break;
				}
				default:
					return;
				}
			}
		}

		private void OnCloseSurpriseUI(object sender, int type, BaseEventArgs args)
		{
			EventArgSelectSurprise eventArgSelectSurprise = args as EventArgSelectSurprise;
			if (eventArgSelectSurprise == null)
			{
				return;
			}
			this.isShowSpecialUI = false;
			this.PushEvent(eventArgSelectSurprise.pushType, eventArgSelectSurprise.selectData);
		}

		private void OnOpenMiniGameUI(object sender, int type, BaseEventArgs args)
		{
			this.isShowSpecialUI = true;
		}

		private void OnCloseMiniGameUI(object sender, int type, BaseEventArgs args)
		{
			EventCloseMiniGameUI eventCloseMiniGameUI = args as EventCloseMiniGameUI;
			if (eventCloseMiniGameUI != null)
			{
				this.isShowSpecialUI = false;
				Singleton<GameEventController>.Instance.CloseMiniGame(eventCloseMiniGameUI.miniGameType, eventCloseMiniGameUI.rewardList);
			}
		}

		private void OnNewScenePause(object sender, int type, BaseEventArgs args)
		{
			EventArgChangeMapPause eventArgChangeMapPause = args as EventArgChangeMapPause;
			if (eventArgChangeMapPause == null)
			{
				return;
			}
			this.isEventPuase = eventArgChangeMapPause.isPause;
			if (eventArgChangeMapPause.isPause)
			{
				this.chapterStateController.Show(false, false);
				this.chapterNameController.Show(false, null);
				return;
			}
			Singleton<GameEventController>.Instance.ContinueEvent();
			this.RefreshStageName(true, eventArgChangeMapPause.mapId);
		}

		private void OnEventMoveToNpc(object sender, int type, BaseEventArgs args)
		{
			EventArgsMoveToNpc eventArgsMoveToNpc = args as EventArgsMoveToNpc;
			if (eventArgsMoveToNpc == null)
			{
				return;
			}
			NpcFunction function = eventArgsMoveToNpc.function;
			if (function <= NpcFunction.Normal || function == NpcFunction.BattleNpc || function == NpcFunction.EnemyComing)
			{
				if (this.isMoveToNpc)
				{
					return;
				}
				this.PushEvent(GameEventPushType.MoveToNpc, null);
				this.isMoveToNpc = true;
				this.eventController.SetMoveToNpc(true);
				string text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_132");
				if (eventArgsMoveToNpc.function == NpcFunction.EnemyComing)
				{
					text = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_EnemyComing");
				}
				this.ShowEventInfoTip(text);
			}
		}

		private void OnEventArriveNpc(object sender, int type, BaseEventArgs args)
		{
			this.isMoveToNpc = false;
			this.eventController.SetMoveToNpc(false);
			this.PushEvent(GameEventPushType.NpcArrived, null);
			this.eventController.ChangeEventDefaultInfo();
		}

		private void OnEventArriveEnemy(object sender, int type, BaseEventArgs args)
		{
			this.isMoveToNpc = false;
			this.eventController.SetMoveToNpc(false);
			this.PushEvent(GameEventPushType.NpcArrived, null);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("GameEventData_130");
			this.ShowEventInfoTip(infoByID);
		}

		private void OnRemoveNpcFinish(object sender, int type, BaseEventArgs args)
		{
			this.ShowEventInfoTip(Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_115"));
			this.PushEvent(GameEventPushType.NpcLeaved, null);
		}

		private void OnSelectBeginSkill(object sender, int type, BaseEventArgs args)
		{
			this.SelectSkillByBegin(200);
		}

		private void OnCheckUnlockSkillShow(object sender, int type, BaseEventArgs args)
		{
			List<GameEventSkillBuildData> unlockSkills = Singleton<GameEventController>.Instance.GetUnlockSkills();
			if (unlockSkills.Count > 0)
			{
				GameEventSkillBuildData gameEventSkillBuildData = unlockSkills[0];
				Singleton<GameEventController>.Instance.RemoveUnlockSkill(0);
				GameApp.View.OpenView(ViewName.UnlockSkillViewModule, gameEventSkillBuildData, 2, null, null);
			}
		}

		private void OnEventSetSpecialAni(object sender, int type, BaseEventArgs args)
		{
			EventArgsBool eventArgsBool = args as EventArgsBool;
			if (eventArgsBool != null)
			{
				this.isPlaySpecialAni = eventArgsBool.Value;
			}
		}

		private void OnEventCloseFishingResult(object sender, int type, BaseEventArgs args)
		{
			this.PushEvent(GameEventPushType.CloseFishingResult, null);
		}

		private void OnEventCloseFishingGame(object sender, int type, BaseEventArgs args)
		{
			this.PushEvent(GameEventPushType.CloseFishingGame, null);
		}

		private void OnEventNewStage(object sender, int type, BaseEventArgs eventArgs)
		{
			int currentStage = Singleton<GameEventController>.Instance.GetCurrentStage();
			int chapterTotalStage = this.GetChapterTotalStage();
			if (currentStage > chapterTotalStage)
			{
				this.isChapterPass = false;
				this.OpenResult(-1);
			}
		}

		private void OnEventUIAnimationStart(object sender, int type, BaseEventArgs eventArgs)
		{
			this.isPlaySpecialAni = true;
		}

		private void OnEventUIAnimationFinish(object sender, int type, BaseEventArgs eventArgs)
		{
			this.PushEvent(GameEventPushType.UIAniFinish, null);
			this.isPlaySpecialAni = false;
		}

		private void OnEventShowButton(object sender, int type, BaseEventArgs eventArgs)
		{
			this.eventController.EventAnimationFinish(null);
		}

		private void OnEventBattleStart(object sender, int type, BaseEventArgs eventArgs)
		{
			this.BattleStart();
		}

		private void OnEventBattleStartRun(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsBattleStart eventArgsBattleStart = eventArgs as EventArgsBattleStart;
			bool flag = true;
			if (eventArgsBattleStart != null && eventArgsBattleStart.data != null)
			{
				int i = 0;
				while (i < eventArgsBattleStart.data.m_members.Count)
				{
					GameStartMemberData gameStartMemberData = eventArgsBattleStart.data.m_members[i];
					if (gameStartMemberData != null && gameStartMemberData.m_instanceId == 100)
					{
						if (gameStartMemberData.m_maxLegacyPower.Count > 0)
						{
							flag = false;
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			if (flag)
			{
				this.RefreshBattle();
			}
		}

		private void OnEventNorMoreEvent(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OpenResult(-1);
		}

		private void OnEventShowActivity(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsShowActivity eventArgsShowActivity = eventArgs as EventArgsShowActivity;
			if (eventArgsShowActivity != null && eventArgsShowActivity.flyActList != null)
			{
				for (int i = 0; i < eventArgsShowActivity.flyActList.Count; i++)
				{
					switch (eventArgsShowActivity.flyActList[i])
					{
					case ChapterActivityKind.Rank:
						this.rankEnterCtrl.PlayAni(eventArgsShowActivity.IsShow);
						break;
					case ChapterActivityKind.BattlePass:
						this.chapterBattlePassCtrl.PlayAni(eventArgsShowActivity.IsShow);
						break;
					case ChapterActivityKind.Wheel:
						this.wheelEnterCtrl.PlayAni(eventArgsShowActivity.IsShow);
						break;
					}
				}
			}
		}

		private void SetFlyEndNode()
		{
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd.SetData(FlyItemModel.Battle, CurrencyType.BattleExp, new List<Transform> { this.attributeController.m_expNode.transform });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd);
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd2 = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd2.SetData(FlyItemModel.Battle, CurrencyType.BattleFood, new List<Transform> { this.attributeController.m_goldNode.transform });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd2);
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd3 = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd3.SetData(FlyItemModel.Battle, FlyItemOtherType.Gold, new List<Transform> { this.playerCoinCtrl.GetFlyNode(CurrencyType.Gold) });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd3);
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd4 = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd4.SetData(FlyItemModel.Battle, FlyItemOtherType.Diamond, new List<Transform> { this.playerCoinCtrl.GetFlyNode(CurrencyType.Diamond) });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd4);
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd5 = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd5.SetData(FlyItemModel.Battle, FlyItemOtherType.Chips, new List<Transform> { this.playerCoinCtrl.GetFlyNode(CurrencyType.Chips) });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd5);
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd6 = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd6.SetData(FlyItemModel.Battle, FlyItemOtherType.ActivityScoreNormal, new List<Transform> { this.chapterBattlePassCtrl.GetFlyNode() });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd6);
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd7 = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd7.SetData(FlyItemModel.Battle, FlyItemOtherType.ActivityScoreRank, new List<Transform> { this.rankEnterCtrl.GetFlyNode() });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd7);
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd8 = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd8.SetData(FlyItemModel.Battle, FlyItemOtherType.BagItem, new List<Transform> { this.bagNode });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd8);
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd9 = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd9.SetData(FlyItemModel.Battle, FlyItemOtherType.ActivityScoreWheel, new List<Transform> { this.wheelEnterCtrl.GetFlyNode() });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd9);
		}

		private void OnDropInfo(object sender, int type, BaseEventArgs args)
		{
			EventArgDropInfo eventArgDropInfo = args as EventArgDropInfo;
			if (eventArgDropInfo != null)
			{
				this.attParams = new List<NodeParamBase>();
				this.attParams.AddRange(eventArgDropInfo.paramList);
			}
		}

		private void OnFlyDrop(object sender, int type, BaseEventArgs args)
		{
			if (this.attParams == null)
			{
				return;
			}
			if (this.isGameOver)
			{
				return;
			}
			int num = -1;
			List<ItemData> list = new List<ItemData>();
			List<FlyItemData> list2 = new List<FlyItemData>();
			List<ChapterActivityKind> list3 = new List<ChapterActivityKind>();
			for (int i = 0; i < this.attParams.Count; i++)
			{
				if (this.attParams[i].FinalCount != 0.0)
				{
					int num2 = (int)(this.attParams[i].FinalCount / 20.0);
					if (num2 < 3)
					{
						num2 = 3;
					}
					if (num2 > 10)
					{
						num2 = 10;
					}
					if (this.attParams[i].GetNodeKind() == NodeKind.EventAtt)
					{
						NodeAttParam nodeAttParam = this.attParams[i] as NodeAttParam;
						if (nodeAttParam != null)
						{
							GameEventAttType attType = nodeAttParam.attType;
							if (attType != GameEventAttType.Exp)
							{
								if (attType == GameEventAttType.Food)
								{
									list.Add(new ItemData(51, (long)num2));
								}
							}
							else
							{
								list.Add(new ItemData(52, (long)num2));
							}
						}
					}
					else if (this.attParams[i].GetNodeKind() == NodeKind.Item)
					{
						NodeItemParam nodeItemParam = this.attParams[i] as NodeItemParam;
						if (nodeItemParam != null)
						{
							if (nodeItemParam.itemType == ItemType.eCurrency)
							{
								if (nodeItemParam.itemId == 1 || nodeItemParam.itemId == 4)
								{
									num = 1;
									FlyItemData flyItemData = new FlyItemData(FlyItemOtherType.Gold, 0L, (long)num2, (long)num2, null);
									list2.Add(flyItemData);
								}
								else if (nodeItemParam.itemId == 2)
								{
									num = 2;
									FlyItemData flyItemData2 = new FlyItemData(FlyItemOtherType.Diamond, 0L, (long)num2, (long)num2, null);
									list2.Add(flyItemData2);
								}
							}
							else
							{
								FlyItemData flyItemData3 = new FlyItemData(FlyItemOtherType.BagItem, 0L, (long)num2, (long)num2, nodeItemParam.itemId);
								list2.Add(flyItemData3);
							}
						}
					}
					else if (this.attParams[i].GetNodeKind() == NodeKind.ActivityScore)
					{
						NodeScoreParam nodeScoreParam = this.attParams[i] as NodeScoreParam;
						if (nodeScoreParam != null)
						{
							list3.Add(nodeScoreParam.activityKind);
							FlyItemOtherType flyType = nodeScoreParam.GetFlyType();
							if (flyType != FlyItemOtherType.Null)
							{
								FlyItemData flyItemData4 = new FlyItemData(flyType, 0L, (long)num2, (long)num2, nodeScoreParam.activityRowId);
								list2.Add(flyItemData4);
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				if (num > 0)
				{
					this.playerCoinCtrl.Show((CurrencyType)num, null);
				}
				this.isFlyAni = true;
				GameApp.View.FlyItemDatas(FlyItemModel.Battle, list, new OnFlyNodeItemDatasItemFinished(this.FlyItemFinish), null);
			}
			if (list2.Count > 0)
			{
				this.isFlyAni = true;
				if (list3.Count > 0)
				{
					EventArgsShowActivity eventArgsShowActivity = new EventArgsShowActivity();
					eventArgsShowActivity.SetData(list3, true);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShowActivity, eventArgsShowActivity);
				}
				GameApp.View.FlyItemDatas(FlyItemModel.Battle, list2, new OnFlyNodeFlyNodeOthersItemFinished(this.FlyItemFinish), null);
			}
		}

		private void OnFlyItems(object sender, int type, BaseEventArgs args)
		{
			EventArgFlyItems eventArgFlyItems = args as EventArgFlyItems;
			if (eventArgFlyItems != null)
			{
				List<NodeParamBase> flyItems = eventArgFlyItems.flyItems;
				List<FlyItemData> list = new List<FlyItemData>();
				List<ChapterActivityKind> list2 = new List<ChapterActivityKind>();
				for (int i = 0; i < flyItems.Count; i++)
				{
					NodeItemParam nodeItemParam = flyItems[i] as NodeItemParam;
					if (nodeItemParam != null)
					{
						if (nodeItemParam.type == NodeItemType.Item)
						{
							NodeItemParam nodeItemParam2 = nodeItemParam;
							FlyItemOtherType flyItemOtherType;
							if (nodeItemParam2.itemId == 1 || nodeItemParam2.itemId == 4)
							{
								flyItemOtherType = FlyItemOtherType.Gold;
							}
							else
							{
								if (nodeItemParam2.itemId != 2)
								{
									goto IL_0135;
								}
								flyItemOtherType = FlyItemOtherType.Diamond;
							}
							int num = (int)nodeItemParam.FinalCount;
							FlyItemData flyItemData = new FlyItemData(flyItemOtherType, 0L, (long)num, (long)num, null);
							list.Add(flyItemData);
						}
					}
					else
					{
						NodeScoreParam nodeScoreParam = flyItems[i] as NodeScoreParam;
						if (nodeScoreParam != null)
						{
							list2.Add(nodeScoreParam.activityKind);
							int num2 = (int)nodeScoreParam.FinalCount;
							FlyItemData flyItemData2 = new FlyItemData(nodeScoreParam.GetFlyType(), 0L, (long)num2, (long)num2, nodeScoreParam.activityRowId);
							list.Add(flyItemData2);
						}
						else
						{
							NodeAttParam nodeAttParam = flyItems[i] as NodeAttParam;
							if (nodeAttParam != null && nodeAttParam.attType == GameEventAttType.Chips)
							{
								int num3 = (int)nodeAttParam.FinalCount;
								FlyItemData flyItemData3 = new FlyItemData(FlyItemOtherType.Chips, 0L, (long)num3, (long)num3, null);
								list.Add(flyItemData3);
							}
						}
					}
					IL_0135:;
				}
				if (list.Count > 0)
				{
					if (list2.Count > 0)
					{
						EventArgsShowActivity eventArgsShowActivity = new EventArgsShowActivity();
						eventArgsShowActivity.SetData(list2, true);
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShowActivity, eventArgsShowActivity);
					}
					BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
					this.playerCoinCtrl.SetCoin(CurrencyType.Gold, playerData.PlayerCoin, 1f);
					this.playerCoinCtrl.SetCoin(CurrencyType.Diamond, (long)playerData.Diamond, 1f);
					this.playerCoinCtrl.SetCoin(CurrencyType.Chips, (long)playerData.Chips.mVariable, 1f);
					GameApp.View.FlyItemDatas(FlyItemModel.Battle, list, new OnFlyNodeFlyNodeOthersItemFinished(this.FlyItemFinish), null);
				}
			}
		}

		private void FlyItemFinish(FlyNodeItemDatas data, int index, int endNodeIndex, int itemIndex, float progress)
		{
			this.FlyFinish(index, endNodeIndex, progress);
		}

		private void FlyItemFinish(FlyNodeOthers data, int index, int endNodeIndex, float progress)
		{
			this.FlyFinish(index, endNodeIndex, progress);
		}

		private void FlyFinish(int index, int endNodeIndex, float progress)
		{
			if (this.attParams != null && this.attParams.Count > 0)
			{
				List<NodeAttParam> list = new List<NodeAttParam>();
				List<NodeItemParam> list2 = new List<NodeItemParam>();
				for (int i = 0; i < this.attParams.Count; i++)
				{
					NodeParamBase nodeParamBase = this.attParams[i];
					if (nodeParamBase.GetNodeKind() == NodeKind.EventAtt)
					{
						NodeAttParam nodeAttParam = nodeParamBase as NodeAttParam;
						if (nodeAttParam != null)
						{
							list.Add(nodeAttParam);
						}
					}
					else if (nodeParamBase.GetNodeKind() == NodeKind.Item)
					{
						NodeItemParam nodeItemParam = nodeParamBase as NodeItemParam;
						if (nodeItemParam != null)
						{
							list2.Add(nodeItemParam);
						}
					}
				}
				Singleton<GameEventController>.Instance.MergerAttribute(list);
				Singleton<GameEventController>.Instance.AddDrops(list2);
				BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
				this.playerCoinCtrl.SetCoin(CurrencyType.Gold, playerData.PlayerCoin, 0.4f);
				this.playerCoinCtrl.SetCoin(CurrencyType.Diamond, (long)playerData.Diamond, 0.4f);
				this.attParams.Clear();
			}
			if (index == endNodeIndex && progress >= 1f)
			{
				this.isFlyAni = false;
			}
		}

		private void OnRoundRefresh(object sender, int type, BaseEventArgs args)
		{
			EventArgsRoundStart eventArgsRoundStart = args as EventArgsRoundStart;
			if (eventArgsRoundStart != null)
			{
				this.textRound.text = Singleton<LanguageManager>.Instance.GetInfoByID("event_round", new object[] { string.Format("{0}/{1}", eventArgsRoundStart.CurRound, eventArgsRoundStart.MaxRound) });
			}
		}

		private void OnEventGetWheelScore(object sender, int type, BaseEventArgs args)
		{
			EventArgsChapterWheelGetScore eventArgsChapterWheelGetScore = args as EventArgsChapterWheelGetScore;
			if (eventArgsChapterWheelGetScore != null)
			{
				this.wheelEnterCtrl.PlayAnimation(eventArgsChapterWheelGetScore.OldScore, eventArgsChapterWheelGetScore.NewScore);
			}
		}

		public CustomButton buttonPause;

		public GameObject eventsObj;

		public GameObject battleObj;

		public UISpeedButtonCtrl buttonSpeed;

		public GridLayoutGroup skillLayout;

		public GameObject skillIconItemObj;

		public GameObject specialStateObj;

		public CustomText textRound;

		public UIChapterStateController chapterStateController;

		public UIChapterNameController chapterNameController;

		public UIAttributeController attributeController;

		public UIChapterTaskController chapterTaskController;

		public UIEventController eventController;

		public UIWeatherController weatherController;

		public UISickController sickController;

		public UIFishingController fishingController;

		public UIEventItemController eventItemController;

		public UIEventProgressCtrl progressCtrl;

		public UIPlayerCoinCtrl playerCoinCtrl;

		public ChapterBattlePassCtrl chapterBattlePassCtrl;

		public UIChapterRankEnterCtrl rankEnterCtrl;

		public UIChapterWheelEnterCtrl wheelEnterCtrl;

		public Transform bagNode;

		private ChapterDataModule chapterModule;

		private List<UIGameEventSkillIconItem> skillIconItemList = new List<UIGameEventSkillIconItem>();

		private SequencePool seqPool = new SequencePool();

		private bool isInBattle;

		private bool isShowColor;

		private bool isGameOver;

		private bool isMoveToNpc;

		private bool isPlaySpecialAni;

		private readonly float EventY;

		private bool isSelectSkill;

		private bool isShowSpecialUI;

		private bool isEventPuase;

		private int beginSelectSkillNum;

		private bool isChapterPass;

		private int endStage;

		private Vector3 initChapterNamePos;

		private long befourBattleAtk;

		private long befourBattleDef;

		private List<int> saveStageList = new List<int>();

		private RectTransform btnSpeedTrans;

		public const int FlyBattleMinCount = 3;

		public const int FlyBattleMaxCount = 10;

		public const int FlyBattleAddCount = 20;

		private const int Battle_Success = 1;

		private const int Battle_Fail = -1;

		private List<NodeParamBase> attParams;

		private bool isFlyAni;
	}
}
