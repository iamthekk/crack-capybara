using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class CommonCostItem : CustomBehaviour
	{
		public void SetSimpleText(string text)
		{
			this.m_icon.enabled = false;
			if (this.m_valueTxt != null)
			{
				this.m_valueTxt.text = text;
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.rectTransform);
		}

		public void SetData(ItemData itemData)
		{
			this.SetImage(itemData.ID);
			this.SetCount(itemData.TotalCount);
		}

		public void SetData(int itemId, long costCount)
		{
			this.SetImage(itemId);
			this.SetCount(costCount);
		}

		public void SetData(ItemData itemData, long currentCount, long costCount)
		{
			this.SetImage(itemData.ID);
			this.SetCount(currentCount, costCount);
		}

		public void SetData(int itemID, long currentCount, long costCount)
		{
			this.SetImage(itemID);
			this.SetCount(currentCount, costCount);
		}

		public void SetCustomStyle1(string formatStr, ItemData itemData, long curCount, bool showImg = true)
		{
			if (this.m_valueTxt != null)
			{
				string text = string.Format(formatStr, itemData.TotalCount);
				if (curCount >= itemData.TotalCount)
				{
					this.m_valueTxt.text = text;
				}
				else
				{
					this.m_valueTxt.text = "<color=#ff0000>" + text + "</color>";
				}
				LayoutRebuilder.ForceRebuildLayoutImmediate(base.rectTransform);
			}
			this.m_icon.enabled = showImg;
			if (showImg)
			{
				this.SetImage(itemData.ID);
			}
		}

		public void SetImage(int itemId)
		{
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("itemId:{0} can't find in item table, please check", itemId));
				return;
			}
			this.m_itemID = itemId;
			if (this.m_icon != null)
			{
				this.m_icon.enabled = true;
				this.m_icon.SetImage(elementById.atlasID, elementById.icon);
			}
		}

		private void SetCount(long costCount)
		{
			if (this.m_valueTxt != null)
			{
				this.m_valueTxt.text = DxxTools.FormatNumber(costCount) ?? "";
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.rectTransform);
		}

		private void SetCount(long currentCount, long costCount)
		{
			string text = ((currentCount >= costCount) ? ("<color=#d3f24e>" + DxxTools.FormatNumber(currentCount) + "</color>") : ("<color=#ff0000>" + DxxTools.FormatNumber(currentCount) + "</color>"));
			if (this.m_valueTxt != null)
			{
				this.m_valueTxt.text = text + "/" + DxxTools.FormatNumber(costCount);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.rectTransform);
		}

		private void OnClickItem()
		{
			FrameworkExpand.DispatchItemNotEnoughEvent(this.m_itemID, true);
		}

		protected override void OnInit()
		{
			if (this.m_button_item != null)
			{
				this.m_button_item.m_onClick = new Action(this.OnClickItem);
			}
		}

		protected override void OnDeInit()
		{
			if (this.m_button_item != null)
			{
				this.m_button_item.m_onClick = null;
			}
		}

		[SerializeField]
		private CustomImage m_icon;

		[SerializeField]
		private CustomText m_valueTxt;

		[SerializeField]
		private CustomButton m_button_item;

		private int m_itemID;
	}
}
