using System;
using UnityEngine;

namespace Framework.Logic.UI.UTweener
{
	[AddComponentMenu("Tools/UGUI/Tween/Tween Transform")]
	public class UTweenTransform : UTweener
	{
		protected override void OnUpdate(float factor, bool isFinished)
		{
			if (this.to != null)
			{
				if (this.mTrans == null)
				{
					this.mTrans = base.transform;
					this.mPos = this.mTrans.position;
					this.mRot = this.mTrans.rotation;
					this.mScale = this.mTrans.localScale;
				}
				if (this.from != null)
				{
					this.mTrans.position = this.from.position * (1f - factor) + this.to.position * factor;
					this.mTrans.localScale = this.from.localScale * (1f - factor) + this.to.localScale * factor;
					this.mTrans.rotation = Quaternion.Slerp(this.from.rotation, this.to.rotation, factor);
				}
				else
				{
					this.mTrans.position = this.mPos * (1f - factor) + this.to.position * factor;
					this.mTrans.localScale = this.mScale * (1f - factor) + this.to.localScale * factor;
					this.mTrans.rotation = Quaternion.Slerp(this.mRot, this.to.rotation, factor);
				}
				if (this.parentWhenFinished && isFinished)
				{
					this.mTrans.parent = this.to;
				}
			}
		}

		public static UTweenTransform Begin(GameObject go, float duration, Transform to)
		{
			return UTweenTransform.Begin(go, duration, null, to);
		}

		public static UTweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
		{
			UTweenTransform utweenTransform = UTweener.Begin<UTweenTransform>(go, duration);
			utweenTransform.from = from;
			utweenTransform.to = to;
			if (duration <= 0f)
			{
				utweenTransform.Sample(1f, true);
				utweenTransform.enabled = false;
			}
			return utweenTransform;
		}

		public Transform from;

		public Transform to;

		public bool parentWhenFinished;

		private Transform mTrans;

		private Vector3 mPos;

		private Quaternion mRot;

		private Vector3 mScale;
	}
}
