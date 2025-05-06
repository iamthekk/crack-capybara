using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using SuperScrollView;

namespace HotFix
{
	public class AttributeDetailedViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			if (this.m_scroll != null)
			{
				this.m_scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			}
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as AttributeDetailedViewModule.OpenData;
			if (this.m_openData == null)
			{
				HLog.LogError("AttributeDetailedViewModule.Open Data is null!");
				return;
			}
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			if (this.m_titleTxt != null && !string.IsNullOrEmpty(this.m_openData.m_titleLanguageID))
			{
				this.m_titleTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.m_openData.m_titleLanguageID);
			}
			if (this.m_openData.m_datas != null && this.m_scroll != null)
			{
				this.m_scroll.SetListItemCount(this.m_openData.m_datas.Count, true);
				this.m_scroll.RefreshAllShownItem();
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
			foreach (KeyValuePair<int, AttributeDetailedNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.m_nodes.Clear();
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
			if (this.m_openData.m_datas == null)
			{
				return null;
			}
			if (index < 0 || index >= this.m_openData.m_datas.Count)
			{
				return null;
			}
			AttributeDetailedViewModule.Data data = this.m_openData.m_datas[index];
			AttributeDetailedViewModule.Type type = data.m_type;
			LoopListViewItem2 loopListViewItem;
			if (type != AttributeDetailedViewModule.Type.Attrubute)
			{
				if (type != AttributeDetailedViewModule.Type.Combat)
				{
					loopListViewItem = scroll.NewListViewItem("prefab_attribute");
				}
				else
				{
					loopListViewItem = scroll.NewListViewItem("prefab_CombatAttribute");
				}
			}
			else
			{
				loopListViewItem = scroll.NewListViewItem("prefab_attribute");
			}
			if (loopListViewItem == null)
			{
				return null;
			}
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			AttributeDetailedNode component;
			this.m_nodes.TryGetValue(instanceID, out component);
			if (component == null)
			{
				component = loopListViewItem.gameObject.GetComponent<AttributeDetailedNode>();
				component.Init();
			}
			this.m_nodes[instanceID] = component;
			component.RefreshData(data, index % 2 == 0);
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
			GameApp.View.CloseView(ViewName.AttributeDetailedViewModule, null);
		}

		public UIPopCommon uiPopCommon;

		public CustomText m_titleTxt;

		public LoopListView2 m_scroll;

		public Dictionary<int, AttributeDetailedNode> m_nodes = new Dictionary<int, AttributeDetailedNode>();

		public AttributeDetailedViewModule.OpenData m_openData;

		public class OpenData
		{
			public string m_titleLanguageID;

			public List<AttributeDetailedViewModule.Data> m_datas;
		}

		public class Data
		{
			public override string ToString()
			{
				return string.Format("m_type = {0} m_name = {1} m_to = {2}", this.m_type, this.m_name, this.m_to);
			}

			public AttributeDetailedViewModule.Type m_type = AttributeDetailedViewModule.Type.Attrubute;

			public string m_name;

			public string m_to;
		}

		public enum Type
		{
			Attrubute = 1,
			Combat
		}
	}
}
