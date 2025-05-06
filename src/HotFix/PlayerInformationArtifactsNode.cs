using System;
using Framework.Logic.Component;
using Proto.User;

namespace HotFix
{
	public class PlayerInformationArtifactsNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.artifactItemInfo.Init();
		}

		protected override void OnDeInit()
		{
			this.artifactItemInfo.DeInit();
		}

		public void RefreshUI(PlayerInfoDto playerInfo)
		{
			this.artifactItemInfo.RefreshUI(playerInfo.ArtifactInfo);
		}

		public PlayerInformation_ArtifactItemInfo artifactItemInfo;
	}
}
