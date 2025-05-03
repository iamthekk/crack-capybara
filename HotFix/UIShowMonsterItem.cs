using System;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIShowMonsterItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.spineModelItem.Init();
		}

		protected override void OnDeInit()
		{
			this.spineModelItem.DeInit();
		}

		public void SetData(int memberId, long power)
		{
			this.powerCountText.text = DxxTools.FormatNumber(power);
			this.spineModelItem.ShowMemberModel(memberId, "Idle", true);
		}

		public UISpineModelItem spineModelItem;

		public CustomText powerCountText;
	}
}
