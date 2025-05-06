using System;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using HotFix.VIPUI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class VIPLevelUpViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.mVIPDataModule = GameApp.Data.GetDataModule(DataName.VIPDataModule);
			this.BenefitsUI.Init();
			this.Listener.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			this.ButtonGotoVIP.m_onClick = new Action(this.OnGotoVIPView);
			this.ButtonMask.m_onClick = new Action(this.OnClickCloseThis);
			this.TabToClose.OnClose = new Action(this.OnClickCloseThis);
			this.TabToClose.gameObject.SetActive(false);
			this.ObjGotoVIP.SetActive(false);
		}

		public override void OnOpen(object data)
		{
			VIPLevelUpViewModule.OpenData openData = data as VIPLevelUpViewModule.OpenData;
			if (openData != null)
			{
				this.mOpenData = openData;
			}
			if (this.mOpenData == null)
			{
				this.mOpenData = new VIPLevelUpViewModule.OpenData();
				this.mOpenData.VIPLevelOld = this.mVIPDataModule.VipLevel - 1;
				this.mOpenData.VIPLevelNew = this.mVIPDataModule.VipLevel;
			}
			this.m_seqPool.Clear(false);
			this.BenefitsUI.SetData(this.mVIPDataModule.GetVIPDatas(this.mOpenData.VIPLevelNew));
			this.TabToClose.gameObject.SetActive(false);
			this.ObjGotoVIP.SetActive(false);
			this.RefreshUI();
			this.Animator.Play("open");
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_seqPool.Clear(false);
		}

		public override void OnDelete()
		{
			this.Listener.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			this.BenefitsUI.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnAnimatorListen(GameObject obj, string str)
		{
			if (str == "opened")
			{
				this.BenefitsUI.RefreshUI(new Action(this.OnBenefitsItemPlayOver));
			}
		}

		private void OnBenefitsItemPlayOver()
		{
			if (this.ObjGotoVIP != null)
			{
				Sequence sequence = this.m_seqPool.Get();
				this.ObjGotoVIP.SetActive(true);
				this.ObjGotoVIP.transform.localScale = Vector3.zero;
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.ObjGotoVIP.transform, 1f, 0.2f));
			}
			if (this.TabToClose != null)
			{
				Sequence sequence2 = this.m_seqPool.Get();
				this.TabToClose.gameObject.SetActive(true);
				this.TabToClose.transform.localScale = Vector3.zero;
				TweenSettingsExtensions.AppendInterval(sequence2, 0.1f);
				TweenSettingsExtensions.Append(sequence2, ShortcutExtensions.DOScale(this.TabToClose.transform, 1f, 0.2f));
			}
		}

		private void RefreshUI()
		{
			this.TextLevelOld.text = Singleton<LanguageManager>.Instance.GetInfoByID("8804", new object[] { this.mOpenData.VIPLevelOld.ToString() });
			this.TextLevelNew.text = Singleton<LanguageManager>.Instance.GetInfoByID("8804", new object[] { this.mOpenData.VIPLevelNew.ToString() });
			this.TextTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("8808");
			this.TitleLayout.RefreshUI();
		}

		private void OnClickCloseThis()
		{
			GameApp.View.CloseView(ViewName.VIPLevelUpViewModule, null);
		}

		private void OnGotoVIPView()
		{
			if (!GameApp.View.IsOpened(ViewName.VIPViewModule))
			{
				VIPViewModule.OpenData openData = new VIPViewModule.OpenData();
				openData.VIPLevel = this.mOpenData.VIPLevelNew;
				GameApp.View.OpenView(ViewName.VIPViewModule, openData, 1, null, null);
			}
			GameApp.View.CloseView(ViewName.VIPLevelUpViewModule, null);
		}

		[GameTestMethod]
		private static void OnTest()
		{
			VIPLevelUpViewModule.OpenData openData = new VIPLevelUpViewModule.OpenData();
			openData.VIPLevelOld = 1;
			openData.VIPLevelNew = 5;
			GameApp.View.OpenView(ViewName.VIPLevelUpViewModule, openData, 1, null, null);
		}

		public CustomText TextTitle;

		public CustomText TextLevelOld;

		public CustomText TextLevelNew;

		public VIPRewardsTitleLayoutUI TitleLayout;

		public VIPLevelUpBenefitsUI BenefitsUI;

		public TapToCloseCtrl TabToClose;

		public CustomButton ButtonMask;

		public CustomButton ButtonGotoVIP;

		public Animator Animator;

		public AnimatorListen Listener;

		public GameObject ObjGotoVIP;

		private VIPLevelUpViewModule.OpenData mOpenData;

		private VIPDataModule mVIPDataModule;

		private SequencePool m_seqPool = new SequencePool();

		public class OpenData
		{
			public int VIPLevelOld;

			public int VIPLevelNew;
		}
	}
}
