using System;
using Dxx.Chat;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.ChatUI
{
	public class ChatMoreInputCtrl : ChatProxy.ChatProxy_BaseBehaviour
	{
		protected override void ChatUI_OnInit()
		{
			this.Animator_More.Play("none");
			this.Button_More.m_onClick = new Action(this.OnSwitchShowOrHideMore);
			this.Button_Share.m_onClick = new Action(this.OnOpenShareView);
			this.Button_Donation.m_onClick = new Action(this.OnOpenDonationView);
		}

		protected override void ChatUI_OnUnInit()
		{
		}

		public void OnShow()
		{
			this.Animator_More.Play("none");
			this.mIsShow = false;
		}

		public void OnClose()
		{
		}

		public void SwitchToHide()
		{
			if (!this.mIsShow)
			{
				return;
			}
			this.mIsShow = false;
			this.Animator_More.Play(this.mIsShow ? "show" : "hide");
		}

		private void OnSwitchShowOrHideMore()
		{
			this.mIsShow = !this.mIsShow;
			this.Animator_More.Play(this.mIsShow ? "show" : "hide");
		}

		private void OnOpenShareView()
		{
			this.SwitchToHide();
			ChatProxy.UI.OpenChatShareView(null);
		}

		private void OnOpenDonationView()
		{
			this.SwitchToHide();
			ChatProxy.UI.OpenChatDonationView(null);
		}

		public CustomButton Button_More;

		public Animator Animator_More;

		public CustomButton Button_Share;

		public CustomButton Button_Donation;

		private bool mIsShow;
	}
}
