using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildBossRewardMyInfo : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Prefab_Item.SetActive(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
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

		public void SetData(GuildBOSS_rankRewards rankReward, long bossRefreshTime)
		{
			if (rankReward == null)
			{
				return;
			}
			this.m_rewardData = rankReward;
			this.Text_Tips.text = Singleton<LanguageManager>.Instance.GetInfoByID("15032");
			this.RefreshRewadItems();
			long num = bossRefreshTime - GuildProxy.Net.ServerTime();
			num = ((num > 0L) ? num : 0L);
			string time = Singleton<LanguageManager>.Instance.GetTime(num);
			this.Text_Time.text = Singleton<LanguageManager>.Instance.GetInfoByID("400185", new object[] { time });
		}

		private void RefreshRewadItems()
		{
			List<PropData> list = DxxTools.Reward.ParseReward(this.m_rewardData.Reward).ToPropList();
			for (int i = 0; i < list.Count; i++)
			{
				PropData propData = list[i];
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
			for (int j = list.Count; j < this.ItemCtrlList.Count; j++)
			{
				this.ItemCtrlList[j].SetActive(false);
			}
		}

		public CustomText Text_Tips;

		public CustomText Text_Time;

		public RectTransform RTF_Content;

		public GameObject Prefab_Item;

		public List<UIItem> ItemCtrlList = new List<UIItem>();

		private GuildBOSS_rankRewards m_rewardData;

		private double mLeftSec;

		private int mShowLeft;
	}
}
