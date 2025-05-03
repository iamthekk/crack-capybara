using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIRelicDetailsAttributeGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_titlePrefab.SetActive(false);
			this.m_attributeNextNodePrefab.SetActive(false);
			this.m_attributeNodePrefab.SetActive(false);
			this.m_noteNodePrefab.SetActive(false);
			this.m_spaceNodePrefab.SetActive(false);
		}

		protected override void OnDeInit()
		{
			this.DestroyUI();
		}

		public void RefreshData(List<UIRelicDetailsAttributeGroup.NodeData> nodeDatas)
		{
			this.m_nodeDatas = nodeDatas;
			this.DestroyUI();
			this.CreateUI();
		}

		private void DestroyUI()
		{
			foreach (KeyValuePair<int, UIRelicDetailsAttributeNode> keyValuePair in this.m_dic)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_dic.Clear();
		}

		private void CreateUI()
		{
			if (this.m_nodeDatas == null)
			{
				return;
			}
			for (int i = 0; i < this.m_nodeDatas.Count; i++)
			{
				UIRelicDetailsAttributeGroup.NodeData nodeData = this.m_nodeDatas[i];
				if (nodeData != null)
				{
					UIRelicDetailsAttributeNode uirelicDetailsAttributeNode = null;
					switch (nodeData.m_nodeType)
					{
					case UIRelicDetailsAttributeGroup.NodeType.Title:
						uirelicDetailsAttributeNode = this.m_titlePrefab;
						break;
					case UIRelicDetailsAttributeGroup.NodeType.AttributeNext:
						uirelicDetailsAttributeNode = this.m_attributeNextNodePrefab;
						break;
					case UIRelicDetailsAttributeGroup.NodeType.Attribute:
						uirelicDetailsAttributeNode = this.m_attributeNodePrefab;
						break;
					case UIRelicDetailsAttributeGroup.NodeType.Note:
						uirelicDetailsAttributeNode = this.m_noteNodePrefab;
						break;
					case UIRelicDetailsAttributeGroup.NodeType.Space:
						uirelicDetailsAttributeNode = this.m_spaceNodePrefab;
						break;
					}
					UIRelicDetailsAttributeNode uirelicDetailsAttributeNode2 = Object.Instantiate<UIRelicDetailsAttributeNode>(uirelicDetailsAttributeNode);
					uirelicDetailsAttributeNode2.transform.SetParentNormal(this.m_parent, false);
					uirelicDetailsAttributeNode2.SetActive(true);
					uirelicDetailsAttributeNode2.Init();
					uirelicDetailsAttributeNode2.RefreshData(nodeData);
					this.m_dic[uirelicDetailsAttributeNode2.GetObjectInstanceID()] = uirelicDetailsAttributeNode2;
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_parent);
		}

		public void PlayParticleSystems(int layer)
		{
			foreach (KeyValuePair<int, UIRelicDetailsAttributeNode> keyValuePair in this.m_dic)
			{
				if (!(keyValuePair.Value == null) && keyValuePair.Value.m_data != null && keyValuePair.Value.m_data.m_layer == layer && (keyValuePair.Value.m_data.m_nodeType == UIRelicDetailsAttributeGroup.NodeType.Attribute || keyValuePair.Value.m_data.m_nodeType == UIRelicDetailsAttributeGroup.NodeType.AttributeNext))
				{
					keyValuePair.Value.PlayParticleSystem();
				}
			}
		}

		public RectTransform m_parent;

		public UIRelicDetailsAttributeNode m_titlePrefab;

		public UIRelicDetailsAttributeNode m_attributeNextNodePrefab;

		public UIRelicDetailsAttributeNode m_attributeNodePrefab;

		public UIRelicDetailsAttributeNode m_noteNodePrefab;

		public UIRelicDetailsAttributeNode m_spaceNodePrefab;

		public List<UIRelicDetailsAttributeGroup.NodeData> m_nodeDatas;

		public Dictionary<int, UIRelicDetailsAttributeNode> m_dic = new Dictionary<int, UIRelicDetailsAttributeNode>();

		public class NodeData
		{
			public UIRelicDetailsAttributeGroup.NodeType m_nodeType;

			public string m_name;

			public string m_from;

			public string m_to;

			public int m_layer;
		}

		public enum NodeType
		{
			Title,
			AttributeNext,
			Attribute,
			Note,
			Space
		}
	}
}
