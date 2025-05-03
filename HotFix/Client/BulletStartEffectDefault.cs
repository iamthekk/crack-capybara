using System;
using UnityEngine;

namespace HotFix.Client
{
	public class BulletStartEffectDefault : BulletStartEffectBase
	{
		public override void OnInit(BulletStartEffectFactory factory, GameObject gameObject, CMemberBase owner, CSkillBase skill)
		{
			this.m_time = 0f;
			base.OnInit(factory, gameObject, owner, skill);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.m_time += deltaTime;
			if (this.m_time >= this.m_duration)
			{
				if (this.m_factory != null)
				{
					this.m_factory.RemoveNode(this);
				}
				this.m_time = 0f;
			}
		}

		public float m_duration = 5f;

		private float m_time;
	}
}
