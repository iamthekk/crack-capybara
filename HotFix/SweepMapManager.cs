using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using HotFix.Client;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class SweepMapManager
	{
		public static SweepMapManager Instance { get; private set; }

		public Task Init()
		{
			SweepMapManager.<Init>d__10 <Init>d__;
			<Init>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<Init>d__.<>4__this = this;
			<Init>d__.<>1__state = -1;
			<Init>d__.<>t__builder.Start<SweepMapManager.<Init>d__10>(ref <Init>d__);
			return <Init>d__.<>t__builder.Task;
		}

		public void StartMove()
		{
			this.cameraController.SetTarget(this.sweepMemberController.GetPlayerMove());
			this.cameraController.SetFollowMode(CameraFollowType.Normal);
			this.cameraController.SetFollowActive(true);
			this.sweepMemberController.StartMove();
			this.sceneMapController.StartMove();
		}

		public void DeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Pet_FormationIdsChange, new HandlerEvent(this.OnPetFormationChanged));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIMount_ChangeRide, new HandlerEvent(this.OnMountChangeRide));
			if (this.sceneMapController != null)
			{
				this.sceneMapController.OnDeInit();
			}
			if (this.cameraController != null)
			{
				this.cameraController.OnDeInit();
			}
			if (this.sweepMemberController != null)
			{
				this.sweepMemberController.OnDeInit();
			}
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.mapObj == null || !this.mapObj.activeSelf)
			{
				return;
			}
			SceneMapController sceneMapController = this.sceneMapController;
			if (sceneMapController != null)
			{
				sceneMapController.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			GameCameraController gameCameraController = this.cameraController;
			if (gameCameraController != null)
			{
				gameCameraController.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			SweepMemberController sweepMemberController = this.sweepMemberController;
			if (sweepMemberController == null)
			{
				return;
			}
			sweepMemberController.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public void SetCameraTarget(RawImage img, Vector2 size, int depth = 1000)
		{
			if (this.cameraController.MainCamera != null && img != null)
			{
				if (this.renderTexture == null)
				{
					this.renderTexture = new RenderTexture((int)size.x, (int)size.y, depth);
				}
				this.renderTexture.name = "SweepCameraTextureTarget";
				this.cameraController.MainCamera.targetTexture = this.renderTexture;
				img.texture = this.renderTexture;
			}
		}

		public void SetShow(bool isShow)
		{
			this.mapObj.SetActiveSafe(isShow);
		}

		public void ResetWordPosition()
		{
			this.cameraController.ResetWordPosition();
			this.sceneMapController.ResetWordPosition();
		}

		private void OnPetFormationChanged(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.sweepMemberController != null)
			{
				this.sweepMemberController.PetFormationChanged();
			}
		}

		private void OnMountChangeRide(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.sweepMemberController != null)
			{
				this.sweepMemberController.MountChanged();
			}
		}

		public const string SweepMap = "Assets/_Resources/Prefab/Map/SweepRoot.prefab";

		private GameObject mapObj;

		private SceneMapController sceneMapController;

		private GameCameraController cameraController;

		private SweepMemberController sweepMemberController;

		private RenderTexture renderTexture;
	}
}
