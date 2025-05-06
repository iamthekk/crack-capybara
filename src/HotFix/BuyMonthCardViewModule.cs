using System;
using System.Collections;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class BuyMonthCardViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonDark.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.nodeRT = this.nodeCtrl.GetComponent<RectTransform>();
			this.buttonRT = this.buttonDark.GetComponent<RectTransform>();
			this.nodeCtrl.Init();
		}

		public override void OnOpen(object data)
		{
			this.nodeCtrl.Refresh();
			base.StartCoroutine(this.WaitForLayoutUpdate());
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.nodeCtrl.DeInit();
			this.buttonDark.onClick.RemoveListener(new UnityAction(this.OnClickClose));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.BuyMonthCardViewModule, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterSweep_Refresh, null);
		}

		private IEnumerator WaitForLayoutUpdate()
		{
			yield return new WaitForEndOfFrame();
			float num = -200f;
			float y = this.nodeRT.sizeDelta.y;
			this.buttonRT.anchoredPosition = new Vector2(this.buttonRT.anchoredPosition.x, -y / 2f + num);
			yield break;
		}

		public UIPrivilegeCardNodeCtrl nodeCtrl;

		public CustomButton buttonDark;

		private RectTransform nodeRT;

		private RectTransform buttonRT;
	}
}
