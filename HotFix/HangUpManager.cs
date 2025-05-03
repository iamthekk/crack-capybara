using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using HotFix.Client;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class HangUpManager
	{
		public static HangUpManager Instance { get; private set; }

		public Task Init()
		{
			HangUpManager.<Init>d__17 <Init>d__;
			<Init>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<Init>d__.<>4__this = this;
			<Init>d__.<>1__state = -1;
			<Init>d__.<>t__builder.Start<HangUpManager.<Init>d__17>(ref <Init>d__);
			return <Init>d__.<>t__builder.Task;
		}

		public void Update(float deltaTime, float unscaledDeltaTime)
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
			HangUpMemberController hangUpMemberController = this.hangUpMemberController;
			if (hangUpMemberController == null)
			{
				return;
			}
			hangUpMemberController.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public void DeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Pet_FormationIdsChange, new HandlerEvent(this.OnPetFormationChanged));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIMount_ChangeRide, new HandlerEvent(this.OnMountChangeRide));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ClothesData_SelfClothesChanged, new HandlerEvent(this.OnPlayerClothesChange));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Change_SceneSkin, new HandlerEvent(this.OnChangeSceneSkin));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_NewWorld_RefreshTopPlayer, new HandlerEvent(this.OnRefreshTopPlayer));
			if (this.sceneMapController != null)
			{
				this.sceneMapController.OnDeInit();
			}
			if (this.cameraController != null)
			{
				this.cameraController.OnDeInit();
			}
			if (this.hangUpMemberController != null)
			{
				this.hangUpMemberController.OnDeInit();
			}
			if (this.m_hotSpringSkinCtrl != null)
			{
				this.m_hotSpringSkinCtrl.DeInit();
			}
			if (this.newWorldRootController != null)
			{
				this.newWorldRootController.DeInit();
			}
		}

		public void StartMove()
		{
			this.cameraController.SetTarget(this.hangUpMemberController.GetPlayerMove());
			this.cameraController.SetFollowMode(CameraFollowType.Normal);
			this.cameraController.FirstEnter();
			this.hangUpMemberController.FirstEnter(delegate
			{
				this.cameraController.SetFollowActive(true);
				this.hangUpMemberController.StartMove();
				this.sceneMapController.StartMove();
			});
		}

		public float GetPlayerMoveSpeed()
		{
			return this.hangUpMemberController.GetPlayerMoveSpeed();
		}

		public void ResetWordPosition()
		{
			this.cameraController.ResetWordPosition();
			this.sceneMapController.ResetWordPosition();
		}

		private void OnChangeSceneSkin(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.m_ClosthesDataModule == null)
			{
				return;
			}
			if (this.sceneMapController == null)
			{
				return;
			}
			if (this.newWorldDataModule == null)
			{
				return;
			}
			if (this.newWorldDataModule.IsShowNewWorldScene())
			{
				this.cameraController.SelfObj.SetActiveSafe(false);
				this.sceneMapController.SelfObj.SetActiveSafe(false);
				this.hangUpMemberController.SelfObj.SetActiveSafe(false);
				if (this.m_hotSpringSkinObj != null)
				{
					this.m_hotSpringSkinObj.SetActiveSafe(false);
				}
				this.CreateNewWorldSkin();
				return;
			}
			if (this.newWorldSkinObj != null)
			{
				this.newWorldSkinObj.SetActiveSafe(false);
			}
			if (this.m_ClosthesDataModule.SelfSceneSkinData.CurSkinId == 1)
			{
				this.cameraController.SelfObj.SetActiveSafe(true);
				this.sceneMapController.SelfObj.SetActiveSafe(true);
				this.hangUpMemberController.SelfObj.SetActiveSafe(true);
				if (this.m_hotSpringSkinObj != null)
				{
					this.m_hotSpringSkinObj.SetActiveSafe(false);
					return;
				}
			}
			else
			{
				this.cameraController.SelfObj.SetActiveSafe(false);
				this.sceneMapController.SelfObj.SetActiveSafe(false);
				this.hangUpMemberController.SelfObj.SetActiveSafe(false);
				this.OnCreateHotSpringSkin();
			}
		}

		private async Task OnCreateHotSpringSkin()
		{
			Avatar_SceneSkin sceneSkinCfg = GameApp.Table.GetManager().GetAvatar_SceneSkin(this.m_ClosthesDataModule.SelfSceneSkinData.CurSkinId);
			if (sceneSkinCfg != null)
			{
				if (this.m_hotSpringSkinObj == null)
				{
					AsyncOperationHandle<GameObject> handle = GameApp.Resources.LoadAssetAsync<GameObject>(sceneSkinCfg.prefabPath);
					await handle.Task;
					if (handle.Status != 1)
					{
						HLog.LogError("load LoadAssetAsync,path = " + sceneSkinCfg.prefabPath);
						return;
					}
					this.m_hotSpringSkinObj = Object.Instantiate<GameObject>(handle.Result);
					this.m_hotSpringSkinCtrl = this.m_hotSpringSkinObj.GetComponent<HotSpringSkinRootController>();
					this.m_hotSpringSkinObj.SetParentNormal(this.m_SkinRoot, false);
					this.m_hotSpringSkinCtrl.Init();
					handle = default(AsyncOperationHandle<GameObject>);
				}
				this.m_hotSpringSkinObj.SetActiveSafe(true);
			}
		}

		private async Task CreateNewWorldSkin()
		{
			if (this.newWorldDataModule.IsShowNewWorldScene())
			{
				if (this.newWorldSkinObj == null)
				{
					AsyncOperationHandle<GameObject> handle = GameApp.Resources.LoadAssetAsync<GameObject>("Assets/_Resources/Prefab/Map/NewWorldRoot.prefab");
					await handle.Task;
					if (handle.Status != 1)
					{
						HLog.LogError("load LoadAssetAsync,path = Assets/_Resources/Prefab/Map/NewWorldRoot.prefab");
						return;
					}
					this.newWorldSkinObj = Object.Instantiate<GameObject>(handle.Result);
					this.newWorldSkinObj.SetParentNormal(this.m_SkinRoot, false);
					this.newWorldRootController = this.newWorldSkinObj.GetComponent<NewWorldRootController>();
					this.newWorldRootController.Init();
					handle = default(AsyncOperationHandle<GameObject>);
				}
				this.newWorldSkinObj.SetActiveSafe(true);
			}
		}

		private void OnPetFormationChanged(object sender, int type, BaseEventArgs eventArgs)
		{
			this.hangUpMemberController.PetFormationChanged();
			HotSpringSkinRootController hotSpringSkinCtrl = this.m_hotSpringSkinCtrl;
			if (hotSpringSkinCtrl == null)
			{
				return;
			}
			hotSpringSkinCtrl.PetFormationChanged();
		}

		private void OnMountChangeRide(object sender, int type, BaseEventArgs eventArgs)
		{
			this.hangUpMemberController.MountChanged();
			if (this.newWorldRootController != null)
			{
				this.newWorldRootController.MountChanged();
			}
		}

		private void OnPlayerClothesChange(object sender, int type, BaseEventArgs eventArgs)
		{
			this.hangUpMemberController.PlayerClothesChange();
			HotSpringSkinRootController hotSpringSkinCtrl = this.m_hotSpringSkinCtrl;
			if (hotSpringSkinCtrl != null)
			{
				hotSpringSkinCtrl.PlayerClothesChange();
			}
			if (this.newWorldRootController != null)
			{
				this.newWorldRootController.PlayerClothesChange();
			}
		}

		private void OnRefreshTopPlayer(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsRefreshTopPlayer eventArgsRefreshTopPlayer = eventArgs as EventArgsRefreshTopPlayer;
			if (eventArgsRefreshTopPlayer != null && eventArgsRefreshTopPlayer.topPlayer != null && this.newWorldRootController != null)
			{
				this.newWorldRootController.CheckShowTopPlayer(eventArgsRefreshTopPlayer.topPlayer);
			}
		}

		public const string HangupMap = "Assets/_Resources/Prefab/Map/HangUpRoot.prefab";

		public const string NewWorldMap = "Assets/_Resources/Prefab/Map/NewWorldRoot.prefab";

		private GameObject mapObj;

		private SceneMapController sceneMapController;

		private GameCameraController cameraController;

		private HangUpMemberController hangUpMemberController;

		private GameObject m_SkinRoot;

		private ClothesDataModule m_ClosthesDataModule;

		private GameObject m_hotSpringSkinObj;

		private HotSpringSkinRootController m_hotSpringSkinCtrl;

		private GameObject newWorldSkinObj;

		private NewWorldRootController newWorldRootController;

		private NewWorldDataModule newWorldDataModule;
	}
}
