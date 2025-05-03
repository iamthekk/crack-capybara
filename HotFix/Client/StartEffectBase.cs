using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HotFix.Client
{
	public class StartEffectBase
	{
		public virtual void OnInit(StartEffectFactory factory, GameObject gameObject, CMemberBase owner, CSkillBase skill, PointRotationDirection direction)
		{
			this.m_factory = factory;
			this.m_owner = owner;
			this.m_skill = skill;
			this.m_gameObject = gameObject;
			this.m_instanceID = this.m_gameObject.GetInstanceID();
			this.m_particleSystem = this.m_gameObject.GetComponentInChildren<ParticleSystem>();
			this.m_pointRotation = new PointRotationController(this.m_gameObject, direction, owner.m_gameObject);
		}

		public virtual void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			PointRotationController pointRotation = this.m_pointRotation;
			if (pointRotation == null)
			{
				return;
			}
			pointRotation.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public virtual async Task OnDeInit()
		{
			this.m_pointRotation = null;
			await Task.CompletedTask;
		}

		public virtual async Task OnReset()
		{
			await Task.CompletedTask;
		}

		public virtual void OnPause(bool pause)
		{
			if (this.m_particleSystem != null && this.m_particleSystem.isPlaying)
			{
				this.m_particleSystem.Pause(pause);
			}
		}

		public int m_instanceID;

		public GameObject m_gameObject;

		public CMemberBase m_owner;

		public CSkillBase m_skill;

		protected StartEffectFactory m_factory;

		protected ParticleSystem m_particleSystem;

		protected PointRotationController m_pointRotation;

		protected List<GameObject> m_allObjects = new List<GameObject>();
	}
}
