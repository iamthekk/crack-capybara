using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class PushGiftItemBase : CustomBehaviour
	{
		protected override void OnInit()
		{
			this._isOk = false;
			this.loopListView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("Item_Default");
			UIItem component = loopListViewItem.GetComponent<UIItem>();
			if (!loopListViewItem.IsInitHandlerCalled)
			{
				component.Init();
				loopListViewItem.IsInitHandlerCalled = true;
			}
			ItemData itemData = this._pushGiftData.Items[index];
			component.SetData(itemData.ToPropData());
			component.OnRefresh();
			component.SetActive(true);
			return loopListViewItem;
		}

		public void SetData(PushGiftData data, Action onClose)
		{
			this._pushGiftData = data;
			this.OnClose = onClose;
			if (this.loopListView.ItemTotalCount == data.Items.Count)
			{
				this.loopListView.RefreshAllShowItems();
			}
			else
			{
				this.loopListView.SetListItemCount(data.Items.Count, true);
			}
			this.timeObj.SetActiveSafe(data.PushConfig.type == 0 && data.EndTime > DxxTools.Time.ServerTimestamp);
			this.textTime2.text = Singleton<LanguageManager>.Instance.GetInfoByID("purchase_time_before");
			if (!string.IsNullOrEmpty(this._pushGiftData.PushConfig.Des))
			{
				this.textDes.text = Singleton<LanguageManager>.Instance.GetInfoByID(this._pushGiftData.PushConfig.Des);
			}
			else
			{
				this.textDes.text = "";
			}
			if (this._pushGiftData.PurchaseConfig.valueBet != 0)
			{
				this.discountObj.SetActiveSafe(true);
				this.textDiscount1.text = Singleton<LanguageManager>.Instance.GetInfoByID("discountStrength", new object[] { this._pushGiftData.PurchaseConfig.valueBet * 100 });
			}
			else
			{
				this.discountObj.SetActiveSafe(false);
			}
			this.payButtonCtrl.SetData(this._pushGiftData.ConfigId, "", new Action<bool>(this.OnPaySuccess), new Action(this.OnCloseReward));
		}

		protected override void OnDeInit()
		{
		}

		private void OnPaySuccess(bool isOk)
		{
			if (isOk)
			{
				this._isOk = true;
			}
		}

		private void OnClickClose()
		{
			Action onClose = this.OnClose;
			if (onClose == null)
			{
				return;
			}
			onClose();
		}

		private void OnCloseReward()
		{
			if (!this._isOk)
			{
				return;
			}
			if (this._pushGiftData.PushConfig.type == 1)
			{
				EventArgsBuySupplyGiftSuccess instance = Singleton<EventArgsBuySupplyGiftSuccess>.Instance;
				instance.ConfigId = this._pushGiftData.PushConfig.conditionParams;
				GameApp.Event.DispatchNow(null, 251, instance);
			}
			GameApp.Event.DispatchNow(null, 249, null);
			this.OnClickClose();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this._pushGiftData.PushConfig.type == 0)
			{
				long num = this._pushGiftData.EndTime - DxxTools.Time.ServerTimestamp;
				long num2 = ((num < 0L) ? 0L : num);
				this.textTime.text = DxxTools.FormatTime(num2);
				if (num <= 0L)
				{
					this.timeObj.SetActiveSafe(false);
					if (this._pushGiftData.IsPop)
					{
						this.OnClickClose();
					}
					GameApp.Event.DispatchNow(this, 248, null);
				}
			}
		}

		public CustomText textDes;

		public PurchaseButtonCtrl payButtonCtrl;

		public GameObject timeObj;

		public CustomText textTime;

		public CustomText textTime2;

		public LoopListView2 loopListView;

		public CustomText textDiscount1;

		public GameObject discountObj;

		private PushGiftData _pushGiftData;

		private Action OnClose;

		private bool _isOk;
	}
}
