using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Proto.Common;
using Proto.User;

namespace HotFix
{
	public class PlayerInformationPetsNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			for (int i = 0; i < this.m_petItemInfoList.Count; i++)
			{
				PlayerInformation_PetItemInfo playerInformation_PetItemInfo = this.m_petItemInfoList[i];
				if (!(playerInformation_PetItemInfo == null))
				{
					playerInformation_PetItemInfo.Init();
				}
			}
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.m_petItemInfoList.Count; i++)
			{
				PlayerInformation_PetItemInfo playerInformation_PetItemInfo = this.m_petItemInfoList[i];
				if (!(playerInformation_PetItemInfo == null))
				{
					playerInformation_PetItemInfo.DeInit();
				}
			}
		}

		public void RefreshUI(PlayerInfoDto playerInfo)
		{
			this.m_playerInfo = playerInfo;
			for (int i = 0; i < this.m_petItemInfoList.Count; i++)
			{
				PlayerInformation_PetItemInfo playerInformation_PetItemInfo = this.m_petItemInfoList[i];
				if (!(playerInformation_PetItemInfo == null))
				{
					playerInformation_PetItemInfo.RefreshUI(this.GetPetDto(i));
				}
			}
		}

		private PetDto GetPetDto(int index)
		{
			if (this.m_playerInfo == null || this.m_playerInfo.Pets == null)
			{
				return null;
			}
			for (int i = 0; i < this.m_playerInfo.Pets.Count; i++)
			{
				PetDto petDto = this.m_playerInfo.Pets[i];
				if ((ulong)petDto.FormationPos == (ulong)((long)(index + 1)))
				{
					return petDto;
				}
			}
			return null;
		}

		public List<PlayerInformation_PetItemInfo> m_petItemInfoList = new List<PlayerInformation_PetItemInfo>();

		private PlayerInfoDto m_playerInfo;
	}
}
