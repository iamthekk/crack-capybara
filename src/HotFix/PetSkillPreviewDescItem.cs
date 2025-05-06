using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class PetSkillPreviewDescItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int itemIndex, PetSkillEffectTipViewModule.ItemNodeData data, LoopListView2 loopList)
		{
			int itemIndex2 = data.itemIndex;
			int skillGroupId = data.battleSkill.skillGroupId;
			Pet_petSkill elementById = GameApp.Table.GetManager().GetPet_petSkillModelInstance().GetElementById(skillGroupId);
			int petLevel = data.petLevel;
			int[] unlockLevel = elementById.unlockLevel;
			int[] level = elementById.level;
			bool flag = itemIndex2 < unlockLevel.Length && petLevel >= unlockLevel[itemIndex2];
			int num = level[itemIndex2];
			int num2 = ((itemIndex2 < unlockLevel.Length) ? unlockLevel[itemIndex2] : 1);
			this.txtTitle.color = (flag ? this.colorTitleUnlock : this.colorTitleLock);
			this.txtDesc.color = (flag ? this.colorDescUnlock : this.colorDescLock);
			GameSkill_skill elementById2 = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(num);
			this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("text_level_n", new object[] { num2 });
			this.txtDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.fullDetailID);
			this.txtUnlock.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_skill_unlock", new object[] { num2 });
			this.txtUnlock.gameObject.SetActive(!flag);
			this.imgBgTitle1.gameObject.SetActive(flag);
			this.imgBgTitle2.gameObject.SetActive(!flag);
			this.imgBg1.gameObject.SetActive(flag);
			this.imgBg2.gameObject.SetActive(!flag);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTransform);
			loopList.OnItemSizeChanged(itemIndex);
		}

		public RectTransform rectTransform;

		public Color32 colorTitleUnlock;

		public Color32 colorTitleLock;

		public Color32 colorDescUnlock;

		public Color32 colorDescLock;

		public CustomImage imgBg1;

		public CustomImage imgBg2;

		public CustomImage imgBgTitle1;

		public CustomImage imgBgTitle2;

		public CustomText txtTitle;

		public CustomText txtDesc;

		public CustomText txtUnlock;
	}
}
