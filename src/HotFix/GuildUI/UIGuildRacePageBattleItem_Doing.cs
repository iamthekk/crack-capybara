using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix.GuildUI
{
	public class UIGuildRacePageBattleItem_Doing : UIGuildRacePageBattleItem
	{
		protected override void GuildUI_OnInit()
		{
			this.Player1.Init();
			this.Player2.Init();
			this.VS.Init();
			this.Text_Score1.text = "0";
			this.Text_Score2.text = "0";
		}

		protected override void GuildUI_OnUnInit()
		{
			UIGuildRacePageBattleItem_Doing_Player player = this.Player1;
			if (player != null)
			{
				player.DeInit();
			}
			UIGuildRacePageBattleItem_Doing_Player player2 = this.Player2;
			if (player2 != null)
			{
				player2.DeInit();
			}
			UIGuildRacePageBattleItem_Doing_VS vs = this.VS;
			if (vs == null)
			{
				return;
			}
			vs.DeInit();
		}

		public override void RefreshUI()
		{
			this.Player1.SetData(this.Data, this.Data.User1);
			this.Player1.RefreshUI();
			this.Player2.SetData(this.Data, this.Data.User2);
			this.Player2.RefreshUI();
		}

		public void RefreshOnBattleChange(GuildRaceBattleProcess process)
		{
			if (process == null)
			{
				return;
			}
			GuildRaceBattleProcess.UserPK curUserPK = process.CurUserPK;
			if (curUserPK == null)
			{
				return;
			}
			int num = process.CurBattleIndex;
			List<GuildRaceBattleProcess.UserBattle> battles = curUserPK.Battles;
			int num2 = 0;
			while (num2 < battles.Count && num2 < num)
			{
				GuildRaceBattleProcess.UserBattle userBattle = battles[num2];
				if (userBattle != null)
				{
					GuildRaceUserVSRecordResult result = userBattle.Result;
					if (result != null && result.WinUserID != 0L)
					{
						this.Player1.SetScore(num2, this.Player1.UserID == result.WinUserID);
						this.Player2.SetScore(num2, this.Player2.UserID == result.WinUserID);
					}
				}
				num2++;
			}
			if (num < 0)
			{
				num = 0;
			}
			this.Player1.RevertScore(num);
			this.Player2.RevertScore(num);
			this.RefreshScore(process);
		}

		public void RefreshScore(GuildRaceBattleProcess process)
		{
			int num = 0;
			int num2 = 0;
			int curUserPKIndex = process.CurUserPKIndex;
			List<GuildRaceBattleProcess.UserPK> pklist = process.PKList;
			int num3 = 0;
			while (num3 < pklist.Count && num3 < curUserPKIndex)
			{
				GuildRaceBattleProcess.UserPK userPK = pklist[num3];
				if (userPK != null)
				{
					GuildRace_baseRace raceBaseTable = GuildProxy.Table.GetRaceBaseTable(GuildProxy.Table.GuildRaceUserIndexToPosition(userPK.Index + 1));
					if (raceBaseTable == null)
					{
						HLog.LogError("排位赛 base 表错误，出战顺序未能找到相应的阵位");
					}
					else
					{
						int num4 = raceBaseTable.TypeIntArray[1];
						if (userPK.WinUser != null && !userPK.WinUser.IsEmptyUser && userPK.WinUser == userPK.User1)
						{
							num += num4;
						}
						if (userPK.WinUser != null && !userPK.WinUser.IsEmptyUser && userPK.WinUser == userPK.User2)
						{
							num2 += num4;
						}
					}
				}
				num3++;
			}
			this.Text_Score1.text = num.ToString();
			this.Text_Score2.text = num2.ToString();
		}

		public void RefreshCurBattle(GuildRaceBattleProcess process)
		{
			if (process == null)
			{
				return;
			}
			if (process.IsWaittingTime)
			{
				this.VS.SetStateWait();
				this.VS.SetTime(process.TimeSec, process.TimeSecMax);
				return;
			}
			this.VS.SetStateBattle();
			this.VS.SetTime(process.TimeSec, process.TimeSecMax);
		}

		public UIGuildRacePageBattleItem_Doing_Player Player1;

		public UIGuildRacePageBattleItem_Doing_Player Player2;

		public UIGuildRacePageBattleItem_Doing_VS VS;

		public CustomText Text_Score1;

		public CustomText Text_Score2;
	}
}
