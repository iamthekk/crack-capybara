using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.ViewModule;
using Server;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class EquipSelectorViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.m_scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as EquipSelectorViewModule.OpenData;
			EquipDataModule dataModule = GameApp.Data.GetDataModule(DataName.EquipDataModule);
			this.m_datas = dataModule.GetEquipDatasFoSelector((int)this.m_openData.m_equiptype);
			if (this.m_datas != null && this.m_datas.Count > 0)
			{
				if (this.m_emptyEquipNode != null)
				{
					this.m_emptyEquipNode.SetActive(false);
				}
				if (this.m_scroll != null)
				{
					this.m_scroll.gameObject.SetActive(true);
					this.m_scroll.SetListItemCount(this.m_datas.Count, true);
					this.m_scroll.RefreshAllShownItem();
					return;
				}
			}
			else
			{
				if (this.m_emptyEquipNode != null)
				{
					this.m_emptyEquipNode.SetActive(true);
				}
				if (this.m_scroll != null)
				{
					this.m_scroll.gameObject.SetActive(false);
				}
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_openData = null;
		}

		public override void OnDelete()
		{
			this.uiPopCommon.OnClick = null;
			foreach (KeyValuePair<int, UIEquipSelectorNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.m_nodes.Clear();
			this.m_datas = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 scroll, int index)
		{
			if (this.m_openData == null)
			{
				return null;
			}
			if (index < 0 || index >= this.m_datas.Count)
			{
				return null;
			}
			EquipData equipData = this.m_datas[index];
			LoopListViewItem2 loopListViewItem = scroll.NewListViewItem("Item");
			if (loopListViewItem == null)
			{
				return null;
			}
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UIEquipSelectorNode component;
			this.m_nodes.TryGetValue(instanceID, out component);
			if (component == null)
			{
				component = loopListViewItem.gameObject.GetComponent<UIEquipSelectorNode>();
				component.Init();
			}
			this.m_nodes[instanceID] = component;
			component.RefreshData(equipData);
			return loopListViewItem;
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickCloseBt();
			}
		}

		private void OnClickCloseBt()
		{
			GameApp.View.CloseView(ViewName.EquipSelectorViewModule, null);
		}

		public UIPopCommon uiPopCommon;

		public GameObject m_emptyEquipNode;

		public LoopListView2 m_scroll;

		public Dictionary<int, UIEquipSelectorNode> m_nodes = new Dictionary<int, UIEquipSelectorNode>();

		public EquipSelectorViewModule.OpenData m_openData;

		public List<EquipData> m_datas;

		public class OpenData
		{
			public EquipType m_equiptype;
		}
	}
}
