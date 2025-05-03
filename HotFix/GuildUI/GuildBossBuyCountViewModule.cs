using System;
using Dxx.Guild;
using Framework.Logic.UI;
using Proto.Guild;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildBossBuyCountViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			base.OnViewCreate();
			this.PopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnClickPopCommon);
			this.buttonGoldBuy.onClick = new Action(this.OnBuyCountOfGold);
			this.buttonDiamondBuy.onClick = new Action(this.OnBuyCountOfDiamond);
		}

		private void OnBuyCountOfGold()
		{
			if (this.CheckBuyEnable(GuildBossBuyKind.Gold))
			{
				GuildNetUtil.Guild.DoRequest_GuildBossBuyCount(GuildBossBuyKind.Gold, delegate(bool result, GuildBossBuyCntResponse resp)
				{
					if (result)
					{
						GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_GuildBoss_Refresh);
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400251"));
					}
				});
			}
		}

		private void OnBuyCountOfDiamond()
		{
			if (this.CheckBuyEnable(GuildBossBuyKind.Diamonds))
			{
				GuildNetUtil.Guild.DoRequest_GuildBossBuyCount(GuildBossBuyKind.Diamonds, delegate(bool result, GuildBossBuyCntResponse resp)
				{
					GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_GuildBoss_Refresh);
					GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400251"));
				});
			}
		}

		protected override void OnViewOpen(object data)
		{
			base.OnViewOpen(data);
			GuildBossInfo guildBoss = base.SDK.GuildActivity.GuildBoss;
			this.goldBuyData = guildBoss.GetBuyCountData(GuildBossBuyKind.Gold);
			this.diamondBuyData = guildBoss.GetBuyCountData(GuildBossBuyKind.Diamonds);
			int num = 0;
			int num2 = 0;
			bool flag = false;
			bool flag2 = false;
			if (this.goldBuyData != null)
			{
				num = this.goldBuyData.MaxBuyCount - this.goldBuyData.BuyCount;
				num = ((num < 0) ? 0 : num);
				this.buttonGoldBuy.SetData(this.goldBuyData.BuyCost.id, this.goldBuyData.BuyCost.count);
				flag = this.goldBuyData.BuyCount >= this.goldBuyData.MaxBuyCount;
			}
			if (this.diamondBuyData != null)
			{
				num2 = this.diamondBuyData.MaxBuyCount - this.diamondBuyData.BuyCount;
				num2 = ((num2 < 0) ? 0 : num2);
				this.buttonDiamondBuy.SetData(this.diamondBuyData.BuyCost.id, this.diamondBuyData.BuyCost.count);
				flag2 = this.diamondBuyData.BuyCount >= this.diamondBuyData.MaxBuyCount;
			}
			this.textGoldLimit.text = GuildProxy.Language.GetInfoByID1("400248", num);
			this.textDiamondLimit.text = GuildProxy.Language.GetInfoByID1("400248", num2);
			this.textGoldBuyCount.text = "x1";
			this.textDiamondBuyCount.text = "x1";
			this.buttonGoldBuy.gameObject.SetActiveSafe(!flag);
			this.buttonDiamondBuy.gameObject.SetActiveSafe(!flag2);
			this.goldBuyFinishObj.SetActiveSafe(flag);
			this.diamondBuyFinishObj.SetActiveSafe(flag2);
		}

		protected override void OnViewClose()
		{
			base.OnViewClose();
		}

		protected override void OnViewDelete()
		{
			base.OnViewDelete();
		}

		private bool CheckBuyEnable(GuildBossBuyKind kind)
		{
			if (kind == GuildBossBuyKind.Gold)
			{
				if (this.goldBuyData == null || GuildProxy.GameUser.MyGold() < this.goldBuyData.BuyCost.count)
				{
					return false;
				}
			}
			else if (kind == GuildBossBuyKind.Diamonds && (this.diamondBuyData == null || GuildProxy.GameUser.MyDiamond() < this.diamondBuyData.BuyCost.count))
			{
				return false;
			}
			return true;
		}

		private void OnClickPopCommon(UIPopCommon.UIPopCommonClickType type)
		{
			this.CloseThisView();
		}

		private void CloseThisView()
		{
			GuildProxy.UI.CloseUIGuildBossBuyCount();
		}

		public UIPopCommon PopCommon;

		public UIGuildCurrencyButton buttonGoldBuy;

		public UIGuildCurrencyButton buttonDiamondBuy;

		public CustomText textGoldLimit;

		public CustomText textGoldBuyCount;

		public CustomText textDiamondLimit;

		public CustomText textDiamondBuyCount;

		public GameObject goldBuyFinishObj;

		public GameObject diamondBuyFinishObj;

		private GuildBossBuyCountData goldBuyData;

		private GuildBossBuyCountData diamondBuyData;
	}
}
