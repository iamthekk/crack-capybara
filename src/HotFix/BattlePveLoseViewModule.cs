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
	public class BattlePveLoseViewModule : BaseViewModule
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
		}

		public override void OnDelete()
		{
			this.BtnClose = null;
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
			GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayShow(delegate
				{
					EventArgsGameEnd instance = Singleton<EventArgsGameEnd>.Instance;
					instance.SetData(GameOverType.Win);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_GameOver, instance);
					GameApp.View.CloseView(ViewName.BattleTestLoseViewModule, null);
					GameApp.State.ActiveState(StateName.MainState);
				});
			});
		}

		[SerializeField]
		private Button BtnClose;
	}
}
