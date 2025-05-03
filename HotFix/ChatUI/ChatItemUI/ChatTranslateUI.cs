using System;
using Dxx.Chat;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.ChatUI.ChatItemUI
{
	public class ChatTranslateUI : ChatProxy.ChatProxy_BaseBehaviour
	{
		protected override void ChatUI_OnInit()
		{
			this.BtnTranslate.m_onClick = new Action(this.OnClickTranslateOrRevert);
		}

		protected override void ChatUI_OnUnInit()
		{
			this.BtnTranslate.m_onClick = null;
		}

		private void OnClickTranslateOrRevert()
		{
			switch (this.CurState)
			{
			case ChatTranslateState.NotTranslate:
			{
				Action onTranslate = this.OnTranslate;
				if (onTranslate == null)
				{
					return;
				}
				onTranslate();
				return;
			}
			case ChatTranslateState.Translating:
				return;
			case ChatTranslateState.Translated:
			{
				Action onRevert = this.OnRevert;
				if (onRevert == null)
				{
					return;
				}
				onRevert();
				return;
			}
			case ChatTranslateState.Revert:
			{
				Action onTranslate2 = this.OnTranslate;
				if (onTranslate2 == null)
				{
					return;
				}
				onTranslate2();
				return;
			}
			default:
				return;
			}
		}

		public void SwitchState(ChatTranslateState state)
		{
			this.CurState = state;
			this.RefreshUI();
		}

		public void RefreshUI()
		{
			switch (this.CurState)
			{
			case ChatTranslateState.NotTranslate:
				this.ObjLoading.SetActive(false);
				this.ObjBtnTranslate.SetActive(true);
				return;
			case ChatTranslateState.Translating:
				this.ObjLoading.SetActive(true);
				this.ObjBtnTranslate.SetActive(false);
				return;
			case ChatTranslateState.Translated:
				this.ObjLoading.SetActive(false);
				this.ObjBtnTranslate.SetActive(true);
				return;
			case ChatTranslateState.Revert:
				this.ObjLoading.SetActive(false);
				this.ObjBtnTranslate.SetActive(true);
				return;
			default:
				return;
			}
		}

		public GameObject ObjLoading;

		public GameObject ObjBtnTranslate;

		public CustomButton BtnTranslate;

		public ChatTranslateState CurState;

		public Action OnTranslate;

		public Action OnRevert;
	}
}
