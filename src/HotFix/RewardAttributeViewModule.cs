using System;
using System.Collections;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class RewardAttributeViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.openData = data as RewardAttributeViewModule.OpenData;
			if (this.openData == null)
			{
				HLog.LogError("RewardAttributeViewModule OnOpen data is null");
				return;
			}
			this.UpdateView();
			base.StartCoroutine(this.AutoCloseTrigger());
		}

		private IEnumerator AutoCloseTrigger()
		{
			if (this.openData.isInSweep)
			{
				yield return new WaitForSeconds(this.openData.autoCloseTime);
				this.OnBtnCloseClick();
			}
			yield break;
		}

		private void UpdateView()
		{
			AttributeTypeData attParam = NodeAttParam.GetAttParam(this.openData.nodeAttParam);
			this.textAttribute.text = attParam.m_value;
			attParam.SetImage(this.imgAttributeIcon);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.textAttribute.transform.parent as RectTransform);
			if (this.openData.nodeAttParam.FinalCount > 0.0)
			{
				this.imageTitle.sprite = this.spriteTitleGood;
				this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("minigame_reward_title_good");
				return;
			}
			this.imageTitle.sprite = this.spriteTitleBad;
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("minigame_reward_title_bad");
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			RewardAttributeViewModule.OpenData openData = this.openData;
			if (openData == null)
			{
				return;
			}
			Action viewCloseCallback = openData.viewCloseCallback;
			if (viewCloseCallback == null)
			{
				return;
			}
			viewCloseCallback();
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.btnBg.onClick.AddListener(new UnityAction(this.OnBtnCloseClick));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.btnBg.onClick.RemoveListener(new UnityAction(this.OnBtnCloseClick));
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.RewardAttributeViewModule, null);
		}

		public Animator viewAnimator;

		public CustomButton btnBg;

		public CustomImage imgAttributeIcon;

		public CustomText textAttribute;

		public Sprite spriteTitleGood;

		public Sprite spriteTitleBad;

		public Image imageTitle;

		public CustomText textTitle;

		private RewardAttributeViewModule.OpenData openData;

		public class OpenData
		{
			public NodeAttParam nodeAttParam;

			public Action viewCloseCallback;

			public bool isInSweep;

			public float autoCloseTime;
		}
	}
}
