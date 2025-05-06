using System;
using LocalModels.Bean;

namespace HotFix
{
	public class UIItemLogicPropHourglass : BaseUIItemLogic
	{
		public override void OnRefreshCustom()
		{
			if (this.m_item == null)
			{
				return;
			}
			this.m_item.CreateHeader();
			if (this.m_item.m_headerParent != null)
			{
				this.m_item.m_headerParent.gameObject.SetActive(true);
			}
			ItemGift_ItemGift itemGift = base.m_tableData.GetItemGift();
			if (itemGift == null)
			{
				return;
			}
			this.m_item.m_header.SetHeaderValue((long)itemGift.seconds);
		}
	}
}
