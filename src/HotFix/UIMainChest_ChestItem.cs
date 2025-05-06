using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIMainChest_ChestItem : CustomBehaviour
	{
		public void SetCallback(Action<UIMainChest_ChestItem> callback)
		{
			this.callback = callback;
		}

		public void SetData(ChestList_ChestReward cfg)
		{
			this.cfg = cfg;
			this.chestType = cfg.chestType;
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(cfg.itemId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("itemId:{0} not found i item config", cfg.itemId));
				return;
			}
			this.imgChest.SetImage(elementById.atlasID, elementById.icon);
			this.UpdateView();
		}

		public void UpdateView()
		{
			long chestCount = this.chestDataModule.GetChestCount(this.cfg.itemId);
			this.txtChestCount.text = "x" + DxxTools.FormatNumber(chestCount);
			this.redNode.Value = (int)chestCount;
		}

		protected override void OnInit()
		{
			this.goSelect.SetActive(false);
			this.btnItem.m_onClick = new Action(this.OnBtnItemClick);
			this.redNode.Value = 0;
			this.imgIconPosY = this.rectFg.anchoredPosition.y;
			this.chestDataModule = GameApp.Data.GetDataModule(DataName.ChestDataModule);
		}

		protected override void OnDeInit()
		{
			this.btnItem.m_onClick = null;
		}

		private void OnBtnItemClick()
		{
			Action<UIMainChest_ChestItem> action = this.callback;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		public void SetSelect(bool isSelect)
		{
			this.goSelect.SetActive(isSelect);
			Vector2 anchoredPosition = this.rectFg.anchoredPosition;
			anchoredPosition.y = (isSelect ? (this.imgIconPosY + 15f) : this.imgIconPosY);
			this.rectFg.anchoredPosition = anchoredPosition;
		}

		public RectTransform rectFg;

		public GameObject goSelect;

		public CustomButton btnItem;

		public CustomImage imgChest;

		public CustomText txtChestCount;

		public RedNodeOneCtrl redNode;

		[NonSerialized]
		public int chestType;

		private Action<UIMainChest_ChestItem> callback;

		private ChestDataModule chestDataModule;

		private float imgIconPosY;

		public ChestList_ChestReward cfg;
	}
}
