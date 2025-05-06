using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIActivityController : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.buttonActivity.onClick.AddListener(new UnityAction(this.OnClickActivity));
			this.activityProgressCtrl.Init();
			this.buttonActivity.gameObject.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			this.buttonActivity.onClick.RemoveListener(new UnityAction(this.OnClickActivity));
			this.activityProgressCtrl.DeInit();
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
			if (this.activityData.IsEnd())
			{
				this.isEnd = true;
				this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_chapter_activity_end");
				EventArgsInt eventArgsInt = new EventArgsInt();
				eventArgsInt.SetData((int)this.activityData.Kind);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_ChapterActivity_CheckShow, eventArgsInt);
				return;
			}
			long num = ChapterActivityDataModule.ServerTime();
			this.lastTime += deltaTime;
			if (this.textTime.gameObject.activeInHierarchy && this.lastTime > 1f)
			{
				this.lastTime -= 1f;
				this.textTime.text = Singleton<LanguageManager>.Instance.GetTime(this.activityData.EndTime - num);
			}
		}

		public void SetChapterId(int id)
		{
			this.chapterId = id;
			this.LoadBg();
		}

		private void OnClickActivity()
		{
			if (this.isClickDisabled)
			{
				return;
			}
			GameApp.View.OpenView(ViewName.ChapterActivityNormalViewModule, this.chapterId, 1, null, null);
		}

		public void CheckShow(bool isPlayAni)
		{
			this.isEnd = false;
			ChapterActivityDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule);
			this.activityData = dataModule.GetActiveActivityData(ChapterActivityKind.Normal);
			if (this.activityData != null)
			{
				this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.activityData.ActivityTitleId);
				this.buttonActivity.gameObject.SetActiveSafe(true);
				this.activityProgressCtrl.SetData(this.activityData, isPlayAni);
				return;
			}
			this.buttonActivity.gameObject.SetActiveSafe(false);
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
			this.child.anchoredPosition = new Vector2(this.child.anchoredPosition.x, 300f);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.child, -20f, 0.3f, false)), ShortcutExtensions46.DOAnchorPosY(this.child, 0f, 0.2f, false));
		}

		private void PlayHideAni()
		{
			Sequence sequence = DOTween.Sequence();
			this.child.anchoredPosition = Vector2.zero;
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.child, -20f, 0.2f, false)), ShortcutExtensions46.DOAnchorPosY(this.child, 300f, 0.3f, false));
		}

		public Transform GetFlyNode()
		{
			return this.flyNode;
		}

		private Task LoadBg()
		{
			UIActivityController.<LoadBg>d__23 <LoadBg>d__;
			<LoadBg>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadBg>d__.<>4__this = this;
			<LoadBg>d__.<>1__state = -1;
			<LoadBg>d__.<>t__builder.Start<UIActivityController.<LoadBg>d__23>(ref <LoadBg>d__);
			return <LoadBg>d__.<>t__builder.Task;
		}

		public void SetClickDisabled(bool isDisabled)
		{
			this.isClickDisabled = isDisabled;
		}

		public RectTransform child;

		public CustomButton buttonActivity;

		public UIChapterActivityProgressCtrl activityProgressCtrl;

		public CustomText textTitle;

		public CustomText textTime;

		public RectTransform flyNode;

		public CustomImage imageBg;

		private ChapterActivityData activityData;

		private bool isClickDisabled;

		private int chapterId;

		private bool isEnd;

		private float lastTime;

		public enum UIMode
		{
			ActivityNormal,
			MainCity,
			Chapter,
			Sweep
		}
	}
}
