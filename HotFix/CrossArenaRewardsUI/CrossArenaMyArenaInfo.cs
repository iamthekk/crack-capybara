using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.CrossArenaRewardsUI
{
	public class CrossArenaMyArenaInfo : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Prefab_Item.SetActive(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.mLeftSec -= (double)unscaledDeltaTime;
			this.RefreshTime();
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.ItemCtrlList.Count; i++)
			{
				this.ItemCtrlList[i].DeInit();
			}
			this.ItemCtrlList.Clear();
			this.RTF_Content.DestroyChildren();
		}

		public void SetData(CrossArenaRankRewards data)
		{
			this.mData = data;
		}

		public void SwitchShowKind(CrossArenaRewardsKind kind)
		{
			this.mShowKind = kind;
			this.mProgress = new CrossArenaProgress();
			this.mProgress.BuildProgress();
			this.Text_Tips.text = Singleton<LanguageManager>.Instance.GetInfoByID("15032");
			CrossArenaRewardsKind crossArenaRewardsKind = this.mShowKind;
			if (crossArenaRewardsKind != CrossArenaRewardsKind.Daily)
			{
				if (crossArenaRewardsKind == CrossArenaRewardsKind.Season)
				{
					this.mLeftSec = (double)this.mProgress.CalcTimeSecToSeasonEnd();
					this.PropDataList = this.mData.SeasonRewards;
				}
			}
			else
			{
				this.mLeftSec = (double)this.mProgress.CalcTimeSecToDailyEnd();
				this.PropDataList = this.mData.DailyRewards;
			}
			this.RefreshRewadItems();
			this.mShowLeft = -1;
			this.RefreshTime();
		}

		private void RefreshRewadItems()
		{
			if (this.PropDataList == null)
			{
				this.PropDataList = new List<PropData>();
			}
			for (int i = 0; i < this.PropDataList.Count; i++)
			{
				PropData propData = this.PropDataList[i];
				UIItem uiitem;
				if (i < this.ItemCtrlList.Count)
				{
					uiitem = this.ItemCtrlList[i];
				}
				else
				{
					uiitem = null;
				}
				if (uiitem == null)
				{
					uiitem = Object.Instantiate<GameObject>(this.Prefab_Item, this.RTF_Content).GetComponent<UIItem>();
					uiitem.Init();
					if (i < this.ItemCtrlList.Count)
					{
						this.ItemCtrlList[i] = uiitem;
					}
					else
					{
						this.ItemCtrlList.Add(uiitem);
					}
				}
				uiitem.SetData(propData);
				uiitem.OnRefresh();
				uiitem.SetActive(true);
			}
			for (int j = this.PropDataList.Count; j < this.ItemCtrlList.Count; j++)
			{
				this.ItemCtrlList[j].SetActive(false);
			}
		}

		private void RefreshTime()
		{
			if ((int)this.mLeftSec != this.mShowLeft)
			{
				this.mShowLeft = (int)this.mLeftSec;
				if (this.mShowLeft < 0)
				{
					this.mShowLeft = 0;
				}
				this.Text_Time.text = Singleton<LanguageManager>.Instance.GetInfoByID("15017", new object[] { DxxTools.FormatFullTimeWithDay((long)this.mShowLeft) });
			}
		}

		public CustomText Text_Tips;

		public CustomText Text_Time;

		public RectTransform RTF_Content;

		public GameObject Prefab_Item;

		public List<PropData> PropDataList = new List<PropData>();

		public List<UIItem> ItemCtrlList = new List<UIItem>();

		private CrossArenaRankRewards mData;

		private CrossArenaRewardsKind mShowKind;

		private CrossArenaProgress mProgress;

		private double mLeftSec;

		private int mShowLeft;
	}
}
