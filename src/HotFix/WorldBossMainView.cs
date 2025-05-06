using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Mission;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class WorldBossMainView : BaseViewModule
	{
		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_WorldBoss_Update, new HandlerEvent(this.OnBossDataUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_InfoCommonView_Close, new HandlerEvent(this.OnInfoViewClose));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_WorldBoss_Update, new HandlerEvent(this.OnBossDataUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_InfoCommonView_Close, new HandlerEvent(this.OnInfoViewClose));
		}

		public override void OnCreate(object data)
		{
			this.boxButton.m_onClick = new Action(this.OnBoxButtonClick);
			this.rankButton.m_onClick = new Action(this.OnRankButtonClick);
			this.closeButton.m_onClick = new Action(this.OnCloseButtonClick);
			this.startButton.m_onClick = new Action(this.OnStartButtonClick);
			this.historyButton.m_onClick = new Action(this.OnHistoryButtonClick);
			this.infoButton.m_onClick = new Action(this.OnShowInfoPanel);
			this._data = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
			this.InitRender();
			this._seqPool = new SequencePool();
		}

		public override void OnDelete()
		{
			this.boxButton.m_onClick = null;
			this.rankButton.m_onClick = null;
			this.closeButton.m_onClick = null;
			this.startButton.m_onClick = null;
			this.historyButton.m_onClick = null;
			this.infoButton.m_onClick = null;
			this.DeInitRender();
			this._data = null;
			this._seqPool.Clear(false);
			this._seqPool = null;
		}

		private void UpdateTimeCount()
		{
			this.UpdateSeasonTime();
			this.UpdateRoundTime();
			this.UpdateStartBtn();
		}

		private void UpdateSeasonTime()
		{
			long seasonRemainTime = this._data.GetSeasonRemainTime();
			this.SetSeasonRefreshTime(seasonRemainTime);
		}

		private void UpdateRoundTime()
		{
			long roundRemainTime = this._data.GetRoundRemainTime();
			this.SetRoundRefreshTime(roundRemainTime);
		}

		private void UpdateBossMode()
		{
			WorldBoss_WorldBoss worldBoss_WorldBoss = GameApp.Table.GetManager().GetWorldBoss_WorldBoss(this._data.ChapterId);
			int bossId = worldBoss_WorldBoss.bossId;
			if (bossId > 0)
			{
				GameMember_member gameMember_member = GameApp.Table.GetManager().GetGameMember_member(bossId);
				this.bossName.SetText(gameMember_member.nameLanguageID);
				this.bossSpine.gameObject.SetActiveSafe(true);
				this.bossSpine.ShowMemberModel(bossId, "Idle", true);
				this.bossSpine.SetScale(worldBoss_WorldBoss.uiScale);
				return;
			}
			this.bossSpine.gameObject.SetActiveSafe(false);
			this.bossName.text = "";
		}

		private void OnBossDataUpdate(object sender, int type, BaseEventArgs args)
		{
			if (!this.CheckStateAnTip())
			{
				GameApp.View.CloseView(ViewName.WorldBossViewModule, null);
				return;
			}
			base.gameObject.SetActiveSafe(true);
			this.UpdateBossMode();
			this.UpdateBoxText();
			this.UpdateDamage();
			this.SetRankInfo();
			this.UpdateTimeCount();
			this.CheckFirstOpen();
		}

		private void CheckFirstOpen()
		{
			if (this.IsFirstOpen())
			{
				this.isFirstOpen = true;
				this._data.WorldBossChapter = this._data.ChapterId;
				this.OnShowInfoPanel();
				return;
			}
			this.ShowRankUpAnim();
		}

		public override void OnOpen(object data)
		{
			if (!this._data.HasInfo())
			{
				base.gameObject.SetActiveSafe(false);
				this._data.GetWorldBossInfo(true, new Action<bool, int>(this.OnNet_GetWorldBossInfo));
				return;
			}
			this.OnBossDataUpdate(null, 0, null);
		}

		public override void OnClose()
		{
			this._hasClickStart = false;
			if (GameApp.View.IsOpened(ViewName.WorldBossAnimViewModule))
			{
				GameApp.View.CloseView(ViewName.WorldBossAnimViewModule, null);
			}
			if (GameApp.View.IsOpened(ViewName.WorldBossDamageViewModule))
			{
				GameApp.View.CloseView(ViewName.WorldBossDamageViewModule, null);
			}
			if (GameApp.View.IsOpened(ViewName.WorldBossRankViewModule))
			{
				GameApp.View.CloseView(ViewName.WorldBossRankViewModule, null);
			}
		}

		private void OnRankButtonClick()
		{
			GameApp.View.OpenView(ViewName.WorldBossRankViewModule, null, 1, null, null);
		}

		private void OnCloseButtonClick()
		{
			GameApp.View.CloseView(ViewName.WorldBossViewModule, null);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (!this._data.HasInfo())
			{
				return;
			}
			this.UpdateTimeCount();
		}

		private void OnNet_GetWorldBossInfo(bool success, int code)
		{
			if (!success)
			{
				this.OnCloseButtonClick();
			}
		}

		private void InitRender()
		{
			this.bossSpine.Init();
		}

		private void DeInitRender()
		{
			this.bossSpine.DeInit();
		}

		private void SetRoundRefreshTime(long time)
		{
			this.bossRefreshTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Boss_FreshTime", new object[] { Singleton<LanguageManager>.Instance.GetTime(time) });
		}

		private void OnShowInfoPanel()
		{
			Module_moduleInfo elementById = GameApp.Table.GetManager().GetModule_moduleInfoModelInstance().GetElementById(this.systemId);
			if (elementById != null)
			{
				InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData();
				openData.m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameId);
				openData.m_contextInfo = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID(elementById.infoId), this._data.Id, this._data.Round);
				GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
			}
		}

		private void OnInfoViewClose(object sender, int type, BaseEventArgs args)
		{
			if (this.isFirstOpen)
			{
				this.ShowRankUpAnim();
			}
		}

		private void SetRankInfo()
		{
			int rankLevel = this._data.RankLevel;
			if (this._data.Id >= 1 && rankLevel > 0)
			{
				this.rankObj.SetActiveSafe(true);
				WorldBoss_Subsection worldBoss_Subsection = GameApp.Table.GetManager().GetWorldBoss_Subsection(rankLevel);
				this.rankGroupRank.SetImage(worldBoss_Subsection.atlasName, worldBoss_Subsection.atlasId);
				this.rankGroupName.SetText(worldBoss_Subsection.languageId);
				return;
			}
			this.rankObj.SetActiveSafe(false);
		}

		private void UpdateDamage()
		{
			this.damageText.text = DxxTools.FormatNumber(this._data.TotalDamage);
		}

		private void SetSeasonRefreshTime(long time)
		{
			this.seasonRefreshTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Season_EndTime", new object[] { Singleton<LanguageManager>.Instance.GetTime(time) });
		}

		private void ShowRankUpAnim()
		{
			this.isFirstOpen = false;
			if (this._isShowRankAnim)
			{
				return;
			}
			if (this._data.WorldBossGroupType != 0)
			{
				this.rankObj.SetActive(false);
				this._isShowRankAnim = true;
				WorldBossAnimView.OpenData openData = new WorldBossAnimView.OpenData
				{
					OnClose = new Action(this.OnRankAnimClose),
					FlyTarget = this.rankGroupRank.transform.position,
					ScaleTarget = this.rankGroupRank.GetComponent<RectTransform>().sizeDelta,
					GroupType = this._data.WorldBossGroupType,
					RankLevel = this._data.RankLevel,
					LastRank = this._data.LastRank,
					LastRankLevel = this._data.LastRankLevel
				};
				this.ClearGroupType();
				GameApp.View.OpenView(ViewName.WorldBossAnimViewModule, openData, 1, null, null);
			}
		}

		private void OnRankAnimClose()
		{
			this.rankObj.SetActive(true);
			this._isShowRankAnim = false;
			Sequence sequence = this._seqPool.Get();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.rankGroupRank.transform, 1.3f * Vector3.one, 0.1f));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.rankGroupRank.transform, 1f * Vector3.one, 0.1f));
		}

		private bool IsFirstOpen()
		{
			return this._data.WorldBossChapter == 0;
		}

		private void ClearGroupType()
		{
			this._data.WorldBossGroupType = 0;
		}

		private void SetStartBtnState()
		{
			if (this._data.CanPlay() && this._data.CanFreePlay())
			{
				this.startButton.SetGrayState(false);
				return;
			}
			this.startButton.SetGrayState(true);
		}

		private void UpdateStartBtn()
		{
			this.SetStartBtnState();
			int freeCount = this._data.FreeCount;
			if (freeCount < 1)
			{
				this.SetStartTxtByTime(this._data.GetRefreshCountRemainTime());
				return;
			}
			this.startText_Left.text = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_ChallengeTimes_DayLeft", new object[] { freeCount });
		}

		private void SetStartTxtByTime(long time)
		{
			this.startText_Left.text = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_ChallengeTimes_FreshTime", new object[] { Singleton<LanguageManager>.Instance.GetTime(time) });
		}

		private void SetBoxAnimEnable(bool enable)
		{
			this.boxAnim.enabled = enable;
			this.boxClaimRed.SetActiveSafe(enable);
		}

		private void UpdateBoxText()
		{
			bool flag = this._data.HasInfo() && this._data.CurReward != null;
			this.boxObj.SetActiveSafe(flag);
			if (!flag)
			{
				return;
			}
			this.SetBoxAnimEnable(false);
			this.boxClaimTipText.text = Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Reward_Claim_TimeTip");
			this.boxText.text = string.Format("{0}/{1}", this._data.ChallengeCount, this._data.CurRewardNeedCount);
			this.boxSlider.value = (float)this._data.ChallengeCount / (float)this._data.CurRewardNeedCount;
			this.boxClaimedObj.SetActiveSafe(this._data.HasGetAllBox);
			if (this._data.CanOpenBox())
			{
				this.SetBoxAnimEnable(true);
			}
		}

		private void OnStartButtonClick()
		{
			if (this._hasClickStart)
			{
				return;
			}
			this._hasClickStart = true;
			NetworkUtils.WorldBoss.DoStartWorldBoss(delegate(bool result, StartWorldBossResponse resp)
			{
				if (result)
				{
					this.UpdateStartBtn();
					this.UpdateBoxText();
					this.DoBattle();
					return;
				}
				this._hasClickStart = false;
			});
		}

		private void OnBoxButtonClick()
		{
			if (this._boxClicked || !this._data.HasInfo())
			{
				return;
			}
			this._boxClicked = true;
			if (this._data.CanOpenBox())
			{
				NetworkUtils.WorldBoss.DoGetWorldBossBoxReward(this._data.RewardMaxClaimed, new Action<bool>(this.NetEvent_GetWorldBossBoxReward));
				return;
			}
			this._boxClicked = false;
			List<ItemData> list = new List<ItemData>();
			for (int i = 0; i < this._data.CurReward.Reward.Length; i++)
			{
				string[] array = this._data.CurReward.Reward[i].Split(',', StringSplitOptions.None);
				ItemData itemData = new ItemData(int.Parse(array[0]), (long)int.Parse(array[1]));
				list.Add(itemData);
			}
			UIBoxInfoViewModule.Transfer transfer = new UIBoxInfoViewModule.Transfer
			{
				nodeType = UIBoxInfoViewModule.UIBoxInfoNodeType.Up,
				rewards = list,
				position = this.boxButton.transform.position,
				secondLayer = true
			};
			GameApp.View.OpenView(ViewName.RewardBoxInfoViewModule, transfer, 1, null, null);
		}

		private void OnHistoryButtonClick()
		{
		}

		private bool CheckStateAnTip()
		{
			if (this._data == null)
			{
				return false;
			}
			if (this._data.Id < 1)
			{
				if (this._data.NextOpenTimestamp > 0L)
				{
					this._data.ShowNextSeasonTimeTip();
				}
				else
				{
					this._data.ShowNoSeasonTip();
				}
				return false;
			}
			return true;
		}

		private void NetEvent_GetWorldBossBoxReward(bool success)
		{
			this._boxClicked = false;
			if (success && this._data.BoxRewardDtos != null && this._data.BoxRewardDtos.Count > 0)
			{
				DxxTools.UI.OpenRewardCommon(this._data.BoxRewardDtos, null, true);
			}
		}

		private void DoBattle()
		{
			GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayShow(delegate
				{
					EventArgsRefreshMainOpenData instance = Singleton<EventArgsRefreshMainOpenData>.Instance;
					instance.SetData(DxxTools.UI.GetWorldBossOpenData());
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_MainDataModule_RefreshMainOpenData, instance);
					this.OnCloseButtonClick();
					EventArgsGameDataEnter instance2 = Singleton<EventArgsGameDataEnter>.Instance;
					instance2.SetData(GameModel.WorldBoss, null);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance2);
					GameApp.State.ActiveState(StateName.BattleWorldBossState);
				});
			});
		}

		[Header("基础")]
		public int systemId = 114;

		[SerializeField]
		private CustomButton infoButton;

		[SerializeField]
		private CustomButton closeButton;

		[Header("宝箱")]
		[SerializeField]
		private GameObject boxObj;

		[SerializeField]
		private CustomButton boxButton;

		[SerializeField]
		private Slider boxSlider;

		[SerializeField]
		private CustomText boxText;

		[SerializeField]
		private CustomText boxClaimTipText;

		[SerializeField]
		private Animator boxAnim;

		[SerializeField]
		private GameObject boxClaimRed;

		[SerializeField]
		private GameObject boxClaimedObj;

		[Header("挑战")]
		[SerializeField]
		private SimpleGrayButton startButton;

		[SerializeField]
		private CustomText startText_Left;

		[Header("Boss")]
		[SerializeField]
		private CustomText bossName;

		[SerializeField]
		private CustomText bossRefreshTime;

		[SerializeField]
		private UISpineModelItem bossSpine;

		[Header("记录")]
		[SerializeField]
		private CustomText seasonRefreshTime;

		[SerializeField]
		private CustomText damageText;

		[SerializeField]
		private CustomText rankGroupName;

		[SerializeField]
		private CustomImage rankGroupRank;

		[SerializeField]
		private GameObject rankObj;

		[SerializeField]
		private CustomButton historyButton;

		[Header("排行榜")]
		[SerializeField]
		private CustomButton rankButton;

		[Header("资源")]
		public SpriteRegister spriteRegister;

		private WorldBossDataModule _data;

		private SequencePool _seqPool;

		private bool isFirstOpen;

		private bool _hasClickStart;

		private bool _boxClicked;

		private bool _isShowRankAnim;
	}
}
