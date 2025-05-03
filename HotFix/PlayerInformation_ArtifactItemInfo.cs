using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Common;
using UnityEngine;

namespace HotFix
{
	public class PlayerInformation_ArtifactItemInfo : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void RefreshUI(ArtifactInfo artifactInfo)
		{
			try
			{
				if (artifactInfo != null && artifactInfo.SkillArtifactId > 0U)
				{
					Artifact_advanceArtifact elementById = GameApp.Table.GetManager().GetArtifact_advanceArtifactModelInstance().GetElementById((int)artifactInfo.SkillArtifactId);
					int quality = elementById.quality;
					Quality_itemQuality elementById2 = GameApp.Table.GetManager().GetQuality_itemQualityModelInstance().GetElementById(quality);
					int initSkill = elementById.initSkill;
					GameSkill_skill elementById3 = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(initSkill);
					this.imgQualityFrame.SetImage(elementById2.atlasId, elementById2.bgSpriteName);
					this.imgIcon.SetImage(elementById3.iconAtlasID, elementById3.icon);
					this.txtLv.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_stage", new object[] { artifactInfo.Stage });
				}
				else
				{
					this.txtLv.text = "";
				}
				this.content.SetActive(artifactInfo != null && artifactInfo.SkillArtifactId > 0U);
				this.empty.SetActive(artifactInfo == null || artifactInfo.SkillArtifactId <= 0U);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				this.content.SetActive(false);
				this.empty.SetActive(true);
			}
		}

		public GameObject content;

		public GameObject empty;

		public CustomImage imgQualityFrame;

		public CustomImage imgIcon;

		public CustomText txtLv;
	}
}
