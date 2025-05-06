using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Proto.Pay;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class IAPFundRogueDungeonNodeCtrl : IAPFundCtrlBase
	{
		protected override void OnInit()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnRefreshLevelFund));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPLevelFundRewards, new HandlerEvent(this.OnRefreshLevelFund));
			this.mDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.LoopList.InitListView(-1, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.ObjEmpty.SetActive(false);
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnRefreshLevelFund));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPLevelFundRewards, new HandlerEvent(this.OnRefreshLevelFund));
			foreach (KeyValuePair<int, IAPFundItem> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
		}

		public override void OnShow()
		{
			base.OnShow();
			this.playui = true;
			this.m_seqPool.Clear(false);
			this.GroupData = this.mDataModule.LevelFund.GetCurrentFundGroup(IAPLevelFundGroupKind.RogueDungeonFloor);
			this.RefreshUI();
			int showIndex = this.GetShowIndex();
			if (showIndex > 0)
			{
				this.LoopList.MovePanelToItemIndex(showIndex, 0f);
			}
		}

		public override void OnHide()
		{
			base.OnHide();
			this.m_seqPool.Clear(false);
			this.playui = false;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIRechargeGift_Refresh, null);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (this.DataList.Count == 0)
			{
				return null;
			}
			if (index < 0 || index >= this.DataList.Count + 1 + 1)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem;
			if (index < 1)
			{
				loopListViewItem = listView.NewListViewItem("IAPFundEmptyTopItem");
				IAPFundEmptyItem component = loopListViewItem.GetComponent<IAPFundEmptyItem>();
				bool flag = this.DataList[0].CheckArrive();
				if (component)
				{
					component.SetData(!flag, true);
				}
				return loopListViewItem;
			}
			index--;
			if (index >= this.DataList.Count)
			{
				loopListViewItem = listView.NewListViewItem("IAPFundEmptyBottomItem");
				IAPFundEmptyItem component2 = loopListViewItem.GetComponent<IAPFundEmptyItem>();
				List<IAPLevelFundData> dataList = this.DataList;
				bool flag2 = dataList[dataList.Count - 1].CheckArrive();
				if (component2)
				{
					component2.SetData(!flag2, flag2);
				}
				return loopListViewItem;
			}
			IAPLevelFundData iaplevelFundData = this.DataList[index];
			loopListViewItem = listView.NewListViewItem("IAPFundItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			IAPFundItem iapfundItem = this.TryGetUI(instanceID);
			if (iapfundItem == null)
			{
				iapfundItem = this.TryAddUI(instanceID, loopListViewItem, loopListViewItem.GetComponent<IAPFundItem>());
			}
			if (iapfundItem != null)
			{
				IAPFundItem iapfundItem2 = iapfundItem;
				iapfundItem2.SetData(iaplevelFundData, this.GroupData, new Action(this.OnCollectAll));
				iapfundItem2.SetActive(true);
				iapfundItem2.Refresh(this.m_seqPool);
			}
			return loopListViewItem;
		}

		private IAPFundItem TryGetUI(int key)
		{
			IAPFundItem iapfundItem;
			if (this.mUICtrlDic.TryGetValue(key, out iapfundItem))
			{
				return iapfundItem;
			}
			return null;
		}

		private IAPFundItem TryAddUI(int key, LoopListViewItem2 loopitem, IAPFundItem ui)
		{
			ui.Init();
			IAPFundItem iapfundItem;
			if (this.mUICtrlDic.TryGetValue(key, out iapfundItem))
			{
				if (iapfundItem == null)
				{
					iapfundItem = ui;
					this.mUICtrlDic[key] = ui;
				}
				return ui;
			}
			this.mUICtrlDic.Add(key, ui);
			return ui;
		}

		private void OnRefreshLevelFund(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshUI();
		}

		public void RefreshUI()
		{
			if (base.gameObject == null)
			{
				return;
			}
			if (this.GroupData == null)
			{
				return;
			}
			this.Button_Buy.SetData(this.GroupData.PurchaseID, null, new Action<int>(this.OnBuyLevelFund), null, null, null);
			this.Text_TotalDiamonds.text = this.GroupData.GetTotalKeyCount(28).ToString();
			this.DataList = new List<IAPLevelFundData>();
			this.DataList.AddRange(this.GroupData.GetList());
			this.LoopList.SetListItemCount(this.DataList.Count + 1 + 1, true);
			this.LoopList.RefreshAllShownItem();
			if (this.GroupData.HasBuy)
			{
				this.btnBuyGray.SetUIGray();
			}
			else
			{
				this.btnBuyGray.Recovery();
			}
			for (int i = 0; i < this.buyHide.Count; i++)
			{
				this.buyHide[i].SetActiveSafe(!this.GroupData.HasBuy);
			}
			for (int j = 0; j < this.buyShow.Count; j++)
			{
				this.buyShow[j].SetActiveSafe(this.GroupData.HasBuy);
			}
			this.ObjEmpty.SetActive(this.DataList.Count <= 0);
		}

		private void OnBuyLevelFund(int purchaseId)
		{
			if (this.GroupData == null || this.GroupData.HasBuy)
			{
				return;
			}
			GameApp.Purchase.Manager.Buy(purchaseId, 0, "", delegate(bool isOk)
			{
				if (isOk)
				{
					this.RefreshUI();
				}
			}, null);
		}

		private void OnCloseSelf()
		{
			GameApp.View.CloseView(ViewName.IAPFundRogueDungeonViewModule, null);
		}

		private void OnCollectAll()
		{
			List<int> canCollectList = this.GroupData.GetCanCollectList();
			if (canCollectList.Count <= 0)
			{
				return;
			}
			List<int> preCollectedFree = this.GroupData.FreeCollectedRewardedList;
			List<int> preCollectedPay = this.GroupData.PayCollectedRewardedList;
			NetworkUtils.Purchase.LevelFundRewardRequest(canCollectList, delegate(bool result, LevelFundRewardResponse resp)
			{
				if (result)
				{
					DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
					List<int> freeCollectedRewardedList = this.GroupData.FreeCollectedRewardedList;
					foreach (int num in preCollectedFree)
					{
						if (freeCollectedRewardedList.Contains(num))
						{
							freeCollectedRewardedList.Remove(num);
						}
					}
					List<int> payCollectedRewardedList = this.GroupData.PayCollectedRewardedList;
					foreach (int num2 in preCollectedPay)
					{
						if (payCollectedRewardedList.Contains(num2))
						{
							payCollectedRewardedList.Remove(num2);
						}
					}
					GameApp.SDK.Analyze.Track_InvestGet("地牢基金", this.GroupData.GroupID, this.GroupData.Level, resp.CommonData.Reward, this.GroupData.PurchaseID, freeCollectedRewardedList, payCollectedRewardedList);
				}
			});
		}

		private int GetShowIndex()
		{
			if (this.GroupData != null)
			{
				for (int i = 0; i < this.DataList.Count; i++)
				{
					IAPLevelFundData iaplevelFundData = this.DataList[i];
					bool flag = iaplevelFundData.freeState == 1;
					bool flag2 = iaplevelFundData.payState == 1;
					if (this.GroupData.HasBuy)
					{
						if (!flag || !flag2)
						{
							return i;
						}
					}
					else if (!flag)
					{
						return i;
					}
				}
			}
			return 0;
		}

		public PurchaseButtonCtrl Button_Buy;

		public UIGrays btnBuyGray;

		public LoopListView2 LoopList;

		public GameObject ObjEmpty;

		public CustomText Text_TotalDiamonds;

		public List<GameObject> buyHide;

		public List<GameObject> buyShow;

		private IAPLevelFundGroup GroupData;

		private List<IAPLevelFundData> DataList = new List<IAPLevelFundData>();

		private Dictionary<int, IAPFundItem> mUICtrlDic = new Dictionary<int, IAPFundItem>();

		private const int TopEmptyCount = 1;

		private const int BottomEmptyCount = 1;

		private IAPDataModule mDataModule;

		private SequencePool m_seqPool = new SequencePool();

		private bool playui;
	}
}
