using System;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class HoverWaitRoundCount : BaseHover
	{
		public override HoverType GetHoverType()
		{
			return HoverType.WaitRoundCount;
		}

		protected override void OnInit()
		{
			this.canvasGroupNode.alpha = 0f;
			this.RefreshTargetPos(base.target.transform.position + new Vector3(0f, 0f, 0f));
			this.listen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameView_Refresh_WaitRoundCount, new HandlerEvent(this.OnEventUpdateWaitRoundCount));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameHover_ShowHpHUD, new HandlerEvent(this.OnEventShowHpHUD));
		}

		protected override void OnDeInit()
		{
			if (this.listen != null)
			{
				this.listen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameView_Refresh_WaitRoundCount, new HandlerEvent(this.OnEventUpdateWaitRoundCount));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameHover_ShowHpHUD, new HandlerEvent(this.OnEventShowHpHUD));
		}

		protected override void OnUpdateImpl(float deltaTime, float unscaleDeltaTime)
		{
			if (base.target)
			{
				this.RefreshTargetPos(base.target.transform.position + new Vector3(0f, 0f, 0f));
			}
		}

		private void OnAnimatorListen(GameObject obj, string arg)
		{
		}

		private void OnEventUpdateWaitRoundCount(object sender, int type, BaseEventArgs args)
		{
			EventArgsAddHover eventArgsAddHover = args as EventArgsAddHover;
			if (eventArgsAddHover == null)
			{
				return;
			}
			if (base.ownerId != eventArgsAddHover.targetData.id)
			{
				return;
			}
			long num = (long)eventArgsAddHover.hoverData;
			this.text.text = string.Format("{0}", num);
			if (num > 0L)
			{
				ShortcutExtensions46.DOFade(this.canvasGroupNode, 1f, 0.3f);
				return;
			}
			ShortcutExtensions46.DOFade(this.canvasGroupNode, 0f, 0.3f);
		}

		private void OnEventShowHpHUD(object sender, int type, BaseEventArgs args)
		{
		}

		public CanvasGroup canvasGroupNode;

		public CustomImage img;

		public CustomText text;

		public Animator animator;

		public AnimatorListen listen;

		private const float OffsetY = 0f;
	}
}
