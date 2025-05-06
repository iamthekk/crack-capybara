using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using Proto.Common;
using Proto.LeaderBoard;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class NewWorldRootController : CustomBehaviour
	{
		protected override async void OnInit()
		{
			if (this.spineFeiTing != null)
			{
				this.spineAnimator = new SpineAnimator(this.spineFeiTing);
				this.spineAnimator.PlayAni("Idle", true);
			}
			await this.CreatePlayer();
			await this.CreateMount();
			NetworkUtils.DoRankRequest(RankType.NewWorld, 1, false, true, null);
		}

		protected override void OnDeInit()
		{
			this.topUserDto = null;
			this.isCreateTopUser = false;
		}

		private async Task CreatePlayer()
		{
			Component transform = this.myPlayerPoint.transform;
			TaskOutValue<MainMemberModeItemData> outPlayer = new TaskOutValue<MainMemberModeItemData>();
			await ModelUtils.CreatePlayerMember(transform.gameObject, outPlayer);
			if (outPlayer.Value != null)
			{
				this.mainMemberData = outPlayer.Value;
				if (this.mainMemberData.MainMemberSpinePlayer != null)
				{
					this.mainMemberData.MainMemberSpinePlayer.PlayAni(MemberAnimationName.GetSelfIdleAnimationName(), true);
				}
			}
		}

		private async Task CreateMount()
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

		private async Task CreateTopPlayer()
		{
			if (!this.isCreateTopUser)
			{
				if (this.topUserDto != null && this.topUserDto.UserInfo != null)
				{
					this.isCreateTopUser = true;
					TaskOutValue<MainMemberModeItemData> outPlayer = new TaskOutValue<MainMemberModeItemData>();
					int memberID = GameApp.Data.GetDataModule(DataName.HeroDataModule).MainCardData.m_memberID;
					int num = 0;
					if (this.topUserDto.UserInfo.Equips != null)
					{
						for (int i = 0; i < this.topUserDto.UserInfo.Equips.Count; i++)
						{
							EquipmentDto equipmentDto = this.topUserDto.UserInfo.Equips[i];
							if (equipmentDto != null)
							{
								Equip_equip equip_equip = GameApp.Table.GetManager().GetEquip_equip((int)equipmentDto.EquipId);
								if (equip_equip != null && equip_equip.Type == 1)
								{
									num = equip_equip.id;
									break;
								}
							}
						}
					}
					Dictionary<SkinType, SkinData> skinDatas = new ClothesData((int)this.topUserDto.UserInfo.SkinHeaddressId, (int)this.topUserDto.UserInfo.SkinBodyId, (int)this.topUserDto.UserInfo.SkinAccessoryId).GetSkinDatas();
					await ModelUtils.CreatePlayerMember(memberID, num, this.topPlayerPoint, skinDatas, outPlayer);
					if (outPlayer.Value != null)
					{
						this.topMemberData = outPlayer.Value;
						if (this.topUserDto.UserInfo.MountInfo != null)
						{
							int mountMemberId = GameApp.Data.GetDataModule(DataName.MountDataModule).GetMountMemberId(this.topUserDto.UserInfo.MountInfo);
							TaskOutValue<MountMemberModeItemData> outMount = new TaskOutValue<MountMemberModeItemData>();
							await ModelUtils.CreatePlayerMount(this.topMemberData, mountMemberId, outMount);
							if (outMount.Value != null)
							{
								this.topPlayerMountSpine = outMount.Value.MountSpinePlayer;
								this.topPlayerMount = outMount.Value.MountMemberRoot;
							}
							outMount = null;
						}
						this.topMemberData.MainMemberRoot.transform.localScale = Vector3.one * 0.7f;
					}
				}
			}
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

		public void CheckShowTopPlayer(RankUserDto dto)
		{
			if (dto == null)
			{
				return;
			}
			this.topUserDto = dto;
			if (this.topMemberData != null && this.topMemberData.MainMemberRoot != null)
			{
				Object.Destroy(this.topMemberData.MainMemberRoot);
				this.topMemberData = null;
				this.topPlayerMount = null;
				this.isCreateTopUser = false;
			}
			this.CreateTopPlayer();
		}

		public Transform feiTingTrans;

		public SkeletonAnimation spineFeiTing;

		public GameObject topPlayerPoint;

		public GameObject myPlayerPoint;

		private SpineAnimator spineAnimator;

		private MainMemberModeItemData mainMemberData;

		private int currentMountId;

		private GameObject mountPlayer;

		private MountSpinePlayer mountSpinePlayer;

		private RankUserDto topUserDto;

		private MainMemberModeItemData topMemberData;

		private GameObject topPlayerMount;

		private MountSpinePlayer topPlayerMountSpine;

		private bool isCreateTopUser;
	}
}
