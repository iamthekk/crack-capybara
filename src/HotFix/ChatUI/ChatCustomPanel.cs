using System;
using Dxx.Chat;
using Framework.Logic.AttributeExpansion;

namespace HotFix.ChatUI
{
	public class ChatCustomPanel : ChatProxy.ChatProxy_BaseBehaviour
	{
		protected override void ChatUI_OnInit()
		{
			this.OutputUI.Init();
			this.OutputUI.OnClickViewArea = new Action(this.OnClickViewArea);
			this.InputUI.Init();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.OutputUI != null)
			{
				this.OutputUI.OnUpdate();
			}
		}

		protected override void ChatUI_OnUnInit()
		{
			this.OnClose();
			this.OutputUI.DeInit();
			this.InputUI.DeInit();
		}

		public void OnShow()
		{
			if (this.isShow)
			{
				return;
			}
			this.isShow = true;
			this.OutputUI.OnShow();
			this.InputUI.OnShow();
		}

		public void OnClose()
		{
			if (!this.isShow)
			{
				return;
			}
			this.isShow = false;
			this.OutputUI.OnClose();
			this.InputUI.OnClose();
		}

		public void SetChatGroup(SocketGroupType groupType, string groupId)
		{
			this.ChatGroupID = groupId;
			this.ChatGroupType = groupType;
			this.OutputUI.OnSwitchChannel(groupType, this.ChatGroupID);
			this.OutputUI.RefreshAllUI(true);
			this.InputUI.OnSwitchChannel(groupType, groupId);
		}

		private void OnClickViewArea()
		{
			this.InputUI.OnCalcleInput();
		}

		public void ResetListView()
		{
			this.OutputUI.ResetListView();
		}

		public void OnLanguageChange()
		{
			if (this.OutputUI != null)
			{
				this.OutputUI.RefreshAllUI(false);
			}
		}

		public ChatOutputUI OutputUI;

		public ChatInputUI InputUI;

		[Label]
		public string ChatGroupID;

		[Label]
		public SocketGroupType ChatGroupType;

		private bool isShow;
	}
}
