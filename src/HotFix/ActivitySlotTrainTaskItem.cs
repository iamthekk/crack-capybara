using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class ActivitySlotTrainTaskItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.InitNode();
		}

		private void InitNode()
		{
			this.taskTitleObj.SetActive(false);
			this.buyTitleObj.SetActive(false);
			this.sliderNode.gameObject.SetActive(false);
			this.taskButton.gameObject.SetActive(false);
			this.taskButton.m_onClick = null;
			this.taskNodeOneCtrl.gameObject.SetActive(false);
			this.taskGoButton.gameObject.SetActive(false);
			this.taskGoButton.m_onClick = null;
			this.taskGoNodeOneCtrl.gameObject.SetActive(false);
			this.payButton.gameObject.SetActive(false);
			this.payRedNodeOneCtrl.gameObject.SetActive(false);
			this.adButton.m_onClick = null;
			this.adButton.gameObject.SetActive(false);
			this.adRedNodeOneCtrl.gameObject.SetActive(false);
			this.finishedText.transform.parent.gameObject.SetActive(false);
			if (this.m_viewType == ActivitySlotTrainTaskViewType.Quest)
			{
				this.m_TitleObj = this.taskTitleObj;
				this.m_TitleText = this.taskTitleText;
			}
			else
			{
				this.m_TitleObj = this.buyTitleObj;
				this.m_TitleText = this.buyTitleText;
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

		private void SetViewType(ActivitySlotTrainTaskViewType viewType)
		{
			this.m_viewType = viewType;
		}

		public void ShowQuest(ActivitySlotTrainTaskViewModule.QuestItemInfo itemInfo)
		{
			this.SetViewType(ActivitySlotTrainTaskViewType.Quest);
			this.InitNode();
			this.m_TitleObj.SetActive(true);
			this.m_TitleText.text = Singleton<LanguageManager>.Instance.GetInfoByID(itemInfo.cfg.Describe, new object[] { itemInfo.cfg.Need });
			this.sliderNode.gameObject.SetActive(true);
			RectTransform rectTransform = this.sliderImage.rectTransform;
			float num = Mathf.Min((float)itemInfo.score / (float)itemInfo.cfg.Need, 1f);
			rectTransform.sizeDelta = new Vector2((this.sliderNode.rect.width - this.sliderLeftPadding * 2f) * num, rectTransform.sizeDelta.y);
			this.sliderText.text = string.Format("{0}/{1}", itemInfo.score, itemInfo.cfg.Need);
			string[] reward = itemInfo.cfg.Reward;
			for (int i = 0; i < reward.Length; i++)
			{
				int num2 = int.Parse(reward[i].Split(',', StringSplitOptions.None)[0]);
				long num3 = long.Parse(reward[i].Split(',', StringSplitOptions.None)[1]);
				ItemData itemData = new ItemData(num2, num3);
				this.leftItems[i].gameObject.SetActiveSafe(true);
				this.leftItems[i].SetData(itemData.ToPropData());
				this.leftItems[i].onClick = new Action<UIItem, PropData, object>(this.onClickItem);
				this.leftItems[i].OnRefresh();
			}
			if (itemInfo.isFinished)
			{
				this.finishedText.transform.parent.gameObject.SetActive(true);
				this.finishedText.text = Singleton<LanguageManager>.Instance.GetInfoByID("btn_txt_state_finished");
				return;
			}
			if (itemInfo.canPick)
			{
				this.taskButton.gameObject.SetActiveSafe(true);
				this.taskButtonText.text = Singleton<LanguageManager>.Instance.GetInfoByID("btn_txt_state_collect");
				this.taskNodeOneCtrl.gameObject.SetActiveSafe(true);
				this.taskNodeOneCtrl.SetType(240);
				this.taskNodeOneCtrl.Value = 1;
				this.taskButton.m_onClick = delegate
				{
					NetworkUtils.ActivitySlotTrain.RequestTurnTableTaskReceiveReward(itemInfo.cfg.ID);
				};
				return;
			}
			if (itemInfo.cfg.Jump > 0)
			{
				this.taskGoButton.gameObject.SetActive(true);
				this.taskGoButtonText.text = Singleton<LanguageManager>.Instance.GetInfoByID("1257");
				this.taskGoButton.m_onClick = delegate
				{
					ViewJumpType jump = (ViewJumpType)itemInfo.cfg.Jump;
					this.JumpToView(jump);
				};
				return;
			}
			this.taskButton.gameObject.SetActive(true);
			this.taskButtonText.text = Singleton<LanguageManager>.Instance.GetInfoByID("1256");
		}

		private async void JumpToView(ViewJumpType jumpType)
		{
			if (Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(jumpType, null, true))
			{
				if (jumpType == ViewJumpType.Tower)
				{
					if (!GameApp.Data.GetDataModule(DataName.TowerDataModule).IsAllFinish)
					{
						await Singleton<ViewJumpCtrl>.Instance.JumpTo(jumpType, null);
					}
					else
					{
						jumpType = ViewJumpType.RogueDungeon;
						await Singleton<ViewJumpCtrl>.Instance.JumpTo(jumpType, null);
					}
				}
				else
				{
					await Singleton<ViewJumpCtrl>.Instance.JumpTo(jumpType, null);
				}
				GameApp.View.CloseView(ViewName.ActivitySlotTrainTaskViewModule, null);
				GameApp.View.CloseView(ViewName.ActivitySlotTrainViewModule, null);
			}
		}

		public void ShowPay(ActivitySlotTrainTaskViewModule.PayItemInfo itemInfo)
		{
			this.SetViewType(ActivitySlotTrainTaskViewType.Pay);
			this.InitNode();
			this.m_TitleObj.SetActive(true);
			this.m_TitleText.text = Singleton<LanguageManager>.Instance.GetInfoByID(itemInfo.cfg.ObjName);
			for (int i = 0; i < itemInfo.cfg.ObjGoods.Length; i++)
			{
				this.leftItems[i].gameObject.SetActiveSafe(true);
				int num = int.Parse(itemInfo.cfg.ObjGoods[i].Split(',', StringSplitOptions.None)[0]);
				long num2 = long.Parse(itemInfo.cfg.ObjGoods[i].Split(',', StringSplitOptions.None)[1]);
				ItemData itemData = new ItemData(num, num2);
				this.leftItems[i].SetData(itemData.ToPropData());
				this.leftItems[i].onClick = new Action<UIItem, PropData, object>(this.onClickItem);
				this.leftItems[i].OnRefresh();
			}
			if (itemInfo.isFinished)
			{
				this.finishedText.transform.parent.gameObject.SetActiveSafe(true);
				this.finishedText.text = Singleton<LanguageManager>.Instance.GetInfoByID("9100094");
				return;
			}
			if (itemInfo.cfg.AdId > 0)
			{
				if (itemInfo.payMax <= 0)
				{
					this.adLimitText.text = "";
				}
				else
				{
					this.adLimitText.text = Singleton<LanguageManager>.Instance.GetInfoByID("buy_limit_times", new object[] { itemInfo.payCount, itemInfo.payMax });
				}
				this.adButton.gameObject.SetActiveSafe(true);
				if (itemInfo.isFree)
				{
					this.adRedNodeOneCtrl.gameObject.SetActiveSafe(true);
					this.adRedNodeOneCtrl.SetType(240);
					this.adRedNodeOneCtrl.Value = 1;
				}
				Action<bool> <>9__2;
				this.adButton.m_onClick = delegate
				{
					int adId = itemInfo.cfg.AdId;
					Action<bool> action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate(bool isSuccess)
						{
							if (isSuccess)
							{
								NetworkUtils.ActivitySlotTrain.RequestTurnPayAd(itemInfo.cfg.id, itemInfo.cfg.AdId);
							}
						});
					}
					AdBridge.PlayRewardVideo(adId, action);
				};
				return;
			}
			if (itemInfo.payMax <= 0)
			{
				this.payLimitText.text = "";
			}
			else
			{
				this.payLimitText.text = Singleton<LanguageManager>.Instance.GetInfoByID("buy_limit_times", new object[] { itemInfo.payCount, itemInfo.payMax });
			}
			this.payButton.gameObject.SetActiveSafe(true);
			if (itemInfo.isFree)
			{
				this.payRedNodeOneCtrl.gameObject.SetActiveSafe(true);
				this.payRedNodeOneCtrl.SetType(240);
				this.payRedNodeOneCtrl.Value = 1;
			}
			this.payButton.SetData(itemInfo.iapData.id, null, delegate(int purchaseId)
			{
				GameApp.Purchase.Manager.Buy(itemInfo.iapData.id, 3, string.Format("{0},{1}", itemInfo.cfg.id, itemInfo.cfg.id), delegate(bool result)
				{
				}, null);
			}, null, null, null);
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

		private ActivitySlotTrainTaskViewType m_viewType;

		public RectTransform taskAniNode;

		public GameObject taskTitleObj;

		public CustomText taskTitleText;

		public GameObject buyTitleObj;

		public CustomText buyTitleText;

		public RectTransform sliderNode;

		public CustomText sliderText;

		public Image sliderImage;

		public float sliderLeftPadding;

		public List<UIItem> leftItems;

		public CustomText finishedText;

		public CustomButton taskButton;

		public CustomText taskButtonText;

		public RedNodeOneCtrl taskNodeOneCtrl;

		public CustomButton taskGoButton;

		public CustomText taskGoButtonText;

		public RedNodeOneCtrl taskGoNodeOneCtrl;

		public PurchaseButtonCtrl payButton;

		public CustomText payLimitText;

		public RedNodeOneCtrl payRedNodeOneCtrl;

		public CustomButton adButton;

		public CustomText adLimitText;

		public RedNodeOneCtrl adRedNodeOneCtrl;

		private GameObject m_TitleObj;

		private CustomText m_TitleText;
	}
}
