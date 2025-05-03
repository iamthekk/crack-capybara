using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Modules;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class LanguageChooseViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_closeBt.onClick.AddListener(new UnityAction(this.OnClickCloseView));
			this.Button_Mask.onClick.AddListener(new UnityAction(this.OnClickCloseView));
			this.copyItem.SetActive(false);
			IList<LanguageRaft_languageTab> allElements = GameApp.Table.GetManager().GetLanguageRaft_languageTabModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				if (allElements[i].use > 0)
				{
					this.m_typeList.Add(allElements[i]);
				}
			}
			this.m_typeList.Sort((LanguageRaft_languageTab a, LanguageRaft_languageTab b) => a.sortId.CompareTo(b.sortId));
		}

		public override void OnOpen(object data)
		{
			this.m_seqPool.Clear(false);
			LanguageChooseOpenData languageChooseOpenData = data as LanguageChooseOpenData;
			if (languageChooseOpenData != null)
			{
				this.m_openData = languageChooseOpenData;
			}
			else
			{
				this.m_openData = new LanguageChooseOpenData();
				LanguageDataModule dataModule = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
				this.m_openData.DefaultLanguage = dataModule.GetCurrentLanguageType;
				this.m_openData.Callback = new Action<LanguageType>(this.ChangeGameLanguage);
			}
			this.m_currentlanguage = this.m_openData.DefaultLanguage;
			GameApp.Event.RegisterEvent(LocalMessageName.CC_LANGUAGE_CHOOSE, new HandlerEvent(this.OnEventLanguageChoose));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventLanguageRefresh));
			this.loopListView2.InitListView(this.m_typeList.Count, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.loopListView2.RefreshAllShowItems();
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.loopListView2.ItemList, 0f, 0.05f, 0.3f, 9);
			this.UpdateItemsState();
		}

		private void ChangeGameLanguage(LanguageType type)
		{
			LanguageDataModule dataModule = GameApp.Data.GetDataModule<LanguageDataModule>(1);
			if (type == dataModule.GetCurrentLanguageType)
			{
				return;
			}
			this.m_currentlanguage = type;
			EventArgLanguageType eventArgLanguageType = new EventArgLanguageType();
			eventArgLanguageType.SetData(this.m_currentlanguage);
			GameApp.Event.DispatchNow(this, 1, eventArgLanguageType);
			if (this.m_titleTxt != null)
			{
				this.m_titleTxt.OnRefresh();
			}
			this.UpdateItemsState();
			this.OnClickCloseView();
			GameApp.View.TryDestroyAllCacheUI();
		}

		private void UpdateItemsState()
		{
			for (int i = 0; i < this.m_list.Count; i++)
			{
				this.m_list[i].UpdateButton(this.m_currentlanguage);
			}
		}

		public override void OnClose()
		{
			this.m_seqPool.Clear(false);
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_LANGUAGE_CHOOSE, new HandlerEvent(this.OnEventLanguageChoose));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventLanguageRefresh));
			foreach (LanguageChooseViewItem languageChooseViewItem in this.m_list)
			{
				languageChooseViewItem.DeInit();
			}
			this.m_list.Clear();
		}

		private void OnEventLanguageChoose(object sender, int type, BaseEventArgs arg)
		{
			EventArgLanguageType eventArgLanguageType = arg as EventArgLanguageType;
			if (eventArgLanguageType == null)
			{
				return;
			}
			this.m_currentlanguage = eventArgLanguageType.LanguageType;
			if (this.m_openData != null)
			{
				this.UpdateItemsState();
				this.m_openData.Callback(this.m_currentlanguage);
				return;
			}
			this.ChangeGameLanguage(this.m_currentlanguage);
		}

		private void OnEventLanguageRefresh(object sender, int type, BaseEventArgs arg)
		{
		}

		public override void OnDelete()
		{
			if (this.m_closeBt != null)
			{
				this.m_closeBt.onClick.RemoveListener(new UnityAction(this.OnClickCloseView));
			}
			if (this.Button_Mask != null)
			{
				this.Button_Mask.onClick.RemoveListener(new UnityAction(this.OnClickCloseView));
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.m_typeList.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("copyItem");
			int itemInstanceId = loopListViewItem.gameObject.GetInstanceID();
			LanguageChooseViewItem languageChooseViewItem = this.m_list.Find((LanguageChooseViewItem itemVal) => itemVal.gameObject.GetInstanceID() == itemInstanceId);
			if (languageChooseViewItem == null)
			{
				if (!loopListViewItem.TryGetComponent<LanguageChooseViewItem>(ref languageChooseViewItem))
				{
					return null;
				}
				languageChooseViewItem.SetType(this.m_typeList[index]);
				languageChooseViewItem.Init();
				this.m_list.Add(languageChooseViewItem);
			}
			else
			{
				languageChooseViewItem.SetType(this.m_typeList[index]);
			}
			languageChooseViewItem.RefreshUI();
			languageChooseViewItem.UpdateButton(this.m_currentlanguage);
			return loopListViewItem;
		}

		private void OnClickCloseView()
		{
			GameApp.View.CloseView(ViewName.LanguageChooseViewModule, null);
		}

		public CustomButton m_closeBt;

		public CustomButton Button_Mask;

		public LoopListView2 loopListView2;

		public GameObject copyItem;

		public CustomLanguageText m_titleTxt;

		private LanguageChooseOpenData m_openData;

		private LanguageType m_currentlanguage;

		private SequencePool m_seqPool = new SequencePool();

		private List<LanguageChooseViewItem> m_list = new List<LanguageChooseViewItem>();

		private List<LanguageRaft_languageTab> m_typeList = new List<LanguageRaft_languageTab>();
	}
}
