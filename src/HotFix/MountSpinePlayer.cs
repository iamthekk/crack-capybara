using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class MountSpinePlayer : RoleSpinePlayerBase
	{
		public override void Init(ComponentRegister componentRegister)
		{
			this.isInit = true;
			if (this.backAnimation)
			{
				this.backAnimator = new SpineAnimator(this.backAnimation);
			}
			if (this.frontAnimation)
			{
				this.frontAnimator = new SpineAnimator(this.frontAnimation);
			}
			if (this.backAnimator != null)
			{
				this.backAnimator.AnimationState.Event += new AnimationState.TrackEntryEventDelegate(this.OnEvent);
			}
			else if (this.frontAnimator != null)
			{
				this.frontAnimator.AnimationState.Event += new AnimationState.TrackEntryEventDelegate(this.OnEvent);
			}
			this.Init_ColorRender();
		}

		public override void DeInit()
		{
			if (!this.isInit)
			{
				return;
			}
			this.isInit = false;
			this.DeInit_ColorRender();
			if (this.backAnimator != null)
			{
				this.backAnimator.AnimationState.Event -= new AnimationState.TrackEntryEventDelegate(this.OnEvent);
				this.backAnimator.ClearTracks();
				return;
			}
			if (this.frontAnimator != null)
			{
				this.frontAnimator.AnimationState.Event -= new AnimationState.TrackEntryEventDelegate(this.OnEvent);
				this.frontAnimator.ClearTracks();
			}
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			ColorRender colorRender = this.colorRender;
			if (colorRender == null)
			{
				return;
			}
			colorRender.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		private void OnDestroy()
		{
			this.DeInit();
		}

		public override void SetSpeed(float speed)
		{
			SpineAnimator spineAnimator = this.backAnimator;
			if (spineAnimator != null)
			{
				spineAnimator.SetSpeed(speed);
			}
			SpineAnimator spineAnimator2 = this.frontAnimator;
			if (spineAnimator2 == null)
			{
				return;
			}
			spineAnimator2.SetSpeed(speed);
		}

		public override void PlayAni(string animationName, bool isLoop)
		{
			this.PlayAni(animationName, isLoop, null);
		}

		public override void PlayAni(string animationName, bool isLoop, AnimationState.TrackEntryEventDelegate complete)
		{
			if (this.backAnimator != null && this.backAnimator.IsHaveAni(animationName))
			{
				this.backAnimator.PlayAni(animationName, isLoop, complete);
			}
			if (this.frontAnimator != null && this.frontAnimator.IsHaveAni(animationName))
			{
				this.frontAnimator.PlayAni(animationName, isLoop);
			}
		}

		public override TrackEntry PlayAni(string animationName, bool isLoop, AnimationState.TrackEntryEventDelegate spineEvent, AnimationState.TrackEntryDelegate complete)
		{
			TrackEntry trackEntry = null;
			TrackEntry trackEntry2 = null;
			if (this.backAnimator != null)
			{
				trackEntry = this.backAnimator.PlayAni(animationName, isLoop, spineEvent, complete);
			}
			if (this.frontAnimator != null)
			{
				trackEntry2 = this.frontAnimator.PlayAni(animationName, isLoop, spineEvent, complete);
			}
			if (trackEntry != null)
			{
				return trackEntry;
			}
			if (trackEntry2 != null)
			{
				return trackEntry2;
			}
			return null;
		}

		public override float GetAnimationDuration(string animationName)
		{
			if (this.backAnimator != null)
			{
				return this.backAnimator.GetAnimationDuration(animationName);
			}
			if (this.frontAnimator != null)
			{
				return this.frontAnimator.GetAnimationDuration(animationName);
			}
			return 0f;
		}

		public override TrackEntry AddAni(string animationName, bool isLoop)
		{
			TrackEntry trackEntry = null;
			if (this.backAnimator != null)
			{
				trackEntry = this.backAnimator.AddAni(animationName, isLoop, 0f);
			}
			if (this.frontAnimator != null)
			{
				TrackEntry trackEntry2 = this.frontAnimator.AddAni(animationName, isLoop, 0f);
				if (trackEntry == null)
				{
					trackEntry = trackEntry2;
				}
			}
			return trackEntry;
		}

		public override bool IsHaveAni(string animationName)
		{
			if (this.backAnimator != null)
			{
				return this.backAnimator.IsHaveAni(animationName);
			}
			return this.frontAnimator != null && this.frontAnimator.IsHaveAni(animationName);
		}

		public override void PlayAnimation(string animationName)
		{
			this.PlayAnimation(animationName, null, null);
		}

		public override void PlayAnimation(string animationName, AnimationState.TrackEntryEventDelegate spineEvent)
		{
			this.PlayAnimation(animationName, spineEvent, null);
		}

		public override void PlayAnimation(string animationName, AnimationState.TrackEntryDelegate complete)
		{
			this.PlayAnimation(animationName, null, complete);
		}

		public override void PlayAnimation(string animationName, AnimationState.TrackEntryEventDelegate spineEvent, AnimationState.TrackEntryDelegate complete)
		{
			if (string.IsNullOrEmpty(animationName))
			{
				return;
			}
			bool flag = MemberAnimationName.IsLoop(animationName);
			if (this.backAnimator != null && this.backAnimator.IsHaveAni(animationName))
			{
				this.backAnimator.PlayAni(animationName, flag, spineEvent, complete);
			}
			if (this.frontAnimator != null && this.frontAnimator.IsHaveAni(animationName))
			{
				this.frontAnimator.PlayAni(animationName, flag, spineEvent, complete);
			}
		}

		public override void AddAnimation(string animationName)
		{
			bool flag = MemberAnimationName.IsLoop(animationName);
			if (this.backAnimator != null && this.backAnimator.IsHaveAni(animationName))
			{
				SpineAnimator spineAnimator = this.backAnimator;
				if (spineAnimator != null)
				{
					spineAnimator.AddAni(animationName, flag, 0f);
				}
			}
			if (this.frontAnimator != null && this.frontAnimator.IsHaveAni(animationName))
			{
				SpineAnimator spineAnimator2 = this.frontAnimator;
				if (spineAnimator2 == null)
				{
					return;
				}
				spineAnimator2.AddAni(animationName, flag, 0f);
			}
		}

		public override void SetSortingLayer(string layer)
		{
			SpineAnimator spineAnimator = this.backAnimator;
			if (spineAnimator != null)
			{
				spineAnimator.SetSortingLayer(layer);
			}
			SpineAnimator spineAnimator2 = this.frontAnimator;
			if (spineAnimator2 == null)
			{
				return;
			}
			spineAnimator2.SetSortingLayer(layer);
		}

		public override void SetOrderLayer(int layer)
		{
		}

		public void SetBackOrderLayer(int layer)
		{
			SpineAnimator spineAnimator = this.backAnimator;
			if (spineAnimator == null)
			{
				return;
			}
			spineAnimator.SetOrderLayer(layer);
		}

		public void SetFrontOrderLayer(int layer)
		{
			SpineAnimator spineAnimator = this.frontAnimator;
			if (spineAnimator == null)
			{
				return;
			}
			spineAnimator.SetOrderLayer(layer);
		}

		public override int GetMinOrderLayer()
		{
			int num = 9999;
			if (this.backAnimator != null && this.backAnimator.GetOrderLayer() < num)
			{
				num = this.backAnimator.GetOrderLayer();
			}
			if (this.frontAnimator != null && this.frontAnimator.GetOrderLayer() < num)
			{
				num = this.frontAnimator.GetOrderLayer();
			}
			return num;
		}

		public override int GetMaxOrderLayer()
		{
			int num = 0;
			if (this.backAnimator != null && this.backAnimator.GetOrderLayer() > num)
			{
				num = this.backAnimator.GetOrderLayer();
			}
			if (this.frontAnimator != null && this.frontAnimator.GetOrderLayer() > num)
			{
				num = this.frontAnimator.GetOrderLayer();
			}
			return num;
		}

		public override void SetAnimatorSpeed(float speed)
		{
			SpineAnimator spineAnimator = this.backAnimator;
			if (spineAnimator != null)
			{
				spineAnimator.SetSpeed(speed);
			}
			SpineAnimator spineAnimator2 = this.frontAnimator;
			if (spineAnimator2 == null)
			{
				return;
			}
			spineAnimator2.SetSpeed(speed);
		}

		public override void SetSkin(string skinName)
		{
			SpineAnimator spineAnimator = this.backAnimator;
			if (spineAnimator != null)
			{
				spineAnimator.SetSkin(skinName);
			}
			SpineAnimator spineAnimator2 = this.frontAnimator;
			if (spineAnimator2 == null)
			{
				return;
			}
			spineAnimator2.SetSkin(skinName);
		}

		public override void Init_ColorRender()
		{
			this.m_colorRender = this.colorRender;
			Renderer[] array = null;
			Renderer[] array2 = null;
			if (this.backAnimation != null)
			{
				array = this.backAnimation.GetComponentsInChildren<Renderer>();
			}
			if (this.frontAnimation != null)
			{
				array2 = this.frontAnimation.GetComponentsInChildren<Renderer>();
			}
			List<Renderer> list = new List<Renderer>();
			if (array != null && array.Length != 0)
			{
				list.AddRange(array);
			}
			if (array2 != null && array2.Length != 0)
			{
				list.AddRange(array2);
			}
			if (this.m_colorRender != null)
			{
				this.m_colorRender.OnInit(list);
				this.m_colorRender.SetFillPhase(0f);
			}
		}

		public override void DeInit_ColorRender()
		{
			if (this.m_colorRender != null)
			{
				this.m_colorRender.SetFillPhase(0f);
				this.m_colorRender.SetFillVColor(0f);
				this.m_colorRender.OnDeInit();
			}
		}

		public override void SetWeapon(int equipID)
		{
		}

		public override void SetRoleModelShow(bool isShow)
		{
			if (this.backAnimation != null)
			{
				this.backAnimation.gameObject.SetActive(isShow);
			}
			if (this.frontAnimation != null)
			{
				this.frontAnimation.gameObject.SetActive(isShow);
			}
		}

		public SkeletonAnimation backAnimation;

		public SkeletonAnimation frontAnimation;

		public ColorRender colorRender;

		private SpineAnimator backAnimator;

		private SpineAnimator frontAnimator;

		private bool isInit;
	}
}
