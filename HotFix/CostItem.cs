using System;
using Framework;
using Framework.Logic.Component;

namespace HotFix
{
	public class CostItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.uiItem.Init();
		}

		protected override void OnDeInit()
		{
			this.uiItem.DeInit();
		}

		public void SetData(ItemData itemData)
		{
			this.uiItem.SetData(itemData.ToPropData());
			this.uiItem.OnRefresh();
			long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)itemData.ID));
			long totalCount = itemData.TotalCount;
			string text;
			if (itemDataCountByid >= totalCount)
			{
				text = DxxTools.FormatNumber(itemDataCountByid) + "/" + DxxTools.FormatNumber(totalCount);
			}
			else
			{
				text = "<color=#FF6175>" + DxxTools.FormatNumber(itemDataCountByid) + "</color>/" + DxxTools.FormatNumber(totalCount);
			}
			this.uiItem.SetCountText(text);
		}

		public UIItem uiItem;
	}
}
