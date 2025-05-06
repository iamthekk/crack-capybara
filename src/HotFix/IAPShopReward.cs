using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class IAPShopReward : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.uiItem.Init();
		}

		protected override void OnDeInit()
		{
			this.uiItem.DeInit();
		}

		public void SetActiveForReceive(bool active)
		{
			this.receiveObj.SetActive(active);
		}

		public void SetGrayState(bool isGray)
		{
			this.uiItem.SetGrayState(isGray);
		}

		public void SetCanClick(bool isCanClick)
		{
			this.uiItem.SetEnableButton(isCanClick);
		}

		public void SetCountShowType(UIItem.CountShowType showType)
		{
			this.uiItem.SetCountShowType(showType);
		}

		public void SetData(PropData propData, int index)
		{
			this.uiItem.SetData(propData);
			this.bg1.SetActiveSafe(index != 0);
			this.bg2.SetActiveSafe(index == 0);
		}

		public void SetOnClick(Action<UIItem, PropData, object> onClick)
		{
			this.uiItem.onClick = onClick;
		}

		public void OnRefresh()
		{
			this.uiItem.OnRefresh();
		}

		[SerializeField]
		private UIItem uiItem;

		[SerializeField]
		private GameObject receiveObj;

		[SerializeField]
		private GameObject bg1;

		[SerializeField]
		private GameObject bg2;
	}
}
