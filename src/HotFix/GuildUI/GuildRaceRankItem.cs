using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildRaceRankItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Image_Rank1.SetActive(false);
			this.Image_Rank2.SetActive(false);
			this.Image_Rank3.SetActive(false);
			this.GuildIcon.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			UIGuildIcon guildIcon = this.GuildIcon;
			if (guildIcon == null)
			{
				return;
			}
			guildIcon.DeInit();
		}

		public void SetData(int rank, GuildRaceGuild data)
		{
			this.mRank = rank;
			this.mData = data;
		}

		public void RefreshUI()
		{
			if (this.mData == null)
			{
				return;
			}
			this.Image_Rank1.SetActive(this.mRank == 1);
			this.Image_Rank2.SetActive(this.mRank == 2);
			this.Image_Rank3.SetActive(this.mRank == 3);
			if (this.mRank > 3)
			{
				this.Text_Rank.text = this.mRank.ToString();
			}
			else
			{
				this.Text_Rank.text = "";
			}
			this.Text_Name.text = this.mData.ShareData.GuildShowName;
			this.Text_Power.text = GuildProxy.Language.GetInfoByID1("400416", GuildProxy.Language.FormatNumber((long)this.mData.TotalPower));
			this.Text_Score.text = GuildProxy.Language.GetInfoByID_LogError(400455) + "\r\n" + this.mData.RaceScore.ToString();
			this.GuildIcon.SetIcon(this.mData.ShareData.GuildIcon);
			this.GuildIcon.SetActive(true);
		}

		public void RefreshAsNull()
		{
			this.Image_Rank1.SetActive(false);
			this.Image_Rank2.SetActive(false);
			this.Image_Rank3.SetActive(false);
			this.Text_Rank.text = "";
			this.Text_Name.text = "";
			this.Text_Power.text = "";
			this.Text_Score.text = "";
			this.GuildIcon.SetActive(false);
		}

		public GameObject Image_Rank1;

		public GameObject Image_Rank2;

		public GameObject Image_Rank3;

		public CustomText Text_Rank;

		public CustomText Text_Name;

		public CustomText Text_Power;

		public CustomText Text_Score;

		public UIGuildIcon GuildIcon;

		private int mRank;

		private GuildRaceGuild mData;
	}
}
