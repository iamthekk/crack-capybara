using System;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class EventPointActionBox : EventPointActionNormal
	{
		protected override void InitPoint()
		{
			base.InitPoint();
			if (this.animator == null)
			{
				this.animator = new SpineAnimator(this.spineAni);
			}
		}

		protected override void DeInitPoint()
		{
			base.DeInitPoint();
			this.sequencePool.Clear(false);
		}

		protected override void DoAction(int actionId)
		{
			if (this.animator == null)
			{
				return;
			}
			this.sequencePool.Clear(false);
			switch (actionId)
			{
			case 1:
			{
				this.animator.SetOrderLayer(1);
				this.animator.PlayAni("Idle", true);
				Vector3 localPosition = this.spineObj.transform.localPosition;
				this.spineObj.transform.localPosition = new Vector2(localPosition.x, localPosition.y + 10f);
				TweenSettingsExtensions.Append(this.sequencePool.Get(), TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.spineObj.transform, localPosition.y, 0.2f, false), 12));
				return;
			}
			case 2:
				this.animator.PlayAni("Open", false);
				this.animator.AddAni("Open_Idle", true, 0f);
				return;
			case 3:
				this.renderCtrl = this.spineAni.GetComponent<ColorRenderCtrl>();
				if (this.renderCtrl)
				{
					this.renderCtrl.HideAni(delegate
					{
						this.spineObj.SetActiveSafe(false);
					});
				}
				return;
			default:
				return;
			}
		}

		public GameObject spineParent;

		public GameObject spineObj;

		public SkeletonAnimation spineAni;

		private SpineAnimator animator;

		private readonly SequencePool sequencePool = new SequencePool();

		private ColorRenderCtrl renderCtrl;
	}
}
