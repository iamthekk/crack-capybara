using System;
using Framework.Logic.Component;

namespace HotFix.GuildUI
{
	public class GuildBossTaskRewardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Ctrl_Item.Init();
		}

		protected override void OnDeInit()
		{
			this.Ctrl_Item.DeInit();
		}

		public void SetData(PropData propData)
		{
			this.Ctrl_Item.SetData(propData);
			this.Ctrl_Item.OnRefresh();
		}

		public UIItem Ctrl_Item;
	}
}
