using System;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Modules;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix.Client
{
	public class BigSkillControl
	{
		public bool IsPlaying
		{
			get
			{
				return this.m_isPlaying;
			}
		}

		public void SetOwner(CMemberBase owner)
		{
			this.m_owner = owner;
		}

		public async Task OnInit()
		{
			ArtSkill_Skill artTable = GameApp.Table.GetManager().GetArtSkill_SkillModelInstance().GetElementById(3);
			await PoolManager.Instance.CheckPrefab(artTable.path);
			this.m_effect = PoolManager.Instance.Out(artTable.path, this.m_owner.m_gameObject.transform.position, this.m_owner.m_gameObject.transform.rotation, this.m_owner.m_gameObject.transform);
			this.m_effectPs = this.m_effect.GetComponent<ParticleSystem>();
			this.m_effect.SetActiveSafe(false);
		}

		public void OnDeInit()
		{
			this.m_effectPs = null;
			PoolManager.Instance.Put(this.m_effect);
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_effect == null)
			{
				return;
			}
			if (!this.m_isPlaying)
			{
				return;
			}
			this.m_duration += deltaTime;
			if (this.m_duration >= this.m_playTime)
			{
				this.m_isPlaying = false;
				this.m_effect.SetActiveSafe(false);
			}
		}

		public async Task Preload()
		{
			ArtSkill_Skill elementById = GameApp.Table.GetManager().GetArtSkill_SkillModelInstance().GetElementById(3);
			await PoolManager.Instance.Cache(elementById.path);
		}

		public void Play()
		{
			this.m_duration = 0f;
			this.m_isPlaying = true;
			this.m_effect.SetActiveSafe(true);
			ParticleSystemExpand.ParticleStop(this.m_effectPs, true, 1);
			ParticleSystemExpand.ParticlePlay(this.m_effectPs, true);
		}

		public void Stop()
		{
			this.m_isPlaying = false;
			this.m_effect.SetActiveSafe(false);
		}

		private CMemberBase m_owner;

		private GameObject m_effect;

		private ParticleSystem m_effectPs;

		private float m_duration;

		private readonly float m_playTime = 3f;

		private bool m_isPlaying;
	}
}
