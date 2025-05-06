using System;
using DG.Tweening;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIBonusRewardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.item.Init();
		}

		protected override void OnDeInit()
		{
			this.item.DeInit();
			this.sequencePool.Clear(false);
		}

		public void SetData(ItemData data, int index)
		{
			this.itemData = data;
			this.mIndex = index;
			if (data != null)
			{
				this.item.SetData(data.ToPropData());
				this.item.OnRefresh();
			}
			for (int i = 0; i < this.fxArr.Length; i++)
			{
				UIBonusRewardItem.QualityEffect qualityEffect = this.fxArr[i];
				if (qualityEffect != null && qualityEffect.fx != null)
				{
					qualityEffect.fx.gameObject.SetActiveSafe(false);
				}
			}
		}

		public void ShowReward()
		{
			base.gameObject.SetActiveSafe(true);
			base.transform.localScale = Vector3.zero;
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(base.transform, 1.2f, 0.15f)), ShortcutExtensions.DOScale(base.transform, 1f, 0.05f));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				if (this.itemData != null)
				{
					int i = 0;
					while (i < this.fxArr.Length)
					{
						UIBonusRewardItem.QualityEffect qualityEffect = this.fxArr[i];
						if (qualityEffect != null && qualityEffect.quality == this.itemData.Data.quality)
						{
							if (qualityEffect.fx)
							{
								qualityEffect.fx.gameObject.SetActiveSafe(true);
								return;
							}
							break;
						}
						else
						{
							i++;
						}
					}
				}
			});
		}

		public UIItem item;

		public UIBonusRewardItem.QualityEffect[] fxArr;

		private ItemData itemData;

		private int mIndex;

		private SequencePool sequencePool = new SequencePool();

		[Serializable]
		public class QualityEffect
		{
			public int quality;

			public ParticleSystem fx;
		}
	}
}
