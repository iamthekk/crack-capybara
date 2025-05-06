using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class PetLevelEffectItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(PetLevelEffectTipViewModule.ItemNodeData data, bool isEnd)
		{
			int petId = data.petId;
			int petLevel = data.petLevel;
			int itemLevel = data.itemLevel;
			Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(petId);
			this.imgNormalReach.gameObject.SetActive(!isEnd && petLevel >= itemLevel);
			this.imgNormalUnreach.gameObject.SetActive(!isEnd && petLevel < itemLevel);
			this.imgSpecialReach.gameObject.SetActive(isEnd && petLevel >= itemLevel);
			this.imgSpecialUnreach.gameObject.SetActive(isEnd && petLevel < itemLevel);
			this.imgBgUnlock.gameObject.SetActive(petLevel >= itemLevel);
			this.imgBgLock.gameObject.SetActive(petLevel < itemLevel);
			this.imgIconLock.gameObject.SetActive(petLevel < itemLevel);
			this.txtLevel.text = itemLevel.ToString();
			int petLevelEffectId = elementById.GetPetLevelEffectId(itemLevel);
			Pet_petLevelEffect pet_petLevelEffect = ((petLevelEffectId > 0) ? GameApp.Table.GetManager().GetPet_petLevelEffectModelInstance().GetElementById(petLevelEffectId) : null);
			this.txtDesc.color = ((petLevel >= itemLevel) ? this.txtUnlockColor : this.txtLockColor);
			if (pet_petLevelEffect != null)
			{
				this.txtDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID(pet_petLevelEffect.levelupDesc);
				return;
			}
			this.txtDesc.text = string.Empty;
		}

		public Color32 txtUnlockColor;

		public Color32 txtLockColor;

		public CustomImage imgBgUnlock;

		public CustomImage imgBgLock;

		public CustomImage imgIconLock;

		public CustomImage imgNormalReach;

		public CustomImage imgNormalUnreach;

		public CustomImage imgSpecialReach;

		public CustomImage imgSpecialUnreach;

		public CustomText txtLevel;

		public CustomText txtDesc;
	}
}
