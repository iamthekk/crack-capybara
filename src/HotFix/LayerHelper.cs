using System;
using UnityEngine;

namespace HotFix
{
	public static class LayerHelper
	{
		public static void ChangeLayer(this GameObject obj, int layerId)
		{
			Transform[] componentsInChildren = obj.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = layerId;
			}
		}
	}
}
