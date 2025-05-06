using System;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix.Client
{
	public class GameCameraController
	{
		public Camera MainCamera
		{
			get
			{
				return this.m_camera;
			}
		}

		public async Task OnInit(GameObject gameObject, bool isAutoRatio = true)
		{
			this.SelfObj = gameObject;
			ComponentRegister component = gameObject.GetComponent<ComponentRegister>();
			this.m_camera = component.GetGameObject("Camera").GetComponent<Camera>();
			if (isAutoRatio)
			{
				this.m_camera.orthographicSize *= Singleton<QualityManager>.Instance.GetDefaultCameraRatio(true);
			}
			this.m_mask = component.GetGameObject("Mask");
			this.m_follow = component.GetGameObject("Follow").transform;
			EventArgsBindCamera instance = Singleton<EventArgsBindCamera>.Instance;
			instance.m_camera = this.m_camera;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_BindCamera, instance);
			this.m_cameraShake = new CameraShake();
			this.m_cameraShake.SetData(this.m_camera);
			this.m_cameraShake.OnInit();
			this.RegisterEvent();
			await Task.CompletedTask;
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime = 0f)
		{
			CameraShake cameraShake = this.m_cameraShake;
			if (cameraShake != null)
			{
				cameraShake.OnUpdate(deltaTime, deltaTime);
			}
			this.OnLateUpdate();
		}

		public async Task OnDeInit()
		{
			CameraShake cameraShake = this.m_cameraShake;
			if (cameraShake != null)
			{
				cameraShake.OnDeInit();
			}
			this.UnRegisterEvent();
			await Task.CompletedTask;
		}

		private void RegisterEvent()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Battle_Shake, new HandlerEvent(this.OnEventShake));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Battle_ShowHighlight, new HandlerEvent(this.OnEventShowHighlight));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Battle_HideHighlight, new HandlerEvent(this.OnEventHideHighlight));
		}

		private void UnRegisterEvent()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Battle_Shake, new HandlerEvent(this.OnEventShake));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Battle_ShowHighlight, new HandlerEvent(this.OnEventShowHighlight));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Battle_HideHighlight, new HandlerEvent(this.OnEventHideHighlight));
		}

		private void OnEventShake(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsGameCameraShake eventArgsGameCameraShake = eventArgs as EventArgsGameCameraShake;
			if (eventArgsGameCameraShake != null && eventArgsGameCameraShake.shakeType != 0)
			{
				CameraShake cameraShake = this.m_cameraShake;
				if (cameraShake == null)
				{
					return;
				}
				cameraShake.Shake(eventArgsGameCameraShake.shakeType, eventArgsGameCameraShake.delay, eventArgsGameCameraShake.duration, eventArgsGameCameraShake.power, eventArgsGameCameraShake.count);
				return;
			}
			else
			{
				CameraShake cameraShake2 = this.m_cameraShake;
				if (cameraShake2 == null)
				{
					return;
				}
				cameraShake2.Shake();
				return;
			}
		}

		private void OnEventShowHighlight(object sender, int type, BaseEventArgs eventargs)
		{
			this.m_mask.SetActive(true);
		}

		private void OnEventHideHighlight(object sender, int type, BaseEventArgs eventargs)
		{
			this.m_mask.SetActive(false);
		}

		public void OnPause(bool pause)
		{
		}

		public void SetController(MapMemberControllerBase mapMemberCtrl)
		{
			this.mapMemberController = mapMemberCtrl;
		}

		public void SetFollowActive(bool isActive)
		{
			this.curActive = isActive;
		}

		public void SetTarget(Transform target)
		{
			this.target = target;
			this.tempNormalOffset = new Vector3(0f, this.cameraxOffset, 0f);
		}

		public void SetFollowMode(CameraFollowType type)
		{
			this.curFollowType = type;
			if (type == CameraFollowType.Tackle)
			{
				this.tempTackleOffset = this.target.position + this.tempNormalOffset + new Vector3(3.5f, 0f, 0f);
			}
		}

		public void SetBattleEnd()
		{
			this.SetFollowMode(CameraFollowType.BattleEnd);
		}

		public void OnLateUpdate()
		{
			if (this.isFollowPause)
			{
				return;
			}
			if (!this.curActive)
			{
				return;
			}
			if (this.target == null)
			{
				return;
			}
			switch (this.curFollowType)
			{
			case CameraFollowType.Normal:
				this.UpdateFollowPosition(this.target.position, 0.1f);
				return;
			case CameraFollowType.Tackle:
				this.UpdateFollowPosition(this.target.position, 0.15f);
				return;
			case CameraFollowType.TackleToBattle:
			{
				Vector3 vector = this.target.position + new Vector3(this.GetPlayerOffsetX(), 0f, 0f);
				this.UpdateFollowPosition(vector, 0.15f);
				return;
			}
			case CameraFollowType.BattleStart:
			{
				Vector3 vector2 = this.target.position + new Vector3(this.GetPlayerOffsetX(), 0f, 0f);
				this.UpdateFollowPosition(vector2, 0.15f);
				return;
			}
			case CameraFollowType.BattleFish:
			{
				Vector3 vector3 = this.target.position + new Vector3(this.GetFishOffsetX(), 0f, 0f);
				this.UpdateFollowPosition(vector3, 0.15f);
				return;
			}
			case CameraFollowType.BattleEnd:
				this.velocity = Vector3.zero;
				if (Utility.Math.Abs(this.m_follow.position.x - this.target.position.x) < 0.1f || this.target.position.x >= this.m_follow.position.x)
				{
					this.SetFollowMode(CameraFollowType.Normal);
				}
				return;
			default:
				return;
			}
		}

		private void UpdateFollowPosition(Vector3 targetPos, float smoothTime)
		{
			this.moveSpeed = this.GetPlayerSpeed() * 2f * Time.timeScale;
			this.adjustTime = smoothTime / Time.timeScale;
			if (this.adjustTime < 0.1f)
			{
				this.m_follow.position = this.target.position;
				this.velocity = Vector3.zero;
				return;
			}
			this.m_follow.position = Vector3.SmoothDamp(this.m_follow.position, targetPos, ref this.velocity, this.adjustTime, this.moveSpeed, Time.deltaTime);
		}

		private float GetPlayerSpeed()
		{
			float num = 0f;
			if (this.mapMemberController != null)
			{
				num = this.mapMemberController.GetPlayerMoveSpeed();
			}
			return num;
		}

		public float GetMoveSpeed()
		{
			return this.velocity.x;
		}

		public float GetPlayerOffsetX()
		{
			float num = 2.6f;
			if (this.mapMemberController != null)
			{
				num += this.mapMemberController.GetPlayerOffsetX();
			}
			return num;
		}

		public float GetFishOffsetX()
		{
			float num = 2f;
			if (this.mapMemberController != null)
			{
				num += this.mapMemberController.GetPlayerOffsetX();
			}
			return num;
		}

		public Vector3 GetPosition()
		{
			return this.m_follow.position;
		}

		public void ResetWordPosition()
		{
			Vector3 position = this.m_follow.position;
			position.x -= 1000f;
			this.m_follow.position = position;
		}

		public void SetReadyEnter(float y)
		{
			Vector3 position = this.m_follow.position;
			position.y = y;
			this.m_follow.transform.position = position;
			this.initSize = this.MainCamera.orthographicSize;
			this.MainCamera.orthographicSize = this.initSize + 2f;
		}

		public void ResetSceneSkinPos()
		{
			if (this.target != null)
			{
				this.m_follow.transform.position = this.target.position;
				this.MainCamera.orthographicSize = this.initSize;
			}
		}

		public void SetHotSpringPos(float x, float size)
		{
			Vector3 position = this.m_follow.position;
			position.x = x;
			this.m_follow.transform.position = position;
			this.MainCamera.orthographicSize = size;
		}

		public void FirstEnter()
		{
			TweenSettingsExtensions.Append(DOTween.Sequence(), ShortcutExtensions.DOOrthoSize(this.MainCamera, this.initSize, 0.8f));
		}

		public Camera m_camera;

		public GameObject m_mask;

		public CameraShake m_cameraShake;

		public Transform m_follow;

		private MapMemberControllerBase mapMemberController;

		public GameObject SelfObj;

		private float cameraxOffset = 0.5f;

		public float moveSpeed = 50f;

		public float accelerationTime = 500f;

		public float decelerationTime = 100f;

		private float adjustTime;

		private CameraFollowType curFollowType;

		public Transform target;

		public Vector3 velocity = Vector3.zero;

		private Vector3 tempNormalOffset = Vector3.zero;

		private Vector3 tempTackleOffset = Vector3.zero;

		private bool curActive;

		private bool isFollowPause;

		private float initSize;
	}
}
