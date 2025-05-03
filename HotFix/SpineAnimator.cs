using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class SpineAnimator
	{
		public SkeletonAnimation curSkeletonAnimation
		{
			get
			{
				return this.skeletonAnimation;
			}
		}

		public AnimationState AnimationState
		{
			get
			{
				return this.animationState;
			}
		}

		public float Speed
		{
			get
			{
				if (this.animationState == null)
				{
					return 0f;
				}
				return this.animationState.TimeScale;
			}
		}

		public SpineAnimator(SkeletonAnimation anim)
		{
			this.skeletonAnimation = anim;
			this.skeletonAnimation.ClearState();
			this.skeleton = this.skeletonAnimation.Skeleton;
			if (this.skeleton == null)
			{
				HLog.LogError(string.Format("Spine skeleton data is missing, prefab={0}", anim.gameObject));
			}
			this.animationState = this.skeletonAnimation.AnimationState;
			this.meshRender = anim.gameObject.GetComponent<MeshRenderer>();
			this.isUI = false;
		}

		public SpineAnimator(SkeletonGraphic grap)
		{
			this.skeletonGraphic = grap;
			this.skeleton = this.skeletonGraphic.Skeleton;
			this.animationState = this.skeletonGraphic.AnimationState;
			this.isUI = true;
		}

		public SpineAnimator()
		{
		}

		public void Init(SkeletonAnimation anim, SkeletonDataAsset dataAsset, string skinName)
		{
			anim.skeletonDataAsset = dataAsset;
			anim.initialSkinName = skinName;
			anim.Initialize(true, false);
			this.skeletonAnimation = anim;
			this.skeleton = anim.Skeleton;
			if (this.skeleton == null)
			{
				HLog.LogError(string.Format("Spine skeleton data is missing, prefab={0}", anim.gameObject));
			}
			this.animationState = anim.AnimationState;
			this.meshRender = anim.gameObject.GetComponent<MeshRenderer>();
		}

		public void DeInit()
		{
		}

		public void ChangeAnimation(SkeletonAnimation anim)
		{
			this.skeletonAnimation = anim;
			this.skeletonAnimation.ClearState();
			this.skeleton = this.skeletonAnimation.Skeleton;
			this.animationState = this.skeletonAnimation.AnimationState;
			this.meshRender = anim.gameObject.GetComponent<MeshRenderer>();
			this.isUI = false;
		}

		public void SetToSetupPose()
		{
			if (this.skeleton == null)
			{
				return;
			}
			this.skeleton.SetToSetupPose();
			this.skeleton.SetBonesToSetupPose();
			this.skeleton.SetSlotsToSetupPose();
		}

		public void SetSkin(string skinName)
		{
			if (this.skeleton == null)
			{
				return;
			}
			this.skeleton.SetSkin(skinName);
		}

		public void LookRight()
		{
			if (this.skeleton == null)
			{
				return;
			}
			this.skeleton.ScaleX = 1f;
		}

		public void LookLeft()
		{
			if (this.skeleton == null)
			{
				return;
			}
			this.skeleton.ScaleX = -1f;
		}

		public void SetOrderLayer(int layer)
		{
			if (this.meshRender == null)
			{
				return;
			}
			this.meshRender.sortingOrder = layer;
		}

		public int GetOrderLayer()
		{
			if (this.meshRender == null)
			{
				return 0;
			}
			return this.meshRender.sortingOrder;
		}

		public void SetSortingLayer(string layer)
		{
			if (this.meshRender == null)
			{
				return;
			}
			if (layer.Equals(string.Empty))
			{
				return;
			}
			this.meshRender.sortingLayerName = layer;
		}

		public void SetSpeed(float speed)
		{
			if (this.animationState == null)
			{
				return;
			}
			if (this.animationState.TimeScale != speed)
			{
				this.animationState.TimeScale = speed;
			}
		}

		public float GetAnimationDuration(string name)
		{
			if (this.skeleton == null)
			{
				return 0f;
			}
			Animation animation = this.skeleton.Data.FindAnimation(name);
			if (animation != null)
			{
				return animation.Duration;
			}
			return 0f;
		}

		public void ClearTracks()
		{
			if (this.animationState != null)
			{
				TrackEntry current = this.animationState.GetCurrent(0);
				if (current != null)
				{
					current.Reset();
				}
			}
			if (this.skeletonAnimation != null)
			{
				this.skeletonAnimation.ClearState();
			}
		}

		public string GetCurrentAnimationName(int trackIndex = 0)
		{
			if (this.animationState == null)
			{
				return null;
			}
			TrackEntry current = this.animationState.GetCurrent(trackIndex);
			if (current == null)
			{
				return null;
			}
			return current.Animation.Name;
		}

		public bool IsHaveAni(string aniName)
		{
			return this.skeleton != null && this.skeleton.Data.FindAnimation(aniName) != null;
		}

		public void PlayAniByIsHave(string aniName, bool isLoop)
		{
			if (this.IsHaveAni(aniName))
			{
				this.PlayAni(aniName, isLoop);
			}
		}

		public TrackEntry PlayAni(string aniName, bool isLoop)
		{
			return this.PlayAni(0, aniName, isLoop, null, null);
		}

		public TrackEntry PlayAni(string aniName, bool isLoop, AnimationState.TrackEntryDelegate complete)
		{
			return this.PlayAni(0, aniName, isLoop, null, complete);
		}

		public TrackEntry PlayAni(string aniName, bool isLoop, AnimationState.TrackEntryEventDelegate spineEvent)
		{
			return this.PlayAni(0, aniName, isLoop, spineEvent, null);
		}

		public TrackEntry PlayAni(string aniName, bool isLoop, AnimationState.TrackEntryEventDelegate spineEvent, AnimationState.TrackEntryDelegate complete)
		{
			return this.PlayAni(0, aniName, isLoop, spineEvent, complete);
		}

		public TrackEntry PlayAni(int trackIndex, string aniName, bool isLoop, AnimationState.TrackEntryEventDelegate spineEvent, AnimationState.TrackEntryDelegate complete)
		{
			if (this.animationState == null)
			{
				return null;
			}
			TrackEntry trackEntry = null;
			if (this.IsHaveAni(aniName))
			{
				trackEntry = this.animationState.SetAnimation(trackIndex, aniName, isLoop);
				if (spineEvent != null)
				{
					trackEntry.Event += spineEvent;
				}
				if (complete != null)
				{
					trackEntry.Complete += complete;
				}
			}
			return trackEntry;
		}

		public TrackEntry AddAni(string name, bool isLoop, float delay = 0f)
		{
			return this.AddAni(0, name, isLoop, delay);
		}

		public TrackEntry AddAni(int trackIndex, string name, bool isLoop, float delay = 0f)
		{
			if (this.animationState == null)
			{
				return null;
			}
			if (this.IsHaveAni(name))
			{
				return this.animationState.AddAnimation(trackIndex, name, isLoop, delay);
			}
			return null;
		}

		public void StopAni(int trackIndex)
		{
			this.StopAni(trackIndex, this.animationState.Data.DefaultMix);
		}

		public void StopAni(int trackIndex, float midDuration)
		{
			if (this.animationState == null)
			{
				return;
			}
			this.animationState.SetEmptyAnimation(trackIndex, midDuration);
		}

		public float GetAnimTimeByName(string animName)
		{
			SkeletonData skeletonData;
			if (this.skeletonAnimation != null)
			{
				skeletonData = this.skeletonAnimation.skeletonDataAsset.GetSkeletonData(true);
			}
			else
			{
				if (!(this.skeletonGraphic != null))
				{
					return 0f;
				}
				skeletonData = this.skeletonGraphic.skeletonDataAsset.GetSkeletonData(true);
			}
			return skeletonData.FindAnimation(animName).Duration;
		}

		private SkeletonAnimation skeletonAnimation;

		private SkeletonGraphic skeletonGraphic;

		private Skeleton skeleton;

		private AnimationState animationState;

		private MeshRenderer meshRender;

		public bool isUI;
	}
}
