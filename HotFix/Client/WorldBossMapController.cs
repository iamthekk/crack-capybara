using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix.Client
{
	public class WorldBossMapController
	{
		public async Task OnInit(GameObject gameObject)
		{
			ComponentRegister component = gameObject.GetComponent<ComponentRegister>();
			this.mapParent = component.GetGameObject("Parent");
			this.worldBossDataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
			TaskOutValue<GameObject> outPrefab = new TaskOutValue<GameObject>();
			await this.LoadMap(this.worldBossDataModule.ChapterId, outPrefab);
			if (outPrefab.Value != null)
			{
				Object.Instantiate<GameObject>(outPrefab.Value).SetParentNormal(this.mapParent, false);
			}
		}

		public async Task OnDeInit()
		{
			await Task.CompletedTask;
		}

		private Task LoadMap(int worldBossId, TaskOutValue<GameObject> outPrefab)
		{
			WorldBossMapController.<LoadMap>d__4 <LoadMap>d__;
			<LoadMap>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadMap>d__.worldBossId = worldBossId;
			<LoadMap>d__.outPrefab = outPrefab;
			<LoadMap>d__.<>1__state = -1;
			<LoadMap>d__.<>t__builder.Start<WorldBossMapController.<LoadMap>d__4>(ref <LoadMap>d__);
			return <LoadMap>d__.<>t__builder.Task;
		}

		private GameObject mapParent;

		private WorldBossDataModule worldBossDataModule;
	}
}
