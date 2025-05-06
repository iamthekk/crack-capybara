using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class ChainPacksNodeCtrl : CustomBehaviour
	{
		public int ActId { get; private set; }

		public ChainPacksDataModule.EChainPacksOriginType ActOriginType { get; private set; }

		public ChainPacks_ChainPacks packCfg { get; private set; }

		public int dataIndex { get; private set; }

		protected override void OnInit()
		{
			this.btnFree.m_onClick = new Action(this.OnClickBtnFree);
			this.btnFree.gameObject.SetActive(false);
			this.payButton.gameObject.SetActive(false);
			this.redNodeOneCtrl.gameObject.SetActive(false);
			if (this.firstInit)
			{
				this.contentDefaultPos = this.itemContent.localPosition;
			}
		}

		protected override void OnDeInit()
		{
			this.btnFree.m_onClick = null;
			foreach (UIItem uiitem in this.itemList)
			{
				this.viewModule.ReleasePoolItem(uiitem);
			}
			this.itemList.Clear();
		}

		public void SetFresh(ChainPacksViewModule view, int actId, ChainPacksDataModule.EChainPacksOriginType actOriginType, ChainPacks_ChainPacks cfg, int index)
		{
			this.dataModule = GameApp.Data.GetDataModule(DataName.ChainPacksDataModule);
			this.viewModule = view;
			this.ActId = actId;
			this.ActOriginType = actOriginType;
			this.packCfg = cfg;
			this.dataIndex = index;
			this.itemList.Clear();
			this.itemContent.localPosition = this.contentDefaultPos;
			this.FreshTheme();
			for (int i = 0; i < this.packCfg.rewards.Length; i++)
			{
				string[] array = this.packCfg.rewards[i].Split(',', StringSplitOptions.None);
				ItemData itemData = new ItemData(int.Parse(array[0]), long.Parse(array[1]));
				UIItem poolItem = this.viewModule.GetPoolItem();
				this.itemList.Add(poolItem);
				Transform transform = poolItem.transform;
				transform.SetParent(this.itemContent);
				transform.SetSiblingIndex(i);
				transform.localPosition = Vector3.zero;
				transform.localScale = this.itemScale;
				poolItem.Init();
				poolItem.SetData(itemData.ToPropData());
				poolItem.OnRefresh();
			}
			this.objArrow.SetActive(this.dataIndex != 0);
			this.FreshState(false);
		}

		private void FreshTheme()
		{
			this.imageNodeBg_Current.SetSprite(this.viewModule.spriteRegister.GetSprite(this.viewModule.actInfo.actTypeCfg.sprite_NodeBg_Current));
			this.imageNodeBg_Other.SetSprite(this.viewModule.spriteRegister.GetSprite(this.viewModule.actInfo.actTypeCfg.sprite_NodeBg_Other));
		}

		public void FreshState(bool pickedRefresh)
		{
			int pickedIndex = this.dataModule.GetPickedIndex(this.ActId);
			bool flag = this.dataIndex == pickedIndex && this.dataModule.GetActEndIndex(this.ActId) == this.dataIndex;
			if (this.dataIndex == pickedIndex + 1 || flag)
			{
				this.bgCurrent.SetActiveSafe(true);
				this.bgOther.SetActiveSafe(false);
			}
			else
			{
				this.bgCurrent.SetActiveSafe(false);
				this.bgOther.SetActiveSafe(true);
			}
			if (this.packCfg.PurchaseId == 0 && (this.dataIndex == pickedIndex + 1 || flag))
			{
				this.redNodeOneCtrl.gameObject.SetActiveSafe(true);
				this.redNodeOneCtrl.SetType(240);
				this.redNodeOneCtrl.Value = 1;
			}
			else
			{
				this.redNodeOneCtrl.gameObject.SetActiveSafe(false);
			}
			if (this.dataIndex <= pickedIndex)
			{
				this.getedObj.SetActiveSafe(true);
				this.btnFree.gameObject.SetActiveSafe(false);
				this.payButton.gameObject.SetActiveSafe(false);
				return;
			}
			this.getedObj.SetActiveSafe(false);
			if (this.packCfg.PurchaseId != 0)
			{
				this.btnFree.gameObject.SetActiveSafe(false);
				this.payButton.gameObject.SetActiveSafe(true);
				if (!pickedRefresh)
				{
					this.payButton.SetData(this.packCfg.PurchaseId, null, new Action<int>(this.OnClickBtnPay), null, null, null);
				}
				this.lockBtnPay.gameObject.SetActiveSafe(this.dataIndex > pickedIndex + 1);
				return;
			}
			this.btnFree.gameObject.SetActiveSafe(true);
			this.payButton.gameObject.SetActiveSafe(false);
			this.lockBtnFree.gameObject.SetActiveSafe(this.dataIndex > pickedIndex + 1);
			if (this.dataIndex <= pickedIndex)
			{
				this.textBtnFree.text = Singleton<LanguageManager>.Instance.GetInfoByID("Global_Geted");
				return;
			}
			this.textBtnFree.text = Singleton<LanguageManager>.Instance.GetInfoByID("common_free");
		}

		private void OnClickBtnFree()
		{
			if (!this.CheckCanPick())
			{
				return;
			}
			int id = this.packCfg.id;
			if (this.ActOriginType == ChainPacksDataModule.EChainPacksOriginType.Common)
			{
				NetworkUtils.ChainPacks.DoFreePickChainPacksRewardRequest(this.ActId, id, true, null);
				return;
			}
			if (this.ActOriginType == ChainPacksDataModule.EChainPacksOriginType.Push)
			{
				NetworkUtils.ChainPacks.DoFreePickChainPacksPushRewardRequest(this.ActId, id, true, null);
			}
		}

		private void OnClickBtnPay(int purchaseId)
		{
			if (!this.CheckCanPick())
			{
				return;
			}
			int pkId = this.packCfg.id;
			GameApp.Purchase.Manager.Buy(this.packCfg.PurchaseId, 14, string.Format("{0},{1}", this.ActId, pkId), delegate(bool result)
			{
				if (result)
				{
					GameApp.Data.GetDataModule(DataName.ChainPacksDataModule).NetPickedReward(this.ActId, pkId);
				}
			}, null);
		}

		private bool CheckCanPick()
		{
			int num = this.dataModule.GetPickedIndex(this.ActId) + 1;
			if (num < this.dataIndex)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("chainpacks_unlock"));
				return false;
			}
			return num == this.dataIndex;
		}

		public RectTransform itemContent;

		public Vector3 itemScale;

		public GameObject objArrow;

		public GameObject bgCurrent;

		public GameObject bgOther;

		public CustomButton btnFree;

		public CustomText textBtnFree;

		public GameObject lockBtnFree;

		public PurchaseButtonCtrl payButton;

		public GameObject lockBtnPay;

		public GameObject getedObj;

		public RedNodeOneCtrl redNodeOneCtrl;

		[Header("主题")]
		public CustomImage imageNodeBg_Current;

		public CustomImage imageNodeBg_Other;

		private ChainPacksViewModule viewModule;

		private List<UIItem> itemList = new List<UIItem>();

		private ChainPacksDataModule dataModule;

		private Vector3 contentDefaultPos;

		private bool firstInit = true;
	}
}
