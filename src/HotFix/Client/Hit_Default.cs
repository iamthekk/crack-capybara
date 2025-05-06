using System;
using System.Threading.Tasks;

namespace HotFix.Client
{
	public class Hit_Default : HitBase
	{
		protected override void OnInit()
		{
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.m_curTime += deltaTime;
			if (this.m_curTime >= 2f)
			{
				if (this.m_factory != null)
				{
					this.m_factory.RemoveHit(this);
				}
				this.m_curTime = 0f;
			}
		}

		protected override Task OnDeInit()
		{
			return Task.CompletedTask;
		}

		public const float m_duration = 2f;

		private float m_curTime;
	}
}
