using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Common;
using Proto.Tower;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class TowerMainViewModule : BaseViewModule
	{
		private int LayerListCount
		{
			get
			{
				return this.towerDataModule.MaxLevelCount + 1;
			}
		}

		private int EndLayerIndex
		{
			get
			{
				return this.LayerListCount - 2;
			}
		}

		private int TopLayerIndex
		{
			get
			{
				return this.LayerListCount - 1;
			}
		}

		public ViewName GetName()
		{
			return ViewName.TowerMainViewModule;
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(this.GetName(), null);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_ViewPopCheck_ReCheck, null);
		}

		public override void OnCreate(object data)
		{
			this.returnBackButton.onClick.AddListener(new UnityAction(this.CloseSelf));
			this.towerDataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
			this.uiItemInfoButton.Init();
			this.uiItemInfoButton.SetItemIcon(19);
			this.uiItemInfoButton.SetOnClick(new Action(this.OnChallengeClick));
			this.uiItemInfoButton.SetInfoText(Singleton<LanguageManager>.Instance.GetInfoByID("uitower_challenge"));
			LoopListViewInitParam loopListViewInitParam = LoopListViewInitParam.CopyDefaultInitParam();
			loopListViewInitParam.mSnapVecThreshold = 400f;
			loopListViewInitParam.mSnapFinishThreshold = 8f;
			loopListViewInitParam.mSmoothDumpRate = 0.2f;
			this.layerListView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetLayerItem), loopListViewInitParam);
			this.layerListView.mOnSnapItemFinished = new Action<LoopListView2, LoopListViewItem2>(this.OnLayerScrollSnapNearestFinished);
			this.lastLayerScrollSnapIndex = -1;
			this.layerListView.ScrollRect.vertical = false;
			this.layerListView.ScrollRect.horizontal = false;
			this.layerListView.CanDrag = false;
			this.progressCtr.Init();
			this.rankButton.onClick.AddListener(delegate
			{
				GameApp.View.OpenView(ViewName.TowerRankViewModule, null, 1, null, null);
			});
			List<int> list = new List<int>();
			list.Add(2);
			list.Add(1);
			list.Add(19);
			this.currencyCtrl.Init();
			this.currencyCtrl.SetStyle(EModuleId.Tower, list);
		}

		public override void OnOpen(object data)
		{
			bool isPlayLevelMoveAnim = this.towerDataModule.IsPlayLevelMoveAnim;
			this.RefreshAll();
			if (!isPlayLevelMoveAnim)
			{
				this.PlayOpenAni();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.currencyCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
			this.layerSequencePool.Clear(false);
			this.challengeSequencePool.Clear(false);
		}

		public override void OnDelete()
		{
			foreach (KeyValuePair<int, TowerLayerViewCtr> keyValuePair in this.shopItemDic)
			{
				keyValuePair.Value.DeInit();
			}
			if (this.topShopItem != null)
			{
				this.topShopItem.DeInit();
			}
			this.progressCtr.DeInit();
			this.shopItemDic.Clear();
			this.currencyCtrl.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_TowerView_ToNextTower, new HandlerEvent(this.OnEventToNextTower));
			manager.RegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_TowerView_ToNextTower, new HandlerEvent(this.OnEventToNextTower));
			manager.UnRegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
		}

		private bool IsOpen()
		{
			return GameApp.View.IsOpened(ViewName.TowerMainViewModule);
		}

		private void OnTicketUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			this.UpdateChallengeInfo();
		}

		private void OnEventToNextTower(object sender, int type, BaseEventArgs eventArgs)
		{
			if (GameApp.View.IsOpened(ViewName.LoadingMainViewModule))
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayHide(delegate
				{
					GameApp.View.CloseView(ViewName.LoadingMainViewModule, null);
					this.<OnEventToNextTower>g__ToNextTower|40_1();
				});
				return;
			}
			GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 1, null, delegate(GameObject loadObj)
			{
				LoadingMainViewModule loadingViewModule = GameApp.View.GetViewModule(ViewName.LoadingMainViewModule);
				loadingViewModule.PlayShow(delegate
				{
					loadingViewModule.PlayHide(delegate
					{
						GameApp.View.CloseView(ViewName.LoadingMainViewModule, null);
					});
					this.<OnEventToNextTower>g__ToNextTower|40_1();
				});
			});
		}

		private LoopListViewItem2 OnGetLayerItem(LoopListView2 listView, int pageIndex)
		{
			TowerChallenge_Tower curTowerConfig = this.towerDataModule.CurTowerConfig;
			if (pageIndex < 0 || pageIndex > this.TopLayerIndex + 1)
			{
				return null;
			}
			bool flag = this.layerListView.CurSnapNearestItemIndex == pageIndex;
			if (pageIndex == this.TopLayerIndex + 1)
			{
				return listView.NewListViewItem("EmptyNode");
			}
			LoopListViewItem2 loopListViewItem;
			if (pageIndex == this.TopLayerIndex)
			{
				loopListViewItem = listView.NewListViewItem("TowerLayerItemPrefab_Top");
				if (this.topShopItem == null)
				{
					this.topShopItem = loopListViewItem.GetComponent<TowerLayerTopViewCtr>();
					this.topShopItem.Init();
				}
				else if (this.topShopItem.gameObject != loopListViewItem.gameObject)
				{
					this.topShopItem.DeInit();
					this.topShopItem.Init();
				}
				this.topShopItem.RefreshData(curTowerConfig, flag);
			}
			else
			{
				flag &= this.towerDataModule.CurTowerLevelIndex == pageIndex;
				if (this.towerDataModule.IsShowTowerTopReward(this.towerDataModule.CurTowerLevelId))
				{
					flag = false;
				}
				string text = ((pageIndex == this.EndLayerIndex) ? "TowerLayerItemPrefab_End" : "TowerLayerItemPrefab");
				loopListViewItem = listView.NewListViewItem(text);
				int instanceID = loopListViewItem.gameObject.GetInstanceID();
				TowerLayerViewCtr component;
				this.shopItemDic.TryGetValue(instanceID, out component);
				if (component == null)
				{
					component = loopListViewItem.GetComponent<TowerLayerViewCtr>();
					component.Init();
					this.shopItemDic[instanceID] = component;
				}
				component.RefreshData(pageIndex, curTowerConfig, flag);
				if (flag)
				{
					this.currentNode = component;
				}
			}
			this.SetItemSibling(loopListViewItem, listView, pageIndex);
			return loopListViewItem;
		}

		private void SetItemSibling(LoopListViewItem2 item, LoopListView2 listView, int pageIndex)
		{
			LoopListViewItem2 loopListViewItem = null;
			LoopListViewItem2 loopListViewItem2 = null;
			if (pageIndex == this.TopLayerIndex)
			{
				loopListViewItem2 = item;
			}
			else
			{
				foreach (LoopListViewItem2 loopListViewItem3 in listView.ItemList)
				{
					if (this.TopLayerIndex == loopListViewItem3.ItemIndex)
					{
						loopListViewItem2 = loopListViewItem3;
					}
					else if (loopListViewItem3.ItemIndex < pageIndex && (!(loopListViewItem != null) || loopListViewItem.ItemIndex <= loopListViewItem3.ItemIndex))
					{
						loopListViewItem = loopListViewItem3;
					}
				}
				if (loopListViewItem)
				{
					int siblingIndex = loopListViewItem.transform.GetSiblingIndex();
					int num = ((item.transform.GetSiblingIndex() > siblingIndex) ? siblingIndex : (siblingIndex - 1));
					item.transform.SetSiblingIndex(num);
				}
				else
				{
					item.transform.SetAsLastSibling();
				}
			}
			if (loopListViewItem2)
			{
				loopListViewItem2.transform.SetAsLastSibling();
			}
		}

		private void OnLayerScrollSnapNearestBegin(Action onFinish)
		{
			this.isSnapAni = true;
			this.towerTitleCG.alpha = 0f;
			this.rewardProgressTrans.anchoredPosition = new Vector2(-1080f, this.rewardProgressTrans.anchoredPosition.y);
			this.challengeTrans.anchoredPosition = new Vector2(this.challengeTrans.anchoredPosition.x, -500f);
			this.backTrans.anchoredPosition = new Vector2(this.backTrans.anchoredPosition.x, -500f);
			this.rankTrans.anchoredPosition = new Vector2(this.rankTrans.anchoredPosition.x, -500f);
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.AppendInterval(sequence, 1f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				Action onFinish2 = onFinish;
				if (onFinish2 == null)
				{
					return;
				}
				onFinish2();
			});
		}

		private void OnLayerScrollSnapNearestFinished(LoopListView2 listView, LoopListViewItem2 item)
		{
			if (this.lastLayerScrollSnapIndex == listView.CurSnapNearestItemIndex)
			{
				return;
			}
			this.lastLayerScrollSnapIndex = listView.CurSnapNearestItemIndex;
			listView.RefreshItemByItemIndex(listView.CurSnapNearestItemIndex);
			this.UpdateChallengeInfo();
			this.UpdateTowerInfo();
			this.UpdateTitle();
			if (this.isSnapAni)
			{
				TowerLayerViewCtr component = item.GetComponent<TowerLayerViewCtr>();
				if (component != null)
				{
					component.PlayShowAni(new Action(this.<OnLayerScrollSnapNearestFinished>g__NodeAniFinish|44_0));
					return;
				}
				this.<OnLayerScrollSnapNearestFinished>g__NodeAniFinish|44_0();
			}
		}

		private void RefreshAll()
		{
			if (this.towerDataModule.CurTowerConfig == null)
			{
				HLog.LogError("TowerDataModule.CurTowerConfig is null");
				this.CloseSelf();
				return;
			}
			this.UpdateLayerList(new Action(this.UpdateTitle));
			this.progressCtr.UpdateProgressList();
			this.UpdateTowerInfo();
			this.UpdateChallengeInfo();
		}

		private void OnChallengeClick()
		{
			UserTicket ticket = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.Tower);
			if (ticket == null || ticket.NewNum <= 0U)
			{
				GameApp.View.ShowItemNotEnoughTip(UserTicketKind.Tower.GetHashCode(), true);
				return;
			}
			this.SetButtonEnabled(false);
			NetworkUtils.Tower.TowerChallengeRequest(this.towerDataModule.CurTowerLevelId, delegate(bool res, TowerChallengeResponse response)
			{
				if (!this.IsOpen())
				{
					return;
				}
				if (!res)
				{
					this.SetButtonEnabled(true);
					return;
				}
				if (response.Result == 0U)
				{
					RedPointController.Instance.ClickRecord("DailyActivity.ChallengeTag.Tower");
				}
				EventArgsTowerChallengeEnd instance = Singleton<EventArgsTowerChallengeEnd>.Instance;
				instance.Result = (int)response.Result;
				instance.LevelId = (int)response.ConfigId;
				EventArgsAddItemTipData eventArgsAddItemTipData = new EventArgsAddItemTipData();
				eventArgsAddItemTipData.SetDataCount(19, -1, this.uiItemInfoButton.transform.position);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddItemTipNode, eventArgsAddItemTipData);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_TowerDataMoudule_TowerChallengeEnd, instance);
				EventArgsBattleTowerEnter instance2 = Singleton<EventArgsBattleTowerEnter>.Instance;
				instance2.SetData(response);
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_BattleTower_BattleTowerEnter, instance2);
				this.DoBattle(instance, delegate
				{
					this.SetButtonEnabled(true);
				});
			});
		}

		private void DoBattle(EventArgsTowerChallengeEnd data, Action beginGame)
		{
			Action <>9__1;
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				LoadingViewModule viewModule = GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule);
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate
					{
						EventArgsRefreshMainOpenData instance = Singleton<EventArgsRefreshMainOpenData>.Instance;
						instance.SetData(DxxTools.UI.GetTowerOpenData());
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance);
						EventArgsSetTowerBattleReadyData instance2 = Singleton<EventArgsSetTowerBattleReadyData>.Instance;
						instance2.SetData(data.LevelId, false, data.Result);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_TowerDataMoudule_SetBattleReadyData, instance2);
						Action beginGame2 = beginGame;
						if (beginGame2 != null)
						{
							beginGame2();
						}
						GameApp.View.CloseView(this.GetName(), null);
						EventArgsGameDataEnter instance3 = Singleton<EventArgsGameDataEnter>.Instance;
						instance3.SetData(GameModel.Tower, null);
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance3);
						GameApp.State.ActiveState(StateName.BattleTowerState);
					});
				}
				viewModule.PlayShow(action);
			});
		}

		private bool IsInTopLayer()
		{
			return this.layerListView.CurSnapNearestItemIndex >= this.TopLayerIndex;
		}

		private void UpdateTitle()
		{
			bool flag = this.IsInTopLayer();
			this.towerTitleObj.gameObject.SetActive(true);
			this.titleTopLayerObj.SetActive(flag);
			this.rewardProgressObj.SetActive(!flag);
		}

		private void UpdateTowerInfo()
		{
			TowerChallenge_Tower curTowerConfig = this.towerDataModule.CurTowerConfig;
			if (this.IsInTopLayer())
			{
				TowerChallenge_Tower towerChallenge_Tower = GameApp.Table.GetManager().GetTowerChallenge_TowerModelInstance().GetAllElements()
					.Last<TowerChallenge_Tower>();
				if (towerChallenge_Tower != null && curTowerConfig.id == towerChallenge_Tower.id && this.towerDataModule.CheckTowerRewardIsClaimed(curTowerConfig))
				{
					this.titleTopLayerText.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitower_allpass");
				}
				return;
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(curTowerConfig.name);
			this.titleTopLayerText.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitower_pass", new object[] { infoByID });
		}

		private void UpdateChallengeInfo()
		{
			if (this.IsInTopLayer())
			{
				this.uiItemInfoButton.gameObject.SetActive(false);
				return;
			}
			this.uiItemInfoButton.gameObject.SetActive(true);
			int num = this.towerDataModule.CurTowerConfig.level[this.layerListView.CurSnapNearestItemIndex];
			if (this.towerDataModule.CheckCanChallengeByLevelId(num))
			{
				long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid(19UL);
				int num2 = 1;
				string text = ((itemDataCountByid >= (long)num2) ? string.Format("<color=#FFFFFF>{0}</color>", num2) : string.Format("<color=#FF0000>{0}</color>", num2));
				this.uiItemInfoButton.SetCountText(text, false);
			}
		}

		private void UpdateLayerList(Action onNoAnimMoveLevel)
		{
			TowerMainViewModule.<>c__DisplayClass52_0 CS$<>8__locals1 = new TowerMainViewModule.<>c__DisplayClass52_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.onNoAnimMoveLevel = onNoAnimMoveLevel;
			this.layerListView.SetListItemCount(this.LayerListCount + 1, true);
			if (this.towerDataModule.IsPlayLevelMoveAnim)
			{
				this.layerListView.MovePanelToItemIndex(this.towerDataModule.GetLevelIndexByLevelId(this.towerDataModule.MoveLevelLastLevelId), 0f);
				Action onNoAnimMoveLevel2 = CS$<>8__locals1.onNoAnimMoveLevel;
				if (onNoAnimMoveLevel2 != null)
				{
					onNoAnimMoveLevel2();
				}
				CS$<>8__locals1.<UpdateLayerList>g__UpdateToCurLevel|0(true);
				EventArgsSetTowerBattleReadyData instance = Singleton<EventArgsSetTowerBattleReadyData>.Instance;
				instance.SetData(0, false, 0);
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_TowerDataMoudule_SetBattleReadyData, instance);
				return;
			}
			CS$<>8__locals1.<UpdateLayerList>g__UpdateToCurLevel|0(false);
		}

		private void PlayOpenAni()
		{
			this.challengeTrans.anchoredPosition = new Vector2(this.challengeTrans.anchoredPosition.x, -500f);
			this.backTrans.anchoredPosition = new Vector2(this.backTrans.anchoredPosition.x, -500f);
			this.rankTrans.anchoredPosition = new Vector2(this.rankTrans.anchoredPosition.x, -500f);
			this.rewardProgressTrans.anchoredPosition = new Vector2(-1080f, this.rewardProgressTrans.anchoredPosition.y);
			this.layerListView.transform.localScale = Vector3.one * 0.9f;
			Sequence sequence = DOTween.Sequence();
			float num = 0.2f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.layerListView.transform, Vector3.one, num));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosX(this.rewardProgressTrans, -476f, num, false));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(this.challengeTrans, 0f, num, false));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(this.backTrans, 0f, num, false));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(this.rankTrans, 0f, num, false));
		}

		private void SetButtonEnabled(bool isEnabled)
		{
			this.rankButton.enabled = isEnabled;
			this.returnBackButton.enabled = isEnabled;
			this.uiItemInfoButton.SetButtonEnable(isEnabled);
		}

		[GameTestMethod("爬塔", "下一层动画", "", 403)]
		private static void OpenTowerSnap()
		{
			TowerDataModule dataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
			EventArgsSetTowerBattleReadyData instance = Singleton<EventArgsSetTowerBattleReadyData>.Instance;
			instance.SetData(dataModule.CurTowerLevelId + 1, false, 1);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_TowerDataMoudule_SetBattleReadyData, instance);
			GameApp.View.OpenView(ViewName.TowerMainViewModule, null, 1, null, null);
		}

		[CompilerGenerated]
		private void <OnEventToNextTower>g__ToNextTower|40_1()
		{
			if (this.IsOpen())
			{
				this.RefreshAll();
			}
		}

		[CompilerGenerated]
		private void <OnLayerScrollSnapNearestFinished>g__NodeAniFinish|44_0()
		{
			Sequence sequence = DOTween.Sequence();
			float num = 0.2f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.towerTitleCG, 1f, num));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosX(this.rewardProgressTrans, -476f, num, false));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(this.challengeTrans, 0f, num, false));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(this.backTrans, 0f, num, false));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(this.rankTrans, 0f, num, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.isSnapAni = false;
			});
		}

		[SerializeField]
		private GameObject towerTitleObj;

		[SerializeField]
		private GameObject titleTopLayerObj;

		[SerializeField]
		private CustomText titleTopLayerText;

		[SerializeField]
		private UIItemInfoButton uiItemInfoButton;

		[SerializeField]
		private CustomButton rankButton;

		[SerializeField]
		private CustomButton returnBackButton;

		[SerializeField]
		private TowerLoopListView2 layerListView;

		[SerializeField]
		private TowerProgressCtr progressCtr;

		[SerializeField]
		private GameObject rewardProgressObj;

		[SerializeField]
		private ModuleCurrencyCtrl currencyCtrl;

		public RectTransform backTrans;

		public RectTransform challengeTrans;

		public RectTransform rankTrans;

		public RectTransform rewardProgressTrans;

		public CanvasGroup towerTitleCG;

		private readonly Dictionary<int, TowerLayerViewCtr> shopItemDic = new Dictionary<int, TowerLayerViewCtr>();

		private TowerLayerTopViewCtr topShopItem;

		private TowerDataModule towerDataModule;

		private readonly SequencePool layerSequencePool = new SequencePool();

		private readonly SequencePool challengeSequencePool = new SequencePool();

		private int lastLayerScrollSnapIndex;

		private bool isSnapAni;

		private TowerLayerViewCtr currentNode;
	}
}
