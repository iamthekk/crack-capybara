using System;
using Framework.Logic.Component;
using Proto.User;

namespace HotFix
{
	public class PlayerInformationMountsNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.mountItemInfo.Init();
		}

		protected override void OnDeInit()
		{
			this.mountItemInfo.DeInit();
		}

		public void RefreshUI(PlayerInfoDto playerInfo)
		{
			this.mountItemInfo.RefreshUI(playerInfo.MountInfo);
		}

		public PlayerInformation_MountItemInfo mountItemInfo;
	}
}
