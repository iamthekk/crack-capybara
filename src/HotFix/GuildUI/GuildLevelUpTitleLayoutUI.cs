using System;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildLevelUpTitleLayoutUI : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public void RefreshUI(string text)
		{
		}

		public RectTransform RTFRoot;

		public RectTransform RTFLineLeft;

		public RectTransform RTFLineRight;
	}
}
