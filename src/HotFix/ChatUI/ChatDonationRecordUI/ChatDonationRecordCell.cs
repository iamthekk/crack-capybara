using System;
using Dxx.Chat;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.ChatUI.ChatDonationRecordUI
{
	public class ChatDonationRecordCell : ChatProxy.ChatProxy_BaseBehaviour
	{
		protected override void ChatUI_OnInit()
		{
			this.Text_Record.text = "";
			this.RewardItem.Init();
		}

		protected override void ChatUI_OnUnInit()
		{
			if (this.RewardItem != null)
			{
				this.RewardItem.DeInit();
			}
		}

		public virtual void SetData(GuildDonationRecord data)
		{
			this.mRecord = data;
		}

		public virtual void RefreshUI()
		{
			if (this.mRecord == null)
			{
				return;
			}
			PropData propData = new PropData();
			propData.rowId = 0UL;
			propData.id = (uint)this.mRecord.itemId;
			propData.count = (ulong)this.mRecord.itemCount;
			this.RewardItem.SetData(propData);
			this.RewardItem.OnRefresh();
			if (this.mRecord.type == 1)
			{
				this.RefreshUI1();
				return;
			}
			if (this.mRecord.type == 2)
			{
				this.RefreshUI2();
			}
		}

		public virtual void RefreshUI1()
		{
			if (this.mRecord == null)
			{
				return;
			}
			string nickName = ChatProxy.Language.GetNickName(this.mRecord.toUserNickName, (ulong)this.mRecord.toUserId);
			string infoByID1_LogError = ChatProxy.Language.GetInfoByID1_LogError(420819, nickName);
			this.Text_Record.text = infoByID1_LogError;
			Vector2 sizeDelta = this.Text_Record.rectTransform.sizeDelta;
			sizeDelta.x = this.Text_Record.preferredWidth + 10f;
			this.Text_Record.rectTransform.sizeDelta = sizeDelta;
		}

		public virtual void RefreshUI2()
		{
			if (this.mRecord == null)
			{
				return;
			}
			string nickName = ChatProxy.Language.GetNickName(this.mRecord.fromNickName, (ulong)this.mRecord.fromUserId);
			string infoByID1_LogError = ChatProxy.Language.GetInfoByID1_LogError(420818, nickName);
			this.Text_Record.text = infoByID1_LogError;
			Vector2 sizeDelta = this.Text_Record.rectTransform.sizeDelta;
			sizeDelta.x = this.Text_Record.preferredWidth + 10f;
			this.Text_Record.rectTransform.sizeDelta = sizeDelta;
		}

		public CustomText Text_Record;

		public UIItem RewardItem;

		protected GuildDonationRecord mRecord;
	}
}
