using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class CommonFundRewardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.item.Init();
		}

		protected override void OnDeInit()
		{
			this.item.DeInit();
		}

		public void SetData(PropData propData)
		{
			this.item.SetData(propData);
		}

		public void Refresh()
		{
			this.item.OnRefresh();
		}

		public void SetState(bool isMask, bool isLock, bool isGot, bool isCanGet)
		{
			if (this.maskObj)
			{
				this.maskObj.SetActive(isMask);
			}
			if (this.lockObj)
			{
				this.lockObj.SetActive(isLock);
			}
			if (this.getObj)
			{
				this.getObj.SetActive(isGot);
			}
			if (this.canGetObj)
			{
				this.canGetObj.SetActive(isCanGet);
			}
			if (this.redNode)
			{
				this.redNode.SetActive(isCanGet);
			}
		}

		public void SetClick(Action<UIItem, PropData, object> onClick)
		{
			this.item.onClick = onClick;
		}

		public UIItem item;

		public GameObject lockObj;

		public GameObject maskObj;

		public GameObject getObj;

		public GameObject canGetObj;

		public GameObject redNode;
	}
}
