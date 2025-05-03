using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.Modules;
using HotFix.Client;
using LocalModels.Bean;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class MainMemberSpinePlayer : RoleSpinePlayerBase
	{
		public bool IsInit { get; private set; }

		public override void Init(ComponentRegister componentRegister)
		{
			this.IsInit = true;
			this.SetRoleModelShow(true);
			this.weaponAnimator = new SpineAnimator(this.weaponAnimation);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_EquipDataModule_RefreshEquipDressRowIds, new HandlerEvent(this.OnEventRefreshEquip));
			this.sagecraftContrller = new SagecraftController();
			this.sagecraftContrller.Init(this.SagecraftNode);
			this.Init_Morph();
		}

		public override void DeInit()
		{
			if (!this.IsInit)
			{
				return;
			}
			this.cacheEquipId = 0;
			this.IsInit = false;
			this.DeInit_ColorRender();
			this.DeInit_Morph();
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_EquipDataModule_RefreshEquipDressRowIds, new HandlerEvent(this.OnEventRefreshEquip));
			SagecraftController sagecraftController = this.sagecraftContrller;
			if (sagecraftController == null)
			{
				return;
			}
			sagecraftController.DeInit();
		}

		private void OnDestroy()
		{
			this.DeInit();
		}

		public void SetAutoCheckWeaponChange(bool boo)
		{
			this.isAutoCheckWeaponChange = boo;
		}

		private void OnEventRefreshEquip(object sender, int type, BaseEventArgs eventargs)
		{
			if (!this.isAutoCheckWeaponChange)
			{
				return;
			}
			this.SetWeapon(GameApp.Data.GetDataModule(DataName.EquipDataModule).GetWeaponId());
		}

		public override void SetWeapon(int equipId)
		{
			if (this.weaponAnimation)
			{
				if (equipId <= 0)
				{
					this.cacheEquipId = equipId;
					if (this.handWeaponNode.activeSelf)
					{
						this.handWeaponNode.SetActive(false);
					}
					return;
				}
				if (!this.handWeaponNode.activeSelf)
				{
					this.handWeaponNode.SetActive(true);
				}
				if (this.cacheEquipId.Equals(equipId))
				{
					return;
				}
				this.cacheEquipId = equipId;
				Equip_equip equip_equip = GameApp.Table.GetManager().GetEquip_equip(equipId);
				ArtSkin_equipSkin elementById = GameApp.Table.GetManager().GetArtSkin_equipSkinModelInstance().GetElementById(equip_equip.skinId);
				if (elementById == null)
				{
					HLog.LogError(string.Format("GameArt_Skin is error  id = {0}", equip_equip.skinId));
					return;
				}
				this.sagecraftContrller.SetSagecraftTpye((WeaponType)elementById.subType);
				this.weaponAnimation.skeleton.SetSkin(elementById.skinName);
				this.weaponAnimation.skeleton.SetSlotsToSetupPose();
				this.weaponAnimation.AnimationState.Apply(this.weaponAnimation.skeleton);
			}
		}

		public override void SetSpeed(float speed)
		{
			this.MainSetSpeed(speed);
			foreach (KeyValuePair<int, MorphBehaviour> keyValuePair in this.morphDict)
			{
				MorphBehaviour value = keyValuePair.Value;
				SpineAnimator bodyAnimator = value.bodyAnimator;
				if (bodyAnimator != null)
				{
					bodyAnimator.SetSpeed(speed);
				}
				SpineAnimator handAnimator = value.handAnimator;
				if (handAnimator != null)
				{
					handAnimator.SetSpeed(speed);
				}
			}
		}

		public override void PlayAni(string animationName, bool isLoop)
		{
			this.PlayAni(animationName, isLoop, null);
		}

		public override void PlayAni(string animationName, bool isLoop, AnimationState.TrackEntryEventDelegate spineEvent)
		{
			this.MainPlayAnim(animationName, isLoop, spineEvent, null);
			foreach (KeyValuePair<int, MorphBehaviour> keyValuePair in this.morphDict)
			{
				MorphBehaviour value = keyValuePair.Value;
				if (value.bodyAnimator.IsHaveAni(animationName))
				{
					value.bodyAnimator.PlayAni(animationName, isLoop, spineEvent);
				}
				if (value.handAnimator.IsHaveAni(animationName))
				{
					value.handAnimator.PlayAni(animationName, isLoop);
				}
			}
			this.CheckAndSetWeaponActive(animationName);
		}

		public override TrackEntry PlayAni(string animationName, bool isLoop, AnimationState.TrackEntryEventDelegate spineEvent, AnimationState.TrackEntryDelegate complete)
		{
			this.CheckAndSetWeaponActive(animationName);
			this.MainPlayAnim(animationName, isLoop, spineEvent, complete);
			TrackEntry trackEntry = null;
			foreach (KeyValuePair<int, MorphBehaviour> keyValuePair in this.morphDict)
			{
				MorphBehaviour value = keyValuePair.Value;
				trackEntry = value.bodyAnimator.PlayAni(animationName, isLoop, spineEvent, complete);
				value.handAnimator.PlayAni(animationName, isLoop, spineEvent, complete);
			}
			return trackEntry;
		}

		public override float GetAnimationDuration(string animationName)
		{
			return this.spineAnimatorBody.GetAnimationDuration(animationName);
		}

		public override TrackEntry AddAni(string animationName, bool isLoop)
		{
			TrackEntry trackEntry = this.spineAnimatorBody.AddAni(animationName, isLoop, 0f);
			this.MainAddAnim(animationName, isLoop);
			foreach (KeyValuePair<int, MorphBehaviour> keyValuePair in this.morphDict)
			{
				MorphBehaviour value = keyValuePair.Value;
				if (value.bodyAnimator.IsHaveAni(animationName))
				{
					value.bodyAnimator.AddAni(animationName, isLoop, 0f);
				}
				if (value.handAnimator.IsHaveAni(animationName))
				{
					value.handAnimator.AddAni(animationName, isLoop, 0f);
				}
			}
			return trackEntry;
		}

		public override bool IsHaveAni(string animationName)
		{
			return this.spineAnimatorBody.IsHaveAni(animationName);
		}

		public override void PlayAnimation(string animationName)
		{
			this.PlayAnimation(animationName, null, null);
		}

		public override void PlayAnimation(string animationName, AnimationState.TrackEntryEventDelegate spineEvent)
		{
			this.PlayAnimation(animationName, spineEvent, null);
		}

		public override void PlayAnimation(string animationName, AnimationState.TrackEntryDelegate complete)
		{
			this.PlayAnimation(animationName, null, complete);
		}

		public override void PlayAnimation(string animationName, AnimationState.TrackEntryEventDelegate spineEvent, AnimationState.TrackEntryDelegate complete)
		{
			if (string.IsNullOrEmpty(animationName))
			{
				return;
			}
			this.CheckAndSetWeaponActive(animationName);
			bool flag = MemberAnimationName.IsLoop(animationName);
			this.MainPlayAnim(animationName, flag, spineEvent, complete);
			foreach (KeyValuePair<int, MorphBehaviour> keyValuePair in this.morphDict)
			{
				MorphBehaviour value = keyValuePair.Value;
				if (value.bodyAnimator.IsHaveAni(animationName))
				{
					value.bodyAnimator.PlayAni(animationName, flag, spineEvent, complete);
				}
				if (value.handAnimator.IsHaveAni(animationName))
				{
					value.handAnimator.PlayAni(animationName, flag, spineEvent, complete);
				}
			}
		}

		public override void AddAnimation(string animationName)
		{
			bool flag = MemberAnimationName.IsLoop(animationName);
			foreach (KeyValuePair<int, MorphBehaviour> keyValuePair in this.morphDict)
			{
				MorphBehaviour value = keyValuePair.Value;
				if (value.bodyAnimator.IsHaveAni(animationName))
				{
					SpineAnimator bodyAnimator = value.bodyAnimator;
					if (bodyAnimator != null)
					{
						bodyAnimator.AddAni(animationName, flag, 0f);
					}
				}
				if (value.handAnimator.IsHaveAni(animationName))
				{
					SpineAnimator handAnimator = value.handAnimator;
					if (handAnimator != null)
					{
						handAnimator.AddAni(animationName, flag, 0f);
					}
				}
			}
		}

		public override void SetSortingLayer(string layer)
		{
			SpineAnimator spineAnimator = this.weaponAnimator;
			if (spineAnimator != null)
			{
				spineAnimator.SetSortingLayer(layer);
			}
			foreach (KeyValuePair<int, MorphBehaviour> keyValuePair in this.morphDict)
			{
				MorphBehaviour value = keyValuePair.Value;
				SpineAnimator bodyAnimator = value.bodyAnimator;
				if (bodyAnimator != null)
				{
					bodyAnimator.SetSortingLayer(layer);
				}
				SpineAnimator handAnimator = value.handAnimator;
				if (handAnimator != null)
				{
					handAnimator.SetSortingLayer(layer);
				}
			}
		}

		public override void SetOrderLayer(int layer)
		{
			this.MainSetOrderLayer(layer);
			foreach (KeyValuePair<int, MorphBehaviour> keyValuePair in this.morphDict)
			{
				MorphBehaviour value = keyValuePair.Value;
				SpineAnimator bodyAnimator = value.bodyAnimator;
				if (bodyAnimator != null)
				{
					bodyAnimator.SetOrderLayer(layer + 60);
				}
				SpineAnimator handAnimator = value.handAnimator;
				if (handAnimator != null)
				{
					handAnimator.SetOrderLayer(layer);
				}
			}
		}

		public override int GetMinOrderLayer()
		{
			int num = 9999;
			if (this.spineAnimatorBody != null && this.spineAnimatorBody.GetOrderLayer() < num)
			{
				num = this.spineAnimatorBody.GetOrderLayer();
			}
			if (this.weaponAnimator != null && this.weaponAnimator.GetOrderLayer() < num)
			{
				num = this.weaponAnimator.GetOrderLayer();
			}
			if (this.spineAnimatorHandBack != null && this.spineAnimatorHandBack.GetOrderLayer() < num)
			{
				num = this.spineAnimatorHandBack.GetOrderLayer();
			}
			if (this.spineAnimatorBack != null && this.spineAnimatorBack.GetOrderLayer() < num)
			{
				num = this.spineAnimatorBack.GetOrderLayer();
			}
			return num;
		}

		public override int GetMaxOrderLayer()
		{
			int num = 0;
			if (this.spineAnimatorBody != null && this.spineAnimatorBody.GetOrderLayer() > num)
			{
				num = this.spineAnimatorBody.GetOrderLayer();
			}
			if (this.weaponAnimator != null && this.weaponAnimator.GetOrderLayer() > num)
			{
				num = this.weaponAnimator.GetOrderLayer();
			}
			if (this.spineAnimatorHandFront != null && this.spineAnimatorHandFront.GetOrderLayer() > num)
			{
				num = this.spineAnimatorHandFront.GetOrderLayer();
			}
			if (this.spineAnimatorHeadFront != null && this.spineAnimatorHeadFront.GetOrderLayer() > num)
			{
				num = this.spineAnimatorHeadFront.GetOrderLayer();
			}
			return num;
		}

		public int GetWeaponLayer()
		{
			int num = 0;
			if (this.weaponAnimator != null && this.weaponAnimator.GetOrderLayer() > num)
			{
				num = this.weaponAnimator.GetOrderLayer();
			}
			return num;
		}

		public override void SetAnimatorSpeed(float speed)
		{
			foreach (KeyValuePair<int, MorphBehaviour> keyValuePair in this.morphDict)
			{
				MorphBehaviour value = keyValuePair.Value;
				SpineAnimator bodyAnimator = value.bodyAnimator;
				if (bodyAnimator != null)
				{
					bodyAnimator.SetSpeed(speed);
				}
				SpineAnimator handAnimator = value.handAnimator;
				if (handAnimator != null)
				{
					handAnimator.SetSpeed(speed);
				}
			}
			SpineAnimator spineAnimator = this.weaponAnimator;
			if (spineAnimator == null)
			{
				return;
			}
			spineAnimator.SetSpeed(speed);
		}

		public override void SetSkin(string skinName)
		{
		}

		public override void Init_ColorRender()
		{
			this.m_colorRender = this.modelRootGo.GetComponent<ColorRender>();
			List<Renderer> list = new List<Renderer>();
			Renderer[] componentsInChildren = this.weaponAnimation.GetComponentsInChildren<Renderer>();
			list.AddRange(componentsInChildren);
			List<Renderer> mainRenderers = this.GetMainRenderers();
			list.AddRange(mainRenderers);
			foreach (KeyValuePair<int, MorphBehaviour> keyValuePair in this.morphDict)
			{
				MorphBehaviour value = keyValuePair.Value;
				Renderer[] componentsInChildren2 = value.bodyAnimation.GetComponentsInChildren<Renderer>();
				Renderer component = value.handAnimation.GetComponent<Renderer>();
				list.AddRange(componentsInChildren2);
				list.Add(component);
			}
			if (this.m_colorRender != null)
			{
				this.m_colorRender.OnInit(list);
				this.m_colorRender.SetFillPhase(0f);
			}
		}

		public override void DeInit_ColorRender()
		{
			if (this.m_colorRender != null)
			{
				this.m_colorRender.SetFillPhase(0f);
				this.m_colorRender.SetFillVColor(0f);
				this.m_colorRender.OnDeInit();
			}
		}

		private void CheckAndSetWeaponActive(string animationName)
		{
			ArtAnimation_animation elementById = GameApp.Table.GetManager().GetArtAnimation_animationModelInstance().GetElementById(animationName);
			bool flag = (elementById != null && elementById.hideWeapon > 0) || this.cacheEquipId <= 0;
			if (flag && this.handWeaponNode.activeSelf)
			{
				this.handWeaponNode.SetActive(false);
				return;
			}
			if (!flag && !this.handWeaponNode.activeSelf)
			{
				this.handWeaponNode.SetActive(true);
			}
		}

		public override async Task ShowSagecraft(SagecraftType type)
		{
			await this.sagecraftContrller.ShowSagecraft(type);
			this.sagecraftContrller.SetOrderLayer(this.weaponAnimator.GetOrderLayer());
		}

		public override void DestroySagecraft(SagecraftType type)
		{
			this.sagecraftContrller.Destroy(type);
		}

		public override void SetRoleModelShow(bool isShow)
		{
			this.basicNode.SetActive(isShow);
			this.handWeaponNode.SetActive(isShow);
		}

		public SkeletonAnimation SkeletonAnimation_AnimBody
		{
			get
			{
				return this.AnimBody;
			}
		}

		public SkeletonAnimation SkeletonAnimation_AnimHandBack
		{
			get
			{
				return this.AnimHandBack;
			}
		}

		public SkeletonAnimation SkeletonAnimation_AnimHandFront
		{
			get
			{
				return this.AnimHandFront;
			}
		}

		public async Task InitSkin()
		{
			ClothesDataModule dataModule = GameApp.Data.GetDataModule(DataName.ClothesDataModule);
			Dictionary<SkinType, SkinData> dictionary = ((!this.IsEnemyPlayer) ? dataModule.BattleLeftRoleClothesData.GetSkinDatas() : dataModule.BattleRightRoleClothesData.GetSkinDatas());
			await this.InitSkin(dictionary);
		}

		public async Task InitSkin(Dictionary<SkinType, SkinData> skinDatas)
		{
			if (skinDatas != null)
			{
				if (!this.isInit)
				{
					this.curAnimationName = "Idle";
					this.spineAnimatorEffect = new SpineAnimator(this.AnimEffect);
					this.RelationComponent();
				}
				this.isInit = true;
				await this.SetSpine(skinDatas);
			}
		}

		private async Task SetSpine(Dictionary<SkinType, SkinData> skinDatas)
		{
			if (skinDatas != null)
			{
				await this.SetSkin(skinDatas[SkinType.Back], this.spineAnimatorBack, this.AnimBack, this.SpineSkin_Back);
				await this.SetSkin(skinDatas[SkinType.Head], this.spineAnimatorHeadBack, this.AnimHeadBack, this.SpineSkin_Back);
				await this.SetSkin(skinDatas[SkinType.Body], this.spineAnimatorHandBack, this.AnimHandBack, this.SpineSkin_Back);
				await this.SetSkin(skinDatas[SkinType.Body], this.spineAnimatorBody, this.AnimBody, this.SpineSkin_Body);
				await this.SetSkin(skinDatas[SkinType.Head], this.spineAnimatorHeadFront, this.AnimHeadFront, this.SpineSkin_Front);
				await this.SetSkin(skinDatas[SkinType.Body], this.spineAnimatorHandFront, this.AnimHandFront, this.SpineSkin_Front);
				this.spineAnimatorBody.AnimationState.Event += new AnimationState.TrackEntryEventDelegate(this.OnEvent);
				this.MainPlayAnim(this.curAnimationName, true, null, null);
				if (this.HasSkin(this.AnimHeadBack.skeletonDataAsset, this.SpineSkin_Back))
				{
					this.spineAnimatorHeadBack.PlayAni("Idle", true);
				}
				this.spineAnimatorHeadFront.PlayAni("Idle", true);
				this.spineAnimatorBack.PlayAni("Idle", true);
			}
		}

		private async Task SetSkin(SkinData skinData, SpineAnimator animator, SkeletonAnimation anim, string skinName)
		{
			await PoolManager.Instance.CheckPrefab(skinData.Path);
			SkeletonAnimation componentInChildren = PoolManager.Instance.GetAsset(skinData.Path).GetComponentInChildren<SkeletonAnimation>();
			if (skinData.SkinType == SkinType.Head && skinName.Equals(this.SpineSkin_Back))
			{
				bool flag = this.HasSkin(componentInChildren.skeletonDataAsset, skinName);
				anim.gameObject.SetActive(flag);
				if (!flag)
				{
					return;
				}
			}
			animator.Init(anim, componentInChildren.skeletonDataAsset, skinName);
			animator.SetSortingLayer("Member");
			if (skinData.SkinType == SkinType.Body)
			{
				if (skinName.Equals(this.SpineSkin_Body))
				{
					this.SetBoneFollower(animator);
				}
				else if (skinName.Equals(this.SpineSkin_Back))
				{
					this.Point_Weapon.skeletonRenderer = animator.curSkeletonAnimation;
					BoneFollowHelper helper_Weapon = this.Helper_Weapon;
					if (helper_Weapon != null)
					{
						helper_Weapon.AutoSet();
					}
				}
			}
		}

		private bool HasSkin(SkeletonDataAsset dataAsset, string skinName)
		{
			return !(dataAsset == null) && dataAsset.GetSkeletonData(true).FindSkin(skinName) != null;
		}

		public void DeInitSkin()
		{
			this.isInit = false;
			if (this.spineAnimatorBody != null && this.spineAnimatorBody.AnimationState != null)
			{
				this.spineAnimatorBody.AnimationState.Event -= new AnimationState.TrackEntryEventDelegate(this.OnEvent);
			}
			Object.Destroy(this.AnimBody.gameObject);
			Object.Destroy(this.AnimHandFront.gameObject);
			Object.Destroy(this.AnimHandBack.gameObject);
			Object.Destroy(this.AnimHeadFront.gameObject);
			Object.Destroy(this.AnimHeadBack.gameObject);
			Object.Destroy(this.AnimBack.gameObject);
			this.spineAnimatorEffect = null;
		}

		public void SetRoleClothesViewLayer(int layerId)
		{
			GameObject gameObject = this.goBody;
			if (gameObject != null)
			{
				gameObject.ChangeLayer(layerId);
			}
			GameObject gameObject2 = this.goHandFront;
			if (gameObject2 != null)
			{
				gameObject2.ChangeLayer(layerId);
			}
			GameObject gameObject3 = this.goHandBack;
			if (gameObject3 != null)
			{
				gameObject3.ChangeLayer(layerId);
			}
			GameObject gameObject4 = this.goHeadFront;
			if (gameObject4 != null)
			{
				gameObject4.ChangeLayer(layerId);
			}
			GameObject gameObject5 = this.goHeadBack;
			if (gameObject5 != null)
			{
				gameObject5.ChangeLayer(layerId);
			}
			GameObject gameObject6 = this.goBack;
			if (gameObject6 == null)
			{
				return;
			}
			gameObject6.ChangeLayer(layerId);
		}

		private void RelationComponent()
		{
			this.AnimBody = this.CreateSpinePoint(this.goBody);
			this.AnimHandFront = this.CreateSpinePoint(this.goHandFront);
			this.AnimHandBack = this.CreateSpinePoint(this.goHandBack);
			this.AnimHeadFront = this.CreateSpinePoint(this.goHeadFront);
			this.AnimHeadBack = this.CreateSpinePoint(this.goHeadBack);
			this.AnimBack = this.CreateSpinePoint(this.goBack);
			this.spineHelper_Head = this.spinePoint_Head.transform.GetComponent<BoneFollowHelper>();
			this.spineHelper_Back = this.spinePoint_Back.transform.GetComponent<BoneFollowHelper>();
			this.Helper_Center = this.Point_Center.transform.GetComponent<BoneFollowHelper>();
			this.Helper_Foot = this.Point_Foot.transform.GetComponent<BoneFollowHelper>();
			this.Helper_Hand_L = this.Point_Hand_L.transform.GetComponent<BoneFollowHelper>();
			this.Helper_Hand_R = this.Point_Hand_R.transform.GetComponent<BoneFollowHelper>();
			this.Helper_Head = this.Point_Head.transform.GetComponent<BoneFollowHelper>();
			this.Helper_Back = this.Point_Back.transform.GetComponent<BoneFollowHelper>();
			this.Helper_Weapon = this.Point_Weapon.transform.GetComponent<BoneFollowHelper>();
		}

		private SkeletonAnimation CreateSpinePoint(GameObject parent)
		{
			GameObject gameObject = new GameObject("SpineAnim");
			gameObject.transform.SetParent(parent.transform);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localRotation = Quaternion.identity;
			GameObjectExpand.SetLayer(gameObject, parent.layer, true);
			return gameObject.AddComponent<SkeletonAnimation>();
		}

		private void SetBoneFollower(SpineAnimator animator)
		{
			this.spinePoint_Head.skeletonRenderer = animator.curSkeletonAnimation;
			if (this.spinePoint_Head.skeletonRenderer != null)
			{
				BoneFollowHelper boneFollowHelper = this.spineHelper_Head;
				if (boneFollowHelper != null)
				{
					boneFollowHelper.AutoSet();
				}
			}
			this.spinePoint_Back.skeletonRenderer = animator.curSkeletonAnimation;
			if (this.spinePoint_Back.skeletonRenderer != null)
			{
				BoneFollowHelper boneFollowHelper2 = this.spineHelper_Back;
				if (boneFollowHelper2 != null)
				{
					boneFollowHelper2.AutoSet();
				}
			}
			this.Point_Center.skeletonRenderer = animator.curSkeletonAnimation;
			if (this.Point_Center.skeletonRenderer != null)
			{
				BoneFollowHelper helper_Center = this.Helper_Center;
				if (helper_Center != null)
				{
					helper_Center.AutoSet();
				}
			}
			this.Point_Foot.skeletonRenderer = animator.curSkeletonAnimation;
			if (this.spinePoint_Back.skeletonRenderer != null)
			{
				BoneFollowHelper helper_Foot = this.Helper_Foot;
				if (helper_Foot != null)
				{
					helper_Foot.AutoSet();
				}
			}
			this.Point_Hand_L.skeletonRenderer = animator.curSkeletonAnimation;
			if (this.spinePoint_Back.skeletonRenderer != null)
			{
				BoneFollowHelper helper_Hand_L = this.Helper_Hand_L;
				if (helper_Hand_L != null)
				{
					helper_Hand_L.AutoSet();
				}
			}
			this.Point_Hand_R.skeletonRenderer = animator.curSkeletonAnimation;
			if (this.spinePoint_Back.skeletonRenderer != null)
			{
				BoneFollowHelper helper_Hand_R = this.Helper_Hand_R;
				if (helper_Hand_R != null)
				{
					helper_Hand_R.AutoSet();
				}
			}
			this.Point_Head.skeletonRenderer = animator.curSkeletonAnimation;
			if (this.spinePoint_Back.skeletonRenderer != null)
			{
				BoneFollowHelper helper_Head = this.Helper_Head;
				if (helper_Head != null)
				{
					helper_Head.AutoSet();
				}
			}
			this.Point_Back.skeletonRenderer = animator.curSkeletonAnimation;
			if (this.spinePoint_Back.skeletonRenderer != null)
			{
				BoneFollowHelper helper_Back = this.Helper_Back;
				if (helper_Back == null)
				{
					return;
				}
				helper_Back.AutoSet();
			}
		}

		private void MainSetSpeed(float speed)
		{
			this.spineAnimatorBody.SetSpeed(speed);
			this.spineAnimatorHandFront.SetSpeed(speed);
			this.spineAnimatorHandBack.SetSpeed(speed);
			this.spineAnimatorHeadFront.SetSpeed(speed);
			this.spineAnimatorHeadBack.SetSpeed(speed);
			this.spineAnimatorBack.SetSpeed(speed);
			this.spineAnimatorEffect.SetSpeed(speed);
		}

		private void MainPlayAnim(string animationName, bool isLoop, AnimationState.TrackEntryEventDelegate spineEvent = null, AnimationState.TrackEntryDelegate complete = null)
		{
			this.curAnimationName = animationName;
			this.spineAnimatorBody.PlayAni(animationName, isLoop, spineEvent, complete);
			this.spineAnimatorHandFront.PlayAni(animationName, isLoop);
			this.spineAnimatorHandBack.PlayAni(animationName, isLoop);
			SpineAnimator spineAnimator = this.spineAnimatorEffect;
			if (spineAnimator == null)
			{
				return;
			}
			spineAnimator.PlayAni(animationName, isLoop);
		}

		private void MainAddAnim(string animationName, bool isLoop)
		{
			this.curAnimationName = animationName;
			this.spineAnimatorBody.AddAni(animationName, isLoop, 0f);
			this.spineAnimatorHandFront.AddAni(animationName, isLoop, 0f);
			this.spineAnimatorHandBack.AddAni(animationName, isLoop, 0f);
		}

		private void MainSetOrderLayer(int layer)
		{
			this.spineAnimatorBack.SetOrderLayer(layer + 10);
			this.spineAnimatorHandBack.SetOrderLayer(layer + 20);
			SpineAnimator spineAnimator = this.weaponAnimator;
			if (spineAnimator != null)
			{
				spineAnimator.SetOrderLayer(layer + 25);
			}
			this.sagecraftContrller.SetOrderLayer(layer + 25);
			this.spineAnimatorHeadBack.SetOrderLayer(layer + 50);
			this.spineAnimatorBody.SetOrderLayer(layer + 60);
			this.spineAnimatorHeadFront.SetOrderLayer(layer + 70);
			this.spineAnimatorHandFront.SetOrderLayer(layer + 80);
			SpineAnimator spineAnimator2 = this.spineAnimatorEffect;
			if (spineAnimator2 == null)
			{
				return;
			}
			spineAnimator2.SetOrderLayer(layer + 90);
		}

		private List<Renderer> GetMainRenderers()
		{
			List<Renderer> list = new List<Renderer>();
			Renderer component = this.AnimBody.GetComponent<Renderer>();
			list.Add(component);
			Renderer component2 = this.AnimHandFront.GetComponent<Renderer>();
			list.Add(component2);
			Renderer component3 = this.AnimHandBack.GetComponent<Renderer>();
			list.Add(component3);
			Renderer component4 = this.AnimHeadFront.GetComponent<Renderer>();
			list.Add(component4);
			Renderer component5 = this.AnimHeadBack.GetComponent<Renderer>();
			list.Add(component5);
			Renderer component6 = this.AnimBack.GetComponent<Renderer>();
			list.Add(component6);
			return list;
		}

		public void Refresh(Dictionary<SkinType, SkinData> skinDatas = null)
		{
			if (skinDatas == null)
			{
				skinDatas = GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfClothesData.GetSkinDatas();
			}
			this.InitSkin(skinDatas);
		}

		private void ResetWeaponPoint()
		{
			if (this.spineAnimatorHandBack != null)
			{
				this.Point_Weapon.skeletonRenderer = this.spineAnimatorHandBack.curSkeletonAnimation;
				BoneFollowHelper helper_Weapon = this.Helper_Weapon;
				if (helper_Weapon == null)
				{
					return;
				}
				helper_Weapon.AutoSet();
			}
		}

		private void Init_Morph()
		{
		}

		private void DeInit_Morph()
		{
		}

		public override MorphBehaviour GetMorph(int morphId)
		{
			MorphBehaviour morphBehaviour;
			if (this.morphDict.TryGetValue(morphId, out morphBehaviour))
			{
				return morphBehaviour;
			}
			return null;
		}

		public override async Task PreInitMorph(int morphId)
		{
			if (!this.morphDict.ContainsKey(morphId))
			{
				if (morphId <= 0)
				{
					await Task.CompletedTask;
				}
				ArtMember_morph morphTable = GameApp.Table.GetManager().GetArtMember_morphModelInstance().GetElementById(morphId);
				if (morphTable == null)
				{
					HLog.LogError(HLog.ToColor(string.Format("ArtMember_morph is error, id: {0}", morphId), 2));
				}
				else
				{
					await PoolManager.Instance.CheckPrefab(morphTable.path);
					GameObject gameObject = PoolManager.Instance.Out(morphTable.path, Vector3.zero, Quaternion.identity, this.goBody.transform.parent);
					gameObject.ChangeLayer(6);
					MorphBehaviour component = gameObject.GetComponent<MorphBehaviour>();
					component.Init(this.handWeaponNode, morphId);
					this.morphDict[morphId] = component;
					component.transform.localPosition = this.goBody.transform.localPosition;
					component.transform.localScale = this.goBody.transform.localScale;
					component.transform.localRotation = this.goBody.transform.localRotation;
					component.bodyAnimator.PlayAni("Idle", true);
					component.handAnimator.PlayAni("Idle", true);
					MorphBehaviour morphBehaviour = component;
					if (morphBehaviour != null)
					{
						Renderer[] componentsInChildren = morphBehaviour.bodyAnimation.GetComponentsInChildren<Renderer>();
						Renderer component2 = morphBehaviour.handAnimation.GetComponent<Renderer>();
						List<Renderer> list = new List<Renderer>();
						list.AddRange(componentsInChildren);
						list.Add(component2);
						if (this.m_colorRender != null)
						{
							this.m_colorRender.AddRenderers(list);
						}
					}
				}
			}
		}

		public override void ActiveMorph(int morphId)
		{
			this.SetRoleClothesViewLayer((morphId == 0) ? 0 : 6);
			if (morphId == 0 && this.curMorph != null)
			{
				this.ResetWeaponPoint();
				this.curMorph.Hide();
				return;
			}
			MorphBehaviour morphBehaviour = this.curMorph;
			if (morphBehaviour != null && morphBehaviour.morphId != morphId)
			{
				morphBehaviour.Hide();
			}
			MorphBehaviour morphBehaviour2;
			if (this.morphDict.TryGetValue(morphId, out morphBehaviour2))
			{
				this.curMorph = morphBehaviour2;
				this.curMorph.Show();
			}
		}

		public GameObject handWeaponNode;

		public GameObject basicNode;

		public SkeletonAnimation weaponAnimation;

		public GameObject modelRootGo;

		public ComponentRegister SagecraftNode;

		private SpineAnimator weaponAnimator;

		private SagecraftController sagecraftContrller;

		private bool isAutoCheckWeaponChange;

		private int cacheEquipId;

		private readonly string SpineSkin_Body = "Body";

		private readonly string SpineSkin_Front = "Front";

		private readonly string SpineSkin_Back = "Back";

		[Space(30f)]
		[Header("时装")]
		public GameObject goBody;

		public GameObject goHandFront;

		public GameObject goHandBack;

		public GameObject goHeadFront;

		public GameObject goHeadBack;

		public GameObject goBack;

		[Space(10f)]
		private SkeletonAnimation AnimBody;

		private SkeletonAnimation AnimHandFront;

		private SkeletonAnimation AnimHandBack;

		private SkeletonAnimation AnimHeadFront;

		private SkeletonAnimation AnimHeadBack;

		private SkeletonAnimation AnimBack;

		[Space(10f)]
		private SpineAnimator spineAnimatorBody = new SpineAnimator();

		private SpineAnimator spineAnimatorHandFront = new SpineAnimator();

		private SpineAnimator spineAnimatorHandBack = new SpineAnimator();

		private SpineAnimator spineAnimatorHeadFront = new SpineAnimator();

		private SpineAnimator spineAnimatorHeadBack = new SpineAnimator();

		private SpineAnimator spineAnimatorBack = new SpineAnimator();

		[Space(10f)]
		public BoneFollower spinePoint_Head;

		public BoneFollower spinePoint_Back;

		private BoneFollowHelper spineHelper_Head;

		private BoneFollowHelper spineHelper_Back;

		[Space(10f)]
		public BoneFollower Point_Center;

		public BoneFollower Point_Foot;

		public BoneFollower Point_Hand_L;

		public BoneFollower Point_Hand_R;

		public BoneFollower Point_Head;

		public BoneFollower Point_Back;

		public BoneFollower Point_Weapon;

		private BoneFollowHelper Helper_Center;

		private BoneFollowHelper Helper_Foot;

		private BoneFollowHelper Helper_Hand_L;

		private BoneFollowHelper Helper_Hand_R;

		private BoneFollowHelper Helper_Head;

		private BoneFollowHelper Helper_Back;

		private BoneFollowHelper Helper_Weapon;

		[Space(10f)]
		public SkeletonAnimation AnimEffect;

		private SpineAnimator spineAnimatorEffect;

		private bool isInit;

		private string curAnimationName = string.Empty;

		public bool IsEnemyPlayer;

		public Dictionary<int, MorphBehaviour> morphDict = new Dictionary<int, MorphBehaviour>();

		[NonSerialized]
		public MorphBehaviour curMorph;
	}
}
