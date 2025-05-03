using System;
using Dxx.Chat;
using Framework.Logic.UI;
using UnityEngine.UI;

namespace HotFix.ChatUI.ChatDonationUI
{
	public class ChatDonationCell_Reward : ChatProxy.ChatProxy_BaseBehaviour
	{
		protected override void ChatUI_OnInit()
		{
		}

		protected override void ChatUI_OnUnInit()
		{
		}

		public void SetData(int itemid, int itemcount)
		{
			if (ChatProxy.Table.GetItemTab(itemid) != null)
			{
				throw new NotImplementedException();
			}
			this.Text_Count.text = string.Format("X{0}", itemcount);
		}

		public Image Image_Icon;

		public CustomText Text_Count;
	}
}
