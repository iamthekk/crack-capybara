using System;
using Framework.Logic.Component;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class EquipMergeAnimationSpine : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.SpineAnimation = base.gameObject.GetComponent<SkeletonGraphic>();
		}

		protected override void OnDeInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.isPlay)
			{
				this.triggerOpenUITimer -= deltaTime;
				if (this.triggerOpenUITimer <= 0f)
				{
					this.isPlay = false;
					Action onSpecialTriggerCallback = this.OnSpecialTriggerCallback;
					if (onSpecialTriggerCallback == null)
					{
						return;
					}
					onSpecialTriggerCallback();
				}
			}
		}

		public void Play(string animationName, float speedScale)
		{
			this.animationName = animationName;
			this.speedScale = speedScale;
			this.SpineAnimation.AnimationState.Complete -= new AnimationState.TrackEntryDelegate(this.OnSpinePlayComplete);
			this.SpineAnimation.AnimationState.Complete += new AnimationState.TrackEntryDelegate(this.OnSpinePlayComplete);
			this.isPlay = true;
			this.triggerOpenUITimer = 1f;
			Animation animation = this.SpineAnimation.Skeleton.Data.FindAnimation(animationName);
			if (animation != null)
			{
				this.triggerOpenUITimer = Mathf.Max(animation.Duration - 0.25f, 0f);
				this.SpineAnimation.timeScale = speedScale;
				this.SpineAnimation.AnimationState.SetAnimation(0, animationName, true);
			}
		}

		private void OnSpinePlayComplete(TrackEntry trackEntry)
		{
			if (this.SpineAnimation != null)
			{
				this.SpineAnimation.AnimationState.Complete -= new AnimationState.TrackEntryDelegate(this.OnSpinePlayComplete);
				this.SpineAnimation.timeScale = 0f;
			}
			Action onSpineEndCallback = this.OnSpineEndCallback;
			if (onSpineEndCallback == null)
			{
				return;
			}
			onSpineEndCallback();
		}

		public SkeletonGraphic SpineAnimation;

		public Action OnSpecialTriggerCallback;

		public Action OnSpineEndCallback;

		private string animationName;

		private float speedScale;

		private bool isPlay;

		private float triggerOpenUITimer = 1f;
	}
}
