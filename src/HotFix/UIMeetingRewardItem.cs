using System;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIMeetingRewardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.uiItem.Init();
		}

		protected override void OnDeInit()
		{
			this.uiItem.DeInit();
		}

		public void SetData(PropData propData, IAP_PushPacks gift, int day, Action onClickItem)
		{
			this.OnClickSelf = onClickItem;
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			bool flag = dataModule.MeetingGift.IsBought(gift.id);
			bool flag2 = dataModule.MeetingGift.IsGet(gift.id, day);
			bool flag3 = dataModule.MeetingGift.IsCanGet(gift.id, day);
			if (flag && flag3)
			{
				this.uiItem.onClick = new Action<UIItem, PropData, object>(this.OnClickItem);
			}
			else
			{
				this.uiItem.onClick = new Action<UIItem, PropData, object>(this.uiItem.OnBtnItemClick);
			}
			this.uiItem.SetData(propData);
			this.uiItem.OnRefresh();
			this.SetReceive(flag2);
			this.SetCanClick(!flag2);
			this.redNode.SetActiveSafe(flag3);
		}

		private void SetReceive(bool isGet)
		{
			this.receiveObj.SetActiveSafe(isGet);
		}

		private void SetCanClick(bool isCanClick)
		{
			this.uiItem.SetEnableButton(isCanClick);
		}

		private void OnClickItem(UIItem item, PropData propData, object arg)
		{
			Action onClickSelf = this.OnClickSelf;
			if (onClickSelf == null)
			{
				return;
			}
			onClickSelf();
		}

		public UIItem uiItem;

		public GameObject receiveObj;

		public GameObject redNode;

		private Action OnClickSelf;
	}
}
