using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIMountItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(MountBasicData basicData)
		{
			if (basicData == null || basicData.MemberConfig == null)
			{
				return;
			}
			string atlasPath = GameApp.Table.GetAtlasPath(basicData.MemberConfig.iconAtlasID);
			this.imageIcon.SetImage(atlasPath, basicData.MemberConfig.iconSpriteName);
		}

		public void SetData(MountAdvanceData advanceData)
		{
			if (advanceData == null || advanceData.MemberConfig == null)
			{
				return;
			}
			string text = GameApp.Table.GetAtlasPath(advanceData.MemberConfig.iconAtlasID);
			this.imageIcon.SetImage(text, advanceData.MemberConfig.iconSpriteName);
			text = GameApp.Table.GetAtlasPath(105);
			this.imageQuality.SetImage(text, string.Format("item_frame_{0}", advanceData.Config.quality));
		}

		public CustomImage imageQuality;

		public CustomImage imageIcon;
	}
}
