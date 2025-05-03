using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine.Events;

namespace HotFix
{
	public class EquipMergeAnimationViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.mButton_Close.OnClose = new Action(this.OnSpineEndCallback);
			this.mSpineUI.OnSpecialTriggerCallback = new Action(this.OnSpecialTriggerCallback);
			this.mSpineUI.OnSpineEndCallback = new Action(this.OnSpineEndCallback);
			this.mSpineUI.Init();
		}

		public override void OnOpen(object data)
		{
			this.canClose = false;
			EquipMergeAnimationViewModule.OpenData openData = data as EquipMergeAnimationViewModule.OpenData;
			if (openData != null)
			{
				this.mOpenData = openData;
			}
			float num = 1f;
			if (this.mOpenData != null)
			{
				this.mCallback = this.mOpenData.Callback;
				num = this.mOpenData.PlaySpeedScale;
				if (num <= 0f)
				{
					num = 1f;
				}
			}
			this.mSpineUI.Play("Attack", num);
			GameApp.Sound.PlayClip(629, 1f);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.mSpineUI.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			CustomButton customButton = this.mButton_Mask;
			if (customButton != null)
			{
				customButton.onClick.RemoveListener(new UnityAction(this.OnSpineEndCallback));
			}
			this.mButton_Mask = null;
			this.mButton_Close.OnClose = null;
			this.mButton_Close = null;
			EquipMergeAnimationSpine equipMergeAnimationSpine = this.mSpineUI;
			if (equipMergeAnimationSpine != null)
			{
				equipMergeAnimationSpine.DeInit();
			}
			this.mSpineUI = null;
			this.mOpenData = null;
			this.mCallback = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnSpineEndCallback()
		{
			if (!this.canClose)
			{
				return;
			}
			GameApp.View.CloseView(ViewName.EquipMergeAnimationViewModule, null);
		}

		private void OnSpecialTriggerCallback()
		{
			this.canClose = true;
			Action action = this.mCallback;
			if (action != null)
			{
				action();
			}
			this.mCallback = null;
		}

		public CustomButton mButton_Mask;

		public TapToCloseCtrl mButton_Close;

		public EquipMergeAnimationSpine mSpineUI;

		private EquipMergeAnimationViewModule.OpenData mOpenData;

		private Action mCallback;

		private bool canClose;

		public class OpenData
		{
			public float PlaySpeedScale = 1f;

			public Action Callback;
		}
	}
}
