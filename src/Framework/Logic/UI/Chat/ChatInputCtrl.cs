using System;
using Framework.Logic.Platform;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework.Logic.UI.Chat
{
	public class ChatInputCtrl : MonoBehaviour
	{
		public TouchScreenKeyboard.Status LastTouchScreenKeyboardStatus
		{
			get
			{
				return this.mTouchScreenKeyboardStatus;
			}
		}

		public void OnEnable()
		{
			if (this.MainInput != null)
			{
				this.MainInput.onEndEdit.RemoveAllListeners();
				this.MainInput.onEndEdit.AddListener(new UnityAction<string>(this.OnInputEndEdit));
			}
		}

		private void OnInputEndEdit(string text)
		{
			Action onEndEdit = this.OnEndEdit;
			if (onEndEdit == null)
			{
				return;
			}
			onEndEdit();
		}

		public string GetInputContent()
		{
			if (this.MainInput != null)
			{
				return this.MainInput.text;
			}
			return "";
		}

		private void Update()
		{
			if (this.MainInput == null)
			{
				return;
			}
			if (this.mInputFocus != this.MainInput.isFocused)
			{
				this.mInputFocus = this.MainInput.isFocused;
				Action<bool> onInputFocus = this.OnInputFocus;
				if (onInputFocus != null)
				{
					onInputFocus(this.mInputFocus);
				}
			}
			TouchScreenKeyboard touchScreenKeyboard = this.MainInput.touchScreenKeyboard;
			if (touchScreenKeyboard == null)
			{
				return;
			}
			bool flag = this.mInputFocus;
			if (this.mTouchScreenKeyboardStatus != touchScreenKeyboard.status)
			{
				this.mTouchScreenKeyboardStatus = touchScreenKeyboard.status;
				TouchScreenKeyboard.Status status = touchScreenKeyboard.status;
				if (status != null)
				{
					if (status - 1 > 2)
					{
					}
					flag = false;
				}
				else
				{
					flag = true;
					Action<bool> onInputFocus2 = this.OnInputFocus;
					if (onInputFocus2 != null)
					{
						onInputFocus2(this.mInputFocus);
					}
				}
			}
			if (this.mInputFocus != flag)
			{
				this.mInputFocus = flag;
				Action<bool> onInputFocus3 = this.OnInputFocus;
				if (onInputFocus3 != null)
				{
					onInputFocus3(this.mInputFocus);
				}
			}
			if (flag)
			{
				float num = (float)Singleton<PlatformHelper>.Instance.GetKeyboardHeight();
				if (this.mLastKeyboardHeight != num)
				{
					this.mLastKeyboardHeight = num;
					Action<float> onKeyboardHeightChange = this.OnKeyboardHeightChange;
					if (onKeyboardHeightChange == null)
					{
						return;
					}
					onKeyboardHeightChange(num);
				}
			}
		}

		public InputField MainInput;

		public Action<float> OnKeyboardHeightChange;

		private float mLastKeyboardHeight;

		public Action OnEndEdit;

		public Action<bool> OnInputFocus;

		private bool mInputFocus;

		private TouchScreenKeyboard.Status mTouchScreenKeyboardStatus;
	}
}
