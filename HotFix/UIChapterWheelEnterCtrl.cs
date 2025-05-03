using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIChapterWheelEnterCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.dataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule);
			this.buttonAct.onClick.AddListener(new UnityAction(this.OnClickButton));
		}

		protected override void OnDeInit()
		{
			this.buttonAct.onClick.RemoveListener(new UnityAction(this.OnClickButton));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.dataModule == null)
			{
				return;
			}
			if (this.dataModule.WheelInfo == null)
			{
				return;
			}
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			long num = this.dataModule.WheelInfo.EndTime - serverTimestamp;
			if (num > 0L)
			{
				this.textTime.text = Singleton<LanguageManager>.Instance.GetTime(num);
				return;
			}
			this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_end");
		}

		public void Show()
		{
			this.child.gameObject.SetActiveSafe(true);
		}

		public void Hide()
		{
			this.child.gameObject.SetActiveSafe(false);
		}

		public void SetData(Action clickBtn, bool isPlayAni)
		{
			this.onClickSelf = clickBtn;
			this.playAniEnabled = isPlayAni;
			if (this.dataModule.WheelInfo == null)
			{
				this.Hide();
				return;
			}
			this.textScore.text = ((this.dataModule.WheelInfo.Score == 0) ? "" : this.dataModule.WheelInfo.Score.ToString());
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

		public void PlayAnimation(int oldScore, int newScore)
		{
			if (!this.playAniEnabled)
			{
				return;
			}
			UIChapterWheelEnterCtrl.AniData aniData = new UIChapterWheelEnterCtrl.AniData();
			aniData.oldScore = oldScore;
			aniData.newScore = newScore;
			this.aniList.Add(aniData);
			if (!this.isPlayAnim)
			{
				this.DoPlayAnim();
			}
		}

		private void DoPlayAnim()
		{
			if (this.aniList.Count > 0)
			{
				UIChapterWheelEnterCtrl.AniData aniData = this.aniList[0];
				this.aniList.RemoveAt(0);
				float num = 1f;
				Sequence sequence = DOTween.Sequence();
				TweenSettingsExtensions.Append(sequence, this.RefreshScoreAni(aniData.oldScore, aniData.newScore, num));
				TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.AnimationFinish));
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

		private void AnimationFinish()
		{
			if (this.aniList.Count > 0)
			{
				this.DoPlayAnim();
				return;
			}
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.AppendInterval(sequence, 1f);
			TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.PlayHideAni));
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

		private void OnClickButton()
		{
			Action action = this.onClickSelf;
			if (action == null)
			{
				return;
			}
			action();
		}

		public RectTransform child;

		public CustomButton buttonAct;

		public CustomText textScore;

		public CustomText textTime;

		public Transform flyNode;

		private ChapterActivityWheelDataModule dataModule;

		private Action onClickSelf;

		private bool playAniEnabled;

		private List<UIChapterWheelEnterCtrl.AniData> aniList = new List<UIChapterWheelEnterCtrl.AniData>();

		private bool isPlayAnim;

		private bool isShowAni;

		public class AniData
		{
			public int oldScore;

			public int newScore;
		}
	}
}
