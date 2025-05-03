using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class TalentReward : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.effect.SetActive(false);
			this.btnItem.m_onClick = new Action(this.OnBtnItemClick);
		}

		protected override void OnDeInit()
		{
			this.btnItem.m_onClick = null;
		}

		public void SetData(int talentId)
		{
			this.cfg = GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetElementById(talentId);
			this.rewardType = this.cfg.rewardType;
			this.rewards = this.cfg.reward;
			if (this.rewardType == 1)
			{
				ItemData itemData = this.rewards.ToItemDataList()[0];
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID);
				this.imgIcon.SetImage(elementById.atlasID, elementById.icon);
				this.txtNum.text = "x" + DxxTools.FormatNumber(itemData.TotalCount);
				return;
			}
			if (this.rewardType == 2 || this.rewardType == 3)
			{
				this.imgIcon.SetImage(this.cfg.iconAtlasID, this.cfg.iconID);
				this.txtNum.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.cfg.talentName);
				return;
			}
			if (this.cfg.evolution > 0)
			{
				if (this.cfg.iconAtlasID > 0)
				{
					this.imgIcon.SetImage(this.cfg.iconAtlasID, this.cfg.iconID);
				}
				if (!string.IsNullOrEmpty(this.cfg.talentName))
				{
					this.txtNum.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.cfg.talentName);
					return;
				}
				this.txtNum.text = "";
			}
		}

		private void OnBtnItemClick()
		{
			if (this.rewardType == 1)
			{
				ItemData itemData = this.rewards.ToItemDataList()[0];
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID);
				this.imgIcon.SetImage(elementById.atlasID, elementById.icon);
				this.txtNum.text = "x" + DxxTools.FormatNumber(itemData.TotalCount);
				PropData propData = itemData.ToPropData();
				float num = base.rectTransform.rect.height * base.rectTransform.localScale.y * 0.5f + 10f;
				DxxTools.UI.OnItemClick(null, propData, null, base.transform.position, num);
				return;
			}
			if (!string.IsNullOrEmpty(this.cfg.talentName))
			{
				TalentRewardTipViewModule.OpenData openData = new TalentRewardTipViewModule.OpenData();
				openData.title = Singleton<LanguageManager>.Instance.GetInfoByID(this.cfg.talentName);
				openData.desc = Singleton<LanguageManager>.Instance.GetInfoByID(this.cfg.talentDesc);
				openData.clickPos = base.transform.position;
				openData.offsetY = this.imgIcon.rectTransform.rect.height * this.imgIcon.rectTransform.localScale.y * 0.5f + 10f;
				GameApp.View.OpenView(ViewName.TalentRewardTipViewModule, openData, 1, null, null);
			}
		}

		public CustomButton btnItem;

		public CustomImage imgIcon;

		public CustomText txtNum;

		public GameObject effect;

		public int rewardType;

		public string rewards;

		private TalentNew_talent cfg;
	}
}
