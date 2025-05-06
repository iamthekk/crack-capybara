using System;
using Framework;
using Framework.EventSystem;
using Framework.ViewModule;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class BattleFightViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.spineAnimator = new SpineAnimator(this.m_skeletonAnimation);
		}

		public override void OnOpen(object data)
		{
			BattleFightViewModule.OpenData openData = data as BattleFightViewModule.OpenData;
			if (openData != null)
			{
				this.openData = openData;
			}
			if (this.openData != null)
			{
				this.spinTrans.anchoredPosition = new Vector2(this.spinTrans.anchoredPosition.x, this.openData.spinOffsetY);
			}
			this.spineAnimator.PlayAni("Fight", false, new AnimationState.TrackEntryEventDelegate(this.HandleEvent), delegate(TrackEntry trackEntry)
			{
				if (this.openData != null)
				{
					Action aniFinish = this.openData.aniFinish;
					if (aniFinish != null)
					{
						aniFinish();
					}
				}
				GameApp.View.CloseView(ViewName.BattleFightViewModule, null);
			});
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		private void HandleEvent(TrackEntry trackEntry, Event e)
		{
			GameApp.Sound.PlayClip(55, 1f);
		}

		[SerializeField]
		private SkeletonGraphic m_skeletonAnimation;

		[SerializeField]
		private RectTransform spinTrans;

		private SpineAnimator spineAnimator;

		private BattleFightViewModule.OpenData openData;

		public class OpenData
		{
			public Action aniFinish;

			public float spinOffsetY;
		}
	}
}
