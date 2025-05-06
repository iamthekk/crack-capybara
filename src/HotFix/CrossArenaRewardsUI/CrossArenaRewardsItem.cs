using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.CrossArenaRewardsUI
{
	public class CrossArenaRewardsItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Rank1.SetActive(false);
			this.Rank2.SetActive(false);
			this.Rank3.SetActive(false);
			this.Text_Rank.text = "";
			this.Prefab_Item.SetActive(false);
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.mUIList.Count; i++)
			{
				this.mUIList[i].DeInit();
			}
			this.mUIList.Clear();
		}

		public void SetGameObject(GameObject obj)
		{
		}

		public void SetData(CrossArenaRankRewards data, CrossArenaRewardsKind kind)
		{
			this.mData = data;
			this.mRewardsKind = kind;
		}

		public void RefreshUI()
		{
			if (this.mData.RankStart <= 3)
			{
				this.Rank1.SetActive(this.mData.RankStart == 1);
				this.Rank2.SetActive(this.mData.RankStart == 2);
				this.Rank3.SetActive(this.mData.RankStart == 3);
				this.Text_Rank.text = "";
			}
			else
			{
				this.Rank1.SetActive(false);
				this.Rank2.SetActive(false);
				this.Rank3.SetActive(false);
				if (this.mData.RankStart == this.mData.RankEnd)
				{
					this.Text_Rank.text = this.mData.RankStart.ToString();
				}
				else
				{
					this.Text_Rank.text = string.Format("{0}-{1}", this.mData.RankStart, this.mData.RankEnd);
				}
			}
			this.RefreshRewardsList();
		}

		public void RefreshRewardsList()
		{
			List<PropData> list = new List<PropData>();
			CrossArenaRewardsKind crossArenaRewardsKind = this.mRewardsKind;
			if (crossArenaRewardsKind != CrossArenaRewardsKind.Daily)
			{
				if (crossArenaRewardsKind == CrossArenaRewardsKind.Season)
				{
					list = this.mData.SeasonRewards;
				}
			}
			else
			{
				list = this.mData.DailyRewards;
			}
			for (int i = 0; i < list.Count; i++)
			{
				UIItem uiitem;
				if (i < this.mUIList.Count)
				{
					uiitem = this.mUIList[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.Prefab_Item, this.RTF_Rewards);
					gameObject.SetActive(true);
					uiitem = gameObject.GetComponent<UIItem>();
					uiitem.Init();
					this.mUIList.Add(uiitem);
				}
				uiitem.SetActive(true);
				uiitem.SetData(list[i]);
				uiitem.OnRefresh();
			}
			for (int j = list.Count; j < this.mUIList.Count; j++)
			{
				this.mUIList[j].SetActive(false);
			}
		}

		public GameObject Rank1;

		public GameObject Rank2;

		public GameObject Rank3;

		public CustomText Text_Rank;

		public RectTransform RTF_Rewards;

		public GameObject Prefab_Item;

		private CrossArenaRankRewards mData;

		private CrossArenaRewardsKind mRewardsKind;

		private List<UIItem> mUIList = new List<UIItem>();
	}
}
