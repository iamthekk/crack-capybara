using System;
using System.Threading.Tasks;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public abstract class BulletStartEffectBase
	{
		public virtual void OnInit(BulletStartEffectFactory factory, GameObject gameObject, CMemberBase owner, CSkillBase skill)
		{
			this.m_factory = factory;
			this.m_gameObject = gameObject;
			this.m_owner = owner;
			this.m_skill = skill;
			this.m_instanceID = this.m_gameObject.GetInstanceID();
			this.SetLayer(false);
			if (owner.m_memberData.Camp == MemberCamp.Friendly)
			{
				this.m_gameObject.transform.localScale = Vector3.one;
				return;
			}
			if (owner.m_memberData.Camp == MemberCamp.Enemy)
			{
				this.m_gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}

		public virtual void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public virtual async Task OnDeInit()
		{
			await Task.CompletedTask;
		}

		public virtual async Task OnReset()
		{
			await Task.CompletedTask;
		}

		public virtual void OnGameStart()
		{
		}

		public virtual void OnPause(bool pause)
		{
			this.SetLayer(pause);
		}

		public virtual void SetLayer(bool pause)
		{
		}

		public virtual void OnGameOver(GameOverType gameOverType)
		{
		}

		public int m_instanceID;

		public GameObject m_gameObject;

		public CMemberBase m_owner;

		public CSkillBase m_skill;

		public BulletStartEffectFactory m_factory;
	}
}
