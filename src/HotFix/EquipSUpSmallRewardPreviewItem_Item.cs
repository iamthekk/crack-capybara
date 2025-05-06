using System;
using Framework.Logic.Component;

namespace HotFix
{
	public class EquipSUpSmallRewardPreviewItem_Item : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.item.Init();
		}

		protected override void OnDeInit()
		{
			this.item.DeInit();
		}

		public void UpdateData(int index, object data)
		{
			ItemData itemData = new ItemData((int)data, 1L);
			this.item.SetData(itemData.ToPropData());
			this.item.OnRefresh();
		}

		public UIItem item;
	}
}
