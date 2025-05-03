using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildRaceRecordMainUserVS_PlayerUI : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Head.Init();
			this.Obj_Empty.SetActive(false);
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
			List<GuildRaceUserVSRecordResult> resultList = this.VSData.ResultList;
			int num = 0;
			for (int i = 0; i < resultList.Count; i++)
			{
				if (resultList[i].WinUserID == this.Data.UserData.UserID)
				{
					num++;
				}
				else
				{
					num--;
				}
			}
			this.mIsWin = num > 0;
		}

		public void RefreshUI()
		{
			if (this.Data == null || this.Data.IsEmptyUser || this.VSData == null)
			{
				this.RefreshUIAsEmpty();
				return;
			}
			this.Obj_Empty.SetActive(false);
			this.Obj_EmptyIcon.SetActive(false);
			this.Head.gameObject.SetActive(true);
			this.Head.Refresh(this.Data.UserData.Avatar, this.Data.UserData.AvatarFrame);
			this.Head.SetDefaultClick(this.Data.UserData.UserID);
			this.Text_Name.text = this.Data.UserData.GetNick();
			if (this.mIsWin)
			{
				this.Text_Result.text = GuildProxy.Language.GetInfoByID_LogError(400427);
				this.Text_Result.color = new Color32(68, 199, 33, byte.MaxValue);
				GuildRace_baseRace raceBaseTable = GuildProxy.Table.GetRaceBaseTable(this.Data.Position);
				if (raceBaseTable != null)
				{
					this.Text_Score.text = GuildProxy.Language.GetInfoByID1_LogError(400429, raceBaseTable.TypeIntArray[1]);
				}
				else
				{
					this.Text_Score.text = GuildProxy.Language.GetInfoByID1_LogError(400429, 0);
				}
				this.Text_Score.color = new Color32(4, 161, 51, byte.MaxValue);
			}
			else
			{
				this.Text_Result.text = GuildProxy.Language.GetInfoByID_LogError(400428);
				this.Text_Result.color = new Color32(182, 190, 187, byte.MaxValue);
				this.Text_Score.text = GuildProxy.Language.GetInfoByID1_LogError(400429, 0);
				this.Text_Score.color = new Color32(125, 125, 125, byte.MaxValue);
			}
			this.SetGuildName(this.Data.GuildName);
		}

		private void RefreshUIAsEmpty()
		{
			this.Obj_Empty.SetActive(false);
			this.Head.gameObject.SetActive(false);
			this.Obj_EmptyIcon.SetActive(true);
			this.Head.SetActive(false);
			this.Text_Name.text = GuildProxy.Language.GetInfoByID_LogError(400448);
			this.Text_Result.text = GuildProxy.Language.GetInfoByID_LogError(400428);
			this.Text_Score.text = GuildProxy.Language.GetInfoByID1_LogError(400429, 0);
			this.Text_Score.color = new Color32(125, 125, 125, byte.MaxValue);
			this.Text_Result.color = new Color32(182, 190, 187, byte.MaxValue);
		}

		public void SetGuildName(string guildname)
		{
			this.Text_Guild.text = guildname;
		}

		public UIGuildHead Head;

		public CustomText Text_Name;

		public CustomText Text_Guild;

		public CustomText Text_Score;

		public CustomText Text_Result;

		public GameObject Obj_Empty;

		public GameObject Obj_EmptyIcon;

		public GuildRaceMember Data;

		public GuildRaceUserVSRecord VSData;

		[HideInInspector]
		public int BattleIndex;

		private bool mIsWin;
	}
}
