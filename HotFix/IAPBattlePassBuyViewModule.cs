using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;

namespace HotFix
{
	public class IAPBattlePassBuyViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.Rewards.Init();
			this.UIPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
		}

		public override void OnOpen(object data)
		{
			this.BuildRewardsDataList();
			this.ButtonPay.SetData(this.mBattlePass.BattlePassPurchaseID, null, new Action<int>(this.SureBuy), null, null, null);
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
			this.TextBuyTips.text = Singleton<LanguageManager>.Instance.GetInfoByID("uibattlepassbuy_tips");
		}

		private void BuildRewardsDataList()
		{
			this.mRewardsDataList.Clear();
			IAPBattlePass battlePass = GameApp.Data.GetDataModule(DataName.IAPDataModule).BattlePass;
			this.mBattlePass = battlePass;
			if (battlePass == null)
			{
				return;
			}
			Dictionary<uint, PropData> dictionary = new Dictionary<uint, PropData>();
			List<IAPBattlePassData> dataList = battlePass.DataList;
			for (int i = 0; i < dataList.Count; i++)
			{
				IAPBattlePassData iapbattlePassData = dataList[i];
				int num = 1;
				if (iapbattlePassData.IsFinal)
				{
					num = battlePass.FinalRewardMaxCount;
				}
				List<PropData> list = iapbattlePassData.FreeRewards;
				if (list != null)
				{
					for (int j = 0; j < list.Count; j++)
					{
						PropData propData = list[j];
						this.AddItemToDic(propData, num, dictionary);
					}
				}
				list = iapbattlePassData.PayRewards;
				if (list != null)
				{
					for (int k = 0; k < list.Count; k++)
					{
						PropData propData2 = list[k];
						this.AddItemToDic(propData2, num, dictionary);
					}
				}
			}
			this.mRewardsDataList.AddRange(dictionary.Values);
		}

		private void AddItemToDic(PropData item, int rate, Dictionary<uint, PropData> dic)
		{
			if (rate < 0)
			{
				rate = 0;
			}
			PropData propData;
			if (dic.TryGetValue(item.id, out propData))
			{
				propData.count += item.count * (ulong)rate;
				return;
			}
			propData = new PropData();
			propData.id = item.id;
			propData.count = item.count * (ulong)rate;
			propData.level = item.level;
			propData.exp = item.exp;
			dic.Add(propData.id, propData);
		}

		private void SureBuy(int purchaseid)
		{
			IAPBattlePass battlePass = GameApp.Data.GetDataModule(DataName.IAPDataModule).BattlePass;
			if (battlePass == null || battlePass.HasBuy)
			{
				this.OnClickCloseThis();
				return;
			}
			if (purchaseid != battlePass.BattlePassPurchaseID)
			{
				this.OnClickCloseThis();
				return;
			}
			GameApp.Purchase.Manager.Buy(battlePass.BattlePassPurchaseID, 0, "", delegate(bool isOk)
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
			if (GameApp.View.IsOpened(ViewName.IAPBattlePassBuyViewModule))
			{
				GameApp.View.CloseView(ViewName.IAPBattlePassBuyViewModule, null);
			}
		}

		public IAPPurchaseRewards Rewards;

		public UIPopCommon UIPopCommon;

		public PurchaseButtonCtrl ButtonPay;

		public CustomText TextBuyTips;

		private List<PropData> mRewardsDataList = new List<PropData>();

		private IAPBattlePass mBattlePass;
	}
}
