using System;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class CommonProgressItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.buttonReward.onClick.AddListener(new UnityAction(this.OnClickReward));
			this.rewardEffect.gameObject.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			this.buttonReward.onClick.RemoveListener(new UnityAction(this.OnClickReward));
		}

		public void SetData(List<ItemData> rewards)
		{
			this.showRewards = rewards;
			if (this.showRewards.Count > 0)
			{
				ItemData itemData = this.showRewards[0];
				itemData.SetReCalc();
				Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(itemData.ID);
				if (item_Item != null)
				{
					string atlasPath = GameApp.Table.GetAtlasPath(item_Item.atlasID);
					this.imageRewardIcon.SetImage(atlasPath, item_Item.icon);
					this.imageRewardNum.text = "x" + DxxTools.FormatNumber(itemData.TotalCount);
				}
			}
			this.HideEffect();
		}

		public void HideEffect()
		{
			this.rewardEffect.gameObject.SetActiveSafe(false);
		}

		public Sequence ShowEffect()
		{
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.rewardEffect.gameObject.SetActiveSafe(true);
				this.rewardEffect.Stop();
				this.rewardEffect.Clear();
				this.rewardEffect.Play();
			});
			TweenSettingsExtensions.AppendInterval(sequence, 1f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.rewardEffect.gameObject.SetActiveSafe(false);
			});
			return sequence;
		}

		public Sequence DoCanvasFade()
		{
			this.rewardCanvas.alpha = 1f;
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.rewardCanvas, 0f, 0.2f));
			return sequence;
		}

		public Sequence DoCanvasFadeScale()
		{
			this.rewardCanvas.alpha = 0f;
			this.rewardCanvas.transform.localScale = Vector3.one;
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.rewardCanvas, 1f, 0.2f)), ShortcutExtensions.DOScale(this.rewardCanvas.transform, 1.05f, 0.2f));
			return sequence;
		}

		public Sequence DoCanvasScale()
		{
			Sequence sequence = DOTween.Sequence();
			this.rewardCanvas.transform.localScale = Vector3.zero;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.rewardCanvas.transform, 1f, 0.1f));
			return sequence;
		}

		private void OnClickReward()
		{
			if (this.IsLock)
			{
				return;
			}
			UIBoxInfoViewModule.Transfer transfer = new UIBoxInfoViewModule.Transfer
			{
				nodeType = UIBoxInfoViewModule.UIBoxInfoNodeType.Up,
				rewards = this.showRewards,
				position = this.buttonReward.transform.position,
				anchoredPositionOffset = new Vector3(0f, 50f, 0f),
				secondLayer = true
			};
			GameApp.View.OpenView(ViewName.RewardBoxInfoViewModule, transfer, 1, null, null);
		}

		public void SetClickLock(bool isLock)
		{
			this.IsLock = isLock;
		}

		public CanvasGroup rewardCanvas;

		public CustomImage imageRewardIcon;

		public CustomText imageRewardNum;

		public CustomButton buttonReward;

		public UIParticle rewardEffect;

		private List<ItemData> showRewards;

		private bool IsLock;
	}
}
