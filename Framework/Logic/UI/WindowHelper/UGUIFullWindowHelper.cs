using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.UI.WindowHelper
{
	public class UGUIFullWindowHelper : MonoBehaviour
	{
		private void Awake()
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
					rectTransform.sizeDelta = Utility.UI.GetWindowSize();
				}
			}
		}

		public List<RectTransform> list;
	}
}
