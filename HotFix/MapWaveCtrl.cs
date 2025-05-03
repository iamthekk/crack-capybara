using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.Logic;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class MapWaveCtrl
	{
		public async Task Init(int[] normalWaves, int[] specialWaves)
		{
			if (normalWaves != null && normalWaves.Length != 0)
			{
				await this.LoadPrefab(normalWaves, this.normalWavePrefabs);
			}
			if (specialWaves != null && specialWaves.Length != 0)
			{
				await this.LoadPrefab(specialWaves, this.specialWavePrefabs);
			}
		}

		public void DeInit()
		{
			for (int i = 0; i < this.waveItems.Count; i++)
			{
				this.waveItems[i].DeInit();
				Object.Destroy(this.waveItems[i].gameObject);
			}
			this.waveItems.Clear();
			this.normalWavePrefabs.Clear();
			this.specialWavePrefabs.Clear();
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			for (int i = 0; i < this.waveItems.Count; i++)
			{
				this.waveItems[i].OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		private async Task LoadPrefab(int[] ids, List<GameObject> list)
		{
			if (ids != null)
			{
				foreach (int num in ids)
				{
					if (num != 0)
					{
						ArtMap_Map elementById = GameApp.Table.GetManager().GetArtMap_MapModelInstance().GetElementById(num);
						if (elementById != null && !string.IsNullOrEmpty(elementById.path))
						{
							AsyncOperationHandle<GameObject> prefab = GameApp.Resources.LoadAssetAsync<GameObject>(elementById.path);
							await prefab.Task;
							if (prefab.Result != null)
							{
								list.Add(prefab.Result);
							}
							prefab = default(AsyncOperationHandle<GameObject>);
						}
						else
						{
							HLog.LogError(string.Format("[ArtMap_Map] not found id or path, id={0}", num));
						}
					}
				}
			}
		}

		public void CreateNormalWaves(Transform parent)
		{
			this.CreateWave(parent, this.normalWavePrefabs);
		}

		public void CreateSpecialWaves(Transform parent)
		{
			this.CreateWave(parent, this.specialWavePrefabs);
		}

		private void CreateWave(Transform parent, List<GameObject> prefabs)
		{
			if (prefabs == null || prefabs.Count == 0)
			{
				return;
			}
			for (int i = 0; i < 20; i++)
			{
				int num = Utility.Math.Random(0, prefabs.Count);
				GameObject gameObject = Object.Instantiate<GameObject>(prefabs[num]);
				gameObject.SetParentNormal(parent, false);
				gameObject.SetActiveSafe(true);
				MapWaveItem component = gameObject.GetComponent<MapWaveItem>();
				component.Init();
				this.waveItems.Add(component);
				int num2 = (int)(Utility.Math.Random(0f, 3f) * 1000f);
				component.ShowWave(num2);
			}
		}

		public void ClearAll()
		{
			for (int i = 0; i < this.waveItems.Count; i++)
			{
				this.waveItems[i].SetWaitDestroy();
			}
			this.waveItems.Clear();
		}

		public void SetTime(float to, float duration)
		{
			for (int i = 0; i < this.waveItems.Count; i++)
			{
				this.waveItems[i].SetTime(to, duration);
			}
		}

		public const int WaveCount = 20;

		public const float OffsetX = 10.8f;

		public const float OffsetY = 4.6f;

		private List<GameObject> normalWavePrefabs = new List<GameObject>();

		private List<GameObject> specialWavePrefabs = new List<GameObject>();

		private List<MapWaveItem> waveItems = new List<MapWaveItem>();
	}
}
