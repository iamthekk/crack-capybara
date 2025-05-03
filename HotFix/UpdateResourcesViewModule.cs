using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine.Events;

namespace HotFix
{
	public class UpdateResourcesViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.m_uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.m_okBt.onClick.AddListener(new UnityAction(this.OnClickOkBt));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_uiPopCommon.OnClick = null;
			this.m_okBt.onClick.RemoveAllListeners();
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

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickOkBt();
			}
		}

		private void OnClickOkBt()
		{
			GameApp.View.CloseView(ViewName.UpdateResourcesViewModule, null);
			GameApp.Quit();
		}

		public UIPopCommon m_uiPopCommon;

		public CustomButton m_okBt;
	}
}
