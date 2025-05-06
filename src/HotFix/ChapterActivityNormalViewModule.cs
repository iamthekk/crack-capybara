using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ChapterActivityNormalViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.chapterActivityData = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule);
			this.progressCtrl.Init();
			this.infoCtrl.Init();
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.chapterId = (int)data;
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.animationListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimationListen));
			ChapterActivityData activeActivityData = this.chapterActivityData.GetActiveActivityData(ChapterActivityKind.Normal);
			this.activityData = activeActivityData as ChapterActivityNormalData;
			if (this.activityData == null)
			{
				return;
			}
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.activityData.ActivityTitleId);
			this.progressCtrl.SetData(this.activityData, false);
			this.infoCtrl.OnShow();
			this.infoCtrl.ShowModel(this.activityData);
			this.LoadAsset();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
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
			this.buttonMask.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.animationListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimationListen));
			this.infoCtrl.OnHide();
		}

		public override void OnDelete()
		{
			this.progressCtrl.DeInit();
			this.infoCtrl.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.ChapterActivityNormalViewModule, null);
		}

		private Task LoadAsset()
		{
			ChapterActivityNormalViewModule.<LoadAsset>d__23 <LoadAsset>d__;
			<LoadAsset>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadAsset>d__.<>4__this = this;
			<LoadAsset>d__.<>1__state = -1;
			<LoadAsset>d__.<>t__builder.Start<ChapterActivityNormalViewModule.<LoadAsset>d__23>(ref <LoadAsset>d__);
			return <LoadAsset>d__.<>t__builder.Task;
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

		public CustomButton buttonMask;

		public CustomImage imageMap;

		public CustomImage imageTitle;

		public CustomImage imageLight1;

		public CustomImage imageLight2;

		public GameObject starParent;

		public CustomText textTitle;

		public CustomText textTime;

		public CustomButton buttonClose;

		public UIChapterActivityProgressCtrl progressCtrl;

		public UIChapterActivityInfoCtrl infoCtrl;

		public AnimatorListen animationListen;

		private ChapterActivityDataModule chapterActivityData;

		private int chapterId;

		private ChapterActivityNormalData activityData;
	}
}
