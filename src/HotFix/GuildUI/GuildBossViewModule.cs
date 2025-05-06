using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Guild;
using Server;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class GuildBossViewModule : GuildProxy.GuildProxy_BaseView
	{
		private GuildBossInfo guildBossInfo
		{
			get
			{
				return base.SDK.GuildActivity.GuildBoss;
			}
		}

		protected override void OnViewCreate()
		{
			base.OnViewCreate();
			this.Button_Tip.Init();
			this.m_bossSpine.Init();
			this.Button_Close.m_onClick = new Action(this.CloseThisView);
			this.Button_Task.m_onClick = new Action(this.OnClickTask);
			this.Button_ChallengeRecord.m_onClick = new Action(this.OnClickChallengeRecord);
			this.ButtonChallenge.m_onClick = new Action(this.OnClickBattleFree);
			this.Button_GuildRank.m_onClick = new Action(this.OnClickGuildRank);
			this.Button_SelfRank.m_onClick = new Action(this.OnClickSelfRank);
			this.Button_Box.m_onClick = new Action(this.OnClickShowBoxTips);
		}

		protected override void OnViewOpen(object data)
		{
			base.OnViewOpen(data);
			if (!base.SDK.GuildInfo.HasGuild)
			{
				this.CloseThisView();
			}
			this.mIsReqBattle = false;
			base.gameObject.SetActiveSafe(true);
			DelayCall.Instance.CallLoop(1000, new DelayCall.CallAction(this.RefreshTime));
			this.OnRefreshView();
			GuildProxy.GameEvent.AddReceiver(LocalMessageName.CC_GuildBoss_Refresh, new HandlerEvent(this.OnRefresh));
			base.SDK.Event.RegisterEvent(202, new GuildHandlerEvent(this.OnRefresh));
			RedPointController.Instance.RegRecordChange("Guild.Boss.Challenge", new Action<RedNodeListenData>(this.OnRefreshGuildBossRedPoint));
			RedPointController.Instance.RegRecordChange("Guild.Boss.Task", new Action<RedNodeListenData>(this.OnRefreshGuildBossTaskRedPoint));
			RedPointController.Instance.RegRecordChange("Guild.Boss.BoxReward", new Action<RedNodeListenData>(this.OnRefreshGuildBossBoxRewardRedPoint));
		}

		protected override void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnViewUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnViewClose()
		{
			base.OnViewClose();
			this.m_seqPool.Clear(false);
			base.gameObject.SetActiveSafe(false);
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.RefreshTime));
			GuildProxy.GameEvent.RemoveReceiver(LocalMessageName.CC_GuildBoss_Refresh, new HandlerEvent(this.OnRefresh));
			base.SDK.Event.UnRegisterEvent(202, new GuildHandlerEvent(this.OnRefresh));
			RedPointController.Instance.UnRegRecordChange("Guild.Boss.Challenge", new Action<RedNodeListenData>(this.OnRefreshGuildBossRedPoint));
			RedPointController.Instance.UnRegRecordChange("Guild.Boss.Task", new Action<RedNodeListenData>(this.OnRefreshGuildBossTaskRedPoint));
			RedPointController.Instance.UnRegRecordChange("Guild.Boss.BoxReward", new Action<RedNodeListenData>(this.OnRefreshGuildBossBoxRewardRedPoint));
		}

		protected override void OnViewDelete()
		{
			base.OnViewDelete();
			this.Button_Tip.DeInit();
			this.m_bossSpine.DeInit();
			this.m_seqPool.Clear(false);
			this.Button_Close.m_onClick = null;
			this.Button_Task.m_onClick = null;
			this.Button_ChallengeRecord.m_onClick = null;
			this.ButtonChallenge.m_onClick = null;
			this.Button_GuildRank.m_onClick = null;
			this.Button_SelfRank.m_onClick = null;
			this.Button_Box.m_onClick = null;
		}

		private void OnClickUpDan()
		{
			GuildProxy.UI.OpenUIGuildBossUpDan(new GuildBossUpDanViewModule.OpenData
			{
				GuildDan = this.guildBossInfo.GuildDan,
				GuildSeason = this.guildBossInfo.GuildSeason,
				Rank = this.guildBossInfo.PersonRank,
				LastRank = this.guildBossInfo.LastPersonRank,
				FlyTarget = this.Ctrl_GuildDan.transform.position,
				ScaleTarget = this.Ctrl_GuildDan.GetComponent<RectTransform>().sizeDelta
			});
		}

		private List<ItemData> GetDamageBoxReward()
		{
			List<ItemData> list = new List<ItemData>();
			if (this.guildBossInfo == null)
			{
				return list;
			}
			if (this.guildBossInfo.BossData == null)
			{
				return list;
			}
			GuildBOSS_guildBossStep guildBossStepTable = GuildProxy.Table.GetGuildBossStepTable(this.guildBossInfo.BossData.BossStep);
			if (guildBossStepTable != null)
			{
				list.AddRange(guildBossStepTable.KillReward.ToItemDataList());
			}
			return list;
		}

		private void OnClickShowBoxTips()
		{
			if (this.guildBossInfo == null)
			{
				return;
			}
			if (this.guildBossInfo.KillRewardList.Count > 0)
			{
				GuildNetUtil.Guild.DoRequest_GetGuildBossBoxKillReward(delegate(bool result, GuildBossKilledRewardResponse resp)
				{
					if (resp != null && resp.CommonData != null)
					{
						this.OnRefreshGuildBossBoxKillReward();
						GuildProxy.UI.OpenUICommonReward(resp.CommonData.Reward, null);
					}
				});
				return;
			}
			List<ItemData> damageBoxReward = this.GetDamageBoxReward();
			int num = (damageBoxReward.Count - 1) * 50;
			UIBoxInfoViewModule.Transfer transfer = new UIBoxInfoViewModule.Transfer
			{
				nodeType = UIBoxInfoViewModule.UIBoxInfoNodeType.Up,
				rewards = damageBoxReward,
				position = this.Button_Box.transform.position,
				anchoredPositionOffset = new Vector3((float)(-(float)num), 20f, 0f),
				secondLayer = true,
				IsSetArrowPos = true
			};
			GameApp.View.OpenView(ViewName.RewardBoxInfoViewModule, transfer, 1, null, null);
		}

		private void OnClickTask()
		{
			GuildProxy.UI.OpenUIGuildBossTask(null);
		}

		private void CloseThisView()
		{
			GuildProxy.UI.CloseUIGuildBoss();
		}

		private void OnClickSelfRank()
		{
			GuildProxy.UI.OpenUIGuildBossGuildRank(new GuildBossRankViewModule.GuildBossRankData
			{
				ViewType = RankViewType.Order
			});
		}

		private void OnClickGuildRank()
		{
			Guild_guildConst guildConstTable = GuildProxy.Table.GetGuildConstTable(140);
			if (guildConstTable == null)
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID1("guild_boss_10", guildConstTable.TypeInt));
				return;
			}
			if (this.m_bossData.BossStep <= guildConstTable.TypeInt)
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID1("guild_boss_10", guildConstTable.TypeInt));
				return;
			}
			GuildProxy.UI.OpenUIGuildBossGuildRank(new GuildBossRankViewModule.GuildBossRankData
			{
				ViewType = RankViewType.Guild
			});
		}

		private void OnClickBattleFree()
		{
			this.OnClickBattle(GuildBossBuyKind.Free);
		}

		private void OnClickBattle(GuildBossBuyKind buykind = GuildBossBuyKind.Free)
		{
			if (this.mIsReqBattle)
			{
				return;
			}
			this.mIsReqBattle = true;
			GuildNetUtil.Guild.DoRequest_GuildBossStartBattle(delegate(bool result, GuildBossStartBattleResponse resp)
			{
				if (result)
				{
					this.mIsReqBattle = false;
					this.DoBattle(null);
				}
				else
				{
					this.mIsReqBattle = false;
				}
				if (resp != null && resp.Code == 300037)
				{
					GuildProxy.UI.ShowTips(Singleton<LanguageManager>.Instance.GetInfoByID("300037"));
					return;
				}
				if (resp != null && resp.Code == 300038)
				{
					DxxTools.UI.OpenPopCommon(GuildProxy.Language.GetInfoByID("guild_boss_out"), delegate(int id)
					{
						GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 2, null, delegate(GameObject obj)
						{
							GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayShow(delegate
							{
								GameApp.View.CloseView(ViewName.BattleGuildBossViewModule, null);
								GameApp.View.CloseView(ViewName.SelectSkillViewModule, null);
								GameApp.View.CloseView(ViewName.BattleGuildBossFinishViewModule, null);
								GameApp.State.ActiveState(StateName.MainState);
							});
						});
					}, string.Empty, GuildProxy.Language.GetInfoByID("Global_Confirm"), string.Empty, false, 2);
				}
			});
		}

		private void OnClickChallengeRecord()
		{
			GuildProxy.UI.OpenUIGuildBossChallengeRecord(null);
		}

		private void OnRefreshView()
		{
			if (this.guildBossInfo == null)
			{
				return;
			}
			this.m_bossData = this.guildBossInfo.BossData;
			if (this.m_bossData == null)
			{
				return;
			}
			GuildBOSS_guildBossStep guildBossStepTable = GuildProxy.Table.GetGuildBossStepTable(this.m_bossData.BossStep);
			if (guildBossStepTable == null)
			{
				return;
			}
			Quality_guildBossQuality guildBossStepQualityTable = GuildProxy.Table.GetGuildBossStepQualityTable(this.m_bossData.BossStep);
			if (guildBossStepQualityTable != null)
			{
				this.Text_Grade.text = HLog.StringBuilder("<color=", guildBossStepQualityTable.gradeColor, ">", GuildProxy.Language.GetInfoByID(guildBossStepTable.BossStepName), "</color>");
				GuildProxy.Resources.SetDxxImage(this.Image_Grade, guildBossStepQualityTable.atlasId, guildBossStepQualityTable.guildBossGradeBgName);
			}
			this.Ctrl_GuildDan.SetData(this.guildBossInfo.GuildDan);
			this.Text_JoinNum.text = this.guildBossInfo.DayPartCount.ToString();
			List<CardData> guildBossEnemyCardDatas = GuildController.GetGuildBossEnemyCardDatas(GuildProxy.Table.TableMgr, this.m_bossData.BossStep, -1L);
			long num = 0L;
			if (guildBossEnemyCardDatas.Count > 0)
			{
				num = guildBossEnemyCardDatas[0].m_memberAttributeData.GetHpMax().GetValue();
			}
			long num2 = 0L;
			if (this.m_bossData.CurHP == -1L)
			{
				if (guildBossEnemyCardDatas.Count > 0)
				{
					num2 = num;
				}
			}
			else
			{
				num2 = this.m_bossData.CurHP;
			}
			this.Slider_Hp.maxValue = 1f;
			double num3 = (double)num2 / (double)num;
			this.Slider_Hp.value = (float)num3;
			int num4 = Utility.Math.FloorToInt((float)num3 * 100f);
			this.Text_Hp.text = HLog.StringBuilder(DxxTools.FormatNumber(num2), "/", DxxTools.FormatNumber(num), "(", num4.ToString(), "%)");
			this.Image_SelfRankLock.SetActiveSafe(false);
			this.Image_GuildRankLock.SetActiveSafe(true);
			this.Obj_SeasonTime.SetActiveSafe(false);
			Guild_guildConst guildConstTable = GuildProxy.Table.GetGuildConstTable(140);
			if (guildConstTable != null && this.m_bossData.BossStep > guildConstTable.TypeInt)
			{
				this.Obj_SeasonTime.SetActiveSafe(true);
				this.Image_GuildRankLock.SetActiveSafe(false);
			}
			this.Text_GuildRank.text = ((this.guildBossInfo.GuildRank <= 0) ? GuildProxy.Language.GetInfoByID("uitower_norank") : this.guildBossInfo.GuildRank.ToString());
			this.Text_SelfRank.text = ((this.guildBossInfo.PersonRank <= 0) ? GuildProxy.Language.GetInfoByID("uitower_norank") : this.guildBossInfo.PersonRank.ToString());
			string text;
			if (guildBossStepTable.BossBgType == 1)
			{
				text = "#FFEAB8";
				GuildProxy.Resources.SetDxxImage(this.Image_Line, 129, "img_guildBoss_rankLine01");
				GuildProxy.Resources.SetDxxImage(this.Image_LineBg, 129, "img_guildBoss_rank01");
				GuildProxy.Resources.SetDxxSprite(this.Image_Bg, this.ImageRegister.GetSprite("1"));
			}
			else
			{
				text = "#C7CCFF";
				GuildProxy.Resources.SetDxxImage(this.Image_Line, 129, "img_guildBoss_rankLine02");
				GuildProxy.Resources.SetDxxImage(this.Image_LineBg, 129, "img_guildBoss_rank02");
				GuildProxy.Resources.SetDxxSprite(this.Image_Bg, this.ImageRegister.GetSprite("2"));
			}
			this.Text_SelfRankTitle.text = HLog.StringBuilder("<color=", text, ">", GuildProxy.Language.GetInfoByID("guild_boss_8"), "</color>");
			this.Text_GuildRankTitle.text = HLog.StringBuilder("<color=", text, ">", GuildProxy.Language.GetInfoByID("400228"), "</color>");
			this.RefreshTime();
			this.RefreshBattleCount();
			this.RefreshBossModel();
			this.OnCheckGuildBossDan();
			this.OnRefreshGuildBossBoxKillReward();
		}

		private void OnRefreshGuildBossBoxKillReward()
		{
			if (this.guildBossInfo.KillRewardList.Count > 0)
			{
				this.Text_BoxNum.gameObject.SetActiveSafe(true);
				this.Text_BoxNum.text = HLog.StringBuilder("x", this.guildBossInfo.KillRewardList.Count.ToString());
				this.Button_BoxAni.SetBool("Idle", false);
				return;
			}
			this.Text_BoxNum.gameObject.SetActiveSafe(false);
			this.Button_BoxAni.SetBool("Idle", true);
		}

		private void OnCheckGuildBossDan()
		{
			if (this.guildBossInfo == null)
			{
				return;
			}
			long userId = GameApp.Data.GetDataModule(DataName.LoginDataModule).userId;
			string guildBossSeason = PlayerPrefsKeys.GetGuildBossSeason();
			string guildBossDan = PlayerPrefsKeys.GetGuildBossDan();
			long num = 0L;
			if (string.IsNullOrEmpty(guildBossSeason))
			{
				PlayerPrefsKeys.SetGuildBossSeason(string.Format("{0}_{1}", userId, this.guildBossInfo.GuildSeason.ToString()));
				PlayerPrefsKeys.SetGuildBossDan(string.Format("{0}_{1}", userId, this.guildBossInfo.GuildDan.ToString()));
				return;
			}
			bool flag = false;
			string[] array = guildBossSeason.Split('|', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				if (long.Parse(array[i].Split('_', StringSplitOptions.None)[0]) == userId)
				{
					flag = true;
					num = long.Parse(array[i].Split('_', StringSplitOptions.None)[1]);
					break;
				}
			}
			if (!flag)
			{
				string text = string.Format("|{0}_{1}", userId, this.guildBossInfo.GuildSeason);
				string text2 = string.Format("|{0}_{1}", userId, this.guildBossInfo.GuildDan);
				PlayerPrefsKeys.SetGuildBossSeason(HLog.StringBuilder(guildBossSeason, text));
				PlayerPrefsKeys.SetGuildBossDan(HLog.StringBuilder(guildBossDan, text2));
				return;
			}
			if ((long)this.guildBossInfo.GuildSeason > num)
			{
				GuildProxy.UI.OpenUIGuildBossUpDan(new GuildBossUpDanViewModule.OpenData
				{
					GuildDan = this.guildBossInfo.GuildDan,
					GuildSeason = this.guildBossInfo.GuildSeason,
					Rank = this.guildBossInfo.GuildRank,
					LastRank = this.guildBossInfo.LastGuildRank,
					FlyTarget = this.Ctrl_GuildDan.transform.position,
					ScaleTarget = this.Ctrl_GuildDan.GetComponent<RectTransform>().sizeDelta
				});
			}
		}

		private void RefreshBattleCount()
		{
			GuildBossInfo guildBoss = base.SDK.GuildActivity.GuildBoss;
			if (guildBoss.ChallengeCount > 0)
			{
				this.ButtonChallenge.GetComponent<UIGrays>().Recovery();
				this.ButtonChallenge.enabled = true;
				this.Text_Recovery.gameObject.SetActiveSafe(false);
				this.challengeLeftCount.gameObject.SetActiveSafe(true);
				this.challengeLeftCount.text = Singleton<LanguageManager>.Instance.GetInfoByID("guild_boss_challengeCount", new object[] { guildBoss.ChallengeCount });
				return;
			}
			this.ButtonChallenge.GetComponent<UIGrays>().SetUIGray();
			this.ButtonChallenge.enabled = false;
			this.Text_Recovery.gameObject.SetActiveSafe(true);
			this.challengeLeftCount.gameObject.SetActiveSafe(false);
		}

		private void RefreshTime()
		{
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			long num = this.guildBossInfo.ServerSeasonEndTime - serverTimestamp;
			if (num <= 0L)
			{
				num = 0L;
			}
			this.Text_SeasonTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("400185", new object[] { DxxTools.FormatFullTimeWithDay(num) });
			if (this.guildBossInfo.ChallengeCount >= this.guildBossInfo.MaxRecoveryCount)
			{
				this.Text_Recovery.text = "";
				return;
			}
			long num2 = this.guildBossInfo.NextChallengeRecoverTime - GuildProxy.Net.ServerTime();
			num2 = ((num2 > 0L) ? num2 : 0L);
			if (num2 > 0L)
			{
				string time = Singleton<LanguageManager>.Instance.GetTime(num2);
				this.Text_Recovery.text = GuildProxy.Language.GetInfoByID1("WorldBoss_ChallengeTimes_FreshTime", time);
				return;
			}
			this.Text_Recovery.text = "";
		}

		private void OnRefreshGuildBossBoxRewardRedPoint(RedNodeListenData obj)
		{
			this.RedCtrl_GuildBossBoxReward.Value = obj.m_count;
		}

		private void OnRefreshGuildBossTaskRedPoint(RedNodeListenData obj)
		{
			this.RedCtrl_GuildBossTask.Value = obj.m_count;
		}

		private void OnRefreshGuildBossRedPoint(RedNodeListenData obj)
		{
			this.RedCtrl_GuildBoss.Value = obj.m_count;
		}

		private void OnRefresh(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnRefreshView();
		}

		private void OnRefresh(int type, GuildBaseEvent eventArgs)
		{
			this.OnRefreshView();
		}

		private void DoBattle(Action beginGame)
		{
			Action <>9__1;
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				LoadingViewModule viewModule = GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule);
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate
					{
						EventArgsRefreshMainOpenData instance = Singleton<EventArgsRefreshMainOpenData>.Instance;
						instance.SetData(DxxTools.UI.GetGuildBossOpenData());
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance);
						Action beginGame2 = beginGame;
						if (beginGame2 != null)
						{
							beginGame2();
						}
						EventArgsGameDataEnter instance2 = Singleton<EventArgsGameDataEnter>.Instance;
						instance2.SetData(GameModel.GuildBoss, null);
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance2);
						GameApp.State.ActiveState(StateName.BattleGuildBossState);
					});
				}
				viewModule.PlayShow(action);
			});
		}

		private async void RefreshBossModel()
		{
			this.m_bossSpine.gameObject.SetActiveSafe(false);
			this.TextBossName.text = "";
			if (this.guildBossInfo != null)
			{
				GuildBOSS_guildBoss currentBossShowModel = this.guildBossInfo.GetCurrentBossShowModel();
				if (currentBossShowModel != null)
				{
					if (this.m_guildBossMemberId != currentBossShowModel.BossId)
					{
						this.TextBossName.SetText(currentBossShowModel.NameID);
						if (GameApp.Table.GetManager().GetGameMember_member(currentBossShowModel.BossId) != null)
						{
							this.m_bossSpine.gameObject.SetActiveSafe(true);
							this.m_bossSpine.ShowMemberModel(currentBossShowModel.BossId, "Idle", true);
							this.m_bossSpine.SetScale(currentBossShowModel.uiScale);
						}
					}
				}
			}
		}

		public UIHelpButton Button_Tip;

		public CustomButton Button_Close;

		public CustomButton Button_Task;

		public CustomText challengeLeftCount;

		[Header("Boss基础信息")]
		public CustomText TextBossName;

		public Slider Slider_Hp;

		public CustomText Text_Hp;

		public GameObject Obj_SeasonTime;

		public CustomText Text_SeasonTime;

		public CustomText Text_JoinNum;

		[Header("段位")]
		public UIGuildDan Ctrl_GuildDan;

		public CustomImage Image_Grade;

		public CustomText Text_Grade;

		[Header("BOSS模型")]
		public UISpineModelItem m_bossSpine;

		private int m_guildBossMemberId;

		[Header("挑战次数")]
		public CustomButton ButtonChallenge;

		public CustomText Text_Recovery;

		[Header("排行")]
		public CustomButton Button_GuildRank;

		public GameObject Image_GuildRankLock;

		public CustomText Text_GuildRank;

		public CustomText Text_GuildRankTitle;

		public CustomButton Button_SelfRank;

		public GameObject Image_SelfRankLock;

		public CustomText Text_SelfRank;

		public CustomText Text_SelfRankTitle;

		[Header("挑战记录")]
		public CustomButton Button_ChallengeRecord;

		[Header("背景相关替换")]
		public SpriteRegister ImageRegister;

		public CustomImage Image_Line;

		public CustomImage Image_LineBg;

		public CustomImage Image_Bg;

		[Header("宝箱奖励")]
		public CustomButton Button_Box;

		public Animator Button_BoxAni;

		public CustomText Text_BoxNum;

		private bool mIsReqBattle;

		[Header("红点")]
		public RedNodeOneCtrl RedCtrl_GuildBoss;

		public RedNodeOneCtrl RedCtrl_GuildBossTask;

		public RedNodeOneCtrl RedCtrl_GuildBossBoxReward;

		public Animator Ani_Show;

		private GuildBossData m_bossData;

		private SequencePool m_seqPool = new SequencePool();
	}
}
