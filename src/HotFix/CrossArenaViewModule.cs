using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using HotFix.CrossArenaUI;
using Proto.CrossArena;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class CrossArenaViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.CrossArenaViewModule;
		}

		public override void OnCreate(object data)
		{
			this.Button_Back.onClick.AddListener(new UnityAction(this.OnCloseThis));
			this.Button_Challenge.Init();
			this.Button_Challenge.SetOnClick(new Action(this.OnChallenge));
			this.Button_Challenge.SetInfoTextByLanguageId(15004);
			this.Button_Challenge.SetItemIcon(10);
			this.Info.Init();
			this.RankPanel.Init();
			this.ObjFullScreenMask.SetActive(false);
			this.UpdateCostTicket(null, 0, null);
			this.helpButton.Init();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.UpdateCostTicket));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_CrossArena_MyRankInfoUpdate, new HandlerEvent(this.OnEventMyRankInfoUpdate));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.UpdateCostTicket));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_CrossArena_MyRankInfoUpdate, new HandlerEvent(this.OnEventMyRankInfoUpdate));
		}

		public override void OnOpen(object data)
		{
			this.mDataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
			this.RankPanel.ResetData();
			this.BuildProgress();
			this.AnimatorView.Play("open");
			this.ObjFullScreenMask.SetActive(true);
			NetworkUtils.CrossArena.DoCrossArenaEnterRequest(delegate(bool result, CrossArenaEnterResponse resp)
			{
				if (this == null || base.gameObject == null)
				{
					return;
				}
				if (!this.IsViewOpen())
				{
					return;
				}
				if (this.ObjFullScreenMask != null)
				{
					this.ObjFullScreenMask.SetActive(false);
				}
				if (result)
				{
					this.RefreshUIAfterServer();
				}
			});
			GlobalUpdater.Instance.RegisterUpdater(new Action(this.OnUpdateCheckClose));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			CrossArenaMainInfo info = this.Info;
			if (info == null)
			{
				return;
			}
			info.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
			GlobalUpdater.Instance.UnRegisterUpdater(new Action(this.OnUpdateCheckClose));
		}

		public override void OnDelete()
		{
			CustomButton button_Back = this.Button_Back;
			if (button_Back != null)
			{
				button_Back.onClick.RemoveListener(new UnityAction(this.OnCloseThis));
			}
			if (this.Info != null)
			{
				this.Info.DeInit();
			}
			if (this.Button_Challenge != null)
			{
				this.Button_Challenge.DeInit();
			}
			if (this.RankPanel != null)
			{
				this.RankPanel.DeInit();
			}
			this.helpButton.DeInit();
		}

		private void RefreshUIAfterServer()
		{
			this.Info.RefreshUI();
			this.RankPanel.RefreshOnViewOpen();
			if (this.mDataModule.LocalCache.NeedShowDanChange)
			{
				GameApp.View.OpenView(ViewName.CrossArenaDanChangeViewModule, null, 1, null, null);
			}
		}

		public bool IsViewOpen()
		{
			return base.isActiveAndEnabled && base.gameObject != null && base.gameObject.activeSelf;
		}

		private void OnEventCrossArenaRefresh(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshUIAfterServer();
		}

		private void UpdateCostTicket(object sender, int type, BaseEventArgs eventArgs)
		{
			uint newNum = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.CrossArena).NewNum;
			string text = ((newNum >= 1U) ? string.Format("{0}/1", newNum) : "<color=#FF6175>0</color>/1");
			this.Button_Challenge.SetCountText(text, false);
		}

		private void OnEventMyRankInfoUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.RankPanel != null && this.RankPanel.MyRank != null)
			{
				this.RankPanel.MyRank.RefreshMyRank();
			}
		}

		private void OnCloseThis()
		{
			GameApp.View.CloseView(this.GetName(), null);
		}

		private void OnChallenge()
		{
			GameApp.View.OpenView(ViewName.CrossArenaChallengeViewModule, null, 1, null, null);
		}

		private void BuildProgress()
		{
			this.curCrossArenaProgress = new CrossArenaProgress();
			this.curCrossArenaProgress.BuildProgress();
		}

		private void OnUpdateCheckClose()
		{
			long num = (long)Mathf.Min((float)this.curCrossArenaProgress.DailyCloseTime, (float)this.curCrossArenaProgress.SeasonCloseTime);
			if (DxxTools.Time.ServerTimestamp >= num)
			{
				GlobalUpdater.Instance.UnRegisterUpdater(new Action(this.OnUpdateCheckClose));
				string text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID("15043"), Array.Empty<object>());
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("17");
				DxxTools.UI.OpenPopCommon(text, delegate(int id)
				{
					this.TryCloseView(ViewName.CrossArenaViewModule);
					this.TryCloseView(ViewName.CrossArenaRewardsViewModule);
					this.TryCloseView(ViewName.CrossArenaChallengeViewModule);
					this.TryCloseView(ViewName.CrossArenaRecordViewModule);
					this.TryCloseView(ViewName.CrossArenaDanChangeViewModule);
				}, string.Empty, infoByID, string.Empty, false, 2);
			}
		}

		private void TryCloseView(ViewName viewName)
		{
			if (!GameApp.View.IsOpened(viewName))
			{
				return;
			}
			GameApp.View.CloseView(viewName, null);
		}

		public CustomButton Button_Back;

		public UIItemInfoButton Button_Challenge;

		public Animator AnimatorView;

		public CrossArenaMainInfo Info;

		public CrossArenaMainRankPanel RankPanel;

		public GameObject ObjFullScreenMask;

		public UIHelpButton helpButton;

		private CrossArenaDataModule mDataModule;

		private CrossArenaProgress curCrossArenaProgress;
	}
}
