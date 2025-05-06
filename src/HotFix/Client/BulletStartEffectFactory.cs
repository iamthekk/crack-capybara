using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public class BulletStartEffectFactory
	{
		public BulletStartEffectFactory(CMemberBase owner, CSkillBase skill)
		{
			this.m_owner = owner;
			this.m_skill = skill;
		}

		public void OnInit()
		{
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.nodes = this.m_nodes.Values.ToList<BulletStartEffectBase>();
			for (int i = 0; i < this.nodes.Count; i++)
			{
				BulletStartEffectBase bulletStartEffectBase = this.nodes[i];
				if (bulletStartEffectBase != null)
				{
					bulletStartEffectBase.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
		}

		public async Task OnDeInit()
		{
			await this.RemoveAllNodes();
		}

		public async Task OnReset()
		{
			await this.RemoveAllNodes();
		}

		public void OnGameStart()
		{
		}

		public void OnPause(bool pause)
		{
			List<BulletStartEffectBase> list = this.m_nodes.Values.ToList<BulletStartEffectBase>();
			for (int i = 0; i < list.Count; i++)
			{
				BulletStartEffectBase bulletStartEffectBase = list[i];
				if (bulletStartEffectBase != null)
				{
					bulletStartEffectBase.OnPause(pause);
				}
			}
		}

		public void OnGameOver(GameOverType gameOverType)
		{
		}

		public async Task AddNode(TaskOutValue<BulletStartEffectBase> outBulletStartEffect, Vector3 pos, SkillFireBulletData fireBulletData)
		{
			if (fireBulletData != null)
			{
				if (fireBulletData.m_bulletStartPrefabID != 0)
				{
					ArtSkill_Skill table = GameApp.Table.GetManager().GetArtSkill_SkillModelInstance().GetElementById(fireBulletData.m_bulletStartPrefabID);
					if (table != null)
					{
						await PoolManager.Instance.CheckPrefab(table.path);
						GameObject gameObject = PoolManager.Instance.Out(table.path, pos, this.m_owner.m_gameObject.transform.rotation, null);
						BulletStartEffectDefault bulletStartEffectDefault = new BulletStartEffectDefault();
						bulletStartEffectDefault.OnInit(this, gameObject, this.m_owner, this.m_skill);
						this.m_nodes[gameObject.GetInstanceID()] = bulletStartEffectDefault;
						outBulletStartEffect.SetValue(bulletStartEffectDefault);
					}
				}
			}
		}

		public async Task RemoveNode(BulletStartEffectBase node)
		{
			if (node != null)
			{
				await node.OnDeInit();
				this.m_nodes.Remove(node.m_instanceID);
				PoolManager.Instance.Put(node.m_gameObject);
			}
		}

		public async Task RemoveAllNodes()
		{
			List<BulletStartEffectBase> nodes = this.m_nodes.Values.ToList<BulletStartEffectBase>();
			for (int i = 0; i < nodes.Count; i++)
			{
				BulletStartEffectBase bulletStartEffectBase = nodes[i];
				await this.RemoveNode(bulletStartEffectBase);
			}
			this.m_nodes.Clear();
		}

		public CSkillBase m_skill;

		public CMemberBase m_owner;

		private Dictionary<int, BulletStartEffectBase> m_nodes = new Dictionary<int, BulletStartEffectBase>();

		private List<BulletStartEffectBase> nodes;
	}
}
