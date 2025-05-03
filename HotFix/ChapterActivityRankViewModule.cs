using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Chapter;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class ChapterActivityRankViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.chapterActivityData = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule);
			this.loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.timeObj.SetActiveSafe(false);
			this.timeUpObj.SetActiveSafe(false);
			this.lockObj.SetActiveSafe(false);
			this.progressObj.SetActiveSafe(false);
			this.progressTipObj.SetActiveSafe(false);
			this.tipObj.SetActiveSafe(false);
			this.scoreInfoCtrl.gameObject.SetActiveSafe(false);
			this.rankCtrl.gameObject.SetActiveSafe(false);
			this.progressRectTrans = this.progressObj.GetComponent<RectTransform>();
			this.progressCtrl.Init();
			this.scoreInfoCtrl.Init();
			this.rankCtrl.Init();
		}

		public override void OnOpen(object data)
		{
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickCloseSelf));
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnClickCloseSelf));
			this.animationListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimationListen));
			this.buttonClose.gameObject.SetActiveSafe(true);
			bool flag = false;
			int num = 0;
			ChapterActivityRewardData rewardActivity = this.chapterActivityData.GetRewardActivity(ChapterActivityKind.Rank);
			if (rewardActivity != null)
			{
				num = rewardActivity.ActivityId;
				ChapterActivity_RankActivity elementById = GameApp.Table.GetManager().GetChapterActivity_RankActivityModelInstance().GetElementById(rewardActivity.ActivityId);
				if (elementById != null)
				{
					this.LoadAsset(elementById.bg);
					this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.name);
				}
				this.RefreshCollect();
				flag = true;
			}
			else
			{
				ChapterActivityData activeActivityData = this.chapterActivityData.GetActiveActivityData(ChapterActivityKind.Rank);
				if (activeActivityData != null)
				{
					this.activityData = activeActivityData as ChapterActivityRankData;
					num = (int)activeActivityData.ActivityId;
					this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(activeActivityData.ActivityTitleId);
					if (this.activityData != null)
					{
						this.LoadAsset(this.activityData.Config.bg);
						if ((ulong)this.activityData.TotalScore < (ulong)((long)this.activityData.Config.unlockScore))
						{
							this.RefreshLock();
						}
						else
						{
							this.RefreshRank();
							flag = true;
						}
					}
				}
			}
			if (num == 0)
			{
				this.OnClickCloseSelf();
				return;
			}
			if (flag)
			{
				this.rankCtrl.ShowNetLoading(true);
				NetworkUtils.Chapter.DoChapterActRankRequest((uint)num, delegate(bool result, ChapterActRankResponse response)
				{
					this.rankCtrl.ShowNetLoading(false);
					if (result && response != null)
					{
						this.rankList = response.RankList;
						for (int i = 0; i < response.RankList.Count; i++)
						{
							if (response.RankList[i].UserId == this.loginDataModule.userId)
							{
								this.selfRankDto = response.RankList[i];
								break;
							}
						}
						this.RefreshRankList();
					}
				});
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.activityData == null)
			{
				return;
			}
			if (this.activityData.IsEnd())
			{
				this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_end");
				return;
			}
			long num = ChapterActivityDataModule.ServerTime();
			this.textTime.text = Singleton<LanguageManager>.Instance.GetTime(this.activityData.EndTime - num);
		}

		public override void OnClose()
		{
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickCloseSelf));
			this.buttonMask.onClick.RemoveListener(new UnityAction(this.OnClickCloseSelf));
			this.animationListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimationListen));
			this.isReward = false;
		}

		public override void OnDelete()
		{
			this.progressCtrl.DeInit();
			this.scoreInfoCtrl.DeInit();
			this.rankCtrl.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterActivity_CheckShow, new HandlerEvent(this.OnEventCheckActivity));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterActivity_CheckShow, new HandlerEvent(this.OnEventCheckActivity));
		}

		private Task LoadAsset(string path)
		{
			ChapterActivityRankViewModule.<LoadAsset>d__35 <LoadAsset>d__;
			<LoadAsset>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadAsset>d__.<>4__this = this;
			<LoadAsset>d__.path = path;
			<LoadAsset>d__.<>1__state = -1;
			<LoadAsset>d__.<>t__builder.Start<ChapterActivityRankViewModule.<LoadAsset>d__35>(ref <LoadAsset>d__);
			return <LoadAsset>d__.<>t__builder.Task;
		}

		private void RefreshLock()
		{
			if (this.activityData == null)
			{
				return;
			}
			this.timeObj.SetActiveSafe(!this.activityData.IsEnd());
			this.timeUpObj.SetActiveSafe(this.activityData.IsEnd());
			this.imageBg.color = Color.gray;
			this.lockObj.SetActiveSafe(true);
			this.tipObj.SetActiveSafe(true);
			this.scoreInfoCtrl.gameObject.SetActiveSafe(true);
			this.scoreInfoCtrl.SetData(this.activityData.Config);
			this.rankCtrl.gameObject.SetActiveSafe(false);
			this.progressObj.SetActiveSafe(true);
			this.progressTipObj.SetActiveSafe(false);
			this.progressRectTrans.anchoredPosition = new Vector2(this.progressRectTrans.anchoredPosition.x, 0f);
			string[] array = Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_rank_unlock").Split('@', StringSplitOptions.None);
			if (array.Length >= 2)
			{
				this.textUnlockTip1.text = (array[0].Contains("{0}") ? string.Format(array[0], this.activityData.Config.unlockScore) : array[0]);
				this.textUnlockTip2.text = (array[1].Contains("{0}") ? string.Format(array[1], this.activityData.Config.unlockScore) : array[1]);
			}
			string atlasPath = GameApp.Table.GetAtlasPath(this.activityData.ScoreAtlasId);
			this.imageScoreIcon.SetImage(atlasPath, this.activityData.ScoreIcon);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.unlockTipTrans);
			this.progressCtrl.SetData(this.activityData, false);
			this.openAni.Play("ShowLock", 0, 0f);
		}

		private void RefreshRank()
		{
			if (this.activityData == null)
			{
				return;
			}
			this.timeObj.SetActiveSafe(true);
			this.timeUpObj.SetActiveSafe(false);
			this.imageBg.color = Color.white;
			this.lockObj.SetActiveSafe(false);
			this.tipObj.SetActiveSafe(false);
			this.scoreInfoCtrl.gameObject.SetActiveSafe(false);
			this.rankCtrl.gameObject.SetActiveSafe(true);
			this.progressObj.SetActiveSafe(true);
			this.progressTipObj.SetActiveSafe(true);
			this.progressRectTrans.anchoredPosition = new Vector2(this.progressRectTrans.anchoredPosition.x, 40f);
			int num = this.activityData.AllProgressCount();
			int num2 = this.activityData.CollectedCount();
			this.textProgressTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_rank_gift_tip", new object[] { num2, num });
			if (this.selfRankDto == null)
			{
				this.selfRankDto = new ChapterActRankDto
				{
					UserId = this.loginDataModule.userId,
					NickName = this.loginDataModule.NickName,
					Avatar = this.loginDataModule.Avatar,
					AvatarFrame = this.loginDataModule.AvatarFrame,
					Score = (int)this.activityData.TotalScore,
					Rank = 0
				};
			}
			this.progressCtrl.SetData(this.activityData, false);
			this.rankCtrl.SetData(this.activityData.Config, this.selfRankDto, 0UL);
			this.openAni.Play("ShowRank", 0, 0f);
		}

		private void RefreshCollect()
		{
			ChapterActivityRewardData rewardActivity = this.chapterActivityData.GetRewardActivity(ChapterActivityKind.Rank);
			if (rewardActivity == null)
			{
				return;
			}
			this.imageBg.color = Color.white;
			this.timeObj.SetActiveSafe(false);
			this.timeUpObj.SetActiveSafe(true);
			this.lockObj.SetActiveSafe(false);
			this.tipObj.SetActiveSafe(false);
			this.scoreInfoCtrl.gameObject.SetActiveSafe(false);
			this.rankCtrl.gameObject.SetActiveSafe(true);
			this.progressObj.SetActiveSafe(false);
			if (rewardActivity.RowId > 0UL)
			{
				this.buttonClose.gameObject.SetActiveSafe(false);
				this.isReward = true;
			}
			if (this.selfRankDto == null)
			{
				this.selfRankDto = new ChapterActRankDto
				{
					UserId = this.loginDataModule.userId,
					NickName = this.loginDataModule.NickName,
					Avatar = this.loginDataModule.Avatar,
					AvatarFrame = this.loginDataModule.AvatarFrame,
					Score = 0,
					Rank = 0
				};
			}
			ChapterActivity_RankActivity elementById = GameApp.Table.GetManager().GetChapterActivity_RankActivityModelInstance().GetElementById(rewardActivity.ActivityId);
			this.rankCtrl.SetData(elementById, this.selfRankDto, rewardActivity.RowId);
			this.openAni.Play("ShowCollect", 0, 0f);
		}

		private void RefreshRankList()
		{
			this.rankCtrl.RefreshRankList(this.rankList, this.selfRankDto);
		}

		private void OnClickCloseSelf()
		{
			if (this.isReward)
			{
				return;
			}
			GameApp.View.CloseView(ViewName.ChapterActivityRankViewModule, null);
		}

		private void OnAnimationListen(GameObject obj, string ani)
		{
			if (ani.Equals("Finish"))
			{
				this.progressCtrl.OnClickScore();
				Sequence sequence = DOTween.Sequence();
				TweenSettingsExtensions.AppendInterval(sequence, 2f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					if (GameApp.View.IsOpened(ViewName.RewardBoxInfoViewModule))
					{
						GameApp.View.CloseView(ViewName.RewardBoxInfoViewModule, null);
					}
				});
			}
		}

		private void OnEventCheckActivity(object sender, int type, BaseEventArgs eventArgs)
		{
			ChapterActivityRewardData rewardActivity = this.chapterActivityData.GetRewardActivity(ChapterActivityKind.Rank);
			if (rewardActivity != null)
			{
				uint activityId = (uint)rewardActivity.ActivityId;
				this.rankCtrl.ShowNetLoading(true);
				NetworkUtils.Chapter.DoChapterActRankRequest(activityId, delegate(bool result, ChapterActRankResponse response)
				{
					this.rankCtrl.ShowNetLoading(false);
					if (result && response != null)
					{
						this.rankList = response.RankList;
						for (int i = 0; i < response.RankList.Count; i++)
						{
							if (response.RankList[i].UserId == this.loginDataModule.userId)
							{
								this.selfRankDto = response.RankList[i];
								break;
							}
						}
						this.RefreshCollect();
						this.RefreshRankList();
					}
				});
			}
		}

		public CustomImage imageBg;

		public CustomText textTitle;

		public GameObject timeObj;

		public GameObject timeUpObj;

		public CustomText textTime;

		public GameObject lockObj;

		public RectTransform unlockTipTrans;

		public CustomText textUnlockTip1;

		public CustomText textUnlockTip2;

		public CustomImage imageScoreIcon;

		public GameObject progressObj;

		public GameObject progressTipObj;

		public CustomText textProgressTip;

		public GameObject tipObj;

		public UIChapterActivityProgressCtrl progressCtrl;

		public UIChapterActivityScoreInfoCtrl scoreInfoCtrl;

		public UIChapterActivityRankCtrl rankCtrl;

		public CustomButton buttonClose;

		public CustomButton buttonMask;

		public Animator openAni;

		public AnimatorListen animationListen;

		private ChapterActivityDataModule chapterActivityData;

		private ChapterActivityRankData activityData;

		private ChapterActRankDto selfRankDto;

		private RepeatedField<ChapterActRankDto> rankList;

		private RectTransform progressRectTrans;

		private bool isReward;

		private LoginDataModule loginDataModule;
	}
}
