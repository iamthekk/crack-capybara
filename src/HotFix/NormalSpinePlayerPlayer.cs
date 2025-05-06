using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class NormalSpinePlayerPlayer : RoleSpinePlayerBase
	{
		public override void Init(ComponentRegister componentRegister)
		{
			this.isInit = true;
			this.componentRegister = componentRegister;
			this.bodyAnimation = componentRegister.GetGameObject("Model").GetComponent<SkeletonAnimation>();
			this.bodyAnimator = new SpineAnimator(this.bodyAnimation);
			this.bodyAnimator.AnimationState.Event += new AnimationState.TrackEntryEventDelegate(this.OnEvent);
			this.PlayAni("Idle", true);
		}

		public override void DeInit()
		{
			if (!this.isInit)
			{
				return;
			}
			this.isInit = false;
			this.DeInit_ColorRender();
			if (this.bodyAnimator != null)
			{
				this.bodyAnimator.AnimationState.Event -= new AnimationState.TrackEntryEventDelegate(this.OnEvent);
				this.bodyAnimator.ClearTracks();
			}
		}

		private void OnDestroy()
		{
			this.DeInit();
		}

		public override void SetSpeed(float speed)
		{
			SpineAnimator spineAnimator = this.bodyAnimator;
			if (spineAnimator == null)
			{
				return;
			}
			spineAnimator.SetSpeed(speed);
		}

		public override void PlayAni(string animationName, bool isLoop)
		{
			if (this.bodyAnimator.IsHaveAni(animationName))
			{
				this.bodyAnimator.PlayAni(animationName, isLoop);
			}
		}

		public override void PlayAni(string animationName, bool isLoop, AnimationState.TrackEntryEventDelegate complete)
		{
			if (this.bodyAnimator.IsHaveAni(animationName))
			{
				this.bodyAnimator.PlayAni(animationName, isLoop, complete);
			}
		}

		public override TrackEntry PlayAni(string animationName, bool isLoop, AnimationState.TrackEntryEventDelegate spineEvent, AnimationState.TrackEntryDelegate complete)
		{
			return this.bodyAnimator.PlayAni(animationName, isLoop, spineEvent, complete);
		}

		public override float GetAnimationDuration(string animationName)
		{
			return this.bodyAnimator.GetAnimationDuration(animationName);
		}

		public override TrackEntry AddAni(string animationName, bool isLoop)
		{
			return this.bodyAnimator.AddAni(animationName, isLoop, 0f);
		}

		public override bool IsHaveAni(string animationName)
		{
			return this.bodyAnimator.IsHaveAni(animationName);
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
			ArtAnimation_animation elementById = GameApp.Table.GetManager().GetArtAnimation_animationModelInstance().GetElementById(animationName);
			if (elementById != null)
			{
				bool flag = elementById.hideWeapon > 0;
			}
			bool flag2 = MemberAnimationName.IsLoop(animationName);
			if (this.bodyAnimator.IsHaveAni(animationName))
			{
				this.bodyAnimator.PlayAni(animationName, flag2, spineEvent, complete);
			}
		}

		public override void AddAnimation(string animationName)
		{
			bool flag = MemberAnimationName.IsLoop(animationName);
			if (this.bodyAnimator.IsHaveAni(animationName))
			{
				SpineAnimator spineAnimator = this.bodyAnimator;
				if (spineAnimator == null)
				{
					return;
				}
				spineAnimator.AddAni(animationName, flag, 0f);
			}
		}

		public override void SetSortingLayer(string layer)
		{
			SpineAnimator spineAnimator = this.bodyAnimator;
			if (spineAnimator == null)
			{
				return;
			}
			spineAnimator.SetSortingLayer(layer);
		}

		public override void SetOrderLayer(int layer)
		{
			SpineAnimator spineAnimator = this.bodyAnimator;
			if (spineAnimator == null)
			{
				return;
			}
			spineAnimator.SetOrderLayer(layer);
		}

		public override int GetMinOrderLayer()
		{
			if (this.bodyAnimator != null)
			{
				return this.bodyAnimator.GetOrderLayer();
			}
			return base.GetMinOrderLayer();
		}

		public override int GetMaxOrderLayer()
		{
			if (this.bodyAnimator != null)
			{
				return this.bodyAnimator.GetOrderLayer();
			}
			return base.GetMaxOrderLayer();
		}

		public override void SetAnimatorSpeed(float speed)
		{
			SpineAnimator spineAnimator = this.bodyAnimator;
			if (spineAnimator == null)
			{
				return;
			}
			spineAnimator.SetSpeed(speed);
		}

		public override void SetSkin(string skinName)
		{
			SpineAnimator spineAnimator = this.bodyAnimator;
			if (spineAnimator == null)
			{
				return;
			}
			spineAnimator.SetSkin(skinName);
		}

		public override void Init_ColorRender()
		{
			this.m_colorRender = this.componentRegister.GetComponent<ColorRender>();
			Renderer[] componentsInChildren = this.bodyAnimation.GetComponentsInChildren<Renderer>();
			List<Renderer> list = new List<Renderer>();
			list.AddRange(componentsInChildren);
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

		protected SpineAnimator bodyAnimator;

		private ComponentRegister componentRegister;

		private bool isInit;
	}
}
