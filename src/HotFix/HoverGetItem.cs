using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class HoverGetItem : BaseHover
	{
		public override HoverType GetHoverType()
		{
			return HoverType.GetItem;
		}

		protected override void OnInit()
		{
			this.RefreshTargetPos(base.target.transform.position + new Vector3(0f, 0.5f, 0f));
			if (this.listen != null)
			{
				this.listen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
			if (this.animator != null)
			{
				this.animator.SetTrigger("Run");
			}
			HoverEventItemData hoverEventItemData = this.hoverData as HoverEventItemData;
			if (hoverEventItemData == null)
			{
				return;
			}
			this.endPos = hoverEventItemData.endPos;
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(hoverEventItemData.item.languageId);
			this.text.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(153, new object[] { infoByID });
			this.item.Init();
			this.item.Refresh(hoverEventItemData.item);
		}

		protected override void OnDeInit()
		{
			if (this.listen != null)
			{
				this.listen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
		}

		protected override void OnUpdateImpl(float deltaTime, float unscaleDeltaTime)
		{
		}

		private void OnAnimatorListen(GameObject obj, string arg)
		{
			if (arg == "End")
			{
				this.FlyToPos();
			}
		}

		private void FlyToPos()
		{
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMove(this.item.transform, this.endPos, 0.7f, false), 11));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(this.item.transform, Vector3.one, 0.7f));
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOFade(this.text, 0f, 0.1f));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.item.transform.localPosition = Vector3.zero;
				base.RemoveHover();
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshEventTaskItems, null);
			});
		}

		public CustomText text;

		public UIGameEventItemItem item;

		public Animator animator;

		public AnimatorListen listen;

		private const float OffsetY = 0.5f;

		public const float FlyMoveTime = 0.7f;

		private SequencePool sequencePool = new SequencePool();

		private Vector3 endPos;
	}
}
