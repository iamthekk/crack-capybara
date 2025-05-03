using System;
using System.Threading.Tasks;
using Framework.Platfrom;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public abstract class CBuffBase
	{
		public void SetGuid(string guid)
		{
			this.m_guid = guid;
		}

		public void Init(CBuffFactory factory, CBuffData buffData, CMemberBase owner, CMemberBase attacker, PointRotationDirection direction)
		{
			this.isDeInit = false;
			this.m_buffData = buffData;
			this.m_buffFactory = factory;
			this.m_owner = owner;
			this.m_attacker = attacker;
			this.ReadParameters(buffData.m_parameters);
			this.OnInit();
			this.LoadRes(direction);
		}

		public async Task LoadRes(PointRotationDirection direction)
		{
			if (this.m_buffData != null)
			{
				string path = this.m_buffData.m_prefabPath;
				if (!path.Equals(string.Empty))
				{
					await PoolManager.Instance.CheckPrefab(path);
				}
				if (!this.isDeInit)
				{
					Transform transform = this.m_owner.m_body.GetTransform(this.m_buffData.m_bodyType);
					GameObject gameObject = PoolManager.Instance.Out(path, transform.position, transform.rotation, transform);
					this.m_gameObject = gameObject;
					this.m_pointRotation = new PointRotationController(gameObject, direction, this.m_owner.m_gameObject);
					string removeEffectPath = this.m_buffData.m_removeEffectPath;
					if (!removeEffectPath.Equals(string.Empty))
					{
						await PoolManager.Instance.CheckPrefab(removeEffectPath);
					}
				}
			}
		}

		public void Update(float deltaTime, float unscaledDeltaTime)
		{
			PointRotationController pointRotation = this.m_pointRotation;
			if (pointRotation != null)
			{
				pointRotation.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			this.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public void DeInit(bool removeEffectImmediate = false)
		{
			this.isDeInit = true;
			this.OnDeInit();
			if (this.m_gameObject != null)
			{
				if (removeEffectImmediate)
				{
					this.RecycleGo();
				}
				else if (this.m_buffData.m_effectDuration <= 0f)
				{
					this.RecycleGo();
				}
				else
				{
					float num = MathTools.Clamp(this.m_buffData.m_effectDuration - this.m_buffTimer, 0f, this.m_buffData.m_effectDuration);
					if ((double)num <= 0.01)
					{
						this.RecycleGo();
					}
					else
					{
						DelayCall.Instance.CallOnce((int)(num * 1000f), new DelayCall.CallAction(this.RecycleGo));
					}
				}
			}
			this.m_owner = null;
			this.m_attacker = null;
			this.m_buffFactory = null;
			this.m_pointRotation = null;
			this.m_guid = null;
			CBuffData buffData = this.m_buffData;
			if (buffData != null)
			{
				buffData.Dispose();
			}
			this.m_buffData = null;
		}

		private void RecycleGo()
		{
			if (this.m_gameObject != null)
			{
				PoolManager.Instance.Put(this.m_gameObject);
				this.CreateRemoveEffect();
				this.m_gameObject = null;
			}
		}

		private async Task CreateRemoveEffect()
		{
			if (this.m_buffData != null)
			{
				string path = this.m_buffData.m_removeEffectPath;
				if (!path.Equals(string.Empty))
				{
					await PoolManager.Instance.CheckPrefab(path);
					Transform transform = this.m_owner.m_body.GetTransform(this.m_buffData.m_removeEffectPos);
					GameObject obj = PoolManager.Instance.Out(path, transform.position, transform.rotation, transform);
					await TaskExpand.Delay(1000);
					PoolManager.Instance.Put(obj);
					obj = null;
				}
				await Task.CompletedTask;
			}
		}

		public virtual void OnInit()
		{
		}

		protected virtual void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.m_buffTimer += deltaTime;
		}

		public virtual void OnDeInit()
		{
		}

		public virtual void OnTrigger(int layer)
		{
		}

		public abstract void ReadParameters(string parameters);

		public CMemberBase m_owner;

		public CMemberBase m_attacker;

		public string m_guid;

		public CBuffFactory m_buffFactory;

		public CBuffData m_buffData;

		private GameObject m_gameObject;

		public PointRotationController m_pointRotation;

		protected float m_buffTimer;

		protected bool isDeInit;
	}
}
