using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildRaceInfo : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Button_Record.onClick.AddListener(new UnityAction(this.OnOpenRecordUI));
		}

		protected override void GuildUI_OnUnInit()
		{
			CustomButton button_Record = this.Button_Record;
			if (button_Record == null)
			{
				return;
			}
			button_Record.onClick.RemoveListener(new UnityAction(this.OnOpenRecordUI));
		}

		protected override void GuildUI_OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.CheckRefreshTime();
		}

		private void CheckRefreshTime()
		{
			if (GuildRaceBattleController.Instance == null)
			{
				return;
			}
			if (this.mSetAsNull)
			{
				return;
			}
			GuildRaceBattleController instance = GuildRaceBattleController.Instance;
			long num = GuildProxy.Net.ServerTime();
			if (num != this.mLastRefreshTime)
			{
				this.mLastRefreshTime = num;
				string raceDanName = GuildProxy.Language.GetRaceDanName(base.SDK.GuildActivity.GuildRace.RaceDan);
				this.Text_RaceDan.text = GuildProxy.Language.GetInfoByID1_LogError(400402, raceDanName);
				long sessionEndTime = (long)instance.SessionEndTime;
				this.Text_RaceTime.text = GuildProxy.Language.GetInfoByID1("400403", GuildProxy.Language.GetRaceEndTime(sessionEndTime - num));
				if (instance.CurrentRaceKind == GuildRaceStageKind.Battle && !instance.HasGetBattleRecord)
				{
					this.Text_Score.text = GuildProxy.Language.GetInfoByID1("400410", 0);
					return;
				}
				this.Text_Score.text = GuildProxy.Language.GetInfoByID1("400410", instance.MyGuildCurScore);
			}
		}

		public void RefreshUIOnOpenView()
		{
			this.mSetAsNull = false;
			this.CheckRefreshTime();
		}

		private void OnOpenRecordUI()
		{
			GuildRaceBattleController instance = GuildRaceBattleController.Instance;
			if (instance == null || instance.CurrentRaceStage == null)
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID_LogError(400458));
				return;
			}
			if (!instance.IsMyGuildJoinRace())
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID_LogError(400457));
				return;
			}
			if (instance.CurrentRaceStage.StageKind < GuildRaceStageKind.GuildMate)
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID_LogError(400458));
				return;
			}
			GuildProxy.UI.OpenGuildRaceRecord(null);
		}

		public void RefreshUIAsNULL()
		{
			this.Text_RaceDan.text = "";
			this.Text_RaceTime.text = "";
			this.Text_Score.text = "";
			this.mSetAsNull = true;
		}

		public CustomText Text_RaceDan;

		public CustomText Text_RaceTime;

		public CustomText Text_Score;

		public CustomButton Button_Record;

		private bool mSetAsNull;

		private long mLastRefreshTime;
	}
}
