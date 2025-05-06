using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class UIBgText : MonoBehaviour
	{
		public void SetText(string info)
		{
			if (this.m_text != null)
			{
				this.m_text.text = info;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_text.rectTransform);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.transform as RectTransform);
		}

		public Text m_text;
	}
}
