using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class RideCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.spineAnimator = new SpineAnimator(this.skeleton);
			this.renderers = this.skeleton.GetComponentsInChildren<Renderer>();
			if (this.colorRender != null)
			{
				this.colorRender.OnInit(new List<Renderer>(this.renderers));
				this.colorRender.SetFillPhase(0f);
			}
		}

		protected override void OnDeInit()
		{
			if (this.colorRender != null)
			{
				this.colorRender.SetFillPhase(0f);
				this.colorRender.SetFillVColor(0f);
				this.colorRender.OnDeInit();
			}
			this.renderers = null;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.colorRender != null)
			{
				this.colorRender.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public void SetSkin(string skin)
		{
			if (this.spineAnimator != null)
			{
				this.spineAnimator.SetSkin(skin);
			}
		}

		public void SetOrderLayer(int layer)
		{
			if (this.spineAnimator != null)
			{
				this.spineAnimator.SetOrderLayer(layer);
			}
		}

		public void FastMove()
		{
			this.spineAnimator.PlayAni("FastMove", true);
		}

		public void NormalMove()
		{
			this.spineAnimator.PlayAni("Move", true);
		}

		public void StopMove()
		{
			this.spineAnimator.PlayAni("Idle", true);
		}

		public void HideAni(Action onFinish)
		{
			if (this.colorRender != null)
			{
				this.colorRender.PlayAlgha(onFinish);
				return;
			}
			if (onFinish != null)
			{
				onFinish();
			}
		}

		public void ResetAlpha()
		{
			this.colorRender.SetFillAlgha(1f);
		}

		public void PlayVColor(float to, float duration, Action onFinish = null)
		{
			if (this.colorRender != null)
			{
				this.colorRender.PlayVColor(to, duration, onFinish);
				return;
			}
			if (onFinish != null)
			{
				onFinish();
			}
		}

		public void SetVColor(float to)
		{
			if (this.colorRender != null)
			{
				this.colorRender.SetFillVColor(to);
			}
		}

		public float GetPlayAlphaTime()
		{
			if (this.colorRender != null)
			{
				return this.colorRender.alghaDurtion;
			}
			return 0f;
		}

		public Transform pointPlayer;

		public SkeletonAnimation skeleton;

		public ColorRender colorRender;

		private SpineAnimator spineAnimator;

		private Renderer[] renderers;

		private const string Animation_FastMove = "FastMove";

		private const string Animation_NormalMove = "Move";

		private const string Animation_StopMove = "Idle";
	}
}
