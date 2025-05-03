using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIRankRewardCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.InitNode();
		}

		private void InitNode()
		{
			for (int i = 0; i < this.leftItems.Count; i++)
			{
				this.leftItems[i].gameObject.SetActive(false);
				this.leftItems[i].Init();
				this.leftItems[i].onClick = new Action<UIItem, PropData, object>(this.onClickItem);
			}
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.leftItems.Count; i++)
			{
				this.leftItems[i].DeInit();
			}
		}

		public void SetFresh(RankType rankType, RankReward reward)
		{
			base.gameObject.SetActiveSafe(true);
			this.InitNode();
			int rankStart = reward.RankStart;
			int rankEnd = reward.RankEnd;
			bool flag = true;
			if (rankType == RankType.WorldBoss)
			{
				flag = false;
			}
			if (flag && rankStart > 0 && rankStart < 4 && rankStart == rankEnd)
			{
				this.rankImage.enabled = true;
				this.rankImage.SetImage(GameApp.Table.GetAtlasPath(128), "rank_" + rankStart.ToString());
			}
			else
			{
				this.rankImage.enabled = false;
			}
			this.rankText.text = ((rankStart == rankEnd) ? rankStart.ToString() : string.Format("{0}-{1}", rankStart, rankEnd));
			if (reward.Data != null)
			{
				for (int i = 0; i < reward.Data.Length; i++)
				{
					this.leftItems[i].gameObject.SetActiveSafe(true);
					this.leftItems[i].SetData(reward.Data[i]);
					this.leftItems[i].OnRefresh();
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.itemLayout);
		}

		private void onClickItem(UIItem item, PropData data, object arg)
		{
			DxxTools.UI.ShowItemInfo(new ItemInfoOpenData
			{
				m_propData = data,
				m_openDataType = ItemInfoOpenDataType.eBag,
				m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume)
			}, item.transform.position, 70f);
		}

		public CustomImage rankImage;

		public CustomText rankText;

		public List<UIItem> leftItems;

		public RectTransform itemLayout;
	}
}
