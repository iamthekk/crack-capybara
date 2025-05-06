using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using HotFix.GuildUI;
using LocalModels.Bean;
using Proto.Common;
using Proto.LeaderBoard;
using UnityEngine;

namespace HotFix
{
	public class UIRankTopItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void OnShow()
		{
			this.modelItem.Init();
			this.modelItem.OnShow();
			this.TitleCtrl.Init();
		}

		public void OnHide()
		{
			this.modelItem.OnHide(false);
			this.modelItem.DeInit();
			this.TitleCtrl.DeInit();
		}

		public void SetData(RankType rankType, RankUserDto dto, string cameraKey)
		{
			this.rankUser = dto;
			if (this.rankUser != null)
			{
				this.infoObj.SetActiveSafe(true);
				this.emptyObj.SetActiveSafe(false);
				this.guildObj.SetActiveSafe(false);
				this.powerObj.SetActiveSafe(false);
				if (this.rankUser.UserInfo != null)
				{
					this.textPlayerName.text = this.rankUser.UserInfo.NickName;
					int memberID = GameApp.Data.GetDataModule(DataName.HeroDataModule).MainCardData.m_memberID;
					int num = 0;
					if (this.rankUser.UserInfo.Equips != null)
					{
						for (int i = 0; i < this.rankUser.UserInfo.Equips.Count; i++)
						{
							EquipmentDto equipmentDto = this.rankUser.UserInfo.Equips[i];
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
					Dictionary<SkinType, SkinData> skinDatas = ClothesDataModule.GetPlayerClothesData((int)this.rankUser.UserInfo.SkinHeaddressId, (int)this.rankUser.UserInfo.SkinBodyId, (int)this.rankUser.UserInfo.SkinAccessoryId).GetSkinDatas();
					int num2 = 0;
					if (this.rankUser.UserInfo.MountInfo != null)
					{
						num2 = GameApp.Data.GetDataModule(DataName.MountDataModule).GetMountMemberId(this.rankUser.UserInfo.MountInfo);
					}
					this.modelItem.ShowPlayerModel(cameraKey, memberID, num, skinDatas, num2);
					if (rankType == RankType.NewWorld)
					{
						this.textPower.text = DxxTools.FormatNumber((long)this.rankUser.UserInfo.Power);
						this.powerObj.SetActiveSafe(true);
						return;
					}
				}
			}
			else
			{
				this.modelItem.ClearModel();
				this.infoObj.SetActiveSafe(false);
				this.emptyObj.SetActiveSafe(true);
			}
		}

		public void ShowCustomNewWorld()
		{
			this.powerObj.SetActiveSafe(false);
			if (this.TitleCtrl != null)
			{
				if (this.rankUser != null && this.rankUser.UserInfo != null)
				{
					this.TitleCtrl.SetAndFresh((int)this.rankUser.UserInfo.TitleId);
				}
				this.guildObj.SetActiveSafe(false);
				return;
			}
			if (this.rankUser != null && this.rankUser.UserInfo != null && this.rankUser.UserInfo.GuildDto != null)
			{
				this.textGuildName.text = this.rankUser.UserInfo.GuildDto.GuildName;
				this.guildIcon.SetIcon((int)this.rankUser.UserInfo.GuildDto.Avatar);
				this.guildObj.SetActiveSafe(true);
				return;
			}
			this.guildObj.SetActiveSafe(false);
		}

		public UIModelItem modelItem;

		public CustomText textPlayerName;

		public CustomText textGuildName;

		public UITitleCtrl TitleCtrl;

		public UIGuildIcon guildIcon;

		public GameObject infoObj;

		public GameObject guildObj;

		public GameObject emptyObj;

		public GameObject powerObj;

		public CustomText textPower;

		private RankUserDto rankUser;
	}
}
