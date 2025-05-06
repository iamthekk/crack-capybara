using System;
using System.Collections.Generic;
using Framework.Logic.Platform;
using UnityEngine;

namespace Framework.Logic.UI.WindowHelper
{
	public class UGUIBottomWindowHelper : MonoBehaviour
	{
		private void Awake()
		{
			this.OnRefresh();
		}

		public void OnRefresh()
		{
			if (this.list == null || this.list.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this.list.Count; i++)
			{
				RectTransform rectTransform = this.list[i];
				if (!(rectTransform == null))
				{
					rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + Singleton<PlatformHelper>.Instance.GetBottomHeight());
				}
			}
		}

		public List<RectTransform> list;
	}
}
