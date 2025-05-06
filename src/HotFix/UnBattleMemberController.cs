using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class UnBattleMemberController : MapMemberControllerBase
	{
		public void OnInit(GameObject gameObject, int mapId, int rideId, Action onResetWorldPos)
		{
			this.SelfObj = gameObject;
			this.RideId = rideId;
			this.OnResetWorldPos = onResetWorldPos;
			ComponentRegister component = gameObject.GetComponent<ComponentRegister>();
			this.playerMove = component.GetGameObject("PlayerMove").transform;
			this.playerParent = component.GetGameObject("PlayerParent").transform;
			Map_map elementById = GameApp.Table.GetManager().GetMap_mapModelInstance().GetElementById(mapId);
			if (elementById != null)
			{
				for (int i = 0; i < elementById.petOffset.Length; i++)
				{
					List<float> listFloat = elementById.petOffset[i].GetListFloat(',');
					if (listFloat.Count >= 2)
					{
						Vector3 vector;
						vector..ctor(listFloat[0], listFloat[1], 0f);
						this.petOffsetList.Add(vector);
					}
				}
			}
		}

		public void OnDeInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.isPauseMove)
			{
				return;
			}
			if (this.playerMove == null)
			{
				return;
			}
			Vector3 vector = Vector3.right * this.playerMoveSpeed * deltaTime;
			this.playerMove.position += vector;
			if (this.playerMove.position.x > 1000f)
			{
				this.ResetWordPosition();
			}
		}

		public async Task CreateMember()
		{
			await this.CreateRide();
			await this.CreatePlayer();
			await Task.WhenAll(new List<Task>
			{
				this.CreatePets(),
				this.CreateMount()
			});
		}

		protected async Task CreatePlayer()
		{
			Transform transform = this.playerParent;
			if (this.rideCtrl != null)
			{
				transform = this.rideCtrl.transform;
			}
			TaskOutValue<MainMemberModeItemData> outPlayer = new TaskOutValue<MainMemberModeItemData>();
			await ModelUtils.CreatePlayerMember(transform.gameObject, outPlayer);
			if (outPlayer.Value != null)
			{
				this.mainMemberData = outPlayer.Value;
			}
		}

		protected async Task CreatePets()
		{
			if (this.mainMemberData != null && !(this.mainMemberData.MainMemberSpinePlayer == null))
			{
				MainMemberSpinePlayer mainMemberSpinePlayer = this.mainMemberData.MainMemberSpinePlayer;
				int minLayer = mainMemberSpinePlayer.GetMinOrderLayer();
				int maxLayer = mainMemberSpinePlayer.GetMaxOrderLayer();
				PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
				List<CardData> petCards = dataModule.GetFightPetCardData();
				this.petList.Clear();
				this.petObjs.Clear();
				for (int i = 0; i < petCards.Count; i++)
				{
					CardData cardData = petCards[i];
					GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(cardData.m_memberID);
					ArtMember_member modelTable = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(elementById.modelID);
					Vector3 scale = base.GetScale(elementById.modelSacle);
					AsyncOperationHandle<GameObject> handler = GameApp.Resources.LoadAssetAsync<GameObject>(modelTable.path);
					await handler.Task;
					if (handler.Result != null)
					{
						GameObject gameObject = Object.Instantiate<GameObject>(handler.Result);
						this.petObjs.Add(gameObject);
						if (this.rideCtrl != null)
						{
							gameObject.SetParentNormal(this.rideCtrl.transform, false);
						}
						else
						{
							gameObject.SetParentNormal(this.playerParent, false);
						}
						if (i < this.petOffsetList.Count)
						{
							gameObject.transform.localPosition = this.petOffsetList[i];
						}
						NormalSpinePlayerPlayer normalSpinePlayerPlayer = gameObject.GetComponentInChildren<NormalSpinePlayerPlayer>(true);
						ComponentRegister component = gameObject.GetComponent<ComponentRegister>();
						if (component)
						{
							component.GetGameObject("SpineRoot").transform.localScale = scale;
						}
						if (normalSpinePlayerPlayer == null)
						{
							normalSpinePlayerPlayer = gameObject.AddComponent<NormalSpinePlayerPlayer>();
						}
						if (normalSpinePlayerPlayer != null)
						{
							normalSpinePlayerPlayer.Init(gameObject.GetComponent<ComponentRegister>());
							normalSpinePlayerPlayer.PlayAnimation("Idle");
							if (i == 0 || i == 2)
							{
								normalSpinePlayerPlayer.SetOrderLayer(maxLayer + 5);
							}
							else if (i == 1)
							{
								normalSpinePlayerPlayer.SetOrderLayer(minLayer - 5);
							}
							this.petList.Add(normalSpinePlayerPlayer);
						}
						else
						{
							HLog.LogError("Not found NormalSpinePlayerPlayer in " + modelTable.path);
						}
					}
					modelTable = null;
					scale = default(Vector3);
					handler = default(AsyncOperationHandle<GameObject>);
				}
			}
		}

		protected Task CreateRide()
		{
			UnBattleMemberController.<CreateRide>d__19 <CreateRide>d__;
			<CreateRide>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CreateRide>d__.<>4__this = this;
			<CreateRide>d__.<>1__state = -1;
			<CreateRide>d__.<>t__builder.Start<UnBattleMemberController.<CreateRide>d__19>(ref <CreateRide>d__);
			return <CreateRide>d__.<>t__builder.Task;
		}

		protected async Task CreateMount()
		{
			MountDataModule dataModule = GameApp.Data.GetDataModule(DataName.MountDataModule);
			this.currentMountId = dataModule.GetMountModelId();
			if (this.mainMemberData != null)
			{
				TaskOutValue<MountMemberModeItemData> outMount = new TaskOutValue<MountMemberModeItemData>();
				await ModelUtils.CreateSelfPlayerMount(this.mainMemberData, outMount);
				if (outMount.Value != null)
				{
					this.mountSpinePlayer = outMount.Value.MountSpinePlayer;
					this.mountPlayer = outMount.Value.MountMemberRoot;
				}
				outMount = null;
			}
		}

		public void StartMove()
		{
			this.ChangeMoveAni();
			this.playerMoveSpeed = this.PlayerNormalSpeed;
			this.isPauseMove = false;
		}

		protected void ChangeMoveAni()
		{
			if (this.mainMemberData == null || this.mainMemberData.MainMemberSpinePlayer == null)
			{
				return;
			}
			MainMemberSpinePlayer mainMemberSpinePlayer = this.mainMemberData.MainMemberSpinePlayer;
			if (this.rideCtrl != null)
			{
				mainMemberSpinePlayer.PlayAnimation(MemberAnimationName.GetSelfIdleAnimationName());
				this.PlayPetAnimation("Idle");
				if (this.mountSpinePlayer)
				{
					this.mountSpinePlayer.PlayAnimation("Idle");
					return;
				}
			}
			else
			{
				if (this.mountSpinePlayer)
				{
					this.mountSpinePlayer.PlayAnimation("Run");
					mainMemberSpinePlayer.PlayAnimation(MemberAnimationName.GetSelfIdleAnimationName());
				}
				else
				{
					mainMemberSpinePlayer.PlayAnimation("Run");
				}
				this.PlayPetAnimation("Run");
			}
		}

		protected void ChangeFastMoveAni()
		{
			if (this.mainMemberData == null || this.mainMemberData.MainMemberSpinePlayer == null)
			{
				return;
			}
			MainMemberSpinePlayer mainMemberSpinePlayer = this.mainMemberData.MainMemberSpinePlayer;
			if (this.rideCtrl != null)
			{
				mainMemberSpinePlayer.PlayAnimation(MemberAnimationName.GetSelfIdleAnimationName());
				this.PlayPetAnimation("Idle");
				if (this.mountSpinePlayer)
				{
					this.mountSpinePlayer.PlayAnimation("Idle");
					return;
				}
			}
			else
			{
				if (this.mountSpinePlayer)
				{
					this.mountSpinePlayer.PlayAnimation("FastRun");
					mainMemberSpinePlayer.PlayAnimation(MemberAnimationName.GetSelfIdleAnimationName());
				}
				else
				{
					mainMemberSpinePlayer.PlayAnimation("FastRun");
				}
				this.PlayPetAnimation("Run");
			}
		}

		public Transform GetPlayerMove()
		{
			return this.playerMove;
		}

		public float PlayerNormalSpeed
		{
			get
			{
				if (this.rideCtrl != null)
				{
					return 2f;
				}
				return 4f;
			}
		}

		protected void PlayPetAnimation(string ani)
		{
			for (int i = 0; i < this.petList.Count; i++)
			{
				this.petList[i].PlayAnimation(ani);
			}
		}

		protected void ResetWordPosition()
		{
			Vector3 position = this.playerMove.position;
			position.x -= 1000f;
			this.playerMove.position = position;
			Action onResetWorldPos = this.OnResetWorldPos;
			if (onResetWorldPos == null)
			{
				return;
			}
			onResetWorldPos();
		}

		public async void PetFormationChanged()
		{
			for (int i = 0; i < this.petObjs.Count; i++)
			{
				GameObject gameObject = this.petObjs[i];
				if (gameObject != null)
				{
					Object.DestroyImmediate(gameObject);
				}
			}
			this.petObjs.Clear();
			await this.CreatePets();
			this.ChangeMoveAni();
		}

		public async void MountChanged()
		{
			int mountModelId = GameApp.Data.GetDataModule(DataName.MountDataModule).GetMountModelId();
			if (this.currentMountId != mountModelId)
			{
				if (this.mainMemberData != null)
				{
					if (this.mainMemberData.MainMemberSpineRoot.transform.parent != this.mainMemberData.MainMemberShakeRoot.transform)
					{
						this.mainMemberData.MainMemberSpineRoot.transform.SetParent(this.mainMemberData.MainMemberShakeRoot.transform);
						this.mainMemberData.MainMemberSpineRoot.transform.localPosition = Vector3.zero;
					}
					if (this.mountPlayer != null)
					{
						this.mountSpinePlayer = null;
						Object.Destroy(this.mountPlayer);
					}
					await this.CreateMount();
					this.ChangeMoveAni();
				}
			}
		}

		public void PlayerClothesChange()
		{
			if (this.mainMemberData != null && this.mainMemberData.MainMemberSpinePlayer != null)
			{
				this.mainMemberData.MainMemberSpinePlayer.Refresh(null);
			}
		}

		public GameObject SelfObj;

		protected Transform playerMove;

		protected Transform playerParent;

		private MainMemberModeItemData mainMemberData;

		protected List<NormalSpinePlayerPlayer> petList = new List<NormalSpinePlayerPlayer>();

		protected List<GameObject> petObjs = new List<GameObject>();

		protected RideCtrl rideCtrl;

		protected GameObject mountPlayer;

		protected MountSpinePlayer mountSpinePlayer;

		protected int currentMountId;

		protected bool isPauseMove = true;

		protected int RideId;

		protected Action OnResetWorldPos;
	}
}
