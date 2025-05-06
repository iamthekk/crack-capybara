using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine.Events;

namespace HotFix
{
	public class OfficialAccountModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonCopy.onClick.AddListener(new UnityAction(this.OnClickCopy));
		}

		public override void OnDelete()
		{
			this.buttonMask.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonCopy.onClick.RemoveListener(new UnityAction(this.OnClickClose));
		}

		public override void OnOpen(object data)
		{
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiOfficialAccount_title");
			this.textConstract.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiOfficialAccount_constract");
			this.textButtonCopy.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiOfficialAccount_copy");
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.OfficialAccountModule, null);
		}

		private void OnClickCopy()
		{
			GameApp.SDK.SetClipboardData("冒险者日记");
		}

		public CustomText textTitle;

		public CustomButton buttonMask;

		public CustomButton buttonClose;

		public CustomButton buttonCopy;

		public CustomText textConstract;

		public CustomText textButtonCopy;
	}
}
