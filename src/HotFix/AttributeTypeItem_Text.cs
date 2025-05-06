using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class AttributeTypeItem_Text : AttributeTypeItemBase
	{
		protected override void OnInit()
		{
			ComponentRegister component = this.m_gameObject.GetComponent<ComponentRegister>();
			this.m_text = component.GetGameObject("Text").GetComponent<CustomText>();
			this.imageBg = component.GetGameObject("ImageBg").GetComponent<RectTransform>();
		}

		protected override void OnSetData(AttributeTypeDataBase typeData, string commaString)
		{
			this.m_text.text = typeData.m_value;
			CustomText text = this.m_text;
			text.text += commaString;
			this.m_width = this.imageBg.sizeDelta.x;
		}

		protected override void OnSetString(string str)
		{
			this.m_text.text = str;
			this.m_width = this.m_text.preferredWidth;
		}

		protected override void OnDeInit()
		{
		}

		public CustomText m_text;

		public RectTransform imageBg;
	}
}
