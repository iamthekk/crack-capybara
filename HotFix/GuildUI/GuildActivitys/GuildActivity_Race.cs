using System;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI.GuildActivitys
{
	public class GuildActivity_Race : GuildActivityBase
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnUnInit();
		}

		public override void RefreshUIOnOpen()
		{
		}

		protected override void GuildUI_OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		private void RefreshRaceBeforeServer()
		{
			this.Text_Dan.text = "";
			this.Text_Time.text = "";
			this.Text_ActivityState.text = "";
			this.Obj_Apply.SetActive(false);
		}

		private void RefreshRaceAfterServer()
		{
			if (!base.IsActive() || this.RaceCtrl == null)
			{
				return;
			}
			int raceDan = base.SDK.GuildActivity.GuildRace.RaceDan;
			this.Text_Dan.text = GuildProxy.Language.GetInfoByID1_LogError(400402, GuildProxy.Language.GetRaceDanName(raceDan));
			this.OnUpdate(0f, 0f);
		}

		private void RefreshCurrentStageState(GuildRaceStagePart stage)
		{
			if (stage == null)
			{
				this.Obj_Apply.SetActive(false);
				this.Text_ActivityState.text = "";
				this.Text_Time.text = "";
				return;
			}
			string text = "";
			GuildActivityRace guildRace = base.SDK.GuildActivity.GuildRace;
			bool flag = guildRace.IsGuildReg && guildRace.IsMemberReg;
			switch (stage.StageKind)
			{
			case GuildRaceStageKind.GuildApply:
				text = GuildProxy.Language.GetInfoByID_LogError(400519);
				break;
			case GuildRaceStageKind.GuildMate:
				text = GuildProxy.Language.GetInfoByID_LogError(400520);
				break;
			case GuildRaceStageKind.UserApply:
				text = GuildProxy.Language.GetInfoByID_LogError(400521);
				break;
			case GuildRaceStageKind.BattlePrepare:
				text = GuildProxy.Language.GetInfoByID_LogError(400522);
				break;
			case GuildRaceStageKind.Battle:
				text = GuildProxy.Language.GetInfoByID_LogError(400523);
				break;
			case GuildRaceStageKind.BattleOver:
				text = GuildProxy.Language.GetInfoByID_LogError(400524);
				break;
			case GuildRaceStageKind.SeasonOver:
				text = GuildProxy.Language.GetInfoByID_LogError(400524);
				break;
			}
			this.Obj_Apply.SetActive(flag);
			this.Text_ActivityState.text = text;
			this.Text_Time.text = GuildProxy.Language.GetInfoByID1_LogError(400526, GuildProxy.Language.GetLongNumberTime(this.mLastShowTimeSec));
		}

		protected override void OnClickThis()
		{
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("4044"));
		}

		public GameObject Obj_Apply;

		public CustomText Text_Dan;

		public CustomText Text_Time;

		public CustomText Text_ActivityState;

		public GuildRaceBattleController RaceCtrl;

		private long mLastShowTimeSec;
	}
}
