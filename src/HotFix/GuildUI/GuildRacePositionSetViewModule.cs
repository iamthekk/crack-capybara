using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using Proto.GuildRace;
using SuperScrollView;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildRacePositionSetViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.Button_Mask.onClick.AddListener(new UnityAction(this.ClickCloseThis));
			this.m_closeBt.onClick.AddListener(new UnityAction(this.ClickCloseThis));
			this.Button_Sure.onClick.AddListener(new UnityAction(this.OnSureSelected));
			this.Text_Cost.text = "";
			this.LoopGridView.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
		}

		protected override void OnViewOpen(object data)
		{
			GuildRacePositionSetViewModule.OpenData openData = data as GuildRacePositionSetViewModule.OpenData;
			if (openData != null)
			{
				this.mOpenData = openData;
				int num = GuildProxy.Table.GetRaceBaseTable(this.mOpenData.ReplaceUser.Position).TypeIntArray[0];
				this.mDataList.Clear();
				for (int i = 0; i < this.mOpenData.Members.Count; i++)
				{
					if (this.mOpenData.Members[i] != this.mOpenData.ReplaceUser && !this.mOpenData.Members[i].IsEmptyUser)
					{
						GuildRacePositionSetItemData guildRacePositionSetItemData = new GuildRacePositionSetItemData();
						guildRacePositionSetItemData.NeedAP = num;
						guildRacePositionSetItemData.SetData(this.mOpenData.Members[i]);
						this.mDataList.Add(guildRacePositionSetItemData);
					}
				}
				this.Text_Cost.text = GuildProxy.Language.GetInfoByID1_LogError(400509, num);
				this.RefreshListUI();
			}
		}

		protected override void OnViewClose()
		{
			CustomButton button_Mask = this.Button_Mask;
			if (button_Mask != null)
			{
				button_Mask.onClick.RemoveListener(new UnityAction(this.ClickCloseThis));
			}
			if (this.m_closeBt != null)
			{
				this.m_closeBt.onClick.RemoveListener(new UnityAction(this.ClickCloseThis));
			}
			CustomButton button_Sure = this.Button_Sure;
			if (button_Sure != null)
			{
				button_Sure.onClick.RemoveListener(new UnityAction(this.OnSureSelected));
			}
			foreach (KeyValuePair<int, GuildRacePositionSetItem> keyValuePair in this.mUIItemDic)
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.mUIItemDic.Clear();
		}

		protected override void OnViewDelete()
		{
		}

		public void RefreshListUI()
		{
			this.LoopGridView.SetListItemCount(this.mDataList.Count, true);
			this.LoopGridView.ScrollRect.verticalNormalizedPosition = 1f;
		}

		private LoopGridViewItem OnGetItemByIndex(LoopGridView view, int index, int row, int column)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			GuildRacePositionSetItemData guildRacePositionSetItemData = this.mDataList[index];
			LoopGridViewItem loopGridViewItem = this.LoopGridView.NewListViewItem("UserItem");
			int instanceID = loopGridViewItem.gameObject.GetInstanceID();
			GuildRacePositionSetItem guildRacePositionSetItem = this.TryGetUI(instanceID);
			if (guildRacePositionSetItem == null)
			{
				guildRacePositionSetItem = this.TryAddUI(instanceID, loopGridViewItem, loopGridViewItem.GetComponent<GuildRacePositionSetItem>());
			}
			guildRacePositionSetItem.SetData(guildRacePositionSetItemData);
			guildRacePositionSetItem.RefreshUI();
			guildRacePositionSetItem.SetAsSelect(guildRacePositionSetItem.Data == this.mSelectUser);
			return loopGridViewItem;
		}

		private GuildRacePositionSetItem TryGetUI(int key)
		{
			GuildRacePositionSetItem guildRacePositionSetItem;
			if (this.mUIItemDic.TryGetValue(key, out guildRacePositionSetItem))
			{
				return guildRacePositionSetItem;
			}
			return null;
		}

		private GuildRacePositionSetItem TryAddUI(int key, LoopGridViewItem loopitem, GuildRacePositionSetItem ui)
		{
			ui.Init();
			ui.OnClick = new Action<GuildRacePositionSetItem>(this.OnSelectUser);
			GuildRacePositionSetItem guildRacePositionSetItem;
			if (this.mUIItemDic.TryGetValue(key, out guildRacePositionSetItem))
			{
				if (guildRacePositionSetItem == null)
				{
					guildRacePositionSetItem = ui;
					this.mUIItemDic[key] = ui;
				}
				return ui;
			}
			this.mUIItemDic.Add(key, ui);
			return ui;
		}

		private void OnSelectUser(GuildRacePositionSetItem item)
		{
			if (item == null)
			{
				this.mSelectUser = null;
			}
			else
			{
				this.mSelectUser = item.Data;
			}
			foreach (KeyValuePair<int, GuildRacePositionSetItem> keyValuePair in this.mUIItemDic)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.SetAsSelect(keyValuePair.Value.Data == this.mSelectUser);
				}
			}
		}

		private void OnSureSelected()
		{
			if (this.mSelectUser == null)
			{
				return;
			}
			uint index = (uint)this.mOpenData.ReplaceUser.Index;
			long userID = this.mOpenData.ReplaceUser.UserData.UserID;
			long userID2 = this.mSelectUser.User.UserData.UserID;
			if (userID == userID2)
			{
				this.ClickCloseThis();
				return;
			}
			int num = GuildProxy.Table.GuildRaceMaxInPositionCount();
			if (index == 0U || (ulong)index > (ulong)((long)num))
			{
				HLog.LogError(string.Format("更换位置错误：错误的位置：{0}", index));
				this.ClickCloseThis();
				return;
			}
			GuildNetUtil.Guild.DoRequest_GuildRaceEditSeqRequest(index, userID2, delegate(bool result, GuildRaceEditSeqResponse resp)
			{
				if (result)
				{
					GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_GuildRace_ChangeSeq);
					if (base.CheckIsViewOpen())
					{
						this.ClickCloseThis();
					}
				}
			});
		}

		private void ClickCloseThis()
		{
			GuildProxy.UI.CloseGuildRacePositionSet();
		}

		public CustomButton Button_Mask;

		public CustomButton m_closeBt;

		public CustomButton Button_Sure;

		public CustomText Text_Cost;

		public LoopGridView LoopGridView;

		private List<GuildRacePositionSetItemData> mDataList = new List<GuildRacePositionSetItemData>();

		private Dictionary<int, GuildRacePositionSetItem> mUIItemDic = new Dictionary<int, GuildRacePositionSetItem>();

		private GuildRacePositionSetItemData mSelectUser;

		private GuildRacePositionSetViewModule.OpenData mOpenData;

		public class OpenData
		{
			public GuildRaceMember ReplaceUser;

			public List<GuildRaceMember> Members = new List<GuildRaceMember>();
		}
	}
}
