using System;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class IAPShopTab : IAPTabBase<IAPShopType>
	{
		protected override void OnSelect()
		{
			base.OnSelect();
			this.tabBgImage.sprite = this.tabBgSelectSprite;
			this.unSelectMask.SetActive(false);
			base.rectTransform.sizeDelta = this.selectSize;
		}

		protected override void OnUnSelect()
		{
			base.OnUnSelect();
			this.tabBgImage.sprite = this.tabBgUnSelectSprite;
			this.unSelectMask.SetActive(true);
			base.rectTransform.sizeDelta = this.unSelectSize;
		}

		[SerializeField]
		private CustomImage tabBgImage;

		[SerializeField]
		private Sprite tabBgSelectSprite;

		[SerializeField]
		private Sprite tabBgUnSelectSprite;

		[SerializeField]
		private GameObject unSelectMask;

		[SerializeField]
		private Vector2 selectSize;

		[SerializeField]
		private Vector2 unSelectSize;
	}
}
