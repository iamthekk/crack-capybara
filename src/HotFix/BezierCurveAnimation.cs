using System;
using UnityEngine;

namespace HotFix
{
	public class BezierCurveAnimation
	{
		public void StartAni(Transform trans, Vector3 startPos, Vector3 endPos, float height, float durationTime, Action onFinish)
		{
			this.transform = trans;
			this.startPoint = startPos;
			this.endPoint = endPos;
			this.maxHeight = height;
			this.duration = durationTime;
			this.onAniFinish = onFinish;
			this.startTime = Time.time;
			this.isStart = true;
		}

		public void Update()
		{
			if (this.isPause)
			{
				return;
			}
			if (!this.isStart)
			{
				return;
			}
			float num = (Time.time - this.startTime) / this.duration;
			if (num <= 1f)
			{
				Vector3 vector = this.CalculateBezierPoint(num);
				this.transform.localPosition = vector;
				return;
			}
			Action action = this.onAniFinish;
			if (action != null)
			{
				action();
			}
			this.isStart = false;
		}

		public void SetPause(bool pause)
		{
			this.isPause = pause;
			if (pause)
			{
				this.pauseStartTime = Time.time;
				return;
			}
			float num = Time.time - this.pauseStartTime;
			this.startTime += num;
		}

		private Vector3 CalculateBezierPoint(float t)
		{
			float num = 1f - t;
			float num2 = t * t;
			float num3 = num * num;
			float num4 = num3 * num;
			float num5 = num2 * t;
			return num4 * this.startPoint + 3f * num3 * t * (this.startPoint + Vector3.up * this.maxHeight) + 3f * num * num2 * (this.endPoint + Vector3.up * this.maxHeight) + num5 * this.endPoint;
		}

		private Transform transform;

		private Vector3 startPoint;

		private Vector3 endPoint;

		private float maxHeight = 2f;

		private float duration = 1f;

		private float startTime;

		private bool isStart;

		private Action onAniFinish;

		private bool isPause;

		private float pauseStartTime;
	}
}
