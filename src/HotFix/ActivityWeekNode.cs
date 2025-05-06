using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class ActivityWeekNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.InitNode();
		}

		private void InitNode()
		{
			if (this.m_viewType == ActivityWeekViewModule.ActViewType.RankRewardType)
			{
				this.taskObj.SetActive(false);
				this.rankObj.SetActive(true);
				base.rectTransform.sizeDelta = new Vector2(base.rectTransform.sizeDelta.x, this.m_activityNodeHeight_Rank);
				this.itemRoot.anchoredPosition = this.m_itemPos_Rank;
			}
			else
			{
				this.taskObj.SetActive(true);
				this.rankObj.SetActive(false);
				base.rectTransform.sizeDelta = new Vector2(base.rectTransform.sizeDelta.x, this.m_activityNodeHeight_Normal);
				this.itemRoot.anchoredPosition = this.m_itemPos_Normal;
			}
			this.taskTitleObj.SetActive(false);
			this.buyTitleObj.SetActive(false);
			this.sliderNode.gameObject.SetActive(false);
			this.taskButton.gameObject.SetActive(false);
			this.taskButton.m_onClick = null;
			this.taskNodeOneCtrl.gameObject.SetActive(false);
			this.buyButton.gameObject.SetActive(false);
			this.buyButton.m_onClick = null;
			this.buyRedNodeOneCtrl.gameObject.SetActive(false);
			this.buyCostImage.gameObject.SetActive(false);
			this.payButton.gameObject.SetActive(false);
			this.payRedNodeOneCtrl.gameObject.SetActive(false);
			this.finishedText.transform.parent.gameObject.SetActive(false);
			this.unFinishedText.enabled = false;
			if (this.m_viewType == ActivityWeekViewModule.ActViewType.ConsumeType)
			{
				this.m_TitleObj = this.taskTitleObj;
				this.m_TitleText = this.taskTitleText;
				this.m_Button = this.taskButton;
				this.m_ButtonText = this.taskButtonText;
				this.m_RedNodeOneCtrl = this.taskNodeOneCtrl;
			}
			else
			{
				this.m_TitleObj = this.buyTitleObj;
				this.m_TitleText = this.buyTitleText;
				this.m_Button = this.buyButton;
				this.m_ButtonText = this.buyButtonText;
				this.m_RedNodeOneCtrl = this.buyRedNodeOneCtrl;
			}
			for (int i = 0; i < this.leftItems.Count; i++)
			{
				this.leftItems[i].gameObject.SetActive(false);
				this.leftItems[i].Init();
			}
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.leftItems.Count; i++)
			{
				this.leftItems[i].DeInit();
			}
		}

		private void SetViewType(ActivityWeekViewModule.ActViewType viewType)
		{
			this.m_viewType = viewType;
		}

		public void ShowConsume(int actId, CommonActivity_ConsumeObj table, int buyCount, int score, int maxScore)
		{
			this.SetViewType(ActivityWeekViewModule.ActViewType.ConsumeType);
			this.InitNode();
			this.m_TitleObj.SetActive(true);
			this.m_TitleText.text = Singleton<LanguageManager>.Instance.GetInfoByID(table.ObjName);
			this.sliderNode.gameObject.SetActive(true);
			RectTransform rectTransform = this.sliderImage.rectTransform;
			float num = (float)score / (float)table.ObjNum;
			if (num >= 1f)
			{
				rectTransform.sizeDelta = new Vector2(this.sliderNode.rect.width - this.sliderLeftPadding * 2f, rectTransform.sizeDelta.y);
				this.sliderText.text = string.Format("{0}/{1}", table.ObjNum, table.ObjNum);
			}
			else
			{
				rectTransform.sizeDelta = new Vector2((this.sliderNode.rect.width - this.sliderLeftPadding * 2f) * num, rectTransform.sizeDelta.y);
				this.sliderText.text = string.Format("{0}/{1}", score, table.ObjNum);
			}
			string[] objReward = table.ObjReward;
			for (int i = 0; i < objReward.Length; i++)
			{
				int num2 = int.Parse(objReward[i].Split(',', StringSplitOptions.None)[0]);
				long num3 = long.Parse(objReward[i].Split(',', StringSplitOptions.None)[1]);
				ItemData itemData = new ItemData(num2, num3);
				this.leftItems[i].gameObject.SetActiveSafe(true);
				this.leftItems[i].SetData(itemData.ToPropData());
				this.leftItems[i].onClick = new Action<UIItem, PropData, object>(this.onClickItem);
				this.leftItems[i].OnRefresh();
			}
			if (buyCount >= 1)
			{
				this.finishedText.transform.parent.gameObject.SetActive(true);
				this.finishedText.text = Singleton<LanguageManager>.Instance.GetInfoByID("btn_txt_state_finished");
				return;
			}
			if (score < table.ObjNum)
			{
				this.unFinishedText.enabled = true;
				this.unFinishedText.text = Singleton<LanguageManager>.Instance.GetInfoByID("btn_txt_state_unfinish");
				return;
			}
			this.m_Button.gameObject.SetActiveSafe(true);
			this.m_ButtonText.text = Singleton<LanguageManager>.Instance.GetInfoByID("btn_txt_state_collect");
			this.m_RedNodeOneCtrl.gameObject.SetActiveSafe(true);
			this.m_RedNodeOneCtrl.SetType(240);
			this.m_RedNodeOneCtrl.Value = 1;
			this.m_Button.m_onClick = delegate
			{
				NetworkUtils.ActivityWeek.RequestActTimeReward(actId, 1, table.Id, null);
			};
		}

		public void ShowShop(int actId, CommonActivity_ShopObj table, int buyCount)
		{
			this.SetViewType(ActivityWeekViewModule.ActViewType.ShopType);
			this.InitNode();
			this.m_TitleObj.SetActive(true);
			this.m_TitleText.text = Singleton<LanguageManager>.Instance.GetInfoByID(table.ObjName);
			for (int i = 0; i < table.ObjGoods.Length; i++)
			{
				int num = int.Parse(table.ObjGoods[i].Split(',', StringSplitOptions.None)[0]);
				long num2 = long.Parse(table.ObjGoods[i].Split(',', StringSplitOptions.None)[1]);
				this.leftItems[i].gameObject.SetActiveSafe(true);
				ItemData itemData = new ItemData(num, num2);
				this.leftItems[i].SetData(itemData.ToPropData());
				this.leftItems[i].onClick = new Action<UIItem, PropData, object>(this.onClickItem);
				this.leftItems[i].OnRefresh();
			}
			if (table.objToplimit > 0)
			{
				this.buyLimitText.text = Singleton<LanguageManager>.Instance.GetInfoByID("exchange_limit_times", new object[] { buyCount, table.objToplimit });
			}
			else
			{
				this.buyLimitText.text = "";
			}
			if (table.objToplimit > 0 && buyCount >= table.objToplimit)
			{
				this.finishedText.transform.parent.gameObject.SetActive(true);
				this.finishedText.text = Singleton<LanguageManager>.Instance.GetInfoByID("9100094");
			}
			else
			{
				this.m_Button.gameObject.SetActiveSafe(true);
				int bugId = table.ObjPrice1;
				if (table.ObjPrice2 == 0)
				{
					this.m_ButtonText.text = Singleton<LanguageManager>.Instance.GetInfoByID("101");
					this.m_RedNodeOneCtrl.gameObject.SetActiveSafe(true);
					this.m_RedNodeOneCtrl.SetType(240);
					this.m_RedNodeOneCtrl.Value = 1;
				}
				else
				{
					Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(bugId);
					this.buyCostImage.gameObject.SetActive(true);
					this.buyCostImage.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
					this.m_ButtonText.text = "x" + table.ObjPrice2.ToString();
				}
				this.m_Button.m_onClick = delegate
				{
					if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)bugId)) >= (long)table.ObjPrice2)
					{
						NetworkUtils.ActivityWeek.RequestActiveShopBug(actId, table.id, null);
						return;
					}
					GameApp.View.ShowItemNotEnoughTip(bugId, true);
				};
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.itemCostLayout);
		}

		public void ShowPay(int actId, CommonActivity_PayObj table, int buyCount)
		{
			ActivityWeekNode.<>c__DisplayClass45_0 CS$<>8__locals1 = new ActivityWeekNode.<>c__DisplayClass45_0();
			CS$<>8__locals1.actId = actId;
			CS$<>8__locals1.table = table;
			this.SetViewType(ActivityWeekViewModule.ActViewType.PayType);
			this.InitNode();
			this.m_TitleObj.SetActive(true);
			this.m_TitleText.text = Singleton<LanguageManager>.Instance.GetInfoByID(CS$<>8__locals1.table.ObjName);
			for (int i = 0; i < CS$<>8__locals1.table.ObjGoods.Length; i++)
			{
				this.leftItems[i].gameObject.SetActiveSafe(true);
				int num = int.Parse(CS$<>8__locals1.table.ObjGoods[i].Split(',', StringSplitOptions.None)[0]);
				long num2 = long.Parse(CS$<>8__locals1.table.ObjGoods[i].Split(',', StringSplitOptions.None)[1]);
				ItemData itemData = new ItemData(num, num2);
				this.leftItems[i].SetData(itemData.ToPropData());
				this.leftItems[i].onClick = new Action<UIItem, PropData, object>(this.onClickItem);
				this.leftItems[i].OnRefresh();
			}
			if (CS$<>8__locals1.table.objToplimit > 0 && buyCount >= CS$<>8__locals1.table.objToplimit)
			{
				this.finishedText.transform.parent.gameObject.SetActiveSafe(true);
				this.finishedText.text = Singleton<LanguageManager>.Instance.GetInfoByID("9100094");
				return;
			}
			int iapId = CS$<>8__locals1.table.PurchaseId;
			float num3 = 0f;
			if (iapId > 0)
			{
				IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(iapId);
				if (elementById != null)
				{
					num3 = elementById.price1;
				}
			}
			CustomText customText = ((num3 <= 0f) ? this.buyLimitText : this.payLimitText);
			if (CS$<>8__locals1.table.objToplimit > 0)
			{
				customText.text = Singleton<LanguageManager>.Instance.GetInfoByID("buy_limit_times", new object[]
				{
					buyCount,
					CS$<>8__locals1.table.objToplimit
				});
			}
			else
			{
				customText.text = "";
			}
			if (num3 <= 0f)
			{
				this.m_Button.gameObject.SetActiveSafe(true);
				this.m_ButtonText.text = Singleton<LanguageManager>.Instance.GetInfoByID("101");
				this.m_RedNodeOneCtrl.gameObject.SetActiveSafe(true);
				this.m_RedNodeOneCtrl.SetType(240);
				this.m_RedNodeOneCtrl.Value = 1;
				this.m_Button.m_onClick = delegate
				{
					NetworkUtils.ActivityWeek.RequestActTimePayFreeReward(CS$<>8__locals1.actId, CS$<>8__locals1.table.id, null);
				};
				return;
			}
			this.payButton.gameObject.SetActiveSafe(true);
			if (num3 <= 0f)
			{
				this.payRedNodeOneCtrl.gameObject.SetActiveSafe(true);
				this.payRedNodeOneCtrl.SetType(240);
				this.payRedNodeOneCtrl.Value = 1;
			}
			this.payButton.SetData(iapId, null, delegate(int purchaseId)
			{
				GameApp.Purchase.Manager.Buy(iapId, 1, string.Format("{0},{1}", CS$<>8__locals1.actId, CS$<>8__locals1.table.id), delegate(bool result)
				{
					if (result)
					{
						NetworkUtils.ActivityWeek.RequestActTimeActivityList(true);
					}
				}, null);
			}, null, null, null);
		}

		public void ShowRankReward(int actId, CommonActivity_RankObj table)
		{
			this.SetViewType(ActivityWeekViewModule.ActViewType.RankRewardType);
			this.InitNode();
			int rank = table.rank;
			if (rank > 3)
			{
				CommonActivity_RankObj elementById = GameApp.Table.GetManager().GetCommonActivity_RankObjModelInstance().GetElementById(table.id - 1);
				if (elementById.rank + 1 == rank)
				{
					this.rankText.text = string.Format("{0}", rank);
				}
				else
				{
					this.rankText.text = string.Format("{0}-{1}", elementById.rank + 1, rank);
				}
			}
			else
			{
				this.rankText.text = "";
			}
			if (rank > 0 && rank < 4)
			{
				this.rankImage.enabled = true;
				this.rankImage.SetImage(GameApp.Table.GetAtlasPath(128), "rank_" + rank.ToString());
			}
			else
			{
				this.rankImage.enabled = false;
			}
			for (int i = 0; i < table.reward.Length; i++)
			{
				this.leftItems[i].gameObject.SetActiveSafe(true);
				int num = int.Parse(table.reward[i].Split(',', StringSplitOptions.None)[0]);
				long num2 = long.Parse(table.reward[i].Split(',', StringSplitOptions.None)[1]);
				ItemData itemData = new ItemData(num, num2);
				this.leftItems[i].SetData(itemData.ToPropData());
				this.leftItems[i].onClick = new Action<UIItem, PropData, object>(this.onClickItem);
				this.leftItems[i].OnRefresh();
			}
		}

		private void onClickItem(UIItem item, PropData data, object arg)
		{
			DxxTools.UI.ShowItemInfo(new ItemInfoOpenData
			{
				m_propData = data,
				m_openDataType = ItemInfoOpenDataType.eBag,
				m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume)
			}, item.transform.position, 70f);
		}

		private ActivityWeekViewModule.ActViewType m_viewType;

		public RectTransform taskAniNode;

		public GameObject taskObj;

		public GameObject rankObj;

		public float m_activityNodeHeight_Normal = 256f;

		public float m_activityNodeHeight_Rank = 176f;

		public Vector2 m_itemPos_Normal = new Vector2(39f, -167f);

		public Vector2 m_itemPos_Rank = new Vector2(208f, -80f);

		public GameObject taskTitleObj;

		public CustomText taskTitleText;

		public GameObject buyTitleObj;

		public CustomText buyTitleText;

		public RectTransform sliderNode;

		public CustomText sliderText;

		public Image sliderImage;

		public float sliderLeftPadding;

		public CustomImage rankImage;

		public CustomText rankText;

		public RectTransform itemRoot;

		public List<UIItem> leftItems;

		public CustomText finishedText;

		public CustomText unFinishedText;

		public CustomButton taskButton;

		public CustomText taskButtonText;

		public RedNodeOneCtrl taskNodeOneCtrl;

		public CustomButton buyButton;

		public CustomText buyButtonText;

		public CustomText buyLimitText;

		public CustomImage buyCostImage;

		public RedNodeOneCtrl buyRedNodeOneCtrl;

		public RectTransform itemCostLayout;

		public PurchaseButtonCtrl payButton;

		public CustomText payLimitText;

		public RedNodeOneCtrl payRedNodeOneCtrl;

		private GameObject m_TitleObj;

		private CustomText m_TitleText;

		private CustomButton m_Button;

		private CustomText m_ButtonText;

		private RedNodeOneCtrl m_RedNodeOneCtrl;
	}
}
