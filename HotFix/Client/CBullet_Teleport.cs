using System;
using System.Threading.Tasks;
using Server;

namespace HotFix.Client
{
	public class CBullet_Teleport : CBulletBase
	{
		protected override void OnInit()
		{
			this.m_gameObject.transform.position = this.m_endPos;
			if (this.m_isFollowTarget)
			{
				this.m_gameObject.transform.SetParent(this.m_fireBulletData.m_endPoint);
			}
		}

		protected override Task OnDeInit()
		{
			return null;
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (base.IsPlaying)
			{
				base.BulletHit();
			}
		}

		protected override void OnReadParameters(string parameters)
		{
			if (!string.IsNullOrEmpty(parameters))
			{
				CBullet_Teleport.Data data = JsonManager.ToObject<CBullet_Teleport.Data>(parameters);
				this.m_isFollowTarget = data.IsFollowTarget != 0;
				return;
			}
			this.m_isFollowTarget = false;
		}

		private bool m_isFollowTarget;

		private class Data
		{
			public int IsFollowTarget;
		}
	}
}
