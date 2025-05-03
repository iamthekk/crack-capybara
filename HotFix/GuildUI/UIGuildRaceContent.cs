using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.GuildRace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildRaceContent : GuildProxy.GuildProxy_BaseBehaviour
	{
		public GuildRaceBattleController BattleCtrl
		{
			get
			{
				return GuildRaceBattleController.Instance;
			}
		}

		public GuildActivityRace RaceData
		{
			get
			{
				return base.SDK.GuildActivity.GuildRace;
			}
		}

		protected override void GuildUI_OnInit()
		{
			this.Button_Tips.onClick.AddListener(new UnityAction(this.OnClickOpenTips));
			this.TabButtons.OnSwitch = new Action<CustomChooseButton>(this.OnSwitchTab);
			this.PageMember.Init();
			this.PageOpponent.Init();
			this.PageBattle.Init();
			this.ButtonLock_Opponent.onClick.AddListener(new UnityAction(this.OnShowLockOpponent));
			this.ButtonLock_Opponent.gameObject.SetActive(true);
			this.Lock_Opponent = new UIGuildRaceTabButtonLock();
			this.Lock_Opponent.SetButton(this.ButtonLock_Opponent);
			this.ButtonLock_Battle.onClick.AddListener(new UnityAction(this.OnShowLockBattle));
			this.ButtonLock_Battle.gameObject.SetActive(true);
			this.Lock_Battle = new UIGuildRaceTabButtonLock();
			this.Lock_Battle.SetButton(this.ButtonLock_Battle);
			this.MySelfInfo.Init();
			this.Text_MemberCount.gameObject.SetActive(false);
			this.Text_MemberCount.text = "";
		}

		protected override void GuildUI_OnUnInit()
		{
			CustomButton button_Tips = this.Button_Tips;
			if (button_Tips != null)
			{
				button_Tips.onClick.RemoveListener(new UnityAction(this.OnClickOpenTips));
			}
			CustomButton buttonLock_Opponent = this.ButtonLock_Opponent;
			if (buttonLock_Opponent != null)
			{
				buttonLock_Opponent.onClick.RemoveListener(new UnityAction(this.OnShowLockOpponent));
			}
			CustomButton buttonLock_Battle = this.ButtonLock_Battle;
			if (buttonLock_Battle != null)
			{
				buttonLock_Battle.onClick.RemoveListener(new UnityAction(this.OnShowLockBattle));
			}
			UIGuildRacePageMemberItem mySelfInfo = this.MySelfInfo;
			if (mySelfInfo != null)
			{
				mySelfInfo.DeInit();
			}
			this.PageMember.DeInit();
			this.PageOpponent.DeInit();
			this.PageBattle.DeInit();
		}

		protected override void GuildUI_OnShow()
		{
			this.PageMember.Show();
			this.PageOpponent.Show();
			this.PageBattle.Show();
		}

		protected override void GuildUI_OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.BattleCtrl != null && this.BattleCtrl.CurrentRaceStage != this.mCurrentRaceStage)
			{
				GuildRaceStageKind guildRaceStageKind = ((this.mCurrentRaceStage != null) ? this.mCurrentRaceStage.StageKind : ((GuildRaceStageKind)0));
				this.mCurrentRaceStage = this.BattleCtrl.CurrentRaceStage;
				GuildRaceStageKind guildRaceStageKind2 = ((this.mCurrentRaceStage != null) ? this.mCurrentRaceStage.StageKind : ((GuildRaceStageKind)0));
				this.OnSwitchRaceStage(guildRaceStageKind, guildRaceStageKind2);
			}
			if (this.PageBattle != null)
			{
				UIGuildRacePageBattle pageBattle = this.PageBattle;
				if (pageBattle == null)
				{
					return;
				}
				pageBattle.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		protected override void GuildUI_OnClose()
		{
			this.m_seqPool.Clear(false);
			this.PageMember.Close();
			this.PageOpponent.Close();
			this.PageBattle.Close();
			this.mMyGuildMembers.Clear();
			this.mOpponentGuildMembers.Clear();
		}

		public void RefreshUIOnOpenView(Action callback)
		{
			this.OnWaitGetFirstDataCallback = callback;
			this.TabButtons.ChooseButtonName("Button_Member");
			this.RefreshTabButtonLock();
			if (this.BattleCtrl != null && this.BattleCtrl.CurrentRaceKind == GuildRaceStageKind.Battle)
			{
				this.TabButtons.ChooseButtonName("Button_Battle");
			}
		}

		private void OnSwitchRaceStage(GuildRaceStageKind from, GuildRaceStageKind to)
		{
			this.RefreshTabButtonLock();
			string text = this.TabButtons.CurrentButtonName;
			switch (to)
			{
			case GuildRaceStageKind.UserApply:
				if (from == GuildRaceStageKind.BattleOver)
				{
					this.mMyGuildMembers.Clear();
					this.PageMember.SetMemberList(this.mMyGuildMembers);
					this.PageMember.RefreshUI();
					this.mOpponentGuildMembers.Clear();
					this.PageOpponent.SetMemberList(this.mOpponentGuildMembers);
					this.PageOpponent.RefreshUI();
				}
				text = "Button_Member";
				break;
			case GuildRaceStageKind.BattlePrepare:
				this.LoadGuildRaceMember(false);
				break;
			case GuildRaceStageKind.Battle:
				text = "Button_Battle";
				break;
			}
			if (text == "Button_Battle" && this.Lock_Battle.IsLock)
			{
				text = "Button_Member";
			}
			if (text == "Button_Opponent" && this.Lock_Opponent.IsLock)
			{
				text = "Button_Member";
			}
			if (text != this.TabButtons.CurrentButtonName)
			{
				this.TabButtons.ChooseButtonName(text);
			}
		}

		private void RefreshTabButtonLock()
		{
			this.m_seqPool.Clear(false);
			GuildRaceBattleController battleCtrl = this.BattleCtrl;
			if (battleCtrl == null)
			{
				this.Lock_Opponent.SetLockNow(true);
				this.Lock_Battle.SetLockNow(true);
				return;
			}
			bool flag = !battleCtrl.IsCanGetOpponentMemberList();
			if (!flag && this.Lock_Opponent.IsLock)
			{
				this.Lock_Opponent.PlayToUnLock(this.m_seqPool);
			}
			else
			{
				this.Lock_Opponent.SetLockNow(flag);
			}
			bool flag2 = !battleCtrl.IsCanGetCurDayBattleRecord();
			if (!flag2 && this.Lock_Battle.IsLock)
			{
				this.Lock_Battle.PlayToUnLock(this.m_seqPool);
				return;
			}
			this.Lock_Battle.SetLockNow(flag2);
		}

		public GuildRaceMember MakeMySelfData()
		{
			GuildRaceMember guildRaceMember = null;
			GuildUserShareData myUserData = base.SDK.User.MyUserData;
			if (this.PageMember != null && this.PageMember.MemberDataList != null)
			{
				List<GuildRaceMember> memberDataList = this.PageMember.MemberDataList;
				for (int i = 0; i < memberDataList.Count; i++)
				{
					GuildRaceMember guildRaceMember2 = memberDataList[i];
					if (guildRaceMember2.UserData.UserID == myUserData.UserID)
					{
						guildRaceMember = guildRaceMember2;
						break;
					}
				}
			}
			if (guildRaceMember == null)
			{
				guildRaceMember = new GuildRaceMember();
				guildRaceMember.UserData = myUserData;
				if (!base.SDK.GuildInfo.HasGuild)
				{
					return guildRaceMember;
				}
				guildRaceMember.Position = GuildRaceBattlePosition.None;
				guildRaceMember.ActivityPoint = 0;
				guildRaceMember.RaceScore = 0;
				guildRaceMember.Power = base.SDK.User.MyUserData.Power;
				guildRaceMember.GuildID = base.SDK.GuildInfo.GuildID;
				guildRaceMember.GuildName = base.SDK.GuildInfo.GuildData.GuildShowName;
			}
			return guildRaceMember;
		}

		public void RefreshMySelfUI()
		{
			this.MySelfInfo.SetActive(true);
			GuildRaceMember guildRaceMember = this.MakeMySelfData();
			this.MySelfInfo.SetData(guildRaceMember);
			this.MySelfInfo.RefreshUI();
			this.MySelfInfo.RefreshMineState();
		}

		private void OnClickOpenTips()
		{
			InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData();
			openData.m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID("400424");
			openData.m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID("400425");
			GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
		}

		private void OnSwitchTab(CustomChooseButton button)
		{
			if (button == null)
			{
				return;
			}
			string name = button.name;
			if (!(name == "Button_Member"))
			{
				if (!(name == "Button_Opponent"))
				{
					if (name == "Button_Battle")
					{
						this.PageMember.SetActive(false);
						this.PageOpponent.SetActive(false);
						this.PageBattle.SetActive(true);
						this.LoadBattle();
					}
				}
				else
				{
					this.PageMember.SetActive(false);
					this.PageOpponent.SetActive(true);
					this.PageBattle.SetActive(false);
					this.LoadOpponentGuildRaceMember(false);
				}
			}
			else
			{
				this.PageMember.SetActive(true);
				this.PageOpponent.SetActive(false);
				this.PageBattle.SetActive(false);
				this.LoadGuildRaceMember(false);
			}
			foreach (CustomChooseButton customChooseButton in this.TabButtons.Buttons)
			{
				if (!(customChooseButton == null))
				{
					foreach (Text text in customChooseButton.GetComponentsInChildren<Text>())
					{
						if (!(text == null))
						{
							text.color = ((customChooseButton == button) ? Color.yellow : Color.white);
						}
					}
				}
			}
			this.Text_MemberCount.color = (("Button_Member" == button.name) ? Color.yellow : Color.white);
		}

		private void LoadDataCallback()
		{
			Action onWaitGetFirstDataCallback = this.OnWaitGetFirstDataCallback;
			if (onWaitGetFirstDataCallback != null)
			{
				onWaitGetFirstDataCallback();
			}
			this.OnWaitGetFirstDataCallback = null;
			this.RefreshMySelfUI();
		}

		private void LoadGuildRaceMember(bool force = false)
		{
			if (this.mMyGuildMembers.Count > 0 && !force)
			{
				this.PageMember.SetMemberList(this.mMyGuildMembers);
				this.PageMember.RefreshUI();
				this.LoadDataCallback();
				return;
			}
			if (this.BattleCtrl == null || !this.BattleCtrl.IsCanGetMemberList())
			{
				this.PageMember.SetMemberList(this.mMyGuildMembers);
				this.PageMember.RefreshUI();
				this.LoadDataCallback();
				return;
			}
			GuildNetUtil.Guild.DoRequest_GuildRaceOwnerUserApplyListRequest(delegate(bool result, GuildRaceOwnerUserApplyListResponse resp)
			{
				if (!base.IsActive())
				{
					return;
				}
				if (result)
				{
					this.mMyGuildMembers = resp.List.ToGuildRaceUserList();
					this.SupplementEmptyPosition(this.mMyGuildMembers);
					this.mMyGuildMembers.Sort(new Comparison<GuildRaceMember>(UIGuildRaceContent.SortMemberListBySeq));
					if (this.BattleCtrl != null && this.BattleCtrl.IsCanGetMemberList())
					{
						this.Text_MemberCount.gameObject.SetActive(true);
						this.Text_MemberCount.text = string.Format("({0}/5)", this.GetRealMemberCount());
					}
					else
					{
						this.Text_MemberCount.gameObject.SetActive(false);
					}
					this.PageMember.SetMemberList(this.mMyGuildMembers);
					this.PageMember.RefreshUI();
				}
				else
				{
					HLog.LogError("获取成员列表失败");
				}
				this.LoadDataCallback();
			});
		}

		private void SupplementEmptyPosition(List<GuildRaceMember> list)
		{
			if (list == null)
			{
				return;
			}
			GuildRace_level raceLevelTab = GuildProxy.Table.GetRaceLevelTab(GuildProxy.Table.GuildRaceDan);
			if (raceLevelTab == null)
			{
				return;
			}
			string guildID = base.SDK.GuildInfo.GuildID;
			string guildShowName = base.SDK.GuildInfo.GuildData.GuildShowName;
			int num = raceLevelTab.generalNum;
			int num2 = raceLevelTab.eliteNum;
			int num3 = raceLevelTab.warriorNum;
			GuildRaceMember[] array = new GuildRaceMember[num + num2 + num3];
			for (int i = 0; i < list.Count; i++)
			{
				GuildRaceMember guildRaceMember = list[i];
				if (guildRaceMember.Index != 0)
				{
					if (guildRaceMember.Index > array.Length)
					{
						HLog.LogError("[GuildRace]服务器下发的玩家阵位超过了配表数量！！！");
					}
					else
					{
						array[guildRaceMember.Index - 1] = guildRaceMember;
						list.RemoveAt(i);
						i--;
					}
				}
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != null)
				{
					switch (array[j].Position)
					{
					case GuildRaceBattlePosition.Warrior:
						num3--;
						break;
					case GuildRaceBattlePosition.Elite:
						num2--;
						break;
					case GuildRaceBattlePosition.General:
						num--;
						break;
					}
				}
				else if (num > 0)
				{
					num--;
					array[j] = this.MakeEmptyPositionUser(GuildRaceBattlePosition.General, j + 1, guildID, guildShowName);
				}
				else if (num2 > 0)
				{
					num2--;
					array[j] = this.MakeEmptyPositionUser(GuildRaceBattlePosition.Elite, j + 1, guildID, guildShowName);
				}
				else if (num3 > 0)
				{
					num3--;
					array[j] = this.MakeEmptyPositionUser(GuildRaceBattlePosition.Warrior, j + 1, guildID, guildShowName);
				}
			}
			for (int k = array.Length - 1; k >= 0; k--)
			{
				list.Insert(0, array[k]);
			}
		}

		private int GetRealMemberCount()
		{
			if (this.mMyGuildMembers == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.mMyGuildMembers.Count; i++)
			{
				if (!this.mMyGuildMembers[i].IsEmptyUser)
				{
					num++;
				}
			}
			return num;
		}

		private GuildRaceMember MakeEmptyPositionUser(GuildRaceBattlePosition pos, int index, string guildid, string guildname)
		{
			return new GuildRaceMember
			{
				UserData = GuildUserShareDataEx.MakeEmptyUser(),
				GuildID = guildid,
				GuildName = guildname,
				ActivityPoint = 0,
				RaceScore = 0,
				Power = 0UL,
				Index = index,
				Position = pos
			};
		}

		private static int SortMemberListBySeq(GuildRaceMember x, GuildRaceMember y)
		{
			int num = x.Index.CompareTo(y.Index);
			if (num == 0)
			{
				num = y.ActivityPoint.CompareTo(x.ActivityPoint);
			}
			if (num == 0)
			{
				num = y.Power.CompareTo(x.Power);
			}
			return num;
		}

		private void LoadOpponentGuildRaceMember(bool force = false)
		{
			if (this.mOpponentGuildMembers.Count > 0 && !force)
			{
				this.PageOpponent.SetMemberList(this.mOpponentGuildMembers);
				this.PageOpponent.RefreshUI();
				this.LoadDataCallback();
				return;
			}
			if (this.BattleCtrl == null || !this.BattleCtrl.IsCanGetOpponentMemberList())
			{
				this.PageOpponent.SetMemberList(this.mOpponentGuildMembers);
				this.PageOpponent.RefreshUI();
				this.LoadDataCallback();
				return;
			}
			GuildNetUtil.Guild.DoRequest_GuildRaceOppUserApplyListRequest(delegate(bool result, GuildRaceOppUserApplyListResponse resp)
			{
				if (!base.IsActive())
				{
					return;
				}
				if (result)
				{
					this.mOpponentGuildMembers = resp.List.ToGuildRaceUserList();
					this.PageOpponent.SetMemberList(this.mOpponentGuildMembers);
					this.PageOpponent.RefreshUI();
				}
				else
				{
					HLog.LogError("获取成员列表失败");
				}
				this.LoadDataCallback();
			});
		}

		private void LoadBattle()
		{
			this.BattleCtrl.LoadMyGuildCurDayBattle(delegate(int result)
			{
				if (!base.IsActive())
				{
					return;
				}
				if (result > 0)
				{
					this.PageBattle.RefreshUI();
					this.LoadDataCallback();
				}
			});
		}

		private void OnShowLockOpponent()
		{
		}

		private void OnShowLockBattle()
		{
		}

		public void RefreshUIAsNULL()
		{
			this.PageMember.SetActive(false);
			this.PageOpponent.SetActive(false);
			this.PageBattle.SetActive(false);
			this.MySelfInfo.SetActive(false);
			this.Text_MemberCount.gameObject.SetActive(false);
		}

		public void ForceLoadData()
		{
			this.LoadGuildRaceMember(true);
		}

		public CustomButton Button_Tips;

		public CustomText Text_MemberCount;

		public CustomChooseButtonGroup TabButtons;

		public UIGuildRacePageMember PageMember;

		private const string TabName_Member = "Button_Member";

		public UIGuildRacePageOpponent PageOpponent;

		private const string TabName_Opponent = "Button_Opponent";

		public UIGuildRacePageBattle PageBattle;

		private const string TabName_Battle = "Button_Battle";

		public CustomButton ButtonLock_Opponent;

		public UIGuildRaceTabButtonLock Lock_Opponent;

		public CustomButton ButtonLock_Battle;

		public UIGuildRaceTabButtonLock Lock_Battle;

		public UIGuildRacePageMemberItem MySelfInfo;

		private GuildRaceStagePart mCurrentRaceStage;

		private List<GuildRaceMember> mMyGuildMembers = new List<GuildRaceMember>();

		private List<GuildRaceMember> mOpponentGuildMembers = new List<GuildRaceMember>();

		public Action OnWaitGetFirstDataCallback;

		private SequencePool m_seqPool = new SequencePool();
	}
}
