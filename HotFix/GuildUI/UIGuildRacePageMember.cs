using System;
using System.Collections.Generic;
using Dxx.Guild;
using SuperScrollView;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildRacePageMember : UIGuildRacePageBase
	{
		protected override void GuildUI_OnInit()
		{
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.Obj_EmptyList.SetActive(false);
			this.Obj_GuildNotJoin.SetActive(false);
			this.Obj_NoRaceGroup.SetActive(false);
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index > this.MemberDataList.Count)
			{
				return null;
			}
			GuildRaceMember guildRaceMember = this.MemberDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("GuildRaceItem_Member");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UIGuildRacePageMemberItem uiguildRacePageMemberItem = this.TryGetUI(instanceID);
			if (uiguildRacePageMemberItem == null)
			{
				uiguildRacePageMemberItem = this.TryAddUI(instanceID, loopListViewItem, loopListViewItem.GetComponent<UIGuildRacePageMemberItem>());
			}
			uiguildRacePageMemberItem.SetData(guildRaceMember);
			uiguildRacePageMemberItem.RefreshUI();
			return loopListViewItem;
		}

		private UIGuildRacePageMemberItem TryGetUI(int key)
		{
			UIGuildRacePageMemberItem uiguildRacePageMemberItem;
			if (this.UICtrlDic.TryGetValue(key, out uiguildRacePageMemberItem))
			{
				return uiguildRacePageMemberItem;
			}
			return null;
		}

		private UIGuildRacePageMemberItem TryAddUI(int key, LoopListViewItem2 loopitem, UIGuildRacePageMemberItem ui)
		{
			ui.Init();
			ui.OnClickThis = new Action<UIGuildRacePageMemberItem>(this.OnClickChangePosition);
			UIGuildRacePageMemberItem uiguildRacePageMemberItem;
			if (this.UICtrlDic.TryGetValue(key, out uiguildRacePageMemberItem))
			{
				if (uiguildRacePageMemberItem == null)
				{
					uiguildRacePageMemberItem = ui;
					this.UICtrlDic[key] = ui;
				}
				return ui;
			}
			this.UICtrlDic.Add(key, ui);
			return ui;
		}

		public void SetMemberList(List<GuildRaceMember> list)
		{
			this.MemberDataList = list;
			if (this.MemberDataList == null)
			{
				this.MemberDataList = new List<GuildRaceMember>();
			}
			this.MemberDataList.Sort(new Comparison<GuildRaceMember>(this.SortByIndex));
		}

		public override void RefreshUI()
		{
			if (base.gameObject == null || !base.gameObject.activeSelf)
			{
				return;
			}
			if (!base.SDK.GuildActivity.GuildRace.HasRaceGroup)
			{
				this.Obj_NoRaceGroup.SetActive(true);
				this.Obj_GuildNotJoin.SetActive(false);
				this.Obj_EmptyList.SetActive(false);
				return;
			}
			this.Obj_NoRaceGroup.SetActive(false);
			GuildRaceBattleController instance = GuildRaceBattleController.Instance;
			if (instance == null || !instance.IsMyGuildJoinRace())
			{
				this.Obj_GuildNotJoin.SetActive(true);
				this.Obj_EmptyList.SetActive(false);
				this.Scroll.SetListItemCount(0, true);
				this.Scroll.RefreshAllShownItem();
				return;
			}
			this.Obj_GuildNotJoin.SetActive(false);
			this.Scroll.SetListItemCount(this.MemberDataList.Count, true);
			this.Scroll.RefreshAllShownItem();
			this.Obj_EmptyList.SetActive(this.MemberDataList.Count <= 0);
		}

		private void OnClickChangePosition(UIGuildRacePageMemberItem item)
		{
			if (item == null)
			{
				return;
			}
			GuildRacePositionSetViewModule.OpenData openData = new GuildRacePositionSetViewModule.OpenData();
			openData.Members.AddRange(this.MemberDataList);
			openData.ReplaceUser = item.Data;
			GuildProxy.UI.OpenGuildRacePositionSet(openData);
		}

		private int SortByIndex(GuildRaceMember x, GuildRaceMember y)
		{
			return x.SortIndex.CompareTo(y.SortIndex);
		}

		public LoopListView2 Scroll;

		public GameObject Obj_EmptyList;

		public GameObject Obj_GuildNotJoin;

		public GameObject Obj_NoRaceGroup;

		public List<GuildRaceMember> MemberDataList = new List<GuildRaceMember>();

		public Dictionary<int, UIGuildRacePageMemberItem> UICtrlDic = new Dictionary<int, UIGuildRacePageMemberItem>();
	}
}
