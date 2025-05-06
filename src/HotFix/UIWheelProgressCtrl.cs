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
	public class UIWheelProgressCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.buttonPreview.onClick.AddListener(new UnityAction(this.OnClickPreview));
			this.progressItemLeft.Init();
			this.progressItemRight.Init();
		}

		protected override void OnDeInit()
		{
			this.buttonPreview.onClick.RemoveListener(new UnityAction(this.OnClickPreview));
			this.progressItemLeft.DeInit();
			this.progressItemRight.DeInit();
		}

		public void SetData(int totalScore, List<CommonFundUIData> list, Action aniFinish)
		{
			this.uiDataList = list;
			this.onAllFinish = aniFinish;
			this.SetStatus(totalScore);
		}

		private void SetStatus(int totalScore)
		{
			CommonFundUIData stageData = CommonFundTools.GetStageData(this.uiDataList, totalScore);
			if (stageData == null)
			{
				return;
			}
			bool flag = this.IsAllFinish(totalScore);
			int num = CommonFundTools.GetStageScore(this.uiDataList, totalScore, 0);
			num = ((num < 0) ? 0 : num);
			int num2 = stageData.Score - stageData.PreviousScore;
			if (stageData.FreeRewards.Count > 1)
			{
				this.progressItemLeft.gameObject.SetActiveSafe(true);
				this.progressItemRight.gameObject.SetActiveSafe(true);
				this.plusObj.SetActiveSafe(true);
				this.progressItemLeft.SetData(new List<ItemData> { stageData.FreeRewards[0] });
				this.progressItemRight.SetData(new List<ItemData> { stageData.FreeRewards[1] });
			}
			else if (stageData.FreeRewards.Count > 0)
			{
				this.progressItemLeft.gameObject.SetActiveSafe(false);
				this.plusObj.SetActiveSafe(false);
				this.progressItemRight.gameObject.SetActiveSafe(true);
				this.progressItemRight.SetData(new List<ItemData> { stageData.FreeRewards[0] });
			}
			this.sliderProgress.value = (flag ? 1f : Utility.Math.Clamp01((float)num / (float)num2));
			this.textProgress.text = (flag ? Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_finish") : string.Format("{0}/{1}", num, num2));
			int num3 = this.uiDataList.IndexOf(stageData);
			int num4 = (flag ? this.uiDataList.Count : num3);
			this.textFinishProgress.text = string.Format("{0}/{1}", num4, this.uiDataList.Count);
		}

		private bool IsAllFinish(int totalScore)
		{
			bool flag = false;
			CommonFundUIData lastNormalData = CommonFundTools.GetLastNormalData(this.uiDataList);
			if (lastNormalData != null && totalScore >= lastNormalData.Score)
			{
				flag = true;
			}
			return flag;
		}

		public void PlayAni(int oldScore, int newScore)
		{
			CommonFundUIData stageData = CommonFundTools.GetStageData(this.uiDataList, oldScore);
			CommonFundUIData stageData2 = CommonFundTools.GetStageData(this.uiDataList, newScore);
			if (stageData == null || stageData2 == null)
			{
				return;
			}
			UIWheelProgressCtrl.AniData aniData = new UIWheelProgressCtrl.AniData();
			aniData.oldTotalScore = oldScore;
			aniData.newTotalScore = newScore;
			aniData.startIndex = this.uiDataList.IndexOf(stageData);
			aniData.endIndex = this.uiDataList.IndexOf(stageData2);
			CommonFundUIData lastNormalData = CommonFundTools.GetLastNormalData(this.uiDataList);
			if (lastNormalData != null && newScore >= lastNormalData.Score)
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
			UIWheelProgressCtrl.AniData aniData = this.aniList[0];
			this.aniList.RemoveAt(0);
			float num = 0.5f;
			Sequence sequence = DOTween.Sequence();
			if (aniData.startIndex < 0 || aniData.endIndex < 0)
			{
				this.AnimationFinish();
				return;
			}
			int stageScore = CommonFundTools.GetStageScore(this.uiDataList, aniData.newTotalScore, 0);
			bool stageData = CommonFundTools.GetStageData(this.uiDataList, aniData.oldTotalScore) != null;
			CommonFundUIData stageData2 = CommonFundTools.GetStageData(this.uiDataList, aniData.newTotalScore);
			if (!stageData || stageData2 == null)
			{
				return;
			}
			int num2 = stageData2.Score - stageData2.PreviousScore;
			if (aniData.startIndex == aniData.endIndex)
			{
				int stageScore2 = CommonFundTools.GetStageScore(this.uiDataList, aniData.oldTotalScore, 0);
				if (aniData.isAllFinish)
				{
					TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(stageScore2, num2, num2, num));
				}
				else
				{
					TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(stageScore2, stageScore, num2, num));
				}
			}
			else
			{
				TweenCallback <>9__1;
				for (int i = aniData.startIndex; i <= aniData.endIndex; i++)
				{
					int index = i;
					CommonFundUIData commonFundUIData = this.uiDataList[index];
					if (index == aniData.endIndex)
					{
						TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(0, stageScore, num2, num));
						Sequence sequence2 = sequence;
						TweenCallback tweenCallback;
						if ((tweenCallback = <>9__1) == null)
						{
							tweenCallback = (<>9__1 = delegate
							{
								this.SetStatus(aniData.newTotalScore);
							});
						}
						TweenSettingsExtensions.AppendCallback(sequence2, tweenCallback);
						break;
					}
					int num3 = 0;
					if (index == aniData.startIndex)
					{
						num3 = CommonFundTools.GetStageScore(this.uiDataList, aniData.oldTotalScore, 0);
					}
					int numTo = commonFundUIData.Score - commonFundUIData.PreviousScore;
					TweenSettingsExtensions.Append(sequence, this.PlayProgressAni(num3, numTo, numTo, num));
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.child, 1.2f, 0.2f));
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						CommonFundUIData commonFundUIData2 = this.uiDataList[index + 1];
						int num4 = this.uiDataList.IndexOf(commonFundUIData2);
						this.textFinishProgress.text = string.Format("{0}/{1}", num4, this.uiDataList.Count);
						if (commonFundUIData2.FreeRewards.Count > 1)
						{
							this.progressItemLeft.gameObject.SetActiveSafe(true);
							this.progressItemRight.gameObject.SetActiveSafe(true);
							this.plusObj.SetActiveSafe(true);
							this.progressItemLeft.SetData(new List<ItemData> { commonFundUIData2.FreeRewards[0] });
							this.progressItemRight.SetData(new List<ItemData> { commonFundUIData2.FreeRewards[1] });
							return;
						}
						if (commonFundUIData2.FreeRewards.Count > 0)
						{
							this.progressItemLeft.gameObject.SetActiveSafe(false);
							this.progressItemRight.gameObject.SetActiveSafe(true);
							this.plusObj.SetActiveSafe(false);
							this.progressItemRight.SetData(new List<ItemData> { commonFundUIData2.FreeRewards[0] });
						}
					});
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.child, 1f, 0.1f));
					TweenSettingsExtensions.Join(sequence, this.progressItemLeft.DoCanvasFade());
					TweenSettingsExtensions.Join(sequence, this.progressItemRight.DoCanvasFade());
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						this.sliderProgress.value = 0f;
						Color color = this.textProgress.color;
						color.a = 0f;
						this.textProgress.color = color;
						this.textProgress.text = string.Format("{0}/{1}", 0, numTo);
					});
					TweenSettingsExtensions.Append(sequence, this.progressItemRight.DoCanvasFadeScale());
					TweenSettingsExtensions.Join(sequence, this.progressItemLeft.DoCanvasFadeScale());
					TweenSettingsExtensions.Append(sequence, this.progressItemRight.ShowEffect());
					TweenSettingsExtensions.Join(sequence, this.progressItemLeft.ShowEffect());
					TweenSettingsExtensions.Append(sequence, this.progressItemRight.DoCanvasScale());
					TweenSettingsExtensions.Join(sequence, this.progressItemLeft.DoCanvasScale());
					TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.textProgress, 1f, 0.2f));
				}
			}
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				if (aniData.isAllFinish)
				{
					this.sliderProgress.value = 1f;
					this.textProgress.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_finish");
					this.SetStatus(aniData.newTotalScore);
				}
				this.AnimationFinish();
			});
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
			Action action = this.onAllFinish;
			if (action == null)
			{
				return;
			}
			action();
		}

		private void OnClickPreview()
		{
			if (this.isPlaying || this.isSpinning)
			{
				return;
			}
			GameApp.View.OpenView(ViewName.ChapterActivityWheelPreviewViewModule, null, 1, null, null);
		}

		public void SetLock(bool isLock)
		{
			this.isSpinning = isLock;
			this.progressItemLeft.SetClickLock(isLock);
			this.progressItemRight.SetClickLock(isLock);
		}

		public Transform child;

		public CustomButton buttonPreview;

		public Slider sliderProgress;

		public CustomText textProgress;

		public CustomText textFinishProgress;

		public CommonProgressItem progressItemLeft;

		public CommonProgressItem progressItemRight;

		public GameObject plusObj;

		private Action onAllFinish;

		private List<CommonFundUIData> uiDataList;

		private List<UIWheelProgressCtrl.AniData> aniList = new List<UIWheelProgressCtrl.AniData>();

		private bool isPlaying;

		private bool isSpinning;

		public class AniData
		{
			public int startIndex;

			public int endIndex;

			public int oldTotalScore;

			public int newTotalScore;

			public bool isAllFinish;
		}
	}
}
