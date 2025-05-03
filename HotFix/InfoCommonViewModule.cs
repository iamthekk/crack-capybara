using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;

namespace HotFix
{
	public class InfoCommonViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			InfoCommonViewModule.OpenData openData = data as InfoCommonViewModule.OpenData;
			string text = string.Empty;
			string text2 = string.Empty;
			if (openData != null)
			{
				text = openData.m_tileInfo;
				text2 = openData.m_contextInfo;
			}
			this.SetTileAddContext(text, text2);
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.uiPopCommon.OnClick = null;
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_InfoCommonView_Close, null);
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

		private void SetTileAddContext(string title, string context)
		{
			if (this.m_tileTxt != null)
			{
				this.m_tileTxt.text = title;
			}
			if (this.m_contextTxt != null)
			{
				this.m_contextTxt.text = context;
			}
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickCloseBt();
			}
		}

		private void OnClickCloseBt()
		{
			GameApp.View.CloseView(ViewName.InfoCommonViewModule, null);
		}

		public UIPopCommon uiPopCommon;

		public CustomText m_tileTxt;

		public CustomText m_contextTxt;

		public class OpenData
		{
			public string m_tileInfo;

			public string m_contextInfo;
		}
	}
}
