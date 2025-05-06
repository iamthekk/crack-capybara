using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix
{
	public class ParticleSystemLife : MonoBehaviour
	{
		private void Awake()
		{
			if (this.m_list == null)
			{
				return;
			}
			this.m_timeList = new List<float>();
			for (int i = 0; i < this.m_list.Count; i++)
			{
				ParticleSystem particleSystem = this.m_list[i];
				if (particleSystem == null)
				{
					this.m_timeList.Add(0f);
				}
				else
				{
					this.m_timeList.Add(particleSystem.main.duration);
				}
			}
		}

		public void SetLifeTime(float time)
		{
			if (this.m_list == null)
			{
				return;
			}
			if (this.m_timeList.Count != this.m_list.Count)
			{
				return;
			}
			for (int i = 0; i < this.m_list.Count; i++)
			{
				ParticleSystem particleSystem = this.m_list[i];
				if (!(particleSystem == null))
				{
					ParticleSystem.MainModule main = particleSystem.main;
					float num = this.m_startTime - this.m_timeList[i];
					particleSystem.Stop();
					main.duration = time - num;
					particleSystem.Play();
				}
			}
		}

		[Header("特效默认时间")]
		public float m_startTime = 1f;

		[Header("特效列表")]
		public List<ParticleSystem> m_list;

		private List<float> m_timeList;
	}
}
