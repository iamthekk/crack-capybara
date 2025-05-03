using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class ShopActivitySUpBigRewardSelectViewModule_Item : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.btnSelect.m_onClick = new Action(this.OnBtnSelectClick);
			this.item.Init();
		}

		protected override void OnDeInit()
		{
			this.btnSelect.m_onClick = null;
			this.item.DeInit();
		}

		public void UpdateData(int index, object data)
		{
			this.index = index;
			this.equipId = (int)data;
			ItemData itemData = new ItemData((int)data, 1L);
			this.item.SetData(itemData.ToPropData());
			this.item.OnRefresh();
		}

		public void SetSelect(bool isSelect)
		{
			this.goSelectFrame.SetActive(isSelect);
			this.goSelect.SetActive(isSelect);
		}

		private void OnBtnSelectClick()
		{
			Action<ShopActivitySUpBigRewardSelectViewModule_Item> action = this.onClickCallback;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		public CustomButton btnSelect;

		public UIItem item;

		public GameObject goSelectFrame;

		public GameObject goSelect;

		[NonSerialized]
		public Action<ShopActivitySUpBigRewardSelectViewModule_Item> onClickCallback;

		[NonSerialized]
		public int index;

		[NonSerialized]
		public int equipId;
	}
}
