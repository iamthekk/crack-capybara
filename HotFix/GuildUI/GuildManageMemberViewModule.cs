using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework;
using SuperScrollView;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildManageMemberViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.popCommon.OnClick = new Action<int>(this.OnPopClick);
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			base.SDK.Event.RegisterEvent(8, new GuildHandlerEvent(this.OnGuildMemberDataChange));
		}

		protected override void OnViewDelete()
		{
			base.SDK.Event.UnRegisterEvent(8, new GuildHandlerEvent(this.OnGuildMemberDataChange));
			this.DeInitAllScrollUI();
		}

		protected override void OnViewOpen(object data)
		{
			this.RefreshUI();
		}

		private void RefreshUI()
		{
			IList<GuildUserShareData> memberList = base.SDK.GuildInfo.GetMemberList();
			this.mDataList.Clear();
			this.mDataList.AddRange(memberList);
			this.Scroll.SetListItemCount(this.mDataList.Count + 2, false);
			this.Scroll.RefreshAllShowItems();
			this.m_seqPool.Get();
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.Scroll.ItemList, 0f, 0.05f, 0.2f, 9);
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.mDataList.Count + 2)
			{
				return null;
			}
			if (index < 1 || index + 1 >= this.mDataList.Count + 2)
			{
				return listView.NewListViewItem("EmptyItem");
			}
			index--;
			GuildUserShareData guildUserShareData = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("UIGuildManageMemberItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			GuildManageMemberItem guildManageMemberItem = this.TryGetUI(instanceID);
			GuildManageMemberItem component = loopListViewItem.GetComponent<GuildManageMemberItem>();
			if (guildManageMemberItem == null)
			{
				guildManageMemberItem = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			guildManageMemberItem.RefreshMember(guildUserShareData);
			guildManageMemberItem.SetActive(true);
			return loopListViewItem;
		}

		private GuildManageMemberItem TryGetUI(int key)
		{
			GuildManageMemberItem guildManageMemberItem;
			if (this.mUICtrlDic.TryGetValue(key, out guildManageMemberItem))
			{
				return guildManageMemberItem;
			}
			return null;
		}

		private GuildManageMemberItem TryAddUI(int key, LoopListViewItem2 loopitem, GuildManageMemberItem ui)
		{
			ui.Init();
			GuildManageMemberItem guildManageMemberItem;
			if (this.mUICtrlDic.TryGetValue(key, out guildManageMemberItem))
			{
				if (guildManageMemberItem == null)
				{
					guildManageMemberItem = ui;
					this.mUICtrlDic[key] = ui;
				}
				return ui;
			}
			this.mUICtrlDic.Add(key, ui);
			return ui;
		}

		private void DeInitAllScrollUI()
		{
			foreach (KeyValuePair<int, GuildManageMemberItem> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUICtrlDic.Clear();
		}

		private void OnGuildMemberDataChange(int type, GuildBaseEvent eventArgs)
		{
			this.RefreshUI();
		}

		private void CloseSelfView()
		{
			GameApp.View.CloseView(ViewName.GuildManageMemberViewModule, null);
		}

		private void OnPopClick(int kind)
		{
			this.CloseSelfView();
		}

		public UIGuildPopCommon popCommon;

		[Header("滚动区域")]
		public LoopListView2 Scroll;

		private const int mSTopCount = 1;

		private const int mSBottomCount = 1;

		private const int mSExCount = 2;

		private List<GuildUserShareData> mDataList = new List<GuildUserShareData>();

		private Dictionary<int, GuildManageMemberItem> mUICtrlDic = new Dictionary<int, GuildManageMemberItem>();

		private SequencePool m_seqPool = new SequencePool();
	}
}
