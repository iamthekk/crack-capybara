using System;
using Dxx.Chat;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.ChatUI.ChatShareItemUI
{
	public class ChatShareSwitchButton : ChatProxy.ChatProxy_BaseBehaviour
	{
		protected override void ChatUI_OnInit()
		{
			this.ButtonName = base.gameObject.name;
			this.Button_Switch.m_onClick = new Action(this.OnClickButton);
		}

		protected override void ChatUI_OnUnInit()
		{
			if (this.Button_Switch != null)
			{
				this.Button_Switch.m_onClick = null;
			}
		}

		private void OnClickButton()
		{
			Action<ChatShareSwitchButton> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(this);
		}

		public void SetSelect(bool sel)
		{
			GameObject obj_Select = this.Obj_Select;
			if (obj_Select != null)
			{
				obj_Select.SetActive(sel);
			}
			GameObject obj_UnSelect = this.Obj_UnSelect;
			if (obj_UnSelect == null)
			{
				return;
			}
			obj_UnSelect.SetActive(!sel);
		}

		public CustomButton Button_Switch;

		public GameObject Obj_Select;

		public GameObject Obj_UnSelect;

		[Label]
		public string ButtonName;

		public Action<ChatShareSwitchButton> OnClick;
	}
}
