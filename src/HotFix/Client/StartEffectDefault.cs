using System;
using UnityEngine;

namespace HotFix.Client
{
	public class StartEffectDefault : StartEffectBase
	{
		public override void OnInit(StartEffectFactory factory, GameObject gameObject, CMemberBase owner, CSkillBase skill, PointRotationDirection direction)
		{
			base.OnInit(factory, gameObject, owner, skill, direction);
			this.m_time = 0f;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.m_time += deltaTime;
			if (this.m_time >= 5f)
			{
				if (this.m_factory != null)
				{
					this.m_factory.RemoveNode(this);
				}
				this.m_time = 0f;
			}
		}

		public const float m_duration = 5f;

		private float m_time;
	}
}
