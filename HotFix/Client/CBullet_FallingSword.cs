using System;
using System.Threading.Tasks;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public class CBullet_FallingSword : CBulletBase
	{
		protected override void OnInit()
		{
			double num = -this.m_xCamp / 2.0 + this.random.NextDouble() * this.m_xCamp;
			double num2 = -this.m_yCamp / 2.0 + this.random.NextDouble() * this.m_yCamp;
			this.m_gameObject.transform.position = this.m_endPos + new Vector3((float)num, (float)num2, 0f);
			this.startTime = Time.time;
			this.durationTime = Config.GetTimeByFrame(13);
			this.durationTime -= this.m_loadTime;
			if (this.durationTime < 0.01f)
			{
				this.durationTime = 0.01f;
			}
		}

		protected override Task OnDeInit()
		{
			return null;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (!base.IsPlaying)
			{
				return;
			}
			this.elapsedTime = Time.time - this.startTime;
			if (this.elapsedTime >= this.durationTime)
			{
				base.IsPlaying = false;
				base.BulletHit();
			}
		}

		protected override void OnReadParameters(string parameters)
		{
			this.m_data = ((!string.IsNullOrEmpty(parameters)) ? JsonManager.ToObject<CBullet_FallingSword.Data>(parameters) : new CBullet_FallingSword.Data());
			this.m_xCamp = this.m_data.xCamp;
			this.m_yCamp = this.m_data.yCamp;
		}

		private CBullet_FallingSword.Data m_data;

		private bool m_isFollowTarget;

		private double m_xCamp;

		private double m_yCamp;

		private float durationTime;

		private float startTime;

		private Random random = new Random();

		private float elapsedTime;

		private class Data
		{
			public double xCamp = 3.0;

			public double yCamp = 2.0;
		}
	}
}
