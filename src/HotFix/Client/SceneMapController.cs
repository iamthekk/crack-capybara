using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.Component;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix.Client
{
	public class SceneMapController
	{
		public GameCameraController gameCameraController { get; private set; }

		public int CurrentTime { get; private set; }

		public bool IsChangeTime { get; private set; }

		public int MapID { get; private set; }

		public async Task OnInit(GameObject gameObject, Transform playerTrans, int mapId)
		{
			this.SelfObj = gameObject;
			this.playerTransform = playerTrans;
			this.MapID = mapId;
			this.CurrentTime = 1;
			ComponentRegister component = gameObject.GetComponent<ComponentRegister>();
			this.skyLayer = component.GetGameObject("SkyLayer");
			this.bgLayer = component.GetGameObject("BgLayer");
			this.farLayer = component.GetGameObject("FarLayer");
			this.middleLayer = component.GetGameObject("MiddleLayer");
			this.nearLayer = component.GetGameObject("NearLayer");
			this.waveLayer = component.GetGameObject("WaveLayer");
			this.cloudLayer = component.GetGameObject("CloudLayer");
			this.farFloatingLayer = component.GetGameObject("FarFloatingLayer");
			this.middleFloatingLayer = component.GetGameObject("MiddleFloatingLayer");
			this.nearFloatingLayer = component.GetGameObject("NearFloatingLayer");
			this.farStaticLayer = component.GetGameObject("FarStaticLayer");
			this.planetCtrl = component.GetGameObject("PlanetLayer").GetComponent<MapPlanetCtrl>();
			this.mSkyMapLayerCtrl = this.CreateLayer(this.skyLayer, SceneMapLayerType.Sky, playerTrans);
			this.mBgMapLayerCtrl = this.CreateLayer(this.bgLayer, SceneMapLayerType.Bg, playerTrans);
			this.mFarMapLayerCtrl = this.CreateLayer(this.farLayer, SceneMapLayerType.Far, playerTrans);
			this.mMiddleMapLayerCtrl = this.CreateLayer(this.middleLayer, SceneMapLayerType.Middle, playerTrans);
			this.mNearMapLayerCtrl = this.CreateLayer(this.nearLayer, SceneMapLayerType.Near, playerTrans);
			this.mWaveMapLayerCtrl = this.CreateLayer(this.waveLayer, SceneMapLayerType.Wave, playerTrans);
			Map_map elementById = GameApp.Table.GetManager().GetMap_mapModelInstance().GetElementById(mapId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("未找到地图 id={0}", mapId));
			}
			else
			{
				this.MemberVColor_Daytime = ((elementById.memberColor.Length != 0) ? elementById.memberColor[0] : 0f);
				this.MemberVColor_Dusk = ((elementById.memberColor.Length > 1) ? elementById.memberColor[1] : 0f);
				this.MemberVColor_Night = ((elementById.memberColor.Length > 2) ? elementById.memberColor[2] : 0f);
				this.SpriteColor_Daytime = ((elementById.spriteColor.Length != 0) ? elementById.spriteColor[0] : 255f);
				this.SpriteColor_Dusk = ((elementById.spriteColor.Length > 1) ? elementById.spriteColor[1] : 255f);
				this.SpriteColor_Night = ((elementById.spriteColor.Length > 2) ? elementById.spriteColor[2] : 255f);
				this.IsChangeTime = elementById.changeTime > 0;
				List<Task> list = new List<Task>();
				if (elementById.floatingRandom.Length >= 4)
				{
					this.cloudCtrl = this.cloudLayer.GetComponent<MapFloatingCloudCtrl>();
					this.farFloatingCtrl = this.farFloatingLayer.GetComponent<MapFloatingCtrl>();
					this.middleFloatingCtrl = this.middleFloatingLayer.GetComponent<MapFloatingCtrl>();
					this.nearFloatingCtrl = this.nearFloatingLayer.GetComponent<MapFloatingCtrl>();
					this.farStaticCtrl = this.farStaticLayer.GetComponent<MapFloatingCtrl>();
					this.floatingCtrls.Add(this.cloudCtrl);
					this.floatingCtrls.Add(this.farFloatingCtrl);
					this.floatingCtrls.Add(this.middleFloatingCtrl);
					this.floatingCtrls.Add(this.nearFloatingCtrl);
					this.floatingCtrls.Add(this.farStaticCtrl);
					list.Add(this.InitFloatingLayers(elementById, playerTrans));
				}
				this.mapWaveCtrl = new MapWaveCtrl();
				list.Add(this.mapWaveCtrl.Init(elementById.normalWaves, elementById.specialWaves));
				this.planetCtrl.Init();
				this.planetCtrl.SetData(elementById.isPlanet, new Vector3(elementById.planetOffset[0], elementById.planetOffset[1], 0f), playerTrans, this);
				GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ChangeWaves, new HandlerEvent(this.OnEventChangeWaves));
				GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ChangeHour, new HandlerEvent(this.OnEventChangeHour));
				await Task.WhenAll(list);
				this.ShowNormalWave();
			}
		}

		public async Task OnDeInit()
		{
			this.CurrentTime = 1;
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ChangeWaves, new HandlerEvent(this.OnEventChangeWaves));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ChangeHour, new HandlerEvent(this.OnEventChangeHour));
			this.mapWaveCtrl.DeInit();
			this.mapWaveCtrl = null;
			for (int i = 0; i < this.layerCtrls.Count; i++)
			{
				this.layerCtrls[i].DeInit();
				this.layerCtrls[i] = null;
			}
			this.layerCtrls.Clear();
			for (int j = 0; j < this.floatingCtrls.Count; j++)
			{
				this.floatingCtrls[j].DeInit();
				this.floatingCtrls[j] = null;
			}
			this.floatingCtrls.Clear();
			await Task.CompletedTask;
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.isPause)
			{
				return;
			}
			if (!this.isMoveActive)
			{
				return;
			}
			for (int i = 0; i < this.layerCtrls.Count; i++)
			{
				MapLayerCtrl mapLayerCtrl = this.layerCtrls[i];
				if (mapLayerCtrl != null)
				{
					mapLayerCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
			for (int j = 0; j < this.floatingCtrls.Count; j++)
			{
				MapFloatingCtrl mapFloatingCtrl = this.floatingCtrls[j];
				if (mapFloatingCtrl != null)
				{
					mapFloatingCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
			MapWaveCtrl mapWaveCtrl = this.mapWaveCtrl;
			if (mapWaveCtrl != null)
			{
				mapWaveCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (this.planetCtrl)
			{
				this.planetCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public void SetController(GameCameraController cameraCtrl)
		{
			this.gameCameraController = cameraCtrl;
		}

		private MapLayerCtrl CreateLayer(GameObject layerObj, SceneMapLayerType layerType, Transform trans)
		{
			MapLayerCtrl component = layerObj.GetComponent<MapLayerCtrl>();
			component.Init();
			component.SetData(layerType, this, trans, this.MapID);
			this.layerCtrls.Add(component);
			return component;
		}

		public void SetMoveActive(bool isActive)
		{
		}

		public void SetPlayerMovePause(bool movePause)
		{
			for (int i = 0; i < this.layerCtrls.Count; i++)
			{
				MapLayerCtrl mapLayerCtrl = this.layerCtrls[i];
				if (mapLayerCtrl != null)
				{
					mapLayerCtrl.SetPlayerMovePause(movePause);
				}
			}
			for (int j = 0; j < this.floatingCtrls.Count; j++)
			{
				MapFloatingCtrl mapFloatingCtrl = this.floatingCtrls[j];
				if (mapFloatingCtrl != null)
				{
					mapFloatingCtrl.SetPlayerMovePause(movePause);
				}
			}
		}

		public void OnPause(bool pause)
		{
			this.isPause = pause;
			for (int i = 0; i < this.layerCtrls.Count; i++)
			{
				MapLayerCtrl mapLayerCtrl = this.layerCtrls[i];
				if (mapLayerCtrl != null)
				{
					mapLayerCtrl.SetPause(pause);
				}
			}
			for (int j = 0; j < this.floatingCtrls.Count; j++)
			{
				MapFloatingCtrl mapFloatingCtrl = this.floatingCtrls[j];
				if (mapFloatingCtrl != null)
				{
					mapFloatingCtrl.SetPause(pause);
				}
			}
		}

		public async Task RefreshSceneMap(int mapId)
		{
			Map_map elementById = GameApp.Table.GetManager().GetMap_mapModelInstance().GetElementById(mapId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("Table [Map] not found id = {0}", mapId));
			}
			else
			{
				List<Task> list = new List<Task>();
				list.Add(this.mSkyMapLayerCtrl.RefreshMap(elementById.skyIDs, elementById.skyOffsetY, this.playerTransform));
				list.Add(this.mMiddleMapLayerCtrl.RefreshMap(elementById.middleIDs, elementById.middleOffsetY, this.playerTransform));
				list.Add(this.mBgMapLayerCtrl.RefreshMap(elementById.bgIds, elementById.bgOffsetY, this.playerTransform));
				list.Add(this.mFarMapLayerCtrl.RefreshMap(elementById.farIDs, elementById.farOffsetY, this.playerTransform));
				list.Add(this.mNearMapLayerCtrl.RefreshMap(elementById.nearIDs, elementById.nearOffsetY, this.playerTransform));
				this.cloudCtrl.DeInit();
				this.farFloatingCtrl.DeInit();
				this.middleFloatingCtrl.DeInit();
				this.nearFloatingCtrl.DeInit();
				this.farStaticCtrl.DeInit();
				list.Add(this.InitFloatingLayers(elementById, this.playerTransform));
				await Task.WhenAll(list);
				if (GameApp.View.IsOpened(ViewName.LoadingMapViewModule))
				{
					GameApp.View.GetViewModule(ViewName.LoadingMapViewModule).PlayHide(delegate
					{
						GameApp.View.CloseView(ViewName.LoadingMapViewModule, null);
						EventArgChangeMapPause eventArgChangeMapPause = new EventArgChangeMapPause();
						eventArgChangeMapPause.SetData(false, mapId);
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_NewScene_Pause, eventArgChangeMapPause);
					});
				}
			}
		}

		public void StartMove()
		{
			this.isMoveActive = true;
		}

		public void StopMove()
		{
			this.isMoveActive = false;
		}

		private async Task InitFloatingLayers(Map_map map, Transform playerTrans)
		{
			for (int i = 0; i < this.floatingCtrls.Count; i++)
			{
				this.floatingCtrls[i].Init();
			}
			await Task.WhenAll(new List<Task>
			{
				this.cloudCtrl.SetData(SceneMapLayerType.Cloud, map.clouds, map.cloudOffsetY, map.cloudSpeed, map.floatingRandom[0], playerTrans, this),
				this.farFloatingCtrl.SetData(SceneMapLayerType.FarFloating, map.farFloating, map.farFloatingOffsetY, map.farFloatingSpeed, map.floatingRandom[1], playerTrans, this),
				this.middleFloatingCtrl.SetData(SceneMapLayerType.MiddleFloating, map.middleFloating, map.middleFloatingOffsetY, null, map.floatingRandom[2], playerTrans, this),
				this.nearFloatingCtrl.SetData(SceneMapLayerType.NearFloating, map.nearFloating, map.nearFloatingOffsetY, map.nearFloatingSpeed, map.floatingRandom[3], playerTrans, this),
				this.farStaticCtrl.SetData(SceneMapLayerType.FarStatic, map.farStatic, map.farStaticOffsetY, null, map.floatingRandom[4], playerTrans, this)
			});
		}

		private void ShowNormalWave()
		{
			this.mapWaveCtrl.ClearAll();
			foreach (GameObject gameObject in this.mWaveMapLayerCtrl.daytimeLayer.items)
			{
				this.mapWaveCtrl.CreateNormalWaves(gameObject.transform);
			}
		}

		private void ShowSpecialWave()
		{
			this.mapWaveCtrl.ClearAll();
			foreach (GameObject gameObject in this.mWaveMapLayerCtrl.daytimeLayer.items)
			{
				this.mapWaveCtrl.CreateSpecialWaves(gameObject.transform);
			}
		}

		private void OnEventChangeWaves(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsLong eventArgsLong = eventArgs as EventArgsLong;
			if (eventArgsLong != null)
			{
				if (eventArgsLong.Value == 2L)
				{
					this.ShowSpecialWave();
					return;
				}
				this.ShowNormalWave();
			}
		}

		private void OnEventChangeHour(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.IsChangeTime)
			{
				this.RandomTime();
			}
		}

		public void SetFastSpeed()
		{
			for (int i = 0; i < this.layerCtrls.Count; i++)
			{
				MapLayerCtrl mapLayerCtrl = this.layerCtrls[i];
				if (mapLayerCtrl != null)
				{
					mapLayerCtrl.SetFastSpeed();
				}
			}
			for (int j = 0; j < this.floatingCtrls.Count; j++)
			{
				MapFloatingCtrl mapFloatingCtrl = this.floatingCtrls[j];
				if (mapFloatingCtrl != null)
				{
					mapFloatingCtrl.SetFastSpeed();
				}
			}
		}

		public void SetNormalSpeed()
		{
			for (int i = 0; i < this.layerCtrls.Count; i++)
			{
				MapLayerCtrl mapLayerCtrl = this.layerCtrls[i];
				if (mapLayerCtrl != null)
				{
					mapLayerCtrl.SetNormalSpeed();
				}
			}
			for (int j = 0; j < this.floatingCtrls.Count; j++)
			{
				MapFloatingCtrl mapFloatingCtrl = this.floatingCtrls[j];
				if (mapFloatingCtrl != null)
				{
					mapFloatingCtrl.SetNormalSpeed();
				}
			}
		}

		public void SetHangUpSpeed()
		{
			for (int i = 0; i < this.layerCtrls.Count; i++)
			{
				MapLayerCtrl mapLayerCtrl = this.layerCtrls[i];
				if (mapLayerCtrl != null)
				{
					mapLayerCtrl.SetHangUpSpeed();
				}
			}
			for (int j = 0; j < this.floatingCtrls.Count; j++)
			{
				MapFloatingCtrl mapFloatingCtrl = this.floatingCtrls[j];
				if (mapFloatingCtrl != null)
				{
					mapFloatingCtrl.SetHangUpSpeed();
				}
			}
		}

		public void RandomTime()
		{
			if (this.isTimeAni)
			{
				return;
			}
			this.isTimeAni = true;
			float num = 2f;
			int num2 = Utility.Math.Random(0, 3);
			this.CurrentTime = num2;
			for (int i = 0; i < this.layerCtrls.Count; i++)
			{
				this.layerCtrls[i].SetTime(num2, num);
			}
			if (this.cloudCtrl != null)
			{
				this.cloudCtrl.SetTime(num2, num);
			}
			if (this.planetCtrl != null)
			{
				this.planetCtrl.SetTime(num2, num);
			}
			if (this.mapWaveCtrl != null)
			{
				this.mapWaveCtrl.SetTime(this.GetVColor(num2), num);
			}
			if (EventMemberController.Instance != null)
			{
				EventMemberController.Instance.SetTime(num2, num);
			}
			for (int j = 0; j < this.floatingCtrls.Count; j++)
			{
				MapFloatingCtrl mapFloatingCtrl = this.floatingCtrls[j];
				if (!(mapFloatingCtrl is MapFloatingCloudCtrl))
				{
					mapFloatingCtrl.SetTime(this.GetVColor(num2), num);
				}
			}
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.AppendInterval(sequence, num);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.isTimeAni = false;
			});
		}

		public float GetVColor(int time)
		{
			float num = this.MemberVColor_Daytime;
			if (time == 1)
			{
				num = this.MemberVColor_Dusk;
			}
			else if (time == 2)
			{
				num = this.MemberVColor_Night;
			}
			return num;
		}

		public float GetSpriteColor(int time)
		{
			float num = this.SpriteColor_Daytime;
			if (time == 1)
			{
				num = this.SpriteColor_Dusk;
			}
			else if (time == 2)
			{
				num = this.SpriteColor_Night;
			}
			return num;
		}

		public void ResetWordPosition()
		{
			for (int i = 0; i < this.layerCtrls.Count; i++)
			{
				this.layerCtrls[i].ResetWordPosition();
			}
			for (int j = 0; j < this.floatingCtrls.Count; j++)
			{
				this.floatingCtrls[j].ResetWordPosition();
			}
		}

		public GameObject SelfObj;

		private GameObject skyLayer;

		private GameObject bgLayer;

		private GameObject farLayer;

		private GameObject middleLayer;

		private GameObject nearLayer;

		private GameObject waveLayer;

		private GameObject cloudLayer;

		private GameObject farFloatingLayer;

		private GameObject middleFloatingLayer;

		private GameObject nearFloatingLayer;

		private GameObject farStaticLayer;

		private MapLayerCtrl mSkyMapLayerCtrl;

		private MapLayerCtrl mBgMapLayerCtrl;

		private MapLayerCtrl mFarMapLayerCtrl;

		private MapLayerCtrl mMiddleMapLayerCtrl;

		private MapLayerCtrl mNearMapLayerCtrl;

		private MapLayerCtrl mWaveMapLayerCtrl;

		private MapWaveCtrl mapWaveCtrl;

		private MapFloatingCloudCtrl cloudCtrl;

		private MapFloatingCtrl farFloatingCtrl;

		private MapFloatingCtrl middleFloatingCtrl;

		private MapFloatingCtrl nearFloatingCtrl;

		private MapFloatingCtrl farStaticCtrl;

		private MapPlanetCtrl planetCtrl;

		private bool isMoveActive;

		private bool isPause;

		private Transform playerTransform;

		private List<MapLayerCtrl> layerCtrls = new List<MapLayerCtrl>();

		private List<MapFloatingCtrl> floatingCtrls = new List<MapFloatingCtrl>();

		private float MemberVColor_Daytime;

		private float MemberVColor_Dusk;

		private float MemberVColor_Night;

		private float SpriteColor_Daytime;

		private float SpriteColor_Dusk;

		private float SpriteColor_Night;

		public const int Normal_Waves_Group = 1;

		public const int Special_Waves_Group = 2;

		public const int DAYTIME = 0;

		public const int DUSK = 1;

		public const int NIGHT = 2;

		public const float ChangeTimeDuration = 2f;

		private bool isTimeAni;
	}
}
