using System;
using Framework.Logic.Component;

namespace HotFix
{
	public class SevenDayCarnivalRewardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.uiItem.Init();
		}

		protected override void OnDeInit()
		{
			this.uiItem.DeInit();
		}

		public void RefreshUI(ItemData data)
		{
			this.uiItem.SetData(data.ToPropData());
			this.uiItem.OnRefresh();
		}

		public UIItem uiItem;
	}
}
