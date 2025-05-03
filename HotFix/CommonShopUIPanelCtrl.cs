using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.IntegralShop;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class CommonShopUIPanelCtrl : ShopUIPanelCtrlBase
	{
		public CommonShopUIPanelCtrl(ShopType shopType)
		{
			base.ThisShopType = shopType;
		}

		protected override void OnInit()
		{
			base.OnInit();
			this.isInit = true;
			this.shopDataModule = GameApp.Data.GetDataModule(DataName.ShopDataModule);
			this.currencyUI.Init();
			this.shopDataModule.GetShopConfig(base.ThisShopType, out this.shopConfig);
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(this.shopConfig.currencyID);
			this.currencyUI.Image.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
			this.currencyUI.CurrencyType = (CurrencyType)this.shopConfig.currencyID;
			this.currencyUI.OnClick = new Action<CurrencyType>(this.OnClickCurrencyUI);
			this.currencyAnim.Init();
			this.gridView.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
			this.uiItemInfoButton.Init();
			this.uiItemInfoButton.SetOnClick(new Action(this.RefreshShop));
			this.uiItemInfoButton.SetItemIcon(2);
			this.uiItemInfoButton.SetInfoTextByLanguageId(2405);
			this.refreshShopTimer = new Timer();
			this.shopItemDic = new Dictionary<int, CommonShopUIItem>();
			this.shopContentAnim = new SequencePool();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.refreshShopTimer.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnDeInit()
		{
			base.OnDeInit();
			this.isInit = false;
			this.uiItemInfoButton.DeInit();
			this.currencyUI.OnClick = null;
			this.UnRegisterTimeRefresh();
		}

		protected override void OnShow()
		{
			base.OnShow();
			this.UpdateAllInfo();
			this.CheckShopInfo(delegate(bool isUpdate)
			{
				if (isUpdate)
				{
					this.UpdateAllInfo();
				}
			});
		}

		public override void PlayRefreshShopAnim()
		{
			this.shopContentAnim.Clear(false);
			for (int i = 0; i < this.gridView.ItemTotalCount; i++)
			{
				LoopGridViewItem shownItemByItemIndex = this.gridView.GetShownItemByItemIndex(i);
				if (!(shownItemByItemIndex == null) || shownItemByItemIndex.CachedRectTransform.childCount >= 0)
				{
					DxxTools.UI.DoScaleAnim(this.shopContentAnim.Get(), shownItemByItemIndex.CachedRectTransform.GetChild(0), 0f, 1f, 0.03f * (float)i, 0.2f, 0);
				}
			}
		}

		private LoopGridViewItem OnGetItemByIndex(LoopGridView view, int index, int row, int column)
		{
			if (index < 0 || index >= this.shopItemConfig.Count)
			{
				return null;
			}
			LoopGridViewItem loopGridViewItem = this.gridView.NewListViewItem("CommonShopItem");
			int instanceID = loopGridViewItem.gameObject.GetInstanceID();
			CommonShopUIItem component;
			this.shopItemDic.TryGetValue(instanceID, out component);
			if (component == null)
			{
				component = loopGridViewItem.gameObject.GetComponent<CommonShopUIItem>();
				component.Init();
				component.OnClaimReward = new Action(this.OnBuyItem);
				this.shopItemDic[instanceID] = component;
			}
			component.RefreshData(this.shopItemConfig[index]);
			return loopGridViewItem;
		}

		private void OnBuyItem()
		{
			for (int i = 0; i < this.gridView.ItemTotalCount; i++)
			{
				this.gridView.RefreshItemByItemIndex(i);
			}
			this.SetCurrency(true);
		}

		private void CheckShopInfo(Action<bool> callback)
		{
			if (!this.shopDataModule.HasShopInfo(base.ThisShopType))
			{
				this.OnGetShopInfoRequest(delegate
				{
					Action<bool> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true);
				});
				return;
			}
			Action<bool> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(false);
		}

		private void OnGetShopInfoRequest(Action callback)
		{
			NetworkUtils.Shop.IntegralShopGetInfoRequest(base.ThisShopType, delegate(bool res, IntegralShopGetInfoResponse response)
			{
				if (!this.isInit)
				{
					return;
				}
				if (!res)
				{
					return;
				}
				Action callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2();
			});
		}

		private void RefreshShop()
		{
			int num;
			this.shopDataModule.GetShopRefreshInfo(base.ThisShopType, out num);
			NetworkUtils.Shop.IntegralShopRefreshRequest(base.ThisShopType, 2, num, delegate(bool res, IntegralShopRefreshResponse response)
			{
				if (!this.isInit)
				{
					return;
				}
				if (!res)
				{
					return;
				}
				this.UpdateAllInfo();
				this.PlayRefreshShopAnim();
			});
		}

		private async void OnClickCurrencyUI(CurrencyType obj)
		{
			ViewJumpType viewJumpType = ViewJumpType.MainBattle;
			object obj2 = null;
			switch (base.ThisShopType)
			{
			case ShopType.Guild:
				viewJumpType = ViewJumpType.MainCityGuild;
				break;
			case ShopType.BlackMarket:
				obj2 = new IAPShopViewModule.OpenData(IAPShopJumpTabData.CreateDiamonds(IAPDiamondsType.DiamondsPack));
				viewJumpType = ViewJumpType.MainShop;
				break;
			case ShopType.Integral:
				obj2 = new IAPShopViewModule.OpenData(IAPShopJumpTabData.CreateDiamonds(IAPDiamondsType.DayPack));
				viewJumpType = ViewJumpType.MainShop;
				break;
			case ShopType.ManaCrystal:
				viewJumpType = ViewJumpType.MainCityGuild;
				break;
			}
			if (Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(viewJumpType, null, true))
			{
				await Singleton<ViewJumpCtrl>.Instance.JumpTo(viewJumpType, obj2);
				GameApp.View.CloseView(ViewName.MainShopViewModule, null);
			}
		}

		private void UpdateAllInfo()
		{
			this.shopItemConfig = this.shopDataModule.GetShopItemsConfig(base.ThisShopType, GoodsRefreshType.None);
			this.RegisterTimeRefresh();
			this.SetShopName();
			this.SetRefreshInfo();
			this.SetCurrency(false);
			this.SetShopList();
		}

		private void RegisterTimeRefresh()
		{
			this.refreshShopTimer.OnInit();
			this.refreshShopTimer.m_onFinished = delegate(Timer timer)
			{
				this.UpdateRefreshTime();
				this.refreshShopTimer.Play(1f);
			};
			this.refreshShopTimer.Play(1f);
			this.UpdateRefreshTime();
		}

		private void UnRegisterTimeRefresh()
		{
			this.refreshShopTimer.Stop();
			this.refreshShopTimer.OnDeInit();
		}

		private void UpdateRefreshTime()
		{
			long num;
			bool refreshCountDownTime = this.shopDataModule.GetRefreshCountDownTime(base.ThisShopType, GoodsRefreshType.Day, out num);
			this.SetRefreshTime(num);
			if (refreshCountDownTime && num <= 0L)
			{
				this.UnRegisterTimeRefresh();
				string text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID("2408"), Array.Empty<object>());
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("17");
				DxxTools.UI.OpenPopCommon(text, delegate(int id)
				{
					this.OnGetShopInfoRequest(delegate
					{
						this.UpdateAllInfo();
						this.PlayRefreshShopAnim();
					});
				}, string.Empty, infoByID, string.Empty, false, 2);
			}
		}

		private void SetShopList()
		{
			this.gridView.SetListItemCount(this.shopItemConfig.Count, true);
			this.gridView.RefreshAllShownItem();
			this.gridView.MovePanelToItemByIndex(0, 0f, (float)(-(float)this.gridView.Padding.top));
		}

		private void SetCurrency(bool playAnim)
		{
			long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)this.shopConfig.currencyID));
			if (playAnim)
			{
				this.currencyAnim.JumpToTargetValue(itemDataCountByid);
				return;
			}
			this.currencyUI.SetText(DxxTools.FormatNumber(itemDataCountByid));
		}

		private void SetShopName()
		{
			IntegralShop_data integralShop_data;
			if (!this.shopDataModule.GetShopConfig(base.ThisShopType, out integralShop_data))
			{
				this.shopNameText.text = string.Empty;
			}
			this.shopNameText.SetText(int.Parse(integralShop_data.NameID));
		}

		private void SetRefreshTime(long showTime)
		{
			string time = Singleton<LanguageManager>.Instance.GetTime(showTime);
			this.refreshTimeText.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(2924, new object[] { time });
		}

		private void SetRefreshInfo()
		{
			int num;
			bool shopRefreshInfo = this.shopDataModule.GetShopRefreshInfo(base.ThisShopType, out num);
			bool flag = num <= 0;
			this.uiItemInfoButton.SetActive(shopRefreshInfo);
			this.cannotRefresh.gameObject.SetActive(!shopRefreshInfo);
			if (shopRefreshInfo)
			{
				if (flag)
				{
					this.uiItemInfoButton.SetCountTextByLanguageId(101, true);
					return;
				}
				this.uiItemInfoButton.SetCountText(DxxTools.FormatNumber((long)num), false);
			}
		}

		[SerializeField]
		private LoopGridView gridView;

		[SerializeField]
		private CurrencyUICtrl currencyUI;

		[SerializeField]
		private TextJumpChangeCtrl currencyAnim;

		[SerializeField]
		private CustomText refreshTimeText;

		[SerializeField]
		private GameObject cannotRefresh;

		[SerializeField]
		private CustomText shopNameText;

		[SerializeField]
		private RectTransform shopAnimPos;

		[SerializeField]
		private RectTransform titleAnimPos;

		[SerializeField]
		private UIItemInfoButton uiItemInfoButton;

		public ShopDataModule shopDataModule;

		public IntegralShop_data shopConfig;

		private Dictionary<int, CommonShopUIItem> shopItemDic = new Dictionary<int, CommonShopUIItem>();

		public List<IntegralShop_goods> shopItemConfig = new List<IntegralShop_goods>();

		private Timer refreshShopTimer = new Timer();

		public SequencePool shopContentAnim = new SequencePool();

		private bool isInit;
	}
}
