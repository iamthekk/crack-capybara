using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildRaceRewardsShowItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Prefab_Item.SetActive(false);
		}

		protected override void GuildUI_OnUnInit()
		{
			for (int i = 0; i < this.mItemList.Count; i++)
			{
				this.mItemList[i].DeInit();
			}
			this.RTF_Rewards.DestroyChildren();
			this.mItemList.Clear();
		}

		public void SetData(GuildRaceRewardsShowItemData data)
		{
			this.mData = data;
		}

		public void RefreshUI()
		{
			if (this.mData == null)
			{
				return;
			}
			GuildRace_level raceLevelTab = GuildProxy.Table.GetRaceLevelTab(this.mData.RaceDan);
			GuildProxy.Resources.SetDxxImage(this.Image_Dan, raceLevelTab.atlasID, raceLevelTab.icon);
			List<GuildItemData> list = new List<GuildItemData>();
			list.AddRange(this.mData.Rewards);
			list.AddRange(this.mData.RewardsWin);
			for (int i = 0; i < list.Count; i++)
			{
				GuildRaceRewardsShowItem_RewardItemUI guildRaceRewardsShowItem_RewardItemUI = null;
				if (i < this.mItemList.Count)
				{
					guildRaceRewardsShowItem_RewardItemUI = this.mItemList[i];
				}
				if (guildRaceRewardsShowItem_RewardItemUI == null)
				{
					guildRaceRewardsShowItem_RewardItemUI = Object.Instantiate<GuildRaceRewardsShowItem_RewardItemUI>(this.Prefab_Item, this.RTF_Rewards);
					guildRaceRewardsShowItem_RewardItemUI.SetActive(true);
					guildRaceRewardsShowItem_RewardItemUI.Init();
					if (i < this.mItemList.Count)
					{
						this.mItemList[i] = guildRaceRewardsShowItem_RewardItemUI;
					}
					else
					{
						this.mItemList.Add(guildRaceRewardsShowItem_RewardItemUI);
					}
				}
				guildRaceRewardsShowItem_RewardItemUI.SetItem(list[i]);
				guildRaceRewardsShowItem_RewardItemUI.RefreshUI();
				guildRaceRewardsShowItem_RewardItemUI.SetShowWinObj(i >= this.mData.Rewards.Count);
			}
			for (int j = list.Count; j < this.mItemList.Count; j++)
			{
				this.mItemList[j].SetActive(false);
			}
			this.Text_Dan.text = GuildProxy.Language.GetInfoByID_LogError(400460 + this.mData.RaceDan);
		}

		public RectTransform RTF_Rewards;

		public GuildRaceRewardsShowItem_RewardItemUI Prefab_Item;

		public CustomImage Image_Dan;

		public CustomText Text_Dan;

		private List<GuildRaceRewardsShowItem_RewardItemUI> mItemList = new List<GuildRaceRewardsShowItem_RewardItemUI>();

		private GuildRaceRewardsShowItemData mData;
	}
}
