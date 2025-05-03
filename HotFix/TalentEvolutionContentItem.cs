using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class TalentEvolutionContentItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.talentReward.Init();
			if (this.btnTalentStepBig != null)
			{
				this.btnTalentStepBig.m_onClick = new Action(this.OnBtnTalentStepClick);
			}
			if (this.btnTalentStepSmall != null)
			{
				this.btnTalentStepSmall.m_onClick = new Action(this.OnBtnTalentStepClick);
			}
		}

		protected override void OnDeInit()
		{
			this.talentReward.DeInit();
			if (this.btnTalentStepBig != null)
			{
				this.btnTalentStepBig.m_onClick = null;
			}
			if (this.btnTalentStepSmall != null)
			{
				this.btnTalentStepSmall.m_onClick = null;
			}
		}

		public void SetData(int index, TalentNew_talent cfg)
		{
			this.index = index;
			this.cfgTalent = cfg;
			this.score0.SetActive(cfg.id == 1);
			if (this.cfgTalent.evolution > 0)
			{
				this.cfgTalentEvolution = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(this.cfgTalent.evolution);
			}
			else
			{
				this.cfgTalentEvolution = null;
			}
			TalentDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
			string text = string.Format("{0}", cfg.talentLevel);
			string text2;
			if (dataModule.talentProgressData.curId == cfg.id)
			{
				text2 = "<color=#FFF380>" + text + "</color>";
				int num = 0;
				TalentNew_talent elementById = GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetElementById(cfg.id - 1);
				if (elementById != null)
				{
					num = elementById.talentLevel;
				}
				this.progress.value = Mathf.Clamp01((float)(dataModule.TalentExp - num) / (float)(cfg.talentLevel - num));
			}
			else if (dataModule.TalentExp >= cfg.talentLevel)
			{
				text2 = "<color=#D5F44E>" + text + "</color>";
				this.progress.value = 1f;
			}
			else
			{
				text2 = "<color=#6B657F>" + text + "</color>";
				this.progress.value = 0f;
			}
			this.txtLevel.text = text2;
			int num2;
			if (cfg.evolution > 0)
			{
				num2 = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(cfg.evolution)
					.type;
			}
			else
			{
				num2 = cfg.rewardType + 100;
			}
			this.rewardGo.SetActive(false);
			if (cfg.evolution > 0)
			{
				TalentNew_talentEvolution elementById2 = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(cfg.evolution);
				if (num2 == 3)
				{
					this.talentStepNodeSmallGo.SetActive(false);
					this.talentStepNodeBigGo.SetActive(true);
					this.txtTalentStepDescBig.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.stepLanguageId);
					return;
				}
				this.talentStepNodeSmallGo.SetActive(true);
				this.talentStepNodeBigGo.SetActive(false);
				this.txtTalentStepDescSmall.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.stepLanguageId);
				return;
			}
			else
			{
				if (cfg.rewardType == 1 || cfg.iconAtlasID > 0)
				{
					this.rewardGo.SetActive(true);
					this.receiveCheckmark.SetActive(dataModule.TalentExp >= cfg.talentLevel);
					this.receivedGo.SetActive(dataModule.TalentExp >= cfg.talentLevel);
					this.talentStepNodeSmallGo.SetActive(false);
					this.talentStepNodeBigGo.SetActive(false);
					this.talentReward.SetData(cfg.id);
					return;
				}
				this.rewardGo.SetActive(false);
				this.receiveCheckmark.SetActive(false);
				this.receivedGo.SetActive(false);
				this.talentStepNodeSmallGo.SetActive(false);
				this.talentStepNodeBigGo.SetActive(false);
				return;
			}
		}

		private void OnBtnTalentStepClick()
		{
			if (this.cfgTalentEvolution != null)
			{
				RectTransform rectTransform = null;
				if (this.btnTalentStepSmall.IsActive())
				{
					rectTransform = this.btnTalentStepSmall.transform as RectTransform;
				}
				else if (this.btnTalentStepBig.IsActive())
				{
					rectTransform = this.btnTalentStepBig.transform as RectTransform;
				}
				if (rectTransform == null)
				{
					return;
				}
				TalentRewardTipViewModule.OpenData openData = new TalentRewardTipViewModule.OpenData();
				openData.title = Singleton<LanguageManager>.Instance.GetInfoByID(this.cfgTalentEvolution.stepLanguageId);
				openData.desc = Singleton<LanguageManager>.Instance.GetInfoByID(this.cfgTalentEvolution.desc);
				openData.clickPos = rectTransform.position;
				openData.offsetY = rectTransform.rect.height * rectTransform.localScale.y * 0.5f + 10f;
				GameApp.View.OpenView(ViewName.TalentRewardTipViewModule, openData, 1, null, null);
			}
		}

		public CustomText txtLevel;

		public Slider progress;

		public GameObject score0;

		public GameObject rewardGo;

		public TalentReward talentReward;

		public GameObject receivedGo;

		public GameObject receiveCheckmark;

		public GameObject talentStepNodeBigGo;

		public CustomButton btnTalentStepBig;

		public CustomText txtTalentStepDescBig;

		public GameObject talentStepNodeSmallGo;

		public CustomButton btnTalentStepSmall;

		public CustomText txtTalentStepDescSmall;

		public int index;

		private TalentNew_talent cfgTalent;

		private TalentNew_talentEvolution cfgTalentEvolution;
	}
}
