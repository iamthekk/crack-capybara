using System;
using Dxx.Chat;
using Framework.Logic.UI;

namespace HotFix.ChatUI.ChatShareItemUI
{
	public class ChatShareItemCell : ChatProxy.ChatProxy_BaseBehaviour
	{
		public int ItemID
		{
			get
			{
				if (this.mData == null)
				{
					return 0;
				}
				return this.mData.ItemID;
			}
		}

		public int ItemCount
		{
			get
			{
				if (this.mData == null)
				{
					return 0;
				}
				return this.mData.Count;
			}
		}

		protected override void ChatUI_OnInit()
		{
			this.ItemUI.Init();
			this.Button_Item.m_onClick = new Action(this.OnClickItem);
		}

		protected override void ChatUI_OnUnInit()
		{
			UIItem itemUI = this.ItemUI;
			if (itemUI == null)
			{
				return;
			}
			itemUI.DeInit();
		}

		public void SetData(ChatShareItemData data)
		{
			this.mData = data;
		}

		public void RefreshUI()
		{
			throw new NotImplementedException();
		}

		private void OnClickItem()
		{
			if (this.mData == null)
			{
				return;
			}
			throw new NotImplementedException();
		}

		public UIItem ItemUI;

		public CustomButton Button_Item;

		private ChatShareItemData mData;
	}
}
