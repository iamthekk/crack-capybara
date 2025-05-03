using System;
using System.Threading.Tasks;
using Framework;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix.Client
{
	public abstract class CBulletBase
	{
		public int m_frame { get; protected set; }

		public float m_destroyDuation { get; protected set; }

		public bool IsPlaying { get; protected set; }

		public void Init(CBulletFactory bulletFactory, CFireBulletData fireData, GameBullet_bullet table, CMemberBase owner)
		{
			this.m_instanceID = this.GetHashCode();
			this.isDeInit = false;
			this.m_bulletFactory = bulletFactory;
			this.m_fireBulletData = fireData;
			this.m_frame = table.frame;
			this.m_destroyDuation = table.destroyDuation;
			this.m_owner = owner;
			this.m_endPos = fireData.m_endPoint.position;
			this.startLoadTime = Time.time;
			this.LoadRes();
		}

		protected async Task LoadRes()
		{
			if (this.m_fireBulletData != null)
			{
				string path = this.m_fireBulletData.m_artPath;
				await PoolManager.Instance.CheckPrefab(path);
				this.m_loadTime = Time.time - this.startLoadTime;
				if (!this.isDeInit)
				{
					GameObject gameObject;
					if (!this.m_fireBulletData.IsMainTarget && !this.m_fireBulletData.IsShowBullet)
					{
						gameObject = new GameObject();
					}
					else
					{
						gameObject = PoolManager.Instance.Out(path, this.m_fireBulletData.m_startPoint.position, Quaternion.identity, null);
					}
					gameObject.SetActiveSafe(false);
					Transform transform = gameObject.transform;
					Vector3 position = this.m_fireBulletData.m_startPoint.position;
					transform.position = position;
					this.m_startPos = position;
					this.m_gameObject = gameObject;
					this.m_gameObject.SetActiveSafe(true);
					this.OnInit();
					this.IsPlaying = true;
				}
			}
		}

		public async Task DeInit()
		{
			this.isDeInit = true;
			this.IsPlaying = false;
			if (this.m_gameObject != null)
			{
				PoolManager.Instance.Put(this.m_gameObject);
			}
			await this.OnDeInit();
		}

		public void Update(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_isCountdownDestroy)
			{
				this.m_curDestroyTime -= deltaTime;
				if (this.m_curDestroyTime <= 0f)
				{
					this.m_curDestroyTime = 0f;
					this.m_isCountdownDestroy = false;
					this.m_bulletFactory.RemoveBullet(this);
				}
			}
			if (!this.IsPlaying)
			{
				return;
			}
			this.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public void ReadParameters(string parameters)
		{
			this.OnReadParameters(parameters);
		}

		protected abstract void OnReadParameters(string parameters);

		protected abstract void OnUpdate(float deltaTime, float unscaledDeltaTime);

		protected abstract void OnInit();

		protected abstract Task OnDeInit();

		protected void BulletHit()
		{
			this.OnCountdownDeInit();
			GameApp.Sound.PlayClip(this.m_fireBulletData.m_hitSoundID, 1f);
		}

		protected void OnCountdownDeInit()
		{
			this.IsPlaying = false;
			this.m_curDestroyTime = this.m_destroyDuation;
			this.m_isCountdownDestroy = true;
		}

		public int m_instanceID;

		public GameObject m_gameObject;

		protected Vector3 m_startPos;

		protected Vector3 m_endPos;

		protected bool m_isCountdownDestroy;

		protected float m_curDestroyTime;

		protected float m_loadTime = 999f;

		public CMemberBase m_owner;

		public CBulletFactory m_bulletFactory;

		public CFireBulletData m_fireBulletData;

		protected bool isDeInit;

		private float startLoadTime;
	}
}
