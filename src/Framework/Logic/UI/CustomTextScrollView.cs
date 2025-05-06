using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class CustomTextScrollView : ScrollRect
	{
		public void SetText(string text, bool autoVertical = true)
		{
			this.m_text.text = text;
			RectTransform rectTransform = this.m_text.transform.parent.transform as RectTransform;
			RectTransform rectTransform2 = base.transform as RectTransform;
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			if (autoVertical)
			{
				if (rectTransform.rect.height > rectTransform2.rect.height)
				{
					base.vertical = true;
				}
				else
				{
					base.vertical = false;
				}
			}
			if (rectTransform.rect.height <= rectTransform2.rect.height)
			{
				rectTransform.anchoredPosition = Vector2.zero;
			}
		}

		public CustomText m_text;

		public VerticalLayoutGroup m_verticalLayoutGroup;
	}
}
