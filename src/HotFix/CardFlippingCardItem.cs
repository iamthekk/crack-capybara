using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class CardFlippingCardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.goFront.SetActive(false);
			this.goBack.SetActive(true);
			this.imgLight.gameObject.SetActive(false);
			this.btnItem.onClick.AddListener(new UnityAction(this.OnItemClick));
		}

		protected override void OnDeInit()
		{
			this.isOpened = false;
			this.btnItem.onClick.RemoveAllListeners();
		}

		private void OnItemClick()
		{
			GameApp.Sound.PlayClip(607, 1f);
			Action<CardFlippingCardItem> action = this.onItemClickCallback;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		public void PlayFlipping(int result, ChapterMiniGame_cardFlippingBase cfg, bool isFlippingEnd)
		{
			this.isOpened = true;
			this.result = result;
			this.cfg = cfg;
			this.UpdateView();
			this.goBack.SetActive(true);
			this.goFront.SetActive(false);
			Sequence sequence = DOTween.Sequence();
			Vector3 vector;
			vector..ctor(0f, 90f, 0f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOLocalRotate(this.rootScaleNode, vector, 0.25f, 0));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.goBack.SetActive(false);
				this.goFront.SetActive(true);
				this.PlayRewardAnimation(isFlippingEnd);
			});
			vector..ctor(0f, 0f, 0f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOLocalRotate(this.rootScaleNode, vector, 0.25f, 0));
			TweenExtensions.Play<Sequence>(sequence);
		}

		public void PlayRewardAnimation(bool isFlippingEnd)
		{
			if (!isFlippingEnd)
			{
				Sequence sequence = DOTween.Sequence();
				Tweener tweener = ShortcutExtensions.DOScale(this.rewardScaleNode, Vector3.one * 1.3f, 0f);
				TweenSettingsExtensions.Append(sequence, tweener);
				TweenSettingsExtensions.AppendInterval(sequence, 0.25f);
				TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.rewardScaleNode, Vector3.one, 0.15f), 26));
				TweenExtensions.Play<Sequence>(sequence);
				return;
			}
			ShortcutExtensions.DOScale(this.rewardScaleNode, Vector3.one * 1.3f, 0f);
		}

		public void PlayEndRewardAnimation()
		{
			this.imgLight.gameObject.SetActive(true);
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.rewardScaleNode, Vector3.one * 1.3f, 0.15f));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.rewardScaleNode, Vector3.one * 1.5f, 0.3f));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.rewardScaleNode, Vector3.one * 1.3f, 0.15f));
			TweenExtensions.Play<Sequence>(sequence);
		}

		private void UpdateView()
		{
			if (this.result == 0)
			{
				this.iconResInfo = this.cfg.iconRes1;
				this.iconFrameResInfo = this.cfg.iconFrameRes1;
				this.rewards = this.cfg.reward1;
			}
			else if (this.result == 1)
			{
				this.iconResInfo = this.cfg.iconRes2;
				this.iconFrameResInfo = this.cfg.iconFrameRes2;
				this.rewards = this.cfg.reward2;
			}
			else if (this.result == 2)
			{
				this.iconResInfo = this.cfg.iconRes3;
				this.iconFrameResInfo = this.cfg.iconFrameRes3;
				this.rewards = this.cfg.reward3;
			}
			if (this.iconResInfo != null && this.iconResInfo.Length == 2)
			{
				this.imgIcon.SetImage(int.Parse(this.iconResInfo[0]), this.iconResInfo[1]);
			}
			if (int.Parse(this.rewards[0]) == 2)
			{
				this.imgFrame.enabled = true;
				this.imgFrame.SetImage(int.Parse(this.iconFrameResInfo[0]), this.iconFrameResInfo[1]);
				return;
			}
			this.imgFrame.enabled = false;
		}

		public void PlayBackEffect()
		{
			this.backEffect.gameObject.SetActive(false);
			this.backEffect.gameObject.SetActive(true);
		}

		public GameObject backEffect;

		public CustomButton btnItem;

		public Transform rootScaleNode;

		public Transform rewardScaleNode;

		public GameObject goBack;

		public GameObject goFront;

		public CustomImage imgLight;

		public CustomImage imgMask;

		public CustomImage imgIcon;

		public CustomImage imgFrame;

		public Action<CardFlippingCardItem> onItemClickCallback;

		public bool isOpened;

		public int result = -1;

		public ChapterMiniGame_cardFlippingBase cfg;

		public string[] rewards;

		public string[] iconResInfo;

		public string[] iconFrameResInfo;
	}
}
