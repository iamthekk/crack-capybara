using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIActivitySlotTrainBigItemCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.uiItem.Init();
			this.uiItem.onClick = new Action<UIItem, PropData, object>(this.OnClickItem);
		}

		protected override void OnDeInit()
		{
			this.uiItem.DeInit();
			this.uiItem.onClick = null;
		}

		public void FreshSpine()
		{
			ItemType itemType;
			int num;
			if (!PropDataModule.TryGetModelIdByItemId((int)this.uiItem.m_propData.id, out itemType, out num))
			{
				this.uiItemIconObj.SetActiveSafe(true);
				this.spineModelItem.gameObject.SetActiveSafe(false);
				this.spineMountModelItem.gameObject.SetActiveSafe(false);
				return;
			}
			this.uiItemIconObj.SetActiveSafe(false);
			if (itemType == ItemType.Mount)
			{
				this.spineModelItem.gameObject.SetActiveSafe(false);
				this.spineMountModelItem.gameObject.SetActiveSafe(true);
				this.spineMountModelItem.ShowModel(num, "Idle", true);
				return;
			}
			this.spineModelItem.gameObject.SetActiveSafe(true);
			this.spineMountModelItem.gameObject.SetActiveSafe(false);
			this.spineModelItem.ShowMemberModel(num, "Idle", true);
		}

		private void OnClickItem(UIItem item, PropData data, object arg)
		{
			DxxTools.UI.ShowItemInfo(new ItemInfoOpenData
			{
				m_propData = data,
				m_openDataType = ItemInfoOpenDataType.eBag,
				m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume)
			}, item.transform.position, 70f);
		}

		public UIItem uiItem;

		public CustomText textName;

		public GameObject objSelected;

		public CustomButton btnSelect;

		public GameObject uiItemIconObj;

		public UISpineModelItem spineModelItem;

		public UISpineMountModelItem spineMountModelItem;
	}
}
