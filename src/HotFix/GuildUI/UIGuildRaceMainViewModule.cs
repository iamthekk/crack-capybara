using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.EventSystem;
using Framework.Logic.UI;
using Proto.GuildRace;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildRaceMainViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.Button_Return.onClick.AddListener(new UnityAction(this.OnCloseSelfView));
			this.Button_Rank.onClick.AddListener(new UnityAction(this.OnClickOpenRank));
			this.Button_Challenge.onClick.AddListener(new UnityAction(this.OnClickChallenge));
			this.RaceInfo.Init();
			this.RaceContent.Init();
			this.UILoading.SetActive(false);
		}

		protected override void OnViewOpen(object data)
		{
			this.mCurStateKind = (GuildRaceStageKind)0;
			this.Animator.Play("open");
			this.GetDataFromServerAndRefreshUI(true);
		}

		protected override void OnViewRegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_GuildRace_GuildApply, new HandlerEvent(this.OnRefreshAfterGuildApply));
			manager.RegisterEvent(LocalMessageName.CC_GuildRace_ChangeSeq, new HandlerEvent(this.OnRefreshAfterChangeSeq));
			manager.RegisterEvent(LocalMessageName.CC_GuildRace_UserApply, new HandlerEvent(this.OnRefreshAfterUserApply));
		}

		protected override void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.RaceCtrl != null)
			{
				this.RaceCtrl.OnUpdate(unscaledDeltaTime);
				if (this.RaceCtrl.IsSeasonAllOver)
				{
					this.HandleAfterSeasonAllOver();
					return;
				}
				if (this.mLastShowTimeSec != (long)this.RaceCtrl.CurStageLeftSec)
				{
					this.mLastShowTimeSec = (long)this.RaceCtrl.CurStageLeftSec;
					this.RefreshCurrentStageState(this.RaceCtrl.CurrentRaceStage);
				}
			}
			UIGuildRaceInfo raceInfo = this.RaceInfo;
			if (raceInfo != null)
			{
				raceInfo.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			UIGuildRaceContent raceContent = this.RaceContent;
			if (raceContent == null)
			{
				return;
			}
			raceContent.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnViewUnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_GuildRace_GuildApply, new HandlerEvent(this.OnRefreshAfterGuildApply));
			manager.UnRegisterEvent(LocalMessageName.CC_GuildRace_ChangeSeq, new HandlerEvent(this.OnRefreshAfterChangeSeq));
			manager.UnRegisterEvent(LocalMessageName.CC_GuildRace_UserApply, new HandlerEvent(this.OnRefreshAfterUserApply));
		}

		protected override void OnViewClose()
		{
			this.RaceInfo.Close();
			this.RaceContent.Close();
			GuildRaceBattleController.Destroy();
		}

		protected override void OnViewDelete()
		{
			CustomButton button_Return = this.Button_Return;
			if (button_Return != null)
			{
				button_Return.onClick.RemoveListener(new UnityAction(this.OnCloseSelfView));
			}
			CustomButton button_Rank = this.Button_Rank;
			if (button_Rank != null)
			{
				button_Rank.onClick.RemoveListener(new UnityAction(this.OnClickOpenRank));
			}
			CustomButton button_Challenge = this.Button_Challenge;
			if (button_Challenge != null)
			{
				button_Challenge.onClick.RemoveListener(new UnityAction(this.OnClickChallenge));
			}
			if (this.RaceInfo != null)
			{
				this.RaceInfo.DeInit();
				Object.Destroy(this.RaceInfo);
			}
			if (this.RaceContent != null)
			{
				this.RaceContent.DeInit();
				Object.Destroy(this.RaceContent);
			}
		}

		private void OnCloseSelfView()
		{
			GuildProxy.UI.CloseUIGuildRace();
		}

		private void OnClickOpenRank()
		{
			if (this.RaceCtrl == null || !this.RaceCtrl.IsMyGuildJoinRace())
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID_LogError(400457));
				return;
			}
			GuildProxy.UI.OpenGuildRaceRank(null);
		}

		private void OnClickChallenge()
		{
			if (this.RaceCtrl == null || this.RaceCtrl.CurrentRaceStage == null)
			{
				HLog.LogError("RaceCtrl == null ？？？");
				return;
			}
			if (!base.SDK.GuildActivity.GuildRace.HasRaceGroup)
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID_LogError(400525));
				return;
			}
			GuildRaceStageKind stageKind = this.RaceCtrl.CurrentRaceStage.StageKind;
			if (stageKind == GuildRaceStageKind.GuildApply)
			{
				this.RaceHandle_GuildApply();
				return;
			}
			if (stageKind != GuildRaceStageKind.UserApply)
			{
				return;
			}
			this.RaceHandle_UserApply();
		}

		private void RaceHandle_GuildApply()
		{
			if (this.RaceCtrl.IsMyGuildJoinRace())
			{
				return;
			}
			if (!base.SDK.Permission.HasPermission(GuildPermissionKind.GuildActivities, null))
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID_LogError(400453));
				return;
			}
			GuildProxy.UI.OpenGuildRaceSeasonSelect(null);
		}

		private void RaceHandle_UserApply()
		{
			if (!this.RaceCtrl.IsMyGuildJoinRace())
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID_LogError(400457));
				return;
			}
			if (this.RaceCtrl.IsMeJoinRace())
			{
				return;
			}
			GuildNetUtil.Guild.DoRequest_GuildRaceUserApplyRequest(delegate(bool result, GuildRaceUserApplyResponse resp)
			{
				if (result)
				{
					GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_GuildRace_UserApply);
				}
			});
		}

		private void GetDataFromServerAndRefreshUI(bool isviewopen)
		{
			this.OnRefreshUIBeforeServer();
			if (isviewopen)
			{
				this.RaceInfo.Show();
				this.RaceContent.Show();
			}
			GuildNetUtil.Guild.DoRequest_GuildRaceInfoRequest(delegate(bool result, GuildRaceInfoResponse resp)
			{
				if (base.CheckIsViewOpen())
				{
					if (result)
					{
						if (GuildRaceBattleController.Instance == null)
						{
							GuildRaceBattleController.Create();
						}
						this.RaceCtrl = GuildRaceBattleController.Instance;
						this.OnGetAndRefreshBattleProcess();
						this.OnRefreshUIAfterServer();
						return;
					}
					HLog.LogError("获取公会排位赛数据失败！！！");
				}
			});
		}

		private void OnRefreshUIBeforeServer()
		{
			this.RefreshCurrentStageState(null);
			this.UILoading.SetActive(true);
			this.RaceInfo.RefreshUIAsNULL();
			this.RaceContent.RefreshUIAsNULL();
		}

		private void OnRefreshUIAfterServer()
		{
			this.RaceInfo.RefreshUIOnOpenView();
			this.RaceContent.RefreshUIOnOpenView(delegate
			{
				if (!base.CheckIsViewOpen())
				{
					return;
				}
				this.UILoading.SetActive(false);
			});
			this.CheckShowDanChange();
		}

		private void CheckShowDanChange()
		{
			if (!base.SDK.GetModule<GuildUIDataModule>().RaceDanChange.HasShowRaceDan())
			{
				GuildProxy.UI.OpenGuildRaceDanChange(new GuildRaceDanChangeViewModule.OpenData
				{
					OnViewClose = new Action(this.OnDanChangeViewClose)
				});
				return;
			}
			this.CheckShowMatch();
		}

		private void OnDanChangeViewClose()
		{
			this.CheckShowMatch();
		}

		private void CheckShowMatch()
		{
			if (this.RaceCtrl == null)
			{
				return;
			}
			if (this.RaceCtrl.CurrentRaceKind != GuildRaceStageKind.UserApply)
			{
				return;
			}
			int curBattleDay = this.RaceCtrl.GetCurBattleDay();
			if (base.SDK.GetModule<GuildUIDataModule>().RaceBattleMatch.CanShowMatch(curBattleDay))
			{
				GuildActivityRace guildRace = base.SDK.GuildActivity.GuildRace;
				IList<GuildRaceGuild> allGuildOfGroup = guildRace.AllGuildOfGroup;
				GuildRaceGuild guildRaceInfo = guildRace.GuildRaceInfo;
				GuildRaceGuild guildRaceGuild = null;
				if (allGuildOfGroup != null && guildRaceInfo != null && !string.IsNullOrEmpty(guildRaceInfo.OppGuildID))
				{
					for (int i = 0; i < allGuildOfGroup.Count; i++)
					{
						if (allGuildOfGroup[i].GuildID == guildRaceInfo.OppGuildID)
						{
							guildRaceGuild = allGuildOfGroup[i];
							break;
						}
					}
				}
				if (guildRaceInfo != null && guildRaceGuild != null)
				{
					GuildProxy.UI.OpenGuildRaceBattleMatchViewModule(new GuildRaceBattleMatchViewModule.OpenData
					{
						Day = curBattleDay,
						MyGuild = guildRaceInfo,
						OtherGuild = guildRaceGuild
					});
				}
			}
		}

		private void OnGetAndRefreshBattleProcess()
		{
			if (this.RaceCtrl == null || !this.RaceCtrl.IsCanGetCurDayBattleRecord())
			{
				return;
			}
			this.RaceCtrl.LoadMyGuildCurDayBattle(null);
		}

		private void RefreshCurrentStageState(GuildRaceStagePart stage)
		{
			if (stage == null)
			{
				this.Text_ChallengeStage.text = "";
				this.Gray_Challenge.SetUIGray();
				this.Text_ChallengeTime.text = "";
				return;
			}
			string text = "";
			bool flag = false;
			GuildActivityRace guildRace = base.SDK.GuildActivity.GuildRace;
			switch (stage.StageKind)
			{
			case GuildRaceStageKind.GuildApply:
				if (guildRace.IsGuildReg)
				{
					text = GuildProxy.Language.GetInfoByID_LogError(400512);
					flag = true;
				}
				else
				{
					text = GuildProxy.Language.GetInfoByID_LogError(400511);
				}
				break;
			case GuildRaceStageKind.GuildMate:
				if (guildRace.IsGuildReg)
				{
					text = GuildProxy.Language.GetInfoByID_LogError(400513);
				}
				else
				{
					text = GuildProxy.Language.GetInfoByID_LogError(400456);
				}
				flag = true;
				break;
			case GuildRaceStageKind.UserApply:
				if (guildRace.IsGuildReg)
				{
					if (guildRace.IsMemberReg)
					{
						text = GuildProxy.Language.GetInfoByID_LogError(400515);
						flag = true;
					}
					else
					{
						text = GuildProxy.Language.GetInfoByID_LogError(400514);
					}
				}
				else
				{
					text = GuildProxy.Language.GetInfoByID_LogError(400456);
					flag = true;
				}
				break;
			case GuildRaceStageKind.BattlePrepare:
				if (guildRace.IsGuildReg)
				{
					text = GuildProxy.Language.GetInfoByID_LogError(400516);
				}
				else
				{
					text = GuildProxy.Language.GetInfoByID_LogError(400456);
				}
				flag = true;
				break;
			case GuildRaceStageKind.Battle:
				if (guildRace.IsGuildReg)
				{
					text = GuildProxy.Language.GetInfoByID_LogError(400517);
				}
				else
				{
					text = GuildProxy.Language.GetInfoByID_LogError(400456);
				}
				flag = true;
				break;
			case GuildRaceStageKind.BattleOver:
				if (guildRace.IsGuildReg)
				{
					text = GuildProxy.Language.GetInfoByID_LogError(400518);
				}
				else
				{
					text = GuildProxy.Language.GetInfoByID_LogError(400456);
				}
				flag = true;
				break;
			case GuildRaceStageKind.SeasonOver:
				if (guildRace.IsGuildReg)
				{
					text = GuildProxy.Language.GetInfoByID_LogError(400518);
				}
				else
				{
					text = GuildProxy.Language.GetInfoByID_LogError(400456);
				}
				flag = true;
				break;
			}
			this.Text_ChallengeStage.text = text;
			if (flag)
			{
				this.Gray_Challenge.SetUIGray();
			}
			else
			{
				this.Gray_Challenge.Recovery();
			}
			this.RefreshByStateChangeKind(stage.StageKind);
			this.Text_ChallengeTime.text = GuildProxy.Language.GetLongNumberTime(this.mLastShowTimeSec);
		}

		private void RefreshByStateChangeKind(GuildRaceStageKind stage)
		{
			if (this.mCurStateKind == stage)
			{
				return;
			}
			GuildRaceStageKind guildRaceStageKind = this.mCurStateKind;
			this.mCurStateKind = stage;
			if (this.mCurStateKind == GuildRaceStageKind.UserApply)
			{
				if (guildRaceStageKind == GuildRaceStageKind.BattleOver)
				{
					GuildEvent_RaceApply guildEvent_RaceApply = new GuildEvent_RaceApply();
					guildEvent_RaceApply.IsGuildApply = -1;
					guildEvent_RaceApply.IsUserApply = 0;
					base.SDK.Event.DispatchNow(402, guildEvent_RaceApply);
					this.RefreshCurrentStageState(this.RaceCtrl.CurrentRaceStage);
				}
				this.CheckShowMatch();
			}
			GuildProxy.RedPoint.CalcRedPoint("Guild.Activity.Race", true);
		}

		private void OnRefreshAfterGuildApply(object sender, int type, BaseEventArgs eventArgs)
		{
			this.GetDataFromServerAndRefreshUI(false);
		}

		private void OnRefreshAfterChangeSeq(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.RaceContent != null)
			{
				this.RaceContent.ForceLoadData();
			}
		}

		private void OnRefreshAfterUserApply(object sender, int type, BaseEventArgs eventArgs)
		{
			this.GetDataFromServerAndRefreshUI(false);
			if (this.RaceContent != null)
			{
				this.RaceContent.ForceLoadData();
			}
		}

		private void HandleAfterSeasonAllOver()
		{
			GuildProxy.UI.CloseGuildRaceAllView();
			string infoByID = GuildProxy.Language.GetInfoByID("400119");
			string infoByID_LogError = GuildProxy.Language.GetInfoByID_LogError(400527);
			GuildProxy.UI.OpenUIPopCommonOnlySure(infoByID, infoByID_LogError, null);
		}

		public CustomButton Button_Return;

		public CustomButton Button_Rank;

		public CustomButton Button_Challenge;

		public UIGray Gray_Challenge;

		public CustomText Text_ChallengeTime;

		public CustomText Text_ChallengeStage;

		public UIGuildRaceInfo RaceInfo;

		public UIGuildRaceContent RaceContent;

		public GameObject UILoading;

		public GuildRaceBattleController RaceCtrl;

		public Animator Animator;

		private long mLastShowTimeSec;

		private GuildRaceStageKind mCurStateKind;
	}
}
