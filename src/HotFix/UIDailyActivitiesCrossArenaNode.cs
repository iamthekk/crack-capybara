using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.CrossArena;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIDailyActivitiesCrossArenaNode : UIDailyActivitiesNodeBase
	{
		public override FunctionID FunctionOpenID
		{
			get
			{
				return FunctionID.Main_Activity_CrossArena;
			}
		}

		public override UIDailyActivitiesType DailyType
		{
			get
			{
				return UIDailyActivitiesType.CrossArena;
			}
		}

		protected override void OnInit()
		{
			this.nodeButton.onClick.AddListener(new UnityAction(this.NodeButtonClick));
			this.RefreshProgress();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_CrossArena_SetInfo, new HandlerEvent(this.OnUpdateInfo));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_CrossArena_MyRankInfoUpdate, new HandlerEvent(this.OnUpdateInfo));
			RedPointController.Instance.RegRecordChange("DailyActivity.ChallengeTag.CrossArena", new Action<RedNodeListenData>(base.OnRedPointChange));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.timer += unscaledDeltaTime;
			if (this.timer > 1f)
			{
				this.timer -= (float)((int)this.timer);
				this.RefreshTime();
			}
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_CrossArena_SetInfo, new HandlerEvent(this.OnUpdateInfo));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_CrossArena_MyRankInfoUpdate, new HandlerEvent(this.OnUpdateInfo));
			RedPointController.Instance.UnRegRecordChange("DailyActivity.ChallengeTag.CrossArena", new Action<RedNodeListenData>(base.OnRedPointChange));
			CustomButton customButton = this.nodeButton;
			if (customButton == null)
			{
				return;
			}
			customButton.onClick.RemoveListener(new UnityAction(this.NodeButtonClick));
		}

		protected override void OnShow()
		{
			this.mProgress.BuildProgress();
			this.RefreshArenaUI();
			this.RequestCrossArenaInfo();
			this.RefreshTime();
		}

		protected override void OnHide()
		{
		}

		private void RefreshTime()
		{
			if (DxxTools.Time.ServerTimestamp > this.mProgress.SeasonCloseTime)
			{
				this.mProgress.BuildProgress();
			}
			long num = (long)this.mProgress.CalcTimeSecToSeasonEnd();
			this.timeText.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitarena_end", new object[] { DxxTools.FormatFullTimeWithDay(num) });
			DelayCall.Instance.CallOnce(10, delegate
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.desRT);
			});
		}

		private void NodeButtonClick()
		{
			if (this.mIsLock)
			{
				return;
			}
			this.mProgress.BuildProgress();
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			if (serverTimestamp < this.mProgress.DailyOpenTime || serverTimestamp > this.mProgress.DailyCloseTime)
			{
				int hours = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
				CrossArena_CrossArenaTime elementById = GameApp.Table.GetManager().GetCrossArena_CrossArenaTimeModelInstance().GetElementById(1);
				string openTime = elementById.OpenTime;
				string closeTime = elementById.CloseTime;
				TimeSpan timeSpan = TimeSpan.Parse(openTime);
				TimeSpan timeSpan2 = TimeSpan.Parse(closeTime).Add(new TimeSpan(0, 0, 1));
				TimeSpan timeSpan3 = timeSpan.Add(new TimeSpan(hours, 0, 0));
				TimeSpan timeSpan4 = timeSpan2.Add(new TimeSpan(hours, 0, 0));
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("arena_open_tips", new object[]
				{
					timeSpan3.ToString("hh\\:mm"),
					timeSpan4.ToString("hh\\:mm")
				});
				GameApp.View.ShowStringTip(infoByID);
				return;
			}
			GameApp.View.OpenView(ViewName.CrossArenaViewModule, null, 1, null, null);
		}

		private void RequestCrossArenaInfo()
		{
			if (this.mIsRequestCrossArenaInfoing)
			{
				return;
			}
			this.mIsRequestCrossArenaInfoing = true;
			NetworkUtils.CrossArena.DoCrossArenaGetInfoRequest(delegate(bool result, CrossArenaGetInfoResponse resp)
			{
				if (this == null)
				{
					return;
				}
				this.RefreshArenaUI();
			});
		}

		public void RefreshArenaUI()
		{
			CrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
			this.scoreText.text = dataModule.Score.ToString();
			this.rankText.text = ((dataModule.Rank == 0) ? "--" : dataModule.Rank.ToString());
		}

		private void OnUpdateInfo(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnShow();
		}

		private void RefreshProgress()
		{
			if (this.mProgress == null)
			{
				this.mProgress = new CrossArenaProgress();
			}
			this.mProgress.BuildProgress();
		}

		[SerializeField]
		private CustomText scoreText;

		[SerializeField]
		private CustomText rankText;

		[SerializeField]
		private CustomText timeText;

		[SerializeField]
		private CustomButton nodeButton;

		[SerializeField]
		private RectTransform desRT;

		private CrossArenaProgress mProgress;

		private float timer;

		private bool mIsRequestCrossArenaInfoing;
	}
}
