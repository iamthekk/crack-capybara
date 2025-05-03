using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIChapterRankEnterCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterActivity_GetScoreAni, new HandlerEvent(this.OnEventGetScoreAni));
			this.buttonAct.onClick.AddListener(new UnityAction(this.OnClickChapterRank));
			this.progressObj.SetActiveSafe(false);
			this.rewardEffect.gameObject.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterActivity_GetScoreAni, new HandlerEvent(this.OnEventGetScoreAni));
			this.buttonAct.onClick.RemoveListener(new UnityAction(this.OnClickChapterRank));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.isEnd)
			{
				return;
			}
			if (this.activityData == null)
			{
				return;
			}
			if (!this.activityData.IsEnd())
			{
				long num = ChapterActivityDataModule.ServerTime();
				this.lastTime += deltaTime;
				if (this.textTime.gameObject.activeInHierarchy && this.lastTime > 1f)
				{
					this.lastTime -= 1f;
					this.textTime.text = Singleton<LanguageManager>.Instance.GetTime(this.activityData.EndTime - num);
					return;
				}
			}
			else
			{
				this.lastTime += deltaTime;
				if (this.textTime.gameObject.activeInHierarchy && this.lastTime > 1f)
				{
					this.lastTime -= 1f;
					this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_end");
				}
				this.isEnd = true;
				EventArgsInt eventArgsInt = new EventArgsInt();
				eventArgsInt.SetData((int)this.activityData.Kind);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterActivity_CheckShow, eventArgsInt);
			}
		}

		public void SetData(ChapterActivityData data, Action clickBtn, bool isPlayAni)
		{
			this.isEnd = false;
			this.activityData = data;
			this.onClickSelf = clickBtn;
			this.playAniEnabled = isPlayAni;
			if (this.activityData.Kind == ChapterActivityKind.Rank)
			{
				ChapterActivityRankData chapterActivityRankData = this.activityData as ChapterActivityRankData;
				if (chapterActivityRankData != null && (ulong)chapterActivityRankData.TotalScore < (ulong)((long)chapterActivityRankData.Config.unlockScore))
				{
					this.isLock = true;
					this.unlockScore = chapterActivityRankData.Config.unlockScore;
				}
			}
			this.lockObj.SetActiveSafe(this.isLock);
			this.rewardObj.SetActiveSafe(!this.isLock);
			string atlasPath = GameApp.Table.GetAtlasPath(this.activityData.ScoreAtlasId);
			this.imageScoreIcon.SetImage(atlasPath, this.activityData.ScoreIcon);
			this.imageProgressScoreIcon.SetImage(atlasPath, this.activityData.ScoreIcon);
			this.textScore.text = ((this.activityData.TotalScore == 0U) ? "" : this.activityData.TotalScore.ToString());
			this.RefreshProgress();
		}

		private void RefreshProgress()
		{
			bool flag = this.activityData.IsFinish();
			int num = (flag ? 0 : this.activityData.CurrentProgress.num);
			int currentScore = this.activityData.CurrentScore;
			this.sliderProgress.value = (flag ? 1f : Utility.Math.Clamp01((float)currentScore / (float)num));
			List<ItemData> progressRewards = this.activityData.GetProgressRewards();
			if (progressRewards.Count > 0)
			{
				ItemData itemData = progressRewards[0];
				string atlasPath = GameApp.Table.GetAtlasPath(itemData.Data.atlasID);
				this.imageRewardIcon.SetImage(atlasPath, itemData.Data.icon);
			}
		}

		public void Show()
		{
			this.child.gameObject.SetActiveSafe(true);
		}

		public void Hide()
		{
			this.child.gameObject.SetActiveSafe(false);
		}

		public void PlayAni(bool isShow)
		{
			if (isShow)
			{
				this.PlayShowAni();
				return;
			}
			this.PlayHideAni();
		}

		private void PlayShowAni()
		{
			Sequence sequence = DOTween.Sequence();
			this.child.anchoredPosition = new Vector2((float)Screen.width, this.child.anchoredPosition.y);
			this.child.gameObject.SetActiveSafe(true);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.child, -20f, 0.3f, false)), ShortcutExtensions46.DOAnchorPosX(this.child, 0f, 0.2f, false));
		}

		private void PlayHideAni()
		{
			Sequence sequence = DOTween.Sequence();
			this.child.anchoredPosition = Vector2.zero;
			this.child.gameObject.SetActiveSafe(true);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.child, -20f, 0.2f, false)), ShortcutExtensions46.DOAnchorPosX(this.child, (float)Screen.width, 0.3f, false));
		}

		public Transform GetFlyNode()
		{
			return this.flyNode;
		}

		private void OnClickChapterRank()
		{
			Action action = this.onClickSelf;
			if (action == null)
			{
				return;
			}
			action();
		}

		private void OnEventGetScoreAni(object sender, int type, BaseEventArgs eventArgs)
		{
			if (!this.playAniEnabled)
			{
				return;
			}
			EventArgsChapterActivityGetScoreAni arg = eventArgs as EventArgsChapterActivityGetScoreAni;
			if (arg != null)
			{
				if (arg.RowId != this.activityData.RowId)
				{
					return;
				}
				GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule).AddAnimation();
				Sequence sequence = DOTween.Sequence();
				float duration = 0.5f;
				TweenSettingsExtensions.Append(sequence, this.RefreshScoreAni(arg.OldTotalScore, arg.NewTotalScore, duration));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.progressObj.SetActiveSafe(true);
				});
				if (this.isLock)
				{
					if (arg.NewTotalScore < this.unlockScore)
					{
						float num = Utility.Math.Clamp01((float)arg.OldTotalScore / (float)arg.NewTotalScore);
						TweenSettingsExtensions.Append(sequence, this.RefreshProgressAni(this.sliderProgress.value, num, duration));
						TweenSettingsExtensions.AppendInterval(sequence, duration);
						TweenSettingsExtensions.AppendCallback(sequence, delegate
						{
							this.AnimationAllFinish();
						});
						return;
					}
					TweenSettingsExtensions.Append(sequence, this.RefreshProgressAni(this.sliderProgress.value, 1f, duration));
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						this.lockAnim.Play("Anim_UIFX_TalentItem_Unlock");
					});
					TweenSettingsExtensions.AppendInterval(sequence, 0.5f);
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						this.rewardObj.SetActiveSafe(true);
						this.rewardObj.transform.localScale = Vector3.zero;
					});
					TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.rewardObj.transform, Vector3.one * 1.2f, 0.2f)), ShortcutExtensions.DOScale(this.rewardObj.transform, Vector3.one, 0.1f));
					TweenSettingsExtensions.AppendInterval(sequence, 0.2f);
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						this.isLock = false;
						this.lockObj.SetActiveSafe(false);
						this.sliderProgress.value = 0f;
					});
				}
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.CheckReward(arg, duration);
				});
			}
		}

		private Sequence RefreshScoreAni(int numFrom, int numTo, float duration)
		{
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.OnUpdate<Sequence>(TweenSettingsExtensions.Append(sequence, DOTween.To(() => numFrom, delegate(int x)
			{
				numFrom = x;
			}, numTo, duration)), delegate
			{
				this.textScore.text = numFrom.ToString();
			}), delegate
			{
				this.textScore.text = numTo.ToString();
			});
			return sequence;
		}

		private Sequence RefreshProgressAni(float from, float to, float duration)
		{
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.OnUpdate<Sequence>(TweenSettingsExtensions.Append(sequence, DOTween.To(() => from, delegate(float x)
			{
				from = x;
			}, to, duration)), delegate
			{
				this.sliderProgress.value = from;
			}), delegate
			{
				this.sliderProgress.value = to;
			});
			return sequence;
		}

		private void CheckReward(EventArgsChapterActivityGetScoreAni arg, float duration)
		{
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
			if (arg.rewards.Count == 0 && arg.OldIndex == arg.NewIndex)
			{
				float num = Utility.Math.Clamp01((float)arg.NewScore / (float)arg.chapterObjDic[arg.NewIndex].num);
				TweenSettingsExtensions.Append(sequence, this.RefreshProgressAni(this.sliderProgress.value, num, duration));
				TweenSettingsExtensions.AppendInterval(sequence, 1f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.AnimationAllFinish();
				});
				return;
			}
			TweenCallback <>9__3;
			for (int i = arg.OldIndex; i <= arg.NewIndex; i++)
			{
				int index = i;
				if (index == arg.NewIndex)
				{
					float num2 = Utility.Math.Clamp01((float)arg.NewScore / (float)arg.chapterObjDic[arg.NewIndex].num);
					TweenSettingsExtensions.Append(sequence, this.RefreshProgressAni(0f, num2, duration));
					TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.RefreshProgress));
					break;
				}
				int num3 = 0;
				if (index == arg.OldIndex)
				{
					num3 = arg.OldScore;
				}
				TweenSettingsExtensions.Append(sequence, this.RefreshProgressAni((float)num3, 1f, duration));
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.progressTrans, 1.2f, 0.2f));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					List<ItemData> progressRewards = this.activityData.GetProgressRewards(index + 1);
					if (progressRewards.Count > 0)
					{
						ItemData itemData = progressRewards[0];
						string atlasPath = GameApp.Table.GetAtlasPath(itemData.Data.atlasID);
						this.imageRewardIcon.SetImage(atlasPath, itemData.Data.icon);
					}
				});
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.progressTrans, 1f, 0.1f));
				TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOFade(this.rewardCanvas, 0f, 0.2f));
				Sequence sequence2 = sequence;
				TweenCallback tweenCallback;
				if ((tweenCallback = <>9__3) == null)
				{
					tweenCallback = (<>9__3 = delegate
					{
						this.rewardCanvas.transform.localScale = Vector3.zero;
						this.sliderProgress.value = 0f;
					});
				}
				TweenSettingsExtensions.AppendCallback(sequence2, tweenCallback);
				TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.rewardCanvas, 1f, 0.2f)), ShortcutExtensions.DOScale(this.rewardCanvas.transform, 1.05f, 0.2f));
				TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.ShowEffect));
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.rewardCanvas.transform, 1f, 0.1f));
			}
			Action <>9__4;
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				if (arg.rewards.Count > 0)
				{
					ChapterActivityShowRewardHelper instance = Singleton<ChapterActivityShowRewardHelper>.Instance;
					List<ItemData> rewards = arg.rewards;
					Action action;
					if ((action = <>9__4) == null)
					{
						action = (<>9__4 = delegate
						{
							this.AnimationAllFinish();
						});
					}
					instance.ShowData(rewards, action);
					return;
				}
				this.AnimationAllFinish();
			});
		}

		private void AnimationAllFinish()
		{
			this.progressObj.SetActiveSafe(false);
			EventArgsShowActivity eventArgsShowActivity = new EventArgsShowActivity();
			eventArgsShowActivity.SetData(new List<ChapterActivityKind> { ChapterActivityKind.Rank }, false);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShowActivity, eventArgsShowActivity);
			GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule).AnimationFinish();
		}

		private void ShowEffect()
		{
			this.rewardEffect.gameObject.SetActiveSafe(true);
			this.rewardEffect.Stop();
			this.rewardEffect.Play();
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.AppendInterval(sequence, 1f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.rewardEffect.gameObject.SetActiveSafe(false);
			});
		}

		[GameTestMethod("排行榜活动", "增加排行榜积分", "", 411)]
		private static void AddChapterRankScore()
		{
			ChapterActivityData activeActivityData = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule).GetActiveActivityData(ChapterActivityKind.Rank);
			if (activeActivityData != null)
			{
				MapField<ulong, uint> mapField = new MapField<ulong, uint>();
				ItemData itemData = new ItemData(1, 10L);
				mapField.Add(activeActivityData.RowId, activeActivityData.TotalScore + 107U);
				EventArgsShowActivity eventArgsShowActivity = new EventArgsShowActivity();
				eventArgsShowActivity.SetData(new List<ChapterActivityKind> { activeActivityData.Kind }, true);
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_UIGameEvent_ShowActivity, eventArgsShowActivity);
				EventArgsChapterActivityRefreshScore eventArgsChapterActivityRefreshScore = new EventArgsChapterActivityRefreshScore();
				eventArgsChapterActivityRefreshScore.SetData(mapField, new List<ItemData> { itemData });
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_ChapterActivity_RefreshScore, eventArgsChapterActivityRefreshScore);
			}
		}

		public RectTransform child;

		public CustomButton buttonAct;

		public CustomImage imageScoreIcon;

		public CustomText textScore;

		public CustomText textTime;

		public GameObject progressObj;

		public Slider sliderProgress;

		public CustomImage imageProgressScoreIcon;

		public Transform progressTrans;

		public CanvasGroup rewardCanvas;

		public CustomImage imageRewardIcon;

		public ParticleSystem rewardEffect;

		public GameObject lockObj;

		public Animator lockAnim;

		public GameObject rewardObj;

		public Transform flyNode;

		private ChapterActivityData activityData;

		private Action onClickSelf;

		private bool isLock;

		private int unlockScore;

		private bool playAniEnabled;

		private bool isEnd;

		private float lastTime;
	}
}
