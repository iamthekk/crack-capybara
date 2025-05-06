using System;
using System.Collections.Generic;
using System.Globalization;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class MainShopChapterPackItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			for (int i = 0; i < this.rewardItems.Count; i++)
			{
				this.rewardItems[i].Init();
				this.rewardItems[i].gameObject.SetActive(false);
			}
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(IAP_ChapterPacks cfg)
		{
			this.purchaseButtonCtrl.SetData(cfg.id, null, null, null, null, null);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(cfg.nameId);
			this.txtName.text = infoByID;
			string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID(cfg.descId);
			this.txtName.text = infoByID2;
			this.txtTip.text = "";
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(cfg.id);
			elementById.originPrice1.ToString(CultureInfo.InvariantCulture);
			int valueBet = elementById.valueBet;
			GameApp.Purchase.Manager.GetPriceForPlatformID(elementById.platformID);
			this.imgBg1.gameObject.SetActive(cfg.bannerBgType == 1);
			this.imgBg2.gameObject.SetActive(cfg.bannerBgType == 2);
			this.imgBg3.gameObject.SetActive(cfg.bannerBgType == 3);
			this.imgBg4.gameObject.SetActive(cfg.bannerBgType == 4);
			this.imgBg5.gameObject.SetActive(cfg.bannerBgType == 5);
			if (elementById.valueBet > 1)
			{
				this.goTag.SetActive(true);
				this.txtValueBet.text = string.Format("x{0}", elementById.valueBet);
			}
			else
			{
				this.goTag.SetActive(false);
			}
			List<ItemData> list = cfg.products.ToItemDataList();
			for (int i = 0; i < this.rewardItems.Count; i++)
			{
				if (i < list.Count)
				{
					this.rewardItems[i].gameObject.SetActive(true);
					this.rewardItems[i].SetData(list[i].ToPropData());
					this.rewardItems[i].OnRefresh();
				}
				else
				{
					this.rewardItems[i].gameObject.SetActive(false);
				}
			}
		}

		public RectTransform fg;

		public CustomButton btnItem;

		public PurchaseButtonCtrl purchaseButtonCtrl;

		public CustomImage imgBg1;

		public CustomImage imgBg2;

		public CustomImage imgBg3;

		public CustomImage imgBg4;

		public CustomImage imgBg5;

		public GameObject goTag;

		public CustomText txtValueBet;

		public CustomText txtName;

		public CustomText txtTip;

		public List<UIItem> rewardItems = new List<UIItem>();
	}
}
