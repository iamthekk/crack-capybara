using System;
using System.Collections.Generic;
using Dxx.Guild;
using SuperScrollView;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildRacePageOpponent : UIGuildRacePageBase
	{
		protected override void GuildUI_OnInit()
		{
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.Obj_EmptyList.SetActive(false);
		}

		protected override void GuildUI_OnUnInit()
		{
			foreach (KeyValuePair<int, UIGuildRacePageOpponentItem> keyValuePair in this.UICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.UICtrlDic.Clear();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index > this.MemberDataList.Count)
			{
				return null;
			}
			GuildRaceMember guildRaceMember = this.MemberDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("GuildRaceItem_Opponent");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UIGuildRacePageOpponentItem uiguildRacePageOpponentItem = this.TryGetUI(instanceID);
			if (uiguildRacePageOpponentItem == null)
			{
				uiguildRacePageOpponentItem = this.TryAddUI(instanceID, loopListViewItem, loopListViewItem.GetComponent<UIGuildRacePageOpponentItem>());
			}
			uiguildRacePageOpponentItem.SetData(guildRaceMember);
			uiguildRacePageOpponentItem.RefreshUI();
			return loopListViewItem;
		}

		private UIGuildRacePageOpponentItem TryGetUI(int key)
		{
			UIGuildRacePageOpponentItem uiguildRacePageOpponentItem;
			if (this.UICtrlDic.TryGetValue(key, out uiguildRacePageOpponentItem))
			{
				return uiguildRacePageOpponentItem;
			}
			return null;
		}

		private UIGuildRacePageOpponentItem TryAddUI(int key, LoopListViewItem2 loopitem, UIGuildRacePageOpponentItem ui)
		{
			ui.Init();
			UIGuildRacePageOpponentItem uiguildRacePageOpponentItem;
			if (this.UICtrlDic.TryGetValue(key, out uiguildRacePageOpponentItem))
			{
				if (uiguildRacePageOpponentItem == null)
				{
					uiguildRacePageOpponentItem = ui;
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
			this.Scroll.SetListItemCount(this.MemberDataList.Count, true);
			this.Scroll.RefreshAllShownItem();
			this.Obj_EmptyList.SetActive(this.MemberDataList.Count <= 0);
		}

		private int SortByIndex(GuildRaceMember x, GuildRaceMember y)
		{
			return x.UserData.UserID.CompareTo(y.UserData.UserID);
		}

		public LoopListView2 Scroll;

		public GameObject Obj_EmptyList;

		public List<GuildRaceMember> MemberDataList = new List<GuildRaceMember>();

		public Dictionary<int, UIGuildRacePageOpponentItem> UICtrlDic = new Dictionary<int, UIGuildRacePageOpponentItem>();
	}
}
