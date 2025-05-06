using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class CollectionViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			try
			{
				this.redNodeTabCollection.Value = 0;
				this.redNodeTabSuit.Value = 0;
				this.redNodeTabUpgradeable.Value = 0;
				this.moduleCurrencyCtrl.Init();
				List<int> list = new List<int>();
				list.Add(2);
				list.Add(1);
				list.Add(27);
				this.moduleCurrencyCtrl.SetStyle(EModuleId.Collection, list);
				this.tabGroup.CollectChildButtons();
				this.collectionMainPage.Init();
				this.collectionStarUpgradePage.Init();
				this.collectionSuitPage.Init();
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public override void OnOpen(object data)
		{
			this.tabGroup.ChooseButtonName(this.tabMain.name);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			if (this.curPage != this.collectionMainPage)
			{
				this.collectionMainPage.OnHide();
			}
			if (this.curPage != this.collectionStarUpgradePage)
			{
				this.collectionStarUpgradePage.OnHide();
			}
			if (this.curPage != this.collectionSuitPage)
			{
				this.collectionSuitPage.OnHide();
			}
			this.currentTab = null;
		}

		public override void OnDelete()
		{
			this.moduleCurrencyCtrl.DeInit();
			this.collectionMainPage.DeInit();
			this.collectionStarUpgradePage.DeInit();
			this.collectionSuitPage.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.btnBack.m_onClick = new Action(this.OnBtnCloseClick);
			this.tabGroup.OnSwitch = new Action<CustomChooseButton>(this.OnSwitchPage);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_CollectionViewModule_UpdateView, new HandlerEvent(this.OnEventUpdateView));
			RedPointController.Instance.RegRecordChange("Equip.Collection.Main", new Action<RedNodeListenData>(this.OnRedPointCollectionMainChange));
			RedPointController.Instance.RegRecordChange("Equip.Collection.Suit", new Action<RedNodeListenData>(this.OnRedPointCollectionSuitChange));
			RedPointController.Instance.RegRecordChange("Equip.Collection.StarUpgrade", new Action<RedNodeListenData>(this.OnRedPointCollectionStarUpgradeChange));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.btnBack.m_onClick = null;
			this.tabGroup.OnSwitch = null;
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_CollectionViewModule_UpdateView, new HandlerEvent(this.OnEventUpdateView));
			RedPointController.Instance.UnRegRecordChange("Equip.Collection.Main", new Action<RedNodeListenData>(this.OnRedPointCollectionMainChange));
			RedPointController.Instance.UnRegRecordChange("Equip.Collection.Suit", new Action<RedNodeListenData>(this.OnRedPointCollectionSuitChange));
			RedPointController.Instance.UnRegRecordChange("Equip.Collection.StarUpgrade", new Action<RedNodeListenData>(this.OnRedPointCollectionStarUpgradeChange));
		}

		private void PlayTweenPosition()
		{
			TweenSettingsExtensions.OnUpdate<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetLoops<TweenerCore<float, float, FloatOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => this.curPosY, delegate(float y)
			{
				this.curPosY = y;
			}, this.endPosY, this.duration), this.tweenEase), -1, 1), new TweenCallback(this.UpdateTweenPos));
		}

		private void UpdateTweenPos()
		{
			if (this.curPage != null)
			{
				this.curPage.SetTweenFloatingPos(this.curPosY);
			}
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.CollectionViewModule, null);
		}

		private void OnSwitchPage(CustomChooseButton button)
		{
			if (button == null)
			{
				return;
			}
			if (button == this.currentTab)
			{
				return;
			}
			this.currentTab = button;
			if (button == this.tabMain)
			{
				this.curPage = this.collectionMainPage;
			}
			else if (button == this.tabSuit)
			{
				this.curPage = this.collectionSuitPage;
			}
			else if (button == this.tabUpgradeable)
			{
				this.curPage = this.collectionStarUpgradePage;
			}
			if (this.curPage != this.collectionMainPage)
			{
				this.collectionMainPage.OnHide();
			}
			if (this.curPage != this.collectionStarUpgradePage)
			{
				this.collectionStarUpgradePage.OnHide();
			}
			if (this.curPage != this.collectionSuitPage)
			{
				this.collectionSuitPage.OnHide();
			}
			this.curPage.OnShow();
		}

		private void OnEventCollectionMerge(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.curPage != null)
			{
				this.curPage.UpdateView(false);
			}
		}

		private void OnEventCollectionLevelUp(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.curPage != null)
			{
				this.curPage.UpdateView(false);
			}
		}

		private void OnEventCollectionStarUpgrade(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.curPage != null)
			{
				this.curPage.UpdateView(false);
			}
		}

		private void OnEventUpdateView(object sender, int type, BaseEventArgs eventArgs)
		{
			if (this.curPage != null)
			{
				this.curPage.UpdateView(false);
			}
		}

		private void OnRedPointCollectionMainChange(RedNodeListenData redData)
		{
			if (this.redNodeTabCollection == null)
			{
				return;
			}
			this.redNodeTabCollection.gameObject.SetActive(redData.m_count > 0);
		}

		private void OnRedPointCollectionSuitChange(RedNodeListenData redData)
		{
			if (this.redNodeTabSuit == null)
			{
				return;
			}
			this.redNodeTabSuit.gameObject.SetActive(redData.m_count > 0);
		}

		private void OnRedPointCollectionStarUpgradeChange(RedNodeListenData redData)
		{
			if (this.redNodeTabUpgradeable == null)
			{
				return;
			}
			this.redNodeTabUpgradeable.gameObject.SetActive(redData.m_count > 0);
		}

		[GameTestMethod("界面", "打开藏品界面", "", 0)]
		private static void OnOpenCollection()
		{
			GameApp.View.OpenView(ViewName.CollectionViewModule, null, 1, null, null);
		}

		public ModuleCurrencyCtrl moduleCurrencyCtrl;

		public CustomChooseButtonGroup tabGroup;

		public CustomChooseButton tabMain;

		public CustomChooseButton tabSuit;

		public CustomChooseButton tabUpgradeable;

		public CustomButton btnBack;

		public CollectionMainPage collectionMainPage;

		public CollectionStarUpgradePage collectionStarUpgradePage;

		public CollectionSuitPage collectionSuitPage;

		public RedNodeOneCtrl redNodeTabCollection;

		public RedNodeOneCtrl redNodeTabSuit;

		public RedNodeOneCtrl redNodeTabUpgradeable;

		private CustomChooseButton currentTab;

		private CollectionTabPageBase curPage;

		[Header("动画参数")]
		public float startPosY = 54f;

		public float endPosY = 63f;

		public float curPosY = 54f;

		public float duration = 1f;

		public Ease tweenEase = 4;
	}
}
