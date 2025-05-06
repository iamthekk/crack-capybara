using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Shop.Arena;
using UnityEngine;

namespace HotFix
{
	public abstract class MainShopEquipChestBase : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.shopDataModule = GameApp.Data.GetDataModule(DataName.ShopDataModule);
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.adDataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
			this.shopEquipActivityTable = this.shopDataModule.GetEquipChestTableByType(this.chestType);
			this.shopSummonTable = GameApp.Table.GetManager().GetShop_SummonModelInstance().GetElementById(this.shopEquipActivityTable.id);
			this.btnFreeBuy.m_onClick = new Action(this.OnBtnFreeBuyClick);
			this.btnAdBuy.m_onClick = new Action(this.OnBtnAdBuyClick);
			this.btnItemBuyOne.m_onClick = new Action(this.OnBtnItemBuyOneClick);
			this.btnItemBuyTen.m_onClick = new Action(this.OnBtnItemBuyTenClick);
			this.btnAdRedNode.Value = 0;
			this.btnFreeRedNode.Value = 0;
			if (this.btnOneRedNode != null)
			{
				this.btnOneRedNode.gameObject.SetActiveSafe(false);
			}
			if (this.btnTenRedNode != null)
			{
				this.btnTenRedNode.gameObject.SetActiveSafe(false);
			}
			this.btnProbability.m_onClick = new Action(this.OnClickProbability);
		}

		protected override void OnDeInit()
		{
			this.btnFreeBuy.m_onClick = null;
			this.btnAdBuy.m_onClick = null;
			this.btnItemBuyOne.m_onClick = null;
			this.btnItemBuyTen.m_onClick = null;
			this.btnProbability.m_onClick = null;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.UpdateCountDown();
		}

		public virtual void SetData()
		{
			this.UpdateInfo();
			this.UpdateCost();
		}

		protected virtual void UpdateInfo()
		{
			this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.shopEquipActivityTable.nameId ?? "");
			this.txtDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.shopEquipActivityTable.descId ?? "");
			this.txtMiniPity.gameObject.SetActive(this.shopSummonTable.miniPityCount > 0);
			this.txtHardPity.gameObject.SetActive(this.shopSummonTable.hardPityCount > 0);
			this.pityCountNode.gameObject.SetActive(this.shopSummonTable.miniPityCount > 0 || this.shopSummonTable.hardPityCount > 0);
			this.txtTime.gameObject.SetActive(this.shopSummonTable.freeTimes > 0);
			if (this.shopSummonTable.miniPityCount > 0)
			{
				int shopDrawMiniCount = this.iapDataModule.GetShopDrawMiniCount(this.shopSummonTable.id);
				int num = Mathf.Max(0, this.shopSummonTable.miniPityCount - shopDrawMiniCount);
				this.txtMiniPity.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.shopEquipActivityTable.miniPityDesc, new object[] { num });
			}
			if (this.shopSummonTable.hardPityCount > 0)
			{
				int shopDrawHardCount = this.iapDataModule.GetShopDrawHardCount(this.shopSummonTable.id);
				int num2 = Mathf.Max(0, this.shopSummonTable.hardPityCount - shopDrawHardCount);
				this.txtHardPity.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.shopEquipActivityTable.hardPityDesc, new object[] { num2 });
			}
		}

		protected virtual void UpdateCountDown()
		{
			if (this.txtTime.gameObject.activeSelf)
			{
				long serverTimestamp = DxxTools.Time.ServerTimestamp;
				long num = this.iapDataModule.ShopFreeDrawResetTimestamp - serverTimestamp;
				if (num < 0L)
				{
					num = 0L;
				}
				string text = DxxTools.FormatFullTimeWithDay(num);
				this.txtTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("shop_free_countdown", new object[] { text });
			}
			if (this.shopSummonTable != null)
			{
				bool flag = this.iapDataModule.MonthCard.IsActivePrivilege(CardPrivilegeKind.NoAd);
				int maxTimes = this.adDataModule.GetMaxTimes(this.shopSummonTable.adId);
				int watchTimes = this.adDataModule.GetWatchTimes(this.shopSummonTable.adId);
				long watchCD = this.adDataModule.GetWatchCD(this.shopSummonTable.adId);
				this.txtAdCd.text = ((!flag && watchCD > 0L && watchTimes < maxTimes) ? Singleton<LanguageManager>.Instance.GetTime(watchCD) : "");
				if (this.btnAdBuy.gameObject.activeSelf)
				{
					this.btnAdRedNode.Value = ((flag || watchCD == 0L) ? (maxTimes - watchTimes) : 0);
				}
			}
		}

		protected virtual void UpdateCost()
		{
			int freeTimes = this.shopSummonTable.freeTimes;
			int freeCostTimes = this.iapDataModule.GetFreeCostTimes(this.shopSummonTable.id);
			int num = Mathf.Max(0, freeTimes - freeCostTimes);
			int num2 = this.shopSummonTable.priceId;
			long num3 = ((num2 > 0) ? this.propDataModule.GetItemDataCountByid((ulong)((long)num2)) : 0L);
			if (num > 0)
			{
				this.btnFreeBuy.gameObject.SetActive(true);
				this.btnFreeRedNode.Value = 1;
				this.btnAdBuy.gameObject.SetActive(false);
				this.btnItemBuyOne.gameObject.SetActive(false);
				this.btnItemBuyTen.gameObject.SetActive(false);
				this.txtTime.gameObject.SetActive(false);
				this.txtBtnFreeTimes.text = Singleton<LanguageManager>.Instance.GetInfoByID("shop_free_times", new object[] { num });
				this.UpdateCountDown();
			}
			else
			{
				this.btnFreeBuy.gameObject.SetActive(false);
				int watchTimes = this.adDataModule.GetWatchTimes(this.shopSummonTable.adId);
				int maxTimes = this.adDataModule.GetMaxTimes(this.shopSummonTable.adId);
				bool flag = watchTimes < maxTimes;
				this.btnAdBuy.gameObject.SetActive(flag);
				bool flag2 = this.shopSummonTable.priceId > 0 && this.shopSummonTable.tenPrice > 0 && num3 >= (long)this.shopSummonTable.tenPrice;
				bool flag3 = !flag2 && (this.shopSummonTable.priceId > 0 || this.shopSummonTable.singlePriceOrigin > 0);
				this.btnItemBuyTen.gameObject.SetActive(flag2);
				this.btnItemBuyOne.gameObject.SetActive(flag3);
				this.txtTime.gameObject.SetActive(this.shopSummonTable.freeTimes > 0);
				if (flag2)
				{
					long num4 = (long)this.shopSummonTable.tenPrice;
					this.costItemTen.SetData(num2, num3, num4);
				}
				else
				{
					long num5 = (long)this.shopSummonTable.singlePrice;
					if (this.shopSummonTable.quickDraw > 0 && num3 > 0L && num3 < 10L)
					{
						num5 = num3;
					}
					if (num2 > 0 && num3 < num5 && this.shopSummonTable.singlePriceOrigin > 0)
					{
						num2 = 2;
						num5 = (long)this.shopSummonTable.singlePriceOrigin;
						num3 = this.propDataModule.GetItemDataCountByid((ulong)((long)num2));
						this.costItemOne.SetData(num2, num5);
					}
					else
					{
						this.costItemOne.SetData(num2, num3, num5);
					}
				}
			}
			if (this.btnTenRedNode != null)
			{
				this.btnTenRedNode.gameObject.SetActiveSafe(num <= 0 && this.btnItemBuyTen.gameObject.activeSelf && num3 >= (long)this.shopSummonTable.tenPrice && num2 != 2);
			}
			if (this.btnOneRedNode != null)
			{
				this.btnOneRedNode.gameObject.SetActiveSafe(num <= 0 && this.btnItemBuyOne.gameObject.activeSelf && num3 >= (long)this.shopSummonTable.singlePrice && num2 != 2);
			}
		}

		protected virtual void OnBtnAdBuyClick()
		{
			int watchTimes = this.adDataModule.GetWatchTimes(this.shopSummonTable.adId);
			int maxTimes = this.adDataModule.GetMaxTimes(this.shopSummonTable.adId);
			if (watchTimes >= maxTimes)
			{
				this.SetData();
				return;
			}
			AdBridge.PlayRewardVideo(this.shopSummonTable.adId, delegate(bool isSuccess)
			{
				if (isSuccess)
				{
					this.SendRequest(this.shopSummonTable.id, 1, 1);
				}
			});
		}

		protected virtual void OnBtnItemBuyOneClick()
		{
			this.OnBtnItemBuyClick(1);
		}

		protected virtual void OnBtnItemBuyTenClick()
		{
			this.OnBtnItemBuyClick(2);
		}

		protected virtual void OnBtnItemBuyClick(int drawTimesType)
		{
			int num = this.shopSummonTable.priceId;
			long num2 = (long)((drawTimesType == 2) ? this.shopSummonTable.tenPrice : this.shopSummonTable.singlePrice);
			long num3 = this.propDataModule.GetItemDataCountByid((ulong)((long)num));
			if (num3 < num2)
			{
				if (!((drawTimesType == 2) ? (this.shopSummonTable.tenPriceOrigin > 0) : (this.shopSummonTable.singlePriceOrigin > 0)))
				{
					GameApp.View.ShowItemNotEnoughTip(num, true);
					return;
				}
				long num4 = (long)((drawTimesType == 2) ? this.shopSummonTable.tenPriceOrigin : this.shopSummonTable.singlePriceOrigin);
				num = 2;
				num2 = num4;
				num3 = this.propDataModule.GetItemDataCountByid((ulong)((long)num));
				if (num3 < num2)
				{
					GameApp.View.ShowItemNotEnoughTip(num, true);
					return;
				}
			}
			this.drawCostId = num;
			if (this.drawCostId == 61 && drawTimesType == 1 && num3 > num2)
			{
				this.drawCostCount = (int)num3;
			}
			else
			{
				this.drawCostCount = (int)num2;
			}
			this.SendRequest(this.shopSummonTable.id, 2, drawTimesType);
		}

		protected virtual void OnBtnFreeBuyClick()
		{
			int freeTimes = this.shopSummonTable.freeTimes;
			int freeCostTimes = this.iapDataModule.GetFreeCostTimes(this.shopSummonTable.id);
			if (Mathf.Max(0, freeTimes - freeCostTimes) <= 0)
			{
				this.SetData();
				return;
			}
			this.drawCostId = 0;
			this.drawCostCount = 0;
			this.SendRequest(this.shopSummonTable.id, 2, 1);
		}

		private void OnClickProbability()
		{
			GameApp.View.OpenView(ViewName.EquipShopProbabilityViewModule, this.shopSummonTable.id, 1, null, null);
		}

		protected virtual void SendRequest(int summonId, int costType, int drawType)
		{
			this.drawCostType = costType;
			NetworkUtils.Shop.ShopDoDrawRequest(summonId, costType, drawType, new Action<bool, ShopDoDrawResponse>(this.SendMessageCallback));
		}

		protected virtual void SendMessageCallback(bool isOk, ShopDoDrawResponse resp)
		{
			if (isOk)
			{
				int count = resp.CommonData.Reward.Count;
				OpenEquipBoxViewModule.OpenData openData = new OpenEquipBoxViewModule.OpenData();
				openData.boxId = this.shopSummonTable.boxId;
				openData.itemDatas = resp.CommonData.Reward.ToItemDataList();
				openData.CostType = this.drawCostType;
				openData.ChestType = this.chestType;
				openData.shopSummonId = this.shopSummonTable.id;
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, null);
				GameApp.View.OpenView(ViewName.OpenEquipBoxViewModule, openData, 1, null, delegate
				{
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, null);
				});
				int num = 1120800 + this.shopSummonTable.boxId;
				GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName(num), resp.CommonData.Reward, null, null, null, null);
				GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName(num), resp.CommonData.CostDto, null);
				string text = "";
				if (this.shopSummonTable.boxId == 2)
				{
					if (this.drawCostType == 1)
					{
						GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(2), "REWARD ", "", resp.CommonData.Reward, null);
					}
					text = "冒险者补给箱";
				}
				if (this.shopSummonTable.boxId == 3)
				{
					if (this.drawCostType == 1)
					{
						GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(3), "REWARD ", "", resp.CommonData.Reward, null);
					}
					text = "英雄补给箱";
				}
				GameTGACostCurrency gameTGACostCurrency;
				if (this.drawCostType == 1)
				{
					gameTGACostCurrency = GameTGACostCurrency.Ad;
				}
				else if (this.drawCostId == 0)
				{
					gameTGACostCurrency = GameTGACostCurrency.Free;
				}
				else if (this.drawCostId == 2)
				{
					gameTGACostCurrency = GameTGACostCurrency.Gem;
				}
				else
				{
					gameTGACostCurrency = GameTGACostCurrency.ChestKey;
				}
				GameApp.SDK.Analyze.Track_EquipmentBoxOpen(text, gameTGACostCurrency, this.drawCostCount, resp.CommonData.Reward, null);
			}
		}

		public eEquipChestType chestType = eEquipChestType.GoldChest;

		public CustomImage imgIcon;

		public CustomText txtTitle;

		public CustomText txtDesc;

		public GameObject pityCountNode;

		public CustomText txtMiniPity;

		public CustomText txtHardPity;

		public CustomText txtTime;

		public CustomText txtBtnFreeTimes;

		public CustomText txtAdCd;

		public CustomButton btnFreeBuy;

		public CustomButton btnAdBuy;

		public CustomButton btnItemBuyOne;

		public CustomButton btnItemBuyTen;

		public CommonCostItem costItemOne;

		public CommonCostItem costItemTen;

		public RedNodeOneCtrl btnAdRedNode;

		public RedNodeOneCtrl btnFreeRedNode;

		public RedNodeOneCtrl btnOneRedNode;

		public RedNodeOneCtrl btnTenRedNode;

		public CustomButton btnProbability;

		protected IAPDataModule iapDataModule;

		protected ShopDataModule shopDataModule;

		protected PropDataModule propDataModule;

		protected AdDataModule adDataModule;

		protected Shop_Summon shopSummonTable;

		protected Shop_EquipActivity shopEquipActivityTable;

		private int drawCostType;

		private int drawCostId;

		private int drawCostCount;
	}
}
