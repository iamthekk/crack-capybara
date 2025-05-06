using System;
using DG.Tweening;
using Framework.Logic.Component;
using HotFix.Client;
using UnityEngine;

namespace HotFix
{
	public class MapPlanetCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			if (this.daytimeObj)
			{
				this.daytimeObj.SetActiveSafe(true);
			}
			if (this.duskObj)
			{
				this.duskObj.SetActiveSafe(false);
			}
			if (this.nightObj)
			{
				this.nightObj.SetActiveSafe(false);
			}
			if (this.duskLight)
			{
				this.duskLight.gameObject.SetActiveSafe(false);
			}
		}

		protected override void OnDeInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.playerTrans != null)
			{
				this.move.transform.position = this.GetCameraPosition() + this.playerOffset;
			}
		}

		public void SetData(int isPlanet, Vector3 offset, Transform follow, SceneMapController mapController)
		{
			this.isHidePlanet = isPlanet == 0;
			this.playerOffset = offset;
			this.playerTrans = follow;
			this.sceneMapController = mapController;
			this.currentTime = 0;
		}

		public void SetTime(int random, float duration)
		{
			if (this.isHidePlanet)
			{
				return;
			}
			if (this.isAni)
			{
				return;
			}
			if (this.currentTime == random)
			{
				return;
			}
			this.isAni = true;
			GameObject @object = this.GetObject(this.currentTime);
			GameObject object2 = this.GetObject(random);
			this.HideAni(@object, duration);
			this.ShowAni(object2, duration);
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.AppendInterval(sequence, duration);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.currentTime = random;
				this.isAni = false;
			});
		}

		private GameObject GetObject(int time)
		{
			if (time == 0)
			{
				return this.daytimeObj;
			}
			if (time == 1)
			{
				return this.duskObj;
			}
			if (time == 2)
			{
				return this.nightObj;
			}
			return null;
		}

		private void ShowAni(GameObject obj, float duration)
		{
			if (obj)
			{
				obj.SetActiveSafe(true);
				Vector3 localPosition = obj.transform.localPosition;
				localPosition.y = 5f;
				obj.transform.localPosition = localPosition;
				float num = duration - duration / 4f;
				float num2 = duration / 4f;
				float num3 = -0.5f;
				if (obj == this.duskObj)
				{
					num3 = -1.25f;
				}
				Sequence sequence = DOTween.Sequence();
				TweenSettingsExtensions.AppendInterval(sequence, num);
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOLocalMoveY(obj.transform, num3, num2, false));
				if (obj == this.duskObj && this.duskLight != null)
				{
					Color color = this.duskLight.color;
					color.a = 0f;
					this.duskLight.color = color;
					this.duskLight.gameObject.SetActiveSafe(true);
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions43.DOFade(this.duskLight, 1f, 0.2f));
				}
			}
		}

		private void HideAni(GameObject obj, float duration)
		{
			if (obj)
			{
				float num = duration / 4f;
				Sequence sequence = DOTween.Sequence();
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOLocalMoveY(obj.transform, -5f, num, false));
				if (obj == this.duskObj && this.duskLight != null)
				{
					TweenSettingsExtensions.Join(sequence, ShortcutExtensions43.DOFade(this.duskLight, 0f, num));
				}
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					obj.SetActiveSafe(false);
				});
			}
		}

		private Vector3 GetCameraPosition()
		{
			if (this.sceneMapController != null && this.sceneMapController.gameCameraController != null)
			{
				return this.sceneMapController.gameCameraController.GetPosition();
			}
			return Vector3.zero;
		}

		public GameObject move;

		public GameObject daytimeObj;

		public GameObject duskObj;

		public GameObject nightObj;

		public SpriteRenderer duskLight;

		private Transform playerTrans;

		private Vector3 playerOffset;

		private int currentTime;

		private bool isAni;

		private SceneMapController sceneMapController;

		private bool isHidePlanet;
	}
}
