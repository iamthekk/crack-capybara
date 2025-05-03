using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;

namespace HotFix
{
	public class IAPLevelFundBuyViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.Rewards.Init();
			this.m_uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
		}

		public override void OnOpen(object data)
		{
			IAPLevelFundBuyViewModule.OpenData openData = data as IAPLevelFundBuyViewModule.OpenData;
			if (openData != null)
			{
				this.mOpenData = openData;
			}
			if (this.mOpenData == null)
			{
				this.OnClickCloseThis();
				return;
			}
			this.BuildRewardsDataList();
			if (this.mLevelFundGroup != null)
			{
				this.ButtonPay.SetData(this.mLevelFundGroup.PurchaseID, null, new Action<int>(this.SureBuy), null, null, null);
			}
			this.RefreshUI();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.Rewards.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void RefreshUI()
		{
			this.Rewards.SetData(this.mRewardsDataList);
			this.Rewards.RefreshUI();
			this.TextBuyTips.text = Singleton<LanguageManager>.Instance.GetInfoByID("8703");
		}

		private void BuildRewardsDataList()
		{
			this.mRewardsDataList.Clear();
			IAPLevelFund levelFund = GameApp.Data.GetDataModule(DataName.IAPDataModule).LevelFund;
			this.mLevelFund = levelFund;
			if (this.mLevelFund == null)
			{
				return;
			}
			this.mLevelFundGroup = this.mLevelFund.GetLevelFundGroup(this.mOpenData.LevelFundID);
			if (this.mLevelFundGroup == null)
			{
				return;
			}
			List<PropData> totalRewards = this.mLevelFundGroup.GetTotalRewards();
			Dictionary<uint, PropData> dictionary = new Dictionary<uint, PropData>();
			for (int i = 0; i < totalRewards.Count; i++)
			{
				PropData propData = totalRewards[i];
				this.AddItemToDic(propData, dictionary);
			}
			this.mRewardsDataList.AddRange(dictionary.Values);
		}

		private void AddItemToDic(PropData item, Dictionary<uint, PropData> dic)
		{
			PropData propData;
			if (dic.TryGetValue(item.id, out propData))
			{
				propData.count += item.count;
				return;
			}
			propData = new PropData();
			propData.id = item.id;
			propData.count = item.count;
			propData.level = item.level;
			propData.exp = item.exp;
			dic.Add(propData.id, propData);
		}

		private void SureBuy(int purchaseid)
		{
			if (this.mLevelFundGroup == null || this.mLevelFundGroup.HasBuy)
			{
				this.OnClickCloseThis();
				return;
			}
			if (purchaseid != this.mLevelFundGroup.PurchaseID)
			{
				this.OnClickCloseThis();
				return;
			}
			GameApp.Purchase.Manager.Buy(purchaseid, 0, "", delegate(bool isOk)
			{
				if (isOk)
				{
					this.OnClickCloseThis();
				}
			}, null);
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickCloseThis();
			}
		}

		private void OnClickCloseThis()
		{
			if (GameApp.View.IsOpened(ViewName.IAPLevelFundBuyViewModule))
			{
				GameApp.View.CloseView(ViewName.IAPLevelFundBuyViewModule, null);
			}
		}

		public IAPPurchaseRewards Rewards;

		public UIPopCommon m_uiPopCommon;

		public PurchaseButtonCtrl ButtonPay;

		public CustomText TextBuyTips;

		private List<PropData> mRewardsDataList = new List<PropData>();

		private IAPLevelFundBuyViewModule.OpenData mOpenData;

		private IAPLevelFund mLevelFund;

		private IAPLevelFundGroup mLevelFundGroup;

		public class OpenData
		{
			public int LevelFundID;
		}
	}
}
