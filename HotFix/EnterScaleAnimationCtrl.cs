using System;
using Framework;
using UnityEngine;

namespace HotFix
{
	public class EnterScaleAnimationCtrl : MonoBehaviour
	{
		private void OnDisable()
		{
			Object.Destroy(this);
		}

		private void Update()
		{
			this.CheckAnimation();
		}

		public EnterScaleAnimationCtrl PlayShowAnimation(float startTime, int index, int curveId = 100025)
		{
			this.m_curve = GameApp.UnityGlobal.Curve.GetAnimationCurve(curveId);
			this.m_child = base.GetComponent<RectTransform>();
			this.m_startTime = startTime + (float)index * 0.05f;
			this.m_endTime = this.m_startTime + 0.2f;
			this.m_child.localScale = Vector3.zero;
			this.CheckAnimation();
			return this;
		}

		private void CheckAnimation()
		{
			if (this.m_child == null)
			{
				return;
			}
			float time = Time.time;
			if (time < this.m_startTime)
			{
				return;
			}
			if (time > this.m_endTime)
			{
				this.m_child.localScale = Vector3.one;
				Action action = this.onCompleted;
				if (action != null)
				{
					action();
				}
				Object.Destroy(this);
				return;
			}
			float num = (time - this.m_startTime) / (this.m_endTime - this.m_startTime);
			num = Mathf.Clamp01(num);
			float num2 = this.m_curve.Evaluate(num);
			this.m_child.localScale = Vector3.one * num2;
		}

		public RectTransform m_child;

		private float m_startTime;

		private float m_endTime;

		private AnimationCurve m_curve;

		public Action onCompleted;
	}
}
