using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacySkillViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.PopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnClickClose);
			this.m_talentLegacyDataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			this.Button_SkillTabItem.SetActiveSafe(false);
			this.ListView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemIndex), null);
		}

		private void OnClickClose(UIPopCommon.UIPopCommonClickType obj)
		{
			GameApp.View.CloseView(ViewName.TalentLegacySkillViewModule, null);
		}

		public override void OnOpen(object data)
		{
			this.m_openData = (TalentLegacySkillViewModule.OpenData)data;
			if (this.m_talentLegacyDataModule == null)
			{
				return;
			}
			this.m_talentLegacyInfo = this.m_talentLegacyDataModule.OnGetTalentLegacyInfo();
			if (this.m_talentLegacyInfo == null)
			{
				return;
			}
			this.m_tabCfgList = new List<TalentLegacy_career>();
			IList<TalentLegacy_career> talentLegacy_careerElements = GameApp.Table.GetManager().GetTalentLegacy_careerElements();
			for (int i = 0; i < talentLegacy_careerElements.Count; i++)
			{
				if (talentLegacy_careerElements[i].isOpen == 1)
				{
					this.m_tabCfgList.Add(talentLegacy_careerElements[i]);
				}
			}
			if (this.m_openData.SelectCareerId <= 0)
			{
				this.m_openData.SelectCareerId = this.m_openData.CareerId;
			}
			this.OnRefreshTabList();
			this.OnRefreshList(this.m_openData.SelectCareerId, false);
		}

		private void OnRefreshTabList()
		{
			if (this.m_tabItemList.Count == this.m_tabCfgList.Count)
			{
				for (int i = 0; i < this.m_tabItemList.Count; i++)
				{
					this.m_tabItemList[i].SetData(this.m_tabCfgList[i].id, this.m_openData.State, this.m_openData.CareerId, this.m_openData.Index);
					this.m_tabItemList[i].GetComponent<CustomChooseButton>().OnClickButton = new Action<CustomChooseButton>(this.OnClickTabItem);
					if (this.m_openData.SelectCareerId == this.m_tabCfgList[i].id)
					{
						this.m_tabItemList[i].GetComponent<CustomChooseButton>().m_imageTarget.gameObject.SetActiveSafe(true);
					}
					else
					{
						this.m_tabItemList[i].GetComponent<CustomChooseButton>().m_imageTarget.gameObject.SetActiveSafe(false);
					}
				}
				return;
			}
			for (int j = 0; j < this.m_tabCfgList.Count; j++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.Button_SkillTabItem, this.Obj_TabParent.transform);
				gameObject.SetActiveSafe(true);
				TalentLegacySkillTabItem component = gameObject.GetComponent<TalentLegacySkillTabItem>();
				component.Init();
				component.SetData(this.m_tabCfgList[j].id, this.m_openData.State, this.m_openData.CareerId, this.m_openData.Index);
				gameObject.GetComponent<CustomChooseButton>().OnClickButton = new Action<CustomChooseButton>(this.OnClickTabItem);
				if (this.m_openData.SelectCareerId == this.m_tabCfgList[j].id)
				{
					gameObject.GetComponent<CustomChooseButton>().m_imageTarget.gameObject.SetActiveSafe(true);
				}
				else
				{
					gameObject.GetComponent<CustomChooseButton>().m_imageTarget.gameObject.SetActiveSafe(false);
				}
				this.m_tabItemList.Add(component);
			}
		}

		private void OnRefreshList(int careerId, bool isForceRefresh = false)
		{
			if (!isForceRefresh && careerId == this.m_curCareerId)
			{
				return;
			}
			this.m_curCareerId = careerId;
			this.m_cfgList.Clear();
			this.m_cfgList.AddRange(this.m_talentLegacyDataModule.GetCareerTalentLegacyListAllCfg(careerId, 3));
			this.ListView.SetListItemCount(this.m_cfgList.Count, true);
			this.ListView.RefreshAllShowItems();
		}

		private LoopListViewItem2 OnGetItemIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			if (index > this.m_cfgList.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("TalentLegacySkillItem");
			TalentLegacySkillItem component;
			this.m_skillItemDic.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = loopListViewItem.GetComponent<TalentLegacySkillItem>();
				component.Init();
				this.m_skillItemDic[loopListViewItem.gameObject.GetInstanceID()] = component;
			}
			component.SetData(this.m_openData.CareerId, this.m_cfgList[index].id, this.m_openData.Index);
			return loopListViewItem;
		}

		private void OnClickTabItem(CustomChooseButton obj)
		{
			int num = this.m_openData.CareerId;
			TalentLegacySkillTabItem component = obj.GetComponent<TalentLegacySkillTabItem>();
			if (component != null)
			{
				num = component.CareerId;
			}
			if (this.m_talentLegacyDataModule.IsUnlockTalentLegacyCareer(num))
			{
				for (int i = 0; i < this.m_tabItemList.Count; i++)
				{
					this.m_tabItemList[i].GetComponent<CustomChooseButton>().m_imageTarget.gameObject.SetActiveSafe(false);
				}
				obj.m_imageTarget.gameObject.SetActiveSafe(true);
				this.OnRefreshList(num, false);
				return;
			}
			TalentLegacy_career talentLegacy_career = GameApp.Table.GetManager().GetTalentLegacy_career(num);
			if (talentLegacy_career == null)
			{
				return;
			}
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacy_career.unLockTips));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_curCareerId = -1;
		}

		public override void OnDelete()
		{
			for (int i = 0; i < this.m_tabItemList.Count; i++)
			{
				this.m_tabItemList[i].DeInit();
			}
			foreach (KeyValuePair<int, TalentLegacySkillItem> keyValuePair in this.m_skillItemDic)
			{
				keyValuePair.Value.DeInit();
			}
		}

		private void OnTalentLegacySkillChange(object sender, int type, BaseEventArgs eventargs)
		{
			TalentLegacyDataModule.TalentLegacyCareerInfo talentLegacyCareerInfo = this.m_talentLegacyDataModule.OnGetTalentLegacyCareerInfo(this.m_openData.CareerId);
			if (this.m_talentLegacyInfo.AssemblySlotCount > this.m_openData.Index)
			{
				if (talentLegacyCareerInfo.AssemblyTalentLegacySkillIdList.Count > this.m_openData.Index && talentLegacyCareerInfo.AssemblyTalentLegacySkillIdList[this.m_openData.Index] != 0)
				{
					this.m_openData.State = 2;
				}
				else
				{
					this.m_openData.State = 1;
				}
			}
			this.OnRefreshTabList();
			this.OnRefreshList(this.m_curCareerId, true);
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TalentLegacySkillChange, new HandlerEvent(this.OnTalentLegacySkillChange));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TalentLegacySkillChange, new HandlerEvent(this.OnTalentLegacySkillChange));
		}

		public UIPopCommon PopCommon;

		public GameObject Obj_TabParent;

		public GameObject Button_SkillTabItem;

		public LoopListView2 ListView;

		private TalentLegacySkillViewModule.OpenData m_openData;

		private TalentLegacyDataModule m_talentLegacyDataModule;

		private TalentLegacyDataModule.TalentLegacyInfo m_talentLegacyInfo;

		private Dictionary<int, TalentLegacySkillItem> m_skillItemDic = new Dictionary<int, TalentLegacySkillItem>();

		private List<TalentLegacy_talentLegacyNode> m_cfgList = new List<TalentLegacy_talentLegacyNode>();

		private List<TalentLegacySkillTabItem> m_tabItemList = new List<TalentLegacySkillTabItem>();

		private int m_curCareerId = -1;

		private List<TalentLegacy_career> m_tabCfgList = new List<TalentLegacy_career>();

		public struct OpenData
		{
			public int CareerId;

			public int Index;

			public int SelectCareerId;

			public int State;
		}
	}
}
