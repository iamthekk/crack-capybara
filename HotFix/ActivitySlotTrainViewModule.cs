using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.SevenDayTask;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class ActivitySlotTrainViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.ActivitySlotTrainViewModule;
		}

		public bool IsOpen()
		{
			return base.gameObject.activeSelf;
		}

		public override void OnCreate(object data)
		{
			this.dataModule = GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule);
			this.buttonClose.m_onClick = new Action(this.OnCloseSelf);
			this.buttonCloseTrans = this.buttonClose.GetComponent<RectTransform>();
			this.buttonHelper.m_onClick = new Action(this.OnHelperClick);
			this.currencyCtrl.Init();
			this.InitSlotTrainItems();
			this.buttonSelectBig.m_onClick = new Action(this.OnClickSelectBig);
			this.rewardItemObj.gameObject.SetActiveSafe(false);
			this.CreateTimeRewards();
			this.btnTurnOnce.m_onClick = new Action(this.OnTurnOnce);
			this.btnTurnTen.m_onClick = new Action(this.OnTurnTen);
			this.buttonProbability.m_onClick = new Action(this.OnClickProbability);
			this.buttonSkip.m_onClick = new Action(this.OnClickSkip);
			this.buttonScreenSkipTurn.m_onClick = new Action(this.OnClickScreenSkipTurn);
			this.buttonScreenSkipTurn.gameObject.SetActive(false);
			this.buttonTask.m_onClick = new Action(this.OnClickTask);
			this.buttonGift.m_onClick = new Action(this.OnClickGift);
		}

		public override void OnDelete()
		{
			this.buttonClose.m_onClick = null;
			this.buttonHelper.m_onClick = null;
			this.buttonSelectBig.m_onClick = null;
			for (int i = 0; i < this.slotTrainItems.Count; i++)
			{
				this.slotTrainItems[i].DeInit();
			}
			this.slotTrainItems.Clear();
			for (int j = 0; j < this.timeRewardItems.Count; j++)
			{
				this.timeRewardItems[j].DeInit();
			}
			this.timeRewardItems.Clear();
			this.currencyCtrl.DeInit();
			this.btnTurnOnce.m_onClick = null;
			this.btnTurnTen.m_onClick = null;
			this.buttonProbability.m_onClick = null;
			this.buttonSkip.m_onClick = null;
			this.buttonScreenSkipTurn.m_onClick = null;
			this.buttonTask.m_onClick = null;
			this.buttonGift.m_onClick = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_DayChange_ActivitySlotTrain_DataPull, new HandlerEvent(this.OnEventDayChanged));
			manager.RegisterEvent(LocalMessageName.CC_TurnTableSelectBigGuaranteeItem, new HandlerEvent(this.Event_SelectedBigItem));
			manager.RegisterEvent(LocalMessageName.CC_TurnTableReceiveCumulativeReward, new HandlerEvent(this.Event_ReceiveCumulativeReward));
			manager.RegisterEvent(LocalMessageName.CC_UI_Close_Common_Reward, new HandlerEvent(this.OnCloseSlotRewardUI));
			manager.RegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.Event_ItemUpdate));
			manager.RegisterEvent(LocalMessageName.CC_ActivitySlotTrainQuestChanged, new HandlerEvent(this.OnEventQuestChanged));
			manager.RegisterEvent(LocalMessageName.CC_ActivitySlotTrainPayChanged, new HandlerEvent(this.OnEventPayChanged));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_DayChange_ActivitySlotTrain_DataPull, new HandlerEvent(this.OnEventDayChanged));
			manager.UnRegisterEvent(LocalMessageName.CC_TurnTableSelectBigGuaranteeItem, new HandlerEvent(this.Event_SelectedBigItem));
			manager.UnRegisterEvent(LocalMessageName.CC_TurnTableReceiveCumulativeReward, new HandlerEvent(this.Event_ReceiveCumulativeReward));
			manager.UnRegisterEvent(LocalMessageName.CC_UI_Close_Common_Reward, new HandlerEvent(this.OnCloseSlotRewardUI));
			manager.UnRegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.Event_ItemUpdate));
			manager.UnRegisterEvent(LocalMessageName.CC_ActivitySlotTrainQuestChanged, new HandlerEvent(this.OnEventQuestChanged));
			manager.UnRegisterEvent(LocalMessageName.CC_ActivitySlotTrainPayChanged, new HandlerEvent(this.OnEventPayChanged));
		}

		public override void OnOpen(object data)
		{
			if (!this.dataModule.CanShow())
			{
				this.CloseSelf(true);
				return;
			}
			this.skipSelectObj.SetActive(this.dataModule.SkipAnimation);
			this.selectedIdx = -1;
			this.textProbability.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_probability_btn");
			this.textSkip.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_btn_skipani");
			this.textButtonTask.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_btn_quest");
			this.textButtonGift.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_btn_gift");
			this.FreshAll();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.selectNodeCtrl.IsPlaying)
			{
				if (this.isAddSpeed)
				{
					this.selectNodeCtrl.speedRatio += deltaTime * this.MAX_SPIN_SPEED_RATIO * this.SPIN_ACCELERATE_RATIO;
					this.selectNodeCtrl.speedRatio = Utility.Math.Clamp(this.selectNodeCtrl.speedRatio, 0f, this.MAX_SPIN_SPEED_RATIO);
				}
				this.selectNodeCtrl.OnUpdate(deltaTime);
			}
			this.RefreshCountdown();
		}

		public override void OnClose()
		{
			this.sequencePool.Clear(false);
			if (GameApp.View.IsOpened(ViewName.ActivitySlotTrainTaskViewModule))
			{
				GameApp.View.CloseView(ViewName.ActivitySlotTrainTaskViewModule, null);
			}
			if (GameApp.View.IsOpened(ViewName.ActivitySlotTrainBigSelectViewModule))
			{
				GameApp.View.CloseView(ViewName.ActivitySlotTrainBigSelectViewModule, null);
			}
			if (GameApp.View.IsOpened(ViewName.UIActivitySlotTrainProbabilityViewModule))
			{
				GameApp.View.CloseView(ViewName.UIActivitySlotTrainProbabilityViewModule, null);
			}
		}

		private void FreshAll()
		{
			if (!this.dataModule.CanShow())
			{
				this.ActTimeOut();
				return;
			}
			GameApp.Sound.PlayClip(79, 1f);
			if (this.InitPoolItems())
			{
				this.ResetSlot();
			}
		}

		private void FreshRes()
		{
			this.currencyCtrl.SetItemId(this.mainCfg.priceId);
		}

		private void FreshResNum()
		{
			this.FreshTurnButtons();
		}

		private void RefreshCountdown()
		{
			if (!this.dataModule.CanShow())
			{
				this.ActTimeOut();
				return;
			}
			this.textEndTimePrefix.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_lefttime_prefix");
			this.textEndTime.text = DxxTools.FormatFullTimeWithDay(this.dataModule.LeftTime);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.endTimeLayoutGroup);
		}

		private bool InitPoolItems()
		{
			this.mainCfg = GameApp.Table.GetManager().GetActivityTurntable_ActivityTurntableModelInstance().GetElementById(this.dataModule.TurntableId);
			this.normalPoolCfg = GameApp.Table.GetManager().GetItem_dropModelInstance().GetElementById(int.Parse(this.mainCfg.pool[0]));
			this.smallPoolCfg = GameApp.Table.GetManager().GetItem_dropModelInstance().GetElementById(int.Parse(this.mainCfg.pool[1]));
			this.bigPoolCfg = GameApp.Table.GetManager().GetItem_dropModelInstance().GetElementById(int.Parse(this.mainCfg.pool[2]));
			this.posItemDic.Clear();
			this.BindPosItem(1, this.smallPoolCfg, 0);
			this.BindPosItem(14, this.smallPoolCfg, 1);
			for (int i = 0; i < 12; i++)
			{
				this.BindPosItem(i + 2, this.normalPoolCfg, i);
			}
			this.playCount = 0;
			return true;
		}

		private void BindPosItem(int pos, Item_drop drop, int dropIndex)
		{
			string[] array = drop.reward[dropIndex].Split(',', StringSplitOptions.None);
			ItemData itemData = new ItemData(int.Parse(array[0]), long.Parse(array[1]));
			this.posItemDic[pos] = itemData.ToPropData();
		}

		private void ResetSlot()
		{
			this.FreshRes();
			this.FreshSlotItems();
			this.FreshTimeRewards();
			this.FreshTurnButtons();
			this.FreshTaskReds();
			this.PlayOpenAni();
		}

		private void InitSlotTrainItems()
		{
			for (int i = 0; i < this.slotTrainItems.Count; i++)
			{
				this.slotTrainItems[i].gameObject.SetActiveSafe(false);
				this.slotTrainItems[i].Init();
			}
		}

		private void CreateTimeRewards()
		{
			IList<ActivityTurntable_TurntableReward> activityTurntable_TurntableRewardElements = GameApp.Table.GetManager().GetActivityTurntable_TurntableRewardElements();
			for (int i = 0; i < activityTurntable_TurntableRewardElements.Count; i++)
			{
				UIActivitySlotTrainRewardItem uiactivitySlotTrainRewardItem = Object.Instantiate<UIActivitySlotTrainRewardItem>(this.rewardItemObj, this.rewardItemObj.transform.parent);
				uiactivitySlotTrainRewardItem.cfg = activityTurntable_TurntableRewardElements[i];
				uiactivitySlotTrainRewardItem.gameObject.SetActive(true);
				uiactivitySlotTrainRewardItem.uiItem.Init();
				uiactivitySlotTrainRewardItem.uiItem.onClick = new Action<UIItem, PropData, object>(this.OnClickRewardItem);
				this.timeRewardItems.Add(uiactivitySlotTrainRewardItem);
			}
		}

		private void FreshSlotItems()
		{
			for (int i = 1; i < this.slotTrainItems.Count; i++)
			{
				this.FreshSlotItem(i);
			}
			this.FreshSelectedBigItem();
		}

		private void FreshSlotItem(int pos)
		{
			UIActivitySlotTrainItem uiactivitySlotTrainItem = this.slotTrainItems[pos];
			uiactivitySlotTrainItem.gameObject.SetActive(true);
			uiactivitySlotTrainItem.Refresh(this.posItemDic[pos], this.showNode);
			int num = pos % 6 + 1;
			uiactivitySlotTrainItem.SetSoundId(num);
			this.FreshSlotItemLimited(pos, false);
		}

		private void FreshSlotItemLimited(int index, bool byTurnAni = false)
		{
			UIActivitySlotTrainItem uiactivitySlotTrainItem = this.slotTrainItems[index];
			if (index == 0)
			{
				uiactivitySlotTrainItem.txtLimited.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_limited", new object[] { 1 });
				return;
			}
			if (index == 1 || index == 14)
			{
				int num;
				if (byTurnAni)
				{
					this.dataModule.LastSmallGuaranteeCount.TryGetValue(index, ref num);
					num = Mathf.Max(0, num - 1);
					this.dataModule.LastSmallGuaranteeCount[index] = num;
				}
				else if (this.dataModule.SmallGuaranteeCount != null && this.dataModule.SmallGuaranteeCount.ContainsKey(index))
				{
					num = this.dataModule.SmallGuaranteeCount[index];
				}
				else if (index == 1)
				{
					num = int.Parse(this.mainCfg.limitItems[0].Split(',', StringSplitOptions.None)[1]);
				}
				else
				{
					num = int.Parse(this.mainCfg.limitItems[1].Split(',', StringSplitOptions.None)[1]);
				}
				uiactivitySlotTrainItem.txtLimited.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_limited", new object[] { num });
			}
		}

		private void FreshSelectedBigItem()
		{
			int num = this.dataModule.BigGuaranteeItemConfigId;
			long num2 = (long)((ulong)this.dataModule.BigGuaranteeItemNum);
			if (num <= 0)
			{
				string[] array = this.bigPoolCfg.reward[0].Split(',', StringSplitOptions.None);
				num = int.Parse(array[0]);
				num2 = long.Parse(array[1]);
			}
			else
			{
				for (int i = 0; i < this.bigPoolCfg.reward.Length; i++)
				{
					string[] array2 = this.bigPoolCfg.reward[i].Split(',', StringSplitOptions.None);
					if ((ulong)uint.Parse(array2[0]) == (ulong)((long)num))
					{
						num2 = long.Parse(array2[1]);
						break;
					}
				}
			}
			PropData propData = new ItemData(num, num2).ToPropData();
			this.posItemDic[0] = propData;
			this.FreshSlotItem(0);
			ItemType itemType;
			int num3;
			if (PropDataModule.TryGetModelIdByItemId(num, out itemType, out num3))
			{
				this.slotTrainItems[0].uiItemIconObj.SetActiveSafe(false);
				this.bigUIItemIcon.gameObject.SetActiveSafe(false);
				if (itemType == ItemType.Mount)
				{
					this.slotTrainItems[0].spineModelItem.gameObject.SetActiveSafe(false);
					this.slotTrainItems[0].spineMountModelItem.gameObject.SetActiveSafe(true);
					this.slotTrainItems[0].spineMountModelItem.ShowModel(num3, "Idle", true);
					this.bigSpineModelItem.gameObject.SetActiveSafe(false);
					this.bigSpineMountModelItem.gameObject.SetActiveSafe(true);
					this.bigSpineMountModelItem.ShowModel(num3, "Idle", true);
				}
				else
				{
					this.slotTrainItems[0].spineModelItem.gameObject.SetActiveSafe(true);
					this.slotTrainItems[0].spineMountModelItem.gameObject.SetActiveSafe(false);
					this.slotTrainItems[0].spineModelItem.ShowMemberModel(num3, "Idle", true);
					this.bigSpineModelItem.gameObject.SetActiveSafe(true);
					this.bigSpineMountModelItem.gameObject.SetActiveSafe(false);
					this.bigSpineModelItem.ShowModel(num3, "Idle", true);
				}
			}
			else
			{
				this.slotTrainItems[0].uiItemIconObj.SetActiveSafe(true);
				this.bigUIItemIcon.gameObject.SetActiveSafe(true);
				this.bigUIItemIcon.sprite = this.slotTrainItems[0].uiItem.m_imgItemIcon.sprite;
				this.slotTrainItems[0].spineModelItem.gameObject.SetActiveSafe(false);
				this.slotTrainItems[0].spineMountModelItem.gameObject.SetActiveSafe(false);
				this.bigSpineModelItem.gameObject.SetActiveSafe(false);
				this.bigSpineMountModelItem.gameObject.SetActiveSafe(false);
			}
			if (this.bigPoolCfg.reward.Length > 1)
			{
				this.buttonSelectBig.gameObject.SetActive(true);
				this.txtSelectBig.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_selectbig");
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)this.slotTrainItems[0].uiItem.m_propData.id);
				this.buttonSelectBigIcon.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
				return;
			}
			this.buttonSelectBig.gameObject.SetActive(false);
		}

		private void FreshTimeRewards()
		{
			this.textGetBigTip1.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_getbigtip1", new object[] { this.mainCfg.bigPityCount });
			this.textGetBigTip2.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_getbigtip2");
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.textGetBigTipLayout);
			IList<ActivityTurntable_TurntableReward> activityTurntable_TurntableRewardElements = GameApp.Table.GetManager().GetActivityTurntable_TurntableRewardElements();
			for (int i = 0; i < this.timeRewardItems.Count; i++)
			{
				UIActivitySlotTrainRewardItem uiactivitySlotTrainRewardItem = this.timeRewardItems[i];
				string[] array = activityTurntable_TurntableRewardElements[i].reward[0].Split(',', StringSplitOptions.None);
				ItemData itemData = new ItemData(int.Parse(array[0]), long.Parse(array[1]));
				uiactivitySlotTrainRewardItem.uiItem.SetData(itemData.ToPropData());
				uiactivitySlotTrainRewardItem.uiItem.OnRefresh();
			}
			this.FreshTurnTableTimes();
		}

		private void FreshTurnTableTimes()
		{
			this.textGetBigTip3.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_getbigtip3", new object[] { this.mainCfg.bigPityCount - this.dataModule.BigGuaranteeCount % this.mainCfg.bigPityCount });
			this.textGetLimitTimesTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_getlettimetip", new object[] { this.dataModule.GetLimitLeftCount });
			this.textTurnedTimes.text = this.dataModule.Count.ToString();
			this.textTurnedTimesDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_times");
			this.FreshTurnTableTimePickStates();
		}

		private void FreshTurnTableTimePickStates()
		{
			IList<ActivityTurntable_TurntableReward> activityTurntable_TurntableRewardElements = GameApp.Table.GetManager().GetActivityTurntable_TurntableRewardElements();
			for (int i = 0; i < this.timeRewardItems.Count; i++)
			{
				UIActivitySlotTrainRewardItem uiactivitySlotTrainRewardItem = this.timeRewardItems[i];
				bool flag = this.dataModule.PickedTurntableReward(uiactivitySlotTrainRewardItem.cfg);
				uiactivitySlotTrainRewardItem.maskObj.SetActiveSafe(flag);
				if (flag)
				{
					uiactivitySlotTrainRewardItem.textNeed.text = Singleton<LanguageManager>.Instance.GetInfoByID("btn_txt_state_collected");
					uiactivitySlotTrainRewardItem.canPick.gameObject.SetActiveSafe(false);
					uiactivitySlotTrainRewardItem.redNode.gameObject.SetActiveSafe(false);
					uiactivitySlotTrainRewardItem.canPickEffect.SetActiveSafe(false);
				}
				else if ((ulong)this.dataModule.Count >= (ulong)((long)activityTurntable_TurntableRewardElements[i].point))
				{
					uiactivitySlotTrainRewardItem.textNeed.text = Singleton<LanguageManager>.Instance.GetInfoByID("btn_txt_state_cancollect");
					uiactivitySlotTrainRewardItem.canPick.gameObject.SetActiveSafe(true);
					uiactivitySlotTrainRewardItem.redNode.gameObject.SetActiveSafe(true);
					uiactivitySlotTrainRewardItem.redNode.mType = 240;
					uiactivitySlotTrainRewardItem.redNode.Value = 1;
					uiactivitySlotTrainRewardItem.canPickEffect.SetActiveSafe(true);
				}
				else
				{
					uiactivitySlotTrainRewardItem.textNeed.text = string.Format("<color=#{0}>{1}</color>/{2}", ColorUtility.ToHtmlStringRGBA(this.colorTimeUnEnough), this.dataModule.Count, activityTurntable_TurntableRewardElements[i].point);
					uiactivitySlotTrainRewardItem.canPick.gameObject.SetActiveSafe(false);
					uiactivitySlotTrainRewardItem.redNode.gameObject.SetActiveSafe(false);
					uiactivitySlotTrainRewardItem.canPickEffect.SetActiveSafe(false);
				}
			}
		}

		private void FreshTurnButtons()
		{
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(this.mainCfg.priceId);
			long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)this.mainCfg.priceId));
			this.textTurnOnce.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_btn_turn", new object[] { this.mainCfg.singlePrice });
			this.iconTurnOnce.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
			this.costNumTurnOnce.text = string.Format("x{0}", this.mainCfg.singlePrice);
			if (itemDataCountByid >= (long)this.mainCfg.singlePrice)
			{
				this.costNumTurnOnce.color = this.colorPriceEnough;
			}
			else
			{
				this.costNumTurnOnce.color = this.colorPriceUnEnough;
			}
			this.textTurnTen.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_btn_turn", new object[] { this.mainCfg.tenPrice });
			this.iconTurnTen.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
			this.costNumTurnTen.text = string.Format("x{0}", this.mainCfg.tenPrice);
			if (itemDataCountByid >= (long)this.mainCfg.tenPrice)
			{
				this.costNumTurnTen.color = this.colorPriceEnough;
				return;
			}
			this.costNumTurnTen.color = this.colorPriceUnEnough;
		}

		private void ResetLastSmallGuaranteeCount()
		{
			this.dataModule.ResetLastSmallGuaranteeCount(int.Parse(this.mainCfg.limitItems[0].Split(',', StringSplitOptions.None)[1]), int.Parse(this.mainCfg.limitItems[1].Split(',', StringSplitOptions.None)[1]));
		}

		private void OnTurnOnce()
		{
			this.OnTurn(this.mainCfg.singlePrice);
		}

		private void OnTurnTen()
		{
			this.OnTurn(this.mainCfg.tenPrice);
		}

		private void OnTurn(int count)
		{
			if (this.isPlayOpenAni)
			{
				return;
			}
			if (this.isStarAni)
			{
				return;
			}
			if (this.isOpenRewardUI)
			{
				return;
			}
			if (this.isTradeBtn)
			{
				return;
			}
			PropDataModule propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			if (propDataModule.GetItemDataCountByid((ulong)((long)this.mainCfg.priceId)) < (long)count)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("100000110"));
				this.OnClickGift();
				return;
			}
			this.isTradeBtn = true;
			this.ResetLastSmallGuaranteeCount();
			this.dataModule.SaveLastSmallGuaranteeCount();
			TweenCallback <>9__1;
			NetworkUtils.ActivitySlotTrain.RequestTurnTableExtract(count, delegate(bool success, TurnTableExtractResponse resp)
			{
				if (success)
				{
					this.FreshTurnTableTimes();
					if (this.dataModule.SkipAnimation)
					{
						this.SlotTrainComplete();
					}
					else
					{
						Sequence sequence = this.sequencePool.Get();
						TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.buttonCloseTrans, -304f, 0.1f, false));
						TweenCallback tweenCallback;
						if ((tweenCallback = <>9__1) == null)
						{
							tweenCallback = (<>9__1 = delegate
							{
								this.buttonCloseTrans.localScale = Vector3.zero;
								this.StartSlotTrain();
							});
						}
						TweenSettingsExtensions.AppendCallback(sequence, tweenCallback);
					}
					int num = ((count == this.mainCfg.singlePrice) ? 1 : 2);
					long itemDataCountByid = propDataModule.GetItemDataCountByid((ulong)((long)this.mainCfg.priceId));
					GameApp.SDK.Analyze.Track_CapyRoulette(num, itemDataCountByid, resp.CommonData.Reward);
					return;
				}
				this.isTradeBtn = false;
			});
		}

		private void OnCloseSelf()
		{
			this.CloseSelf(false);
		}

		private void ActTimeOut()
		{
			if (!this.IsOpen())
			{
				return;
			}
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_end"));
			GameApp.View.CloseView(ViewName.ActivitySlotTrainTaskViewModule, null);
			this.CloseSelf(true);
		}

		private void CloseSelf(bool forceCloseAll = false)
		{
			if (!forceCloseAll)
			{
				if (this.isPlayOpenAni)
				{
					return;
				}
				if (this.isStarAni)
				{
					return;
				}
				if (this.isOpenRewardUI)
				{
					return;
				}
			}
			else
			{
				this.isPlayOpenAni = false;
				this.sequencePool.Clear(false);
				if (this.isStarAni)
				{
					this.SkipAnimation();
				}
			}
			GameApp.View.CloseView(this.GetName(), null);
		}

		private void OnHelperClick()
		{
			Module_moduleInfo elementById = GameApp.Table.GetManager().GetModule_moduleInfoModelInstance().GetElementById(this.systemId);
			if (elementById != null)
			{
				InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData
				{
					m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameId),
					m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.infoId)
				};
				GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
			}
		}

		private void OnClickProbability()
		{
			GameApp.View.OpenView(ViewName.UIActivitySlotTrainProbabilityViewModule, this.posItemDic, 1, null, null);
		}

		private void OnClickSkip()
		{
			this.dataModule.SkipAnimation = !this.dataModule.SkipAnimation;
			this.skipSelectObj.SetActiveSafe(this.dataModule.SkipAnimation);
			if (this.isStarAni && this.dataModule.SkipAnimation)
			{
				this.SkipAnimation();
			}
		}

		private void OnClickScreenSkipTurn()
		{
			if (this.isStarAni)
			{
				this.buttonScreenSkipTurn.gameObject.SetActive(false);
				this.SkipAnimation();
			}
		}

		public void PlayOpenAni()
		{
			if (this.isPlayOpenAni)
			{
				return;
			}
			this.isPlayOpenAni = true;
			this.buttonClose.transform.localScale = Vector3.zero;
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
			TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
			for (int i = 0; i < 6; i++)
			{
				int index = i;
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.slotTrainItems[index].Show();
					this.slotTrainItems[index + 6].Show();
				});
				TweenSettingsExtensions.AppendInterval(sequence, 0.05f);
			}
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.buttonCloseTrans.localScale = Vector3.one * 0.7f;
				this.buttonCloseTrans.anchoredPosition = new Vector2(-304f, this.buttonCloseTrans.anchoredPosition.y);
			});
			float num = 0.2f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.buttonCloseTrans, 82f, num, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.isPlayOpenAni = false;
			});
		}

		private void StartSlotTrain()
		{
			this.playCount = 0;
			this.buttonScreenSkipTurn.gameObject.SetActive(true);
			this.PlayTrainAni(0);
		}

		private void PlayTrainAni(int dataIndex)
		{
			this.selectedIdx = dataIndex % this.slotTrainItems.Count;
			this.isStarAni = true;
			DropItemDto dropItemDto = this.dataModule.DropItemDtos[this.playCount];
			this.targetIndex = dropItemDto.Index;
			this.selectNodeCtrl.Init(this.slotTrainItems, new Action(this.OnEndSlotTrain), new Action<int>(this.OnSelect), null);
			this.selectNodeCtrl.StartAtIndex(this.selectedIdx, this.targetIndex, 1);
		}

		private void OnSelect(int index)
		{
			this.FreshSlotItemLimited(index, true);
			if (index == 0)
			{
				this.ResetLastSmallGuaranteeCount();
				this.FreshSlotItemLimited(1, true);
				this.FreshSlotItemLimited(14, true);
			}
			this.slotTrainItems[index].SelectNodeSelect(false);
		}

		private void OnEndSlotTrain()
		{
			this.isStarAni = false;
			this.playCount++;
			Sequence sequence = this.sequencePool.Get();
			if (this.playCount >= this.dataModule.DropItemDtos.Count)
			{
				TweenSettingsExtensions.AppendInterval(sequence, 1f);
				TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.SlotTrainComplete));
				return;
			}
			TweenSettingsExtensions.AppendInterval(sequence, 1f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.ResetMask();
				this.PlayTrainAni(this.targetIndex + 1);
			});
		}

		private void SlotTrainComplete()
		{
			this.sequencePool.Clear(false);
			this.isTradeBtn = false;
			this.isStarAni = false;
			this.selectNodeCtrl.Stop();
			this.buttonScreenSkipTurn.gameObject.SetActive(false);
			this.ResetMask();
			this.FreshSlotItemLimited(0, false);
			this.FreshSlotItemLimited(1, false);
			this.FreshSlotItemLimited(14, false);
			this.ShowResult();
		}

		public void ResetMask()
		{
			for (int i = 0; i < this.slotTrainItems.Count; i++)
			{
				this.slotTrainItems[i].Reset();
			}
		}

		public void SkipAnimation()
		{
			this.SlotTrainComplete();
			for (int i = 0; i < this.slotTrainItems.Count - 1; i++)
			{
				this.slotTrainItems[i].Stop();
			}
			this.slotTrainItems[this.targetIndex].SelectNodeSelect(false);
		}

		private void ShowResult()
		{
			if (this.dataModule.RewardItems == null || this.dataModule.RewardItems.Count < 1)
			{
				return;
			}
			this.isOpenRewardUI = true;
			DxxTools.UI.OpenRewardCommon(this.dataModule.RewardItems, null, true);
		}

		private void FreshTaskReds()
		{
			this.FreshQuestRed();
			this.FreshPayRed();
		}

		private void FreshQuestRed()
		{
			bool flag = this.dataModule.ShowRedByQuest();
			this.redNodeTask.gameObject.SetActiveSafe(flag);
			if (flag)
			{
				this.redNodeTask.Value = 1;
				this.redNodeTask.mType = 240;
			}
		}

		private void FreshPayRed()
		{
			bool flag = this.dataModule.ShowRedByPay();
			this.redNodeGift.gameObject.SetActiveSafe(flag);
			if (flag)
			{
				this.redNodeGift.Value = 1;
				this.redNodeGift.mType = 240;
			}
		}

		private void OnCloseSlotRewardUI(object sender, int type, BaseEventArgs eventargs)
		{
			Sequence sequence = this.sequencePool.Get();
			this.buttonCloseTrans.localScale = Vector3.one * 0.7f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.buttonCloseTrans, 82f, 0.1f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.isOpenRewardUI = false;
			});
		}

		private void OnClickRewardItem(UIItem item, PropData data, object arg)
		{
			for (int i = 0; i < this.timeRewardItems.Count; i++)
			{
				if (this.timeRewardItems[i].uiItem == item && this.dataModule.CanPickTurntableReward(this.timeRewardItems[i].cfg))
				{
					NetworkUtils.ActivitySlotTrain.RequestTurnTableReceiveCumulativeReward(this.timeRewardItems[i].cfg.id, delegate(bool isOk, TurnTableReceiveCumulativeRewardResponse resp)
					{
						if (isOk)
						{
							long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)this.mainCfg.priceId));
							GameApp.SDK.Analyze.Track_CapyRoulette(3, itemDataCountByid, resp.CommonData.Reward);
						}
					});
					return;
				}
			}
			DxxTools.UI.ShowItemInfo(new ItemInfoOpenData
			{
				m_propData = data,
				m_openDataType = ItemInfoOpenDataType.eBag,
				m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume)
			}, item.transform.position, 70f);
		}

		private void OnClickTask()
		{
			GameApp.View.OpenView(ViewName.ActivitySlotTrainTaskViewModule, ActivitySlotTrainTaskViewType.Quest, 1, null, null);
		}

		private void OnClickGift()
		{
			GameApp.View.OpenView(ViewName.ActivitySlotTrainTaskViewModule, ActivitySlotTrainTaskViewType.Pay, 1, null, null);
		}

		private void OnClickSelectBig()
		{
			GameApp.View.OpenView(ViewName.ActivitySlotTrainBigSelectViewModule, null, 1, null, null);
		}

		private void Event_ReceiveCumulativeReward(object sender, int eventid, BaseEventArgs eventArgs)
		{
			if (eventArgs is EventArgsTurnTableReceiveCumulativeRewardData)
			{
				this.FreshTurnTableTimePickStates();
			}
		}

		private void Event_SelectedBigItem(object sender, int eventid, BaseEventArgs eventArgs)
		{
			if (eventArgs is EventArgsTurnTableSelectBigGuaranteeItemData)
			{
				this.FreshSelectedBigItem();
			}
		}

		private void Event_ItemUpdate(object sender, int eventid, BaseEventArgs eventArgs)
		{
			EventArgsItemUpdate eventArgsItemUpdate = eventArgs as EventArgsItemUpdate;
			if (eventArgsItemUpdate != null && eventArgsItemUpdate.itemId == this.mainCfg.priceId)
			{
				this.FreshResNum();
			}
		}

		private void OnEventQuestChanged(object sender, int type, BaseEventArgs eventArgs)
		{
			this.FreshQuestRed();
		}

		private void OnEventPayChanged(object sender, int type, BaseEventArgs eventArgs)
		{
			this.FreshPayRed();
		}

		private void OnEventDayChanged(object sender, int type, BaseEventArgs eventArgs)
		{
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("9100009"));
			this.CloseSelf(true);
		}

		[Header("基础")]
		public int systemId = 113;

		public CustomButton buttonClose;

		public CustomButton buttonMask;

		public CustomButton buttonHelper;

		public CustomButton buttonScreenSkipTurn;

		public CustomButton buttonTask;

		public CustomText textButtonTask;

		public RedNodeOneCtrl redNodeTask;

		public CustomButton buttonGift;

		public CustomText textButtonGift;

		public RedNodeOneCtrl redNodeGift;

		public UIActivityCurrency currencyCtrl;

		public CustomText textEndTimePrefix;

		public CustomText textEndTime;

		public RectTransform endTimeLayoutGroup;

		[Header("奖池")]
		public CustomButton buttonSelectBig;

		public CustomImage buttonSelectBigIcon;

		public CustomText txtSelectBig;

		public GameObject showNode;

		public UISpineModelItem bigSpineModelItem;

		public UISpineMountModelItem bigSpineMountModelItem;

		public Image bigUIItemIcon;

		public List<UIActivitySlotTrainItem> slotTrainItems = new List<UIActivitySlotTrainItem>();

		[Header("抽奖动画")]
		public bool isAddSpeed = true;

		public float MAX_SPIN_SPEED_RATIO = 3.5f;

		public float SPIN_ACCELERATE_RATIO = 0.6f;

		public float STOP_DURATION = 0.2f;

		public ActivitySlotTrainSelectNodeCtrl selectNodeCtrl = new ActivitySlotTrainSelectNodeCtrl();

		[Header("次数奖励")]
		public RectTransform textGetBigTipLayout;

		public CustomText textGetBigTip1;

		public CustomText textGetBigTip2;

		public CustomText textGetBigTip3;

		public CustomText textGetLimitTimesTip;

		public CustomText textTurnedTimes;

		public CustomText textTurnedTimesDesc;

		public UIActivitySlotTrainRewardItem rewardItemObj;

		public Color colorTimeUnEnough = Color.red;

		public Color colorPriceUnEnough = Color.red;

		public Color colorPriceEnough = Color.white;

		private List<UIActivitySlotTrainRewardItem> timeRewardItems = new List<UIActivitySlotTrainRewardItem>();

		[Header("抽奖按钮")]
		public CustomButton btnTurnOnce;

		public CustomText textTurnOnce;

		public CustomImage iconTurnOnce;

		public CustomText costNumTurnOnce;

		public CustomButton btnTurnTen;

		public CustomText textTurnTen;

		public CustomImage iconTurnTen;

		public CustomText costNumTurnTen;

		public CustomButton buttonSkip;

		public CustomText textSkip;

		public GameObject skipSelectObj;

		public CustomButton buttonProbability;

		public CustomText textProbability;

		private SequencePool sequencePool = new SequencePool();

		private int targetIndex = -1;

		private bool isStarAni;

		private bool isTradeBtn;

		private int playCount;

		private bool isOpenRewardUI;

		private bool isPlayOpenAni;

		private RectTransform buttonCloseTrans;

		private ActivitySlotTrainDataModule dataModule;

		private ActivityTurntable_ActivityTurntable mainCfg;

		private Item_drop bigPoolCfg;

		private Item_drop smallPoolCfg;

		private Item_drop normalPoolCfg;

		private Dictionary<int, PropData> posItemDic = new Dictionary<int, PropData>();

		private int selectedIdx = -1;
	}
}
