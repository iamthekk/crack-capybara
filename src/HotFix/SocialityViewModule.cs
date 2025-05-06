using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class SocialityViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_tabDatas.Clear();
			this.m_tabDatas.Add(new SocialityViewModule.TabData
			{
				m_bt = this.m_rankBt,
				m_panel = this.m_rankPanel
			});
			this.m_tabDatas.Add(new SocialityViewModule.TabData
			{
				m_bt = this.m_guidBt,
				m_panel = this.m_guildPanel
			});
			this.m_tabDatas.Add(new SocialityViewModule.TabData
			{
				m_bt = this.m_interactiveBt,
				m_panel = this.m_interactivePanel
			});
			this.m_rankPanel.Init();
			this.m_guildPanel.Init();
			this.m_interactivePanel.Init();
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as SocialityViewModule.OpenData;
			this.m_maskBt.onClick.AddListener(new UnityAction(this.OnClickCloseBtn));
			this.m_closeBtn.onClick.AddListener(new UnityAction(this.OnClickCloseBtn));
			this.m_rankBt.onClick.AddListener(new UnityAction(this.OnClickRankBt));
			this.m_guidBt.onClick.AddListener(new UnityAction(this.OnClickGuidBt));
			this.m_interactiveBt.onClick.AddListener(new UnityAction(this.OnClickInteractiveBt));
			if (this.m_openData != null)
			{
				this.OnSelectIndex(this.m_openData.m_selectIndex);
			}
			else
			{
				this.OnSelectIndex(0);
			}
			RedPointController.Instance.RegRecordChange("Main.Sociality", new Action<RedNodeListenData>(this.OnRedPointChange));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			RedPointController.Instance.UnRegRecordChange("Main.Sociality", new Action<RedNodeListenData>(this.OnRedPointChange));
			this.OnSelectIndex(-1);
			this.m_maskBt.onClick.RemoveAllListeners();
			this.m_closeBtn.onClick.RemoveAllListeners();
			this.m_rankBt.onClick.RemoveAllListeners();
			this.m_guidBt.onClick.RemoveAllListeners();
			this.m_interactiveBt.onClick.RemoveAllListeners();
			this.m_openData = null;
		}

		public override void OnDelete()
		{
			this.m_rankPanel.DeInit();
			this.m_guildPanel.DeInit();
			this.m_interactivePanel.DeInit();
			this.m_tabDatas.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnSelectIndex(int index)
		{
			if (this.m_selectIndex == index)
			{
				return;
			}
			if (this.m_selectIndex != -1)
			{
				SocialityViewModule.TabData tabData = this.m_tabDatas[this.m_selectIndex];
				tabData.m_bt.SetSelect(false);
				tabData.m_panel.OnHide();
				tabData.m_panel.gameObject.SetActive(false);
			}
			if (index >= 0)
			{
				for (int i = 0; i < this.m_tabDatas.Count; i++)
				{
					SocialityViewModule.TabData tabData2 = this.m_tabDatas[i];
					if (tabData2 != null)
					{
						if (index != i)
						{
							tabData2.m_bt.SetSelect(false);
							tabData2.m_panel.SetActive(false);
						}
						else
						{
							tabData2.m_bt.SetSelect(true);
							tabData2.m_panel.OnShow();
							tabData2.m_panel.gameObject.SetActive(true);
						}
					}
				}
			}
			this.m_selectIndex = index;
		}

		public int GetSelectIndex()
		{
			return this.m_selectIndex;
		}

		private void OnRedPointChange(RedNodeListenData obj)
		{
			if (this.m_interactiveBtRedPoint == null)
			{
				return;
			}
			this.m_interactiveBtRedPoint.SetType((obj.m_count <= 1) ? 240 : 100);
			this.m_interactiveBtRedPoint.Value = obj.m_count;
		}

		private void OnClickCloseBtn()
		{
			if (this.m_openData != null)
			{
				MoreExtensionViewModule.TryBackOpenView(this.m_openData.srcViewName);
			}
			GameApp.View.CloseView(ViewName.SocialityViewModule, null);
		}

		private void OnClickRankBt()
		{
			this.OnSelectIndex(0);
		}

		private void OnClickGuidBt()
		{
			this.OnSelectIndex(1);
		}

		private void OnClickInteractiveBt()
		{
			this.OnSelectIndex(2);
			RedPointController.Instance.ClickRecord("Main.Sociality");
		}

		public CustomButton m_maskBt;

		public CustomButton m_closeBtn;

		public SocialityViewModule.OpenData m_openData;

		public CustomChooseButton m_rankBt;

		public CustomChooseButton m_guidBt;

		public CustomChooseButton m_interactiveBt;

		public SocialityRankPanelGroup m_rankPanel;

		public SocialityGuildPanelGroup m_guildPanel;

		public SocialityInteractivePanelGroup m_interactivePanel;

		public RedNodeOneCtrl m_interactiveBtRedPoint;

		[HideInInspector]
		public int m_selectIndex = -1;

		public List<SocialityViewModule.TabData> m_tabDatas = new List<SocialityViewModule.TabData>();

		public class OpenData
		{
			public int m_selectIndex;

			public ViewName srcViewName;

			public Action onCloseCallback;
		}

		public class TabData
		{
			public CustomChooseButton m_bt;

			public BaseSocialityPanel m_panel;
		}
	}
}
