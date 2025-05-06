using System;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using HotFix.Client;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class MapLayerCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.normalSpeedOffset = 0f;
			this.fastSpeedOffset = 0f;
			this.hangupSpeedOffset = 0f;
		}

		protected override void OnDeInit()
		{
			this.daytimeLayer.DeInit();
			if (this.duskLayer)
			{
				this.duskLayer.DeInit();
			}
			if (this.nightLayer)
			{
				this.nightLayer.DeInit();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.isPause)
			{
				return;
			}
			this.daytimeLayer.OnUpdate(deltaTime, unscaledDeltaTime);
			if (this.duskLayer)
			{
				this.duskLayer.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (this.nightLayer)
			{
				this.nightLayer.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (!this.isPlayerMovePause && (this.layerType == SceneMapLayerType.Far || this.layerType == SceneMapLayerType.Bg))
			{
				if (this.isHangUpSpeed)
				{
					this.moveSpeed = this.GetHangUpSpeed();
				}
				else
				{
					this.moveSpeed = (this.isFastSpeed ? this.GetFastSpeed() : this.GetNormalSpeed());
				}
				Vector3 vector = Vector3.right * this.moveSpeed * deltaTime;
				base.gameObject.transform.position += vector;
			}
		}

		public async Task SetData(SceneMapLayerType type, SceneMapController mapController, Transform playerTrans, int mapId)
		{
			this.layerType = type;
			this.sceneMapController = mapController;
			this.daytimeLayer.Init();
			Map_map elementById = GameApp.Table.GetManager().GetMap_mapModelInstance().GetElementById(mapId);
			switch (this.layerType)
			{
			case SceneMapLayerType.Sky:
				this.SetOffset(elementById.skyOffsetY);
				await this.Load(elementById.skyIDs, playerTrans);
				break;
			case SceneMapLayerType.Bg:
				this.SetOffset(elementById.bgOffsetY);
				if (elementById.bgSpeed.Length >= 3)
				{
					this.normalSpeedOffset = elementById.bgSpeed[0];
					this.fastSpeedOffset = elementById.bgSpeed[1];
					this.hangupSpeedOffset = elementById.bgSpeed[2];
				}
				if (elementById.bgIds.Length != 0)
				{
					base.gameObject.SetActiveSafe(true);
					await this.Load(elementById.bgIds, playerTrans);
				}
				else
				{
					base.gameObject.SetActiveSafe(false);
				}
				break;
			case SceneMapLayerType.Far:
				this.SetOffset(elementById.farOffsetY);
				if (elementById.farSpeed.Length >= 3)
				{
					this.normalSpeedOffset = elementById.farSpeed[0];
					this.fastSpeedOffset = elementById.farSpeed[1];
					this.hangupSpeedOffset = elementById.farSpeed[2];
				}
				await this.Load(elementById.farIDs, playerTrans);
				break;
			case SceneMapLayerType.Middle:
				this.SetOffset(elementById.middleOffsetY);
				await this.Load(elementById.middleIDs, playerTrans);
				break;
			case SceneMapLayerType.Near:
				this.SetOffset(elementById.nearOffsetY);
				await this.Load(elementById.nearIDs, playerTrans);
				break;
			case SceneMapLayerType.Wave:
				this.SetOffset(elementById.waveOffsetY);
				if (this.daytimeLayer)
				{
					this.daytimeLayer.SetData(playerTrans);
				}
				if (this.duskLayer)
				{
					this.duskLayer.SetData(playerTrans);
				}
				if (this.nightLayer)
				{
					this.nightLayer.SetData(playerTrans);
				}
				break;
			}
			this.currentTime = 0;
		}

		public void SetPause(bool isPause)
		{
			this.isPause = isPause;
		}

		private async Task Load(string[] ids, Transform playerTrans)
		{
			if (ids.Length != 0)
			{
				await this.daytimeLayer.SetData(ids[0], 1, playerTrans);
			}
			if (ids.Length > 1 && !string.IsNullOrEmpty(ids[1]) && !ids[1].Equals("0"))
			{
				this.duskLayer = this.CreateOtherLayer(this.duskLayer, "Dusk");
				if (this.duskLayer != null)
				{
					await this.duskLayer.SetData(ids[1], 0, playerTrans);
				}
			}
			if (ids.Length > 2 && !string.IsNullOrEmpty(ids[2]) && !ids[2].Equals("0"))
			{
				this.nightLayer = this.CreateOtherLayer(this.nightLayer, "Night");
				if (this.nightLayer != null)
				{
					await this.nightLayer.SetData(ids[2], 0, playerTrans);
				}
			}
		}

		private void SetOffset(float offset)
		{
			base.gameObject.transform.localPosition = new Vector3(base.gameObject.transform.localPosition.x, offset);
		}

		public async Task RefreshMap(string[] maps, float offset, Transform playerTrans)
		{
			await this.Load(maps, playerTrans);
			this.SetOffset(offset);
		}

		private float GetPlayerSpeed()
		{
			float num = 0f;
			if (EventMemberController.Instance != null)
			{
				num = EventMemberController.Instance.GetPlayerMoveSpeed();
			}
			return num;
		}

		protected float GetCameraSpeed()
		{
			float num = 0f;
			if (this.sceneMapController != null && this.sceneMapController.gameCameraController != null)
			{
				num = this.sceneMapController.gameCameraController.GetMoveSpeed();
			}
			return num;
		}

		public void SetPlayerMovePause(bool movePause)
		{
			this.isPlayerMovePause = movePause;
		}

		private MapLayerItem CreateOtherLayer(MapLayerItem layer, string layerName)
		{
			return this.CreateLayer(layerName);
		}

		private MapLayerItem CreateLayer(string objName)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.daytimeLayer.gameObject);
			gameObject.name = objName;
			gameObject.SetParentNormal(base.gameObject, false);
			MapLayerItem component = gameObject.GetComponent<MapLayerItem>();
			component.Init();
			return component;
		}

		public void SetTime(int time, float duration)
		{
			if (this.isAni)
			{
				return;
			}
			if (this.currentTime != time)
			{
				MapLayerItem layerItem = this.GetLayerItem(this.currentTime);
				MapLayerItem layerItem2 = this.GetLayerItem(time);
				if (layerItem != null && layerItem2 != null)
				{
					this.isAni = true;
					Sequence sequence = DOTween.Sequence();
					layerItem.DoFade(0f, duration);
					layerItem2.DoFade(1f, duration);
					TweenSettingsExtensions.AppendInterval(sequence, duration);
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						this.currentTime = time;
						this.isAni = false;
					});
				}
			}
		}

		private MapLayerItem GetLayerItem(int time)
		{
			if (time == 0)
			{
				return this.daytimeLayer;
			}
			if (time == 1)
			{
				return this.duskLayer;
			}
			if (time == 2)
			{
				return this.nightLayer;
			}
			return null;
		}

		public void SetFastSpeed()
		{
			this.isFastSpeed = true;
			this.isHangUpSpeed = false;
		}

		public void SetNormalSpeed()
		{
			this.isFastSpeed = false;
			this.isHangUpSpeed = false;
		}

		public void SetHangUpSpeed()
		{
			this.isHangUpSpeed = true;
		}

		private float GetNormalSpeed()
		{
			float num = 0f;
			SceneMapLayerType sceneMapLayerType = this.layerType;
			if (sceneMapLayerType - SceneMapLayerType.Bg <= 1)
			{
				num = (this.isPlayerMovePause ? 0f : (this.GetCameraSpeed() + this.normalSpeedOffset));
			}
			return num;
		}

		private float GetFastSpeed()
		{
			float num = 0f;
			SceneMapLayerType sceneMapLayerType = this.layerType;
			if (sceneMapLayerType - SceneMapLayerType.Bg <= 1)
			{
				num = (this.isPlayerMovePause ? 0f : (this.GetCameraSpeed() + this.fastSpeedOffset));
			}
			return num;
		}

		private float GetHangUpSpeed()
		{
			return this.hangupSpeedOffset;
		}

		public void ResetWordPosition()
		{
			if (this.daytimeLayer != null)
			{
				this.daytimeLayer.ResetWordPosition();
			}
			if (this.duskLayer != null)
			{
				this.duskLayer.ResetWordPosition();
			}
			if (this.nightLayer != null)
			{
				this.nightLayer.ResetWordPosition();
			}
		}

		public MapLayerItem daytimeLayer;

		private SceneMapLayerType layerType;

		private bool isPause;

		private float moveSpeed = 1f;

		private bool isPlayerMovePause;

		private MapLayerItem duskLayer;

		private MapLayerItem nightLayer;

		private SceneMapController sceneMapController;

		private int currentTime;

		private bool isAni;

		private bool isFastSpeed;

		private bool isHangUpSpeed;

		[SerializeField]
		private float normalSpeedOffset;

		[SerializeField]
		private float fastSpeedOffset;

		[SerializeField]
		private float hangupSpeedOffset;
	}
}
