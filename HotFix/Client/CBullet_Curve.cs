using System;
using System.Threading.Tasks;
using Framework;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public class CBullet_Curve : CBulletBase
	{
		protected override void OnInit()
		{
			this.arrowTransform = this.m_gameObject.transform;
			this.startPoint = new Vector2(this.m_startPos.x, this.m_startPos.y);
			this.endPoint = new Vector2(this.m_endPos.x, this.m_endPos.y);
			this.flightDuration = Config.GetTimeByFrame(base.m_frame);
			if (!this.m_data.CurveID.Equals(0))
			{
				this.curve = GameApp.UnityGlobal.Curve.GetAnimationCurve(this.m_data.CurveID);
			}
			this.elapsedTime = 0f;
			bool flag = this.m_owner.m_memberData.Camp != MemberCamp.Friendly;
			this.arrowTransform.rotation = Quaternion.Euler(0f, 0f, flag ? (-30f) : 30f);
			this.m_gameObject.transform.localScale = (flag ? new Vector3(-1f, 1f, 1f) : Vector3.one);
			base.IsPlaying = true;
		}

		protected override Task OnDeInit()
		{
			return Task.CompletedTask;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (base.IsPlaying)
			{
				this.Move(this.startPoint, this.endPoint, this.curve);
			}
		}

		protected override void OnReadParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<CBullet_Curve.Data>(parameters) : new CBullet_Curve.Data());
		}

		private void Move(Vector2 start, Vector2 end, AnimationCurve curve)
		{
			if (this.elapsedTime < this.flightDuration)
			{
				float num = this.elapsedTime / this.flightDuration;
				float num2 = curve.Evaluate(num);
				Vector2 vector = Vector2.Lerp(start, end, num) + Vector2.up * num2;
				this.arrowTransform.position = vector;
				float num3 = curve.Evaluate(num + 0.1f) - num2;
				Vector2 normalized = new Vector2((float)((this.m_owner.m_memberData.Camp != MemberCamp.Friendly) ? 1 : (-1)), num3).normalized;
				float num4 = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
				this.arrowTransform.rotation = Quaternion.Euler(0f, 0f, -num4 * this.angleValue);
				this.elapsedTime += Time.deltaTime;
				return;
			}
			base.IsPlaying = false;
			base.BulletHit();
		}

		private CBullet_Curve.Data m_data;

		private Transform arrowTransform;

		private Vector2 startPoint;

		private Vector2 endPoint;

		private float flightDuration = 0.5f;

		private float angleValue = 2f;

		private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 0f, 0f);

		private float elapsedTime;

		private class Data
		{
			public int CurveID;
		}
	}
}
