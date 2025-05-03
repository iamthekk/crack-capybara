using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class PetSpine : MonoBehaviour
	{
		public SkeletonGraphic SkeletonGraphic
		{
			get
			{
				if (!this._skeletonGraphic)
				{
					base.TryGetComponent<SkeletonGraphic>(ref this._skeletonGraphic);
				}
				return this._skeletonGraphic;
			}
		}

		public SkeletonAnimation SkeletonAnimation
		{
			get
			{
				if (!this._skeletonAnimation)
				{
					base.TryGetComponent<SkeletonAnimation>(ref this._skeletonAnimation);
				}
				return this._skeletonAnimation;
			}
		}

		public Skeleton Skeleton
		{
			get
			{
				if (this.SkeletonGraphic)
				{
					return this.SkeletonGraphic.Skeleton;
				}
				if (this.SkeletonAnimation)
				{
					return this.SkeletonAnimation.Skeleton;
				}
				return null;
			}
		}

		public AnimationState AnimationState
		{
			get
			{
				if (this.SkeletonGraphic)
				{
					return this.SkeletonGraphic.AnimationState;
				}
				if (this.SkeletonAnimation)
				{
					return this.SkeletonAnimation.AnimationState;
				}
				return null;
			}
		}

		private SkeletonGraphic _skeletonGraphic;

		private SkeletonAnimation _skeletonAnimation;
	}
}
