using System;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class IAPDiamondsSubTab : IAPTabBase<IAPDiamondsType>
	{
		protected override void OnSelect()
		{
			base.OnSelect();
			this.tabBgImage.sprite = this.tabBgSelectSprite;
			this.tabInfoText.color = Color.white;
			this.tabInfoTextOutline.effectColor = new Color(0.66f, 0.38f, 0.08f);
		}

		protected override void OnUnSelect()
		{
			base.OnUnSelect();
			this.tabBgImage.sprite = this.tabBgUnSelectSprite;
			this.tabInfoText.color = new Color(0.83f, 0.83f, 0.83f);
			this.tabInfoTextOutline.effectColor = new Color(0.38f, 0.38f, 0.38f);
		}

		[SerializeField]
		private CustomImage tabBgImage;

		[SerializeField]
		private Sprite tabBgSelectSprite;

		[SerializeField]
		private Sprite tabBgUnSelectSprite;

		[SerializeField]
		private CustomLanguageText tabInfoText;

		[SerializeField]
		private CustomOutLine tabInfoTextOutline;
	}
}
