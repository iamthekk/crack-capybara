using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class PopCommonViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				HLog.LogError("PopCommonViewModule OnOpen data == null");
				return;
			}
			this.m_data = (PopCommonData)data;
			if (this.m_data == null)
			{
				HLog.LogError("PopCommonViewModule OnOpen m_data == null");
				return;
			}
			CustomButton customButton = this.Button_Sure;
			CustomButton customButton2 = this.Button_Cancel;
			CustomText customText = this.Text_Sure;
			CustomText customText2 = this.Text_Cancel;
			if (this.m_data.m_swappingButton)
			{
				customButton = this.Button_Cancel;
				customButton2 = this.Button_Sure;
				customText = this.Text_Cancel;
				customText2 = this.Text_Sure;
				this.Button_Cancel.transform.SetAsLastSibling();
			}
			if (string.IsNullOrEmpty(this.m_data.m_title))
			{
				this.Text_Title.text = Singleton<LanguageManager>.Instance.GetInfoByID("43");
			}
			else
			{
				this.Text_Title.text = this.m_data.m_title;
			}
			this.Text_Content.text = this.m_data.m_content;
			if (string.IsNullOrEmpty(this.m_data.m_sureContent))
			{
				customText.text = Singleton<LanguageManager>.Instance.GetInfoByID("17");
			}
			else
			{
				customText.text = this.m_data.m_sureContent;
			}
			if (string.IsNullOrEmpty(this.m_data.m_cancelContent))
			{
				customText2.text = Singleton<LanguageManager>.Instance.GetInfoByID("18");
			}
			else
			{
				customText2.text = this.m_data.m_cancelContent;
			}
			this.PopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnPopClick);
			this.PopCommon.SetActiveForClose(this.m_data.m_isShowClose);
			customButton.gameObject.SetActive(this.m_data.m_isShowSure);
			customButton.onClick.AddListener(new UnityAction(this.OnClickSure));
			customButton2.gameObject.SetActive(this.m_data.m_isShowCancel);
			customButton2.onClick.AddListener(new UnityAction(this.OnClickCancle));
		}

		private void OnPopClick(UIPopCommon.UIPopCommonClickType type)
		{
			this.OnClickClose();
		}

		private void closeWindow()
		{
			GameApp.View.CloseView(ViewName.PopCommonViewModule, null);
		}

		public override void OnClose()
		{
			if (this.Button_Sure != null)
			{
				this.Button_Sure.onClick.RemoveListener(new UnityAction(this.OnClickSure));
			}
			if (this.Button_Cancel != null)
			{
				this.Button_Cancel.onClick.RemoveListener(new UnityAction(this.OnClickCancle));
			}
		}

		public override void OnDelete()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickClose()
		{
			Action onClose = this.m_data.m_onClose;
			if (onClose != null)
			{
				onClose();
			}
			this.closeWindow();
		}

		private void OnClickCancle()
		{
			Action onCancel = this.m_data.m_onCancel;
			if (onCancel != null)
			{
				onCancel();
			}
			this.closeWindow();
		}

		private void OnClickSure()
		{
			Action onSure = this.m_data.m_onSure;
			if (onSure != null)
			{
				onSure();
			}
			this.closeWindow();
		}

		public RectTransform ForGuide_GetSureButton()
		{
			if (this.Button_Sure == null)
			{
				return null;
			}
			return this.Button_Sure.transform as RectTransform;
		}

		public RectTransform ForGuide_GetCancelButton()
		{
			if (this.Button_Cancel == null)
			{
				return null;
			}
			return this.Button_Cancel.transform as RectTransform;
		}

		public CustomText Text_Title;

		public CustomText Text_Content;

		public CustomText Text_Sure;

		public CustomText Text_Cancel;

		public UIPopCommon PopCommon;

		public CustomButton Button_Sure;

		public CustomButton Button_Cancel;

		private PopCommonData m_data;
	}
}
