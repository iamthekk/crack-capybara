using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine.UI;

namespace HotFix
{
	public class PetLevelProgressItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.btnInfo.m_onClick = new Action(this.OnBtnInfoClick);
		}

		protected override void OnDeInit()
		{
			this.btnInfo.m_onClick = null;
		}

		public void SetData(int petId, int level)
		{
			this.petId = petId;
			this.petLevel = level;
			int num = 20;
			int num2 = level / num * num;
			int i = num2 + num;
			int petMaxLevel = Singleton<GameConfig>.Instance.PetMaxLevel;
			while (i > petMaxLevel)
			{
				i -= 5;
				num2 -= 5;
			}
			int num3 = level - num2;
			int num4 = level / 5 * 5 + 5;
			this.expSlider.value = (float)num3 / (float)num;
			for (int j = 0; j < this.petLevelItems.Count; j++)
			{
				PetLevelItem petLevelItem = this.petLevelItems[j];
				int num5 = num2 + 5 * j;
				petLevelItem.SetData(level, num5, j == this.petLevelItems.Count - 1);
			}
			if (num4 > Singleton<GameConfig>.Instance.PetMaxLevel)
			{
				this.textAttribute.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_max_level");
				return;
			}
			int petLevelEffectId = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(petId)
				.GetPetLevelEffectId(num4);
			Pet_petLevelEffect elementById = GameApp.Table.GetManager().GetPet_petLevelEffectModelInstance().GetElementById(petLevelEffectId);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("pet_training_lv_add_prefix", new object[] { num4 });
			string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.levelupDesc);
			this.textAttribute.text = infoByID + infoByID2;
		}

		private void OnBtnInfoClick()
		{
			if (this.petId > 0 && this.petLevel > 0)
			{
				PetLevelEffectTipViewModule.OpenData openData = new PetLevelEffectTipViewModule.OpenData();
				openData.petId = this.petId;
				openData.petLevel = this.petLevel;
				GameApp.View.OpenView(ViewName.PetLevelEffectTipViewModule, openData, 2, null, null);
			}
		}

		public CustomText textTitle;

		public CustomButton btnInfo;

		public List<PetLevelItem> petLevelItems = new List<PetLevelItem>();

		public Slider expSlider;

		public CustomText textAttribute;

		private int petId;

		private int petLevel;
	}
}
