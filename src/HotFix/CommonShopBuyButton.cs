using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class CommonShopBuyButton : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.customButton = base.GetComponent<CustomButton>();
			this.customButton.onClick.AddListener(new UnityAction(this.OnClick));
		}

		protected override void OnDeInit()
		{
			this.customButton.onClick.RemoveListener(new UnityAction(this.OnClick));
			this.customButton = null;
		}

		private void OnClick()
		{
			Action action = this.onClick;
			if (action == null)
			{
				return;
			}
			action();
		}

		public void SetData(Action onClickVal)
		{
			this.onClick = onClickVal;
		}

		public void RefreshView(string priceVal, string originalPriceVal, int currencyIDVal)
		{
			if (this.priceIcon != null)
			{
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(currencyIDVal);
				this.priceIcon.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
			}
			if (this.priceText != null)
			{
				this.priceText.text = priceVal;
			}
			if (this.originalPriceText != null)
			{
				this.originalPriceText.text = originalPriceVal;
			}
		}

		[SerializeField]
		private CustomText priceText;

		[SerializeField]
		private CustomText originalPriceText;

		[SerializeField]
		public CustomImage priceIcon;

		private CustomButton customButton;

		private Action onClick;
	}
}
