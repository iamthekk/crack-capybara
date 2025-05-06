using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class AttributeShowNodeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.attributeShowAttributeItem.gameObject.SetActive(false);
		}

		protected override void OnDeInit()
		{
			foreach (AttributeShowAttributeItem attributeShowAttributeItem in this.items)
			{
				attributeShowAttributeItem.DeInit();
			}
			this.items.Clear();
		}

		public void SetData(AttributeShowViewModule.ItemNodeData data)
		{
			this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(data.titleId);
			for (int i = 0; i < data.attributeList.Count; i++)
			{
				AttributeShowViewModule.AttributeShowItemData attributeShowItemData = data.attributeList[i];
				AttributeShowAttributeItem attributeShowAttributeItem = ((this.items.Count > i) ? this.items[i] : null);
				if (attributeShowAttributeItem == null)
				{
					attributeShowAttributeItem = Object.Instantiate<AttributeShowAttributeItem>(this.attributeShowAttributeItem, this.attributeShowAttributeItem.transform.parent, false);
					this.items.Add(attributeShowAttributeItem);
				}
				attributeShowAttributeItem.gameObject.SetActive(true);
				attributeShowAttributeItem.SetData(attributeShowItemData, i);
			}
		}

		public CustomText txtTitle;

		public AttributeShowAttributeItem attributeShowAttributeItem;

		private List<AttributeShowAttributeItem> items = new List<AttributeShowAttributeItem>();
	}
}
