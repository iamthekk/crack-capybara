using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class MainViewModule : BaseViewModule
	{
		public override async void OnCreate(object obj)
		{
			this.m_shadowImage.SetActiveSafe(false);
			MainViewModuleLoader mainViewModuleLoader = base.Loader as MainViewModuleLoader;
			this.m_pagesCtrl.SetLoadObjects(mainViewModuleLoader.m_loadPageObjects);
			this.m_pagesCtrl.Init();
			if (this.newWorldButtonCtrl != null)
			{
				Singleton<GameFunctionController>.Instance.SetFunctionTarget("NewWorld", this.newWorldButtonCtrl.transform);
			}
		}

		public override void OnOpen(object data)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Main_ShowCorner, new HandlerEvent(this.OnEventShowCorner));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Main_ShowCurrency, new HandlerEvent(this.OnEventShowCurrency));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Main_FoldStateChange, new HandlerEvent(this.OnFoldStateChange));
			this.m_shadowAni.gameObject.SetActiveSafe(false);
			if (this.m_leftButtonsCtrl != null)
			{
				this.m_leftButtonsCtrl.Init();
			}
			if (this.m_rightButtonsCtrl != null)
			{
				this.m_rightButtonsCtrl.Init();
			}
			if (this.m_extensionButtonsCtrl != null)
			{
				this.m_extensionButtonsCtrl.Init();
			}
			if (this.chatNode != null)
			{
				this.chatNode.Init();
			}
			if (this.m_pagesCtrl != null)
			{
				this.m_pagesCtrl.OnOpen();
			}
			if (this.newWorldButtonCtrl != null)
			{
				this.newWorldButtonCtrl.Init();
			}
			GameApp.Event.RegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventChangeLanguage));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_pagesCtrl != null)
			{
				this.m_pagesCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (this.m_leftButtonsCtrl != null)
			{
				this.m_leftButtonsCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (this.m_rightButtonsCtrl != null)
			{
				this.m_rightButtonsCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (this.m_extensionButtonsCtrl != null)
			{
				this.m_extensionButtonsCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (this.chatNode != null)
			{
				this.chatNode.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (this.newWorldButtonCtrl != null)
			{
				this.newWorldButtonCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (HangUpManager.Instance != null)
			{
				HangUpManager.Instance.Update(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnClose()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventChangeLanguage));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Main_ShowCorner, new HandlerEvent(this.OnEventShowCorner));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Main_ShowCurrency, new HandlerEvent(this.OnEventShowCurrency));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Main_FoldStateChange, new HandlerEvent(this.OnFoldStateChange));
			if (this.m_pagesCtrl != null)
			{
				this.m_pagesCtrl.OnClose();
			}
			if (this.m_leftButtonsCtrl != null)
			{
				this.m_leftButtonsCtrl.DeInit();
			}
			if (this.m_rightButtonsCtrl != null)
			{
				this.m_rightButtonsCtrl.DeInit();
			}
			if (this.m_extensionButtonsCtrl != null)
			{
				this.m_extensionButtonsCtrl.DeInit();
			}
			if (this.chatNode != null)
			{
				this.chatNode.DeInit();
			}
			if (this.newWorldButtonCtrl != null)
			{
				this.newWorldButtonCtrl.DeInit();
			}
			this.m_seqPool.Clear(false);
		}

		public override void OnDelete()
		{
			if (this.m_pagesCtrl != null)
			{
				this.m_pagesCtrl.DeInit();
			}
			this.m_pagesCtrl = null;
			this.m_leftButtonsCtrl = null;
			this.m_rightButtonsCtrl = null;
			this.m_extensionButtonsCtrl = null;
			this.newWorldButtonCtrl = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void GotoPage(UIMainPageName mainPageName, object openData = null)
		{
			if (this.m_pagesCtrl != null)
			{
				this.m_pagesCtrl.GotoPage(mainPageName, openData);
			}
		}

		public UIMainPageName GetCurrentPageEnum()
		{
			if (this.m_pagesCtrl != null)
			{
				return this.m_pagesCtrl.GetCurrentPageEnum();
			}
			return UIMainPageName.Battle;
		}

		public UIBaseMainPageNode GetCurrentPage()
		{
			if (this.m_pagesCtrl != null)
			{
				return this.m_pagesCtrl.GetCurrentPage();
			}
			return null;
		}

		public T GetPage<T>(UIMainPageName pageName) where T : UIBaseMainPageNode
		{
			if (this.m_pagesCtrl == null)
			{
				return default(T);
			}
			return this.m_pagesCtrl.GetPage<T>(pageName);
		}

		private void OnEventShowCorner(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsBool eventArgsBool = eventargs as EventArgsBool;
			if (eventArgsBool == null)
			{
				return;
			}
			this.SetShowChat(eventArgsBool.Value);
			this.SetShowCorner(eventArgsBool.Value);
		}

		private void SetShowCorner(bool isShow)
		{
			if (this.m_leftButtonsCtrl != null)
			{
				this.m_leftButtonsCtrl.SetActive(isShow);
			}
			if (this.m_rightButtonsCtrl != null)
			{
				this.m_rightButtonsCtrl.SetActive(isShow);
			}
			if (this.m_extensionButtonsCtrl != null)
			{
				this.m_extensionButtonsCtrl.SetActive(isShow);
			}
			if (this.newWorldButtonCtrl != null)
			{
				this.newWorldButtonCtrl.SetActive(isShow);
			}
			if (!isShow)
			{
				return;
			}
			if (this.m_leftButtonsCtrl == null || this.m_leftButtonsCtrl.gameObject == null)
			{
				return;
			}
			if (this.m_rightButtonsCtrl == null || this.m_rightButtonsCtrl.gameObject == null)
			{
				return;
			}
			if (this.m_extensionButtonsCtrl == null || this.m_extensionButtonsCtrl.gameObject == null)
			{
				return;
			}
			if (this.newWorldButtonCtrl == null || this.newWorldButtonCtrl.gameObject == null)
			{
				return;
			}
			this.m_leftButtonsCtrl.OnRefresh();
			this.m_rightButtonsCtrl.OnRefresh();
			this.m_extensionButtonsCtrl.OnRefresh();
			this.newWorldButtonCtrl.OnRefresh();
			Sequence sequence = this.m_seqPool.Get();
			Vector2 vector;
			vector..ctor(-200f, 0f);
			this.m_leftButtonsCtrl.rectTransform.anchoredPosition = vector;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(this.m_leftButtonsCtrl.rectTransform, 0f, 0.2f, false));
			Vector2 vector2;
			vector2..ctor(200f, 0f);
			this.m_rightButtonsCtrl.rectTransform.anchoredPosition = vector2;
			TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosX(this.m_rightButtonsCtrl.rectTransform, 0f, 0.2f, false));
		}

		private void SetShowChat(bool isShow)
		{
			if (this.chatNode == null)
			{
				return;
			}
			this.chatNode.SetView(isShow);
		}

		private void OnEventShowCurrency(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsBool eventArgsBool = eventargs as EventArgsBool;
			if (eventArgsBool == null)
			{
				return;
			}
			if (eventArgsBool.Value)
			{
				if (!GameApp.View.IsOpenedOrLoading(ViewName.CurrencyViewModule))
				{
					GameApp.View.OpenView(ViewName.CurrencyViewModule, null, 0, null, null);
					return;
				}
			}
			else if (GameApp.View.IsOpened(ViewName.CurrencyViewModule))
			{
				GameApp.View.CloseView(ViewName.CurrencyViewModule, null);
			}
		}

		private void OnEventChangeLanguage(object sender, int type, BaseEventArgs eventargs)
		{
			if (this.m_pagesCtrl != null)
			{
				this.m_pagesCtrl.OnLanguageChange();
			}
			if (this.m_leftButtonsCtrl != null)
			{
				this.m_leftButtonsCtrl.OnLanguageChange();
			}
			if (this.m_rightButtonsCtrl != null)
			{
				this.m_rightButtonsCtrl.OnLanguageChange();
			}
			if (this.m_extensionButtonsCtrl != null)
			{
				this.m_extensionButtonsCtrl.OnLanguageChange();
			}
		}

		private void OnShadowShow(object sender, int type, BaseEventArgs eventArgs)
		{
			this.m_shadowAni.gameObject.SetActiveSafe(true);
			this.m_shadowAni.SetTrigger("Show");
		}

		private void OnShadowHide(object sender, int type, BaseEventArgs eventArgs)
		{
			this.m_shadowAni.gameObject.SetActiveSafe(true);
			this.m_shadowAni.SetTrigger("Hide");
		}

		private void OnFunctionOpen(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsFunctionOpen eventArgsFunctionOpen = eventArgs as EventArgsFunctionOpen;
			if (eventArgsFunctionOpen == null)
			{
				return;
			}
			if (this.m_pagesCtrl != null)
			{
				this.m_pagesCtrl.OnRefreshFunctionOpenState();
			}
			FunctionID[] array = new FunctionID[]
			{
				FunctionID.Activity_Sign,
				FunctionID.IAPShop,
				FunctionID.Activity_Carnival,
				FunctionID.RechargeGift,
				FunctionID.FirstTopUp,
				FunctionID.PrivilegeCard
			};
			FunctionID[] array2 = new FunctionID[]
			{
				FunctionID.Activity_SlotTrain,
				FunctionID.Activity_Week
			};
			IEnumerable<FunctionID> enumerable = new FunctionID[]
			{
				FunctionID.Bag,
				FunctionID.Mail,
				FunctionID.Task,
				FunctionID.Ranking,
				FunctionID.Friend,
				FunctionID.Activity_Sign,
				FunctionID.TVReward,
				FunctionID.Setting
			};
			if (array.Contains((FunctionID)eventArgsFunctionOpen.FunctionID) && this.m_leftButtonsCtrl != null)
			{
				this.m_leftButtonsCtrl.OnRefresh();
				this.m_leftButtonsCtrl.SwitchToUnfold();
			}
			if (array2.Contains((FunctionID)eventArgsFunctionOpen.FunctionID) && this.m_rightButtonsCtrl != null)
			{
				this.m_rightButtonsCtrl.OnRefresh();
				this.m_rightButtonsCtrl.SwitchToUnfold();
			}
			if (enumerable.Contains((FunctionID)eventArgsFunctionOpen.FunctionID) && this.m_extensionButtonsCtrl != null)
			{
				this.m_extensionButtonsCtrl.OnRefresh();
			}
			if (eventArgsFunctionOpen.FunctionID == 71 && this.newWorldButtonCtrl != null)
			{
				this.newWorldButtonCtrl.OnRefresh();
			}
			if (this.chatNode != null)
			{
				this.chatNode.OnRefreshShowByFunction(this.m_pagesCtrl.m_currentTabNode.m_pageName == UIMainPageName.Battle.GetHashCode());
			}
			RedPointController.Instance.ReCalc("Main", true);
		}

		private void OnFoldStateChange(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsMainFoldChange eventArgsMainFoldChange = eventArgs as EventArgsMainFoldChange;
			if (eventArgsMainFoldChange == null)
			{
				return;
			}
			int kind = eventArgsMainFoldChange.Kind;
			if (kind != 1)
			{
				if (kind != 2)
				{
					return;
				}
				if (this.m_rightButtonsCtrl != null)
				{
					this.m_rightButtonsCtrl.SwitchFoldState(eventArgsMainFoldChange.ToFoldState);
				}
			}
			else if (this.m_leftButtonsCtrl != null)
			{
				this.m_leftButtonsCtrl.SwitchFoldState(eventArgsMainFoldChange.ToFoldState);
				return;
			}
		}

		public T ForGuide_GetUITabContent<T>(int pageName) where T : UIBaseMainPageNode
		{
			if (this.m_pagesCtrl == null)
			{
				return default(T);
			}
			return this.m_pagesCtrl.GetUIBaseMainPageNode(pageName) as T;
		}

		public RectTransform ForGuide_GetTaskButton()
		{
			if (this.m_rightButtonsCtrl == null)
			{
				return null;
			}
			if (this.m_extensionButtonsCtrl.btnExtension == null)
			{
				return null;
			}
			return this.m_extensionButtonsCtrl.GetButtonTransform();
		}

		public UIMainPageName GetCurrentPageName()
		{
			if (this.m_pagesCtrl != null)
			{
				UIBaseMainPageNode currentPage = this.m_pagesCtrl.GetCurrentPage();
				if (currentPage != null)
				{
					return (UIMainPageName)currentPage.m_pageName;
				}
			}
			return UIMainPageName.Battle;
		}

		public void EnterNewWorld(Action onAniFinish)
		{
			CloudLoadingViewModule.OpenData openData = new CloudLoadingViewModule.OpenData();
			openData.onAnimFinish = onAniFinish;
			openData.onCloudClose = delegate
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Change_SceneSkin, null);
				if (this.newWorldButtonCtrl != null)
				{
					this.newWorldButtonCtrl.OnRefresh();
				}
			};
			GameApp.View.OpenView(ViewName.CloudLoadingViewModule, openData, 3, null, null);
		}

		[GameTestMethod("新大陆", "前往新大陆动画", "", 410)]
		private static void OpenGameEventDemon()
		{
			MainViewModule viewModule = GameApp.View.GetViewModule(ViewName.MainViewModule);
			if (viewModule)
			{
				viewModule.EnterNewWorld(null);
			}
		}

		[SerializeField]
		private GameObject m_shadowImage;

		[SerializeField]
		private Animator m_shadowAni;

		[SerializeField]
		private UIMainPagesCtrl m_pagesCtrl;

		[SerializeField]
		private UIMainLeftButtonsCtrl m_leftButtonsCtrl;

		[SerializeField]
		private UIMainRightButtonsCtrl m_rightButtonsCtrl;

		[SerializeField]
		private UIMainExtensionButtonsCtrl m_extensionButtonsCtrl;

		[SerializeField]
		private RectTransform m_bottomAnchor;

		[SerializeField]
		private ChatHomeNode chatNode;

		[SerializeField]
		private UIMainNewWorldButtonCtrl newWorldButtonCtrl;

		private SequencePool m_seqPool = new SequencePool();
	}
}
