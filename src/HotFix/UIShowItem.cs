using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIShowItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.buttonSelf.onClick.AddListener(new UnityAction(this.OnClickSelf));
			this.uiItem.Init();
		}

		protected override void OnDeInit()
		{
			this.buttonSelf.onClick.RemoveListener(new UnityAction(this.OnClickSelf));
			this.uiItem.DeInit();
		}

		public void SetData(int index, int itemId)
		{
			this.mIndex = index;
			PropData propData = new PropData();
			propData.id = (uint)itemId;
			propData.count = 0UL;
			this.uiItem.SetData(propData);
			this.uiItem.OnRefresh();
			this.uiItem.SetCountText("");
			this.uiItem.SetClickDisabled(true);
		}

		public void Refresh(int selectIndex, bool isUnLock, bool isRedPoint)
		{
			this.selectObj.SetActiveSafe(this.mIndex == selectIndex);
			this.lockObj.SetActiveSafe(!isUnLock);
			this.lockMask.SetActiveSafe(!isUnLock);
			this.redPoint.gameObject.SetActive(isRedPoint);
		}

		public int GetIndex()
		{
			return this.mIndex;
		}

		private void OnClickSelf()
		{
			EventArgsInt eventArgsInt = new EventArgsInt();
			eventArgsInt.SetData(this.mIndex);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIMount_SelectAdvance, eventArgsInt);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIArtifact_SelectAdvance, eventArgsInt);
		}

		public CustomButton buttonSelf;

		public UIItem uiItem;

		public GameObject selectObj;

		public GameObject lockMask;

		public GameObject lockObj;

		public RedNodeOneCtrl redPoint;

		private int mIndex;
	}
}
