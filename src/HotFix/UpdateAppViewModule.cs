using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UpdateAppViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.m_okBt.onClick.AddListener(new UnityAction(this.OnClickOkBt));
			this.m_gotoBt.onClick.AddListener(new UnityAction(this.OnClickGotoBt));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.uiPopCommon.OnClick = null;
			this.m_okBt.onClick.RemoveAllListeners();
			this.m_gotoBt.onClick.RemoveAllListeners();
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
			GameApp.View.CloseView(ViewName.UpdateAppViewModule, null);
			GameApp.Quit();
		}

		private void OnClickGotoBt()
		{
			string @string = GameApp.Config.GetString("ChannelName");
			Application.OpenURL(Singleton<PathManager>.Instance.GetAppUrl(@string));
		}

		public UIPopCommon uiPopCommon;

		public CustomButton m_okBt;

		public CustomButton m_gotoBt;
	}
}
