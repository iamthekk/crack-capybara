using System;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class AttributeShowAttributeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(AttributeShowViewModule.AttributeShowItemData data, int index)
		{
			this.txtAttributeName.text = Singleton<LanguageManager>.Instance.GetInfoByID(data.attributeTable.LanguageId);
			double num = data.attributeValue.AsDouble();
			if (data.attributeTable.ID.Contains("%"))
			{
				num = Utility.Math.Round(num * 100.0, 2);
				this.txtAttributeValue.text = num.ToString() + "%";
			}
			else
			{
				num = Utility.Math.Round(num, 0);
				this.txtAttributeValue.text = DxxTools.FormatNumber((long)num);
			}
			this.imgBg.gameObject.SetActive(index % 2 != 0);
		}

		public CustomImage imgBg;

		public CustomText txtAttributeName;

		public CustomText txtAttributeValue;
	}
}
