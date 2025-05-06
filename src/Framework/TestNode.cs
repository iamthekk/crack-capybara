using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNode.GameEvent;

public class TestNode : MonoBehaviour
{
	private void Start()
	{
		Node node = this.graph.nodes[0];
		if (node is GameEventNormalNode)
		{
			GameEventNormalNode gameEventNormalNode = node as GameEventNormalNode;
			NodePort outputPort = gameEventNormalNode.GetOutputPort("exit");
			if (!outputPort.IsConnected)
			{
				HLog.LogError("Node is not connected");
				return;
			}
			List<NodePort> connections = outputPort.GetConnections();
			for (int i = 0; i < connections.Count; i++)
			{
				if (connections[i].node is GameEventSelectNode)
				{
					Node node2 = connections[i].node;
				}
				else if (connections[i].node is GameEventIFNode)
				{
					Node node3 = connections[i].node;
					gameEventNormalNode.GetOutputPort("exit");
				}
			}
		}
	}

	public GameEventGraph graph;
}
