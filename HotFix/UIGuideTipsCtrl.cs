using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIGuideTipsCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetTips(string id)
		{
			this.TextTips.text = Singleton<LanguageManager>.Instance.GetInfoByID(id);
			Vector2 sizeDelta = this.RTFSize.sizeDelta;
			sizeDelta.y = this.TextTips.preferredHeight + 42f;
			this.RTFSize.sizeDelta = sizeDelta;
		}

		public RectTransform RTFSize;

		public CustomText TextTips;
	}
}
