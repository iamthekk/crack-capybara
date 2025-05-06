using System;
using UnityEngine;

namespace Framework.Logic.UI.UTweener
{
	[AddComponentMenu("Tools/UGUI/Tween/Tween Color")]
	public class UTweenColor : UTweener
	{
		private void Cache()
		{
			this.mSr = base.GetComponent<SpriteRenderer>();
			Renderer component = base.GetComponent<Renderer>();
			if (component != null)
			{
				this.mMat = component.material;
			}
			this.mCanvas = base.GetComponent<CanvasRenderer>();
			this.mLight = base.GetComponent<Light>();
			this.mCached = true;
		}

		[Obsolete("Use 'value' instead")]
		public Color color
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		public Color value
		{
			get
			{
				if (!this.mCached)
				{
					this.Cache();
				}
				if (this.mMat != null)
				{
					return this.mMat.color;
				}
				if (this.mSr != null)
				{
					return this.mSr.color;
				}
				if (this.mLight != null)
				{
					return this.mLight.color;
				}
				if (this.mCanvas != null)
				{
					return this.mCanvas.GetColor();
				}
				return Color.black;
			}
			set
			{
				if (!this.mCached)
				{
					this.Cache();
				}
				if (this.mMat != null)
				{
					this.mMat.color = value;
					return;
				}
				if (this.mSr != null)
				{
					this.mSr.color = value;
					return;
				}
				if (this.mCanvas != null)
				{
					this.mCanvas.SetColor(value);
					return;
				}
				if (this.mLight != null)
				{
					this.mLight.color = value;
					this.mLight.enabled = value.r + value.g + value.b > 0.01f;
				}
			}
		}

		protected override void OnUpdate(float factor, bool isFinished)
		{
			this.value = Color.Lerp(this.from, this.to, factor);
		}

		public static UTweenColor Begin(GameObject go, float duration, Color color)
		{
			UTweenColor utweenColor = UTweener.Begin<UTweenColor>(go, duration);
			utweenColor.from = utweenColor.value;
			utweenColor.to = color;
			if (duration <= 0f)
			{
				utweenColor.Sample(1f, true);
				utweenColor.enabled = false;
			}
			return utweenColor;
		}

		[ContextMenu("Set 'From' to current value")]
		public override void SetStartToCurrentValue()
		{
			this.from = this.value;
		}

		[ContextMenu("Set 'To' to current value")]
		public override void SetEndToCurrentValue()
		{
			this.to = this.value;
		}

		[ContextMenu("Assume value of 'From'")]
		private void SetCurrentValueToStart()
		{
			this.value = this.from;
		}

		[ContextMenu("Assume value of 'To'")]
		private void SetCurrentValueToEnd()
		{
			this.value = this.to;
		}

		public Color from = Color.white;

		public Color to = Color.white;

		private bool mCached;

		private Material mMat;

		private Light mLight;

		private SpriteRenderer mSr;

		private CanvasRenderer mCanvas;
	}
}
