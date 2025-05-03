using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	[RuntimeCustomSerializedProperty("HotFix.UIMainPagesCtrl")]
	public class UIMainPagesCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_datas.Add(new UIMainPageData(1, this.m_loadPages.Get("Assets/_Resources/Prefab/UI/Main/UIMainShop.prefab").GetComponent<UIMainShop>(), "page_shop", this.shopTabNode, "MainShop", "UIMain_Shop", 1));
			this.m_datas.Add(new UIMainPageData(2, this.m_loadPages.Get("Assets/_Resources/Prefab/UI/Main/UIMainEquip.prefab").GetComponent<UIMainEquip>(), "page_equip", this.equipTabNode, "Equip", "UIMain_Equip", 2));
			this.m_datas.Add(new UIMainPageData(3, this.m_loadPages.Get("Assets/_Resources/Prefab/UI/Main/UIMainCity.prefab").GetComponent<UIMainCity>(), "page_battle", this.battleTabNode, "Main", "UIMain_Adventure", 3));
			this.m_datas.Add(new UIMainPageData(4, this.m_loadPages.Get("Assets/_Resources/Prefab/UI/Main/UIMainTalent.prefab").GetComponent<UIMainTalent>(), "page_talent", this.talentTabNode, "Talent", "UIMain_Talent", 4));
			this.m_datas.Add(new UIMainPageData(5, this.m_loadPages.Get("Assets/_Resources/Prefab/UI/Main/UIMainChest.prefab").GetComponent<UIMainChest>(), "page_chest", this.chestTabNode, "Chest", "UIMain_Chest", 5));
			for (int i = 0; i < this.m_datas.Count; i++)
			{
				UIMainPageData uimainPageData = this.m_datas[i];
				if (uimainPageData != null)
				{
					UIBaseMainPageTabNode tabNode = uimainPageData.m_tabNode;
					tabNode.SetPageName(uimainPageData.m_pageName);
					tabNode.SetRedPointName(uimainPageData.m_redPointName);
					tabNode.SetLanguageID(uimainPageData.m_languageID);
					tabNode.SetFunctionID(uimainPageData.m_functionID);
					tabNode.m_onClick = new Action<UIBaseMainPageTabNode, object>(this.OnTabClick);
					tabNode.Init();
					this.m_tabs[uimainPageData.m_pageName] = tabNode;
				}
			}
			for (int j = 0; j < this.m_datas.Count; j++)
			{
				UIMainPageData uimainPageData2 = this.m_datas[j];
				if (uimainPageData2 != null)
				{
					Singleton<GameFunctionController>.Instance.SetFunctionTarget(string.Format("Main_{0}", j), this.m_tabs[uimainPageData2.m_pageName].transform);
					if (uimainPageData2.m_pageName == 4)
					{
						Singleton<GameFunctionController>.Instance.SetFunctionTarget("TalentLegacy", this.m_tabs[uimainPageData2.m_pageName].transform);
					}
				}
			}
			for (int k = 0; k < this.m_datas.Count; k++)
			{
				UIMainPageData uimainPageData3 = this.m_datas[k];
				if (uimainPageData3 != null)
				{
					UIBaseMainPageNode uibaseMainPageNode = Object.Instantiate<UIBaseMainPageNode>(uimainPageData3.m_pagePrefab);
					uibaseMainPageNode.gameObject.SetParentNormal(this.m_pagesParent, false);
					uibaseMainPageNode.SetPageName(uimainPageData3.m_pageName);
					uibaseMainPageNode.SetLoadObjects(this.m_loadPages);
					uibaseMainPageNode.Init();
					uibaseMainPageNode.SetActive(false);
					this.m_pages[uimainPageData3.m_pageName] = uibaseMainPageNode;
				}
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_currentSelectPage != null)
			{
				this.m_currentSelectPage.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			foreach (KeyValuePair<int, UIBaseMainPageTabNode> keyValuePair in this.m_tabs)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
		}

		protected override void OnDeInit()
		{
			this.m_datas.Clear();
			foreach (KeyValuePair<int, UIBaseMainPageNode> keyValuePair in this.m_pages)
			{
				keyValuePair.Value.DeInit();
				Object.Destroy(keyValuePair.Value.gameObject);
			}
			this.m_pages.Clear();
			foreach (KeyValuePair<int, UIBaseMainPageTabNode> keyValuePair2 in this.m_tabs)
			{
				keyValuePair2.Value.DeInit();
			}
			this.m_tabs.Clear();
		}

		public void SetLoadObjects(LoadPool<GameObject> loadpool)
		{
			this.m_loadPages = loadpool;
		}

		public void OnOpen()
		{
			int currentSelectName = (int)this.m_currentSelectName;
			for (int i = 0; i < this.m_datas.Count; i++)
			{
				UIMainPageData uimainPageData = this.m_datas[i];
				if (uimainPageData != null)
				{
					if (uimainPageData.m_pageName == currentSelectName)
					{
						this.m_currentTabNode = this.m_tabs[uimainPageData.m_pageName];
						this.m_currentTabNode.OnSelect(true, false);
					}
					else
					{
						this.m_tabs[uimainPageData.m_pageName].OnSelect(false, false);
					}
				}
			}
			for (int j = 0; j < this.m_datas.Count; j++)
			{
				UIMainPageData uimainPageData2 = this.m_datas[j];
				if (uimainPageData2 != null)
				{
					if (uimainPageData2.m_pageName == currentSelectName)
					{
						this.m_currentSelectPage = this.m_pages[uimainPageData2.m_pageName];
						this.m_currentSelectPage.Show();
						GameTGAExtend.OnViewOpen("MainViewModule" + uimainPageData2.m_pageName.ToString());
					}
					else
					{
						this.m_pages[uimainPageData2.m_pageName].SetActive(false);
					}
				}
			}
			this.OnRefreshFunctionOpenState();
			for (int k = 0; k < this.m_datas.Count; k++)
			{
				if (!(this.m_datas[k].m_tabNode == null) && !(this.m_datas[k].m_tabNode.m_btn == null))
				{
					GuideController.Instance.AddTarget(string.Format("Main_Down_Button{0}", this.m_datas[k].m_pageName), this.m_datas[k].m_tabNode.m_btn.transform);
				}
			}
			this.OnRefreshTalentTabIcon();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UITalent_UpGradeBack, new HandlerEvent(this.OnTalentUpGradeBack));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnTalentUpGradeBack));
		}

		public void OnClose()
		{
			this.m_currentTabNode = null;
			if (this.m_currentSelectPage != null)
			{
				this.m_currentSelectPage.Hide();
			}
			this.m_currentSelectPage = null;
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UITalent_UpGradeBack, new HandlerEvent(this.OnTalentUpGradeBack));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnTalentUpGradeBack));
		}

		private void OnRefreshTalentTabIcon()
		{
			string text = "tab_icon_talent";
			if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TalentLegacy, false))
			{
				text = "tab_icon_talentLegacy";
			}
			this.talentTabNode.OnSetTabIcon(text);
		}

		public UIBaseMainPageNode GetUIBaseMainPageNode(int pageName)
		{
			if (this.m_pages.ContainsKey(pageName))
			{
				return this.m_pages[pageName];
			}
			return null;
		}

		public UIBaseMainPageTabNode GetUIBaseMainPageTabNode(int pageName)
		{
			if (this.m_tabs.ContainsKey(pageName))
			{
				return this.m_tabs[pageName];
			}
			return null;
		}

		private void OnTabClick(UIBaseMainPageTabNode obj, object openData)
		{
			if (obj.IsLock())
			{
				GameApp.View.ShowStringTip(Singleton<GameFunctionController>.Instance.GetLockTips(obj.GetFunctionID()));
				return;
			}
			if (this.m_currentTabNode == obj)
			{
				this.m_currentTabNode.OnSelect(true, false);
				return;
			}
			if (this.m_currentTabNode != null)
			{
				this.m_currentTabNode.OnSelect(false, true);
			}
			this.m_currentTabNode = obj;
			this.m_currentTabNode.OnSelect(true, true);
			if (this.m_currentSelectPage != null)
			{
				this.m_currentSelectPage.Hide();
				GameTGAExtend.OnViewClose("MainViewModule" + this.m_currentSelectPage.m_pageName.ToString());
			}
			this.m_currentSelectPage = this.m_pages[obj.m_pageName];
			if (openData != null)
			{
				UIBaseMainPageNode.OpenData openData2 = openData as UIBaseMainPageNode.OpenData;
				if (openData2 != null)
				{
					this.m_currentSelectPage.Show(openData2);
				}
				else
				{
					this.m_currentSelectPage.Show();
				}
			}
			else
			{
				this.m_currentSelectPage.Show();
			}
			GameTGAExtend.OnViewOpen("MainViewModule" + obj.m_pageName.ToString());
			this.OnRefreshTalentTabIcon();
			this.HandleAfterSwitchPage();
			GuideController.Instance.MainUISwitchPage(this.m_currentTabNode.m_pageName);
			EventArgGuideTrigger eventArgGuideTrigger = new EventArgGuideTrigger();
			eventArgGuideTrigger.SetData(GuideTriggerKind.MainDownButton, this.m_currentTabNode.m_pageName.ToString());
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Guide_Trigger, eventArgGuideTrigger);
		}

		private void HandleAfterSwitchPage()
		{
			if (this.m_currentSelectPage != null)
			{
				switch (this.m_currentSelectPage.m_pageName)
				{
				case 1:
				case 2:
				case 4:
				case 5:
					break;
				case 3:
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_ViewPopCheck_ReCheck, null);
					break;
				default:
					return;
				}
			}
		}

		public void GotoPage(UIMainPageName mainPageName, object openData = null)
		{
			UIBaseMainPageTabNode uibaseMainPageTabNode;
			this.m_tabs.TryGetValue((int)mainPageName, out uibaseMainPageTabNode);
			if (uibaseMainPageTabNode == null)
			{
				return;
			}
			this.OnTabClick(uibaseMainPageTabNode, openData);
		}

		public UIMainPageName GetCurrentPageEnum()
		{
			return (UIMainPageName)this.m_currentTabNode.m_pageName;
		}

		public UIBaseMainPageNode GetCurrentPage()
		{
			return this.m_currentSelectPage;
		}

		public T GetPage<T>(UIMainPageName pageName) where T : UIBaseMainPageNode
		{
			UIBaseMainPageNode uibaseMainPageNode;
			this.m_pages.TryGetValue((int)pageName, out uibaseMainPageNode);
			return uibaseMainPageNode as T;
		}

		public void OnLanguageChange()
		{
			foreach (KeyValuePair<int, UIBaseMainPageNode> keyValuePair in this.m_pages)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.OnLanguageChange();
				}
			}
			foreach (KeyValuePair<int, UIBaseMainPageTabNode> keyValuePair2 in this.m_tabs)
			{
				if (!(keyValuePair2.Value == null))
				{
					keyValuePair2.Value.OnLanguageChange();
				}
			}
		}

		public void OnRefreshFunctionOpenState()
		{
			foreach (KeyValuePair<int, UIBaseMainPageTabNode> keyValuePair in this.m_tabs)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.OnRefreshFunctionOpenState();
				}
			}
		}

		private void OnTalentUpGradeBack(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefreshTalentTabIcon();
		}

		[SerializeField]
		private RectTransform m_pagesParent;

		[SerializeField]
		private UIMainPageTabNodeDefalut shopTabNode;

		[SerializeField]
		private UIMainPageTabNodeDefalut equipTabNode;

		[SerializeField]
		private UIMainPageTabNodeDefalut battleTabNode;

		[SerializeField]
		private UIMainPageTabNodeDefalut talentTabNode;

		[SerializeField]
		private UIMainPageTabNodeDefalut chestTabNode;

		private Dictionary<int, UIBaseMainPageNode> m_pages = new Dictionary<int, UIBaseMainPageNode>();

		private Dictionary<int, UIBaseMainPageTabNode> m_tabs = new Dictionary<int, UIBaseMainPageTabNode>();

		private List<UIMainPageData> m_datas = new List<UIMainPageData>();

		private UIMainPageName m_currentSelectName = UIMainPageName.Battle;

		public UIBaseMainPageTabNode m_currentTabNode;

		private UIBaseMainPageNode m_currentSelectPage;

		private LoadPool<GameObject> m_loadPages;
	}
}
