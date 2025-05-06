using System;
using System.Collections.Generic;
using Dxx.Chat;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class ChatHomePanel : CustomBehaviour
	{
		public Action OnBack { get; set; }

		protected override void OnInit()
		{
			this.curPanelType = ChatHomeSubPanelType.Null;
			this.SetHeight(true, true);
			foreach (ChatHomeSubTab chatHomeSubTab in this.tabList)
			{
				chatHomeSubTab.Init();
				chatHomeSubTab.SetData(false, new Action<ChatHomeSubPanelType>(this.SwitchSubPanel));
			}
			foreach (ChatHomeSubPanel chatHomeSubPanel in this.panelList)
			{
				chatHomeSubPanel.Init();
			}
			this.backButton.m_onClick = new Action(this.BackOnClick);
			this.expansionButton.m_onClick = new Action(this.ExpansionOnClick);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventChangeLanguage));
		}

		private void ExpansionOnClick()
		{
			this.SetHeight(!this.curIsSmall, false);
			foreach (ChatHomeSubPanel chatHomeSubPanel in this.panelList)
			{
				chatHomeSubPanel.ResetListView();
			}
		}

		private void BackOnClick()
		{
			Action onBack = this.OnBack;
			if (onBack == null)
			{
				return;
			}
			onBack();
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventChangeLanguage));
			foreach (ChatHomeSubTab chatHomeSubTab in this.tabList)
			{
				chatHomeSubTab.DeInit();
			}
			foreach (ChatHomeSubPanel chatHomeSubPanel in this.panelList)
			{
				chatHomeSubPanel.DeInit();
			}
		}

		private void SwitchSubPanel(ChatHomeSubPanelType panelType)
		{
			if (this.curPanelType == panelType)
			{
				return;
			}
			this.curPanelType = panelType;
			foreach (ChatHomeSubTab chatHomeSubTab in this.tabList)
			{
				chatHomeSubTab.SetSelect(chatHomeSubTab.TabType == panelType);
				if (chatHomeSubTab.TabType == panelType)
				{
					if (panelType == ChatHomeSubPanelType.Scene)
					{
						GameApp.SocketNet.SetSocketGroup(3, "");
						GameApp.SocketNet.SetSocketGroup(2, ChatProxy.SceneChat.GetGroupID());
					}
					else if (panelType == ChatHomeSubPanelType.Server)
					{
						GameApp.SocketNet.SetSocketGroup(2, "");
						GameApp.SocketNet.SetSocketGroup(3, ChatProxy.ServerChat.GetGroupID());
					}
				}
			}
			foreach (ChatHomeSubPanel chatHomeSubPanel in this.panelList)
			{
				if (chatHomeSubPanel.PanelType == panelType)
				{
					chatHomeSubPanel.SetSelect(false);
				}
				else
				{
					chatHomeSubPanel.SetUnSelect(false);
				}
			}
		}

		private void SetHeight(bool isSmall, bool isForce)
		{
			if (!isForce && isSmall == this.curIsSmall)
			{
				return;
			}
			this.curIsSmall = isSmall;
			float num = (isSmall ? this.smallHeight : this.bigHeight);
			Vector2 sizeDelta = base.rectTransform.sizeDelta;
			sizeDelta.y = num;
			base.rectTransform.sizeDelta = sizeDelta;
			this.expansionButton.transform.localEulerAngles = (isSmall ? Vector3.zero : new Vector3(0f, 0f, 180f));
		}

		public void SetView(ChatHomeSubPanelType panelType)
		{
			this.SwitchSubPanel(panelType);
			base.gameObject.SetActive(this.IsShow());
		}

		public bool IsShow()
		{
			return this.curPanelType > ChatHomeSubPanelType.Null;
		}

		private void OnEventChangeLanguage(object sender, int type, BaseEventArgs eventargs)
		{
			for (int i = 0; i < this.panelList.Count; i++)
			{
				if (this.panelList[i].isActiveAndEnabled)
				{
					this.panelList[i].OnLanguageChange();
				}
			}
		}

		[SerializeField]
		private List<ChatHomeSubTab> tabList;

		[SerializeField]
		private List<ChatHomeSubPanel> panelList;

		[SerializeField]
		private CustomButton backButton;

		[SerializeField]
		private CustomButton expansionButton;

		[SerializeField]
		private float smallHeight;

		[SerializeField]
		private float bigHeight;

		private ChatHomeSubPanelType curPanelType;

		private bool curIsSmall;
	}
}
