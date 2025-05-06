using System;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class CardFlippingRewardPreviewItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.effectNode.gameObject.SetActive(false);
			ShortcutExtensions46.DOFade(this.imgMask, 1f, 0f);
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(CardFlippingViewModule.CardFlippingRewardType rewardType, ChapterMiniGame_cardFlippingBase cfg)
		{
			this.rewardType = rewardType;
			this.cfg = cfg;
			this.UpdateView();
		}

		public void PlayRewardAnimation()
		{
			ShortcutExtensions46.DOFade(this.imgMask, 0f, 0f);
			this.effectNode.gameObject.SetActive(true);
		}

		public void PlayEndRewardAnimation()
		{
		}

		private void UpdateView()
		{
			int num = (int)this.rewardType;
			string[] array = null;
			string[] array2 = null;
			if (num == 0)
			{
				array = this.cfg.iconRes1;
				array2 = this.cfg.iconFrameRes1;
				string[] reward = this.cfg.reward1;
			}
			else if (num == 1)
			{
				array = this.cfg.iconRes2;
				array2 = this.cfg.iconFrameRes2;
				string[] reward2 = this.cfg.reward2;
			}
			else if (num == 2)
			{
				array = this.cfg.iconRes3;
				array2 = this.cfg.iconFrameRes3;
				string[] reward3 = this.cfg.reward3;
			}
			if (array != null && array.Length == 2)
			{
				this.imgIcon.SetImage(int.Parse(array[0]), array[1]);
			}
			if (array2 != null && array2.Length == 2)
			{
				this.imgFrame.enabled = true;
				this.imgFrame.SetImage(int.Parse(array2[0]), array2[1]);
				return;
			}
			this.imgFrame.enabled = false;
		}

		public Transform scaleNode;

		public CustomImage imgBg;

		public CustomImage imgFrame;

		public CustomImage imgIcon;

		public CustomImage imgMask;

		public GameObject effectNode;

		private ChapterMiniGame_cardFlippingBase cfg;

		private CardFlippingViewModule.CardFlippingRewardType rewardType;
	}
}
