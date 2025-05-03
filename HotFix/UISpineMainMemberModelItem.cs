using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using LocalModels.Bean;
using Server;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class UISpineMainMemberModelItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void ShowModel(int modelId, string defaultAni, bool isLoopAni)
		{
			this.ShowModel(modelId, 0, defaultAni, isLoopAni);
		}

		public void SetScale(float scale)
		{
			if (this.scaleNode != null)
			{
				this.scaleNode.localScale = scale * Vector3.one;
			}
		}

		public Task ShowModel(int modelId, int skinId, string defaultAni, bool isLoopAni)
		{
			UISpineMainMemberModelItem.<ShowModel>d__11 <ShowModel>d__;
			<ShowModel>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ShowModel>d__.<>4__this = this;
			<ShowModel>d__.modelId = modelId;
			<ShowModel>d__.skinId = skinId;
			<ShowModel>d__.defaultAni = defaultAni;
			<ShowModel>d__.isLoopAni = isLoopAni;
			<ShowModel>d__.<>1__state = -1;
			<ShowModel>d__.<>t__builder.Start<UISpineMainMemberModelItem.<ShowModel>d__11>(ref <ShowModel>d__);
			return <ShowModel>d__.<>t__builder.Task;
		}

		private void OnEventRefreshEquip(object sender, int type, BaseEventArgs eventArgs)
		{
			this.UpdateWeapon();
		}

		public void UpdateWeapon()
		{
			if (this.weapon != null)
			{
				EquipDataModule dataModule = GameApp.Data.GetDataModule(DataName.EquipDataModule);
				ulong equipDressRowId = dataModule.GetEquipDressRowId(EquipType.Weapon, 0);
				if (equipDressRowId <= 0UL)
				{
					if (this.handNode.activeSelf)
					{
						this.handNode.SetActive(false);
					}
					return;
				}
				if (!this.handNode.activeSelf)
				{
					this.handNode.SetActive(true);
				}
				if (this.cacheRowId.Equals(equipDressRowId))
				{
					return;
				}
				this.cacheRowId = equipDressRowId;
				EquipData equipDataByRowId = dataModule.GetEquipDataByRowId(equipDressRowId);
				if (equipDataByRowId == null)
				{
					return;
				}
				Equip_equip elementById = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)equipDataByRowId.id);
				ArtSkin_equipSkin elementById2 = GameApp.Table.GetManager().GetArtSkin_equipSkinModelInstance().GetElementById(elementById.skinId);
				this.weapon.Skeleton.SetSkin(elementById2.skinName);
				this.weapon.Skeleton.SetSlotsToSetupPose();
				this.weapon.AnimationState.Apply(this.weapon.Skeleton);
			}
		}

		public void ShowMemberModel(int memberId, string defaultAni, bool isLoopAni)
		{
			this.body.gameObject.SetActive(false);
			this.weapon.gameObject.SetActive(false);
			this.hand.gameObject.SetActive(false);
			GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(memberId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("找不到memberId={0}", memberId));
				return;
			}
			this.ShowModel(elementById.modelID, elementById.initSkinID, defaultAni, isLoopAni);
		}

		public Animation GetAni(string aniName)
		{
			return this.body.AnimationState.Data.SkeletonData.FindAnimation(aniName);
		}

		public void PlayAnimation(string aniName, bool isLoop)
		{
			if (this.GetAni(aniName) != null)
			{
				this.body.AnimationState.SetAnimation(0, aniName, isLoop);
				this.hand.AnimationState.SetAnimation(0, aniName, isLoop);
			}
		}

		public void AddAnimation(string aniName, bool isLoop, float delay = 0f)
		{
			if (this.GetAni(aniName) != null)
			{
				this.body.AnimationState.AddAnimation(0, aniName, isLoop, delay);
				this.hand.AnimationState.AddAnimation(0, aniName, isLoop, delay);
			}
		}

		public void SetAnimationTimeScale(float timeScale)
		{
			this.body.timeScale = timeScale;
			this.weapon.timeScale = timeScale;
			this.hand.timeScale = timeScale;
		}

		public bool IsShow()
		{
			return this.body.gameObject.activeSelf;
		}

		public Transform scaleNode;

		public GameObject handNode;

		public SkeletonGraphic body;

		public SkeletonGraphic weapon;

		public SkeletonGraphic hand;

		private ulong cacheRowId;
	}
}
