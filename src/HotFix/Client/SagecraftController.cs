using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix.Client
{
	public class SagecraftController
	{
		public void Init(ComponentRegister register)
		{
			this.effectRoot = register.transform;
			for (int i = 1; i < 21; i++)
			{
				WeaponType weaponType = (WeaponType)i;
				MeshRenderer component = register.GetGameObject(weaponType.ToString()).GetComponent<MeshRenderer>();
				if (component != null)
				{
					this.weaponMeshs[weaponType] = component;
				}
			}
			for (int j = 1; j < 5; j++)
			{
				this.Cache((SagecraftType)j);
			}
		}

		public void DeInit()
		{
			this.DestroyAll();
			this.weaponMeshs.Clear();
			this.weaponMeshs = null;
		}

		private async Task Cache(SagecraftType type)
		{
			ArtSagecraft_Sagecraft elementById = GameApp.Table.GetManager().GetArtSagecraft_SagecraftModelInstance().GetElementById((int)type);
			if (elementById == null)
			{
				HLog.LogError(HLog.ToColor(string.Format("GameArt_Sagecraft is erorr  type = {0}", type), 3));
			}
			else
			{
				await PoolManager.Instance.CheckPrefab(elementById.path);
			}
		}

		public void SetSagecraftTpye(WeaponType type)
		{
			this.curSagecraftType = type;
		}

		public async Task ShowSagecraft(SagecraftType type)
		{
			if (!this.SagecraftDatas.ContainsKey(type))
			{
				ArtSagecraft_Sagecraft artTable = GameApp.Table.GetManager().GetArtSagecraft_SagecraftModelInstance().GetElementById((int)type);
				if (artTable == null)
				{
					HLog.LogError(HLog.ToColor(string.Format("GameArt_Sagecraft is erorr  type = {0}", type), 3));
				}
				else
				{
					SagecraftData data = new SagecraftData();
					data.SetCount(1);
					this.SagecraftDatas[type] = data;
					await PoolManager.Instance.CheckPrefab(artTable.path);
					GameObject gameObject = PoolManager.Instance.Out(artTable.path, Vector3.zero, Quaternion.identity, this.effectRoot);
					ParticleSystem[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						ParticleSystem.ShapeModule shape = componentsInChildren[i].shape;
						if (shape.shapeType == 13)
						{
							shape.meshRenderer = this.weaponMeshs[this.curSagecraftType];
						}
					}
					data.Init(gameObject);
					artTable = null;
					data = null;
				}
			}
			else
			{
				this.SagecraftDatas[type].AddCount();
			}
		}

		public void SetOrderLayer(int addValue)
		{
			foreach (SagecraftData sagecraftData in this.SagecraftDatas.Values)
			{
				sagecraftData.SetOrderLayer(addValue);
			}
		}

		public void Destroy(SagecraftType type)
		{
			SagecraftData sagecraftData;
			this.SagecraftDatas.TryGetValue(type, out sagecraftData);
			if (sagecraftData != null)
			{
				sagecraftData.SubtractCount();
				if (sagecraftData.curCount < 1)
				{
					PoolManager.Instance.Put(sagecraftData.goEffect);
					sagecraftData.DeInit();
					this.SagecraftDatas.Remove(type);
				}
			}
		}

		public void DestroyAll()
		{
			List<SagecraftData> list = this.SagecraftDatas.Values.ToList<SagecraftData>();
			for (int i = 0; i < list.Count; i++)
			{
				SagecraftData sagecraftData = list[i];
				if (sagecraftData != null)
				{
					PoolManager.Instance.Put(sagecraftData.goEffect);
					sagecraftData.DeInit();
				}
			}
			this.SagecraftDatas.Clear();
		}

		private Transform effectRoot;

		private Dictionary<WeaponType, MeshRenderer> weaponMeshs = new Dictionary<WeaponType, MeshRenderer>();

		private Dictionary<SagecraftType, SagecraftData> SagecraftDatas = new Dictionary<SagecraftType, SagecraftData>();

		private WeaponType curSagecraftType = WeaponType.LiuLangChangJian;
	}
}
