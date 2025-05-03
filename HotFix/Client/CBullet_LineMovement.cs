using System;
using System.Threading.Tasks;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public class CBullet_LineMovement : CBulletBase
	{
		protected override void OnInit()
		{
			this.moveTime = Config.GetTimeByFrame(base.m_frame);
			this.startTime = Time.time;
			this.moveTime -= this.m_loadTime;
			if (this.moveTime < 0.05f)
			{
				this.moveTime = 0.05f;
			}
			if (this.m_lookAt)
			{
				this.LookAt(this.m_gameObject.transform.position, this.m_endPos);
			}
		}

		protected override Task OnDeInit()
		{
			return null;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (!this.isMoving)
			{
				float num = Time.time - this.startTime;
				if (num < this.moveTime)
				{
					float num2 = num / this.moveTime;
					this.m_gameObject.transform.position = Vector3.Lerp(this.m_startPos, this.m_endPos, num2);
					return;
				}
				this.m_gameObject.transform.position = this.m_endPos;
				this.isMoving = true;
				base.BulletHit();
			}
		}

		protected override void OnReadParameters(string parameters)
		{
			if (!string.IsNullOrEmpty(parameters))
			{
				this.m_data = JsonManager.ToObject<CBullet_LineMovement.Data>(parameters);
				this.m_lookAt = this.m_data.LookAt != 0;
				return;
			}
			this.m_data = new CBullet_LineMovement.Data();
		}

		private void LookAt(Vector3 start, Vector3 end)
		{
			Vector2 vector = (end - start).normalized;
			this.m_gameObject.transform.right = vector;
		}

		private bool isMoving;

		private float moveTime = 0.16f;

		private float startTime;

		public CBullet_LineMovement.Data m_data;

		private bool m_lookAt = true;

		public class Data
		{
			public int LookAt;
		}
	}
}
