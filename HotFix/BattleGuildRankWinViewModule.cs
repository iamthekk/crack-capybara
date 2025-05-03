using System;
using Framework;
using Framework.EventSystem;
using Framework.ViewModule;
using HotFix.Client;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class BattleGuildRankWinViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.BtnClose.onClick.AddListener(new UnityAction(this.OnBtnCloseHandler));
			this.spineAnimator = new SpineAnimator(this.skeletonAnimation);
			this.spineAnimator.PlayAni("Win", false, delegate(TrackEntry trackEntry)
			{
				this.spineAnimator.PlayAni("Win_Loop", true);
			});
			GameApp.Sound.PlayClip(52, 1f);
		}

		public override void OnClose()
		{
			Button btnClose = this.BtnClose;
			if (btnClose != null)
			{
				Button.ButtonClickedEvent onClick = btnClose.onClick;
				if (onClick != null)
				{
					onClick.RemoveListener(new UnityAction(this.OnBtnCloseHandler));
				}
			}
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
			GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayShow(delegate
				{
					EventArgsGameEnd instance = Singleton<EventArgsGameEnd>.Instance;
					instance.SetData(GameOverType.Win);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_GameOver, instance);
					GameApp.View.CloseView(ViewName.BattleGuildRankWinViewModule, null);
					GameApp.State.ActiveState(StateName.MainState);
				});
			});
		}

		[SerializeField]
		private Button BtnClose;

		[SerializeField]
		private SkeletonGraphic skeletonAnimation;

		private SpineAnimator spineAnimator;
	}
}
