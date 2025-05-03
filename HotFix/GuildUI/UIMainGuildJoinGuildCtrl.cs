using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using Proto.Guild;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIMainGuildJoinGuildCtrl : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.loadingMask.SetActiveSafe(false);
			this.guildListModule = GuildSDKManager.Instance.GuildList;
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.inputSearch.onEndEdit.AddListener(new UnityAction<string>(this.OnSearchEditEnd));
			this.buttonSearch.onClick.AddListener(new UnityAction(this.OnClickSearch));
			this.buttonAutoJoin.onClick.AddListener(new UnityAction(this.AutoJoin));
			this.buttonChange.onClick.AddListener(new UnityAction(this.OnClickChangeList));
			this.buttonCreate.onClick.AddListener(new UnityAction(this.OnClickCreateGuild));
			this.buttonJoinEnabled.OnClickButton = new Action<CustomChooseButton>(this.OnClickJoinState);
			this.CreateGuildCtrl.Init();
		}

		protected override void GuildUI_OnShow()
		{
			this.textNoSearchGuild.SetActive(false);
			base.gameObject.SetActiveSafe(true);
			if (!this.isReqGuilds)
			{
				this.isReqGuilds = true;
				this.ClearRequestHistory();
				string isSelectJoinGuild = PlayerPrefsKeys.GetIsSelectJoinGuild();
				if (string.IsNullOrEmpty(isSelectJoinGuild))
				{
					this.isShowJoinEnabled = true;
					PlayerPrefsKeys.SetIsSelectJoinGuild("1");
				}
				else
				{
					this.isShowJoinEnabled = isSelectJoinGuild == "1";
				}
				this.buttonJoinEnabled.SetSelect(this.isShowJoinEnabled);
				this.ReqGuildList(0, "", true);
			}
		}

		protected override void GuildUI_OnClose()
		{
			this.CreateGuildCtrl.Close();
			this.isReqGuilds = false;
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.inputSearch != null)
			{
				this.inputSearch.onEndEdit.RemoveListener(new UnityAction<string>(this.OnSearchEditEnd));
			}
			if (this.buttonSearch != null)
			{
				this.buttonSearch.onClick.RemoveListener(new UnityAction(this.OnClickSearch));
			}
			if (this.buttonAutoJoin != null)
			{
				this.buttonAutoJoin.onClick.RemoveListener(new UnityAction(this.AutoJoin));
			}
			if (this.buttonChange != null)
			{
				this.buttonChange.onClick.RemoveListener(new UnityAction(this.OnClickChangeList));
			}
			this.buttonJoinEnabled.OnClickButton = null;
			this.DeInitListItem();
			this.CreateGuildCtrl.DeInit();
		}

		private void OnSearchEditEnd(string text)
		{
			this.userSearch = text;
		}

		private void RefreshGuilds()
		{
			this.m_seqPool.Clear(false);
			List<GuildShareData> guildList = this.guildListModule.GetGuildList();
			this.mDataList.Clear();
			this.mDataList.AddRange(guildList);
			this.Scroll.SetListItemCount(this.mDataList.Count + 1 + 1, true);
			this.Scroll.RefreshAllShowItems();
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.Scroll.ItemList, 0f, 0.05f, 0.2f, 9);
		}

		private void ReqGuildList(int searchtype, string searchvalue, bool isforcenet = false)
		{
			if (searchtype == 0 && this.searchType == 0)
			{
				this.ClearRequestHistory();
			}
			else
			{
				this.ClearRequestHistory();
			}
			this.ShowLoading(isforcenet);
			bool flag = this.isShowJoinEnabled;
			this.m_seqPool.Get();
			GuildNetUtil.Guild.DoRequest_GetGuildHallList(searchtype, searchvalue, flag, delegate(bool result, GuildSearchResponse response)
			{
				if (!this.IsActive())
				{
					return;
				}
				this.HideLoading(isforcenet);
				if (!result)
				{
					this.searchType = searchtype;
					this.searchValue = searchvalue;
					this.isForceNet = isforcenet;
					return;
				}
				this.RefreshGuilds();
				if (searchtype == 1 && this.mDataList.Count == 0)
				{
					this.textNoSearchGuild.SetActive(true);
					this.emptyGuildTextObj.SetActiveSafe(false);
					return;
				}
				if (this.mDataList.Count == 0)
				{
					this.emptyGuildTextObj.SetActiveSafe(true);
				}
				else
				{
					this.emptyGuildTextObj.SetActiveSafe(false);
				}
				this.textNoSearchGuild.SetActive(false);
			});
		}

		private void ShowLoading(bool isForceNet)
		{
			this.loadingMask.SetActiveSafe(isForceNet);
			this.loadingCtrl.gameObject.SetActiveSafe(true);
			this.objEmptyList.SetActiveSafe(false);
			this.Scroll.SetListItemCount(0, true);
		}

		private void HideLoading(bool isForceNet)
		{
			this.loadingMask.SetActiveSafe(false);
			this.loadingCtrl.gameObject.SetActiveSafe(false);
		}

		private void ClearRequestHistory()
		{
			base.SDK.GuildList.ReleaseListGorup(0);
		}

		private void AutoJoin()
		{
			if (!GuildSDKManager.Instance.CheckQuitGuildTime())
			{
				return;
			}
			GuildNetUtil.Guild.DoRequest_GuildAutoJoin(delegate(bool result, int joinResult, GuildAutoJoinResponse response)
			{
				if (result)
				{
					string infoByID = GuildProxy.Language.GetInfoByID("400119");
					switch (joinResult)
					{
					case 0:
					{
						string infoByID2 = GuildProxy.Language.GetInfoByID("400030");
						GuildProxy.UI.OpenUIPopCommonSimple(infoByID, infoByID2);
						return;
					}
					case 1:
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400108"));
						GuildProxy.UI.CloseMainGuildInfo();
						GuildSDKManager.Instance.OpenGuild();
						return;
					case 2:
					{
						string infoByID3 = GuildProxy.Language.GetInfoByID("400031");
						GuildProxy.UI.OpenUIPopCommonSimple(infoByID, infoByID3);
						return;
					}
					case 3:
						GuildProxy.UI.OpenUIPopCommonSimple(infoByID, GuildProxy.Language.GetInfoByID("400032"));
						break;
					default:
						return;
					}
				}
			});
		}

		public void Hide()
		{
			base.gameObject.SetActiveSafe(false);
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.mDataList.Count + 1 + 1)
			{
				return null;
			}
			if (index < 1 || index + 1 >= this.mDataList.Count + 1 + 1)
			{
				return listView.NewListViewItem("TopEmpty");
			}
			index--;
			GuildShareData guildShareData = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("UIMainGuildJoinGuildItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UIMainGuildJoinGuildItem uimainGuildJoinGuildItem = this.TryGetUI(instanceID);
			UIMainGuildJoinGuildItem component = loopListViewItem.GetComponent<UIMainGuildJoinGuildItem>();
			if (uimainGuildJoinGuildItem == null)
			{
				uimainGuildJoinGuildItem = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			uimainGuildJoinGuildItem.Refresh(guildShareData);
			uimainGuildJoinGuildItem.SetActive(true);
			return loopListViewItem;
		}

		private UIMainGuildJoinGuildItem TryGetUI(int key)
		{
			UIMainGuildJoinGuildItem uimainGuildJoinGuildItem;
			if (this.UICtrlDic.TryGetValue(key, out uimainGuildJoinGuildItem))
			{
				return uimainGuildJoinGuildItem;
			}
			return null;
		}

		private UIMainGuildJoinGuildItem TryAddUI(int key, LoopListViewItem2 loopitem, UIMainGuildJoinGuildItem ui)
		{
			ui.Init();
			UIMainGuildJoinGuildItem uimainGuildJoinGuildItem;
			if (this.UICtrlDic.TryGetValue(key, out uimainGuildJoinGuildItem))
			{
				if (uimainGuildJoinGuildItem == null)
				{
					uimainGuildJoinGuildItem = ui;
					this.UICtrlDic[key] = ui;
				}
				return ui;
			}
			this.UICtrlDic.Add(key, ui);
			return ui;
		}

		private void DeInitListItem()
		{
			foreach (KeyValuePair<int, UIMainGuildJoinGuildItem> keyValuePair in this.UICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.UICtrlDic.Clear();
		}

		private void OnClickCreateGuild()
		{
			this.CreateGuildCtrl.Show();
		}

		private void OnClickChoose()
		{
			if (string.IsNullOrEmpty(this.userSearch))
			{
				return;
			}
			this.userSearch = string.Empty;
			this.inputSearch.text = string.Empty;
			this.ReqGuildList(0, "", false);
		}

		private void OnClickSearch()
		{
			if (string.IsNullOrEmpty(this.userSearch))
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400048"));
				return;
			}
			if (GuildProxy.Language.CheckTextLength(this.userSearch) > GuildProxy.Table.NAME_LENGTH_MAX)
			{
				string infoByID = GuildProxy.Language.GetInfoByID("400119");
				string infoByID2 = GuildProxy.Language.GetInfoByID1("400064", GuildProxy.Table.NAME_LENGTH_MAX);
				GuildProxy.UI.OpenUIPopCommonSimple(infoByID, infoByID2);
				return;
			}
			this.ReqGuildList(1, this.userSearch, false);
		}

		private void OnClickChangeList()
		{
			this.inputSearch.text = string.Empty;
			this.ReqGuildList(0, "", false);
		}

		private void OnClickJoinState(CustomChooseButton btn)
		{
			this.isShowJoinEnabled = !this.isShowJoinEnabled;
			PlayerPrefsKeys.SetIsSelectJoinGuild(this.isShowJoinEnabled ? "1" : "0");
			this.buttonJoinEnabled.SetSelect(this.isShowJoinEnabled);
			if (string.IsNullOrEmpty(this.userSearch))
			{
				this.ReqGuildList(0, "", false);
				return;
			}
			this.ReqGuildList(1, this.userSearch, false);
		}

		[Header("简易创建工会")]
		public UIMainGuildCreateSimpleCtrl CreateGuildCtrl;

		[Header("搜索")]
		public InputField inputSearch;

		public CustomButton buttonSearch;

		public CustomChooseButton buttonJoinEnabled;

		[Header("滚动列表")]
		public LoopListView2 Scroll;

		private const int mScrollTopEmptyCount = 1;

		private const int mScrollBottomEmptyCount = 1;

		public GameObject objEmptyList;

		public UINetLoading loadingCtrl;

		private List<GuildShareData> mDataList = new List<GuildShareData>();

		public Dictionary<int, UIMainGuildJoinGuildItem> UICtrlDic = new Dictionary<int, UIMainGuildJoinGuildItem>();

		[Header("其他")]
		public CustomButton buttonAutoJoin;

		public CustomButton buttonChange;

		public CustomButton buttonCreate;

		public GameObject loadingMask;

		public GameObject textNoSearchGuild;

		public GameObject emptyGuildTextObj;

		private string userSearch;

		private GuildListModule guildListModule;

		private bool isShowJoinEnabled;

		private int searchType;

		private string searchValue = "";

		private bool isForceNet;

		private bool isReqGuilds;

		private SequencePool m_seqPool = new SequencePool();
	}
}
