using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIActivitySlotTrainBigSelectViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonClose.m_onClick = new Action(this.OnClickClose);
			this.buttonMask.m_onClick = new Action(this.OnClickClose);
			this.itemPrefab.SetActive(false);
		}

		public override void OnDelete()
		{
			this.buttonClose.m_onClick = null;
			this.buttonMask.m_onClick = null;
		}

		public override void OnOpen(object data)
		{
			this.dataModule = GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule);
			this.mainCfg = GameApp.Table.GetManager().GetActivityTurntable_ActivityTurntableModelInstance().GetElementById(this.dataModule.TurntableId);
			this.bigPoolCfg = GameApp.Table.GetManager().GetItem_dropModelInstance().GetElementById(int.Parse(this.mainCfg.pool[2]));
			this.FreshTexts();
			this.RefreshItems();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			foreach (UIActivitySlotTrainBigItemCtrl uiactivitySlotTrainBigItemCtrl in this.itemCacheList)
			{
				uiactivitySlotTrainBigItemCtrl.DeInit();
			}
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void FreshTexts()
		{
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_selectbig_title");
			this.textTopDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_selectbig_tipup");
			this.textBottomDescTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_selectbig_tipdowntitle");
			this.textBottomDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_selectbig_tipdown");
		}

		private void RefreshItems()
		{
			foreach (UIActivitySlotTrainBigItemCtrl uiactivitySlotTrainBigItemCtrl in this.itemCacheList)
			{
				uiactivitySlotTrainBigItemCtrl.gameObject.SetActive(false);
			}
			this.itemCtrls.Clear();
			for (int i = 0; i < this.bigPoolCfg.reward.Length; i++)
			{
				UIActivitySlotTrainBigItemCtrl item;
				if (i < this.itemCacheList.Count)
				{
					item = this.itemCacheList[i];
				}
				else
				{
					item = Object.Instantiate<UIActivitySlotTrainBigItemCtrl>(this.itemPrefab, this.itemParent);
					this.itemCacheList.Add(item);
				}
				if (!item.gameObject.activeSelf)
				{
					item.gameObject.SetActive(true);
				}
				item.Init();
				string[] array = this.bigPoolCfg.reward[i].Split(',', StringSplitOptions.None);
				ItemData itemData = new ItemData(int.Parse(array[0]), long.Parse(array[1]));
				item.uiItem.SetData(itemData.ToPropData());
				item.uiItem.OnRefresh();
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID);
				item.textName.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
				item.FreshSpine();
				item.btnSelect.m_onClick = delegate
				{
					this.OnClickItemCtrl(item);
				};
				this.itemCtrls.Add(item);
			}
			this.SelectBigItem();
		}

		private void SelectBigItem()
		{
			uint num = (uint)this.dataModule.BigGuaranteeItemConfigId;
			if (num <= 0U)
			{
				num = uint.Parse(this.bigPoolCfg.reward[0].Split(',', StringSplitOptions.None)[0]);
			}
			foreach (UIActivitySlotTrainBigItemCtrl uiactivitySlotTrainBigItemCtrl in this.itemCtrls)
			{
				uiactivitySlotTrainBigItemCtrl.objSelected.SetActive(uiactivitySlotTrainBigItemCtrl.uiItem.m_propData.id == num);
			}
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.ActivitySlotTrainBigSelectViewModule, null);
		}

		private void OnClickItemCtrl(UIActivitySlotTrainBigItemCtrl item)
		{
			NetworkUtils.ActivitySlotTrain.RequestTurnTableSelectBigGuaranteeItem((int)item.uiItem.m_propData.id, delegate(bool success)
			{
				if (success)
				{
					this.SelectBigItem();
				}
			});
		}

		public CustomButton buttonMask;

		public CustomButton buttonClose;

		public CustomText textTitle;

		public CustomText textTopDesc;

		public CustomText textBottomDescTitle;

		public CustomText textBottomDesc;

		public RectTransform itemParent;

		public UIActivitySlotTrainBigItemCtrl itemPrefab;

		private List<UIActivitySlotTrainBigItemCtrl> itemCtrls = new List<UIActivitySlotTrainBigItemCtrl>();

		private List<UIActivitySlotTrainBigItemCtrl> itemCacheList = new List<UIActivitySlotTrainBigItemCtrl>();

		private ActivitySlotTrainDataModule dataModule;

		private float activityTimeLeft;

		private int posCount = 15;

		private ActivityTurntable_ActivityTurntable mainCfg;

		private Item_drop bigPoolCfg;
	}
}
