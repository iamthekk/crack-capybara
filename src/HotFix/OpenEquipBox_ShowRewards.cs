using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.Platfrom;
using LocalModels.Bean;
using Shop.Arena;
using UnityEngine;

namespace HotFix
{
	public class OpenEquipBox_ShowRewards : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_commonDataModule = GameApp.Data.GetDataModule(DataName.CommonDataModule);
			this.m_shopDataModule = GameApp.Data.GetDataModule(DataName.ShopDataModule);
			this.m_iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.m_propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.m_adDataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
			this.rewardObjectPool = LocalUnityObjctPool.Create(base.gameObject);
			this.rewardObjectPool.CreateCache("UIItemPool", this.rewardItemPrefab.gameObject);
			this.rewardItemPrefab.SetActive(false);
			this.btnTapClose.m_onClick = new Action(this.OnBtnCloseClick);
			this.Button_Skip.m_onClick = new Action(this.OnClickBtnSkip);
			this.Button_BuyOne.m_onClick = new Action(this.OnBtnItemBuyOneClick);
			this.Button_BuyTen.m_onClick = new Action(this.OnBtnItemBuyTenClick);
			if (this.titleText != null)
			{
				this.titleText.text = Singleton<LanguageManager>.Instance.GetInfoByID("Common_Reward");
			}
		}

		protected override void OnDeInit()
		{
			this.btnTapClose.m_onClick = null;
			this.Button_Skip.m_onClick = null;
			this.Button_BuyOne.m_onClick = null;
			this.Button_BuyTen.m_onClick = null;
			this.showItemSequencePool.Clear(false);
			this.RemoveAllItem();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			foreach (BoxDrawRewardItem boxDrawRewardItem in this.curRewardItem)
			{
				boxDrawRewardItem.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		private void OnBtnItemBuyOneClick()
		{
			this.OnBtnItemBuyClick(1);
		}

		private void OnBtnItemBuyTenClick()
		{
			this.OnBtnItemBuyClick(2);
		}

		private void OnBtnItemBuyClick(int drawTimesType)
		{
			if (this.m_shopSummonTable == null)
			{
				return;
			}
			int num = this.m_shopSummonTable.priceId;
			long num2 = (long)((drawTimesType == 2) ? this.m_shopSummonTable.tenPrice : this.m_shopSummonTable.singlePrice);
			long num3 = this.m_propDataModule.GetItemDataCountByid((ulong)((long)num));
			if (num3 < num2)
			{
				if (!((drawTimesType == 2) ? (this.m_shopSummonTable.tenPriceOrigin > 0) : (this.m_shopSummonTable.singlePriceOrigin > 0)))
				{
					GameApp.View.ShowItemNotEnoughTip(num, true);
					return;
				}
				long num4 = (long)((drawTimesType == 2) ? this.m_shopSummonTable.tenPriceOrigin : this.m_shopSummonTable.singlePriceOrigin);
				num = 2;
				num2 = num4;
				num3 = this.m_propDataModule.GetItemDataCountByid((ulong)((long)num));
				if (num3 < num2)
				{
					GameApp.View.ShowItemNotEnoughTip(num, true);
					return;
				}
			}
			if (!this.btnTapClose.gameObject.activeSelf)
			{
				return;
			}
			this.Button_Skip.gameObject.SetActiveSafe(false);
			this.Button_BuyOne.gameObject.SetActiveSafe(false);
			this.Button_BuyTen.gameObject.SetActiveSafe(false);
			this.m_drawCostId = num;
			if (this.m_drawCostId == 61 && drawTimesType == 1 && num3 > num2)
			{
				this.m_drawCostCount = (int)num3;
			}
			else
			{
				this.m_drawCostCount = (int)num2;
			}
			this.SendRequest(this.m_shopSummonTable.id, 2, drawTimesType);
		}

		private void SendRequest(int summonId, int costType, int drawType)
		{
			this.m_drawCostType = costType;
			NetworkUtils.Shop.ShopDoDrawRequest(summonId, costType, drawType, new Action<bool, ShopDoDrawResponse>(this.SendMessageCallback));
		}

		private void SendMessageCallback(bool isOk, ShopDoDrawResponse resp)
		{
			if (isOk)
			{
				OpenEquipBoxViewModule.OpenData openData = new OpenEquipBoxViewModule.OpenData();
				openData.boxId = this.m_shopSummonTable.boxId;
				openData.itemDatas = resp.CommonData.Reward.ToItemDataList();
				openData.CostType = this.m_drawCostType;
				openData.ChestType = this.m_chestType;
				openData.shopSummonId = this.m_shopSummonTable.id;
				openData.iapMainActivityType = this.iapMainActivityType;
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, null);
				this.PlayRewardAnimation(openData.itemDatas, this.PageEndCallback, openData);
				int num = 1120800 + this.m_shopSummonTable.boxId;
				if (this.iapMainActivityType == 2)
				{
					num = 1120899;
				}
				GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName(num), resp.CommonData.Reward, null, null, null, null);
				GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName(num), resp.CommonData.CostDto, null);
				string text = "";
				List<TGACommonItemInfo> list = new List<TGACommonItemInfo>();
				if (this.iapMainActivityType == 2)
				{
					IAPShopActivityData shopSUpActivityData = this.m_iapDataModule.GetShopSUpActivityData();
					if (shopSUpActivityData != null && shopSUpActivityData.shopActDetailDto != null && shopSUpActivityData.shopActDetailDto.SummonPoolItems != null)
					{
						for (int i = 0; i < shopSUpActivityData.shopActDetailDto.SummonPoolItems.Count; i++)
						{
							list.Add(new TGACommonItemInfo(shopSUpActivityData.shopActDetailDto.SummonPoolItems[i], 1L));
						}
					}
					if (resp.CommonData.Reward.Count > 1)
					{
						text = "up宝箱十连";
					}
					else
					{
						text = "up宝箱单抽";
					}
				}
				else if (this.m_shopSummonTable.boxId == 2)
				{
					if (this.m_drawCostType == 1)
					{
						GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(2), "REWARD ", "", resp.CommonData.Reward, null);
					}
					text = "冒险者补给箱";
				}
				else if (this.m_shopSummonTable.boxId == 3)
				{
					if (this.m_drawCostType == 1)
					{
						GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(3), "REWARD ", "", resp.CommonData.Reward, null);
					}
					text = "英雄补给箱";
				}
				GameTGACostCurrency gameTGACostCurrency;
				if (this.m_drawCostType == 1)
				{
					gameTGACostCurrency = GameTGACostCurrency.Ad;
				}
				else if (this.m_drawCostId == 0)
				{
					gameTGACostCurrency = GameTGACostCurrency.Free;
				}
				else if (this.m_drawCostId == 2)
				{
					gameTGACostCurrency = GameTGACostCurrency.Gem;
				}
				else
				{
					gameTGACostCurrency = GameTGACostCurrency.ChestKey;
				}
				GameApp.SDK.Analyze.Track_EquipmentBoxOpen(text, gameTGACostCurrency, this.m_drawCostCount, resp.CommonData.Reward, list);
			}
		}

		private void OnClickBtnSkip()
		{
			bool rememberTipState = this.m_commonDataModule.GetRememberTipState(RememberTipType.MainShopTabSkip);
			this.SkipSelectObj.SetActiveSafe(!rememberTipState);
			this.m_commonDataModule.SetRememberTipState(RememberTipType.MainShopTabSkip, !rememberTipState);
		}

		private void OnBtnCloseClick()
		{
			Action pageEndCallback = this.PageEndCallback;
			if (pageEndCallback == null)
			{
				return;
			}
			pageEndCallback();
		}

		public void PlayRewardAnimation(List<ItemData> rewards, Action callback, OpenEquipBoxViewModule.OpenData openData)
		{
			this.m_openData = openData;
			this.iapMainActivityType = this.m_openData.iapMainActivityType;
			this.m_chestType = openData.ChestType;
			this.PageEndCallback = callback;
			this.rewards = rewards;
			this.btnTapClose.gameObject.SetActive(false);
			if (rewards == null)
			{
				this.OnBtnCloseClick();
				return;
			}
			this.OnRefreshView();
			this.SetInitView();
			this.PlayAnim();
		}

		private void OnRefreshView()
		{
			this.SkipSelectObj.SetActiveSafe(this.m_commonDataModule.GetRememberTipState(RememberTipType.MainShopTabSkip));
			this.m_shopSummonTable = GameApp.Table.GetManager().GetShop_SummonModelInstance().GetElementById(this.m_openData.shopSummonId);
			if (this.m_shopSummonTable == null)
			{
				return;
			}
			this.m_showBuyTen = false;
			this.m_showBtnItemBuy = false;
			this.Button_Skip.gameObject.SetActiveSafe(false);
			this.Button_BuyOne.gameObject.SetActiveSafe(false);
			this.Button_BuyTen.gameObject.SetActiveSafe(false);
			if (this.m_openData.CostType == 1)
			{
				this.Button_BuyOne.gameObject.SetActiveSafe(false);
				this.Button_BuyTen.gameObject.SetActiveSafe(false);
				return;
			}
			int num = this.m_shopSummonTable.priceId;
			long num2 = ((num > 0) ? this.m_propDataModule.GetItemDataCountByid((ulong)((long)num)) : 0L);
			if (this.m_chestType == eEquipChestType.DiamondChest)
			{
				this.m_showBuyTen = true;
				ItemData itemData = new ItemData();
				itemData.SetID(this.m_shopSummonTable.priceId);
				itemData.SetCount((long)this.m_shopSummonTable.tenPrice);
				if (this.m_shopSummonTable.singlePriceOrigin > 0 && num2 < (long)this.m_shopSummonTable.tenPrice)
				{
					itemData.SetID(2);
					itemData.SetCount((long)this.m_shopSummonTable.tenPriceOrigin);
					this.Com_BuyTenCostItem.SetData(itemData);
					return;
				}
				this.Com_BuyTenCostItem.SetData(itemData, num2, itemData.TotalCount);
				return;
			}
			else
			{
				this.m_showBuyTen = this.m_shopSummonTable.priceId > 0 && this.m_shopSummonTable.tenPrice > 0 && num2 >= (long)this.m_shopSummonTable.tenPrice;
				this.m_showBtnItemBuy = !this.m_showBuyTen && (this.m_shopSummonTable.priceId > 0 || this.m_shopSummonTable.singlePriceOrigin > 0);
				if (this.m_showBuyTen)
				{
					long num3 = (long)this.m_shopSummonTable.tenPrice;
					this.Com_BuyTenCostItem.SetData(num, num2, num3);
					return;
				}
				long num4 = (long)this.m_shopSummonTable.singlePrice;
				if (this.m_shopSummonTable.quickDraw > 0 && num2 > 0L && num2 < 10L)
				{
					num4 = num2;
				}
				if (num > 0 && num2 < num4 && this.m_shopSummonTable.singlePriceOrigin > 0)
				{
					this.Button_BuyOne.gameObject.SetActiveSafe(false);
					num = 2;
					num4 = (long)this.m_shopSummonTable.singlePriceOrigin;
					num2 = this.m_propDataModule.GetItemDataCountByid((ulong)((long)num));
					this.Com_BuyOneCostItem.SetData(num, num4);
					return;
				}
				this.Com_BuyOneCostItem.SetData(num, num2, num4);
				return;
			}
		}

		private void SetInitView()
		{
			this.bgGameObject.SetActive(false);
			this.multiLightBg.alpha = 0f;
			this.multiFlashWhiteEffect.gameObject.SetActive(false);
		}

		public void PlayAnim()
		{
			if (this.rewards.Count == 1)
			{
				this.ShowSingle();
				return;
			}
			this.ShowMulti();
		}

		private async void ShowSingle()
		{
			this.isShowItemOneByOne = false;
			this.showItemSequencePool.Clear(false);
			this.bgGameObject.SetActive(true);
			await this.InstantiateItem(this.singleParent);
			BoxDrawRewardItem uiItem = this.curRewardItem[0];
			uiItem.transform.localScale = Vector3.one * 1.2f;
			foreach (BoxDrawRewardItem boxDrawRewardItem in this.curRewardItem)
			{
				boxDrawRewardItem.SetActive(true);
				boxDrawRewardItem.ShowQualityEffect();
			}
			TweenCallback <>9__2;
			Action <>9__1;
			TweenSettingsExtensions.AppendCallback(this.showItemSequencePool.Get(), delegate
			{
				BoxDrawRewardItem boxDrawRewardItem2 = this.curRewardItem[0];
				ItemData itemData = this.rewards[0];
				if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID)
					.quality >= 5)
				{
					OpenEquipBox_ShowRewards <>4__this = this;
					List<BoxDrawRewardItem> list = this.curRewardItem;
					int num = 0;
					Action action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate
						{
							Sequence sequence = this.showItemSequencePool.Get();
							TweenSettingsExtensions.AppendInterval(sequence, 0.3f);
							TweenCallback tweenCallback;
							if ((tweenCallback = <>9__2) == null)
							{
								tweenCallback = (<>9__2 = delegate
								{
									this.ShowButtons();
								});
							}
							TweenSettingsExtensions.AppendCallback(sequence, tweenCallback);
							TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(uiItem.transform, Vector3.zero, 0.6f, false), 12));
							TweenSettingsExtensions.Join(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(uiItem.transform, Vector3.one, 0.6f), 12));
							TweenSettingsExtensions.AppendCallback(sequence, null);
						});
					}
					<>4__this.ShowItem(list, num, action, true);
					return;
				}
				boxDrawRewardItem2.ShowItem(new Action(this.ShowItemOneByOne));
			});
		}

		private async void ShowMulti()
		{
			this.isShowItemOneByOne = false;
			this.showItemSequencePool.Clear(false);
			this.bgGameObject.SetActive(true);
			this.multiLightBg.alpha = 1f;
			await this.InstantiateItem(this.multiItemParent);
			Sequence sequence = this.showItemSequencePool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, 0f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.multiLightBg.alpha = 0f;
				this.multiFlashWhiteEffect.gameObject.SetActive(true);
				this.multiFlashWhiteEffect.Stop();
				this.multiFlashWhiteEffect.Play();
			});
			await TaskExpand.Delay(150);
			foreach (BoxDrawRewardItem boxDrawRewardItem in this.curRewardItem)
			{
				boxDrawRewardItem.SetActive(true);
				boxDrawRewardItem.ShowQualityEffect();
			}
			bool flag = false;
			for (int i = 0; i < this.rewards.Count; i++)
			{
				ItemData itemData = this.rewards[i];
				if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID)
					.quality < 5)
				{
					flag = true;
				}
			}
			if (flag)
			{
				await TaskExpand.Delay(800);
				for (int j = 0; j < this.curRewardItem.Count; j++)
				{
					BoxDrawRewardItem boxDrawRewardItem2 = this.curRewardItem[j];
					ItemData itemData2 = this.rewards[j];
					if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData2.ID)
						.quality < 5)
					{
						boxDrawRewardItem2.ShowItem(new Action(this.ShowItemOneByOne));
					}
				}
			}
			else
			{
				this.ShowItemOneByOne();
			}
		}

		private void ShowItemOneByOne()
		{
			if (this.isShowItemOneByOne)
			{
				return;
			}
			this.isShowItemOneByOne = true;
			this.ShowItem(this.curRewardItem, 0, new Action(this.ShowButtons), true);
		}

		private void ShowItem(List<BoxDrawRewardItem> itemList, int itemIndex, Action onComplete, bool skipLowerQuality)
		{
			if (itemIndex >= itemList.Count)
			{
				Action onComplete2 = onComplete;
				if (onComplete2 == null)
				{
					return;
				}
				onComplete2();
				return;
			}
			else
			{
				BoxDrawRewardItem boxDrawRewardItem = itemList[itemIndex];
				int itemIndex2 = itemIndex;
				itemIndex = itemIndex2 + 1;
				if (!skipLowerQuality)
				{
					boxDrawRewardItem.ShowItem(delegate
					{
						this.ShowItem(itemList, itemIndex, onComplete, skipLowerQuality);
					});
					return;
				}
				ItemData itemData = boxDrawRewardItem.ItemData;
				if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID)
					.quality >= 5)
				{
					boxDrawRewardItem.ShowItem(delegate
					{
						this.ShowItem(itemList, itemIndex, onComplete, skipLowerQuality);
					});
					return;
				}
				this.ShowItem(itemList, itemIndex, onComplete, skipLowerQuality);
				return;
			}
		}

		private void ShowButtons()
		{
			this.btnTapClose.gameObject.SetActiveSafe(true);
			this.Button_Skip.gameObject.SetActiveSafe(true);
			this.Button_BuyTen.gameObject.SetActiveSafe(this.m_showBuyTen);
			this.Button_BuyOne.gameObject.SetActiveSafe(this.m_showBtnItemBuy);
		}

		private async Task InstantiateItem(Transform parent)
		{
			this.RemoveAllItem();
			List<Task> list = new List<Task>();
			foreach (ItemData itemData in this.rewards)
			{
				BoxDrawRewardItem component = this.rewardObjectPool.DeQueue("UIItemPool").GetComponent<BoxDrawRewardItem>();
				this.curRewardItem.Add(component);
				Transform transform = component.transform;
				transform.SetParent(parent);
				transform.localScale = this.rewardItemPrefab.transform.localScale;
				transform.localPosition = Vector3.zero;
				transform.SetAsLastSibling();
				component.Init();
				list.Add(component.SetData(itemData, this.bgEffectPoint, this.fgEffectPoint));
			}
			await Task.WhenAll(list);
		}

		private void RemoveAllItem()
		{
			foreach (BoxDrawRewardItem boxDrawRewardItem in this.curRewardItem)
			{
				boxDrawRewardItem.DeInit();
				this.rewardObjectPool.EnQueue("UIItemPool", boxDrawRewardItem.gameObject);
			}
			this.curRewardItem.Clear();
		}

		private const string UIItemPoolKey = "UIItemPool";

		public CustomButton btnTapClose;

		[SerializeField]
		private GameObject bgGameObject;

		[SerializeField]
		private GameObject titleGameObject;

		[SerializeField]
		private CustomText titleText;

		[SerializeField]
		private BoxDrawRewardItem rewardItemPrefab;

		[SerializeField]
		private CustomGridLayoutGroup gridLayout;

		[SerializeField]
		private Transform bgEffectPoint;

		[SerializeField]
		private Transform fgEffectPoint;

		[SerializeField]
		private Transform singleParent;

		[SerializeField]
		private Transform multiItemParent;

		[SerializeField]
		private CanvasGroup multiLightBg;

		[SerializeField]
		private ParticleSystem multiFlashWhiteEffect;

		[SerializeField]
		private CustomButton Button_Skip;

		[SerializeField]
		private GameObject SkipSelectObj;

		[Header("再来一次")]
		public CustomButton Button_BuyOne;

		public CommonCostItem Com_BuyOneCostItem;

		public CustomButton Button_BuyTen;

		public CommonCostItem Com_BuyTenCostItem;

		private LocalUnityObjctPool rewardObjectPool;

		private readonly List<BoxDrawRewardItem> curRewardItem = new List<BoxDrawRewardItem>();

		private readonly SequencePool showItemSequencePool = new SequencePool();

		private List<ItemData> rewards;

		private CommonDataModule m_commonDataModule;

		protected IAPDataModule m_iapDataModule;

		protected ShopDataModule m_shopDataModule;

		protected PropDataModule m_propDataModule;

		protected AdDataModule m_adDataModule;

		private Shop_Summon m_shopSummonTable;

		private eEquipChestType m_chestType;

		private int m_drawCostType;

		private int m_drawCostId;

		private int m_drawCostCount;

		private OpenEquipBoxViewModule.OpenData m_openData;

		private bool m_showBuyTen;

		private bool m_showBtnItemBuy;

		private int iapMainActivityType;

		private Action PageEndCallback;

		private bool isShowItemOneByOne;
	}
}
