using System;
using Dxx.Guild;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildRaceRecordMainUserVS : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Player1.Init();
			this.Player2.Init();
			this.VS.Init();
			this.Obj_Super.SetActive(false);
		}

		protected override void GuildUI_OnUnInit()
		{
			GuildRaceRecordMainUserVS_PlayerUI player = this.Player1;
			if (player != null)
			{
				player.DeInit();
			}
			GuildRaceRecordMainUserVS_PlayerUI player2 = this.Player2;
			if (player2 != null)
			{
				player2.DeInit();
			}
			GuildRaceRecordMainUserVS_VSUI vs = this.VS;
			if (vs == null)
			{
				return;
			}
			vs.DeInit();
		}

		public void SetData(GuildRaceUserVSRecord data, int index)
		{
			this.mData = data;
			this.mIndex = index;
			this.VS.SetData(data);
			this.Player1.SetData(data, data.User1, index);
			this.Player2.SetData(data, data.User2, index);
		}

		public void RefreshUI()
		{
			this.VS.RefreshUI();
			this.Player1.RefreshUI();
			this.Player2.RefreshUI();
			if (this.mData != null && this.mData.IsSuperPosition)
			{
				this.Obj_Super.SetActive(true);
				return;
			}
			this.Obj_Super.SetActive(false);
		}

		public GuildRaceRecordMainUserVS_PlayerUI Player1;

		public GuildRaceRecordMainUserVS_PlayerUI Player2;

		public GuildRaceRecordMainUserVS_VSUI VS;

		public GameObject Obj_Super;

		private GuildRaceUserVSRecord mData;

		private int mIndex;
	}
}
