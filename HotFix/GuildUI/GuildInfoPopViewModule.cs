using System;
using System.Collections.Generic;
using DG.Tweening;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Guild;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class GuildInfoPopViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.popCommon.OnClick = new Action<int>(this.OnPopClick);
			this.ManagerMemberMask.onClick.AddListener(new UnityAction(this.CloseExtendButton));
			this.Button_ManageMember.onClick.AddListener(new UnityAction(this.OpenExtendButton));
			this.Button_Leave.onClick.AddListener(new UnityAction(this.OnLeaveGuild));
			this.Button_Apply.onClick.AddListener(new UnityAction(this.OnOpenGuildApply));
			this.Button_Donation.onClick.AddListener(new UnityAction(this.OnOpenGuildContribute));
			this.Button_Log.onClick.AddListener(new UnityAction(this.OnClickLog));
			this.guildInfo.Init();
			this.LoopList.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		protected override void OnViewOpen(object data)
		{
			base.SDK.Event.RegisterEvent(8, new GuildHandlerEvent(this.OnRefreshByGuild));
			this.ManageButtonsContent.SetActive(false);
			this.RefreshOnOpen();
		}

		protected override void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.expPlayData != null)
			{
				if (this.expPlayData.playOver)
				{
					this.expPlayData = null;
					this.SetCanShowLevelUpFlag(true);
					return;
				}
				this.expPlayData.OnUpdate(deltaTime, unscaledDeltaTime);
				if (this.expPlayData.playLevelChange)
				{
					this.PlayLevelChange();
					this.expPlayData.playLevelChange = false;
				}
				this.RealRefreshLevelAndExp(this.expPlayData.curShowLevel, this.expPlayData.curShowExp);
			}
		}

		protected override void OnViewClose()
		{
			base.SDK.Event.UnRegisterEvent(8, new GuildHandlerEvent(this.OnRefreshByGuild));
			this.m_seqPool.Clear(false);
		}

		protected override void OnViewDelete()
		{
			if (this.Button_ManageMember != null)
			{
				this.Button_ManageMember.onClick.RemoveListener(new UnityAction(this.OpenExtendButton));
			}
			if (this.Button_Leave != null)
			{
				this.Button_Leave.onClick.RemoveListener(new UnityAction(this.OnLeaveGuild));
			}
			if (this.Button_Apply != null)
			{
				this.Button_Apply.onClick.RemoveListener(new UnityAction(this.OnOpenGuildApply));
			}
			if (this.Button_Apply != null)
			{
				this.Button_Donation.onClick.RemoveListener(new UnityAction(this.OnOpenGuildContribute));
			}
			if (this.Button_Log != null)
			{
				this.Button_Log.onClick.RemoveListener(new UnityAction(this.OnClickLog));
			}
			if (this.ManagerMemberMask != null)
			{
				this.ManagerMemberMask.onClick.RemoveListener(new UnityAction(this.CloseExtendButton));
			}
			this.guildInfo.DeInit();
			foreach (KeyValuePair<int, GuildInfoPopUI_Member> keyValuePair in this.uiCtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			int num = 2;
			if (index < 0 || index >= this.mMemberDataList.Count + num)
			{
				return null;
			}
			if (index < 1 || index + 1 >= this.mMemberDataList.Count + num)
			{
				return listView.NewListViewItem("EmptyItem");
			}
			index--;
			GuildUserShareData guildUserShareData = this.mMemberDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("UIGuildHallMemberItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			GuildInfoPopUI_Member guildInfoPopUI_Member = this.TryGetOrAddUI(instanceID, loopListViewItem) as GuildInfoPopUI_Member;
			if (guildInfoPopUI_Member != null)
			{
				guildInfoPopUI_Member.RefreshMember(guildUserShareData);
			}
			return loopListViewItem;
		}

		private GuildInfoPopUI_Member TryGetOrAddUI(int key, LoopListViewItem2 loopitem)
		{
			GuildInfoPopUI_Member component;
			if (this.uiCtrlDic.TryGetValue(key, out component))
			{
				return component;
			}
			component = loopitem.gameObject.GetComponent<GuildInfoPopUI_Member>();
			component.Init();
			this.uiCtrlDic.Add(key, component);
			return component;
		}

		private void RefreshByDataChange()
		{
			if (!base.SDK.GuildInfo.HasGuild)
			{
				return;
			}
			IList<GuildUserShareData> memberList = base.SDK.GuildInfo.GetMemberList();
			this.mMemberDataList.Clear();
			this.mMemberDataList.AddRange(memberList);
			this.mMemberDataList.CustomSort();
			this.LoopList.SetListItemCount(this.mMemberDataList.Count + 1 + 1, true);
			this.LoopList.RefreshAllShowItems();
			this.m_seqPool.Clear(false);
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.LoopList.ItemList, 0f, 0.05f, 0.2f, 9);
			this.maxLevel = GuildProxy.Table.GetMaxGuildLevel();
			this.guildInfo.RefreshUI();
			this.RefreshLevelExp(false);
			this.RefreshPermission();
		}

		private void RefreshOnOpen()
		{
			if (!base.SDK.GuildInfo.HasGuild)
			{
				return;
			}
			IList<GuildUserShareData> memberList = base.SDK.GuildInfo.GetMemberList();
			this.mMemberDataList.Clear();
			this.mMemberDataList.AddRange(memberList);
			this.mMemberDataList.CustomSort();
			this.LoopList.SetListItemCount(this.mMemberDataList.Count + 1 + 1, true);
			this.LoopList.RefreshAllShowItems();
			this.maxLevel = GuildProxy.Table.GetMaxGuildLevel();
			this.guildInfo.RefreshUI();
			this.RefreshLevelExp(false);
			this.RefreshPermission();
		}

		private void CloseExtendButton()
		{
			this.ManageButtonsContent.SetActive(false);
		}

		private void OpenExtendButton()
		{
			this.Button_Apply.gameObject.SetActiveSafe(base.SDK.Permission.HasPermission(GuildPermissionKind.ApplyJoin, null));
			this.ManageButtonsContent.SetActive(!this.ManageButtonsContent.activeSelf);
		}

		private void OnLeaveGuild()
		{
			GuildProxy.UI.OpenUIPopCommonDanger(GuildProxy.Language.GetInfoByID("400119"), GuildProxy.Language.GetInfoByID("400122"), GuildProxy.Language.GetInfoByID("400221"), "", new Action(this.SureLeaveGuild), null);
		}

		private void OnOpenGuildApply()
		{
			this.CloseExtendButton();
			GuildProxy.UI.OpenUIGuildApplyJoin(null);
			GuildProxy.RedPoint.ClickRedPoint("Guild.Hall.ApplyJoin");
		}

		private void OnOpenGuildContribute()
		{
			if (GuildSDKManager.Instance.GuildInfo.DayContributeTimes >= GuildSDKManager.Instance.GuildInfo.GuildContributeConfigs.Count)
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("guild_contribute_finish"));
				return;
			}
			int num = 0;
			Guild_guildLevel guildLevelTable = GuildProxy.Table.GetGuildLevelTable(GuildSDKManager.Instance.GuildInfo.GuildDetailData.ShareData.GuildLevel);
			if (guildLevelTable != null)
			{
				num = guildLevelTable.MaxContribute - GuildSDKManager.Instance.GuildInfo.DayAllContributeTimes;
			}
			if (num <= 0)
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("guild_contribute_finish"));
				return;
			}
			GuildProxy.UI.OpenUIGuildContribute();
		}

		private void OnClickLog()
		{
			GuildProxy.UI.OpenGuildLog();
		}

		private void SureLeaveGuild()
		{
			if (base.SDK.GuildInfo.GetAllMemberCount() == 1)
			{
				GuildNetUtil.Guild.DoRequest_DismissGuild(delegate(bool result, GuildDismissResponse resp)
				{
					if (result)
					{
						this.CloseSelfView();
						GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_Guild_Leave);
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400123"));
					}
				});
				return;
			}
			GuildNetUtil.Guild.DoRequest_LeaveGuild(delegate(bool result, GuildLeaveResponse resp)
			{
				if (result)
				{
					this.CloseSelfView();
					GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_Guild_Leave);
					GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400123"));
				}
			});
		}

		private void RefreshLevelExp(bool showani = false)
		{
			GuildShareData guildData = base.SDK.GuildInfo.GuildData;
			if (showani)
			{
				this.expPlayData = new GuildInfoPopViewModule.LevelExpPlayData();
				this.expPlayData.Reset(this.curShowLevel, this.curShowExp, guildData.GuildLevel, guildData.GuildExp);
				return;
			}
			this.expPlayData = null;
			this.RealRefreshLevelAndExp(guildData.GuildLevel, guildData.GuildExp);
		}

		private void RefreshPermission()
		{
			this.Button_Apply.gameObject.SetActiveSafe(base.SDK.Permission.HasPermission(GuildPermissionKind.ApplyJoin, null));
		}

		private void RealRefreshLevelAndExp(int level, int exp)
		{
			int num = this.curShowExpMax;
			if (this.curShowLevel != level)
			{
				this.curShowExpMax = 0;
			}
			this.curShowLevel = level;
			this.curShowExp = exp;
			if (this.curShowExpMax < 1)
			{
				num = GuildProxy.Table.GetGuildLevelTable(this.curShowLevel).Exp;
				if (num < this.curShowExp)
				{
					num = this.curShowExp;
				}
				if (num < 1)
				{
					num = 1;
				}
				this.curShowExpMax = num;
			}
			this.levelText.text = Singleton<LanguageManager>.Instance.GetInfoByID("400150", new object[] { this.curShowLevel });
			if (this.curShowExp <= 0)
			{
				this.expSlider.fillRect.gameObject.SetActive(false);
			}
			else
			{
				this.expSlider.fillRect.gameObject.SetActive(true);
				this.expSlider.value = Mathf.Max(0.04f, (float)this.curShowExp / (float)num);
			}
			if (level >= this.maxLevel)
			{
				this.expSlider.value = 1f;
				this.expCountText.text = GuildProxy.Language.GetInfoByID("400279");
				return;
			}
			this.expCountText.text = string.Format("{0}/{1}", this.curShowExp, num);
		}

		private void PlayLevelChange()
		{
			Sequence sequence = this.m_seqPool.Get();
			RectTransform rectTransform = this.levelText.rectTransform;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(rectTransform, 1.2f, 0.15f));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(rectTransform, 1f, 0.15f));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				if (base.CheckIsViewOpen() && this.expPlayData != null)
				{
					this.expPlayData = null;
					this.RefreshLevelExp(false);
				}
			});
		}

		private void OnRefreshUIAfterSign()
		{
			this.RefreshLevelExp(true);
		}

		private bool IsSignedIn()
		{
			GuildSignData guildSignData = base.SDK.GuildInfo.GuildSignData;
			return guildSignData.SignCount >= guildSignData.MaxSignCount;
		}

		private void CloseSelfView()
		{
			GameApp.View.CloseView(ViewName.GuildInfoPopViewModule, null);
		}

		private void OnPopClick(int kind)
		{
			this.CloseSelfView();
		}

		private void OnRefreshByGuild(int type, GuildBaseEvent eventArgs)
		{
			this.RefreshByDataChange();
		}

		private void SetCanShowLevelUpFlag(bool canshow)
		{
			GuildUIDataModule module = base.SDK.GetModule<GuildUIDataModule>();
			if (module != null)
			{
				module.LevelUP.SetCanShowLevelUpFlag(canshow);
			}
			if (canshow && base.SDK.GuildInfo.IsShowLevelUp)
			{
				GuildProxy.UI.OpenUIGuildLevelUp(null);
			}
		}

		[Header("PopUI")]
		public UIGuildPopCommon popCommon;

		[Header("Guild Info")]
		[SerializeField]
		private GuildInfoPopUI_Info guildInfo;

		[Header("Level/Exp")]
		[SerializeField]
		private CustomText levelText;

		[SerializeField]
		private Slider expSlider;

		[SerializeField]
		private CustomText expCountText;

		private int maxLevel;

		private int curShowLevel;

		private int curShowExp;

		private int curShowExpMax;

		private GuildInfoPopViewModule.LevelExpPlayData expPlayData;

		[Header("Member Scroll")]
		[SerializeField]
		private LoopListView2 LoopList;

		[Header("Others")]
		[SerializeField]
		private CustomButton Button_ManageMember;

		[SerializeField]
		private CustomButton ManagerMemberMask;

		[SerializeField]
		private CustomButton Button_Leave;

		[SerializeField]
		private CustomButton Button_Apply;

		[SerializeField]
		private GameObject ManageButtonsContent;

		[SerializeField]
		private CustomButton Button_Donation;

		[SerializeField]
		private CustomButton Button_Log;

		private const int mScrollTopEmptyCount = 1;

		private const int mScrollBottomEmptyCount = 1;

		private List<GuildUserShareData> mMemberDataList = new List<GuildUserShareData>();

		private readonly Dictionary<int, GuildInfoPopUI_Member> uiCtrlDic = new Dictionary<int, GuildInfoPopUI_Member>();

		private SequencePool m_seqPool = new SequencePool();

		private class LevelExpPlayData
		{
			public bool playOver
			{
				get
				{
					return this.curShowLevel > this.tarShowLevel || (this.curShowLevel == this.tarShowLevel && this.curShowExp >= this.tarShowExp);
				}
			}

			public void Reset(int curlevel, int curexp, int tarlevel, int tarexp)
			{
				this.playLevelChange = false;
				this.curShowExp = curexp;
				this.curShowLevel = curlevel;
				this.tarShowExp = tarexp;
				this.tarShowLevel = tarlevel;
				if (this.curShowLevel > this.tarShowLevel)
				{
					this.curShowExp = this.tarShowExp;
					this.curShowLevel = this.tarShowLevel;
					return;
				}
				if (this.curShowLevel == this.tarShowLevel && this.curShowExp > this.tarShowExp)
				{
					this.curShowExp = this.tarShowExp;
					this.curShowLevel = this.tarShowLevel;
					return;
				}
				this.nextShowLevel = this.tarShowLevel;
				if (this.curShowLevel < this.tarShowLevel)
				{
					this.nextShowLevel = this.curShowLevel;
					Guild_guildLevel guildLevelTable = GuildProxy.Table.GetGuildLevelTable(this.nextShowLevel);
					if (guildLevelTable != null)
					{
						this.nextShowExp = guildLevelTable.Exp;
					}
					else
					{
						this.nextShowExp = 0;
					}
				}
				else
				{
					this.nextShowExp = this.tarShowExp;
				}
				this.curShowExpFloat = (double)this.curShowExp;
				this.expchg = (double)(this.nextShowExp - this.curShowExp);
				if (this.expchg < 1.0)
				{
					this.expchg = 1.0;
				}
			}

			public void OnUpdate(float deltaTime, float unscaledDeltaTime)
			{
				if (this.curShowExp < this.nextShowExp)
				{
					this.curShowExpFloat += this.expchg * (double)unscaledDeltaTime * 5.0;
					this.curShowExp = (int)this.curShowExpFloat;
					if (this.curShowExp >= this.nextShowExp)
					{
						this.curShowExp = this.nextShowExp;
					}
					return;
				}
				this.playLevelChange = true;
				if (this.playOver)
				{
					return;
				}
				this.nextShowLevel++;
				if (this.nextShowLevel > this.tarShowLevel)
				{
					this.nextShowLevel = this.tarShowLevel;
				}
				this.curShowExp = 0;
				this.curShowLevel = this.nextShowLevel;
				Guild_guildLevel guildLevelTable = GuildProxy.Table.GetGuildLevelTable(this.nextShowLevel);
				if (guildLevelTable != null)
				{
					this.nextShowExp = guildLevelTable.Exp;
				}
				else
				{
					this.nextShowExp = 0;
				}
				this.expchg = (double)(this.nextShowExp - this.curShowExp);
				if (this.expchg < 1.0)
				{
					this.expchg = 1.0;
				}
			}

			public int tarShowLevel;

			public int tarShowExp;

			private int nextShowLevel;

			private int nextShowExp;

			public int curShowLevel;

			public int curShowExp;

			private double curShowExpFloat;

			private double expchg = 1.0;

			public bool playLevelChange;
		}
	}
}
