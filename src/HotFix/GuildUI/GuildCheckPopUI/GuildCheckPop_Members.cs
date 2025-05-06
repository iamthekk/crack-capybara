using System;
using System.Collections.Generic;
using Dxx.Guild;
using SuperScrollView;
using UnityEngine;

namespace HotFix.GuildUI.GuildCheckPopUI
{
	public class GuildCheckPop_Members : GuildCheckPop_Base
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.RTFRoot = base.gameObject.transform as RectTransform;
			this.scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		protected override void GuildUI_OnUnInit()
		{
			this.m_seqPool.Clear(false);
			base.GuildUI_OnUnInit();
			foreach (KeyValuePair<int, UIGuildCheckMemberItem> keyValuePair in this.uiCtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.uiCtrlDic.Clear();
		}

		public override void RefreshUI(GuildShareData sharedata)
		{
			this.m_seqPool.Clear(false);
			this.mGuildData = sharedata;
			this.mGuildDetailData = null;
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			int num = 2;
			if (index < 0 || index > this.memberDataList.Count + num)
			{
				return null;
			}
			if (index < 1 || index + 1 >= this.memberDataList.Count + num)
			{
				return listView.NewListViewItem("EmptyItem");
			}
			index--;
			GuildUserShareData guildUserShareData = this.memberDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("UIGuildCheckMemberItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			this.TryGetOrAddUI(instanceID, loopListViewItem).Refresh(guildUserShareData);
			return loopListViewItem;
		}

		private UIGuildCheckMemberItem TryGetOrAddUI(int key, LoopListViewItem2 loopitem)
		{
			UIGuildCheckMemberItem component;
			if (this.uiCtrlDic.TryGetValue(key, out component))
			{
				return component;
			}
			component = loopitem.gameObject.GetComponent<UIGuildCheckMemberItem>();
			component.Init();
			this.uiCtrlDic.Add(key, component);
			return component;
		}

		public void SetGuildDetailInfo(GuildShareDetailData guildDetailData)
		{
			this.mGuildDetailData = guildDetailData;
			this.memberDataList.Clear();
			this.memberDataList.AddRange(this.mGuildDetailData.Members);
			this.memberDataList.CustomSort();
			this.scroll.SetListItemCount(this.memberDataList.Count + 2, true);
			this.scroll.RefreshAllShownItem();
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.scroll.ItemList, 0f, 0.05f, 0.2f, 9);
		}

		public void SetShowJoinState(bool show)
		{
			if (show)
			{
				this.RTFRoot.offsetMin = new Vector2(this.RTFRoot.offsetMin.x, 175f);
				return;
			}
			this.RTFRoot.offsetMin = new Vector2(this.RTFRoot.offsetMin.x, 0f);
		}

		[SerializeField]
		private LoopListView2 scroll;

		private const int mScrollTopEmptyCount = 1;

		private const int mScrollBottomEmptyCount = 1;

		private const int ScrollExtureCount = 2;

		private List<GuildUserShareData> memberDataList = new List<GuildUserShareData>();

		private Dictionary<int, UIGuildCheckMemberItem> uiCtrlDic = new Dictionary<int, UIGuildCheckMemberItem>();

		private RectTransform RTFRoot;

		private GuildShareData mGuildData;

		private GuildShareDetailData mGuildDetailData;

		private SequencePool m_seqPool = new SequencePool();
	}
}
