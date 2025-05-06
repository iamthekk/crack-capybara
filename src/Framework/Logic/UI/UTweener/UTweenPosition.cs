using System;
using UnityEngine;

namespace Framework.Logic.UI.UTweener
{
	[AddComponentMenu("Tools/UGUI/Tween/Tween Position")]
	public class UTweenPosition : UTweener
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

		public RectTransform cachedRectTransform
		{
			get
			{
				if (this.mRectTrans == null)
				{
					this.mRectTrans = base.GetComponent<RectTransform>();
				}
				return this.mRectTrans;
			}
		}

		public Vector3 value
		{
			get
			{
				switch (this.PositionType)
				{
				case TweenPositionType.Position:
					return base.transform.position;
				case TweenPositionType.LocalPosition:
					return base.transform.localPosition;
				case TweenPositionType.AnchoredPosition:
					return this.cachedRectTransform.anchoredPosition;
				default:
					return Vector3.zero;
				}
			}
			set
			{
				switch (this.PositionType)
				{
				case TweenPositionType.Position:
					base.transform.position = value;
					return;
				case TweenPositionType.LocalPosition:
					base.transform.localPosition = value;
					return;
				case TweenPositionType.AnchoredPosition:
					this.cachedRectTransform.anchoredPosition = value;
					return;
				default:
					return;
				}
			}
		}

		private void Awake()
		{
		}

		protected override void OnUpdate(float factor, bool isFinished)
		{
			this.value = this.from * (1f - factor) + this.to * factor;
		}

		public static UTweenPosition Begin(GameObject go, float duration, Vector3 pos)
		{
			UTweenPosition utweenPosition = UTweener.Begin<UTweenPosition>(go, duration);
			utweenPosition.from = utweenPosition.value;
			utweenPosition.to = pos;
			if (duration <= 0f)
			{
				utweenPosition.Sample(1f, true);
				utweenPosition.enabled = false;
			}
			return utweenPosition;
		}

		public static UTweenPosition Begin(GameObject go, float duration, Vector3 pos, TweenPositionType positionType)
		{
			UTweenPosition utweenPosition = UTweener.Begin<UTweenPosition>(go, duration);
			utweenPosition.PositionType = positionType;
			utweenPosition.from = utweenPosition.value;
			utweenPosition.to = pos;
			if (duration <= 0f)
			{
				utweenPosition.Sample(1f, true);
				utweenPosition.enabled = false;
			}
			return utweenPosition;
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

		public Vector3 from;

		public Vector3 to;

		public TweenPositionType PositionType;

		private Transform mTrans;

		private RectTransform mRectTrans;
	}
}
