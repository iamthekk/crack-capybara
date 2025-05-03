using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.VIPUI
{
	public class VIPRewardsTitleLayoutUI : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void RefreshUI()
		{
			float num = this.TextTitle.preferredWidth + 20f;
			float num2 = (this.RTFRoot.rect.width - num - 140f) / 2f;
			Vector2 vector = this.TextTitle.rectTransform.sizeDelta;
			vector.x = num;
			this.TextTitle.rectTransform.sizeDelta = vector;
			vector = this.RTFLineLeft.sizeDelta;
			vector.x = num2;
			this.RTFLineLeft.sizeDelta = vector;
			vector = this.RTFLineRight.sizeDelta;
			vector.x = num2;
			this.RTFLineRight.sizeDelta = vector;
		}

		public RectTransform RTFRoot;

		public RectTransform RTFLineLeft;

		public RectTransform RTFLineRight;

		public CustomLanguageText TextTitle;
	}
}
