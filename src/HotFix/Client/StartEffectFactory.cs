using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix.Client
{
	public class StartEffectFactory
	{
		public StartEffectFactory(CMemberBase owner, CSkillBase skill)
		{
			this.m_owner = owner;
			this.m_skill = skill;
		}

		public void OnInit()
		{
			int startPrefabID = this.m_skill.skillData.m_startPrefabID;
			if (startPrefabID.Equals(0))
			{
				return;
			}
			ArtSkill_Skill elementById = GameApp.Table.GetManager().GetArtSkill_SkillModelInstance().GetElementById(startPrefabID);
			if (elementById == null)
			{
				return;
			}
			PoolManager.Instance.Cache(elementById.path);
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.nodes = this.m_nodes.Values.ToList<StartEffectBase>();
			for (int i = 0; i < this.nodes.Count; i++)
			{
				StartEffectBase startEffectBase = this.nodes[i];
				if (startEffectBase != null)
				{
					startEffectBase.OnUpdate(deltaTime, unscaledDeltaTime);
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
			List<StartEffectBase> list = this.m_nodes.Values.ToList<StartEffectBase>();
			for (int i = 0; i < list.Count; i++)
			{
				StartEffectBase startEffectBase = list[i];
				if (startEffectBase != null)
				{
					startEffectBase.OnPause(pause);
				}
			}
		}

		public void OnGameOver(GameOverType gameOverType)
		{
		}

		public async Task AddNode(TaskOutValue<StartEffectBase> outstartEffect)
		{
			int startPrefabID = this.m_skill.skillData.m_startPrefabID;
			if (!startPrefabID.Equals(0))
			{
				ArtSkill_Skill table = GameApp.Table.GetManager().GetArtSkill_SkillModelInstance().GetElementById(startPrefabID);
				if (table != null)
				{
					Transform transform = this.m_owner.m_body.GetTransform(this.m_skill.skillData.m_startPosID);
					if (!(transform == null))
					{
						await PoolManager.Instance.CheckPrefab(table.path);
						GameObject gameObject = PoolManager.Instance.Out(table.path, transform.position, this.m_owner.m_gameObject.transform.rotation, transform);
						StartEffectDefault startEffectDefault = new StartEffectDefault();
						startEffectDefault.OnInit(this, gameObject, this.m_owner, this.m_skill, this.m_skill.skillData.m_startDirectionID);
						this.m_nodes[gameObject.GetInstanceID()] = startEffectDefault;
						outstartEffect.SetValue(startEffectDefault);
					}
				}
			}
		}

		public async Task RemoveNode(StartEffectBase node)
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
			List<StartEffectBase> nodes = this.m_nodes.Values.ToList<StartEffectBase>();
			for (int i = 0; i < nodes.Count; i++)
			{
				StartEffectBase startEffectBase = nodes[i];
				await this.RemoveNode(startEffectBase);
			}
			this.m_nodes.Clear();
		}

		public CSkillBase m_skill;

		public CMemberBase m_owner;

		private Dictionary<int, StartEffectBase> m_nodes = new Dictionary<int, StartEffectBase>();

		private List<StartEffectBase> nodes;
	}
}
