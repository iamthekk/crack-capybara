using System;
using System.Threading.Tasks;
using UnityEngine;

namespace HotFix.Client
{
	public abstract class HitBase
	{
		public bool IsPlaying { get; protected set; }

		public void Init(HitFactory factory, GameObject gameObject, CMemberBase owner, GameObject target, PointRotationDirection direction)
		{
			this.m_factory = factory;
			this.m_gameObject = gameObject;
			this.m_owner = owner;
			this.OnInit();
			this.IsPlaying = true;
		}

		public void Update(float deltaTime, float unscaledDeltaTime)
		{
			if (!this.IsPlaying)
			{
				return;
			}
			this.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public async Task DeInit()
		{
			this.IsPlaying = false;
			await this.OnDeInit();
		}

		protected abstract void OnInit();

		protected abstract Task OnDeInit();

		protected abstract void OnUpdate(float deltaTime, float unscaledDeltaTime);

		public virtual void OnReset()
		{
		}

		public virtual void OnGameStart()
		{
		}

		public int m_instanceID;

		public GameObject m_gameObject;

		public CMemberBase m_owner;

		public HitFactory m_factory;
	}
}
