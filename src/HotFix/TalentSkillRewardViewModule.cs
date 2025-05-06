using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class TalentSkillRewardViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.canClose = false;
			this.openData = data as TalentSkillRewardViewModule.OpenData;
			if (this.openData == null)
			{
				GameApp.View.CloseView(ViewName.TalentSkillRewardViewModule, null);
				return;
			}
			GameApp.Sound.PlayClip(647, 1f);
			this.UpdateView();
			this.uiAnimator.Play("Show");
			float animationLength = DxxTools.Animator.GetAnimationLength(this.uiAnimator, "Show");
			DelayCall.Instance.CallOnce((int)(animationLength * 1000f), delegate
			{
				this.canClose = true;
			});
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			Action closeCallback = this.openData.closeCallback;
			if (closeCallback == null)
			{
				return;
			}
			closeCallback();
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.btnClose.m_onClick = new Action(this.OnBtnCloseClick);
			this.btnImgMask.m_onClick = new Action(this.OnBtnCloseClick);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.btnClose.m_onClick = null;
			this.btnImgMask.m_onClick = null;
		}

		private void UpdateView()
		{
			this.imgSkillIcon.SetImage(this.openData.atlasId, this.openData.iconName);
			this.txtSkillName.text = this.openData.title;
			this.txtSkillDesc.text = this.openData.desc;
		}

		private void OnBtnCloseClick()
		{
			if (!this.canClose)
			{
				return;
			}
			GameApp.View.CloseView(ViewName.TalentSkillRewardViewModule, null);
		}

		public Animator uiAnimator;

		public CustomButton btnClose;

		public CustomButton btnImgMask;

		public CustomImage imgSkillIcon;

		public CustomText txtSkillName;

		public CustomText txtSkillDesc;

		private TalentSkillRewardViewModule.OpenData openData;

		private bool canClose;

		public class OpenData
		{
			public int atlasId;

			public string iconName;

			public string title;

			public string desc;

			public Action closeCallback;
		}
	}
}
