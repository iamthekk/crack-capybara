using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework;
using HotFix.GuildUI.GuildCheckPopUI;
using Proto.Guild;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildCheckPopViewModule : GuildProxy.GuildProxy_BaseView
	{
		private bool IsViewOpen
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		protected override void OnViewCreate()
		{
			this.popCommon.OnClick = new Action<int>(this.OnPopClick);
			this.uiCtrlList.Add(this.guildInfo);
			this.uiCtrlList.Add(this.guildMembers);
			this.HideLoading();
			foreach (GuildCheckPop_Base guildCheckPop_Base in this.uiCtrlList)
			{
				guildCheckPop_Base.Init();
			}
		}

		protected override void OnViewOpen(object data)
		{
			this.guildDetailData = data as GuildShareDetailData;
			this.ReqGuildDetail();
		}

		protected override void OnViewClose()
		{
			for (int i = 0; i < this.uiCtrlList.Count; i++)
			{
				this.uiCtrlList[i].Close();
			}
		}

		protected override void OnViewDelete()
		{
			foreach (GuildCheckPop_Base guildCheckPop_Base in this.uiCtrlList)
			{
				guildCheckPop_Base.DeInit();
			}
			this.uiCtrlList.Clear();
		}

		private void RefreshMembers()
		{
			if (this.guildDetailData == null)
			{
				return;
			}
			GuildCheckPop_Members guildCheckPop_Members = this.guildMembers;
			if (guildCheckPop_Members == null)
			{
				return;
			}
			guildCheckPop_Members.SetGuildDetailInfo(this.guildDetailData);
		}

		private void JoinGuild()
		{
			if (this.guildDetailData == null)
			{
				return;
			}
			GuildNetUtil.Guild.DoRequest_RequireJoinGuild(this.guildDetailData.GuildID, GuildSDKManager.Instance.User.UserLanguage, delegate(bool result, int joinResult, GuildApplyJoinResponse response)
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
						this.CloseSelfView();
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400108"));
						GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_Guild_ShowHall);
						return;
					case 2:
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400031"));
						return;
					case 3:
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400032"));
						break;
					default:
						return;
					}
				}
			});
		}

		private void ReqGuildDetail()
		{
			if (this.guildDetailData == null)
			{
				return;
			}
			if (!this.IsViewOpen)
			{
				return;
			}
			for (int i = 0; i < this.uiCtrlList.Count; i++)
			{
				this.uiCtrlList[i].Show();
				this.uiCtrlList[i].RefreshUI(this.guildDetailData.ShareData);
			}
			this.RefreshMembers();
		}

		private void ShowLoading()
		{
			this.objLoading.SetActive(true);
		}

		private void HideLoading()
		{
			this.objLoading.SetActive(false);
		}

		private void CloseSelfView()
		{
			GameApp.View.CloseView(ViewName.GuildCheckPopViewModule, null);
		}

		private void OnPopClick(int kind)
		{
			this.CloseSelfView();
		}

		public UIGuildPopCommon popCommon;

		public GameObject objLoading;

		public GuildCheckPop_Info guildInfo;

		public GuildCheckPop_Members guildMembers;

		private readonly List<GuildCheckPop_Base> uiCtrlList = new List<GuildCheckPop_Base>();

		private GuildShareDetailData guildDetailData;
	}
}
