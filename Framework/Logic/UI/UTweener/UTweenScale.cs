using System;
using UnityEngine;

namespace Framework.Logic.UI.UTweener
{
	[AddComponentMenu("Tools/UGUI/Tween/Tween Scale")]
	public class UTweenScale : UTweener
	{
		public Transform cachedTransform
		{
			get
			{
				if (this.mTrans == null)
				{
					this.mTrans = base.transform;
				}
				return this.mTrans;
			}
		}

		public Vector3 value
		{
			get
			{
				return this.cachedTransform.localScale;
			}
			set
			{
				this.cachedTransform.localScale = value;
			}
		}

		[Obsolete("Use 'value' instead")]
		public Vector3 scale
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

		protected override void OnUpdate(float factor, bool isFinished)
		{
			this.value = this.from * (1f - factor) + this.to * factor;
		}

		public static UTweenScale Begin(GameObject go, float duration, Vector3 scale)
		{
			UTweenScale utweenScale = UTweener.Begin<UTweenScale>(go, duration);
			utweenScale.from = utweenScale.value;
			utweenScale.to = scale;
			if (duration <= 0f)
			{
				utweenScale.Sample(1f, true);
				utweenScale.enabled = false;
			}
			return utweenScale;
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

		public Vector3 from = Vector3.one;

		public Vector3 to = Vector3.one;

		private Transform mTrans;
	}
}
