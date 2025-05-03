using System;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class CloudLoadingViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.spineCloud.Init();
		}

		public override void OnOpen(object data)
		{
			if (data != null)
			{
				this.openData = data as CloudLoadingViewModule.OpenData;
			}
			CloudLoadingViewModule.OpenData openData = this.openData;
			float num = ((openData != null) ? openData.waitTime : 1f);
			float animationDuration = this.spineCloud.GetAnimationDuration("Start");
			float animationDuration2 = this.spineCloud.GetAnimationDuration("End");
			this.spineCloud.PlayAnimation("Start", false);
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.AppendInterval(sequence, animationDuration);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.spineCloud.PlayAnimation("Loop", true);
				CloudLoadingViewModule.OpenData openData2 = this.openData;
				if (openData2 == null)
				{
					return;
				}
				Action onCloudClose = openData2.onCloudClose;
				if (onCloudClose == null)
				{
					return;
				}
				onCloudClose();
			});
			TweenSettingsExtensions.AppendInterval(sequence, num);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.spineCloud.PlayAnimation("End", false);
			});
			TweenSettingsExtensions.AppendInterval(sequence, animationDuration2);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				CloudLoadingViewModule.OpenData openData3 = this.openData;
				if (openData3 != null)
				{
					Action onAnimFinish = openData3.onAnimFinish;
					if (onAnimFinish != null)
					{
						onAnimFinish();
					}
				}
				GameApp.View.CloseView(ViewName.CloudLoadingViewModule, null);
			});
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.spineCloud.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		[SerializeField]
		private UISpineModelItem spineCloud;

		private CloudLoadingViewModule.OpenData openData;

		public class OpenData
		{
			public float waitTime = 1f;

			public Action onCloudClose;

			public Action onAnimFinish;
		}
	}
}
