using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class ColorRenderCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
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

		public void SetData(SkeletonAnimation skeleton)
		{
			if (skeleton == null)
			{
				return;
			}
			this.renderers = skeleton.GetComponentsInChildren<Renderer>();
			this.colorRender = skeleton.GetComponent<ColorRender>();
			if (this.colorRender == null)
			{
				this.colorRender = base.gameObject.AddComponent<ColorRender>();
				this.colorRender.OnInit(new List<Renderer>(this.renderers));
				this.colorRender.SetFillPhase(0f);
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.colorRender != null)
			{
				this.colorRender.OnUpdate(deltaTime, unscaledDeltaTime);
			}
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

		public void ShowAni(Action onFinish)
		{
			if (this.colorRender != null)
			{
				this.colorRender.PlayAlphaShow(onFinish);
				return;
			}
			if (onFinish != null)
			{
				onFinish();
			}
		}

		public void SetAlpha(float a)
		{
			this.colorRender.SetFillAlgha(a);
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

		private ColorRender colorRender;

		private Renderer[] renderers;
	}
}
