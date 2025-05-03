using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using Dxx.Guild;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Chapter;
using Proto.Common;
using Proto.NewWorld;
using Proto.Tower;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMainCity : UIBaseMainPageNode
	{
		protected override void OnInit()
		{
			this.chapterDataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			this.talentDataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
			this.newWorldDataModule = GameApp.Data.GetDataModule(DataName.NewWorldDataModule);
			this.shopDataModule = GameApp.Data.GetDataModule(DataName.ShopDataModule);
			this.buttonSweep.onClick.AddListener(new UnityAction(this.OnClickSweep));
			this.buttonBattle.onClick.AddListener(new UnityAction(this.OnClickBattle));
			this.buttonGuild.onClick.AddListener(new UnityAction(this.OnClickGuild));
			this.buttonDungeon.onClick.AddListener(new UnityAction(this.OnClickDungeon));
			this.buttonChapterReward.onClick.AddListener(new UnityAction(this.OnClickChapterReward));
			this.buttonBattlePass.onClick.AddListener(new UnityAction(this.OnClickBattlePass));
			this.buttonNewWorld.onClick.AddListener(new UnityAction(this.OnClickNewWorld));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_MainCity_Refresh, new HandlerEvent(this.OnEventRefresh));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterActivity_CheckShow, new HandlerEvent(this.OnEventCheckActivity));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterActivity_CloseCheckMain, new HandlerEvent(this.OnEventCloseCheckMain));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Framework_CloseView, new HandlerEvent(this.OnEventCloseView));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Travel_Close_View, new HandlerEvent(this.OnEventCloseTravelView));
			this.chapterBattlePassCtrl.SetChapterId(this.chapterDataModule.ChapterID, true);
			this.chapterBattlePassCtrl.Init();
			RedPointController.Instance.RegRecordChange("Main.ChapterReward", new Action<RedNodeListenData>(this.OnRedPointChapterRewardChange));
			RedPointController.Instance.RegRecordChange("DailyActivity", new Action<RedNodeListenData>(this.OnRedPointDailyActivityChange));
			RedPointController.Instance.RegRecordChange("Main.HangUp", new Action<RedNodeListenData>(this.OnRedPointHangUpChange));
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("Sweep", this.buttonSweep.transform);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("Guild", this.buttonGuild.transform);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("Dungeon", this.buttonDungeon.transform);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("HangUp", this.buttonSweep.transform);
			GuideController.Instance.DelTarget("ButtonDungeon");
			GuideController.Instance.AddTarget("ButtonDungeon", this.buttonDungeon.transform);
			GuideController.Instance.DelTarget("ButtonSweep");
			GuideController.Instance.AddTarget("ButtonSweep", this.buttonSweep.transform);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.chapterBattlePassCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnDeInit()
		{
			this.buttonSweep.onClick.RemoveListener(new UnityAction(this.OnClickSweep));
			this.buttonBattle.onClick.RemoveListener(new UnityAction(this.OnClickBattle));
			this.buttonGuild.onClick.RemoveListener(new UnityAction(this.OnClickGuild));
			this.buttonDungeon.onClick.RemoveListener(new UnityAction(this.OnClickDungeon));
			this.buttonChapterReward.onClick.RemoveListener(new UnityAction(this.OnClickChapterReward));
			this.buttonBattlePass.onClick.RemoveListener(new UnityAction(this.OnClickBattlePass));
			this.buttonNewWorld.onClick.RemoveListener(new UnityAction(this.OnClickNewWorld));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_MainCity_Refresh, new HandlerEvent(this.OnEventRefresh));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterActivity_CheckShow, new HandlerEvent(this.OnEventCheckActivity));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterActivity_CloseCheckMain, new HandlerEvent(this.OnEventCloseCheckMain));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Framework_CloseView, new HandlerEvent(this.OnEventCloseView));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Travel_Close_View, new HandlerEvent(this.OnEventCloseTravelView));
			this.chapterBattlePassCtrl.DeInit();
			RedPointController.Instance.UnRegRecordChange("Main.ChapterReward", new Action<RedNodeListenData>(this.OnRedPointChapterRewardChange));
			RedPointController.Instance.UnRegRecordChange("DailyActivity", new Action<RedNodeListenData>(this.OnRedPointDailyActivityChange));
			RedPointController.Instance.UnRegRecordChange("Main.HangUp", new Action<RedNodeListenData>(this.OnRedPointHangUpChange));
		}

		protected override void OnShow(UIBaseMainPageNode.OpenData openData)
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCurrency, Singleton<EventArgsBool>.Instance.SetData(true));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCorner, Singleton<EventArgsBool>.Instance.SetData(true));
			this.Refresh();
			GameApp.Event.DispatchNow(this, 102, null);
			this.OnCheckPopPushGift();
			TalentLegacyDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			if (dataModule != null && DxxTools.Time.ServerTimestamp - dataModule.RankSaveTimeStamp >= dataModule.RankSaveTimeStampDelay)
			{
				NetworkUtils.DoRankRequest(RankType.NewWorld, 1, false, false, null);
			}
			NetworkUtils.TalentLegacy.DoTalentLegacyInfoRequest(null, false);
		}

		private void Refresh()
		{
			this.textChapter.text = string.Format("{0}.{1}", this.chapterDataModule.CurrentChapter.id, this.chapterDataModule.CurrentChapter.Name);
			this.textMaxStage.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIMainCity_Record", new object[] { this.chapterDataModule.MaxStage });
			ChapterRewardData showReward = this.chapterDataModule.GetShowReward();
			if (showReward != null)
			{
				this.textChapterReward.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIChapter_Chapter", new object[] { showReward.chapterId });
				if (showReward.IsNeedPass)
				{
					this.textRewardStage.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIChapter_PassText");
				}
				else
				{
					this.textRewardStage.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIChapter_Survived", new object[] { showReward.stage });
				}
			}
			else
			{
				this.textChapter.text = "";
				this.textRewardStage.text = "";
			}
			bool flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Sweep, false);
			bool flag2 = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainUI_Guild, false);
			bool flag3 = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainUI_DailyActivities, false);
			this.buttonGuild.gameObject.SetActiveSafe(flag2);
			this.buttonSweep.gameObject.SetActiveSafe(flag);
			this.buttonDungeon.gameObject.SetActiveSafe(flag3);
			bool flag4 = this.newWorldDataModule.IsGoNewWorldEnabled();
			this.buttonNewWorld.gameObject.SetActiveSafe(flag4);
			this.buttonBattle.gameObject.SetActiveSafe(!flag4);
			this.UpdateBattleEnergyCost();
			bool flag5 = this.IsChapterUnlock();
			this.unlockObj.SetActiveSafe(flag5);
			this.lockObj.SetActiveSafe(!flag5);
			if (flag5)
			{
				this.btnGrays.Recovery();
				return;
			}
			this.btnGrays.SetUIGray();
			this.textLockType.text = this.GetLockTypeText();
			this.textLockValue.text = this.GetLockValueText();
		}

		private void UpdateBattleEnergyCost()
		{
			ulong newNum = (ulong)GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.UserLife).NewNum;
			this.textEnergy.text = string.Format("x{0}", this.chapterDataModule.CurrentChapter.CostEnergy);
			if (newNum >= (ulong)((long)this.chapterDataModule.CurrentChapter.CostEnergy))
			{
				this.textEnergy.color = Color.white;
				return;
			}
			this.textEnergy.color = Color.red;
		}

		private bool CheckShowActivity(ChapterActivityKind kind, bool showUI = true)
		{
			if (Singleton<GameEventController>.Instance.ActiveStateName != GameEventStateName.Idle)
			{
				return false;
			}
			ChapterActivityDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule);
			if (kind == ChapterActivityKind.BattlePass || kind == ChapterActivityKind.All)
			{
				this.chapterBattlePassCtrl.SetData(false);
			}
			if (kind == ChapterActivityKind.Rank || kind == ChapterActivityKind.All)
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterRank_Hide, null);
				if (showUI)
				{
					ChapterActivityRewardData chapterActivityRewardData = dataModule.CheckRewardActivity();
					if (chapterActivityRewardData != null && chapterActivityRewardData.Kind == ChapterActivityKind.Rank)
					{
						ViewModuleData viewModuleData = GameApp.View.GetViewModuleData(974);
						if (viewModuleData != null && viewModuleData.m_viewState != 1 && viewModuleData.m_viewState != 2)
						{
							GameApp.View.OpenView(ViewName.ChapterActivityRankViewModule, null, 1, null, null);
						}
						return true;
					}
				}
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterRank_Show, null);
			}
			return false;
		}

		private void CheckBattleRecord()
		{
			RogueDungeonDataModule dataModule = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule);
			if (Singleton<EventRecordController>.Instance.IsHaveChapterRecord())
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("Battle_Record_Tip");
				string popCommonTitle = DxxTools.UI.GetPopCommonTitle();
				string popCommonSure = DxxTools.UI.GetPopCommonSure();
				string popCommonCancel = DxxTools.UI.GetPopCommonCancel();
				DxxTools.UI.OpenPopCommon(infoByID, delegate(int result)
				{
					if (result == 1)
					{
						this.EnterBattle();
						return;
					}
					if (result == -1)
					{
						Singleton<EventRecordController>.Instance.CreateChapterRecord();
						EventRecordPlayerData playerRecord = Singleton<EventRecordController>.Instance.PlayerRecord;
						if (playerRecord != null)
						{
							int recordStage = Singleton<EventRecordController>.Instance.GetRecordStage(GameEventStateName.Chapter);
							List<RewardDto> eventDropRecord = Singleton<EventRecordController>.Instance.GetEventDropRecord(GameEventStateName.Chapter);
							List<RewardDto> battleDropRecord = Singleton<EventRecordController>.Instance.GetBattleDropRecord(GameEventStateName.Chapter);
							List<int> skillBuildIdList = playerRecord.GetSkillBuildIdList();
							GameTGATools.Ins.ChapterEndQuitType = 2;
							NetworkUtils.Chapter.DoEndChapterRequest(this.chapterDataModule.CurrentChapter.id, recordStage, 1, "", this.chapterDataModule.ChapterBattleKey, eventDropRecord, battleDropRecord, skillBuildIdList, delegate(bool success, EndChapterResponse resp)
							{
								if (success && resp != null)
								{
									this.chapterDataModule.RefreshChapterData(resp.ChapterId, resp.WaveIndex, resp.CanRewardList, resp.BattleTimes);
									if (resp.CommonData.Reward.Count > 0)
									{
										DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, new Action(this.CheckNext), true);
										return;
									}
									this.CheckNext();
								}
							});
							return;
						}
						Singleton<EventRecordController>.Instance.DeleteChapterRecord();
						this.CheckNext();
					}
				}, popCommonTitle, popCommonSure, popCommonCancel, false, 2);
				return;
			}
			if (Singleton<EventRecordController>.Instance.IsHaveSweepRecord())
			{
				string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("Sweep_Record_Tip");
				string popCommonTitle2 = DxxTools.UI.GetPopCommonTitle();
				string popCommonSure2 = DxxTools.UI.GetPopCommonSure();
				string popCommonCancel2 = DxxTools.UI.GetPopCommonCancel();
				DxxTools.UI.OpenPopCommon(infoByID2, delegate(int result)
				{
					if (result == 1)
					{
						Singleton<EventRecordController>.Instance.CreateSweepRecord();
						Singleton<GameEventController>.Instance.EnterEventMode(GameEventStateName.Sweep);
						ChapterSweepViewModule.OpenData openData = new ChapterSweepViewModule.OpenData();
						int previousChapterID = this.chapterDataModule.GetPreviousChapterID();
						openData.chapterId = previousChapterID;
						openData.isRecord = true;
						GameApp.View.OpenView(ViewName.ChapterSweepViewModule, openData, 1, null, null);
						return;
					}
					if (result == -1)
					{
						Singleton<EventRecordController>.Instance.CreateSweepRecord();
						SweepRecordData sweepRecord = Singleton<EventRecordController>.Instance.SweepRecord;
						if (sweepRecord != null)
						{
							Singleton<GameEventController>.Instance.SetTempSweep(true);
							int recordStage2 = Singleton<EventRecordController>.Instance.GetRecordStage(GameEventStateName.Sweep);
							List<RewardDto> eventDropRecord2 = Singleton<EventRecordController>.Instance.GetEventDropRecord(GameEventStateName.Sweep);
							List<RewardDto> battleDropRecord2 = Singleton<EventRecordController>.Instance.GetBattleDropRecord(GameEventStateName.Sweep);
							NetworkUtils.Chapter.DoEndChapterSweepRequest(sweepRecord.chapterId, sweepRecord.rate, recordStage2, eventDropRecord2, battleDropRecord2, delegate(bool success, EndChapterSweepResponse response)
							{
								Singleton<GameEventController>.Instance.SetTempSweep(false);
								if (success && response != null)
								{
									if (response.CommonData.Reward.Count > 0)
									{
										DxxTools.UI.OpenRewardCommon(response.CommonData.Reward, new Action(this.CheckNext), true);
										return;
									}
									this.CheckNext();
								}
							});
						}
					}
				}, popCommonTitle2, popCommonSure2, popCommonCancel2, false, 2);
				return;
			}
			if (dataModule.IsBattleSign)
			{
				string infoByID3 = Singleton<LanguageManager>.Instance.GetInfoByID("roguedungon_battlesign_tip");
				string popCommonTitle3 = DxxTools.UI.GetPopCommonTitle();
				string popCommonSure3 = DxxTools.UI.GetPopCommonSure();
				string popCommonCancel3 = DxxTools.UI.GetPopCommonCancel();
				DxxTools.UI.OpenPopCommon(infoByID3, delegate(int result)
				{
					if (result == 1)
					{
						NetworkUtils.RogueDungeon.DoGetPanelInfoRequest(delegate(bool isOk, HellGetPanelInfoResponse response)
						{
							if (isOk)
							{
								this.EnterRogueDungeonBattle();
							}
						});
						return;
					}
					if (result == -1)
					{
						NetworkUtils.RogueDungeon.DoGetPanelInfoRequest(delegate(bool isOk, HellGetPanelInfoResponse response)
						{
							if (isOk)
							{
								NetworkUtils.RogueDungeon.DoHellExitBattleRequest(delegate(bool isOk, HellExitBattleResponse response)
								{
									if (response != null && response.CommonData.Reward != null && response.CommonData.Reward.Count > 0)
									{
										DxxTools.UI.OpenRewardCommon(response.CommonData.Reward, new Action(this.CheckNext), true);
										GameApp.SDK.Analyze.Track_EnterDungeon(3, 0L, 0L, 0L, new List<GameEventSkillBuildData>(), response.CommonData.Reward);
										return;
									}
									this.CheckNext();
								});
							}
						});
					}
				}, popCommonTitle3, popCommonSure3, popCommonCancel3, false, 2);
				return;
			}
			this.CheckNext();
		}

		private void CheckNext()
		{
			bool flag = true;
			if (GameApp.DeepLink != null && !string.IsNullOrEmpty(GameApp.DeepLink.DeepLinkParam))
			{
				flag = !this.CheckDeeplink();
				GameApp.DeepLink.DeepLinkParam = string.Empty;
			}
			if (flag)
			{
				this.CheckFunctionOpen();
			}
		}

		private void OnCheckPopPushGift()
		{
			List<WindowQueueInfo> list = new List<WindowQueueInfo>();
			if (Singleton<GameConfig>.Instance.IsPopNotice)
			{
				string text = "EnableSoftUpdate";
				if (GameApp.SDK.GetCloudDataValue<bool>(text, false))
				{
					PopCommonData popCommonData = DxxTools.UI.GetPopCommonData(Singleton<LanguageManager>.Instance.GetInfoByID("common_download_tip"), delegate(int ret)
					{
						if (ret > 0)
						{
							string @string = GameApp.Config.GetString("ChannelName");
							Application.OpenURL(Singleton<PathManager>.Instance.GetAppUrl(@string));
						}
					}, Singleton<LanguageManager>.Instance.GetInfoByID("43"), Singleton<LanguageManager>.Instance.GetInfoByID("17"), Singleton<LanguageManager>.Instance.GetInfoByID("18"), false);
					list.Add(new WindowQueueInfo(ViewName.PopCommonViewModule, popCommonData, 2));
				}
				Singleton<GameConfig>.Instance.IsPopNotice = false;
				if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Notice, false))
				{
					list.Add(new WindowQueueInfo(ViewName.NoticeViewModule, null, 3));
				}
			}
			PushGiftDataModule dataModule = GameApp.Data.GetDataModule(DataName.PushGiftDataModule);
			if (dataModule._pushGiftDatas.Count > 0)
			{
				foreach (PushGiftData pushGiftData in dataModule._pushGiftDatas)
				{
					if (!pushGiftData.IsPop && DxxTools.Time.ServerTimestamp < pushGiftData.EndTime)
					{
						pushGiftData.IsPop = true;
						list.Add(new WindowQueueInfo(ViewName.PushGiftPopViewModule, pushGiftData, 2));
					}
				}
				dataModule._pushGiftDatas.Clear();
			}
			ChainPacksPushDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.ChainPacksPushDataModule);
			if (dataModule2 != null && dataModule2.IsCheckChainPacksPop())
			{
				dataModule2.OnSetChainPacksPopState();
				list.Add(new WindowQueueInfo(ViewName.ChainPacksViewModule, 1, 1));
			}
			if (this.chapterDataModule.IsShowRateView())
			{
				list.Add(new WindowQueueInfo(ViewName.RateViewModule, 2, 1));
			}
			if (list.Count > 0)
			{
				GameApp.View.ShowWindowToQueue(list, delegate
				{
					this.OpenMainCheck();
				});
				return;
			}
			this.OpenMainCheck();
		}

		private void OpenMainCheck()
		{
			if (this.CheckShowActivity(ChapterActivityKind.All, true))
			{
				return;
			}
			if (Singleton<GameFunctionController>.Instance.CheckNewFunctionOpen() || GuideController.Instance.CurrentGuide != null)
			{
				EventArgsBool eventArgsBool = new EventArgsBool();
				eventArgsBool.SetData(false);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_IAP_BattleFail_ShowFirstRechargeUI, eventArgsBool);
			}
			else
			{
				bool flag = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.FirstTopUp, false);
				IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
				bool flag2 = dataModule.MeetingGift.IsAllEnd();
				IAP_PushPacks unBuyGift = dataModule.MeetingGift.GetUnBuyGift();
				if (dataModule.IsShowFirstRecharge && flag && !flag2 && unBuyGift != null)
				{
					dataModule.SetShowFirstRecharge(false);
					GameApp.View.OpenView(ViewName.MeetingGiftViewModule, null, 1, null, null);
					return;
				}
			}
			this.CheckBattleRecord();
		}

		private void CheckFunctionOpen()
		{
			MainState state = GameApp.State.GetState(StateName.MainState);
			if (state != null)
			{
				state.SetCheckEnabled(true);
			}
			GuideController.Instance.CustomizeStringTrigger("OpenMainUI");
		}

		private bool CheckDeeplink()
		{
			return GameApp.Data.GetDataModule(DataName.DeepLinkDataModule).ParseDeepLinkData();
		}

		protected override void OnHide()
		{
		}

		public override void OnLanguageChange()
		{
			this.Refresh();
			this.chapterBattlePassCtrl.SetData(false);
		}

		private bool IsChapterUnlock()
		{
			int unlockType = this.chapterDataModule.CurrentChapter.Config.unlockType;
			int[] unlock = this.chapterDataModule.CurrentChapter.Config.unlock;
			if (unlockType == 1 && unlock.Length != 0 && this.talentDataModule.talentData.TalentStage < unlock[0])
			{
				return false;
			}
			if (unlockType == 2)
			{
				TalentLegacyDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
				for (int i = 0; i < unlock.Length; i++)
				{
					if (dataModule.IsUnlockTalentLegacyNode(unlock[i]))
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		private string GetLockTypeText()
		{
			int unlockType = this.chapterDataModule.CurrentChapter.Config.unlockType;
			if (unlockType == 1)
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID("chapter_unlock_talent");
			}
			if (unlockType == 2)
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID("chapter_unlock_legacy");
			}
			return "";
		}

		private string GetLockValueText()
		{
			int unlockType = this.chapterDataModule.CurrentChapter.Config.unlockType;
			int[] unlock = this.chapterDataModule.CurrentChapter.Config.unlock;
			if (unlockType == 1)
			{
				int num = unlock[0];
				TalentNew_talentEvolution talentNew_talentEvolution = GameApp.Table.GetManager().GetTalentNew_talentEvolution(num);
				if (talentNew_talentEvolution != null)
				{
					return Singleton<LanguageManager>.Instance.GetInfoByID(talentNew_talentEvolution.stepLanguageId);
				}
			}
			else if (unlockType == 2)
			{
				string legacyTip = this.chapterDataModule.CurrentChapter.Config.legacyTip;
				return Singleton<LanguageManager>.Instance.GetInfoByID(legacyTip);
			}
			return "";
		}

		private string GetLockTip()
		{
			string lockValueText = this.GetLockValueText();
			int unlockType = this.chapterDataModule.CurrentChapter.Config.unlockType;
			if (unlockType == 1)
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID("chapter_unlock_tip1", new object[] { lockValueText });
			}
			if (unlockType == 2)
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID("chapter_unlock_tip2", new object[] { lockValueText });
			}
			return "";
		}

		private void OnTicketUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsTicketUpdate eventArgsTicketUpdate = eventArgs as EventArgsTicketUpdate;
			if (eventArgsTicketUpdate == null)
			{
				return;
			}
			if (eventArgsTicketUpdate.TicketKind == UserTicketKind.UserLife)
			{
				this.UpdateBattleEnergyCost();
			}
		}

		private void OnFunctionOpen(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsFunctionOpen eventArgsFunctionOpen = eventArgs as EventArgsFunctionOpen;
			if (eventArgsFunctionOpen == null)
			{
				return;
			}
			switch (eventArgsFunctionOpen.FunctionID)
			{
			case 23:
				this.buttonSweep.gameObject.SetActive(true);
				break;
			case 24:
				this.buttonGuild.gameObject.SetActive(true);
				break;
			case 25:
				this.buttonDungeon.gameObject.SetActive(true);
				break;
			}
			this.CheckShowActivity(ChapterActivityKind.All, true);
		}

		private void OnEventRefresh(object sender, int type, BaseEventArgs eventArgs)
		{
			this.Refresh();
		}

		private void OnEventCheckActivity(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsInt kind = eventArgs as EventArgsInt;
			if (kind != null)
			{
				this.CheckShowActivity((ChapterActivityKind)kind.Value, true);
				Sequence sequence = DOTween.Sequence();
				TweenSettingsExtensions.AppendInterval(sequence, 3f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.CheckShowActivity((ChapterActivityKind)kind.Value, false);
				});
			}
		}

		private void OnEventCloseCheckMain(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OpenMainCheck();
		}

		private void OnEventCloseView(object sender, int type, BaseEventArgs eventArgs)
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			if (GameApp.View.IsLayerTop(201) || GameApp.View.IsLayerTop(202))
			{
				GuideController.Instance.CustomizeStringTrigger("OpenMainUI");
			}
		}

		private void OnEventCloseTravelView(object sender, int type, BaseEventArgs args)
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			if (this.m_pageName != 3)
			{
				return;
			}
			this.OnCheckPopPushGift();
		}

		private void OnClickBattle()
		{
			if (this.isBattle)
			{
				return;
			}
			if (!this.IsChapterUnlock())
			{
				string lockTip = this.GetLockTip();
				GameApp.View.ShowStringTip(lockTip);
				return;
			}
			GameApp.Data.GetDataModule(DataName.IAPDataModule);
			if ((ulong)GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.UserLife).NewNum < (ulong)((long)this.chapterDataModule.CurrentChapter.CostEnergy))
			{
				GameApp.View.ShowItemNotEnoughTip(UserTicketKind.UserLife.GetHashCode(), true);
				return;
			}
			this.isBattle = true;
			NetworkUtils.Chapter.DoStartChapterRequest(this.chapterDataModule.ChapterID, delegate(bool isOk, StartChapterResponse res)
			{
				if (isOk)
				{
					this.EnterBattle();
					return;
				}
				this.isBattle = false;
			});
		}

		private void OnClickSweep()
		{
			int previousChapterID = this.chapterDataModule.GetPreviousChapterID();
			if (previousChapterID <= 0)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("ui_sweep_tips_lock");
				EventArgsString eventArgsString = new EventArgsString();
				eventArgsString.SetData(infoByID);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddTextTipNode, eventArgsString);
				return;
			}
			ChapterSweepViewModule.OpenData openData = new ChapterSweepViewModule.OpenData();
			openData.chapterId = previousChapterID;
			openData.isRecord = false;
			GameApp.View.OpenView(ViewName.ChapterSweepViewModule, openData, 1, null, null);
		}

		private void OnClickGuild()
		{
			if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainUI_Guild, false))
			{
				GameApp.View.ShowStringTip(Singleton<GameFunctionController>.Instance.GetLockTips(24));
				return;
			}
			GuildSDKManager.Instance.OpenGuild();
		}

		private void OnClickDungeon()
		{
			if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainUI_DailyActivities, false))
			{
				GameApp.View.ShowStringTip(Singleton<GameFunctionController>.Instance.GetLockTips(25));
				return;
			}
			GameApp.View.OpenView(ViewName.DailyActivitiesViewModule, null, 1, null, null);
		}

		private void OnClickChapterReward()
		{
			GameApp.View.OpenView(ViewName.ChapterRewardViewModule, null, 1, null, null);
		}

		private void OnClickBattlePass()
		{
			EventArgsString eventArgsString = new EventArgsString();
			eventArgsString.SetData("功能暂未接入");
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddTextTipNode, eventArgsString);
		}

		private void OnClickChapterRank()
		{
			GameApp.View.OpenView(ViewName.ChapterActivityRankViewModule, null, 1, null, null);
		}

		private void OnClickNewWorld()
		{
			if (this.newWorldDataModule.IsGoNewWorldEnabled())
			{
				NetworkUtils.NewWorld.DoEnterRequest(delegate(bool isOk, EnterResponse resp)
				{
					if (isOk)
					{
						this.buttonNewWorld.gameObject.SetActiveSafe(false);
						this.buttonBattle.gameObject.SetActiveSafe(true);
						MainViewModule viewModule = GameApp.View.GetViewModule(ViewName.MainViewModule);
						if (viewModule != null)
						{
							viewModule.EnterNewWorld(delegate
							{
								if (resp.CommonData.Reward != null && resp.CommonData.Reward.Count > 0)
								{
									DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, new Action(UIMainCity.<OnClickNewWorld>g__OnCloseRewardUI|59_2), true);
									return;
								}
								UIMainCity.<OnClickNewWorld>g__OnCloseRewardUI|59_2();
							});
						}
					}
				});
			}
		}

		private void OnRedPointChapterRewardChange(RedNodeListenData redData)
		{
			if (this.redNodeChapterReward == null)
			{
				return;
			}
			this.redNodeChapterReward.gameObject.SetActive(redData.m_count > 0);
		}

		private void OnRedPointDailyActivityChange(RedNodeListenData redData)
		{
			if (this.redNodeDailyActivity == null)
			{
				return;
			}
			this.redNodeDailyActivity.gameObject.SetActive(redData.m_count > 0);
		}

		private void OnRedPointHangUpChange(RedNodeListenData redData)
		{
			if (this.redNodeSweep == null)
			{
				return;
			}
			this.redNodeSweep.gameObject.SetActive(redData.m_count > 0);
		}

		private void EnterBattle()
		{
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayShow(delegate
				{
					EventArgsGameDataEnter instance = Singleton<EventArgsGameDataEnter>.Instance;
					instance.SetData(GameModel.Chapter, null);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance);
					GameApp.State.ActiveState(StateName.BattleChapterState);
					this.isBattle = false;
				});
			});
		}

		private void EnterRogueDungeonBattle()
		{
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayShow(delegate
				{
					EventArgsRefreshMainOpenData instance = Singleton<EventArgsRefreshMainOpenData>.Instance;
					instance.SetData(DxxTools.UI.GetRogueDungeonOpenData());
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance);
					EventArgsGameDataEnter instance2 = Singleton<EventArgsGameDataEnter>.Instance;
					instance2.SetData(GameModel.RogueDungeon, null);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance2);
					GameApp.State.ActiveState(StateName.BattleRogueDungeonState);
				});
			});
		}

		[CompilerGenerated]
		internal static void <OnClickNewWorld>g__OnCloseRewardUI|59_2()
		{
		}

		[Header("顶部章节信息")]
		public CustomText textChapter;

		public CustomText textMaxStage;

		[Header("战令")]
		public CustomButton buttonBattlePass;

		[Header("主线活动")]
		public ChapterBattlePassCtrl chapterBattlePassCtrl;

		[Header("底部按钮")]
		public CustomButton buttonSweep;

		public CustomButton buttonBattle;

		public CustomButton buttonGuild;

		public CustomButton buttonDungeon;

		public CustomButton buttonChapterReward;

		public CustomButton buttonNewWorld;

		[Header("底部信息")]
		public CustomText textChapterReward;

		public CustomText textRewardStage;

		public CustomText textEnergy;

		public UIGrays btnGrays;

		public GameObject unlockObj;

		public GameObject lockObj;

		public CustomText textLockType;

		public CustomText textLockValue;

		[Header("红点")]
		public RedNodeOneCtrl redNodeChapterReward;

		public RedNodeOneCtrl redNodeDailyActivity;

		public RedNodeOneCtrl redNodeSweep;

		private bool isBattle;

		private ChapterDataModule chapterDataModule;

		private TalentDataModule talentDataModule;

		private NewWorldDataModule newWorldDataModule;

		private ShopDataModule shopDataModule;
	}
}
