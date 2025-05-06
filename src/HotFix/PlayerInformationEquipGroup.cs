using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Proto.User;
using Server;
using UnityEngine;

namespace HotFix
{
	public class PlayerInformationEquipGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Skin_ModelItem.Init();
			this.Skin_ModelItem.OnShow();
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.Init();
			}
			for (int i = 0; i < this.m_equips.Count; i++)
			{
				PlayerInformationEquipNode playerInformationEquipNode = this.m_equips[i];
				if (!(playerInformationEquipNode == null))
				{
					playerInformationEquipNode.Init();
					playerInformationEquipNode.m_equipmentItem.SetActive(false);
				}
			}
		}

		protected override void OnDeInit()
		{
			GameApp.Data.GetDataModule(DataName.ClothesDataModule).PopUIModelItem(this.Skin_ModelItem);
			this.Skin_ModelItem.OnHide(false);
			this.Skin_ModelItem.DeInit();
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.DeInit();
			}
			for (int i = 0; i < this.m_equips.Count; i++)
			{
				PlayerInformationEquipNode playerInformationEquipNode = this.m_equips[i];
				if (!(playerInformationEquipNode == null))
				{
					playerInformationEquipNode.DeInit();
				}
			}
		}

		public void RefreshUI(PlayerInfoDto playerInfo, string cameraKey)
		{
			if (playerInfo == null)
			{
				return;
			}
			this.m_playerInfoDto = playerInfo;
			this.mCameraKey = cameraKey;
			List<EquipData> list = EquipData.EquipListFix(playerInfo.Equipments);
			for (int i = 0; i < this.m_equips.Count; i++)
			{
				PlayerInformationEquipNode playerInformationEquipNode = this.m_equips[i];
				if (!(playerInformationEquipNode == null))
				{
					EquipType equipType = playerInformationEquipNode.m_equipType;
					int equipTypeIndex = playerInformationEquipNode.m_equipTypeIndex;
					EquipData equipData = this.GetEquipData(list, equipType, equipTypeIndex);
					if (equipType == EquipType.Weapon)
					{
						this.weaponId = (int)((equipData != null) ? equipData.id : 0U);
					}
					playerInformationEquipNode.RefreshData(equipData);
				}
			}
			GameApp.Data.GetDataModule(DataName.ClothesDataModule).PushUIModelItem(this.Skin_ModelItem, new Action(this.FreshSkin));
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.SetAndFresh((int)playerInfo.TitleId);
			}
		}

		private EquipData GetEquipData(List<EquipData> list, EquipType type, int equipIndex = 0)
		{
			int num = EquipTypeHelper.GetEquipTypeStartIndex(type) + equipIndex;
			if (list != null && list.Count > num)
			{
				return list[num];
			}
			return null;
		}

		private void FreshSkin()
		{
			if (this.m_playerInfoDto == null)
			{
				return;
			}
			int memberID = GameApp.Data.GetDataModule(DataName.HeroDataModule).MainCardData.m_memberID;
			Dictionary<SkinType, SkinData> skinDatas = ClothesDataModule.GetPlayerClothesData(this.m_playerInfoDto).GetSkinDatas();
			int mountMemberId = GameApp.Data.GetDataModule(DataName.MountDataModule).GetMountMemberId(this.m_playerInfoDto.MountInfo);
			this.Skin_ModelItem.rectTransform.anchoredPosition = ((mountMemberId > 0) ? this.Skin_ModelItemPos_Mount : this.Skin_ModelItemPos_Normal);
			this.Skin_ModelItem.rectTransform.localScale = ((mountMemberId > 0) ? this.Skin_ModelItemScale_Mount : this.Skin_ModelItemScale_Normal) * Vector3.one;
			this.Skin_ModelItem.ShowPlayerModel(this.mCameraKey, memberID, this.weaponId, skinDatas, mountMemberId);
		}

		[Header("称号")]
		public UITitleCtrl TitleCtrl;

		[Header("皮肤")]
		public UIModelItem Skin_ModelItem;

		public Vector2 Skin_ModelItemPos_Normal = new Vector2(0f, -160f);

		public Vector2 Skin_ModelItemPos_Mount = new Vector2(-20f, -200f);

		public float Skin_ModelItemScale_Normal = 2f;

		public float Skin_ModelItemScale_Mount = 1.5f;

		public List<PlayerInformationEquipNode> m_equips;

		private PlayerInfoDto m_playerInfoDto;

		private string mCameraKey;

		private int weaponId;
	}
}
