using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine.Events;

namespace HotFix
{
	public class PetSkillItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.btnItem.onClick.AddListener(new UnityAction(this.OnBtnItemClick));
		}

		protected override void OnDeInit()
		{
			this.onItemClickCallback = null;
			this.btnItem.onClick.RemoveListener(new UnityAction(this.OnBtnItemClick));
			this.data = null;
		}

		public void SetData(PetSkillData skillData, int quality)
		{
			this.data = skillData;
			this.quality = quality;
			this.RefreshSkill();
		}

		private void RefreshSkill()
		{
			GameSkill_skill elementById = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(this.data.curSkillId);
			this.imgSkillIcon.SetImage(elementById.iconAtlasID, elementById.icon);
			Quality_petQuality elementById2 = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(this.quality);
			this.imgSkillBg.SetImage(elementById2.atlasId, elementById2.bgSpriteName);
			this.txtLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("text_level_n", new object[] { this.data.skillLevel });
		}

		private void OnBtnItemClick()
		{
			Action<PetSkillItem> action = this.onItemClickCallback;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		public CustomButton btnItem;

		public CustomText txtLevel;

		public CustomImage imgSkillBg;

		public CustomImage imgSkillIcon;

		[NonSerialized]
		public Action<PetSkillItem> onItemClickCallback;

		[NonSerialized]
		public PetSkillData data;

		[NonSerialized]
		public int quality;
	}
}
