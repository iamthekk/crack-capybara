using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Guild;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildContributeViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			base.OnViewCreate();
			this.contributeBtn.m_onClick = new Action(this.OnClickContributeBtn);
			this.closeButton.m_onClick = new Action(this.CloseSelf);
		}

		protected override void OnViewOpen(object data)
		{
			base.OnViewOpen(data);
			this.Refresh();
			this.isClickBtn = false;
			base.SDK.Event.RegisterEvent(202, new GuildHandlerEvent(this.OnRefreshBack));
		}

		private void Refresh()
		{
			GuildInfoDataModule guildInfo = GuildSDKManager.Instance.GuildInfo;
			int dayAllContributeTimes = guildInfo.DayAllContributeTimes;
			int dayContributeTimes = guildInfo.DayContributeTimes;
			List<Guild_guildcontribute> guildContributeConfigs = guildInfo.GuildContributeConfigs;
			if (dayContributeTimes < guildInfo.GuildContributeConfigs.Count)
			{
				this._buyFinishText.gameObject.SetActiveSafe(false);
				this.contributeBtn.gameObject.SetActiveSafe(true);
				Guild_guildcontribute guild_guildcontribute = guildContributeConfigs[dayContributeTimes];
				if (guild_guildcontribute.CostItem.Length == 0)
				{
					this.freeObj.SetActiveSafe(true);
					this.priceObj.SetActiveSafe(false);
				}
				else
				{
					string[] array = guild_guildcontribute.CostItem[0].Split(',', StringSplitOptions.None);
					int num = ((array.Length != 0) ? int.Parse(array[0]) : 0);
					int num2 = ((array.Length > 1) ? int.Parse(array[1]) : 0);
					Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(num);
					if (num2 == 0)
					{
						this.freeObj.SetActiveSafe(true);
						this.priceObj.SetActiveSafe(false);
					}
					else
					{
						this.priceObj.SetActiveSafe(true);
						this.freeObj.SetActiveSafe(false);
						this.itemImage.SetImage(item_Item.atlasID, item_Item.icon);
						this.priceText.text = num2.ToString();
					}
				}
				this.leftCountText.text = Singleton<LanguageManager>.Instance.GetInfoByID("guild_contribute_des", new object[] { guildContributeConfigs.Count - dayContributeTimes });
				string[] guildItems = guild_guildcontribute.guildItems;
				this._itemList = new List<PropData>();
				for (int i = 0; i < guildItems.Length; i++)
				{
					string[] array2 = guildItems[i].Split(',', StringSplitOptions.None);
					uint num3 = uint.Parse(array2[0]);
					ulong num4 = ulong.Parse(array2[1]);
					PropData propData = new PropData
					{
						id = num3,
						count = num4
					};
					this._itemList.Add(propData);
				}
				this.RefreshList(this._itemList);
			}
			else
			{
				this._buyFinishText.gameObject.SetActiveSafe(true);
				this.contributeBtn.gameObject.SetActiveSafe(false);
				this.leftCountText.text = Singleton<LanguageManager>.Instance.GetInfoByID("guild_contribute_des", new object[] { 0 });
				this.RefreshList(null);
				this.CloseSelf();
			}
			Guild_guildLevel guildLevelTable = GuildProxy.Table.GetGuildLevelTable(guildInfo.GuildDetailData.ShareData.GuildLevel);
			if (guildLevelTable != null)
			{
				this.Text_GuildLeftCount.text = string.Format(GuildProxy.Language.GetInfoByID1("guild_contribute_remainNum", guildLevelTable.MaxContribute - dayAllContributeTimes), Array.Empty<object>());
			}
		}

		protected override void OnViewDelete()
		{
			base.OnViewDelete();
			this.contributeBtn.m_onClick = null;
			this.closeButton.m_onClick = null;
		}

		protected override void OnViewClose()
		{
			base.OnViewClose();
			base.SDK.Event.UnRegisterEvent(202, new GuildHandlerEvent(this.OnRefreshBack));
		}

		private void RefreshList(List<PropData> dataList)
		{
			int num = 0;
			if (dataList != null)
			{
				num = dataList.Count;
				for (int i = 0; i < dataList.Count; i++)
				{
					UIItem item = this.GetItem(i);
					item.SetData(dataList[i]);
					item.OnRefresh();
				}
			}
			for (int j = num; j < this.itemList.Count; j++)
			{
				this.itemList[j].gameObject.SetActive(false);
			}
		}

		private UIItem GetItem(int index)
		{
			UIItem uiitem;
			if (this.itemList.Count > index)
			{
				uiitem = this.itemList[index];
				uiitem.gameObject.SetActive(true);
			}
			else
			{
				uiitem = Object.Instantiate<UIItem>(this.tempItem, this.itemParent);
				uiitem.Init();
				uiitem.gameObject.SetActive(true);
				this.itemList.Add(uiitem);
			}
			return uiitem;
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(ViewName.GuildContributeViewModule, null);
		}

		private void OnClickContributeBtn()
		{
			if (this.isClickBtn)
			{
				return;
			}
			if (GuildSDKManager.Instance.GuildInfo.DayContributeTimes >= GuildSDKManager.Instance.GuildInfo.GuildContributeConfigs.Count)
			{
				return;
			}
			int num = 0;
			Guild_guildLevel guildLevelTable = GuildProxy.Table.GetGuildLevelTable(GuildSDKManager.Instance.GuildInfo.GuildDetailData.ShareData.GuildLevel);
			if (guildLevelTable != null)
			{
				num = guildLevelTable.MaxContribute - GuildSDKManager.Instance.GuildInfo.DayAllContributeTimes;
			}
			if (num <= 0)
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("guild_contribute_finish"));
				return;
			}
			this.isClickBtn = true;
			GuildNetUtil.Guild.DoRequest_GuildContribute(delegate(bool result, GuildContributeResponse resp)
			{
				if (result)
				{
					GuildNetUtil.Guild.DoRequest_GetGuildDetailInfo(GuildSDKManager.Instance.GuildInfo.GuildID, delegate(bool result1, GuildGetDetailResponse resp1)
					{
						if (result1)
						{
							this.Refresh();
							DxxTools.UI.OpenRewardCommon(this._itemList.ToItemDatas(), null, true);
							GameApp.Event.DispatchNow(null, 122, null);
						}
						else
						{
							GuildNetUtil.Guild.DoRequest_GuildLoginRequest(null);
						}
						this.isClickBtn = false;
						GameApp.SDK.Analyze.Track_GuildActivity("donation", TGA_GuildActivityData.Create(), null);
					}, false);
					return;
				}
				this.isClickBtn = false;
			});
		}

		private void OnRefreshBack(int type, GuildBaseEvent eventArgs)
		{
			this.Refresh();
		}

		public Transform itemParent;

		public UIItem tempItem;

		private List<UIItem> itemList = new List<UIItem>();

		public CustomButton contributeBtn;

		public CustomLanguageText leftCountText;

		public CustomLanguageText Text_GuildLeftCount;

		public GameObject freeObj;

		public GameObject priceObj;

		public CustomText priceText;

		public CustomButton closeButton;

		public CustomImage itemImage;

		private List<PropData> _itemList = new List<PropData>();

		public CustomLanguageText _buyFinishText;

		private bool isClickBtn;
	}
}
