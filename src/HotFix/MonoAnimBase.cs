using System;
using Framework;
using UnityEngine;

namespace HotFix
{
	public abstract class MonoAnimBase : MonoBehaviour
	{
		public void PlayShowAnimation(float startTime, float during)
		{
			this._startTime = startTime;
			this._endTime = this._startTime + during;
			if (this._curve == null)
			{
				this._curve = GameApp.UnityGlobal.Curve.GetAnimationCurve(100024);
			}
			this.CheckAnimation();
		}

		private void Start()
		{
		}

		private void CheckAnimation()
		{
			float unscaledTime = Time.unscaledTime;
			if (unscaledTime < this._startTime && this._state != MonoAnimBase.AnimState.Idle)
			{
				this.SetState(MonoAnimBase.AnimState.Idle);
				return;
			}
			if (unscaledTime > this._endTime && this._state != MonoAnimBase.AnimState.Finish)
			{
				this.SetState(MonoAnimBase.AnimState.Finish);
				return;
			}
			this.SetState(MonoAnimBase.AnimState.Running);
		}

		private void SetState(MonoAnimBase.AnimState state)
		{
			this._state = state;
			switch (state)
			{
			case MonoAnimBase.AnimState.Idle:
				this.SetPercent(0f);
				return;
			case MonoAnimBase.AnimState.Running:
			{
				float num = (Time.unscaledTime - this._startTime) / (this._endTime - this._startTime);
				num = Mathf.Clamp01(num);
				if (this._curve == null)
				{
					this._curve = GameApp.UnityGlobal.Curve.GetAnimationCurve(100024);
				}
				num = this._curve.Evaluate(num);
				this.SetPercent(num);
				return;
			}
			case MonoAnimBase.AnimState.Finish:
				this.SetPercent(1f);
				return;
			default:
				return;
			}
		}

		private void Update()
		{
			this.CheckAnimation();
		}

		protected abstract void SetPercent(float percent);

		private float _startTime;

		private float _endTime;

		private AnimationCurve _curve;

		private MonoAnimBase.AnimState _state;

		private enum AnimState
		{
			None,
			Idle,
			Running,
			Finish
		}
	}
}
