using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Platfrom;
using LocalModels.Bean;
using Server;
using Spine;
using UnityEngine;

namespace HotFix.Client
{
	public class EventMemberController : BattleMemberController
	{
		public static EventMemberController Instance { get; private set; }

		public bool IsCheckBattle { get; private set; }

		public async Task Init(GameObject gameObject, CMemberFactory factory, Camera camera3D, GameMapData data)
		{
			EventMemberController.Instance = this;
			this.mGameObject = gameObject;
			this.memberFactory = factory;
			this.gameMapData = data;
			ComponentRegister component = gameObject.GetComponent<ComponentRegister>();
			this.playerMove = component.GetGameObject("PlayerMove").transform;
			this.playerParent = component.GetGameObject("PlayerParent");
			this.npcParent = component.GetGameObject("NPC");
			this.fishParent = component.GetGameObject("Fish");
			this.enemyMove = component.GetGameObject("EnemyMove").transform;
			this.enemyParent = component.GetGameObject("EnemyParent");
			this.playerRaftAni = component.GetGameObject("PlayerRaftAni").GetComponent<Animator>();
			this.enemyRaftAni = component.GetGameObject("EnemyRaftAni").GetComponent<Animator>();
			this.weatherObj = component.GetGameObject("Weather");
			this.trashParent = component.GetGameObject("TrashParent").transform;
			this.pointParent = component.GetGameObject("Point");
			Map_map elementById = GameApp.Table.GetManager().GetMap_mapModelInstance().GetElementById(this.gameMapData.MapId);
			if (elementById != null)
			{
				this.mapType = (MapType)elementById.mapType;
				if (elementById.playerOffset.Length >= 2)
				{
					this.playerOffsetX = elementById.playerOffset[0];
					this.playerParent.transform.localPosition = new Vector3(elementById.playerOffset[0], elementById.playerOffset[1], 0f);
					this.enemyParent.transform.localPosition = new Vector3(-elementById.playerOffset[0], elementById.playerOffset[1], 0f);
				}
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
			await this.Init_Ride();
			this.Init_PlayerMove();
			this.Init_Member();
			this.Init_PlayerAction();
			this.Init_FollowingNpc();
			this.Init_Battle();
			this.Init_Fishing();
			this.Init_EnemyMove();
			this.IsCheckBattle = GameApp.SDK.GetCloudDataValue<bool>("CheckChapterBattle", false);
			await Task.CompletedTask;
		}

		public async Task OnDeInit()
		{
			EventMemberController.Instance = null;
			this.StopMoveSound();
			this.DeInit_PlayerMove();
			this.DeInit_PlayerAction();
			this.DeInit_FollowingNpc();
			this.DeInit_Battle();
			this.DeInit_Fishing();
			this.DeInit_EnemyMove();
			this.DeInit_Ride();
			this.DeInit_Member();
			this.waitRemoveMembers.Clear();
			this.waitRemoveEventPoints.Clear();
			await Task.CompletedTask;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.isPause)
			{
				return;
			}
			this.Update_PlayerMove(deltaTime);
			this.Update_Battle(deltaTime);
			this.Update_FollowingNpc(deltaTime);
			this.Update_PassingNpc(deltaTime);
			this.Update_Fishing(deltaTime);
			this.Update_EnemyMove(deltaTime);
			this.Update_Ride(deltaTime, unscaledDeltaTime);
			this.Update_WaveChange(deltaTime);
			this.CheckRemoveNpc();
			if (this.actionNormal != null)
			{
				this.actionNormal.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public void SetController(GameCameraController cameraCtrl, SceneMapController sceneMapCtrl, ClientPointController pointCtrl)
		{
			this.cameraController = cameraCtrl;
			this.sceneMapController = sceneMapCtrl;
			this.clientPointController = pointCtrl;
		}

		public void OnPause(bool pause)
		{
			this.isPause = pause;
			this.OnPause_Fishing(pause);
		}

		public Transform GetPlayerMove()
		{
			return this.playerMove;
		}

		public Transform GetPlayerParent()
		{
			return this.playerParent.transform;
		}

		public Transform GetEnemyParent()
		{
			return this.enemyParent.transform;
		}

		public Transform GetNpcParent()
		{
			return this.npcParent.transform;
		}

		public Transform GetTrashParent()
		{
			return this.trashParent;
		}

		public Transform GetAttachParent()
		{
			if (this.moveToMember != null && this.moveToMember.m_body != null && this.moveToMember.m_body.m_pointNpc1 != null)
			{
				return this.moveToMember.m_body.m_pointNpc1.transform;
			}
			return this.npcParent.transform;
		}

		public void SetPlayerState(EventMemberController.EventPlayerState state)
		{
			this.playerState = state;
			if (this.playerState == EventMemberController.EventPlayerState.Move)
			{
				if (this.playerRide != null)
				{
					this.playerRide.NormalMove();
				}
				this.PlayMoveSound();
			}
			else
			{
				if (this.playerRide != null)
				{
					this.playerRide.StopMove();
				}
				this.ChangePlayerIdleAni();
				this.StopMoveSound();
			}
			if (this.sceneMapController != null)
			{
				this.sceneMapController.SetPlayerMovePause(this.playerState != EventMemberController.EventPlayerState.Move);
			}
		}

		public async Task DoNpcFunction(NpcFunction npcFunction, CMemberBase member, int stage)
		{
			int npcFunction2 = member.m_memberData.m_npcFunction;
			GameMember_npcFunction elementById = GameApp.Table.GetManager().GetGameMember_npcFunctionModelInstance().GetElementById(npcFunction2);
			if (elementById == null)
			{
				HLog.LogError(string.Format("[GameMember_npcFunction] not found id= {0}, memberId={1}", npcFunction2, member.m_memberData.m_id));
			}
			else
			{
				switch (npcFunction)
				{
				case NpcFunction.Normal:
				{
					float num = member.m_gameObject.transform.position.x + elementById.npcOffset[0];
					float num2 = member.m_gameObject.transform.position.y + elementById.npcOffset[1];
					float z = member.m_gameObject.transform.position.z;
					Quaternion rotation = member.m_gameObject.transform.rotation;
					member.m_roleSpinePlayer.SetOrderLayer(-15);
					if (elementById.ride == 0)
					{
						if (elementById.npcOffset.Length > 1)
						{
							member.m_gameObject.transform.position = new Vector3(num, num2);
						}
						else
						{
							member.m_gameObject.transform.position = new Vector3(member.m_gameObject.transform.position.x, GameConfig.GameBattle_NpcPoint_Height);
						}
					}
					else
					{
						await this.CreateNpcRide(elementById.ride, new Vector3(num, num2, z), rotation, elementById.rideScale);
						if (this.npcRide != null)
						{
							member.m_gameObject.transform.SetParentNormal(this.npcRide.pointPlayer, false);
						}
					}
					this.MoveToNpc(npcFunction, member);
					break;
				}
				case NpcFunction.FollowPlayer:
					this.FollowPlayer(member, stage, elementById);
					break;
				case NpcFunction.SmallFish:
					this.fishType = FishType.SmallFish;
					this.CreateFish(member);
					break;
				case NpcFunction.BigFish:
					this.fishType = FishType.BigFish;
					this.CreateFish(member);
					break;
				case NpcFunction.PassingNpc:
					this.CreatePassingMember(member);
					break;
				}
			}
		}

		private void CheckRemoveNpc()
		{
			if (this.waitRemoveMembers.Count > 0)
			{
				if (this.waitRemoveMembers[0] == null)
				{
					this.waitRemoveMembers.RemoveAt(0);
					return;
				}
				Transform transform = this.waitRemoveMembers[0].m_gameObject.transform;
				float num = Vector3.Distance(transform.position, this.playerMove.position);
				if (transform.position.x < this.playerMove.position.x && num > 15f)
				{
					int instanceID = this.waitRemoveMembers[0].InstanceID;
					CMemberFactory cmemberFactory = this.memberFactory;
					if (cmemberFactory != null)
					{
						cmemberFactory.RemoveMember(this.waitRemoveMembers[0]);
					}
					this.waitRemoveMembers.RemoveAt(0);
					this.RemoveRide(instanceID);
				}
			}
			if (this.waitRemoveEventPoints.Count > 0)
			{
				if (this.waitRemoveEventPoints[0] == null)
				{
					this.waitRemoveEventPoints.RemoveAt(0);
					return;
				}
				Transform transform2 = this.waitRemoveEventPoints[0].transform;
				float num2 = Vector3.Distance(transform2.position, this.playerMove.position);
				if (transform2.position.x < this.playerMove.position.x && num2 > 15f)
				{
					Object.Destroy(transform2.gameObject);
					this.waitRemoveEventPoints.RemoveAt(0);
				}
			}
		}

		public void ForceRemoveAllNpc()
		{
			for (int i = 0; i < this.waitRemoveMembers.Count; i++)
			{
				int instanceID = this.waitRemoveMembers[i].InstanceID;
				CMemberFactory cmemberFactory = this.memberFactory;
				if (cmemberFactory != null)
				{
					cmemberFactory.RemoveMember(this.waitRemoveMembers[i]);
				}
				this.RemoveRide(instanceID);
			}
			this.waitRemoveMembers.Clear();
			for (int j = 0; j < this.waitRemoveEventPoints.Count; j++)
			{
				Object.Destroy(this.waitRemoveEventPoints[j].gameObject);
			}
			this.waitRemoveEventPoints.Clear();
		}

		private float GetAnimatorLength(Animator animator, string clipName)
		{
			if (animator == null)
			{
				return 0f;
			}
			float num = 0f;
			foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
			{
				if (animationClip.name.Equals(clipName))
				{
					num = animationClip.length;
					break;
				}
			}
			return num;
		}

		private void PlayMoveSound()
		{
			this.StopMoveSound();
			if (this.playerRide != null)
			{
				DelayCall.Instance.CallLoop(1200, new DelayCall.CallAction(this.PlayWaterSound));
				return;
			}
			DelayCall.Instance.CallLoop(300, new DelayCall.CallAction(this.PlayFootStepSound));
		}

		private void PlayFastMoveSound()
		{
			this.StopMoveSound();
			if (this.playerRide != null)
			{
				DelayCall.Instance.CallLoop(60, new DelayCall.CallAction(this.PlayFastWaterSound));
				return;
			}
			DelayCall.Instance.CallLoop(200, new DelayCall.CallAction(this.PlayFootStepSound));
		}

		private void StopMoveSound()
		{
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.PlayWaterSound));
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.PlayFastWaterSound));
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.PlayFootStepSound));
		}

		private void PlayWaterSound()
		{
			int num = Utility.Math.Random(0, this.soundArr.Length);
			GameApp.Sound.PlayClip(this.soundArr[num], 1f);
		}

		private void PlayFastWaterSound()
		{
			this.PlayWaterSound();
		}

		private void PlayFootStepSound()
		{
			this.curSoundID = this.GetRangeSoundID(this.curSoundID);
			GameApp.Sound.PlayClip(this.curSoundID, 1f);
		}

		private int GetRangeSoundID(int index = -1)
		{
			this.curSoundIDs = new List<int>(this.stepSoundArr);
			if (index >= 0)
			{
				this.curSoundIDs.Remove(index);
			}
			int num = Random.Range(0, this.curSoundIDs.Count);
			return this.curSoundIDs[num];
		}

		public void SetTime(int random, float duration)
		{
			Dictionary<int, CMemberBase> getMembers = this.memberFactory.GetMembers;
			float vcolor = this.sceneMapController.GetVColor(random);
			foreach (CMemberBase cmemberBase in getMembers.Values)
			{
				cmemberBase.PlayVColor(vcolor, duration, null);
			}
			for (int i = 0; i < this.rideList.Count; i++)
			{
				this.rideList[i].PlayVColor(vcolor, duration, null);
			}
			if (this.actionNormal)
			{
				float spriteColor = this.sceneMapController.GetSpriteColor(random);
				this.actionNormal.SetVColor(vcolor, duration);
				this.actionNormal.SetSpriteColor(spriteColor, duration);
			}
		}

		private void SetTime(CMemberBase member)
		{
			if (member == null)
			{
				return;
			}
			if (!this.sceneMapController.IsChangeTime)
			{
				return;
			}
			member.SetVColor(this.sceneMapController.GetVColor(this.sceneMapController.CurrentTime));
		}

		private void SetTime(EventPointBase point)
		{
			if (point == null)
			{
				return;
			}
			if (!this.sceneMapController.IsChangeTime)
			{
				return;
			}
			float vcolor = this.sceneMapController.GetVColor(this.sceneMapController.CurrentTime);
			float spriteColor = this.sceneMapController.GetSpriteColor(this.sceneMapController.CurrentTime);
			point.SetVColor(vcolor, 2f);
			point.SetSpriteColor(spriteColor, 2f);
		}

		private void SetTime(RideCtrl ride)
		{
			if (ride == null)
			{
				return;
			}
			if (!this.sceneMapController.IsChangeTime)
			{
				return;
			}
			ride.SetVColor(this.sceneMapController.GetVColor(this.sceneMapController.CurrentTime));
		}

		private void Init_Battle()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_MonsterGroupFight, new HandlerEvent(this.OnEventBattleNpcFight));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Battle_GameOver, new HandlerEvent(this.OnEventBattleEnd));
		}

		private void DeInit_Battle()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_MonsterGroupFight, new HandlerEvent(this.OnEventBattleNpcFight));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Battle_GameOver, new HandlerEvent(this.OnEventBattleEnd));
		}

		private void Update_Battle(float deltaTime)
		{
			if (this.isBattleNpcMove)
			{
				Vector3 vector = Vector3.right * 5f * deltaTime;
				this.enemyMove.position += vector;
				if (Vector3.Distance(this.playerMove.transform.position, this.enemyMove.transform.position) > 15f)
				{
					this.isBattleNpcMove = false;
					this.MonsterGroupLeaveFinish();
				}
			}
		}

		public void SetEnemyMovePosition(Vector3 pos)
		{
			this.enemyMove.transform.position = pos;
		}

		public async Task DiscoverEnemy(List<CMemberBase> enemyList, int ride, MemberEnterBattleMode enterMode, GameEventBattleType battleType, int dropBox)
		{
			if (enemyList != null)
			{
				this.isDropBox = dropBox > 0;
				this.enemies.AddRange(enemyList);
				this.moveToMode = enterMode;
				this.gameBattleType = battleType;
				if (battleType == GameEventBattleType.Elite)
				{
					ChapterPowerfulEnemyViewModule.OpenData openData = new ChapterPowerfulEnemyViewModule.OpenData(ChapterPowerfulEnemyViewModule.StyleType.Elite, null);
					GameApp.View.OpenView(ViewName.ChapterPowerfulEnemyViewModule, openData, 1, null, null);
				}
				else if (battleType == GameEventBattleType.Boss)
				{
					ChapterPowerfulEnemyViewModule.OpenData openData2 = new ChapterPowerfulEnemyViewModule.OpenData(ChapterPowerfulEnemyViewModule.StyleType.Boss, null);
					GameApp.View.OpenView(ViewName.ChapterPowerfulEnemyViewModule, openData2, 1, null, null);
				}
				else if (this.moveToMode == MemberEnterBattleMode.EnemyComing || this.moveToMode == MemberEnterBattleMode.EnemyComingAppear)
				{
					ChapterPowerfulEnemyViewModule.OpenData openData3 = new ChapterPowerfulEnemyViewModule.OpenData(ChapterPowerfulEnemyViewModule.StyleType.Normal, null);
					GameApp.View.OpenView(ViewName.ChapterPowerfulEnemyViewModule, openData3, 1, null, null);
				}
				if (ride > 0)
				{
					await this.CreateEnemyRide(ride);
				}
				Transform transform = this.enemyParent.transform;
				if (this.enemyRide != null)
				{
					transform = this.enemyRide.pointPlayer;
					this.enemyRide.StopMove();
				}
				if (this.moveToMode == MemberEnterBattleMode.FishNpc)
				{
					if (this.enemies.Count > 0)
					{
						this.fishType = FishType.BattleFish;
						this.CreateFish(this.enemies[0]);
					}
				}
				else
				{
					int num = 0;
					for (int i = 0; i < enemyList.Count; i++)
					{
						enemyList[i].m_gameObject.transform.SetParent(transform);
						Vector3 position = enemyList[i].m_gameObject.transform.position;
						position.x += this.playerOffsetX;
						enemyList[i].m_gameObject.transform.position = position;
					}
					switch (this.moveToMode)
					{
					case MemberEnterBattleMode.CrashEnemy:
					{
						GameCameraController gameCameraController = this.cameraController;
						if (gameCameraController != null)
						{
							gameCameraController.SetFollowMode(CameraFollowType.BattleStart);
						}
						this.MoveToNpc(NpcFunction.Enemy, this.enemyMove.transform, num);
						goto IL_0441;
					}
					case MemberEnterBattleMode.EnemyAppear:
					{
						GameCameraController gameCameraController2 = this.cameraController;
						if (gameCameraController2 != null)
						{
							gameCameraController2.SetFollowMode(CameraFollowType.BattleFish);
						}
						for (int j = 0; j < enemyList.Count; j++)
						{
							enemyList[j].m_roleSpinePlayer.PlayAni("Idle_Water", true);
						}
						this.MoveToNpc(NpcFunction.Enemy, this.enemyMove.transform, num);
						goto IL_0441;
					}
					case MemberEnterBattleMode.BattleNpc:
					{
						GameCameraController gameCameraController3 = this.cameraController;
						if (gameCameraController3 != null)
						{
							gameCameraController3.SetFollowMode(CameraFollowType.BattleStart);
						}
						if (enemyList.Count > 0)
						{
							num = enemyList[0].m_memberData.m_id;
						}
						this.MoveToNpc(NpcFunction.BattleNpc, this.enemyMove.transform, num);
						goto IL_0441;
					}
					case MemberEnterBattleMode.BattleNpcAppear:
					{
						GameCameraController gameCameraController4 = this.cameraController;
						if (gameCameraController4 != null)
						{
							gameCameraController4.SetFollowMode(CameraFollowType.BattleFish);
						}
						for (int k = 0; k < enemyList.Count; k++)
						{
							enemyList[k].m_roleSpinePlayer.PlayAni("Idle_Water", true);
						}
						if (enemyList.Count > 0)
						{
							num = enemyList[0].m_memberData.m_id;
						}
						this.MoveToNpc(NpcFunction.BattleNpc, this.enemyMove.transform, num);
						goto IL_0441;
					}
					case MemberEnterBattleMode.EnemyComing:
						this.MoveToPlayer(NpcFunction.EnemyComing, this.playerMove.transform);
						goto IL_0441;
					case MemberEnterBattleMode.EnemyComingAppear:
					{
						for (int l = 0; l < enemyList.Count; l++)
						{
							enemyList[l].m_roleSpinePlayer.PlayAni("Idle_Water", true);
						}
						this.MoveToPlayer(NpcFunction.EnemyComing, this.playerMove.transform);
						goto IL_0441;
					}
					}
					HLog.LogError(string.Format("未定义的遇敌类型:{0}", (int)enterMode));
				}
				IL_0441:;
			}
		}

		private void BattleStart()
		{
			if (this.gameBattleType == GameEventBattleType.Boss)
			{
				this.gameMapData.PlayBossBgm();
			}
			else
			{
				this.gameMapData.PlayBattleBgm();
			}
			List<List<CardData>> otherWaveCards;
			this.RefreshMemberAttribute(out otherWaveCards);
			BattleFightViewModule.OpenData openData = new BattleFightViewModule.OpenData();
			openData.aniFinish = delegate
			{
				EventArgChapterBattleStart eventArgChapterBattleStart = new EventArgChapterBattleStart();
				eventArgChapterBattleStart.SetData(otherWaveCards);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_ChapterBattle_Start, eventArgChapterBattleStart);
			};
			openData.spinOffsetY = 0f;
			GameApp.View.OpenView(ViewName.BattleFightViewModule, openData, 1, null, null);
		}

		private async Task BattleEnd(GameOverType overType)
		{
			if (overType == GameOverType.Win)
			{
				await TaskExpand.Delay(1000);
				if (this.enemyRide != null)
				{
					this.enemyRide.HideAni(delegate
					{
						this.enemyRide.ResetAlpha();
						this.RemoveEnemyRide();
						this.BattleEndReset();
					});
				}
				else
				{
					this.BattleEndReset();
				}
			}
			else
			{
				this.cameraController.SetFollowActive(false);
			}
		}

		private void BattleEndReset()
		{
			this.cameraController.SetBattleEnd();
			this.cameraController.SetFollowActive(true);
			if (this.isFishBattle)
			{
				this.isFishBattle = false;
				this.ClearEnemyMoveInfo();
			}
			else
			{
				this.playerMoveSpeed = this.PlayerNormalSpeed;
				if (!this.isDropBox)
				{
					this.SetPlayerState(EventMemberController.EventPlayerState.Move);
					this.ChangePlayerMoveAni();
				}
			}
			this.gameMapData.PlayDefaultBgm();
		}

		private void ArriveEnemy()
		{
			this.isMoveToTarget = false;
			this.sceneMapController.SetNormalSpeed();
			this.SetPlayerState(EventMemberController.EventPlayerState.Idle);
			if (this.isTrack)
			{
				this.isTrack = false;
			}
			else
			{
				CMemberBase mainMember = this.memberFactory.MainMember;
				if (mainMember != null)
				{
					mainMember.PlayAnimation(MemberAnimationName.GetSelfIdleAnimationName());
				}
				this.PlayPetAnimation("Idle");
				CMemberBase mainMember2 = this.memberFactory.MainMember;
				if (((mainMember2 != null) ? mainMember2.mountSpinePlayer : null) != null)
				{
					CMemberBase mainMember3 = this.memberFactory.MainMember;
					if (mainMember3 != null)
					{
						mainMember3.PlayAnimation(MemberAnimationName.GetSelfIdleAnimationName());
					}
				}
			}
			if (this.moveToMode == MemberEnterBattleMode.BattleNpc || this.moveToMode == MemberEnterBattleMode.BattleNpcAppear || this.moveToMode == MemberEnterBattleMode.FishNpc)
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEven_ArriveNpc, null);
			}
			else
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEven_ArriveEnemy, null);
			}
			switch (this.moveToMode)
			{
			case MemberEnterBattleMode.CrashEnemy:
				if (this.playerRide != null)
				{
					this.PlayRaftAnimation();
				}
				else
				{
					this.BattleStart();
				}
				this.ClearEnemyMoveInfo();
				return;
			case MemberEnterBattleMode.EnemyAppear:
				this.PlayAppearAnimation(true);
				this.ClearEnemyMoveInfo();
				return;
			case MemberEnterBattleMode.BattleNpc:
			case MemberEnterBattleMode.FishNpc:
				break;
			case MemberEnterBattleMode.BattleNpcAppear:
				this.PlayAppearAnimation(false);
				break;
			default:
				return;
			}
		}

		private void ClearEnemyMoveInfo()
		{
			this.moveToMode = MemberEnterBattleMode.None;
			this.enemies.Clear();
		}

		private void PlayRaftAnimation()
		{
			EventArgsGameCameraShake instance = Singleton<EventArgsGameCameraShake>.Instance;
			instance.SetData(3);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_Shake, instance);
			GameApp.Sound.PlayClip(511, 1f);
			this.playerRaftAni.Play("BoatLeftImpact");
			this.enemyRaftAni.Play("BoatLeftImpact");
			int num = (int)(this.GetAnimatorLength(this.playerRaftAni, "BoatLeftImpact") * 1000f);
			DelayCall.Instance.CallOnce(num, delegate
			{
				if (this.mGameObject == null)
				{
					return;
				}
				this.playerRaftAni.Play("BoatIdle");
				this.enemyRaftAni.Play("BoatIdle");
				this.BattleStart();
			});
		}

		private void PlayAppearAnimation(bool isBattle)
		{
			float num = 0f;
			for (int i = 0; i < this.enemies.Count; i++)
			{
				float animationDuration = this.enemies[i].m_roleSpinePlayer.GetAnimationDuration("Appear");
				if (num < animationDuration)
				{
					num = animationDuration;
				}
				int appearSoundID = this.enemies[i].m_memberData.m_appearSoundID;
				GameApp.Sound.PlayClip(appearSoundID, 1f);
				this.enemies[i].m_roleSpinePlayer.PlayAni("Appear", false);
				this.enemies[i].m_roleSpinePlayer.AddAni("Idle", true);
			}
			if (isBattle)
			{
				DelayCall.Instance.CallOnce((int)(num * 1000f), delegate
				{
					if (this.mGameObject == null)
					{
						return;
					}
					this.BattleStart();
				});
			}
		}

		public void MonsterGroupLeave()
		{
			if (this.enemies.Count == 0)
			{
				return;
			}
			EventArgEventPause eventArgEventPause = new EventArgEventPause();
			eventArgEventPause.SetData(true);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EventPause, eventArgEventPause);
			MemberEnterBattleMode memberEnterBattleMode = this.moveToMode;
			if (memberEnterBattleMode != MemberEnterBattleMode.BattleNpc)
			{
				if (memberEnterBattleMode - MemberEnterBattleMode.FishNpc > 1)
				{
					return;
				}
				if (this.enemies.Count > 0)
				{
					RoleSpinePlayerBase roleSpinePlayer = this.enemies[0].m_roleSpinePlayer;
					if (roleSpinePlayer != null)
					{
						this.enemies[0].m_roleSpinePlayer.PlayAni("Death", false);
						float animationDuration = roleSpinePlayer.GetAnimationDuration("Death");
						DelayCall.Instance.CallOnce((int)(animationDuration * 1000f), delegate
						{
							if (this.mGameObject == null)
							{
								return;
							}
							this.MonsterGroupLeaveFinish();
						});
					}
				}
				return;
			}
			else
			{
				this.isBattleNpcMove = true;
				for (int i = 0; i < this.enemies.Count; i++)
				{
					this.enemies[i].m_gameObject.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
				}
				if (this.enemyRide != null)
				{
					this.enemyRide.NormalMove();
					return;
				}
				for (int j = 0; j < this.enemies.Count; j++)
				{
					if (this.enemies[j] != null)
					{
						this.enemies[j].PlayAnimation("Run");
					}
				}
				return;
			}
		}

		private void MonsterGroupLeaveFinish()
		{
			this.isBattleNpcMove = false;
			for (int i = 0; i < this.enemies.Count; i++)
			{
				CMemberFactory cmemberFactory = this.memberFactory;
				if (cmemberFactory != null)
				{
					cmemberFactory.RemoveMember(this.enemies[i]);
				}
			}
			if (this.enemyRide != null)
			{
				this.RemoveEnemyRide();
			}
			switch (this.moveToMode)
			{
			case MemberEnterBattleMode.BattleNpc:
			case MemberEnterBattleMode.BattleNpcAppear:
			{
				this.LeaveFromNpc();
				GameCameraController gameCameraController = this.cameraController;
				if (gameCameraController != null)
				{
					gameCameraController.SetBattleEnd();
				}
				break;
			}
			}
			this.ClearEnemyMoveInfo();
			EventArgEventPause eventArgEventPause = new EventArgEventPause();
			eventArgEventPause.SetData(false);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EventPause, eventArgEventPause);
		}

		public void MonsterGroupBattle()
		{
			switch (this.moveToMode)
			{
			case MemberEnterBattleMode.BattleNpc:
			case MemberEnterBattleMode.BattleNpcAppear:
				this.ClearEnemyMoveInfo();
				this.BattleStart();
				return;
			case MemberEnterBattleMode.FishNpc:
				this.isFishBattle = true;
				this.ClearEnemyMoveInfo();
				this.BattleStart();
				return;
			default:
				return;
			}
		}

		private void OnEventBattleNpcFight(object sender, int type, BaseEventArgs eventargs)
		{
			if (this.enemies.Count > 0)
			{
				this.MonsterGroupBattle();
			}
		}

		private async void OnEventBattleEnd(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsGameEnd eventArgsGameEnd = eventargs as EventArgsGameEnd;
			if (eventArgsGameEnd != null)
			{
				await this.BattleEnd(eventArgsGameEnd.m_gameOverType);
			}
		}

		public void Init_EnemyMove()
		{
			this.enemyMoveSpeed = this.EnemyNormalSpeed;
		}

		public void DeInit_EnemyMove()
		{
		}

		public void Update_EnemyMove(float deltaTime)
		{
			if (this.isEnemyComing)
			{
				this.MoveToPlayer(deltaTime);
			}
		}

		private void MoveToPlayer(float deltaTime)
		{
			Vector3 vector = Vector3.left * this.enemyMoveSpeed * deltaTime;
			this.enemyMove.position += vector;
			float num = Utility.Math.Abs(this.enemyTargetPos.x - this.enemyMove.position.x);
			if (this.enemySpeedState == 1)
			{
				float num2 = Utility.Math.Clamp01((this.enemyMoveDistance - num) / this.enemySpeedUpDistance);
				this.enemyMoveSpeed = this.EnemyNormalSpeed + (this.EnemyFastSpeed - this.EnemyNormalSpeed) * num2;
				if (num2 >= 1f)
				{
					this.enemySpeedState = 3;
				}
			}
			else if (this.enemySpeedState == 2)
			{
				float num3 = 1f - Utility.Math.Clamp01(num / (this.enemyMoveDistance - this.enemySpeedDownDistance));
				this.enemyMoveSpeed = this.EnemyFastSpeed - (this.EnemyFastSpeed - this.EnemyNormalSpeed) * num3;
				if (num3 >= 1f)
				{
					this.enemySpeedState = 0;
				}
			}
			else if (this.enemySpeedState == 3 && num <= this.enemyMoveDistance - this.enemySpeedDownDistance)
			{
				this.enemySpeedState = 2;
			}
			if (num < 5.3f || this.enemyMove.position.x < this.enemyTargetPos.x)
			{
				this.ArrivePlayer();
			}
		}

		private void MoveToPlayer(NpcFunction function, Transform targetTransform)
		{
			this.SetPlayerState(EventMemberController.EventPlayerState.Idle);
			this.enemyTargetPos = targetTransform.position;
			this.enemyMoveDistance = Utility.Math.Abs(this.enemyTargetPos.x - this.enemyMove.position.x);
			this.enemySpeedUpDistance = this.enemyMoveDistance * 0.1f;
			this.enemySpeedDownDistance = this.enemyMoveDistance * 0.9f;
			this.enemySpeedState = 1;
			if (this.enemyRide != null)
			{
				this.enemyRide.FastMove();
			}
			else
			{
				for (int i = 0; i < this.enemies.Count; i++)
				{
					if (this.enemies[i] != null)
					{
						this.enemies[i].PlayAnimation("Run");
					}
				}
			}
			this.isEnemyComing = true;
			EventArgsMoveToNpc eventArgsMoveToNpc = new EventArgsMoveToNpc();
			eventArgsMoveToNpc.SetData(function, 0);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_MoveToNpc, eventArgsMoveToNpc);
		}

		private void ArrivePlayer()
		{
			this.isEnemyComing = false;
			if (this.enemyRide != null)
			{
				this.enemyRide.StopMove();
			}
			else
			{
				for (int i = 0; i < this.enemies.Count; i++)
				{
					if (this.enemies[i] != null)
					{
						this.enemies[i].PlayAnimation("Idle");
					}
				}
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEven_ArriveEnemy, null);
			MemberEnterBattleMode memberEnterBattleMode = this.moveToMode;
			if (memberEnterBattleMode == MemberEnterBattleMode.EnemyComing)
			{
				GameCameraController gameCameraController = this.cameraController;
				if (gameCameraController != null)
				{
					gameCameraController.SetFollowMode(CameraFollowType.BattleStart);
				}
				if (this.enemyRide != null)
				{
					this.PlayRaftAnimation();
				}
				else
				{
					this.BattleStart();
				}
				this.ClearEnemyMoveInfo();
				return;
			}
			if (memberEnterBattleMode != MemberEnterBattleMode.EnemyComingAppear)
			{
				return;
			}
			GameCameraController gameCameraController2 = this.cameraController;
			if (gameCameraController2 != null)
			{
				gameCameraController2.SetFollowMode(CameraFollowType.BattleFish);
			}
			this.EnemyComingAppear();
			this.ClearEnemyMoveInfo();
		}

		private void EnemyComingAppear()
		{
			float num = 0f;
			for (int i = 0; i < this.enemies.Count; i++)
			{
				float animationDuration = this.enemies[i].m_roleSpinePlayer.GetAnimationDuration("Appear");
				if (num < animationDuration)
				{
					num = animationDuration;
				}
				if (i == 0)
				{
					this.enemies[i].m_roleSpinePlayer.PlayAni("Appear", false, new AnimationState.TrackEntryEventDelegate(this.SpineEvent));
				}
				else
				{
					this.enemies[i].m_roleSpinePlayer.PlayAni("Appear", false);
				}
				this.enemies[i].m_roleSpinePlayer.AddAni("Idle", true);
			}
			int num2 = (int)(this.GetAnimatorLength(this.playerRaftAni, "BoatLeftImpact") * 1000f);
			DelayCall.Instance.CallOnce(num2, delegate
			{
				if (this.mGameObject == null)
				{
					return;
				}
				this.playerRaftAni.Play("BoatIdle");
				this.BattleStart();
			});
		}

		private void SpineEvent(TrackEntry trackEntry, Event e)
		{
			if (e.Data.Name.Equals("Appear"))
			{
				EventArgsGameCameraShake instance = Singleton<EventArgsGameCameraShake>.Instance;
				instance.SetData(3);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_Shake, instance);
				this.playerRaftAni.Play("BoatLeftImpact");
			}
		}

		public float EnemyNormalSpeed
		{
			get
			{
				if (this.enemyRide != null)
				{
					return 5f;
				}
				return 6f;
			}
		}

		public float EnemyFastSpeed
		{
			get
			{
				this.playerRide != null;
				return 12f;
			}
		}

		private Vector3 CreateNpcPos()
		{
			float num = Utility.Math.Random(0f, 2f);
			Vector3 position = this.playerParent.transform.position;
			return new Vector3(position.x + GameConfig.GameBattle_EnemyPoint_Distance + num, position.y, 0f);
		}

		public void CreateEventPoint(GameObject prefab, Chapter_eventPoint table)
		{
			if (prefab == null || table == null)
			{
				return;
			}
			float[] array = ((this.mapType == MapType.Sea) ? table.createOffsetSea : table.createOffsetLand);
			Vector2 vector;
			vector..ctor(array[0], array[1]);
			GameObject gameObject = Object.Instantiate<GameObject>(prefab);
			gameObject.SetParentNormal(this.npcParent, false);
			Vector3 vector2 = this.CreateNpcPos();
			gameObject.transform.position = new Vector3(vector2.x + vector.x, vector2.y + vector.y);
			this.actionNormal = gameObject.GetComponent<EventPointActionNormal>();
			if (this.actionNormal)
			{
				this.actionNormal.SetData(table);
				this.actionNormal.Init();
				this.SetTime(this.actionNormal);
			}
			this.MoveToNpc(NpcFunction.Normal, gameObject.transform, 0);
		}

		public void CreateEventPointBox(GameObject prefab, Chapter_eventPoint table)
		{
			if (prefab == null || table == null)
			{
				return;
			}
			float[] array = ((this.mapType == MapType.Sea) ? table.createOffsetSea : table.createOffsetLand);
			Vector2 vector;
			vector..ctor(array[0], array[1]);
			GameObject gameObject = Object.Instantiate<GameObject>(prefab);
			gameObject.SetParentNormal(this.pointParent, false);
			gameObject.transform.localPosition = vector;
			this.SetPlayerState(EventMemberController.EventPlayerState.Idle);
			this.actionNormal = gameObject.GetComponent<EventPointActionNormal>();
			if (this.actionNormal)
			{
				this.actionNormal.SetData(table);
				this.actionNormal.Init();
			}
		}

		public async Task DoEventPoint(int actionId)
		{
			if (this.actionNormal != null)
			{
				float num;
				this.actionNormal.OnAction(actionId, out num);
				await TaskExpand.Delay((int)(num * 1000f));
			}
		}

		public async Task RemoveBox()
		{
			await this.DoEventPoint(3);
			this.SetPlayerState(EventMemberController.EventPlayerState.Move);
			this.ChangePlayerMoveAni();
			if (this.actionNormal)
			{
				Object.DestroyImmediate(this.actionNormal.gameObject);
				this.actionNormal = null;
			}
		}

		private void Init_Fishing()
		{
			this.fishAnimation = new BezierCurveAnimation();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_Fishing_Ready, new HandlerEvent(this.OnEventFishingReady));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_Fishing_Enter, new HandlerEvent(this.OnEventFishingEnter));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_Fishing_Exit, new HandlerEvent(this.OnEventFishingExit));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_Fishing_AllFinish, new HandlerEvent(this.OnEventFishingAllFinish));
		}

		private void DeInit_Fishing()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_Fishing_Ready, new HandlerEvent(this.OnEventFishingReady));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_Fishing_Enter, new HandlerEvent(this.OnEventFishingEnter));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_Fishing_Exit, new HandlerEvent(this.OnEventFishingExit));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_Fishing_AllFinish, new HandlerEvent(this.OnEventFishingAllFinish));
		}

		private void Update_Fishing(float deltaTime)
		{
			if (this.fishAnimation != null)
			{
				this.fishAnimation.Update();
			}
		}

		private void OnPause_Fishing(bool pause)
		{
			if (this.fishAnimation != null)
			{
				this.fishAnimation.SetPause(pause);
			}
		}

		private void PlayFishingAnim()
		{
			RoleSpinePlayerBase roleSpinePlayer = this.memberFactory.MainMember.m_roleSpinePlayer;
			if (!(roleSpinePlayer != null))
			{
				this.FishingSuccess();
				return;
			}
			string aniName = "Other/Fish_End02";
			if (this.fishType == FishType.SmallFish)
			{
				aniName = "Other/Fish_End01";
			}
			float animationDuration = roleSpinePlayer.GetAnimationDuration("Other/Fish_Begin");
			float animationDuration2 = roleSpinePlayer.GetAnimationDuration(aniName);
			if (animationDuration > 0f && animationDuration2 > 0f)
			{
				CMemberBase mainMember = this.memberFactory.MainMember;
				roleSpinePlayer.PlayAni("Other/Fish_Begin", false);
				roleSpinePlayer.AddAni("Other/Fish_Loop", true);
				int num = (int)((animationDuration + 2f) * 1000f);
				float num2 = 0.625f;
				int fishDelay = (int)(animationDuration2 * 1000f * num2);
				DelayCall.Instance.CallOnce(num, delegate
				{
					if (this.mGameObject == null)
					{
						return;
					}
					roleSpinePlayer.PlayAni(aniName, false);
					roleSpinePlayer.AddAni("Idle", true);
					DelayCall.Instance.CallOnce(fishDelay, new DelayCall.CallAction(this.ShowFish));
				});
				return;
			}
			this.FishingSuccess();
		}

		private void AddFishMember()
		{
			switch (this.fishType)
			{
			case FishType.SmallFish:
				this.EventAddNpc(this.fishId, NpcFunction.SmallFish, 0);
				return;
			case FishType.BigFish:
				this.EventAddNpc(this.fishId, NpcFunction.BigFish, 0);
				return;
			case FishType.BattleFish:
				this.EventAddMonsterGroupMember(this.fishId, MemberEnterBattleMode.FishNpc, this.atkUpgrade, this.hpUpgrade);
				return;
			default:
				return;
			}
		}

		private void CreateFish(CMemberBase member)
		{
			int npcFunction = member.m_memberData.m_npcFunction;
			GameMember_npcFunction elementById = GameApp.Table.GetManager().GetGameMember_npcFunctionModelInstance().GetElementById(npcFunction);
			if (elementById == null)
			{
				HLog.LogError(string.Format("[GameMember_npcFunction] not found id= {0}, memberId={1}", npcFunction, member.m_memberData.m_id));
				return;
			}
			if (elementById.fishingStartPos.Length > 1 && elementById.fishingEndPos.Length > 1)
			{
				this.fishMember = member;
				this.fishMember.m_roleSpinePlayer.SetOrderLayer(1);
				member.m_gameObject.SetParentNormal(this.fishParent, false);
				this.fishParent.transform.localPosition = new Vector3(elementById.fishingStartPos[0], elementById.fishingStartPos[1]);
				this.fishEndPos = new Vector3(elementById.fishingEndPos[0], elementById.fishingEndPos[1]);
				member.m_gameObject.SetActiveSafe(false);
				return;
			}
			HLog.LogError("[NpcFunction]表的fishingStartPos或fishingEndPos的长度错误");
		}

		private void ShowFish()
		{
			if (this.fishMember != null)
			{
				this.fishMember.m_gameObject.SetActiveSafe(true);
				FishType fishType = this.fishType;
				if (fishType != FishType.SmallFish)
				{
					if (fishType - FishType.BigFish > 1)
					{
						return;
					}
					this.fishMember.m_gameObject.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
					string text = "Appear";
					if (this.fishMember.m_roleSpinePlayer != null && this.fishMember.m_roleSpinePlayer.IsHaveAni(text))
					{
						this.fishMember.m_roleSpinePlayer.PlayAni(text, false);
						int num = (int)((this.fishMember.m_roleSpinePlayer.GetAnimationDuration(text) + 0.5f) * 1000f);
						DelayCall.Instance.CallOnce(num, delegate
						{
							if (this.mGameObject == null)
							{
								return;
							}
							this.FishingSuccess();
						});
						return;
					}
					this.FishingSuccess();
				}
				else if (this.fishAnimation != null && this.fishParent != null)
				{
					this.fishAnimation.StartAni(this.fishParent.transform, this.fishParent.transform.localPosition, this.fishEndPos, 2.5f, 0.5f, delegate
					{
						DelayCall.Instance.CallOnce(500, delegate
						{
							if (this.mGameObject == null)
							{
								return;
							}
							this.FishingSuccess();
						});
					});
					return;
				}
			}
		}

		private void FishingSuccess()
		{
			if (this.fishMember != null && this.fishType == FishType.SmallFish)
			{
				this.fishMember.m_gameObject.transform.SetParent(this.playerParent.transform);
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_Fishing_Sucess, null);
		}

		private void OnEventFishingReady(object sender, int type, BaseEventArgs eventargs)
		{
			this.SetPlayerState(EventMemberController.EventPlayerState.Fishing);
		}

		private void OnEventFishingEnter(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgFishing eventArgFishing = eventArgs as EventArgFishing;
			if (eventArgFishing != null)
			{
				this.fishId = eventArgFishing.npcId;
				this.fishType = eventArgFishing.fishType;
				this.atkUpgrade = eventArgFishing.atkUpgrade;
				this.hpUpgrade = eventArgFishing.hpUpgrade;
				this.AddFishMember();
				this.PlayFishingAnim();
			}
		}

		private void OnEventFishingExit(object sender, int type, BaseEventArgs eventargs)
		{
			if (this.fishMember != null)
			{
				this.fishId = 0;
				RoleSpinePlayerBase roleSpinePlayer = this.memberFactory.MainMember.m_roleSpinePlayer;
				if (roleSpinePlayer != null)
				{
					roleSpinePlayer.PlayAni("Idle", true);
					CMemberBase mainMember = this.memberFactory.MainMember;
				}
				this.fishMember.PlayAlpha(delegate
				{
					CMemberFactory cmemberFactory = this.memberFactory;
					if (cmemberFactory != null)
					{
						cmemberFactory.RemoveMember(this.fishMember);
					}
					this.fishMember = null;
				});
			}
		}

		private void OnEventFishingAllFinish(object sender, int type, BaseEventArgs eventargs)
		{
			this.SetPlayerState(EventMemberController.EventPlayerState.Move);
		}

		private void Init_FollowingNpc()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_EnterNewStage, new HandlerEvent(this.OnEventNewStage));
		}

		private void DeInit_FollowingNpc()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_EnterNewStage, new HandlerEvent(this.OnEventNewStage));
			this.followMember = null;
		}

		private void Update_FollowingNpc(float deltaTime)
		{
			if (this.isCatchUp)
			{
				this.CatchUpPlayer(deltaTime);
			}
			FishFollowCtrl fishFollowCtrl = this.fishFollowCtrl;
			if (fishFollowCtrl == null)
			{
				return;
			}
			fishFollowCtrl.Update(deltaTime);
		}

		public void FollowPlayer(CMemberBase npc, int stage, GameMember_npcFunction funcTable)
		{
			if (funcTable.followOffset.Length > 1)
			{
				if (this.followMember != null)
				{
					this.RemoveFollowNpc();
				}
				this.followMember = npc;
				this.followStage = stage;
				this.followMemberTrans = this.followMember.m_gameObject.transform;
				this.followMember.m_roleSpinePlayer.SetSortingLayer("BackGround");
				this.followMember.m_roleSpinePlayer.SetOrderLayer(-76);
				npc.m_gameObject.SetParentNormal(this.playerMove, false);
				this.catchUpPos = new Vector3(funcTable.followOffset[0], funcTable.followOffset[1], 0f);
				npc.m_gameObject.transform.localPosition = new Vector3(this.catchUpPos.x - 8f, this.catchUpPos.y, this.catchUpPos.z);
				npc.m_roleSpinePlayer.SetOrderLayer(-3);
				this.isCatchUp = true;
				return;
			}
			HLog.LogError("[NpcFunction]表的followOffset的长度错误");
		}

		private void CatchUpPlayer(float deltaTime)
		{
			if (this.followMember != null)
			{
				float num = this.playerMoveSpeed * 5f;
				Vector3 vector = Vector3.right * num * deltaTime;
				this.followMemberTrans.localPosition += vector;
				if (Utility.Math.Abs(this.catchUpPos.x - this.followMemberTrans.localPosition.x) <= 0.1f)
				{
					this.followMemberTrans.localPosition = this.catchUpPos;
					this.isCatchUp = false;
					if (this.fishFollowCtrl == null)
					{
						this.fishFollowCtrl = new FishFollowCtrl();
					}
					else
					{
						this.fishFollowCtrl.DeInit();
					}
					this.fishFollowCtrl.Init(this.followMember.m_gameObject, this.memberFactory.MainMember.m_gameObject.transform);
				}
			}
		}

		private void RemoveFollowNpc()
		{
			if (this.followMember != null)
			{
				this.followMemberTrans.SetParent(this.npcParent.transform);
				this.waitRemoveMembers.Add(this.followMember);
				this.followMember = null;
			}
		}

		private void OnEventNewStage(object sender, int type, BaseEventArgs eventargs)
		{
			if (this.followMember != null)
			{
				this.followStage--;
				if (this.followStage < 0)
				{
					this.RemoveFollowNpc();
				}
			}
		}

		public List<int> CurrentMonsterList { get; private set; }

		private void Init_Member()
		{
			this.idCreator = new IDCreator(100);
		}

		private void DeInit_Member()
		{
			this.idCreator = null;
		}

		private void RefreshMemberAttribute(out List<List<CardData>> otherWaveCards)
		{
			List<int> playerSkills = Singleton<GameEventController>.Instance.PlayerData.GetPlayerSkills();
			MemberAttributeData battleAttribute = Singleton<GameEventController>.Instance.PlayerData.GetBattleAttribute();
			FP currentHp = Singleton<GameEventController>.Instance.PlayerData.CurrentHp;
			List<CardData> list = new List<CardData>();
			CardData cardData = new CardData();
			cardData.m_instanceID = this.memberFactory.MainMember.InstanceID;
			cardData.m_memberID = this.memberFactory.MainMember.m_memberData.m_id;
			cardData.m_camp = MemberCamp.Friendly;
			cardData.m_posIndex = MemberPos.One;
			cardData.m_curHp = currentHp;
			cardData.m_memberAttributeData.CopyFrom(battleAttribute);
			cardData.AddSkill(playerSkills);
			cardData.m_isMainMember = true;
			int currentStage = Singleton<GameEventController>.Instance.GetCurrentStage();
			List<CardData> list2 = new List<CardData>();
			otherWaveCards = new List<List<CardData>>();
			if (Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Chapter)
			{
				ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
				ChapterController.GetChapterBattleEnemy(GameApp.Table.GetManager(), dataModule.CurrentChapter.id, currentStage, this.CurrentMonsterList, dataModule.ChapterBattleTimes, out list2, out otherWaveCards);
			}
			list.Add(cardData);
			list.AddRange(Singleton<GameEventController>.Instance.PlayerData.PetCards);
			list.AddRange(list2);
			this.memberFactory.RefreshMemberCardData(list);
		}

		public async Task CreateMembers()
		{
			await Task.WhenAll(new List<Task>
			{
				this.CreatePlayer(),
				this.CreatePets()
			});
		}

		private async Task CreatePlayer()
		{
			TaskOutValue<CMemberBase> outMember = new TaskOutValue<CMemberBase>();
			CMemberBase player = null;
			List<int> playerSkills = Singleton<GameEventController>.Instance.PlayerData.GetPlayerSkills();
			await this.CreateMember(100, Singleton<GameEventController>.Instance.PlayerData.PlayerMemberId, MemberCamp.Friendly, true, MemberPos.One, this.playerParent.transform, -1, -1, outMember, playerSkills);
			if (outMember.Value != null)
			{
				player = outMember.Value;
				this.SetPlayRide(player);
			}
			ModelUtils.CreateBattlePlayerMountModel(player);
		}

		private async Task CreatePets()
		{
			List<CardData> petCards = Singleton<GameEventController>.Instance.PlayerData.PetCards;
			for (int i = 0; i < petCards.Count; i++)
			{
				CardData card = petCards[i];
				TaskOutValue<CMemberBase> outPet = new TaskOutValue<CMemberBase>();
				await this.CreateMember(card.m_instanceID, card.m_memberID, card.m_camp, card.m_isMainMember, card.m_posIndex, this.playerParent.transform, -1, -1, outPet, null);
				if (outPet.Value != null)
				{
					outPet.Value.m_memberData.cardData.CloneFrom(card);
					this.SetPetRide(outPet.Value, i);
				}
				card = null;
				outPet = null;
			}
		}

		public async Task CreateMember(int GUID, int memberID, MemberCamp camp, bool isMainMember, MemberPos posIndex, Transform parent, FP curHp, FP maxHp, List<int> skillIds)
		{
			await this.CreateMember(GUID, memberID, camp, isMainMember, posIndex, parent, curHp, maxHp, null, skillIds);
		}

		public async Task CreateMember(int GUID, int memberID, MemberCamp camp, bool isMainMember, MemberPos posIndex, Transform parent, FP curHp, FP maxHp, TaskOutValue<CMemberBase> outMember, List<int> skillIds)
		{
			GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(memberID);
			if (elementById == null)
			{
				HLog.LogError(string.Format("Not found member id={0}", memberID));
			}
			else
			{
				CMemberData cmemberData = new CMemberData();
				cmemberData.SetTableData(elementById);
				cmemberData.SetMemberData(GUID, camp, isMainMember, posIndex, curHp, maxHp);
				if (skillIds != null && skillIds.Count > 0)
				{
					cmemberData.SetSkills(skillIds);
				}
				if (camp == MemberCamp.Friendly)
				{
					cmemberData.cardData.SetMemberRace(isMainMember ? MemberRace.Hero : MemberRace.Pet);
				}
				else
				{
					cmemberData.cardData.SetMemberRace(MemberRace.Hero);
				}
				ClientPointData pointByIndex = this.clientPointController.GetPointByIndex(cmemberData.Camp, cmemberData.PosIndex);
				Vector3 position = pointByIndex.GetPosition();
				await this.memberFactory.CreateMember(position, cmemberData, pointByIndex, parent);
				CMemberBase member = this.memberFactory.GetMember(GUID);
				if (outMember != null && member != null)
				{
					this.SetTime(member);
					outMember.SetValue(member);
				}
			}
		}

		public async Task CreateMember(CardData cardData, Transform parent, TaskOutValue<CMemberBase> outMember, List<int> skillIds)
		{
			if (cardData != null)
			{
				GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(cardData.m_memberID);
				if (elementById != null)
				{
					CMemberData cmemberData = new CMemberData(cardData);
					cmemberData.SetTableData(elementById);
					if (cardData.m_camp == MemberCamp.Friendly)
					{
						cmemberData.cardData.SetMemberRace(cardData.m_isMainMember ? MemberRace.Hero : MemberRace.Pet);
					}
					else
					{
						cmemberData.cardData.SetMemberRace(MemberRace.Hero);
					}
					ClientPointData pointByIndex = this.clientPointController.GetPointByIndex(cardData.m_camp, cardData.m_posIndex);
					Vector3 position = pointByIndex.GetPosition();
					await this.memberFactory.CreateMember(position, cmemberData, pointByIndex, parent);
					CMemberBase member = this.memberFactory.GetMember(cardData.m_instanceID);
					if (outMember != null && member != null)
					{
						this.SetTime(member);
						outMember.SetValue(member);
					}
				}
			}
		}

		public async Task EventAddEnemy(List<int> monsterCfgIdList)
		{
			this.CurrentMonsterList = monsterCfgIdList;
			if (monsterCfgIdList.Count > 0)
			{
				int monsterCfgId = monsterCfgIdList[0];
				EventArgEventPause arg = new EventArgEventPause();
				arg.SetData(true);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EventPause, arg);
				TaskOutValue<List<CMemberBase>> outList = new TaskOutValue<List<CMemberBase>>();
				await this.CreateMonsterGroup(monsterCfgId, outList);
				this.PreloadWaveMonsterGroup(monsterCfgIdList, 1000);
				int num = 0;
				MemberEnterBattleMode memberEnterBattleMode = MemberEnterBattleMode.CrashEnemy;
				GameEventBattleType gameEventBattleType = GameEventBattleType.Normal;
				int num2 = 0;
				MonsterCfg_monsterCfg monsterCfg_monsterCfg = GameApp.Table.GetManager().GetMonsterCfg_monsterCfg(monsterCfgId);
				if (monsterCfg_monsterCfg != null)
				{
					num = monsterCfg_monsterCfg.ride;
					memberEnterBattleMode = (MemberEnterBattleMode)monsterCfg_monsterCfg.enterBattleMode;
					gameEventBattleType = (GameEventBattleType)monsterCfg_monsterCfg.battleType;
					num2 = monsterCfg_monsterCfg.isDropBox;
				}
				else
				{
					HLog.LogError(string.Format("Table Chapter_monsterCfg not found id ={0}", monsterCfgId));
				}
				if (outList.Value != null && outList.Value.Count > 0)
				{
					await this.DiscoverEnemy(outList.Value, num, memberEnterBattleMode, gameEventBattleType, num2);
				}
				arg.SetData(false);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EventPause, arg);
			}
		}

		public async Task EventAddNpc(int npcId, NpcFunction function, int stage)
		{
			EventArgEventPause arg = new EventArgEventPause();
			arg.SetData(true);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EventPause, arg);
			ClientPointController clientPointController = this.clientPointController;
			if (clientPointController != null)
			{
				clientPointController.CreateNextPoint();
			}
			TaskOutValue<CMemberBase> outMember = new TaskOutValue<CMemberBase>();
			Transform transform = this.GetNpcParent();
			await this.CreateMember(200, npcId, MemberCamp.Enemy, false, MemberPos.One, transform, -1, -1, outMember, null);
			CMemberBase value = outMember.Value;
			if (value != null)
			{
				await this.DoNpcFunction(function, value, stage);
			}
			else
			{
				HLog.LogError(string.Format("Add npc error id={0}", npcId));
			}
			arg.SetData(false);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EventPause, arg);
		}

		public async Task EventAddMonsterGroup(int monsterCfgId, int atk, int hp)
		{
			this.CurrentMonsterList = new List<int> { monsterCfgId };
			EventArgEventPause arg = new EventArgEventPause();
			arg.SetData(true);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EventPause, arg);
			TaskOutValue<List<CMemberBase>> outList = new TaskOutValue<List<CMemberBase>>();
			await this.CreateMonsterGroup(monsterCfgId, outList);
			int num = 0;
			MemberEnterBattleMode memberEnterBattleMode = MemberEnterBattleMode.CrashEnemy;
			GameEventBattleType gameEventBattleType = GameEventBattleType.Normal;
			int num2 = 0;
			MonsterCfg_monsterCfg monsterCfg_monsterCfg = GameApp.Table.GetManager().GetMonsterCfg_monsterCfg(monsterCfgId);
			if (monsterCfg_monsterCfg != null)
			{
				num = monsterCfg_monsterCfg.ride;
				memberEnterBattleMode = (MemberEnterBattleMode)monsterCfg_monsterCfg.enterBattleMode;
				gameEventBattleType = (GameEventBattleType)monsterCfg_monsterCfg.battleType;
				num2 = monsterCfg_monsterCfg.isDropBox;
			}
			else
			{
				HLog.LogError(string.Format("Table Chapter_monsterCfg not found id={0}", monsterCfgId));
			}
			if (outList.Value != null && outList.Value.Count > 0)
			{
				await this.DiscoverEnemy(outList.Value, num, memberEnterBattleMode, gameEventBattleType, num2);
			}
			arg.SetData(false);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EventPause, arg);
		}

		private async Task CreateMonsterGroup(int monsterCfgId, TaskOutValue<List<CMemberBase>> outList)
		{
			await this.CreateMonsterGroup(monsterCfgId, 0, outList);
		}

		private async Task PreloadWaveMonsterGroup(List<int> monsterCfgIds, int delay)
		{
			if (monsterCfgIds != null && monsterCfgIds.Count > 1)
			{
				await TaskExpand.Delay(delay);
				for (int i = 1; i < monsterCfgIds.Count; i++)
				{
					List<Task> list = new List<Task>();
					int num = monsterCfgIds[i];
					MonsterCfg_monsterCfg monsterCfg_monsterCfg = GameApp.Table.GetManager().GetMonsterCfg_monsterCfg(num);
					List<int> list2 = new List<int>();
					if (monsterCfg_monsterCfg.pos1 > 0)
					{
						list2.Add(monsterCfg_monsterCfg.pos1);
					}
					if (monsterCfg_monsterCfg.pos2 > 0)
					{
						list2.Add(monsterCfg_monsterCfg.pos2);
					}
					if (monsterCfg_monsterCfg.pos3 > 0)
					{
						list2.Add(monsterCfg_monsterCfg.pos3);
					}
					if (monsterCfg_monsterCfg.pos4 > 0)
					{
						list2.Add(monsterCfg_monsterCfg.pos4);
					}
					if (monsterCfg_monsterCfg.pos5 > 0)
					{
						list2.Add(monsterCfg_monsterCfg.pos5);
					}
					for (int j = 0; j < list2.Count; j++)
					{
						int num2 = list2[j];
						GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(num2);
						if (elementById == null)
						{
							HLog.LogError(string.Format("CreateMember .. GameArt_memberModel is Error ..memberId is {0}", num2));
							return;
						}
						ArtMember_member elementById2 = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(elementById.modelID);
						if (elementById2 == null)
						{
							HLog.LogError(string.Format("CreateMember .. GameArt_memberModel is Error ..modelID is {0}", num2));
							return;
						}
						string path = elementById2.path;
						list.Add(PoolManager.Instance.Cache(path));
					}
					await Task.WhenAll(list);
				}
			}
		}

		private async Task CreateMonsterGroup(int monsterCfgId, int waveIndex, TaskOutValue<List<CMemberBase>> outList)
		{
			if (outList != null)
			{
				List<CMemberBase> list = new List<CMemberBase>();
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				MonsterCfg_monsterCfg monsterCfg_monsterCfg = GameApp.Table.GetManager().GetMonsterCfg_monsterCfg(monsterCfgId);
				if (monsterCfg_monsterCfg != null)
				{
					num = monsterCfg_monsterCfg.pos1;
					num2 = monsterCfg_monsterCfg.pos2;
					num3 = monsterCfg_monsterCfg.pos3;
					num4 = monsterCfg_monsterCfg.pos4;
					num5 = monsterCfg_monsterCfg.pos5;
				}
				else
				{
					HLog.LogError(string.Format("Table Chapter_monsterCfg not found id ={0}", monsterCfgId));
				}
				if (waveIndex == 0)
				{
					ClientPointController clientPointController = this.clientPointController;
					if (clientPointController != null)
					{
						clientPointController.CreateNextPoint();
					}
				}
				else
				{
					ClientPointController clientPointController2 = this.clientPointController;
					if (clientPointController2 != null)
					{
						clientPointController2.CreateWavePoint();
					}
				}
				Dictionary<int, int> dic = new Dictionary<int, int>();
				if (num != 0)
				{
					dic.Add(0, num);
				}
				if (num2 != 0)
				{
					dic.Add(1, num2);
				}
				if (num3 != 0)
				{
					dic.Add(2, num3);
				}
				if (num4 != 0)
				{
					dic.Add(3, num4);
				}
				if (num5 != 0)
				{
					dic.Add(4, num5);
				}
				int monsterInstanceIndex = 0;
				foreach (int num6 in dic.Keys)
				{
					MemberPos memberPos = (MemberPos)num6;
					int num7 = dic[num6];
					TaskOutValue<CMemberBase> outMember = new TaskOutValue<CMemberBase>();
					Transform transform = this.GetNpcParent();
					await this.CreateMember(200 + waveIndex * 10 + monsterInstanceIndex, num7, MemberCamp.Enemy, false, memberPos, transform, -1, -1, outMember, null);
					monsterInstanceIndex++;
					CMemberBase value = outMember.Value;
					if (value != null)
					{
						list.Add(value);
					}
					outMember = null;
				}
				Dictionary<int, int>.KeyCollection.Enumerator enumerator = default(Dictionary<int, int>.KeyCollection.Enumerator);
				outList.SetValue(list);
			}
		}

		public async Task EventAddMonsterGroupMember(int memberId, MemberEnterBattleMode mode, int atk, int hp)
		{
			EventArgEventPause arg = new EventArgEventPause();
			arg.SetData(true);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EventPause, arg);
			ClientPointController clientPointController = this.clientPointController;
			if (clientPointController != null)
			{
				clientPointController.CreateNextPoint();
			}
			TaskOutValue<CMemberBase> outMember = new TaskOutValue<CMemberBase>();
			Transform transform = this.GetNpcParent();
			await this.CreateMember(200, memberId, MemberCamp.Enemy, false, MemberPos.One, transform, -1, -1, outMember, null);
			CMemberBase value = outMember.Value;
			if (value != null)
			{
				await this.DiscoverEnemy(new List<CMemberBase> { value }, 0, mode, GameEventBattleType.Npc, 0);
			}
			else
			{
				HLog.LogError(string.Format("Add monster group error id={0}", memberId));
			}
			arg.SetData(false);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_EventPause, arg);
		}

		private void Update_PassingNpc(float deltaTime)
		{
			if (this.passingMember != null)
			{
				Vector3 vector = Vector3.left * 1f * deltaTime;
				this.passingMember.m_gameObject.transform.position += vector;
				float num = Vector3.Distance(this.passingMember.m_gameObject.transform.position, this.playerMove.position);
				if (this.passingMember.m_gameObject.transform.position.x < this.playerMove.position.x && num > 15f)
				{
					this.waitRemoveMembers.Add(this.passingMember);
					this.passingMember = null;
				}
			}
		}

		public void CreatePassingMember(CMemberBase npc)
		{
			this.passingMember = npc;
			this.passingMember.m_roleSpinePlayer.SetSortingLayer("BackGround");
			this.passingMember.m_roleSpinePlayer.SetOrderLayer(-76);
		}

		private void Init_PlayerAction()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_PlayMainMemberAni, new HandlerEvent(this.OnEventPlayerAnimation));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ShowEmoticons, new HandlerEvent(this.OnEventShowEmoticons));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Battle_EventSelectSkill, new HandlerEvent(this.OnEventSelectSkills));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_GetEventItem, new HandlerEvent(this.OnEventGetEventItem));
		}

		private void DeInit_PlayerAction()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_PlayMainMemberAni, new HandlerEvent(this.OnEventPlayerAnimation));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ShowEmoticons, new HandlerEvent(this.OnEventShowEmoticons));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Battle_EventSelectSkill, new HandlerEvent(this.OnEventSelectSkills));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_GetEventItem, new HandlerEvent(this.OnEventGetEventItem));
		}

		private void OnEventPlayerAnimation(object sender, int type, BaseEventArgs args)
		{
			EventArgsEventPlayMemebrAni eventArgsEventPlayMemebrAni = args as EventArgsEventPlayMemebrAni;
			if (eventArgsEventPlayMemebrAni != null)
			{
				CMemberBase mainMember = this.memberFactory.MainMember;
				if (mainMember != null)
				{
					this.PlayMainMemberAni(mainMember, eventArgsEventPlayMemebrAni.aniName);
				}
			}
		}

		private void OnEventShowEmoticons(object sender, int type, BaseEventArgs args)
		{
			EventArgsString eventArgsString = args as EventArgsString;
			if (eventArgsString != null)
			{
				EventArgsAddHover eventArgsAddHover = new EventArgsAddHover();
				HoverData hoverData = this.CreateHoverData();
				eventArgsAddHover.SetData(HoverType.Emoticons, hoverData, eventArgsString.Value);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_AddHover, eventArgsAddHover);
			}
		}

		private void OnEventSelectSkills(object sender, int type, BaseEventArgs args)
		{
			EventArgsEventSelectSkill eventArgsEventSelectSkill = args as EventArgsEventSelectSkill;
			if (eventArgsEventSelectSkill != null)
			{
				CMemberBase mainMember = this.memberFactory.MainMember;
				if (mainMember != null)
				{
					for (int i = 0; i < eventArgsEventSelectSkill.skills.Count; i++)
					{
						EventArgsAddHover instance = Singleton<EventArgsAddHover>.Instance;
						HoverData hoverData = this.CreateHoverData();
						instance.SetData(HoverType.GetSkill, hoverData, eventArgsEventSelectSkill.skillBuild);
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_AddHover, instance);
					}
					this.PlayMainMemberAni(mainMember, "Battle/Cast");
				}
			}
		}

		private void OnEventGetEventItem(object sender, int type, BaseEventArgs args)
		{
			EventArgGetEventItemFly eventArgGetEventItemFly = args as EventArgGetEventItemFly;
			if (eventArgGetEventItemFly != null && eventArgGetEventItemFly.item != null && this.memberFactory.MainMember != null)
			{
				Vector3 position = GameApp.View.GetViewModule(ViewName.GameEventViewModule).eventItemController.flyNode.transform.position;
				EventArgsAddHover instance = Singleton<EventArgsAddHover>.Instance;
				HoverData hoverData = this.CreateHoverData();
				instance.SetData(HoverType.GetItem, hoverData, eventArgGetEventItemFly.item, position);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_AddHover, instance);
			}
		}

		private void PlayMainMemberAni(CMemberBase member, string aniName)
		{
			if (member == null)
			{
				return;
			}
			EventArgsBool arg = new EventArgsBool();
			arg.SetData(true);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_SetSpecialAni, arg);
			bool isStop = false;
			if (this.playerState == EventMemberController.EventPlayerState.Move && this.playerRide == null)
			{
				this.SetPlayerState(EventMemberController.EventPlayerState.Idle);
				isStop = true;
			}
			float animationDuration = member.m_roleSpinePlayer.GetAnimationDuration(aniName);
			member.m_roleSpinePlayer.PlayAni(aniName, false);
			DelayCall.Instance.CallOnce((int)(animationDuration * 1000f), delegate
			{
				if (this.mGameObject == null)
				{
					return;
				}
				member.m_roleSpinePlayer.PlayAni(MemberAnimationName.GetSelfIdleAnimationName(), true);
				arg.SetData(false);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_SetSpecialAni, arg);
				if (isStop || this.playerState == EventMemberController.EventPlayerState.Move)
				{
					this.SetPlayerState(EventMemberController.EventPlayerState.Move);
					this.ChangePlayerMoveAni();
				}
			});
		}

		private HoverData CreateHoverData()
		{
			CMemberBase mainMember = this.memberFactory.MainMember;
			return new HoverData(mainMember.InstanceID, mainMember.m_body.m_headTop, mainMember.m_memberData.Camp);
		}

		private void PlayPetAnimation(string aniName)
		{
			List<CMemberBase> petMembers = this.memberFactory.GetPetMembers();
			for (int i = 0; i < petMembers.Count; i++)
			{
				petMembers[i].PlayAnimation(aniName);
			}
		}

		private void Init_PlayerMove()
		{
			this.mapWeatherCtrl = new MapWeatherCtrl();
			this.mapWeatherCtrl.Init(this.weatherObj);
			this.playerMoveSpeed = this.PlayerNormalSpeed;
			this.SetPlayerState(EventMemberController.EventPlayerState.Move);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_EventGroup_End, new HandlerEvent(this.OnEventGroupEnd));
		}

		private void DeInit_PlayerMove()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_EventGroup_End, new HandlerEvent(this.OnEventGroupEnd));
			this.mapWeatherCtrl.DeInit();
			this.mapWeatherCtrl = null;
			this.sequencePool.Clear(false);
		}

		private void Update_PlayerMove(float deltaTime)
		{
			if (this.isPauseMove)
			{
				return;
			}
			if (this.playerState != EventMemberController.EventPlayerState.Move)
			{
				return;
			}
			Vector3 vector = Vector3.right * this.playerMoveSpeed * deltaTime;
			this.playerMove.position += vector;
			if (this.playerMove.position.x > 1000f)
			{
				this.ResetWordPosition();
			}
			if (this.isMoveToTarget)
			{
				this.MoveToTarget();
			}
		}

		public void StartMove()
		{
			this.ChangePlayerMoveAni();
			this.PauseMove(false);
		}

		public void PauseMove(bool pause)
		{
			this.isPauseMove = pause;
		}

		public Vector3 GetCurrentPos()
		{
			return this.playerMove.position;
		}

		private void MoveToTarget()
		{
			float num = Utility.Math.Abs(this.moveToTargetPos.x - this.playerMove.position.x);
			if (this.speedState == 1)
			{
				float num2 = Utility.Math.Clamp01((this.moveDistance - num) / this.speedUpDistance);
				this.playerMoveSpeed = this.PlayerNormalSpeed + (this.PlayerFastSpeed - this.PlayerNormalSpeed) * num2;
				if (num2 >= 1f)
				{
					this.speedState = 3;
				}
			}
			else if (this.speedState == 2)
			{
				float num3 = 1f - Utility.Math.Clamp01(num / (this.moveDistance - this.speedDownDistance));
				this.playerMoveSpeed = this.PlayerFastSpeed - (this.PlayerFastSpeed - this.PlayerNormalSpeed) * num3;
				if (num3 >= 1f)
				{
					this.speedState = 0;
				}
			}
			else if (this.speedState == 3 && num <= this.moveDistance - this.speedDownDistance)
			{
				this.speedState = 2;
			}
			if (this.moveToMode == MemberEnterBattleMode.None)
			{
				if (num < 3.5f)
				{
					this.Tackle(false);
				}
				if (num < 0.1f || this.playerMove.position.x > this.moveToTargetPos.x)
				{
					this.ArriveNpc();
					return;
				}
			}
			else
			{
				if (num < 8.8f)
				{
					this.Tackle(true);
				}
				if (num < 5.3f || this.playerMove.position.x > this.moveToTargetPos.x)
				{
					this.ArriveEnemy();
				}
			}
		}

		private void MoveToNpc(NpcFunction function, CMemberBase currentMember)
		{
			if (currentMember != null && !this.isMoveToTarget)
			{
				this.moveToMember = currentMember;
				this.MoveToNpc(function, currentMember.m_gameObject.transform, currentMember.m_memberData.m_id);
			}
		}

		private void MoveToNpc(NpcFunction function, Transform targetTransform, int memberId)
		{
			this.sceneMapController.SetFastSpeed();
			this.ChangePlayerFastMoveAni();
			this.moveToTargetPos = targetTransform.position;
			this.moveDistance = Utility.Math.Abs(this.moveToTargetPos.x - this.playerMove.position.x);
			this.speedUpDistance = this.moveDistance * 0.1f;
			this.speedDownDistance = this.moveDistance * 0.9f;
			this.speedState = 1;
			this.SetPlayerState(EventMemberController.EventPlayerState.Move);
			if (this.playerRide != null)
			{
				this.PlayFastMoveSound();
				this.playerRide.FastMove();
			}
			this.isMoveToTarget = true;
			EventArgsMoveToNpc eventArgsMoveToNpc = new EventArgsMoveToNpc();
			eventArgsMoveToNpc.SetData(function, memberId);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_MoveToNpc, eventArgsMoveToNpc);
		}

		public void ArriveNpc()
		{
			this.isMoveToTarget = false;
			this.sceneMapController.SetNormalSpeed();
			this.SetPlayerState(EventMemberController.EventPlayerState.Idle);
			if (this.isTrack)
			{
				this.isTrack = false;
			}
			else
			{
				CMemberBase mainMember = this.memberFactory.MainMember;
				if (mainMember != null)
				{
					mainMember.PlayAnimation(MemberAnimationName.GetSelfIdleAnimationName());
				}
				this.PlayPetAnimation("Idle");
				CMemberBase mainMember2 = this.memberFactory.MainMember;
				if (((mainMember2 != null) ? mainMember2.mountSpinePlayer : null) != null)
				{
					CMemberBase mainMember3 = this.memberFactory.MainMember;
					if (mainMember3 != null)
					{
						mainMember3.mountSpinePlayer.PlayAnimation("Idle");
					}
				}
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEven_ArriveNpc, null);
			if (this.actionNormal != null)
			{
				this.actionNormal.OnArrived();
			}
		}

		public void LeaveFromNpc()
		{
			if (this.isMoveToTarget)
			{
				return;
			}
			if (this.actionNormal != null)
			{
				this.actionNormal.OnLeave();
				this.waitRemoveEventPoints.Add(this.actionNormal);
			}
			if (this.moveToMember != null)
			{
				this.waitRemoveMembers.Add(this.moveToMember);
				this.WaitRemoveNpcRide();
			}
			this.moveToMember = null;
			this.npcRide = null;
			this.playerMoveSpeed = this.PlayerNormalSpeed;
			this.SetPlayerState(EventMemberController.EventPlayerState.Move);
			this.ChangePlayerMoveAni();
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RemoveNpc_Finish, null);
		}

		private void OnEventGroupEnd(object sender, int type, BaseEventArgs eventargs)
		{
			if (this.moveToMember != null)
			{
				this.LeaveFromNpc();
				return;
			}
			if (this.actionNormal != null)
			{
				this.LeaveFromNpc();
			}
		}

		private void ChangePlayerIdleAni()
		{
			CMemberBase mainMember = this.memberFactory.MainMember;
			if (mainMember != null)
			{
				mainMember.PlayAnimation(MemberAnimationName.GetSelfIdleAnimationName());
			}
			this.PlayPetAnimation("Idle");
			CMemberBase mainMember2 = this.memberFactory.MainMember;
			if (((mainMember2 != null) ? mainMember2.mountSpinePlayer : null) != null)
			{
				CMemberBase mainMember3 = this.memberFactory.MainMember;
				if (mainMember3 == null)
				{
					return;
				}
				mainMember3.mountSpinePlayer.PlayAnimation("Idle");
			}
		}

		private void ChangePlayerMoveAni()
		{
			if (this.playerRide != null)
			{
				CMemberBase mainMember = this.memberFactory.MainMember;
				if (mainMember != null)
				{
					mainMember.PlayAnimation(MemberAnimationName.GetSelfIdleAnimationName());
				}
				this.PlayPetAnimation("Idle");
				CMemberBase mainMember2 = this.memberFactory.MainMember;
				if (((mainMember2 != null) ? mainMember2.mountSpinePlayer : null) != null)
				{
					CMemberBase mainMember3 = this.memberFactory.MainMember;
					if (mainMember3 == null)
					{
						return;
					}
					mainMember3.mountSpinePlayer.PlayAnimation("Idle");
					return;
				}
			}
			else
			{
				CMemberBase mainMember4 = this.memberFactory.MainMember;
				if (((mainMember4 != null) ? mainMember4.mountSpinePlayer : null) == null)
				{
					CMemberBase mainMember5 = this.memberFactory.MainMember;
					if (mainMember5 == null)
					{
						return;
					}
					mainMember5.PlayAnimation("Idle_to_Run", null, delegate(TrackEntry entry)
					{
						CMemberBase mainMember7 = this.memberFactory.MainMember;
						if (mainMember7 != null)
						{
							mainMember7.PlayAnimation("Run");
						}
						this.PlayPetAnimation("Run");
					});
					return;
				}
				else
				{
					CMemberBase mainMember6 = this.memberFactory.MainMember;
					if (mainMember6 != null)
					{
						mainMember6.PlayAnimation(MemberAnimationName.GetSelfIdleAnimationName());
					}
					if (this.memberFactory.MainMember != null)
					{
						if (this.memberFactory.MainMember.mountSpinePlayer.IsHaveAni("Idle_to_Run"))
						{
							this.memberFactory.MainMember.mountSpinePlayer.PlayAnimation("Idle_to_Run", null, delegate(TrackEntry entry)
							{
								this.memberFactory.MainMember.mountSpinePlayer.PlayAnimation("Run");
								this.PlayPetAnimation("Run");
							});
							return;
						}
						int mountMemberId = GameApp.Data.GetDataModule(DataName.MountDataModule).GetMountMemberId(null);
						HLog.LogError(string.Format("坐骑Member {0} 没有{1}动作", mountMemberId, "Idle_to_Run"));
						this.memberFactory.MainMember.mountSpinePlayer.PlayAnimation("Run");
						this.PlayPetAnimation("Run");
					}
				}
			}
		}

		private void ChangePlayerFastMoveAni()
		{
			if (!(this.playerRide != null))
			{
				CMemberBase mainMember = this.memberFactory.MainMember;
				if (((mainMember != null) ? mainMember.mountSpinePlayer : null) == null)
				{
					CMemberBase mainMember2 = this.memberFactory.MainMember;
					if (mainMember2 != null)
					{
						mainMember2.PlayAnimation("FastRun");
					}
				}
				else
				{
					CMemberBase mainMember3 = this.memberFactory.MainMember;
					if (mainMember3 != null)
					{
						mainMember3.PlayAnimation(MemberAnimationName.GetSelfIdleAnimationName());
					}
					CMemberBase mainMember4 = this.memberFactory.MainMember;
					if (mainMember4 != null)
					{
						mainMember4.mountSpinePlayer.PlayAnimation("FastRun");
					}
				}
				this.PlayPetAnimation("FastRun");
				return;
			}
			CMemberBase mainMember5 = this.memberFactory.MainMember;
			if (mainMember5 == null)
			{
				return;
			}
			mainMember5.PlayAnimation("Battle/Cast_Loop");
		}

		private void Tackle(bool isBattle)
		{
			if (this.playerRide != null)
			{
				return;
			}
			if (this.isTrack)
			{
				return;
			}
			this.isTrack = true;
			this.cameraController.SetFollowMode(isBattle ? CameraFollowType.TackleToBattle : CameraFollowType.Tackle);
			CMemberBase mainMember = this.memberFactory.MainMember;
			if (!(((mainMember != null) ? mainMember.mountSpinePlayer : null) == null))
			{
				CMemberBase mainMember2 = this.memberFactory.MainMember;
				if (mainMember2 != null)
				{
					mainMember2.PlayAnimation(MemberAnimationName.GetSelfIdleAnimationName());
				}
				if (this.memberFactory.MainMember != null)
				{
					if (this.memberFactory.MainMember.roleSpinePlayer.IsHaveAni("Run_to_Idle"))
					{
						this.memberFactory.MainMember.mountSpinePlayer.PlayAnimation("Run_to_Idle", null, delegate(TrackEntry entry)
						{
							this.memberFactory.MainMember.mountSpinePlayer.PlayAnimation("Idle");
							this.PlayPetAnimation("Idle");
						});
						return;
					}
					int mountMemberId = GameApp.Data.GetDataModule(DataName.MountDataModule).GetMountMemberId(null);
					HLog.LogError(string.Format("坐骑Member {0} 没有{1}动作", mountMemberId, "Run_to_Idle"));
					this.memberFactory.MainMember.mountSpinePlayer.PlayAnimation("Idle");
					this.PlayPetAnimation("Idle");
				}
				return;
			}
			CMemberBase mainMember3 = this.memberFactory.MainMember;
			if (mainMember3 == null)
			{
				return;
			}
			mainMember3.PlayAnimation("Run_to_Idle", null, delegate(TrackEntry entry)
			{
				CMemberBase mainMember4 = this.memberFactory.MainMember;
				if (mainMember4 != null)
				{
					mainMember4.PlayAnimation(MemberAnimationName.GetSelfIdleAnimationName());
				}
				this.PlayPetAnimation("Idle");
			});
		}

		private void ResetWordPosition()
		{
			if (this.isMoveToTarget || this.isEnemyComing)
			{
				return;
			}
			Vector3 vector = this.playerMove.position;
			vector.x -= 1000f;
			this.playerMove.position = vector;
			vector = this.enemyMove.position;
			vector.x -= 1000f;
			this.enemyMove.position = vector;
			for (int i = 0; i < this.npcParent.transform.childCount; i++)
			{
				Transform child = this.npcParent.transform.GetChild(i);
				if (child)
				{
					Vector3 position = child.position;
					position.x -= 1000f;
					child.position = position;
				}
			}
			this.cameraController.ResetWordPosition();
			this.sceneMapController.ResetWordPosition();
		}

		public float PlayerNormalSpeed
		{
			get
			{
				if (this.playerRide != null)
				{
					return 2f;
				}
				return 4f;
			}
		}

		public float PlayerFastSpeed
		{
			get
			{
				this.playerRide != null;
				return 8f;
			}
		}

		public void GoInScreen(Action onFinish)
		{
			this.PauseMove(true);
			this.playerMove.localPosition = new Vector3(this.tempPosX - 10f, this.playerMove.localPosition.y, this.playerMove.localPosition.z);
			this.cameraController.SetFollowActive(false);
			this.cameraController.SetReadyEnter(this.playerMove.position.y);
			this.cameraController.FirstEnter();
			Sequence sequence = this.sequencePool.Get();
			this.ChangePlayerFastMoveAni();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOLocalMoveX(this.playerMove, this.tempPosX, 1f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.cameraController.SetFollowActive(true);
				this.sceneMapController.StartMove();
				this.StartMove();
				Action onFinish2 = onFinish;
				if (onFinish2 == null)
				{
					return;
				}
				onFinish2();
			});
		}

		public void GoOutScreen(Action onFinish)
		{
			this.tempPosX = this.playerMove.position.x;
			this.cameraController.SetFollowActive(false);
			this.sceneMapController.StopMove();
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, 2f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				Action onFinish2 = onFinish;
				if (onFinish2 == null)
				{
					return;
				}
				onFinish2();
			});
		}

		private async Task Init_Ride()
		{
			int rideId = this.gameMapData.RideId;
			if (rideId != 0)
			{
				Ride_Ride elementById = GameApp.Table.GetManager().GetRide_RideModelInstance().GetElementById(rideId);
				if (elementById == null)
				{
					HLog.LogError(string.Format("Table Ride not found id={0}", rideId));
				}
				else
				{
					TaskOutValue<RideCtrl> outRide = new TaskOutValue<RideCtrl>();
					await this.CreateRide(elementById.model, elementById.skin, -10, this.playerParent.transform, outRide);
					if (outRide.Value != null)
					{
						this.playerRide = outRide.Value;
						this.playerRide.transform.localPosition = new Vector3(0f, 0f, 0f);
					}
				}
			}
		}

		private void DeInit_Ride()
		{
			for (int i = 0; i < this.rideList.Count; i++)
			{
				RideCtrl rideCtrl = this.rideList[i];
				if (rideCtrl)
				{
					rideCtrl.DeInit();
				}
			}
			this.rideList.Clear();
			this.waitRemoveRideDic.Clear();
		}

		private void Update_Ride(float deltaTime, float unscaledDeltaTime)
		{
			for (int i = 0; i < this.rideList.Count; i++)
			{
				RideCtrl rideCtrl = this.rideList[i];
				if (rideCtrl)
				{
					rideCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
		}

		private void SetPlayRide(CMemberBase player)
		{
			if (player == null)
			{
				return;
			}
			if (this.playerRide != null)
			{
				player.m_gameObject.SetParentNormal(this.playerRide.transform, false);
				return;
			}
			player.m_gameObject.transform.localPosition = Vector3.zero;
		}

		private void SetPetRide(CMemberBase pet, int petIndex)
		{
			if (pet == null)
			{
				return;
			}
			if (this.playerRide != null)
			{
				pet.m_gameObject.SetParentNormal(this.playerRide.transform, false);
			}
			if (petIndex < this.petOffsetList.Count)
			{
				pet.m_gameObject.transform.localPosition = this.petOffsetList[petIndex];
			}
		}

		private async Task CreateEnemyRide(int rideId)
		{
			TaskOutValue<RideCtrl> outRide = new TaskOutValue<RideCtrl>();
			Ride_Ride elementById = GameApp.Table.GetManager().GetRide_RideModelInstance().GetElementById(rideId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("Table Ride not found id={0}", rideId));
			}
			else
			{
				await this.CreateRide(elementById.model, elementById.skin, -10, this.enemyParent.transform, outRide);
				if (outRide.Value != null)
				{
					this.enemyRide = outRide.Value;
					this.enemyRide.transform.localPosition = new Vector3(0f, 0f, 0f);
				}
			}
		}

		private async Task CreateNpcRide(int rideId, Vector3 pos, Quaternion rot, float scele = 1f)
		{
			TaskOutValue<RideCtrl> outRide = new TaskOutValue<RideCtrl>();
			Ride_Ride elementById = GameApp.Table.GetManager().GetRide_RideModelInstance().GetElementById(rideId);
			if (elementById != null)
			{
				int num = -16;
				await this.CreateRide(elementById.model, elementById.skin, num, this.npcParent.transform, outRide);
				if (outRide.Value != null)
				{
					this.npcRide = outRide.Value;
					if (this.npcRide != null)
					{
						this.npcRide.transform.position = pos;
						this.npcRide.transform.rotation = rot;
						this.npcRide.transform.localScale = Vector3.one * scele;
					}
				}
			}
		}

		private async Task CreateRide(int rideId, string skin, int orderLayer, Transform parent, TaskOutValue<RideCtrl> outRide)
		{
			ArtRide_Ride elementById = GameApp.Table.GetManager().GetArtRide_RideModelInstance().GetElementById(rideId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("Table ArtRide not found id = {0}", rideId));
			}
			else
			{
				await this.CreateRide(elementById.path, skin, orderLayer, parent, outRide);
			}
		}

		private Task CreateRide(string ridePath, string skin, int orderLayer, Transform parent, TaskOutValue<RideCtrl> outRide)
		{
			EventMemberController.<CreateRide>d__237 <CreateRide>d__;
			<CreateRide>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CreateRide>d__.<>4__this = this;
			<CreateRide>d__.ridePath = ridePath;
			<CreateRide>d__.skin = skin;
			<CreateRide>d__.orderLayer = orderLayer;
			<CreateRide>d__.parent = parent;
			<CreateRide>d__.outRide = outRide;
			<CreateRide>d__.<>1__state = -1;
			<CreateRide>d__.<>t__builder.Start<EventMemberController.<CreateRide>d__237>(ref <CreateRide>d__);
			return <CreateRide>d__.<>t__builder.Task;
		}

		private void RemoveEnemyRide()
		{
			if (this.enemyRide != null)
			{
				this.enemyRide.DeInit();
				Object.Destroy(this.enemyRide.gameObject);
				this.enemyRide = null;
			}
		}

		private void WaitRemoveNpcRide()
		{
			if (this.npcRide != null)
			{
				this.waitRemoveRideDic.Add(this.moveToMember.InstanceID, this.npcRide);
			}
		}

		private void RemoveRide(int id)
		{
			RideCtrl rideCtrl;
			if (this.waitRemoveRideDic.TryGetValue(id, out rideCtrl))
			{
				this.rideList.Remove(rideCtrl);
				rideCtrl.DeInit();
				Object.Destroy(rideCtrl.gameObject);
				this.waitRemoveRideDic.Remove(id);
			}
		}

		private void Update_WaveChange(float deltaTime)
		{
			if (this.isNewWaveEnter)
			{
				this.NewWaveEnterMove(deltaTime);
			}
		}

		public override async Task WaveChange(int waveIndex, List<CardData> cardDatas, ClientPointController pointCtrl, CMemberFactory cMemberFactory, TaskOutValue<List<CMemberBase>> outMembers)
		{
			if (this.enemyRide != null)
			{
				RideCtrl tempRide = this.enemyRide;
				this.enemyRide = null;
				tempRide.gameObject.transform.SetParent(null);
				tempRide.HideAni(delegate
				{
					if (tempRide)
					{
						tempRide.ResetAlpha();
						tempRide.DeInit();
						Object.Destroy(tempRide.gameObject);
					}
				});
			}
			int num = waveIndex - 1;
			if (num < this.CurrentMonsterList.Count)
			{
				int monsterCfgId = this.CurrentMonsterList[num];
				TaskOutValue<List<CMemberBase>> outList = new TaskOutValue<List<CMemberBase>>();
				await this.CreateMonsterGroup(monsterCfgId, num, outList);
				if (outList.Value != null && outList.Value.Count > 0)
				{
					outMembers.SetValue(outList.Value);
					int num2 = 0;
					int num3 = 0;
					MonsterCfg_monsterCfg monsterCfg_monsterCfg = GameApp.Table.GetManager().GetMonsterCfg_monsterCfg(monsterCfgId);
					if (monsterCfg_monsterCfg != null)
					{
						num2 = monsterCfg_monsterCfg.ride;
						num3 = monsterCfg_monsterCfg.isDropBox;
					}
					else
					{
						HLog.LogError(string.Format("Table Chapter_monsterCfg not found id ={0}", monsterCfgId));
					}
					await this.NewWaveEnter(outList.Value, num2, num3);
				}
			}
		}

		public async Task NewWaveEnter(List<CMemberBase> enemyList, int ride, int dropBox)
		{
			if (enemyList != null)
			{
				this.isDropBox = dropBox > 0;
				this.enemies.AddRange(enemyList);
				if (ride > 0)
				{
					await this.CreateEnemyRide(ride);
				}
				Transform transform = this.enemyParent.transform;
				if (this.enemyRide != null)
				{
					transform = this.enemyRide.pointPlayer;
					this.enemyRide.StopMove();
				}
				for (int i = 0; i < enemyList.Count; i++)
				{
					enemyList[i].m_gameObject.transform.SetParent(transform);
					Vector3 position = enemyList[i].m_gameObject.transform.position;
					position.x += this.playerOffsetX;
					enemyList[i].m_gameObject.transform.position = position;
				}
			}
		}

		public override void WaveEnter(int remainEnterFrameCount)
		{
			this.SetPlayerState(EventMemberController.EventPlayerState.Idle);
			Vector3 position = this.playerMove.transform.position;
			this.newWaveMoveTime = Config.GetTimeByFrame(remainEnterFrameCount);
			this.elapsedTime = 0f;
			this.startPoint = this.enemyMove.position;
			this.endPoint = new Vector3(position.x + 5.3f, this.startPoint.y, this.startPoint.z);
			if (this.enemyRide != null)
			{
				this.enemyRide.FastMove();
			}
			else
			{
				for (int i = 0; i < this.enemies.Count; i++)
				{
					if (this.enemies[i] != null && !this.enemies[i].IsDeath)
					{
						this.enemies[i].PlayAnimation("Run");
					}
				}
			}
			this.isNewWaveEnter = true;
		}

		protected override void NewWaveEnterMove(float deltaTime)
		{
			this.elapsedTime += deltaTime;
			float num = this.elapsedTime / this.newWaveMoveTime;
			this.enemyMove.position = Vector3.Lerp(this.startPoint, this.endPoint, num);
			if (num >= 1f)
			{
				this.isNewWaveEnter = false;
				this.enemyMove.position = this.endPoint;
				this.NewWaveArrive();
			}
		}

		protected override void NewWaveArrive()
		{
			if (this.enemyRide != null)
			{
				this.enemyRide.StopMove();
			}
			else
			{
				for (int i = 0; i < this.enemies.Count; i++)
				{
					if (this.enemies[i] != null && !this.enemies[i].IsDeath)
					{
						this.enemies[i].PlayAnimation("Idle");
					}
				}
			}
			for (int j = 0; j < this.enemies.Count; j++)
			{
				if (this.enemies[j] != null && this.enemies[j].m_gameObject != null && !this.enemies[j].IsDeath)
				{
					this.enemies[j].InitStartPosition();
				}
			}
			this.enemies.Clear();
		}

		private Transform playerMove;

		private GameObject playerParent;

		private GameObject npcParent;

		private GameObject fishParent;

		private Transform enemyMove;

		private GameObject enemyParent;

		private Animator playerRaftAni;

		private Animator enemyRaftAni;

		private GameObject weatherObj;

		private Transform trashParent;

		private GameObject pointParent;

		private List<CMemberBase> waitRemoveMembers = new List<CMemberBase>();

		private List<EventPointActionNormal> waitRemoveEventPoints = new List<EventPointActionNormal>();

		private MemberEnterBattleMode moveToMode;

		private GameEventBattleType gameBattleType = GameEventBattleType.Normal;

		private EventMemberController.EventPlayerState playerState = EventMemberController.EventPlayerState.Move;

		private bool isPause;

		private bool isFishBattle;

		private CMemberFactory memberFactory;

		private GameCameraController cameraController;

		private SceneMapController sceneMapController;

		private ClientPointController clientPointController;

		private const int State_SpeedNormal = 0;

		private const int State_SpeedUp = 1;

		private const int State_SpeedDown = 2;

		private const int State_WaitSpeedDown = 3;

		private EventPointActionNormal actionNormal;

		public GameMapData gameMapData;

		private MapType mapType;

		private GameObject mGameObject;

		private int[] soundArr = new int[] { 501, 502, 503, 504 };

		private int[] stepSoundArr = new int[] { 401, 402, 403, 404, 405, 406, 407 };

		private int curSoundID = -1;

		private List<int> curSoundIDs;

		private List<CMemberBase> enemies = new List<CMemberBase>();

		private bool isDropBox;

		private const string Raft_Animation_Idle = "BoatIdle";

		private const string Raft_Animation_LeftImpact = "BoatLeftImpact";

		private const string Raft_Animation_RightImpact = "BoatRightImpact";

		private float enemyMoveSpeed = 2f;

		private bool isEnemyComing;

		private Vector3 enemyTargetPos;

		private int enemySpeedState;

		private float enemyMoveDistance;

		private float enemySpeedUpDistance;

		private float enemySpeedDownDistance;

		private BezierCurveAnimation fishAnimation;

		private CMemberBase fishMember;

		private int fishId;

		private FishType fishType;

		private Vector3 fishEndPos;

		private int atkUpgrade;

		private int hpUpgrade;

		private const float Fishing_Time = 2f;

		private CMemberBase followMember;

		private Transform followMemberTrans;

		private int followStage;

		private bool isCatchUp;

		private Vector3 catchUpPos;

		private FishFollowCtrl fishFollowCtrl;

		private const float NPC_CatchUp_SpeedRatio = 5f;

		private const float NPC_CatchUp_BornOffsetX = 8f;

		private IDCreator idCreator;

		private List<BattleChapterMemberData> enemyDataList = new List<BattleChapterMemberData>();

		private CMemberBase passingMember;

		private MapWeatherCtrl mapWeatherCtrl;

		private bool isPauseMove = true;

		private bool isMoveToTarget;

		private bool isLeaveTarget;

		private bool isBattleNpcMove;

		private float moveDistance;

		private float speedUpDistance;

		private float speedDownDistance;

		private int speedState;

		private Vector3 moveToTargetPos;

		private CMemberBase moveToMember;

		private bool isTrack;

		private SequencePool sequencePool = new SequencePool();

		private float tempPosX;

		private RideCtrl playerRide;

		private RideCtrl enemyRide;

		private RideCtrl npcRide;

		private Dictionary<int, RideCtrl> waitRemoveRideDic = new Dictionary<int, RideCtrl>();

		private List<RideCtrl> rideList = new List<RideCtrl>();

		private const float Ride_OffsetY = 0f;

		private Vector3 startPoint;

		private Vector3 endPoint;

		public enum EventPlayerState
		{
			Idle,
			Move,
			Fishing
		}
	}
}
