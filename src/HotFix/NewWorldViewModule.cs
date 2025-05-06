using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.LeaderBoard;
using Proto.NewWorld;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class NewWorldViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonRank.onClick.AddListener(new UnityAction(this.OnClickRank));
			this.firstPlayerItem.Init();
			this.secondPlayerItem.Init();
			this.thirdPlayerItem.Init();
			this.chapterTaskItem.Init();
			this.talentTaskItem.Init();
			this.towerTaskItem.Init();
			this.dataModule = GameApp.Data.GetDataModule(DataName.NewWorldDataModule);
		}

		public override void OnOpen(object data)
		{
			this.firstPlayerItem.OnShow();
			this.secondPlayerItem.OnShow();
			this.thirdPlayerItem.OnShow();
			this.RefreshInfo();
			this.RefreshRank();
			this.RefreshTask();
			NetworkUtils.NewWorld.DoNewWorldInfoRequest(delegate(bool isOk, NewWorldInfoResponse resp)
			{
				if (isOk)
				{
					this.RefreshInfo();
				}
			});
			NetworkUtils.DoRankRequest(RankType.NewWorld, 1, false, true, new Action<int, bool, bool, LeaderBoardResponse>(this.RankPageInfo));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			long num = this.dataModule.NewWorldOpenTime - DxxTools.Time.ServerTimestamp;
			if (num > 0L)
			{
				this.timeStr = Singleton<LanguageManager>.Instance.GetTime(num);
				this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("new_world_time", new object[] { this.timeStr });
				return;
			}
			if (this.dataModule.IsAllTaskFinish())
			{
				this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("new_world_open");
				return;
			}
			this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("newworld_task_tip");
		}

		public override void OnClose()
		{
			this.firstPlayerItem.OnHide();
			this.secondPlayerItem.OnHide();
			this.thirdPlayerItem.OnHide();
		}

		public override void OnDelete()
		{
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonRank.onClick.RemoveListener(new UnityAction(this.OnClickRank));
			this.firstPlayerItem.DeInit();
			this.secondPlayerItem.DeInit();
			this.thirdPlayerItem.DeInit();
			this.chapterTaskItem.DeInit();
			this.talentTaskItem.DeInit();
			this.towerTaskItem.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_NewWorld_Refresh_Task, new HandlerEvent(this.OnEventRefreshTask));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_NewWorld_Refresh_Task, new HandlerEvent(this.OnEventRefreshTask));
		}

		private RankUserDto GetRankUser(int rank)
		{
			RepeatedField<RankUserDto> top3User = this.dataModule.Top3User;
			int num = rank - 1;
			if (top3User != null && num < top3User.Count)
			{
				return top3User[num];
			}
			return null;
		}

		private void RefreshInfo()
		{
			this.textArriveTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("new_world_arrive", new object[] { this.dataModule.EnterNewWorldPlayerCount });
		}

		private void RefreshRank()
		{
			RankUserDto selfRank = this.dataModule.SelfRank;
			int num = ((selfRank != null) ? selfRank.Rank : 0);
			this.textMyRank.text = Singleton<LanguageManager>.Instance.GetInfoByID("new_world_board", new object[] { (num > 0) ? num : "??" });
			this.firstPlayerItem.SetData(1, this.GetRankUser(1));
			this.secondPlayerItem.SetData(2, this.GetRankUser(2));
			this.thirdPlayerItem.SetData(3, this.GetRankUser(3));
		}

		private void RefreshTask()
		{
			NewWorld_newWorldTask currentTask = this.dataModule.GetCurrentTask(1);
			NewWorld_newWorldTask currentTask2 = this.dataModule.GetCurrentTask(2);
			NewWorld_newWorldTask currentTask3 = this.dataModule.GetCurrentTask(3);
			this.chapterTaskItem.gameObject.SetActiveSafe(currentTask != null);
			this.talentTaskItem.gameObject.SetActiveSafe(currentTask2 != null);
			this.towerTaskItem.gameObject.SetActiveSafe(currentTask3 != null);
			this.chapterTaskItem.SetData(currentTask, new Action(this.OnGoChapter));
			this.talentTaskItem.SetData(currentTask2, new Action(this.OnGoTalent));
			this.towerTaskItem.SetData(currentTask3, new Action(this.OnGoTower));
			int finishTaskCount = this.dataModule.GetFinishTaskCount();
			int allTaskCount = this.dataModule.GetAllTaskCount();
			bool flag = finishTaskCount >= allTaskCount;
			this.rankObj.SetActiveSafe(flag);
			this.totalTaskObj.SetActiveSafe(!flag);
			float num = (float)finishTaskCount / (float)allTaskCount;
			num = ((num >= 1f) ? 1f : num);
			this.sliderTotalTask.value = num;
			this.textTotalTask.text = Singleton<LanguageManager>.Instance.GetInfoByID("newworld_all_task", new object[] { string.Format("{0}/{1}", finishTaskCount, allTaskCount) });
		}

		private void RankPageInfo(int nextPage, bool isNextPage, bool success, LeaderBoardResponse resp)
		{
			if (success && resp != null)
			{
				this.RefreshRank();
			}
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.NewWorldViewModule, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_MainCity_Refresh, null);
		}

		private void OnClickRank()
		{
			GameApp.View.OpenView(ViewName.NewWorldRankViewModule, null, 1, null, null);
		}

		private void OnGoChapter()
		{
			this.OnClickClose();
			GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Battle, null);
		}

		private void OnGoTalent()
		{
			this.OnClickClose();
			GameApp.View.GetViewModule(ViewName.MainViewModule).GotoPage(UIMainPageName.Talent, null);
		}

		private async void OnGoTower()
		{
			if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_ChallengeTower, false))
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("new_world_go_tower");
				GameApp.View.ShowStringTip(infoByID);
			}
			else
			{
				this.OnClickClose();
				DailyActivitiesViewModule.OpenData openData = new DailyActivitiesViewModule.OpenData();
				openData.openPageType = DailyActivitiesPageType.Challenge;
				await GameApp.View.OpenViewTask(ViewName.DailyActivitiesViewModule, openData, 1, null, null);
				GameApp.View.OpenView(ViewName.TowerMainViewModule, null, 1, null, null);
			}
		}

		private void OnEventRefreshTask(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshTask();
			if (this.dataModule.IsAllTaskFinish())
			{
				NetworkUtils.DoRankRequest(RankType.NewWorld, 1, false, true, new Action<int, bool, bool, LeaderBoardResponse>(this.RankPageInfo));
			}
		}

		public CustomText textArriveTip;

		public CustomText textTime;

		public CustomText textMyRank;

		public CustomButton buttonClose;

		public CustomButton buttonRank;

		public UINewWorldTopPlayerItem firstPlayerItem;

		public UINewWorldTopPlayerItem secondPlayerItem;

		public UINewWorldTopPlayerItem thirdPlayerItem;

		public UINewWorldTaskItem chapterTaskItem;

		public UINewWorldTaskItem talentTaskItem;

		public UINewWorldTaskItem towerTaskItem;

		public GameObject rankObj;

		public GameObject totalTaskObj;

		public Slider sliderTotalTask;

		public CustomText textTotalTask;

		private NewWorldDataModule dataModule;

		private string timeStr;
	}
}
