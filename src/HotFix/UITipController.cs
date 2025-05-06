using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UITipController : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
			this.RemoveAllNodes();
		}

		public void AddNode(string info)
		{
			UITipControllerNode uitipControllerNode = Object.Instantiate<UITipControllerNode>(this.m_nodePrefab, this.m_parent.transform);
			Transform transform = uitipControllerNode.transform;
			transform.SetParent(this.m_parent.transform);
			transform.position = this.m_parent.transform.position;
			transform.localScale = Vector3.one;
			uitipControllerNode.m_onFinished = new Action<UITipControllerNode>(this.OnNodeFinished);
			uitipControllerNode.Init();
			uitipControllerNode.SetInfo(info);
			uitipControllerNode.gameObject.SetActive(true);
			this.m_nodes[uitipControllerNode.GetObjectInstanceID()] = uitipControllerNode;
		}

		public void RemoveNode(UITipControllerNode node)
		{
			if (node == null || node.gameObject == null)
			{
				return;
			}
			node.DeInit();
			this.m_nodes.Remove(node.gameObject.GetInstanceID());
			Object.Destroy(node.gameObject);
		}

		public void RemoveAllNodes()
		{
			foreach (KeyValuePair<int, UITipControllerNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null) && !(keyValuePair.Value.gameObject == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_nodes.Clear();
		}

		private void OnNodeFinished(UITipControllerNode node)
		{
			this.RemoveNode(node);
		}

		[SerializeField]
		private GameObject m_parent;

		[SerializeField]
		private UITipControllerNode m_nodePrefab;

		public Dictionary<int, UITipControllerNode> m_nodes = new Dictionary<int, UITipControllerNode>();
	}
}
