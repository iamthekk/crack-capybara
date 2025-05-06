using System;
using System.Collections.Generic;
using Dxx.Chat;
using UnityEngine;

namespace HotFix.ChatUI.ChatShareItemUI
{
	public class ChatShareSwitchGroup : ChatProxy.ChatProxy_BaseBehaviour
	{
		protected override void ChatUI_OnInit()
		{
			Transform transform = base.gameObject.transform;
			int childCount = transform.childCount;
			this.ButtonList.Clear();
			for (int i = 0; i < childCount; i++)
			{
				ChatShareSwitchButton component = transform.GetChild(i).GetComponent<ChatShareSwitchButton>();
				component.Init();
				component.OnClick = new Action<ChatShareSwitchButton>(this.OnClickButton);
				this.ButtonList.Add(component);
			}
		}

		protected override void ChatUI_OnUnInit()
		{
			foreach (ChatShareSwitchButton chatShareSwitchButton in this.ButtonList)
			{
				chatShareSwitchButton.DeInit();
			}
			this.ButtonList.Clear();
		}

		private void OnClickButton(ChatShareSwitchButton button)
		{
			if (button == this.CurrentButton)
			{
				return;
			}
			for (int i = 0; i < this.ButtonList.Count; i++)
			{
				ChatShareSwitchButton chatShareSwitchButton = this.ButtonList[i];
				if (chatShareSwitchButton == button)
				{
					this.CurrentIndex = i;
					this.CurrentButton = chatShareSwitchButton;
					this.CurrentName = chatShareSwitchButton.ButtonName;
				}
				chatShareSwitchButton.SetSelect(chatShareSwitchButton == button);
			}
			Action onSwitch = this.OnSwitch;
			if (onSwitch == null)
			{
				return;
			}
			onSwitch();
		}

		public void SwitchByName(string btnname, bool force = false)
		{
			if (!force && btnname == this.CurrentName)
			{
				return;
			}
			for (int i = 0; i < this.ButtonList.Count; i++)
			{
				ChatShareSwitchButton chatShareSwitchButton = this.ButtonList[i];
				if (chatShareSwitchButton.ButtonName == btnname)
				{
					this.CurrentIndex = i;
					this.CurrentButton = chatShareSwitchButton;
					this.CurrentName = chatShareSwitchButton.ButtonName;
				}
				chatShareSwitchButton.SetSelect(chatShareSwitchButton.ButtonName == btnname);
			}
			Action onSwitch = this.OnSwitch;
			if (onSwitch == null)
			{
				return;
			}
			onSwitch();
		}

		public void SwitchByIndex(int btnindex, bool force = false)
		{
			if (!force && btnindex == this.CurrentIndex)
			{
				return;
			}
			if (btnindex < 0 || btnindex >= this.ButtonList.Count)
			{
				return;
			}
			for (int i = 0; i < this.ButtonList.Count; i++)
			{
				ChatShareSwitchButton chatShareSwitchButton = this.ButtonList[i];
				if (i == btnindex)
				{
					this.CurrentIndex = i;
					this.CurrentButton = chatShareSwitchButton;
					this.CurrentName = chatShareSwitchButton.ButtonName;
				}
				chatShareSwitchButton.SetSelect(i == btnindex);
			}
			Action onSwitch = this.OnSwitch;
			if (onSwitch == null)
			{
				return;
			}
			onSwitch();
		}

		public List<ChatShareSwitchButton> ButtonList = new List<ChatShareSwitchButton>();

		public int CurrentIndex;

		public ChatShareSwitchButton CurrentButton;

		public string CurrentName;

		public Action OnSwitch;
	}
}
