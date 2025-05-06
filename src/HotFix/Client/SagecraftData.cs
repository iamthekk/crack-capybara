using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotFix.Client
{
	public class SagecraftData
	{
		public void Init(GameObject go)
		{
			if (go != null)
			{
				this.goEffect = go;
				Transform transform = go.transform.FindChildByName("Front");
				if (transform != null)
				{
					this.frontPsDatas = new List<ParticleSystemData>();
					ParticleSystem[] componentsInChildren = transform.GetComponentsInChildren<ParticleSystem>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						ParticleSystemData particleSystemData = new ParticleSystemData();
						particleSystemData.Init(componentsInChildren[i]);
						this.frontPsDatas.Add(particleSystemData);
					}
				}
				Transform transform2 = go.transform.FindChildByName("Back");
				if (transform2 != null)
				{
					this.backPsDatas = new List<ParticleSystemData>();
					ParticleSystem[] componentsInChildren2 = transform2.GetComponentsInChildren<ParticleSystem>();
					for (int j = 0; j < componentsInChildren2.Length; j++)
					{
						ParticleSystemData particleSystemData2 = new ParticleSystemData();
						particleSystemData2.Init(componentsInChildren2[j]);
						this.backPsDatas.Add(particleSystemData2);
					}
				}
			}
		}

		public void SetCount(int count)
		{
			this.curCount = count;
		}

		public void AddCount()
		{
			this.curCount++;
		}

		public void SubtractCount()
		{
			this.curCount--;
		}

		public void SetOrderLayer(int addValue)
		{
			if (this.frontPsDatas != null)
			{
				for (int i = 0; i < this.frontPsDatas.Count; i++)
				{
					this.frontPsDatas[i].SetOrderLayer(addValue + 2);
				}
			}
			if (this.backPsDatas != null)
			{
				for (int j = 0; j < this.backPsDatas.Count; j++)
				{
					this.backPsDatas[j].SetOrderLayer(addValue - 2);
				}
			}
		}

		public void DeInit()
		{
			this.goEffect = null;
		}

		public int curCount;

		public GameObject goEffect;

		private List<ParticleSystemData> frontPsDatas;

		private List<ParticleSystemData> backPsDatas;
	}
}
