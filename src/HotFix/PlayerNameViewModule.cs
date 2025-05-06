using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Proto.User;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class PlayerNameViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			if (this.m_maskBt != null)
			{
				this.m_maskBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			}
			if (this.m_closeBt != null)
			{
				this.m_closeBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			}
			if (this.m_okBt != null)
			{
				this.m_okBt.onClick.AddListener(new UnityAction(this.OnClickOkBt));
			}
			if (this.m_cancelBt != null)
			{
				this.m_cancelBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			}
			if (this.m_nameInput != null)
			{
				this.m_nameInput.characterLimit = Singleton<GameConfig>.Instance.NickName_MaxLength;
				this.m_nameInput.onValidateInput = new InputField.OnValidateInput(this.OnValidateInput);
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			if (this.m_maskBt != null)
			{
				this.m_maskBt.onClick.RemoveAllListeners();
			}
			if (this.m_closeBt != null)
			{
				this.m_closeBt.onClick.RemoveAllListeners();
			}
			if (this.m_okBt != null)
			{
				this.m_okBt.onClick.RemoveAllListeners();
			}
			if (this.m_cancelBt != null)
			{
				this.m_cancelBt.onClick.RemoveAllListeners();
			}
			if (this.m_nameInput != null)
			{
				this.m_nameInput.characterLimit = Singleton<GameConfig>.Instance.NickName_MaxLength;
				this.m_nameInput.onValidateInput = null;
			}
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickCloseBt()
		{
			GameApp.View.CloseView(ViewName.PlayerNameViewModule, null);
		}

		private void OnClickOkBt()
		{
			if (this.m_nameInput == null)
			{
				return;
			}
			string text = this.m_nameInput.text;
			if (string.IsNullOrWhiteSpace(text))
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("80003");
				GameApp.View.ShowStringTip(infoByID);
				return;
			}
			NetworkUtils.PlayerData.DoUserUpdateInfoRequest(text, 0U, 0U, delegate(bool result, UserUpdateInfoResponse resp)
			{
				if (!result)
				{
					return;
				}
				GameApp.View.CloseView(ViewName.PlayerNameViewModule, null);
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("186"));
			}, true);
		}

		private char OnValidateInput(string text, int charIndex, char addedChar)
		{
			if (!DxxTools.Char.CheckLength(text, addedChar, Singleton<GameConfig>.Instance.NickName_MaxLength))
			{
				return '\0';
			}
			if (DxxTools.Char.CheckEmoji(addedChar))
			{
				return '\0';
			}
			return addedChar;
		}

		public CustomButton m_maskBt;

		public CustomButton m_closeBt;

		public InputField m_nameInput;

		public CustomButton m_okBt;

		public CustomButton m_cancelBt;
	}
}
