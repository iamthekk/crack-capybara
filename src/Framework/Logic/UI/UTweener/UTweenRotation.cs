using System;
using UnityEngine;

namespace Framework.Logic.UI.UTweener
{
	[AddComponentMenu("Tools/UGUI/Tween/Tween Rotation")]
	public class UTweenRotation : UTweener
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

		[Obsolete("Use 'value' instead")]
		public Quaternion rotation
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

		public Quaternion value
		{
			get
			{
				return this.cachedTransform.localRotation;
			}
			set
			{
				this.cachedTransform.localRotation = value;
			}
		}

		protected override void OnUpdate(float factor, bool isFinished)
		{
			this.value = (this.quaternionLerp ? Quaternion.Slerp(Quaternion.Euler(this.from), Quaternion.Euler(this.to), factor) : Quaternion.Euler(new Vector3(Mathf.Lerp(this.from.x, this.to.x, factor), Mathf.Lerp(this.from.y, this.to.y, factor), Mathf.Lerp(this.from.z, this.to.z, factor))));
		}

		public static UTweenRotation Begin(GameObject go, float duration, Quaternion rot)
		{
			UTweenRotation utweenRotation = UTweener.Begin<UTweenRotation>(go, duration);
			utweenRotation.from = utweenRotation.value.eulerAngles;
			utweenRotation.to = rot.eulerAngles;
			if (duration <= 0f)
			{
				utweenRotation.Sample(1f, true);
				utweenRotation.enabled = false;
			}
			return utweenRotation;
		}

		[ContextMenu("Set 'From' to current value")]
		public override void SetStartToCurrentValue()
		{
			this.from = this.value.eulerAngles;
		}

		[ContextMenu("Set 'To' to current value")]
		public override void SetEndToCurrentValue()
		{
			this.to = this.value.eulerAngles;
		}

		[ContextMenu("Assume value of 'From'")]
		private void SetCurrentValueToStart()
		{
			this.value = Quaternion.Euler(this.from);
		}

		[ContextMenu("Assume value of 'To'")]
		private void SetCurrentValueToEnd()
		{
			this.value = Quaternion.Euler(this.to);
		}

		public Vector3 from;

		public Vector3 to;

		public bool quaternionLerp;

		private Transform mTrans;
	}
}
