using System;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class PetTypeAttribute : MonoBehaviour
	{
		public void SetData(int petId, int quality)
		{
			GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(petId);
			Quality_petQuality elementById = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(quality);
			Atlas_atlas elementById2 = GameApp.Table.GetManager().GetAtlas_atlasModelInstance().GetElementById(elementById.atlasId);
			string text = ((elementById2 != null) ? elementById2.path : "");
			this.m_imgTypeBg.SetImage(text, elementById.typeBgSpriteName);
			this.m_imgTxtBg.SetImage(text, elementById.typeTxtBg);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			this.m_txtPetQuality.text = string.Concat(new string[] { "<color=", elementById.colorNum, ">", infoByID, "</color>" });
		}

		public CustomText m_txtPetQuality;

		public CustomImage m_imgTypeBg;

		public CustomImage m_imgTxtBg;
	}
}
