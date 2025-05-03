using System;
using DG.Tweening;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIMainChest_ScoreNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.rewardRedNode.Value = 0;
			this.chestDataModule = GameApp.Data.GetDataModule(DataName.ChestDataModule);
			this.btnScoreReward.m_onClick = new Action(this.OnBtnScoreRewardClick);
		}

		protected override void OnDeInit()
		{
			this.btnScoreReward.m_onClick = null;
		}

		public void UpdateProgress(bool isAnimation = false)
		{
			if (!isAnimation)
			{
				this.UpdateProgressImpl();
				return;
			}
			long num = this.chestDataModule.GetCurScore();
			if (this.curScore < this.maxScore && num > this.curScore)
			{
				this.cfg = GameApp.Table.GetManager().GetChestList_ChestListModelInstance().GetElementById(this.chestDataModule.curRewardConfigId);
				this.curScore = num;
				this.maxScore = this.chestDataModule.maxScore;
				float num2 = Mathf.Clamp01((float)this.curScore * 1f / (float)this.maxScore);
				TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions46.DOValue(this.sliderScore, num2, 0.5f, false), delegate
				{
					this.UpdateProgressImpl();
				});
				this.imgSweep.fillAmount = 0f;
				TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions46.DOFillAmount(this.imgSweep, num2, 0.5f), 0.2f), delegate
				{
					this.imgSweep.fillAmount = 0f;
				});
				return;
			}
			this.UpdateProgressImpl();
		}

		private void UpdateProgressImpl()
		{
			this.cfg = GameApp.Table.GetManager().GetChestList_ChestListModelInstance().GetElementById(this.chestDataModule.curRewardConfigId);
			this.curScore = this.chestDataModule.GetCurScore();
			this.maxScore = this.chestDataModule.maxScore;
			string[] array = ((this.chestDataModule.curRewardType == 1) ? this.cfg.reward : this.cfg.rewardCircle);
			this.txtScoreValue.text = string.Format("{0}/{1}", this.curScore, this.maxScore);
			this.sliderScore.value = Mathf.Clamp01((float)this.curScore * 1f / (float)this.maxScore);
			if (this.curScore >= this.maxScore)
			{
				this.txtScoreReward.text = Singleton<LanguageManager>.Instance.GetInfoByID("chest_reward_active");
				this.btnScoreReward.GetComponent<UIGrays>().Recovery();
			}
			else
			{
				this.txtScoreReward.text = Singleton<LanguageManager>.Instance.GetInfoByID("chest_reward_not_active");
				this.btnScoreReward.GetComponent<UIGrays>().SetUIGray();
			}
			ItemData itemData = array.ToItemDataList()[0];
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID);
			this.ImageScoreRewardIcon.SetImage(elementById.atlasID, elementById.icon);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			long num = Utility.Math.Max(this.maxScore - this.curScore, 0L);
			this.txtScoreDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID("chest_score_desc_1", new object[] { num, infoByID });
			this.txtScoreValue.text = Singleton<LanguageManager>.Instance.GetInfoByID("chest_score_desc_2", new object[] { this.curScore, this.maxScore });
			this.rewardRedNode.Value = ((this.curScore >= this.maxScore) ? 1 : 0);
			if (this.curScore >= this.maxScore)
			{
				ShortcutExtensions.DOKill(this.imgSweep, false);
				this.imgSweep.fillAmount = 0f;
				TweenSettingsExtensions.SetLoops<Tweener>(ShortcutExtensions46.DOFillAmount(this.imgSweep, 1f, 2f), -1, 0);
			}
			else
			{
				ShortcutExtensions.DOKill(this.imgSweep, false);
				this.imgSweep.fillAmount = 0f;
			}
			string text = ((this.rewardRedNode.count > 0) ? "Shake" : "Idle");
			this.rewardAnimator.SetTrigger(text);
		}

		private void OnBtnScoreRewardClick()
		{
			if (this.curScore < this.maxScore)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("chest_score_not_enough"));
				return;
			}
			if (this.curScore >= 1000L && this.maxScore < 1000L)
			{
				RememberTipCommonViewModule.OpenData openData = new RememberTipCommonViewModule.OpenData();
				openData.rememberTipType = RememberTipType.MainChest;
				openData.contentStr = Singleton<LanguageManager>.Instance.GetInfoByID("chest_reward_getall_tip");
				openData.onCancelCallback = delegate
				{
					NetworkUtils.Chest.ChestRewardRequest(false, null);
				};
				openData.onConfirmCallback = delegate
				{
					NetworkUtils.Chest.ChestRewardRequest(true, null);
				};
				RememberTipCommonViewModule.TryRunRememberTip(openData);
				return;
			}
			NetworkUtils.Chest.ChestRewardRequest(false, null);
		}

		public CustomImage imgScoreIcon;

		public CustomImage ImageScoreRewardIcon;

		public CustomText txtScoreDesc;

		public CustomText txtScoreValue;

		public Slider sliderScore;

		public CustomButton btnScoreReward;

		public CustomText txtScoreReward;

		public RedNodeOneCtrl rewardRedNode;

		public Animator rewardAnimator;

		public CustomImage imgSweep;

		private ChestDataModule chestDataModule;

		private long curScore;

		private long maxScore;

		private ChestList_ChestList cfg;
	}
}
