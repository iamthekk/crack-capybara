using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class CommonProgressCtrl : CustomBehaviour
	{
		[HideInInspector]
		public bool isAnimationDisabled { get; private set; }

		protected override void OnInit()
		{
			this.buttonScore.onClick.AddListener(new UnityAction(this.OnClickScore));
		}

		protected override void OnDeInit()
		{
			this.buttonScore.onClick.RemoveListener(new UnityAction(this.OnClickScore));
			this.aniList.Clear();
		}

		public void SetAnimationDisabled(bool isDisabled)
		{
			this.isAnimationDisabled = isDisabled;
		}

		public void SetData(int scoreAtlasId, string scoreIcon, List<CommonFundUIData> list, Action onClickScore, Action onAniFinish)
		{
			this.uiDataList = list;
			this.OnScore = onClickScore;
			this.OnAnimationFinish = onAniFinish;
			this.aniList.Clear();
			string atlasPath = GameApp.Table.GetAtlasPath(scoreAtlasId);
			this.imageScoreIcon.SetImage(atlasPath, scoreIcon);
		}

		public void SetStatus(int totalScore, int finalGetCount, int finalLimit)
		{
			this.FinalGetCount = finalGetCount;
			this.FinalLimit = finalLimit;
			CommonFundUIData stageData = CommonFundTools.GetStageData(this.uiDataList, totalScore);
			if (stageData == null)
			{
				return;
			}
			this.normalNode.SetActiveSafe(!stageData.IsLoopReward);
			this.finalNode.SetActiveSafe(stageData.IsLoopReward);
			bool flag = finalGetCount == finalLimit;
			int num;
			int num2;
			if (stageData.IsLoopReward)
			{
				int loopStartScore = CommonFundTools.GetLoopStartScore(this.uiDataList);
				num = (totalScore - loopStartScore) % stageData.Score;
				num2 = stageData.Score;
				this.finalItem.SetData(finalGetCount, finalLimit);
				if (totalScore >= loopStartScore + stageData.Score * finalLimit)
				{
					flag = true;
				}
			}
			else
			{
				num = CommonFundTools.GetStageScore(this.uiDataList, totalScore, finalGetCount);
				num = ((num < 0) ? 0 : num);
				num2 = stageData.Score - stageData.PreviousScore;
				this.freeItem.SetData(stageData.FreeRewards);
				this.payItem.SetData(stageData.PayRewards);
			}
			this.sliderProgress.value = (flag ? 1f : Utility.Math.Clamp01((float)num / (float)num2));
			this.textProgress.text = (flag ? Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_finish") : string.Format("{0}/{1}", num, num2));
		}

		private bool IsAllFinish(int totalScore)
		{
			bool flag = this.FinalGetCount == this.FinalLimit;
			CommonFundUIData finalLoopData = CommonFundTools.GetFinalLoopData(this.uiDataList);
			if (finalLoopData != null)
			{
				int loopStartScore = CommonFundTools.GetLoopStartScore(this.uiDataList);
				if (totalScore >= loopStartScore + finalLoopData.Score * this.FinalLimit)
				{
					flag = true;
				}
			}
			return flag;
		}

		public void PlayAnimation(int oldScore, int newScore)
		{
			if (this.isAnimationDisabled)
			{
				return;
			}
			if (this.IsAllFinish(newScore))
			{
				if (this.aniList.Count == 0)
				{
					this.AnimationFinish();
				}
				return;
			}
			CommonFundUIData stageData = CommonFundTools.GetStageData(this.uiDataList, oldScore);
			CommonFundUIData stageData2 = CommonFundTools.GetStageData(this.uiDataList, newScore);
			if (stageData == null || stageData2 == null)
			{
				return;
			}
			int loopStartScore = CommonFundTools.GetLoopStartScore(this.uiDataList);
			int num = 0;
			int num2 = 0;
			if (oldScore > loopStartScore && stageData.IsLoopReward)
			{
				num = (oldScore - loopStartScore) / stageData.Score;
			}
			if (newScore > loopStartScore && stageData2.IsLoopReward)
			{
				num2 = (newScore - loopStartScore) / stageData2.Score;
				if (num2 > this.FinalLimit)
				{
					num2 = this.FinalLimit;
				}
			}
			CommonProgressCtrl.AniData aniData = new CommonProgressCtrl.AniData();
			aniData.oldTotalScore = oldScore;
			aniData.newTotalScore = newScore;
			if (stageData.IsLoopReward)
			{
				aniData.startIndex = -1;
				aniData.endIndex = -1;
				aniData.loopNum = num2 - num;
				aniData.loopOldScore = (oldScore - loopStartScore) % stageData.Score;
				aniData.loopScore = (newScore - loopStartScore) % stageData.Score;
			}
			else if (stageData2.IsLoopReward)
			{
				CommonFundUIData lastNormalData = CommonFundTools.GetLastNormalData(this.uiDataList);
				if (lastNormalData == null)
				{
					return;
				}
				aniData.startIndex = this.uiDataList.IndexOf(stageData);
				aniData.endIndex = this.uiDataList.IndexOf(lastNormalData);
				aniData.loopNum = num2 - num;
				aniData.loopOldScore = 0;
				aniData.loopScore = (newScore - loopStartScore) % stageData2.Score;
			}
			else
			{
				aniData.startIndex = this.uiDataList.IndexOf(stageData);
				aniData.endIndex = this.uiDataList.IndexOf(stageData2);
				aniData.loopNum = 0;
				aniData.loopScore = 0;
				aniData.loopOldScore = 0;
			}
			CommonFundUIData finalLoopData = CommonFundTools.GetFinalLoopData(this.uiDataList);
			if (finalLoopData != null && newScore >= loopStartScore + finalLoopData.Score * this.FinalLimit)
			{
				aniData.isAllFinish = true;
			}
			this.aniList.Add(aniData);
			this.DoPlayAnimation();
		}

		private void DoPlayAnimation()
		{
			if (this.isPlaying)
			{
				return;
			}
			if (this.aniList.Count == 0)
			{
				return;
			}
			CommonProgressCtrl.AniData aniData = this.aniList[0];
			this.aniList.RemoveAt(0);
			float num = 0.5f;
			Sequence sequence = DOTween.Sequence();
			bool flag = false;
			bool flag2 = false;
			if (aniData.startIndex >= 0 && aniData.endIndex >= 0)
			{
				flag = aniData.loopNum > 0 || aniData.loopScore > 0;
				int num2 = CommonFundTools.GetStageScore(this.uiDataList, aniData.newTotalScore, this.FinalGetCount);
				CommonFundUIData stageData = CommonFundTools.GetStageData(this.uiDataList, aniData.oldTotalScore);
				CommonFundUIData stageData2 = CommonFundTools.GetStageData(this.uiDataList, aniData.newTotalScore);
				if (stageData == null || stageData2 == null)
				{
					return;
				}
				int num3 = stageData2.Score - stageData2.PreviousScore;
				if (aniData.startIndex == aniData.endIndex)
				{
					int stageScore = CommonFundTools.GetStageScore(this.uiDataList, aniData.oldTotalScore, this.FinalGetCount);
					if (!stageData.IsLoopReward && stageData2.IsLoopReward)
					{
						num2 = stageData.Score - stageData.PreviousScore;
						num3 = num2;
					}
					TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(stageScore, num2, num3, num));
					if (!flag)
					{
						TweenSettingsExtensions.AppendInterval(sequence, 1f);
						TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.AnimationFinish));
					}
				}
				else
				{
					int i = aniData.startIndex;
					TweenCallback <>9__2;
					while (i <= aniData.endIndex)
					{
						int index = i;
						CommonFundUIData commonFundUIData = this.uiDataList[index];
						if (index == aniData.endIndex)
						{
							if (!stageData.IsLoopReward && stageData2.IsLoopReward)
							{
								num2 = stageData.Score - stageData.PreviousScore;
								num3 = num2;
							}
							TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(0, num2, num3, num));
							if (!flag)
							{
								Sequence sequence2 = sequence;
								TweenCallback tweenCallback;
								if ((tweenCallback = <>9__2) == null)
								{
									tweenCallback = (<>9__2 = delegate
									{
										this.SetStatus(aniData.newTotalScore, this.FinalGetCount, this.FinalLimit);
									});
								}
								TweenSettingsExtensions.AppendCallback(sequence2, tweenCallback);
								break;
							}
							break;
						}
						else
						{
							int num4 = 0;
							if (index == aniData.startIndex)
							{
								num4 = CommonFundTools.GetStageScore(this.uiDataList, aniData.oldTotalScore, this.FinalGetCount);
							}
							int numTo = commonFundUIData.Score - commonFundUIData.PreviousScore;
							TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(num4, numTo, numTo, num));
							TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.child, 1.2f, 0.2f));
							TweenSettingsExtensions.AppendCallback(sequence, delegate
							{
								CommonFundUIData commonFundUIData2 = this.uiDataList[index + 1];
								if (commonFundUIData2.IsLoopReward)
								{
									this.normalNode.SetActiveSafe(false);
									this.finalNode.SetActiveSafe(true);
									this.finalItem.SetData(this.FinalGetCount, this.FinalLimit);
									return;
								}
								this.normalNode.SetActiveSafe(true);
								this.finalNode.SetActiveSafe(false);
								this.freeItem.SetData(commonFundUIData2.FreeRewards);
								this.payItem.SetData(commonFundUIData2.PayRewards);
							});
							TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.child, 1f, 0.1f));
							if (this.normalNode.activeSelf)
							{
								TweenSettingsExtensions.Join(sequence, this.freeItem.DoCanvasFade());
								TweenSettingsExtensions.Join(sequence, this.payItem.DoCanvasFade());
							}
							else if (this.finalNode.activeSelf)
							{
								TweenSettingsExtensions.Join(sequence, this.finalItem.DoCanvasFade());
							}
							TweenSettingsExtensions.AppendCallback(sequence, delegate
							{
								this.sliderProgress.value = 0f;
								Color color = this.textProgress.color;
								color.a = 0f;
								this.textProgress.color = color;
								this.textProgress.text = string.Format("{0}/{1}", 0, numTo);
							});
							if (this.normalNode.activeSelf)
							{
								TweenSettingsExtensions.Append(sequence, this.freeItem.DoCanvasFadeScale());
								TweenSettingsExtensions.Join(sequence, this.payItem.DoCanvasFadeScale());
								TweenSettingsExtensions.Append(sequence, this.freeItem.ShowEffect());
								TweenSettingsExtensions.Join(sequence, this.payItem.ShowEffect());
								TweenSettingsExtensions.Append(sequence, this.freeItem.DoCanvasScale());
								TweenSettingsExtensions.Join(sequence, this.payItem.DoCanvasScale());
							}
							else if (this.finalNode.activeSelf)
							{
								TweenSettingsExtensions.Append(sequence, this.finalItem.DoCanvasFadeScale());
								TweenSettingsExtensions.Append(sequence, this.finalItem.ShowEffect());
								TweenSettingsExtensions.Append(sequence, this.finalItem.DoCanvasScale());
							}
							TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.textProgress, 1f, 0.2f));
							i++;
						}
					}
					if (!flag)
					{
						TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.AnimationFinish));
					}
				}
			}
			else
			{
				flag2 = true;
			}
			bool lastNormalData = CommonFundTools.GetLastNormalData(this.uiDataList) != null;
			CommonFundUIData final = CommonFundTools.GetFinalLoopData(this.uiDataList);
			if (!lastNormalData || final == null)
			{
				TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.AnimationFinish));
				return;
			}
			if (flag)
			{
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.normalNode.SetActiveSafe(false);
					this.finalNode.SetActiveSafe(true);
					this.sliderProgress.value = 0f;
					this.textProgress.text = string.Format("{0}/{1}", 0, final.Score);
				});
			}
			if (flag || flag2)
			{
				for (int j = 0; j < aniData.loopNum; j++)
				{
					TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(0, final.Score, final.Score, num));
					TweenSettingsExtensions.Join(sequence, this.finalItem.DoCanvasFade());
					TweenSettingsExtensions.Append(sequence, this.finalItem.DoCanvasFadeScale());
					TweenSettingsExtensions.Append(sequence, this.finalItem.ShowEffect());
					TweenSettingsExtensions.Append(sequence, this.finalItem.DoCanvasScale());
				}
				if (aniData.isAllFinish)
				{
					TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(0, final.Score, final.Score, num));
				}
				else if (aniData.loopNum > 0 && aniData.loopScore > 0)
				{
					TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(0, aniData.loopScore, final.Score, num));
				}
				else
				{
					TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(aniData.loopOldScore, aniData.loopScore, final.Score, num));
				}
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					if (aniData.isAllFinish)
					{
						this.sliderProgress.value = 1f;
						this.textProgress.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_finish");
					}
					this.AnimationFinish();
				});
			}
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

		private void AnimationFinish()
		{
			if (this.aniList.Count > 0)
			{
				this.DoPlayAnimation();
				return;
			}
			Action onAnimationFinish = this.OnAnimationFinish;
			if (onAnimationFinish == null)
			{
				return;
			}
			onAnimationFinish();
		}

		public void OnClickScore()
		{
			Action onScore = this.OnScore;
			if (onScore == null)
			{
				return;
			}
			onScore();
		}

		public Transform child;

		public Slider sliderProgress;

		public CustomText textProgress;

		public CustomImage imageScoreIcon;

		public CustomButton buttonScore;

		public CommonProgressItem freeItem;

		public CommonProgressItem payItem;

		public CommonProgressFinalItem finalItem;

		public GameObject normalNode;

		public GameObject finalNode;

		private Action OnScore;

		private Action OnAnimationFinish;

		private List<CommonFundUIData> uiDataList;

		private int FinalGetCount;

		private int FinalLimit;

		private List<CommonProgressCtrl.AniData> aniList = new List<CommonProgressCtrl.AniData>();

		private bool isPlaying;

		public class AniData
		{
			public int startIndex;

			public int endIndex;

			public int loopNum;

			public int loopOldScore;

			public int loopScore;

			public int oldTotalScore;

			public int newTotalScore;

			public bool isAllFinish;
		}
	}
}
