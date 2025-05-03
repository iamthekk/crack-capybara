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
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIChapterActivityProgressCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ChapterActivity_GetScoreAni, new HandlerEvent(this.OnEventGetScoreAni));
			this.buttonScore.m_onClick = new Action(this.OnClickPreview);
			this.buttonReward.onClick.AddListener(new UnityAction(this.OnClickReward));
			this.buttonPreview.m_onClick = new Action(this.OnClickPreview);
			this.rewardEffect.gameObject.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ChapterActivity_GetScoreAni, new HandlerEvent(this.OnEventGetScoreAni));
			this.buttonScore.m_onClick = null;
			this.buttonReward.onClick.RemoveListener(new UnityAction(this.OnClickReward));
			this.buttonPreview.m_onClick = null;
		}

		public void SetData(ChapterActivityData data, bool isPlayAni)
		{
			this.activityData = data;
			this.isPlayAnimation = isPlayAni;
			this.Refresh();
		}

		private void Refresh()
		{
			string atlasPath = GameApp.Table.GetAtlasPath(this.activityData.ScoreAtlasId);
			this.imageScoreIcon.SetImage(atlasPath, this.activityData.ScoreIcon);
			bool flag = this.activityData.IsFinish();
			int num = (flag ? 0 : this.activityData.CurrentProgress.num);
			int currentScore = this.activityData.CurrentScore;
			this.sliderProgress.value = (flag ? 1f : Utility.Math.Clamp01((float)currentScore / (float)num));
			this.textProgress.text = (flag ? Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_finish") : string.Format("{0}/{1}", currentScore, num));
			List<ItemData> progressRewards = this.activityData.GetProgressRewards();
			if (progressRewards.Count > 0)
			{
				ItemData itemData = progressRewards[0];
				string atlasPath2 = GameApp.Table.GetAtlasPath(itemData.Data.atlasID);
				this.imageRewardIcon.SetImage(atlasPath2, itemData.Data.icon);
				this.imageRewardNum.text = "x" + DxxTools.FormatNumber(itemData.TotalCount);
			}
		}

		private void OnClickPreview()
		{
			if (this.isPlayAnimation)
			{
				return;
			}
			ChapterActivityRankPreviewViewModule.OpenData openData = new ChapterActivityRankPreviewViewModule.OpenData();
			openData.ChapterActivity = this.activityData;
			GameApp.View.OpenView(ViewName.ChapterActivityRankPreviewViewModule, openData, 1, null, null);
		}

		public void OnClickScore()
		{
		}

		private void OnClickReward()
		{
			UIBoxInfoViewModule.Transfer transfer = new UIBoxInfoViewModule.Transfer
			{
				nodeType = UIBoxInfoViewModule.UIBoxInfoNodeType.Up,
				rewards = this.activityData.GetProgressRewards(),
				position = this.buttonReward.transform.position,
				anchoredPositionOffset = new Vector3(0f, 50f, 0f),
				secondLayer = true
			};
			GameApp.View.OpenView(ViewName.RewardBoxInfoViewModule, transfer, 1, null, null);
		}

		private void OnEventGetScoreAni(object sender, int type, BaseEventArgs eventArgs)
		{
			if (!this.isPlayAnimation)
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
				int oldScore = arg.OldScore;
				float num = 0.5f;
				Sequence sequence = DOTween.Sequence();
				if (arg.rewards.Count == 0 && arg.OldIndex == arg.NewIndex)
				{
					int newScore = arg.NewScore;
					int num2 = arg.chapterObjDic[arg.OldIndex].num;
					TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(oldScore, newScore, num2, num));
					TweenSettingsExtensions.AppendInterval(sequence, 1f);
					TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.AnimationFinish));
					return;
				}
				for (int i = arg.OldIndex; i <= arg.NewIndex; i++)
				{
					int index = i;
					ChapterActivity_ChapterObj chapterObj = arg.chapterObjDic[index];
					if (index == arg.NewIndex)
					{
						TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(0, arg.NewScore, chapterObj.num, num));
						TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.Refresh));
						break;
					}
					this.rewardEffect.gameObject.SetActiveSafe(false);
					int num3 = 0;
					if (index == arg.OldIndex)
					{
						num3 = arg.OldScore;
					}
					TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(num3, chapterObj.num, chapterObj.num, num));
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.child, 1.2f, 0.2f));
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						List<ItemData> progressRewards = this.activityData.GetProgressRewards(index + 1);
						if (progressRewards.Count > 0)
						{
							ItemData itemData = progressRewards[0];
							string atlasPath = GameApp.Table.GetAtlasPath(itemData.Data.atlasID);
							this.imageRewardIcon.SetImage(atlasPath, itemData.Data.icon);
							this.imageRewardNum.text = "x" + DxxTools.FormatNumber(itemData.TotalCount);
						}
					});
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.child, 1f, 0.1f));
					TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOFade(this.rewardCanvas, 0f, 0.2f));
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						this.rewardCanvas.transform.localScale = Vector3.zero;
						this.sliderProgress.value = 0f;
						Color color = this.textProgress.color;
						color.a = 0f;
						this.textProgress.color = color;
						this.textProgress.text = string.Format("{0}/{1}", 0, chapterObj.num);
					});
					TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.rewardCanvas, 1f, 0.2f)), ShortcutExtensions.DOScale(this.rewardCanvas.transform, 1.05f, 0.2f));
					TweenSettingsExtensions.Append(sequence, this.ShowEffect());
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.rewardCanvas.transform, 1f, 0.1f));
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.textProgress, 1f, 0.2f));
				}
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					if (arg.rewards.Count > 0)
					{
						Singleton<ChapterActivityShowRewardHelper>.Instance.ShowData(arg.rewards, new Action(this.AnimationFinish));
						return;
					}
					this.AnimationFinish();
				});
			}
		}

		private void AnimationFinish()
		{
			if (Singleton<GameEventController>.Instance.ActiveStateName == GameEventStateName.Chapter)
			{
				EventArgsShowActivity eventArgsShowActivity = new EventArgsShowActivity();
				eventArgsShowActivity.SetData(new List<ChapterActivityKind> { ChapterActivityKind.Normal }, false);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ShowActivity, eventArgsShowActivity);
			}
			this.Refresh();
			GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule).AnimationFinish();
		}

		private Sequence PlayProgressAni(int numFrom, int numTo, int numEnd, float duration)
		{
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.OnUpdate<Sequence>(TweenSettingsExtensions.Append(sequence, DOTween.To(() => numFrom, delegate(int x)
			{
				numFrom = x;
			}, numTo, duration)), delegate
			{
				this.textProgress.text = string.Format("{0}/{1}", numFrom, numEnd);
				float num = (float)numFrom / (float)numEnd;
				this.sliderProgress.value = num;
			}), delegate
			{
				this.textProgress.text = string.Format("{0}/{1}", numTo, numEnd);
				float num2 = (float)numTo / (float)numEnd;
				this.sliderProgress.value = num2;
			});
			return sequence;
		}

		private Sequence ShowEffect()
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
			return sequence;
		}

		[GameTestMethod("主线活动", "增加活动积分", "", 411)]
		private static void AddChapterActivityScore()
		{
			ChapterActivityData activeActivityData = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule).GetActiveActivityData(ChapterActivityKind.Normal);
			if (activeActivityData != null)
			{
				MapField<ulong, uint> mapField = new MapField<ulong, uint>();
				ItemData itemData = new ItemData(1, 10L);
				mapField.Add(activeActivityData.RowId, activeActivityData.TotalScore + 12350U);
				EventArgsShowActivity eventArgsShowActivity = new EventArgsShowActivity();
				eventArgsShowActivity.SetData(new List<ChapterActivityKind> { activeActivityData.Kind }, true);
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_UIGameEvent_ShowActivity, eventArgsShowActivity);
				EventArgsChapterActivityRefreshScore eventArgsChapterActivityRefreshScore = new EventArgsChapterActivityRefreshScore();
				eventArgsChapterActivityRefreshScore.SetData(mapField, new List<ItemData> { itemData });
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_ChapterActivity_RefreshScore, eventArgsChapterActivityRefreshScore);
			}
		}

		[GameTestMethod("主线活动", "测试飞积分", "", 412)]
		private static void TestFlyItem()
		{
			FlyItemData flyItemData = new FlyItemData(FlyItemOtherType.ActivityScoreNormal, 0L, 100L, 5L, 1);
			List<FlyItemData> list = new List<FlyItemData>();
			list.Add(flyItemData);
			GameApp.View.FlyItemDatas(FlyItemModel.Battle, list, null, null);
		}

		public Transform child;

		public Slider sliderProgress;

		public CustomText textProgress;

		public CustomImage imageScoreIcon;

		public CanvasGroup rewardCanvas;

		public CustomImage imageRewardIcon;

		public CustomText imageRewardNum;

		public CustomButton buttonScore;

		public CustomButton buttonReward;

		public ParticleSystem rewardEffect;

		public CustomButton buttonPreview;

		private ChapterActivityData activityData;

		private bool isPlayAnimation;
	}
}
