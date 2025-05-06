using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class CardFlippingRewardPreviewGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.endEffect.gameObject.SetActive(false);
			for (int i = 0; i < this.rewardPreviewItemList.Count; i++)
			{
				CardFlippingRewardPreviewItem cardFlippingRewardPreviewItem = this.rewardPreviewItemList[i];
				if (cardFlippingRewardPreviewItem != null)
				{
					cardFlippingRewardPreviewItem.Init();
				}
			}
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.rewardPreviewItemList.Count; i++)
			{
				CardFlippingRewardPreviewItem cardFlippingRewardPreviewItem = this.rewardPreviewItemList[i];
				if (cardFlippingRewardPreviewItem != null)
				{
					cardFlippingRewardPreviewItem.DeInit();
				}
			}
		}

		public void SetData(ChapterMiniGame_cardFlippingBase cfg)
		{
			this.cfg = cfg;
			for (int i = 0; i < this.rewardPreviewItemList.Count; i++)
			{
				CardFlippingRewardPreviewItem cardFlippingRewardPreviewItem = this.rewardPreviewItemList[i];
				if (cardFlippingRewardPreviewItem != null)
				{
					cardFlippingRewardPreviewItem.SetData(this.rewardType, cfg);
				}
			}
			if (this.rewardType == CardFlippingViewModule.CardFlippingRewardType.BigReward)
			{
				if (cfg.reward3[0] == "3")
				{
					ItemData itemData = new ItemData(int.Parse(cfg.reward3[1]), long.Parse(cfg.reward3[2]));
					Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID);
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
					this.textTitle.text = string.Format("{0} x{1}", infoByID, itemData.TotalCount);
					return;
				}
				this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(cfg.rewardTextIds[2]);
				return;
			}
			else
			{
				if (this.rewardType == CardFlippingViewModule.CardFlippingRewardType.MiddleReward)
				{
					this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(cfg.rewardTextIds[1]);
					return;
				}
				this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(cfg.rewardTextIds[0]);
				return;
			}
		}

		public void PlayRewardAnimation(int result, int count)
		{
			if (count > 0 && count <= this.rewardPreviewItemList.Count)
			{
				CardFlippingRewardPreviewItem cardFlippingRewardPreviewItem = this.rewardPreviewItemList[count - 1];
				if (cardFlippingRewardPreviewItem == null)
				{
					return;
				}
				cardFlippingRewardPreviewItem.PlayRewardAnimation();
			}
		}

		public void PlayEndRewardAnimation()
		{
			switch (this.rewardType)
			{
			case CardFlippingViewModule.CardFlippingRewardType.SmallReward:
				GameApp.Sound.PlayClip(608, 1f);
				break;
			case CardFlippingViewModule.CardFlippingRewardType.MiddleReward:
				GameApp.Sound.PlayClip(609, 1f);
				break;
			case CardFlippingViewModule.CardFlippingRewardType.BigReward:
				GameApp.Sound.PlayClip(610, 1f);
				break;
			}
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(base.transform, Vector3.one * 1.3f, 0.25f), 27));
			TweenSettingsExtensions.AppendInterval(sequence, 0.4f);
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(base.transform, Vector3.one * 1.15f, 0.25f), 26));
			for (int i = 0; i < this.rewardPreviewItemList.Count; i++)
			{
				CardFlippingRewardPreviewItem cardFlippingRewardPreviewItem = this.rewardPreviewItemList[i];
				if (cardFlippingRewardPreviewItem != null)
				{
					cardFlippingRewardPreviewItem.PlayEndRewardAnimation();
				}
			}
			this.endEffect.gameObject.SetActive(true);
			string text;
			if (this.rewardType == CardFlippingViewModule.CardFlippingRewardType.BigReward)
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID(this.cfg.rewardTextIds[2]);
			}
			else if (this.rewardType == CardFlippingViewModule.CardFlippingRewardType.MiddleReward)
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID(this.cfg.rewardTextIds[1]);
			}
			else
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID(this.cfg.rewardTextIds[0]);
			}
			this.textTitle.text = "<color=#311B16>" + text + "</color>";
		}

		public GameObject endEffect;

		public CardFlippingViewModule.CardFlippingRewardType rewardType;

		public CustomText textTitle;

		public List<CardFlippingRewardPreviewItem> rewardPreviewItemList = new List<CardFlippingRewardPreviewItem>();

		private ChapterMiniGame_cardFlippingBase cfg;
	}
}
