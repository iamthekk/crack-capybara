using System;
using Framework.Logic.UI;

namespace HotFix.GuildUI
{
	public class GuildBoxGiftItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public CustomImage boxIcon;

		public CustomLanguageText giftFromText;

		public CustomLanguageText donorText;

		public CustomText keyCountText;

		public CustomText countDownText;

		public CustomButton collectBtn;
	}
}
