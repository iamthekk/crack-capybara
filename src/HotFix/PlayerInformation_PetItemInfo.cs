using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Common;
using Server;
using UnityEngine;

namespace HotFix
{
	public class PlayerInformation_PetItemInfo : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.btnItem.m_onClick = new Action(this.OnBtnItemClick);
		}

		protected override void OnDeInit()
		{
			this.btnItem.m_onClick = null;
		}

		public void RefreshUI(PetDto petDto)
		{
			this.data = petDto;
			this.btnItem.enabled = petDto != null;
			try
			{
				if (petDto != null)
				{
					this.txtLv.text = Singleton<LanguageManager>.Instance.GetInfoByID("text_level_n", new object[] { petDto.PetLv });
					int petConfigId = petDto.GetPetConfigId(GameApp.Table.GetManager());
					Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(petConfigId);
					GameMember_member elementById2 = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(elementById.memberId);
					this.imgIcon.SetImage(elementById2.iconAtlasID, elementById2.iconSpriteName);
					Quality_petQuality elementById3 = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(elementById.quality);
					if (elementById3 == null)
					{
						HLog.LogError(string.Format("quality: {0}, Pet quality is invalid!!", elementById.quality));
						return;
					}
					this.imgQualityFrame.SetImage(elementById3.atlasId, elementById3.bgSpriteName);
				}
				else
				{
					this.txtLv.text = "";
				}
				this.content.SetActive(petDto != null);
				this.empty.SetActive(petDto == null);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				this.content.SetActive(false);
				this.empty.SetActive(true);
			}
		}

		private void OnBtnItemClick()
		{
			if (this.data != null)
			{
				if (GameApp.View.IsOpened(ViewName.PetItemTipInfoViewModule))
				{
					GameApp.View.CloseView(ViewName.PetItemTipInfoViewModule, null);
				}
				PetItemTipInfoViewModule.OpenData openData = new PetItemTipInfoViewModule.OpenData();
				openData.petDto = this.data;
				GameApp.View.OpenView(ViewName.PetItemTipInfoViewModule, openData, 2, null, null);
			}
		}

		public CustomButton btnItem;

		public GameObject content;

		public GameObject empty;

		public CustomImage imgQualityFrame;

		public CustomImage imgIcon;

		public CustomText txtLv;

		private PetDto data;
	}
}
