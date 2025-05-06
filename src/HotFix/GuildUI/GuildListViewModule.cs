using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using Framework.Logic.UI.Guild;
using Proto.Guild;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class GuildListViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			base.OnViewCreate();
			this.Loading_Ctrl.OnInit();
			this.Loading_Ctrl.SetText(GuildProxy.Language.GetInfoByID("400127"), GuildProxy.Language.GetInfoByID("400128"));
			this.m_closeBt.m_onClick = new Action(this.ClickCloseThis);
			this.Scroll.mOnBeginDragAction = new Action(this.OnDragStart);
			this.Scroll.mOnDragingAction = new Action(this.OnDraging);
			this.Scroll.mOnEndDragAction = new Action(this.OnDragEnd);
			this.Scroll.InitListView(this.dataList.Count, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.Text_EmptyList.text = "";
			this.Button_CloseSearch.onClick.AddListener(new UnityAction(this.OnClearSearch));
			this.Button_Search.onClick.AddListener(new UnityAction(this.OnSearchGuild));
			this.serachMask.SetActive(false);
		}

		protected override void OnViewOpen(object data)
		{
			base.OnViewOpen(data);
			this.ClearRequestHistory();
			this.Input_Search.onEndEdit.AddListener(new UnityAction<string>(this.OnSearchEditEnd));
			this.Scroll.ScrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScrollValueChange));
			this.ReqGuildList("");
		}

		protected override void OnViewClose()
		{
			base.OnViewClose();
			this.Input_Search.onEndEdit.RemoveListener(new UnityAction<string>(this.OnSearchEditEnd));
			this.Scroll.ScrollRect.onValueChanged.RemoveListener(new UnityAction<Vector2>(this.OnScrollValueChange));
			this.Loading_Ctrl.OnClose();
		}

		protected override void OnViewDelete()
		{
			base.OnViewDelete();
			if (this.Button_CloseSearch != null)
			{
				this.Button_CloseSearch.onClick.RemoveListener(new UnityAction(this.OnClearSearch));
			}
			if (this.Button_Search != null)
			{
				this.Button_Search.onClick.RemoveListener(new UnityAction(this.OnSearchGuild));
			}
		}

		private void OnScrollValueChange(Vector2 arg0)
		{
			if (this.mLoadingTipStatus == GuildListViewModule.LoadingTipStatus.WaitLoad && this.Scroll.ScrollRect.verticalNormalizedPosition > -0.0001f)
			{
				this.Scroll.ScrollRect.verticalNormalizedPosition = 0f;
				this.mLoadingTipStatus = GuildListViewModule.LoadingTipStatus.RequireData;
				this.ReqGuildList(this.searchValue);
			}
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index > this.dataList.Count + 1)
			{
				return null;
			}
			if (index == 0)
			{
				return listView.NewListViewItem("EmptyItem");
			}
			int num = index - 1;
			LoopListViewItem2 loopListViewItem;
			if (num == this.dataList.Count)
			{
				loopListViewItem = listView.NewListViewItem("LoadingItem");
				this.UpdateLoadingTip(loopListViewItem);
				return loopListViewItem;
			}
			GuildShareData guildShareData = this.dataList[num];
			loopListViewItem = listView.NewListViewItem("UIGuildListItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UIGuildListItem uiguildListItem = this.TryGetUI(instanceID);
			if (uiguildListItem == null)
			{
				uiguildListItem = this.TryAddUI(instanceID, loopListViewItem, loopListViewItem.GetComponent<UIGuildListItem>());
			}
			uiguildListItem.SetData(guildShareData);
			return loopListViewItem;
		}

		private UIGuildListItem TryGetUI(int key)
		{
			UIGuildListItem uiguildListItem;
			if (this.UICtrlDic.TryGetValue(key, out uiguildListItem))
			{
				return uiguildListItem;
			}
			return null;
		}

		private UIGuildListItem TryAddUI(int key, LoopListViewItem2 loopitem, UIGuildListItem ui)
		{
			ui.Init();
			UIGuildListItem uiguildListItem;
			if (this.UICtrlDic.TryGetValue(key, out uiguildListItem))
			{
				if (uiguildListItem == null)
				{
					uiguildListItem = ui;
					this.UICtrlDic[key] = ui;
				}
				return ui;
			}
			this.UICtrlDic.Add(key, ui);
			return ui;
		}

		private void RefreshGuilds()
		{
			this.Scroll.SetListItemCount(this.dataList.Count + 2, true);
			this.Scroll.RefreshAllShownItem();
			if (!string.IsNullOrEmpty(this.searchValue) && this.dataList.Count == 0)
			{
				this.Text_EmptyList.text = GuildProxy.Language.GetInfoByID("400049");
				return;
			}
			this.Text_EmptyList.text = "";
		}

		private void OnSearchEditEnd(string text)
		{
			this.userSearch = text;
		}

		private void RetryReqGuildList()
		{
			this.ReqGuildList(this.searchValue);
		}

		private void ReqGuildList(string searchValue)
		{
			int num = (string.IsNullOrEmpty(searchValue) ? 2 : 1);
			if (num != 2 || this.searchType != num)
			{
				this.ClearRequestHistory();
			}
			bool flag = this.hasallserverdata;
			this.searchValue = searchValue;
			this.searchType = num;
			this.ShowLoading();
			GuildNetUtil.Guild.DoRequest_GetGuildGroupList(this.listpage, num, searchValue, delegate(bool result, GuildSearchResponse response)
			{
				if (!base.CheckIsViewOpen())
				{
					return;
				}
				if (result)
				{
					this.HideLoading();
					int num2 = 0;
					if (response.GuildInfoDtos.Count > 0)
					{
						this.listpage++;
						List<GuildShareData> list = response.GuildInfoDtos.ToGuidList();
						for (int i = 0; i < list.Count; i++)
						{
							GuildShareData guildShareData = list[i];
							if (!this.guilddic.ContainsKey(guildShareData.GuildID))
							{
								this.guilddic[guildShareData.GuildID] = guildShareData;
								this.dataList.Add(guildShareData);
								num2++;
							}
						}
					}
					this.hasallserverdata = num2 == 0;
					this.RefreshListAfterGetDataList();
					this.RefreshGuilds();
					return;
				}
				this.Loading_Ctrl.ShowRetry(new Action(this.RetryReqGuildList));
			});
		}

		private void OnClearSearch()
		{
			if (string.IsNullOrEmpty(this.userSearch) && this.searchType == 0)
			{
				return;
			}
			this.userSearch = string.Empty;
			this.Input_Search.text = string.Empty;
			this.ReqGuildList("");
		}

		private void OnSearchGuild()
		{
			if (string.IsNullOrEmpty(this.userSearch) && this.searchType == 0)
			{
				GuildProxy.UI.OpenUIPopCommonSimple(GuildProxy.Language.GetInfoByID("400119"), GuildProxy.Language.GetInfoByID("400126"));
				return;
			}
			this.ReqGuildList(this.userSearch);
		}

		private void ClearRequestHistory()
		{
			this.dataList.Clear();
			this.guilddic.Clear();
			this.listpage = 1;
			this.mLoadingTipStatus = GuildListViewModule.LoadingTipStatus.None;
		}

		private void ShowLoading()
		{
			this.serachMask.SetActiveSafe(true);
			if (this.dataList.Count > 0)
			{
				this.Loading_Ctrl.Hide();
				return;
			}
			this.Loading_Ctrl.OnShow();
		}

		private void HideLoading()
		{
			this.serachMask.SetActiveSafe(false);
			this.Loading_Ctrl.Hide();
		}

		private void OnDragStart()
		{
		}

		private void OnDraging()
		{
			this.RefreshScrollOnDraging();
		}

		private void OnDragEnd()
		{
			this.RefreshScrollAfterDragEnd();
		}

		private void RefreshScrollOnDraging()
		{
			if (this.Scroll.ShownItemCount == 0)
			{
				return;
			}
			if (this.mLoadingTipStatus != GuildListViewModule.LoadingTipStatus.None && this.mLoadingTipStatus != GuildListViewModule.LoadingTipStatus.WaitRelease)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.Scroll.GetShownItemByItemIndex(this.dataList.Count + 1);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex2 = this.Scroll.GetShownItemByItemIndex(this.dataList.Count);
			if (shownItemByItemIndex2 == null)
			{
				return;
			}
			if (this.Scroll.GetItemCornerPosInViewPort(shownItemByItemIndex2, 0).y + this.Scroll.ViewPortSize >= 5f)
			{
				if (this.mLoadingTipStatus != GuildListViewModule.LoadingTipStatus.None)
				{
					return;
				}
				if (string.IsNullOrEmpty(this.searchValue))
				{
					this.mLoadingTipStatus = GuildListViewModule.LoadingTipStatus.WaitRelease;
				}
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
			else
			{
				if (this.mLoadingTipStatus != GuildListViewModule.LoadingTipStatus.WaitRelease)
				{
					return;
				}
				this.mLoadingTipStatus = GuildListViewModule.LoadingTipStatus.None;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
		}

		private void RefreshScrollAfterDragEnd()
		{
			if (this.Scroll.ShownItemCount == 0)
			{
				return;
			}
			if (this.mLoadingTipStatus != GuildListViewModule.LoadingTipStatus.None && this.mLoadingTipStatus != GuildListViewModule.LoadingTipStatus.WaitRelease)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.Scroll.GetShownItemByItemIndex(this.dataList.Count + 1);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			this.Scroll.OnItemSizeChanged(shownItemByItemIndex.ItemIndex);
			if (this.mLoadingTipStatus != GuildListViewModule.LoadingTipStatus.WaitRelease)
			{
				return;
			}
			if (this.Scroll.GetItemCornerPosInViewPort(shownItemByItemIndex, 0).y + this.Scroll.ViewPortSize < 0f)
			{
				if (this.hasallserverdata)
				{
					this.mLoadingTipStatus = GuildListViewModule.LoadingTipStatus.Loaded;
				}
				else
				{
					this.mLoadingTipStatus = GuildListViewModule.LoadingTipStatus.None;
				}
				this.Scroll.SetListItemCount(this.dataList.Count + 2, false);
				this.Scroll.RefreshAllShownItem();
				return;
			}
			if (this.hasallserverdata)
			{
				this.mLoadingTipStatus = GuildListViewModule.LoadingTipStatus.Loaded;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
			this.mLoadingTipStatus = GuildListViewModule.LoadingTipStatus.WaitLoad;
			this.UpdateLoadingTip(shownItemByItemIndex);
		}

		private void UpdateLoadingTip(LoopListViewItem2 item)
		{
			if (item == null)
			{
				return;
			}
			UIGuildListBottomLoadingItem component = item.GetComponent<UIGuildListBottomLoadingItem>();
			if (component == null)
			{
				return;
			}
			switch (this.mLoadingTipStatus)
			{
			case GuildListViewModule.LoadingTipStatus.None:
				component.SetActive(false);
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, 0f);
				return;
			case GuildListViewModule.LoadingTipStatus.WaitRelease:
				component.SetActive(true);
				component.SetScroll(this.Scroll);
				component.SetAsLoading();
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, this.mLoadingTipItemHeight);
				return;
			case GuildListViewModule.LoadingTipStatus.WaitLoad:
			case GuildListViewModule.LoadingTipStatus.RequireData:
				component.SetActive(true);
				component.SetScroll(this.Scroll);
				component.SetAsLoading();
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, this.mLoadingTipItemHeight);
				return;
			case GuildListViewModule.LoadingTipStatus.Loaded:
				component.SetActive(true);
				component.SetScroll(this.Scroll);
				component.SetAsNoMoreData();
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, this.mLoadingTipItemHeight);
				return;
			default:
				return;
			}
		}

		private void RefreshListAfterGetDataList()
		{
			if (this.Scroll.ShownItemCount == 0)
			{
				return;
			}
			if (this.mLoadingTipStatus == GuildListViewModule.LoadingTipStatus.RequireData)
			{
				if (this.hasallserverdata)
				{
					this.mLoadingTipStatus = GuildListViewModule.LoadingTipStatus.Loaded;
				}
				else
				{
					this.mLoadingTipStatus = GuildListViewModule.LoadingTipStatus.None;
				}
				this.Scroll.SetListItemCount(this.dataList.Count + 2, false);
				this.Scroll.RefreshAllShownItem();
			}
		}

		private void ClickCloseThis()
		{
			GuildProxy.UI.CloseUIGuildList();
		}

		public CustomButton m_closeBt;

		public InputField Input_Search;

		public CustomButton Button_CloseSearch;

		public CustomButton Button_Search;

		public CustomButton Button_AutoJoin;

		public CustomButton Button_CreateGuild;

		public UILoadingCtrl Loading_Ctrl;

		public GameObject serachMask;

		public LoopListView2 Scroll;

		public CustomText Text_EmptyList;

		private GuildListViewModule.LoadingTipStatus mLoadingTipStatus;

		private float mLoadingTipItemHeight = 100f;

		private string userSearch;

		private List<GuildShareData> dataList = new List<GuildShareData>();

		private Dictionary<string, GuildShareData> guilddic = new Dictionary<string, GuildShareData>();

		private int listpage = 1;

		public Dictionary<int, UIGuildListItem> UICtrlDic = new Dictionary<int, UIGuildListItem>();

		private bool hasallserverdata;

		private string searchValue = "";

		private int searchType;

		public enum LoadingTipStatus
		{
			None,
			WaitRelease,
			WaitLoad,
			RequireData,
			Loaded
		}
	}
}
