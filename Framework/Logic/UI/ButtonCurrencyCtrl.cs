using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class ButtonCurrencyCtrl : CustomButton
	{
		private RectTransform parentTransform
		{
			get
			{
				if (this.m_parent == null)
				{
					this.m_parent = this.Text_Value.transform.parent as RectTransform;
				}
				return this.m_parent;
			}
		}

		public void SetValue(int value)
		{
			this.Text_Value.text = value.ToString();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.parentTransform);
		}

		public CustomImage Image_Icon;

		public CustomText Text_Value;

		private RectTransform m_parent;
	}
}
