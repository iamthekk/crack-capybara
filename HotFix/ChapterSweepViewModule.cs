using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Chapter;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class ChapterSweepViewModule : BaseViewModule
	{
		public bool IsStartSweep
		{
			get
			{
				return this.isStartSweep;
			}
		}

		private int RealCostEnergy
		{
			get
			{
				return this.costEnergyBase * this.selectRate;
			}
		}

		public override void OnCreate(object data)
		{
			this.sweepDataModule = GameApp.Data.GetDataModule(DataName.ChapterSweepDataModule);
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.hangUpDataModule = GameApp.Data.GetDataModule(DataName.HangUpDataModule);
			this.chapterBattlePassCtrl.Init();
			this.sweepEventCtrl.Init();
			this.buttonSweepCtrl.Init();
			this.sweepTipCtrl.Init();
			this.monthCardCtrl.Init();
			List<int> list = new List<int> { 1, 2 };
			for (int i = 0; i < this.sweepRewardItems.Count; i++)
			{
				UISweepRewardItem uisweepRewardItem = this.sweepRewardItems[i];
				if (i < list.Count)
				{
					uisweepRewardItem.gameObject.SetActiveSafe(true);
					uisweepRewardItem.SetData(list[i]);
					uisweepRewardItem.Init();
					this.itemDic.Add((CurrencyType)list[i], uisweepRewardItem);
				}
				else
				{
					uisweepRewardItem.gameObject.SetActiveSafe(false);
				}
			}
			this.animatorListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnListenAnimation));
			this.nextDayAniListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnWinAniFinish));
			this.buttonHangUp.onClick.AddListener(new UnityAction(this.OnClickHangUp));
			RedPointController.Instance.RegRecordChange("Main.HangUp", new Action<RedNodeListenData>(this.OnRedPointHangUpChange));
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			ChapterSweepViewModule.OpenData openData = data as ChapterSweepViewModule.OpenData;
			if (openData != null)
			{
				this.openData = openData;
			}
			this.sweepMapManager = SweepMapManager.Instance;
			this.sweepMapManager.SetCameraTarget(this.rawImage, this.rawImage.rectTransform.rect.size, 1000);
			this.sweepMapManager.SetShow(true);
			this.sweepMapManager.StartMove();
			this.chapterBattlePassCtrl.SetChapterId(this.openData.chapterId, false);
			this.sweepChapter = GameApp.Table.GetManager().GetChapter_chapterModelInstance().GetElementById(this.openData.chapterId);
			this.rateList = this.sweepDataModule.GetRateList();
			this.costEnergyBase = this.sweepChapter.cost[1];
			this.selectRate = this.sweepDataModule.SweepRate;
			if (this.selectRate < 1)
			{
				this.selectRate = 1;
			}
			else
			{
				int num = this.selectRate;
				List<int> list = this.rateList;
				if (num > list[list.Count - 1])
				{
					List<int> list2 = this.rateList;
					this.selectRate = list2[list2.Count - 1];
				}
			}
			this.SetFlyEndNode();
			this.buttonBack.onClick.AddListener(new UnityAction(this.OnClickBack));
			this.buttonRateSub.onClick.AddListener(new UnityAction(this.OnClickRateSub));
			this.buttonRateAdd.onClick.AddListener(new UnityAction(this.OnClickRateAdd));
			this.buttonRateMax.onClick.AddListener(new UnityAction(this.OnClickRateMax));
			this.buttonSweepCtrl.SetData(new Action(this.OnClickSweep));
			this.buttonSweepCtrl.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("ui_sweep_start"));
			this.textStartTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_sweep_not_start");
			this.chapterBattlePassCtrl.SetData(true);
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
			this.RefreshRate();
			this.RefreshReward(false);
			this.monthCardCtrl.Refresh();
			bool flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.HangUp, false);
			this.buttonHangUp.gameObject.SetActiveSafe(flag);
			this.RefreshHangUpIcon();
			if (this.openData.isRecord)
			{
				this.SetSweep(true);
				int rate = Singleton<EventRecordController>.Instance.SweepRecord.rate;
				this.textRate.text = Singleton<LanguageManager>.Instance.GetInfoByID("sweep_rate", new object[] { rate });
				Singleton<GameEventController>.Instance.StartEvent();
			}
			GuideController.Instance.DelTarget("ButtonRate");
			GuideController.Instance.AddTarget("ButtonRate", this.buttonRateAdd.transform);
			GuideController.Instance.OpenViewTrigger(ViewName.ChapterSweepViewModule);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.chapterBattlePassCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			this.sweepTipCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			this.rankEnterCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			this.wheelEnterCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			bool flag = this.hangUpDataModule.IsMaxReward();
			if (!this.redNodeHangUp.gameObject.activeSelf && flag)
			{
				this.redNodeHangUp.gameObject.SetActiveSafe(true);
				this.RefreshHangUpIcon();
			}
		}

		public override void OnClose()
		{
			Singleton<GameManager>.Instance.ActiveSweepSpeed(false);
			this.buttonBack.onClick.RemoveListener(new UnityAction(this.OnClickBack));
			this.buttonRateSub.onClick.RemoveListener(new UnityAction(this.OnClickRateSub));
			this.buttonRateAdd.onClick.RemoveListener(new UnityAction(this.OnClickRateAdd));
			this.buttonRateMax.onClick.RemoveListener(new UnityAction(this.OnClickRateMax));
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_Travel_Close_View, null);
		}

		public override void OnDelete()
		{
			RedPointController.Instance.UnRegRecordChange("Main.HangUp", new Action<RedNodeListenData>(this.OnRedPointHangUpChange));
			this.animatorListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnListenAnimation));
			this.nextDayAniListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnWinAniFinish));
			this.buttonHangUp.onClick.RemoveListener(new UnityAction(this.OnClickHangUp));
			this.chapterBattlePassCtrl.DeInit();
			this.sweepEventCtrl.DeInit();
			this.sweepTipCtrl.DeInit();
			this.buttonSweepCtrl.DeInit();
			this.rankEnterCtrl.DeInit();
			this.monthCardCtrl.DeInit();
			this.wheelEnterCtrl.DeInit();
			for (int i = 0; i < this.sweepRewardItems.Count; i++)
			{
				this.sweepRewardItems[i].DeInit();
			}
			this.itemDic.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_AddEvent, new HandlerEvent(this.OnAddEvent));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_UIAnimationFinish, new HandlerEvent(this.OnEventUIAnimationFinish));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_CloseMiniGameUI, new HandlerEvent(this.OnEventCloseMiniGameUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterSweep_ResetSweep, new HandlerEvent(this.OnEventResetSweep));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_FlyItems, new HandlerEvent(this.OnFlyItems));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterSweep_PlayOpenAni, new HandlerEvent(this.OnEventPlayOpenAni));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ShowActivity, new HandlerEvent(this.OnEventShowActivity));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_DAY_CHANGE, new HandlerEvent(this.OnRefreshRate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Refresh_HangUp, new HandlerEvent(this.OnEventRefreshHangUp));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterSweep_Refresh, new HandlerEvent(this.OnRefreshRate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterActivityWheel_GetScore, new HandlerEvent(this.OnEventGetWheelScore));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_AddEvent, new HandlerEvent(this.OnAddEvent));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_UIAnimationFinish, new HandlerEvent(this.OnEventUIAnimationFinish));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_CloseMiniGameUI, new HandlerEvent(this.OnEventCloseMiniGameUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterSweep_ResetSweep, new HandlerEvent(this.OnEventResetSweep));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_FlyItems, new HandlerEvent(this.OnFlyItems));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterSweep_PlayOpenAni, new HandlerEvent(this.OnEventPlayOpenAni));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ShowActivity, new HandlerEvent(this.OnEventShowActivity));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_DAY_CHANGE, new HandlerEvent(this.OnRefreshRate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Refresh_HangUp, new HandlerEvent(this.OnEventRefreshHangUp));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterSweep_Refresh, new HandlerEvent(this.OnRefreshRate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterActivityWheel_GetScore, new HandlerEvent(this.OnEventGetWheelScore));
		}

		private void RefreshRate()
		{
			bool flag = this.iapDataModule.MonthCard.IsActivation(IAPMonthCardData.CardType.Month);
			this.buttonRateMax.gameObject.SetActiveSafe(flag);
			int num = this.selectRate;
			List<int> list = this.rateList;
			bool flag2 = num == list[list.Count - 1];
			this.textRate.text = Singleton<LanguageManager>.Instance.GetInfoByID("sweep_rate", new object[] { this.selectRate });
			this.textCost.text = string.Format("-{0}", this.RealCostEnergy);
			this.sweepTipCtrl.SetData(this.selectRate, flag2);
			if ((ulong)GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.UserLife).NewNum < (ulong)((long)this.RealCostEnergy))
			{
				this.textCost.color = Color.red;
			}
			else
			{
				this.textCost.color = Color.white;
			}
			int num2 = this.rateList.IndexOf(this.selectRate);
			if (num2 == 0)
			{
				this.grayRateSub.SetUIGray();
				this.grayRateAdd.Recovery();
				this.grayRateMax.Recovery();
			}
			else if (num2 == this.rateList.Count - 1)
			{
				this.grayRateSub.Recovery();
				this.grayRateAdd.SetUIGray();
				this.grayRateMax.SetUIGray();
			}
			else
			{
				this.grayRateSub.Recovery();
				this.grayRateAdd.Recovery();
				this.grayRateMax.Recovery();
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.costRectTrans);
		}

		private void RefreshHangUpIcon()
		{
			if (this.hangUpDataModule.IsMaxReward())
			{
				this.imageHangUp.sprite = this.hangUpSR.GetSprite("full");
				return;
			}
			this.imageHangUp.sprite = this.hangUpSR.GetSprite("unfull");
		}

		private void OnClickBack()
		{
			if (this.isStartSweep)
			{
				return;
			}
			GameApp.View.CloseView(ViewName.ChapterSweepViewModule, null);
			this.sweepMapManager.SetShow(false);
			EventArgsInt eventArgsInt = new EventArgsInt();
			eventArgsInt.SetData(999);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterActivity_CheckShow, eventArgsInt);
		}

		private void OnClickRateSub()
		{
			if (this.isStartSweep)
			{
				return;
			}
			int num = this.rateList.IndexOf(this.selectRate);
			num--;
			if (num < 0)
			{
				num = 0;
			}
			this.selectRate = this.rateList[num];
			this.sweepTipCtrl.SetShow(true);
			this.RefreshRate();
		}

		private void OnClickRateAdd()
		{
			if (this.isStartSweep)
			{
				return;
			}
			int num = this.rateList.IndexOf(this.selectRate);
			num++;
			if (num >= this.rateList.Count)
			{
				num = this.rateList.Count - 1;
			}
			this.selectRate = this.rateList[num];
			this.sweepTipCtrl.SetShow(true);
			this.RefreshRate();
		}

		private void OnClickRateMax()
		{
			if (this.isStartSweep)
			{
				return;
			}
			int num = this.rateList.Count - 1;
			this.selectRate = this.rateList[num];
			this.sweepTipCtrl.SetShow(true);
			this.RefreshRate();
		}

		private void OnClickHangUp()
		{
			if (this.isStartSweep)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("sweep_button_tip");
				GameApp.View.ShowStringTip(infoByID);
				return;
			}
			if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.HangUp, false))
			{
				GameApp.View.ShowStringTip(Singleton<GameFunctionController>.Instance.GetLockTips(202));
				return;
			}
			GameApp.View.OpenView(ViewName.HangUpViewModule, null, 1, null, null);
		}

		private void OnClickSweep()
		{
			if (this.isStartSweep)
			{
				return;
			}
			if ((ulong)GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.UserLife).NewNum < (ulong)((long)this.RealCostEnergy))
			{
				GameApp.View.ShowItemNotEnoughTip(9, true);
				return;
			}
			NetworkUtils.Chapter.DoStartChapterSweepRequest(this.selectRate, delegate(bool result, StartChapterSweepResponse response)
			{
				if (result)
				{
					this.isStartSweep = true;
					EventArgsAddItemTipData eventArgsAddItemTipData = new EventArgsAddItemTipData();
					eventArgsAddItemTipData.SetDataCount(9, -this.RealCostEnergy, this.buttonSweepCtrl.transform.position);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddItemTipNode, eventArgsAddItemTipData);
					Sequence sequence = DOTween.Sequence();
					TweenSettingsExtensions.AppendInterval(sequence, 1f);
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						EventArgSweepStart eventArgSweepStart = new EventArgSweepStart();
						eventArgSweepStart.SetData(response);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_ChapterSweep_Start, eventArgSweepStart);
						Singleton<EventRecordController>.Instance.CreateSweepRecord();
						Singleton<GameEventController>.Instance.EnterEventMode(GameEventStateName.Sweep);
						Singleton<GameEventController>.Instance.StartEvent();
						this.SetSweep(true);
					});
					return;
				}
				this.SetSweep(false);
			});
		}

		public void SetSweep(bool isSweep)
		{
			if (isSweep)
			{
				bool flag = GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.IsActivePrivilege(CardPrivilegeKind.SweepSpeedUp);
				Singleton<GameManager>.Instance.ActiveSweepSpeed(flag);
			}
			this.isStartSweep = isSweep;
			this.buttonBack.gameObject.SetActive(!isSweep);
			this.sweepTipCtrl.SetShow(!isSweep);
			this.buttonSweepCtrl.SetLock(isSweep);
			this.costRectTrans.gameObject.SetActiveSafe(!isSweep);
			this.chapterBattlePassCtrl.SetClickDisabled(isSweep);
			if (isSweep)
			{
				this.buttonSweepCtrl.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("ui_sweep_sweeping"));
				this.textStartTip.text = "";
				return;
			}
			this.buttonSweepCtrl.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("ui_sweep_start"));
			this.textStartTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_sweep_not_start");
		}

		private void OnAddEvent(object sender, int type, BaseEventArgs args)
		{
			EventArgAddEvent ev = args as EventArgAddEvent;
			if (ev != null)
			{
				if (ev.uiData.isRoot && ev.uiData.sizeType == EventSizeType.Activity)
				{
					this.nextDayAni.Play("Anim_UIFX_NextDay_Jackpot", 0, 0f);
					Sequence sequence = DOTween.Sequence();
					TweenSettingsExtensions.AppendInterval(sequence, 1f);
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						this.sweepEventCtrl.AddEvent(ev.uiData);
					});
					return;
				}
				this.sweepEventCtrl.AddEvent(ev.uiData);
			}
		}

		private void SetFlyEndNode()
		{
			UISweepRewardItem rewardItem = this.GetRewardItem(1);
			if (rewardItem != null)
			{
				EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd = new EventArgFlyItemViewModuleSetEnd();
				eventArgFlyItemViewModuleSetEnd.SetData(FlyItemModel.Battle, FlyItemOtherType.Gold, new List<Transform> { rewardItem.GetFlyNode() });
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd);
			}
			UISweepRewardItem rewardItem2 = this.GetRewardItem(2);
			if (rewardItem2 != null)
			{
				EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd2 = new EventArgFlyItemViewModuleSetEnd();
				eventArgFlyItemViewModuleSetEnd2.SetData(FlyItemModel.Battle, FlyItemOtherType.Diamond, new List<Transform> { rewardItem2.GetFlyNode() });
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd2);
			}
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd3 = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd3.SetData(FlyItemModel.Battle, FlyItemOtherType.ActivityScoreNormal, new List<Transform> { this.chapterBattlePassCtrl.GetFlyNode() });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd3);
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd4 = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd4.SetData(FlyItemModel.Battle, FlyItemOtherType.ActivityScoreRank, new List<Transform> { this.rankEnterCtrl.GetFlyNode() });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd4);
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd5 = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd5.SetData(FlyItemModel.Battle, FlyItemOtherType.BagItem, new List<Transform> { this.bagNode });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd5);
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd6 = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd6.SetData(FlyItemModel.Battle, FlyItemOtherType.ActivityScoreWheel, new List<Transform> { this.wheelEnterCtrl.GetFlyNode() });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd6);
		}

		private void OnEventUIAnimationFinish(object sender, int type, BaseEventArgs eventArgs)
		{
			Singleton<GameEventController>.Instance.PushEvent(GameEventPushType.UIAniFinish, null);
		}

		private void OnEventCloseMiniGameUI(object sender, int type, BaseEventArgs args)
		{
			EventCloseMiniGameUI eventCloseMiniGameUI = args as EventCloseMiniGameUI;
			if (eventCloseMiniGameUI != null)
			{
				Singleton<GameEventController>.Instance.CloseMiniGame(eventCloseMiniGameUI.miniGameType, eventCloseMiniGameUI.rewardList);
			}
		}

		private void OnEventResetSweep(object sender, int type, BaseEventArgs eventArgs)
		{
			Singleton<GameManager>.Instance.ActiveSweepSpeed(false);
			this.SetSweep(false);
			this.rateList = this.sweepDataModule.GetRateList();
			this.RefreshRate();
			this.sweepEventCtrl.ResetEvent();
			for (int i = 0; i < this.sweepRewardItems.Count; i++)
			{
				this.sweepRewardItems[i].SetNum(0L, false);
			}
		}

		private void OnEventPlayOpenAni(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.openAni != null)
			{
				this.openAni.Play("Show", 0, 0f);
			}
		}

		private void OnEventShowActivity(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsShowActivity eventArgsShowActivity = eventArgs as EventArgsShowActivity;
			if (eventArgsShowActivity != null && eventArgsShowActivity.flyActList != null)
			{
				for (int i = 0; i < eventArgsShowActivity.flyActList.Count; i++)
				{
					ChapterActivityKind chapterActivityKind = eventArgsShowActivity.flyActList[i];
					if (chapterActivityKind != ChapterActivityKind.Rank)
					{
						if (chapterActivityKind == ChapterActivityKind.Wheel)
						{
							this.wheelEnterCtrl.PlayAni(eventArgsShowActivity.IsShow);
						}
					}
					else
					{
						this.rankEnterCtrl.PlayAni(eventArgsShowActivity.IsShow);
					}
				}
			}
		}

		private void OnRefreshRate(object sender, int type, BaseEventArgs eventArgs)
		{
			this.rateList = this.sweepDataModule.GetRateList();
			if (this.rateList.IndexOf(this.selectRate) < 0)
			{
				this.selectRate = this.rateList[0];
			}
			this.RefreshRate();
			this.monthCardCtrl.Refresh();
		}

		private void OnEventRefreshHangUp(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshHangUpIcon();
		}

		private void OnTicketUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsTicketUpdate eventArgsTicketUpdate = eventArgs as EventArgsTicketUpdate;
			if (eventArgsTicketUpdate == null)
			{
				return;
			}
			if (eventArgsTicketUpdate.TicketKind == UserTicketKind.UserLife)
			{
				this.RefreshRate();
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
							object obj = null;
							FlyItemOtherType flyItemOtherType;
							if (nodeItemParam2.itemId == 1 || nodeItemParam2.itemId == 4)
							{
								flyItemOtherType = FlyItemOtherType.Gold;
							}
							else if (nodeItemParam2.itemId == 2)
							{
								flyItemOtherType = FlyItemOtherType.Diamond;
							}
							else
							{
								flyItemOtherType = FlyItemOtherType.BagItem;
								obj = nodeItemParam.itemId;
							}
							long num = (long)nodeItemParam.FinalCount;
							FlyItemData flyItemData = new FlyItemData(flyItemOtherType, 0L, num, num, obj);
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
					}
				}
				if (list.Count > 0)
				{
					if (list2.Count > 0)
					{
						EventArgsShowActivity eventArgsShowActivity = new EventArgsShowActivity();
						eventArgsShowActivity.SetData(list2, true);
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShowActivity, eventArgsShowActivity);
					}
					GameApp.View.FlyItemDatas(FlyItemModel.Battle, list, new OnFlyNodeFlyNodeOthersItemFinished(this.FlyItemFinish), null);
				}
			}
		}

		private void FlyItemFinish(FlyNodeOthers data, int index, int endNodeIndex, float progress)
		{
			if (index == endNodeIndex && progress >= 1f)
			{
				this.RefreshReward(true);
			}
		}

		private void RefreshReward(bool isAni)
		{
			for (int i = 0; i < this.sweepRewardItems.Count; i++)
			{
				UISweepRewardItem uisweepRewardItem = this.sweepRewardItems[i];
				long dropItemCount = Singleton<GameEventController>.Instance.GetDropItemCount(uisweepRewardItem.ItemId);
				uisweepRewardItem.SetNum(dropItemCount, isAni);
			}
		}

		private UISweepRewardItem GetRewardItem(int id)
		{
			for (int i = 0; i < this.sweepRewardItems.Count; i++)
			{
				UISweepRewardItem uisweepRewardItem = this.sweepRewardItems[i];
				if (uisweepRewardItem.ItemId == id)
				{
					return uisweepRewardItem;
				}
			}
			return null;
		}

		private void OnListenAnimation(GameObject obj, string param)
		{
			if (param.Equals("Finish"))
			{
				this.sweepTipCtrl.ShowAni(true);
			}
		}

		private void OnWinAniFinish(GameObject obj, string param)
		{
			if (param.Equals("Normal"))
			{
				DeviceVibration.PlayVibration(DeviceVibration.VibrationType.Light);
				GameApp.Sound.PlayClip(81, 1f);
				return;
			}
			if (param.Equals("Lucky"))
			{
				DeviceVibration.PlayVibration(DeviceVibration.VibrationType.Light);
				GameApp.Sound.PlayClip(82, 1f);
				return;
			}
			if (param.Equals("SuperLucky"))
			{
				DeviceVibration.PlayVibration(DeviceVibration.VibrationType.Middle);
				GameApp.Sound.PlayClip(83, 1f);
				return;
			}
			if (param.Equals("Unlucky"))
			{
				DeviceVibration.PlayVibration(DeviceVibration.VibrationType.Light);
				GameApp.Sound.PlayClip(84, 1f);
			}
		}

		private void OnRedPointHangUpChange(RedNodeListenData redData)
		{
			if (this.redNodeHangUp == null)
			{
				return;
			}
			this.redNodeHangUp.gameObject.SetActive(redData.m_count > 0);
		}

		public RawImage rawImage;

		public ChapterBattlePassCtrl chapterBattlePassCtrl;

		public UISweepEventCtrl sweepEventCtrl;

		public UISweepTipCtrl sweepTipCtrl;

		public CustomButton buttonBack;

		public CustomButton buttonRateSub;

		public CustomButton buttonRateAdd;

		public CustomButton buttonRateMax;

		public UIGray grayRateSub;

		public UIGray grayRateAdd;

		public UIGray grayRateMax;

		public UIOneButtonCtrl buttonSweepCtrl;

		public RectTransform costRectTrans;

		public CustomText textCost;

		public CustomText textRate;

		public List<UISweepRewardItem> sweepRewardItems;

		public Animator openAni;

		public AnimatorListen animatorListen;

		public Animator nextDayAni;

		public AnimatorListen nextDayAniListen;

		public UIChapterRankEnterCtrl rankEnterCtrl;

		public CustomText textStartTip;

		public Transform bagNode;

		public UISweepMonthCardCtrl monthCardCtrl;

		public CustomButton buttonHangUp;

		public SpriteRegister hangUpSR;

		public CustomImage imageHangUp;

		public RedNodeOneCtrl redNodeHangUp;

		public UIChapterWheelEnterCtrl wheelEnterCtrl;

		private ChapterSweepViewModule.OpenData openData;

		private Dictionary<CurrencyType, UISweepRewardItem> itemDic = new Dictionary<CurrencyType, UISweepRewardItem>();

		private List<int> rateList;

		private Chapter_chapter sweepChapter;

		private int costEnergyBase;

		private int selectRate = 1;

		private bool isStartSweep;

		private ChapterSweepDataModule sweepDataModule;

		private IAPDataModule iapDataModule;

		private HangUpDataModule hangUpDataModule;

		private SweepMapManager sweepMapManager;

		private int textIndex;

		public class OpenData
		{
			public int chapterId;

			public bool isRecord;
		}
	}
}
