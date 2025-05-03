using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class RememberTipCommonViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			GameApp.Data.GetDataModule(DataName.ChestDataModule);
		}

		public override void OnOpen(object data)
		{
			this.openData = data as RememberTipCommonViewModule.OpenData;
			this.UpdateView();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.btnConfirm.m_onClick = new Action(this.OnConfirmClick);
			this.btnCancel.m_onClick = new Action(this.OnCancelClick);
			this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChanged));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.btnConfirm.m_onClick = null;
			this.btnCancel.m_onClick = null;
			this.toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnToggleValueChanged));
		}

		private void UpdateView()
		{
			this.toggle.gameObject.SetActive(this.openData.rememberTipType > RememberTipType.None);
			if (this.openData.rememberTipType != RememberTipType.None)
			{
				CommonDataModule dataModule = GameApp.Data.GetDataModule(DataName.CommonDataModule);
				this.toggle.isOn = dataModule.GetRememberTipState(this.openData.rememberTipType);
			}
			this.txtContent.text = this.openData.contentStr;
			if (string.IsNullOrEmpty(this.openData.titleStr))
			{
				this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("title_system_tip");
			}
			else
			{
				this.txtTitle.text = this.openData.titleStr;
			}
			if (string.IsNullOrEmpty(this.openData.btnCancelStr))
			{
				this.txtBtnCancel.text = Singleton<LanguageManager>.Instance.GetInfoByID("Global_Cancel");
			}
			else
			{
				this.txtBtnCancel.text = this.openData.btnCancelStr;
			}
			if (string.IsNullOrEmpty(this.openData.btnConfirmStr))
			{
				this.txtBtnConfirm.text = Singleton<LanguageManager>.Instance.GetInfoByID("Global_Confirm");
				return;
			}
			this.txtBtnConfirm.text = this.openData.btnConfirmStr;
		}

		private void OnConfirmClick()
		{
			GameApp.View.CloseView(ViewName.RememberTipCommonViewModule, null);
			GameApp.Data.GetDataModule(DataName.CommonDataModule).SetRememberTipState(this.openData.rememberTipType, this.toggle.isOn, true);
			Action onConfirmCallback = this.openData.onConfirmCallback;
			if (onConfirmCallback == null)
			{
				return;
			}
			onConfirmCallback();
		}

		private void OnCancelClick()
		{
			GameApp.View.CloseView(ViewName.RememberTipCommonViewModule, null);
			GameApp.Data.GetDataModule(DataName.CommonDataModule).SetRememberTipState(this.openData.rememberTipType, this.toggle.isOn, false);
			Action onCancelCallback = this.openData.onCancelCallback;
			if (onCancelCallback == null)
			{
				return;
			}
			onCancelCallback();
		}

		private void OnToggleValueChanged(bool isOn)
		{
			GameApp.Data.GetDataModule(DataName.CommonDataModule).SetRememberTipState(this.openData.rememberTipType, isOn);
		}

		public static void TryRunRememberTip(RememberTipCommonViewModule.OpenData uiOpenData)
		{
			if (uiOpenData == null)
			{
				return;
			}
			if (!GameApp.Data.GetDataModule(DataName.CommonDataModule).GetRememberTipState(uiOpenData.rememberTipType))
			{
				GameApp.View.OpenView(ViewName.RememberTipCommonViewModule, uiOpenData, 1, null, null);
				return;
			}
			Action onConfirmCallback = uiOpenData.onConfirmCallback;
			if (onConfirmCallback == null)
			{
				return;
			}
			onConfirmCallback();
		}

		public CustomText txtTitle;

		public CustomText txtContent;

		public CustomText txtBtnCancel;

		public CustomText txtBtnConfirm;

		public CustomButton btnConfirm;

		public CustomButton btnCancel;

		public Toggle toggle;

		private RememberTipCommonViewModule.OpenData openData;

		public class OpenData
		{
			public string titleStr;

			public string contentStr;

			public string btnCancelStr;

			public string btnConfirmStr;

			public RememberTipType rememberTipType;

			public Action onCancelCallback;

			public Action onConfirmCallback;
		}
	}
}
