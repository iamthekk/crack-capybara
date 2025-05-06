using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix.Client
{
	public class TowerSceneMapController
	{
		public async Task OnInit(GameObject gameObject)
		{
			ComponentRegister component = gameObject.GetComponent<ComponentRegister>();
			this.currentMap = component.GetGameObject("CurrentMap").GetComponent<SpriteRenderer>();
			this.nextMap = component.GetGameObject("NextMap").GetComponent<SpriteRenderer>();
			this.towerDataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
			int fightLevelId = this.towerDataModule.FightLevelId;
			int nextId = this.towerDataModule.NextFightLevelId;
			TaskOutValue<Sprite> outCurrentMap = new TaskOutValue<Sprite>();
			await this.LoadMap(fightLevelId, outCurrentMap);
			if (outCurrentMap.Value != null)
			{
				this.currentMap.sprite = outCurrentMap.Value;
			}
			if (nextId > 0)
			{
				TaskOutValue<Sprite> outNextMap = new TaskOutValue<Sprite>();
				await this.LoadMap(nextId, outNextMap);
				if (outNextMap.Value != null)
				{
					this.nextMap.sprite = outNextMap.Value;
				}
				else
				{
					this.nextMap.sprite = this.currentMap.sprite;
				}
				outNextMap = null;
			}
			else
			{
				this.nextMap.sprite = this.currentMap.sprite;
			}
		}

		public async Task OnDeInit()
		{
			await Task.CompletedTask;
		}

		private Task LoadMap(int levelId, TaskOutValue<Sprite> outSprite)
		{
			TowerSceneMapController.<LoadMap>d__5 <LoadMap>d__;
			<LoadMap>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadMap>d__.levelId = levelId;
			<LoadMap>d__.outSprite = outSprite;
			<LoadMap>d__.<>1__state = -1;
			<LoadMap>d__.<>t__builder.Start<TowerSceneMapController.<LoadMap>d__5>(ref <LoadMap>d__);
			return <LoadMap>d__.<>t__builder.Task;
		}

		private SpriteRenderer currentMap;

		private SpriteRenderer nextMap;

		private TowerDataModule towerDataModule;
	}
}
