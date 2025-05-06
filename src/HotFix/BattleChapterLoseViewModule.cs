using System;
using Framework;
using Framework.EventSystem;
using Framework.ViewModule;
using HotFix.Client;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class BattleChapterLoseViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.BtnClose.onClick.AddListener(new UnityAction(this.OnBtnCloseHandler));
		}

		public override void OnClose()
		{
			this.BtnClose.onClick.RemoveListener(new UnityAction(this.OnBtnCloseHandler));
			this.BtnClose = null;
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

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		private void OnBtnCloseHandler()
		{
			EventArgsGameEnd instance = Singleton<EventArgsGameEnd>.Instance;
			instance.SetData(GameOverType.Win);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_GameOver, instance);
			GameApp.View.CloseView(ViewName.BattleChapterLoseViewModule, null);
		}

		[SerializeField]
		private Button BtnClose;
	}
}
