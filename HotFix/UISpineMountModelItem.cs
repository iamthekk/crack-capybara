using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework.Logic.Component;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class UISpineMountModelItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
			this.ResetEffect();
		}

		private void ResetEffect()
		{
			for (int i = 0; i < this.cacheEffectList.Count; i++)
			{
				GameObject gameObject = this.cacheEffectList[i];
				if (gameObject)
				{
					Object.Destroy(gameObject);
				}
			}
			this.cacheEffectList.Clear();
		}

		public Task ShowModel(int memberId, string defaultAni, bool isLoopAni)
		{
			UISpineMountModelItem.<ShowModel>d__8 <ShowModel>d__;
			<ShowModel>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ShowModel>d__.<>4__this = this;
			<ShowModel>d__.memberId = memberId;
			<ShowModel>d__.defaultAni = defaultAni;
			<ShowModel>d__.isLoopAni = isLoopAni;
			<ShowModel>d__.<>1__state = -1;
			<ShowModel>d__.<>t__builder.Start<UISpineMountModelItem.<ShowModel>d__8>(ref <ShowModel>d__);
			return <ShowModel>d__.<>t__builder.Task;
		}

		public void SetScale(float scale)
		{
			if (this.root != null)
			{
				this.root.localScale = scale * Vector3.one;
			}
		}

		public Animation GetFrontAni(string aniName)
		{
			if (this.modelFront == null || this.modelFront.AnimationState == null || this.modelFront.AnimationState.Data == null || this.modelFront.AnimationState.Data.SkeletonData == null)
			{
				return null;
			}
			return this.modelFront.AnimationState.Data.SkeletonData.FindAnimation(aniName);
		}

		public Animation GetBackAni(string aniName)
		{
			if (this.modelBack == null || this.modelBack.AnimationState == null || this.modelBack.AnimationState.Data == null || this.modelBack.AnimationState.Data.SkeletonData == null)
			{
				return null;
			}
			return this.modelBack.AnimationState.Data.SkeletonData.FindAnimation(aniName);
		}

		public void PlayAnimation(string aniName, bool isLoop)
		{
			if (this.GetFrontAni(aniName) != null)
			{
				this.modelFront.AnimationState.SetAnimation(0, aniName, isLoop);
			}
			if (this.GetBackAni(aniName) != null)
			{
				this.modelBack.AnimationState.SetAnimation(0, aniName, isLoop);
			}
		}

		public void AddAnimation(string aniName, bool isLoop, float delay = 0f)
		{
			if (this.GetFrontAni(aniName) != null)
			{
				this.modelFront.AnimationState.AddAnimation(0, aniName, isLoop, delay);
			}
			if (this.GetBackAni(aniName) != null)
			{
				this.modelBack.AnimationState.AddAnimation(0, aniName, isLoop, delay);
			}
		}

		public void SetAnimationTimeScale(float timeScale)
		{
			this.modelBack.timeScale = timeScale;
			this.modelFront.timeScale = timeScale;
		}

		public float GetAnimationDuration(string aniName)
		{
			Animation animation = this.GetFrontAni(aniName);
			if (animation != null)
			{
				return animation.Duration;
			}
			animation = this.GetBackAni(aniName);
			if (animation != null)
			{
				return animation.Duration;
			}
			return 0f;
		}

		public Transform root;

		public SkeletonGraphic modelBack;

		public SkeletonGraphic modelFront;

		public BoneFollowerGraphic boneFollower;

		private List<GameObject> cacheEffectList = new List<GameObject>();
	}
}
