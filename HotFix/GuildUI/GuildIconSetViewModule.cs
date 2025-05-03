using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Guild;
using SuperScrollView;

namespace HotFix.GuildUI
{
	public class GuildIconSetViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.popCommon.OnClick = new Action<int>(this.OnPopClick);
			this.Button_Save.m_onClick = new Action(this.OnSaveSelectIcon);
			this.GridView.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
		}

		private void OnSaveSelectIcon()
		{
			if (this.mOpenData == null)
			{
				return;
			}
			GuildShareData shareData = base.SDK.GuildInfo.GuildDetailData.ShareData;
			GuildCreateData guildCreateData = new GuildCreateData();
			guildCreateData.CloneFromShareData(shareData);
			guildCreateData.GuildLogo = this.mCurIconId;
			GuildNetUtil.Guild.DoRequest_ModifyGuildInfo(guildCreateData, delegate(bool result, GuildModifyResponse resp)
			{
				if (result)
				{
					Action<int> callback = this.mOpenData.callback;
					if (callback != null)
					{
						callback(this.mCurIconId);
					}
					this.CloseThisView();
				}
			});
		}

		protected override void OnViewOpen(object data)
		{
			base.OnViewOpen(data);
			GuildIconSetData guildIconSetData = data as GuildIconSetData;
			if (guildIconSetData != null)
			{
				this.mOpenData = guildIconSetData;
				this.mDataList.Clear();
				List<Guild_guildStyle> guildStyleTableAll = GuildProxy.Table.GetGuildStyleTableAll();
				for (int i = 0; i < guildStyleTableAll.Count; i++)
				{
					this.mDataList.Add(guildStyleTableAll[i]);
				}
				this.mCurIconId = this.mOpenData.defaultIconId;
				this.CurrentIcon.SetIcon(this.mCurIconId);
				this.GridView.SetListItemCount(this.mDataList.Count, true);
				this.GridView.RefreshAllShownItem();
				return;
			}
			HLog.LogError("GuildIconSetViewModule OpenData Error!!!");
		}

		protected override void OnViewClose()
		{
		}

		protected override void OnViewDelete()
		{
			if (this.Button_Save != null)
			{
				this.Button_Save.m_onClick = new Action(this.OnSaveSelectIcon);
			}
			foreach (KeyValuePair<int, GuildIconStyleCtrl> keyValuePair in this.mUIItemDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUIItemDic.Clear();
			UIGuildIcon currentIcon = this.CurrentIcon;
			if (currentIcon == null)
			{
				return;
			}
			currentIcon.DeInit();
		}

		private void onClickStyle(GuildIconStyleCtrl item, int icon)
		{
			this.mCurIconId = icon;
			item.SetSelect(true);
			this.CurrentIcon.SetIcon(this.mCurIconId);
			foreach (KeyValuePair<int, GuildIconStyleCtrl> keyValuePair in this.mUIItemDic)
			{
				keyValuePair.Value.RefreshSelect(this.mCurIconId);
			}
		}

		private LoopGridViewItem OnGetItemByIndex(LoopGridView view, int index, int row, int column)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			Guild_guildStyle guild_guildStyle = this.mDataList[index];
			LoopGridViewItem loopGridViewItem = view.NewListViewItem("SelGuildIcon");
			int instanceID = loopGridViewItem.gameObject.GetInstanceID();
			GuildIconStyleCtrl guildIconStyleCtrl = this.TryGetUI(instanceID);
			if (guildIconStyleCtrl == null)
			{
				guildIconStyleCtrl = this.TryAddUI(instanceID, loopGridViewItem, loopGridViewItem.GetComponent<GuildIconStyleCtrl>());
			}
			guildIconStyleCtrl.SetData(guild_guildStyle.ID);
			guildIconStyleCtrl.RefreshSelect(this.mCurIconId);
			return loopGridViewItem;
		}

		private GuildIconStyleCtrl TryGetUI(int key)
		{
			GuildIconStyleCtrl guildIconStyleCtrl;
			if (this.mUIItemDic.TryGetValue(key, out guildIconStyleCtrl))
			{
				return guildIconStyleCtrl;
			}
			return null;
		}

		private GuildIconStyleCtrl TryAddUI(int key, LoopGridViewItem loopitem, GuildIconStyleCtrl ui)
		{
			ui.Init();
			ui.OnSelect = new Action<GuildIconStyleCtrl, int>(this.onClickStyle);
			GuildIconStyleCtrl guildIconStyleCtrl;
			if (this.mUIItemDic.TryGetValue(key, out guildIconStyleCtrl))
			{
				if (guildIconStyleCtrl == null)
				{
					guildIconStyleCtrl = ui;
					this.mUIItemDic[key] = ui;
				}
				return ui;
			}
			this.mUIItemDic.Add(key, ui);
			return ui;
		}

		private void CloseThisView()
		{
			GuildProxy.UI.CloseUIGuildIconSet(null);
		}

		private void OnPopClick(int kind)
		{
			this.CloseThisView();
		}

		public UIGuildPopCommon popCommon;

		public UIGuildIcon CurrentIcon;

		public CustomButton Button_Save;

		public LoopGridView GridView;

		private List<Guild_guildStyle> mDataList = new List<Guild_guildStyle>();

		private Dictionary<int, GuildIconStyleCtrl> mUIItemDic = new Dictionary<int, GuildIconStyleCtrl>();

		private int mCurIconId;

		private GuildIconSetData mOpenData;
	}
}
