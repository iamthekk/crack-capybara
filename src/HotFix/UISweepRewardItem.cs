using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UISweepRewardItem : CustomBehaviour
	{
		public int ItemId { get; private set; }

		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int id)
		{
			this.ItemId = id;
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(id);
			if (elementById != null)
			{
				string atlasPath = GameApp.Table.GetAtlasPath(elementById.atlasID);
				this.imageIcon.SetImage(atlasPath, elementById.icon);
			}
			this.textNum.text = "0";
		}

		public void SetNum(long num, bool isAni = true)
		{
			if (isAni)
			{
				long numValue = this.itemNum;
				TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.OnUpdate<Sequence>(TweenSettingsExtensions.Append(this.sequencePool.Get(), DOTween.To(() => numValue, delegate(long x)
				{
					numValue = x;
				}, num, 0.4f)), delegate
				{
					this.textNum.text = DxxTools.FormatNumber(numValue);
				}), delegate
				{
					this.itemNum = num;
					this.textNum.text = DxxTools.FormatNumber(this.itemNum);
				});
				return;
			}
			this.textNum.text = DxxTools.FormatNumber(num);
		}

		public Transform GetFlyNode()
		{
			return this.imageIcon.transform;
		}

		public CustomImage imageIcon;

		public CustomText textNum;

		private long itemNum;

		private SequencePool sequencePool = new SequencePool();
	}
}
