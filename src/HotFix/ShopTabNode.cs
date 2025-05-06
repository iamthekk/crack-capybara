using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ShopTabNode : CustomBehaviour
	{
		public ShopType TabType { get; private set; }

		protected override void OnInit()
		{
			this.tabButton.onClick.AddListener(new UnityAction(this.OnClickNodeButton));
			IntegralShop_data integralShop_data;
			if (GameApp.Data.GetDataModule(DataName.ShopDataModule).GetShopConfig(this.TabType, out integralShop_data))
			{
				this.nameText.SetText(int.Parse(integralShop_data.NameID));
				return;
			}
			this.nameText.text = string.Empty;
		}

		protected override void OnDeInit()
		{
			this.tabButton.onClick.RemoveListener(new UnityAction(this.OnClickNodeButton));
		}

		private void OnClickNodeButton()
		{
			Action<ShopType> action = this.onClick;
			if (action == null)
			{
				return;
			}
			action(this.TabType);
		}

		public void SetData(bool initSelect, ShopType tabType, Action<ShopType> onClickVul)
		{
			this.onClick = onClickVul;
			this.TabType = tabType;
			this.DoSelect(initSelect);
		}

		public void SetSelect(bool isSelectVal)
		{
			if (this.isSelect == isSelectVal)
			{
				return;
			}
			this.DoSelect(isSelectVal);
		}

		private void DoSelect(bool isSelectVal)
		{
			this.isSelect = isSelectVal;
			if (isSelectVal)
			{
				this.OnSelect();
				return;
			}
			this.OnUnSelect();
		}

		private void OnSelect()
		{
			this.tabBgImage.sprite = this.tabBgSelectSprite;
			this.unSelectMask.SetActive(false);
			base.rectTransform.sizeDelta = this.selectSize;
		}

		private void OnUnSelect()
		{
			this.tabBgImage.sprite = this.tabBgUnSelectSprite;
			this.unSelectMask.SetActive(true);
			base.rectTransform.sizeDelta = this.unSelectSize;
		}

		[SerializeField]
		private CustomButton tabButton;

		[SerializeField]
		private CustomImage tabBgImage;

		[SerializeField]
		private Sprite tabBgSelectSprite;

		[SerializeField]
		private Sprite tabBgUnSelectSprite;

		[SerializeField]
		private GameObject unSelectMask;

		[SerializeField]
		private CustomText nameText;

		[SerializeField]
		private Vector2 selectSize;

		[SerializeField]
		private Vector2 unSelectSize;

		private Action<ShopType> onClick;

		private bool isSelect;
	}
}
