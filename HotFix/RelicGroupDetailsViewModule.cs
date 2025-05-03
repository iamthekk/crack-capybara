using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class RelicGroupDetailsViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_relicDataModule = GameApp.Data.GetDataModule(DataName.RelicDataModule);
			if (this.m_nodePrefab != null)
			{
				this.m_nodePrefab.SetActive(false);
			}
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as RelicGroupDetailsViewModule.OpenData;
			if (this.m_openData == null)
			{
				return;
			}
			Relic_group elementById = GameApp.Table.GetManager().GetRelic_groupModelInstance().GetElementById(this.m_openData.m_goupIO);
			if (elementById == null)
			{
				return;
			}
			if (this.m_title != null)
			{
				this.m_title.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.NameId);
			}
			if (this.m_noteTitle != null)
			{
				this.m_noteTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.DescId);
			}
			bool flag;
			int groupMinQuality = this.m_relicDataModule.GetGroupMinQuality(this.m_openData.m_goupIO, out flag);
			foreach (KeyValuePair<int, List<MergeAttributeData>> keyValuePair in this.m_relicDataModule.GetGroupAttributesByGroup(this.m_openData.m_goupIO))
			{
				UIRelicGroupDetailsNode uirelicGroupDetailsNode = Object.Instantiate<UIRelicGroupDetailsNode>(this.m_nodePrefab);
				uirelicGroupDetailsNode.transform.SetParentNormal(this.m_nodeParent, false);
				bool flag2 = !flag || groupMinQuality < keyValuePair.Key;
				uirelicGroupDetailsNode.SetData(keyValuePair.Key, flag2, keyValuePair.Value);
				uirelicGroupDetailsNode.SetActive(true);
				uirelicGroupDetailsNode.Init();
				this.m_nodes[uirelicGroupDetailsNode.GetObjectInstanceID()] = uirelicGroupDetailsNode;
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.m_nodeParent);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_nodeParent);
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.uiPopCommon.OnClick = null;
			foreach (KeyValuePair<int, UIRelicGroupDetailsNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_nodes.Clear();
		}

		public override void OnDelete()
		{
			this.m_relicDataModule = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickCloseBtn();
			}
		}

		private void OnClickCloseBtn()
		{
			GameApp.View.CloseView(ViewName.RelicGroupDetailsViewModule, null);
		}

		public RelicGroupDetailsViewModule.OpenData m_openData;

		public UIPopCommon uiPopCommon;

		public CustomText m_title;

		public CustomText m_noteTitle;

		public RectTransform m_nodeParent;

		public UIRelicGroupDetailsNode m_nodePrefab;

		public Dictionary<int, UIRelicGroupDetailsNode> m_nodes = new Dictionary<int, UIRelicGroupDetailsNode>();

		private RelicDataModule m_relicDataModule;

		public class OpenData
		{
			public int m_goupIO;
		}
	}
}
