using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildRaceRecordBattleItemPlayerUI : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Head.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			UIGuildHead head = this.Head;
			if (head == null)
			{
				return;
			}
			head.DeInit();
		}

		public void SetData(GuildRaceUserVSRecord vsdata, GuildRaceMember data, int index)
		{
			this.VSData = vsdata;
			this.Data = data;
			this.BattleIndex = index;
			this.CurrentRoundResult = this.VSData.ResultList[index];
		}

		public void RefreshUI()
		{
			if (this.Data == null || this.Data.IsEmptyUser || this.VSData == null)
			{
				return;
			}
			this.Head.Refresh(this.Data.UserData.Avatar, this.Data.UserData.AvatarFrame);
			this.Head.SetDefaultClick(this.Data.UserData.UserID);
			this.Text_Name.text = this.Data.UserData.GetNick();
			if (this.CurrentRoundResult.WinUserID == this.Data.UserData.UserID)
			{
				this.Text_Result.text = GuildProxy.Language.GetInfoByID_LogError(400427);
				this.Text_Result.color = new Color32(68, 199, 33, byte.MaxValue);
			}
			else
			{
				this.Text_Result.text = GuildProxy.Language.GetInfoByID_LogError(400428);
				this.Text_Result.color = new Color32(182, 190, 187, byte.MaxValue);
			}
			this.SetGuildName(this.Data.GuildName);
		}

		public void SetGuildName(string guildname)
		{
			this.Text_Guild.text = guildname;
		}

		public UIGuildHead Head;

		public CustomText Text_Name;

		public CustomText Text_Guild;

		public CustomText Text_Result;

		public GuildRaceMember Data;

		public GuildRaceUserVSRecord VSData;

		[HideInInspector]
		public int BattleIndex;

		public GuildRaceUserVSRecordResult CurrentRoundResult;
	}
}
