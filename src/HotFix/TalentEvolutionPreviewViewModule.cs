using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class TalentEvolutionPreviewViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			try
			{
				this.btnBottom2Focus.gameObject.SetActive(false);
				this.btnTop2Focus.gameObject.SetActive(false);
				this.talentDataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
				this.CreateData();
				this.loopListTitle.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetTitleItemByIndex), null);
				this.loopListContent.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetContentItemByIndex), null);
				for (int i = 0; i < this.CareerItemList.Count; i++)
				{
					this.CareerItemList[i].Init();
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public override void OnOpen(object data)
		{
			this.Text_TalentLegacyDesc.SetText(HLog.StringBuilder(Singleton<LanguageManager>.Instance.GetInfoByID("legacy_preview_desc"), "\n"), true);
			this.Obj_Talent.SetActiveSafe(true);
			this.Obj_TalentLegacy.SetActiveSafe(false);
			this.loopListTitle.SetListItemCount(this.titleNodeDatas.Count, true);
			this.loopListTitle.RefreshAllShownItem();
			this.loopListContent.SetListItemCount(this.contentNodeDatas.Count, true);
			this.loopListContent.RefreshAllShownItem();
			this.loopListContent.MovePanelToItemIndex(this.curContentItemIndex - 1, 0f);
			for (int i = 0; i < this.CareerItemList.Count; i++)
			{
				this.CareerItemList[i].SetData();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			for (int i = 0; i < this.CareerItemList.Count; i++)
			{
				this.CareerItemList[i].DeInit();
			}
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.btnBottom2Focus.m_onClick = new Action(this.Jump2FocusContentItem);
			this.btnTop2Focus.m_onClick = new Action(this.Jump2FocusContentItem);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TanlentLegacyClickPreview, new HandlerEvent(this.OnClickTalentLegacyPreview));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TalentLegacyClickTitle, new HandlerEvent(this.OnClickTalentTitle));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.uiPopCommon.OnClick = null;
			this.btnBottom2Focus.m_onClick = null;
			this.btnTop2Focus.m_onClick = null;
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TanlentLegacyClickPreview, new HandlerEvent(this.OnClickTalentLegacyPreview));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TalentLegacyClickTitle, new HandlerEvent(this.OnClickTalentTitle));
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			this.OnBtnCloseClick();
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.TalentEvolutionPreviewViewModule, null);
		}

		private void CreateData()
		{
			this.titleNodeDatas.Clear();
			this.contentNodeDatas.Clear();
			IList<TalentNew_talent> allElements = GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetAllElements();
			this.contentNodeDatas.Add(new TalentEvolutionPreviewViewModule.ContentNodeData
			{
				isTopSpace = true
			});
			for (int i = 0; i < allElements.Count; i++)
			{
				TalentEvolutionPreviewViewModule.ContentNodeData contentNodeData = new TalentEvolutionPreviewViewModule.ContentNodeData();
				contentNodeData.cfg = allElements[i];
				this.contentNodeDatas.Add(contentNodeData);
				if (this.talentDataModule.talentProgressData.curId == contentNodeData.cfg.id)
				{
					this.curContentItemIndex = i + 1;
				}
			}
			this.contentNodeDatas.Add(new TalentEvolutionPreviewViewModule.ContentNodeData
			{
				isBottomSpace = true
			});
			IList<TalentNew_talentMegaStage> allElements2 = GameApp.Table.GetManager().GetTalentNew_talentMegaStageModelInstance().GetAllElements();
			this.titleNodeDatas.Add(new TalentEvolutionPreviewViewModule.TitleNodeData
			{
				isTopSpace = true
			});
			for (int j = 0; j < allElements2.Count; j++)
			{
				TalentEvolutionPreviewViewModule.TitleNodeData titleNodeData = new TalentEvolutionPreviewViewModule.TitleNodeData();
				titleNodeData.cfg = allElements2[j];
				this.titleNodeDatas.Add(titleNodeData);
			}
			this.titleNodeDatas.Add(new TalentEvolutionPreviewViewModule.TitleNodeData
			{
				isTalentLegacy = true
			});
			this.titleNodeDatas.Add(new TalentEvolutionPreviewViewModule.TitleNodeData
			{
				isBottomSpace = true
			});
		}

		private LoopListViewItem2 OnGetTitleItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.titleNodeDatas.Count)
			{
				return null;
			}
			TalentEvolutionPreviewViewModule.TitleNodeData titleNodeData = this.titleNodeDatas[index];
			LoopListViewItem2 loopListViewItem;
			if (titleNodeData.isTopSpace)
			{
				loopListViewItem = view.NewListViewItem("TopSpace");
				int instanceID = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour;
				this.contentItemsDict.TryGetValue(instanceID, out customBehaviour);
				if (customBehaviour == null)
				{
					UIItemSpace component = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component.Init();
					this.contentItemsDict[instanceID] = component;
				}
				return loopListViewItem;
			}
			if (titleNodeData.isBottomSpace)
			{
				loopListViewItem = view.NewListViewItem("BottomSpace");
				int instanceID2 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour2;
				this.contentItemsDict.TryGetValue(instanceID2, out customBehaviour2);
				if (customBehaviour2 == null)
				{
					UIItemSpace component2 = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component2.Init();
					this.contentItemsDict[instanceID2] = component2;
				}
				return loopListViewItem;
			}
			loopListViewItem = view.NewListViewItem("EvolutionTitleItem");
			int instanceID3 = loopListViewItem.gameObject.GetInstanceID();
			CustomBehaviour customBehaviour3;
			this.contentItemsDict.TryGetValue(instanceID3, out customBehaviour3);
			if (customBehaviour3 == null)
			{
				TalentEvolutionTitleItem component3 = loopListViewItem.gameObject.GetComponent<TalentEvolutionTitleItem>();
				component3.Init();
				this.contentItemsDict[instanceID3] = component3;
				customBehaviour3 = component3;
			}
			TalentEvolutionTitleItem talentEvolutionTitleItem = customBehaviour3 as TalentEvolutionTitleItem;
			if (talentEvolutionTitleItem != null)
			{
				talentEvolutionTitleItem.SetData(titleNodeData.cfg);
			}
			return loopListViewItem;
		}

		private LoopListViewItem2 OnGetContentItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.contentNodeDatas.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = null;
			TalentEvolutionPreviewViewModule.ContentNodeData contentNodeData = this.contentNodeDatas[index];
			if (contentNodeData.isTopSpace)
			{
				loopListViewItem = view.NewListViewItem("TopSpace");
				int instanceID = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour;
				this.contentItemsDict.TryGetValue(instanceID, out customBehaviour);
				if (customBehaviour == null)
				{
					UIItemSpace component = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component.Init();
					this.contentItemsDict[instanceID] = component;
				}
			}
			else if (contentNodeData.isBottomSpace)
			{
				loopListViewItem = view.NewListViewItem("BottomSpace");
				int instanceID2 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour2;
				this.contentItemsDict.TryGetValue(instanceID2, out customBehaviour2);
				if (customBehaviour2 == null)
				{
					UIItemSpace component2 = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component2.Init();
					this.contentItemsDict[instanceID2] = component2;
				}
			}
			else
			{
				loopListViewItem = view.NewListViewItem("EvolutionContentItem");
				int instanceID3 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour3;
				this.contentItemsDict.TryGetValue(instanceID3, out customBehaviour3);
				if (customBehaviour3 == null)
				{
					TalentEvolutionContentItem component3 = loopListViewItem.gameObject.GetComponent<TalentEvolutionContentItem>();
					component3.Init();
					this.contentItemsDict[instanceID3] = component3;
					customBehaviour3 = component3;
				}
				TalentEvolutionContentItem talentEvolutionContentItem = customBehaviour3 as TalentEvolutionContentItem;
				if (talentEvolutionContentItem != null)
				{
					talentEvolutionContentItem.SetData(index, contentNodeData.cfg);
				}
			}
			List<TalentEvolutionContentItem> list = new List<TalentEvolutionContentItem>();
			foreach (CustomBehaviour customBehaviour4 in this.contentItemsDict.Values)
			{
				if (customBehaviour4 is TalentEvolutionContentItem)
				{
					list.Add(customBehaviour4 as TalentEvolutionContentItem);
				}
			}
			list.Sort(new Comparison<TalentEvolutionContentItem>(this.SortContentItemList));
			int num = int.MaxValue;
			int num2 = int.MinValue;
			for (int i = 0; i < list.Count; i++)
			{
				list[i].transform.SetAsLastSibling();
				if (list[i].isActiveAndEnabled)
				{
					num = Mathf.Min(num, list[i].index);
					num2 = Mathf.Max(num2, list[i].index);
				}
			}
			if (list.Count <= 0)
			{
				this.m_skipIndex = this.curContentItemIndex - 1;
				this.btnTop2Focus.gameObject.SetActive(false);
				this.btnBottom2Focus.gameObject.SetActive(false);
			}
			else if (this.curContentItemIndex > num2)
			{
				this.m_skipIndex = -1;
				this.btnTop2Focus.gameObject.SetActive(true);
				this.btnBottom2Focus.gameObject.SetActive(false);
			}
			else if (this.curContentItemIndex < num)
			{
				this.m_skipIndex = -1;
				this.btnTop2Focus.gameObject.SetActive(false);
				this.btnBottom2Focus.gameObject.SetActive(true);
			}
			else
			{
				this.m_skipIndex = this.curContentItemIndex - 1;
				this.btnTop2Focus.gameObject.SetActive(false);
				this.btnBottom2Focus.gameObject.SetActive(false);
			}
			return loopListViewItem;
		}

		private void OnClickTalentTitle(object sender, int type, BaseEventArgs eventArgs)
		{
			this.Obj_Talent.SetActiveSafe(true);
			this.Obj_TalentLegacy.SetActiveSafe(false);
			if (this.m_skipIndex == this.curContentItemIndex - 1)
			{
				return;
			}
			this.Jump2FocusContentItem();
		}

		private void OnClickTalentLegacyPreview(object sender, int type, BaseEventArgs eventargs)
		{
			this.Obj_Talent.SetActiveSafe(false);
			this.Obj_TalentLegacy.SetActiveSafe(true);
		}

		private int SortContentItemList(TalentEvolutionContentItem a, TalentEvolutionContentItem b)
		{
			return b.index.CompareTo(a.index);
		}

		private void Jump2FocusContentItem()
		{
			this.m_skipIndex = this.curContentItemIndex - 1;
			this.loopListContent.MoveToTargetItem(this.curContentItemIndex - 1, 0.5f);
		}

		public UIPopCommon uiPopCommon;

		public LoopListView2 loopListTitle;

		public LoopListView2 loopListContent;

		public CustomButton btnTop2Focus;

		public CustomButton btnBottom2Focus;

		public List<TalentLegacyPreViewItem> CareerItemList = new List<TalentLegacyPreViewItem>();

		public GameObject Obj_Talent;

		public GameObject Obj_TalentLegacy;

		public CustomTextScrollView Text_TalentLegacyDesc;

		private List<TalentEvolutionPreviewViewModule.TitleNodeData> titleNodeDatas = new List<TalentEvolutionPreviewViewModule.TitleNodeData>();

		private List<TalentEvolutionPreviewViewModule.ContentNodeData> contentNodeDatas = new List<TalentEvolutionPreviewViewModule.ContentNodeData>();

		private Dictionary<int, CustomBehaviour> titleItemsDict = new Dictionary<int, CustomBehaviour>();

		private Dictionary<int, CustomBehaviour> contentItemsDict = new Dictionary<int, CustomBehaviour>();

		private TalentDataModule talentDataModule;

		private int curContentItemIndex;

		private int m_skipIndex = -1;

		public class TitleNodeData
		{
			public bool isTopSpace;

			public bool isBottomSpace;

			public bool isTalentLegacy;

			public TalentNew_talentMegaStage cfg;
		}

		public class ContentNodeData
		{
			public bool isTopSpace;

			public bool isBottomSpace;

			public TalentNew_talent cfg;
		}
	}
}
