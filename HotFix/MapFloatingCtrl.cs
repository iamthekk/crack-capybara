using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using HotFix.Client;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class MapFloatingCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
			this.ClearAll();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (base.gameObject == null)
			{
				return;
			}
			if (this.isPause)
			{
				return;
			}
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
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].transform.position.x < this.playerTransform.transform.position.x && Utility.Math.Abs(this.playerTransform.transform.position.x - this.items[i].transform.position.x) > this.recycleDis)
				{
					this.ResetIdleItems(this.items[i]);
				}
			}
			for (int j = 0; j < this.floatingItems.Count; j++)
			{
				this.floatingItems[j].OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public async Task SetData(SceneMapLayerType type, int[] ids, float offset, float[] speedArr, int randomId, Transform trans, SceneMapController mapController)
		{
			this.sceneMapController = mapController;
			this.layerType = type;
			this.playerTransform = trans;
			if (ids != null)
			{
				this.recycleDis = 25f;
				Map_floatingRandom elementById = GameApp.Table.GetManager().GetMap_floatingRandomModelInstance().GetElementById(randomId);
				if (elementById != null)
				{
					this.itemCount = elementById.count;
					this.minOffsetX = elementById.minOffsetX;
					this.maxOffsetX = elementById.maxOffsetX;
					this.minOffsetY = elementById.minOffsetY;
					this.maxOffsetY = elementById.maxOffsetY;
					if (speedArr != null && speedArr.Length >= 3)
					{
						this.normalSpeedOffset = speedArr[0];
						this.fastSpeedOffset = speedArr[1];
						this.hangupSpeedOffset = speedArr[2];
					}
					else
					{
						this.normalSpeedOffset = 0f;
						this.fastSpeedOffset = 0f;
						this.hangupSpeedOffset = 0f;
					}
					if (this.maxOffsetX > this.recycleDis)
					{
						this.recycleDis = this.maxOffsetX;
					}
					try
					{
						for (int i = 0; i < ids.Length; i++)
						{
							if (ids[i] == 0)
							{
								break;
							}
							ArtMap_Map elementById2 = GameApp.Table.GetManager().GetArtMap_MapModelInstance().GetElementById(ids[i]);
							if (elementById2 != null && !string.IsNullOrEmpty(elementById2.path))
							{
								AsyncOperationHandle<GameObject> prefab = GameApp.Resources.LoadAssetAsync<GameObject>(elementById2.path);
								await prefab.Task;
								if (prefab.Result != null)
								{
									this.prefabs.Add(prefab.Result);
								}
								prefab = default(AsyncOperationHandle<GameObject>);
							}
							else
							{
								HLog.LogError(string.Format("[ArtMap_Map] not found id or path, id={0}", ids[i]));
							}
						}
					}
					catch (Exception ex)
					{
						HLog.LogException(ex);
					}
					finally
					{
						base.gameObject.transform.localPosition = new Vector3(base.gameObject.transform.localPosition.x, offset);
						this.lastX = -this.minOffsetX;
						this.CreateItems();
						this.SetNormalSpeed();
					}
				}
			}
		}

		public void SetPause(bool isPause)
		{
			this.isPause = isPause;
		}

		protected virtual void CreateItems()
		{
			if (this.prefabs == null || this.prefabs.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.itemCount; i++)
			{
				int num = Utility.Math.Random(0, this.prefabs.Count);
				GameObject gameObject = Object.Instantiate<GameObject>(this.prefabs[num]);
				gameObject.SetParentNormal(base.gameObject, false);
				this.ResetIdleItems(gameObject);
				this.items.Add(gameObject);
				MapFloatingItem component = gameObject.GetComponent<MapFloatingItem>();
				if (component)
				{
					this.floatingItems.Add(component);
					component.Init();
				}
			}
		}

		protected void ResetIdleItems(GameObject item)
		{
			float num = Utility.Math.Random(this.minOffsetX, this.maxOffsetX);
			float num2 = Utility.Math.Random(this.minOffsetY, this.maxOffsetY);
			if (this.layerType == SceneMapLayerType.FarStatic)
			{
				item.transform.localPosition = new Vector3(num, num2, 0f);
			}
			else
			{
				item.transform.localPosition = new Vector3(this.lastX + num, num2, 0f);
				this.lastX = item.transform.localPosition.x;
			}
			if (this.layerType == SceneMapLayerType.MiddleFloating)
			{
				float num3 = this.maxOffsetY - this.minOffsetY;
				if (num3 > 0f)
				{
					float num4 = (num2 - this.minOffsetY) / num3;
					float num5 = 1f - 0.5f * num4;
					item.transform.localScale = Vector3.one * num5;
				}
			}
		}

		protected void ClearAll()
		{
			for (int i = 0; i < this.floatingItems.Count; i++)
			{
				this.floatingItems[i].DeInit();
			}
			for (int j = 0; j < this.items.Count; j++)
			{
				Object.Destroy(this.items[j].gameObject);
			}
			this.items.Clear();
			this.prefabs.Clear();
			this.floatingItems.Clear();
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

		protected float GetCameraSpeed()
		{
			float num = 0f;
			if (this.sceneMapController != null && this.sceneMapController.gameCameraController != null)
			{
				num = this.sceneMapController.gameCameraController.GetMoveSpeed();
			}
			return num;
		}

		private float GetPlayerSpeed()
		{
			float num = 0f;
			if (this.isHangUpSpeed)
			{
				num = HangUpManager.Instance.GetPlayerMoveSpeed();
			}
			else if (EventMemberController.Instance != null)
			{
				num = EventMemberController.Instance.GetPlayerMoveSpeed();
			}
			return num;
		}

		protected float GetNormalSpeed()
		{
			float num = 0f;
			switch (this.layerType)
			{
			case SceneMapLayerType.Cloud:
			case SceneMapLayerType.FarFloating:
				num = (this.isPlayerMovePause ? this.normalSpeedOffset : (this.GetCameraSpeed() + this.normalSpeedOffset));
				break;
			case SceneMapLayerType.MiddleFloating:
				num = 0f;
				break;
			case SceneMapLayerType.NearFloating:
				num = (this.isPlayerMovePause ? 0f : this.normalSpeedOffset);
				break;
			case SceneMapLayerType.FarStatic:
				num = (this.isPlayerMovePause ? 0f : this.GetPlayerSpeed());
				break;
			}
			return num;
		}

		protected float GetFastSpeed()
		{
			float num = 0f;
			switch (this.layerType)
			{
			case SceneMapLayerType.Cloud:
			case SceneMapLayerType.FarFloating:
				num = (this.isPlayerMovePause ? this.fastSpeedOffset : (this.GetCameraSpeed() + this.fastSpeedOffset));
				break;
			case SceneMapLayerType.MiddleFloating:
				num = 0f;
				break;
			case SceneMapLayerType.NearFloating:
				num = (this.isPlayerMovePause ? 0f : this.fastSpeedOffset);
				break;
			case SceneMapLayerType.FarStatic:
				num = (this.isPlayerMovePause ? 0f : this.GetPlayerSpeed());
				break;
			}
			return num;
		}

		protected float GetHangUpSpeed()
		{
			float num = 0f;
			switch (this.layerType)
			{
			case SceneMapLayerType.Cloud:
			case SceneMapLayerType.FarFloating:
			case SceneMapLayerType.NearFloating:
				num = this.hangupSpeedOffset;
				break;
			case SceneMapLayerType.MiddleFloating:
				num = 0f;
				break;
			case SceneMapLayerType.FarStatic:
				num = this.GetPlayerSpeed();
				break;
			}
			return num;
		}

		public void SetPlayerMovePause(bool movePause)
		{
			this.isPlayerMovePause = movePause;
		}

		public void SetTime(float to, float duration)
		{
			for (int i = 0; i < this.floatingItems.Count; i++)
			{
				this.floatingItems[i].SetTime(to, duration);
			}
		}

		public void ResetWordPosition()
		{
			Vector3 position = base.transform.position;
			position.x -= 1000f;
			base.transform.position = position;
		}

		protected const float RecycleDistance = 25f;

		protected const float MinScale = 0.5f;

		protected const float MaxScale = 1f;

		protected SceneMapLayerType layerType;

		protected int itemCount;

		protected float minOffsetX;

		protected float maxOffsetX;

		protected float minOffsetY;

		protected float maxOffsetY;

		protected Transform playerTransform;

		protected List<GameObject> prefabs = new List<GameObject>();

		protected List<GameObject> items = new List<GameObject>();

		protected float lastX;

		protected float moveSpeed;

		protected bool isPause;

		protected bool isFastSpeed;

		protected bool isHangUpSpeed;

		protected bool isPlayerMovePause;

		protected SceneMapController sceneMapController;

		private List<MapFloatingItem> floatingItems = new List<MapFloatingItem>();

		[SerializeField]
		private float normalSpeedOffset;

		[SerializeField]
		private float fastSpeedOffset;

		[SerializeField]
		private float hangupSpeedOffset;

		private float recycleDis;
	}
}
