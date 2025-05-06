using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class AttributeTypeItem_Image : AttributeTypeItemBase
	{
		protected override void OnInit()
		{
			ComponentRegister component = this.m_gameObject.GetComponent<ComponentRegister>();
			this.m_image = component.GetGameObject("Image").GetComponent<CustomImage>();
			this.m_text = component.GetGameObject("Text").GetComponent<CustomText>();
			this.iconNode = component.GetGameObject("IconNode").GetComponent<RectTransform>();
		}

		protected override void OnSetData(AttributeTypeDataBase typeData, string commaString)
		{
			typeData.SetImage(this.m_image);
			this.m_text.text = typeData.m_value;
			this.m_width = this.iconNode.sizeDelta.x + this.m_text.preferredWidth;
		}

		protected override void OnDeInit()
		{
		}

		public override GameObject GetFlyItem()
		{
			return this.m_image.gameObject;
		}

		public CustomImage m_image;

		public CustomText m_text;

		public RectTransform iconNode;
	}
}
