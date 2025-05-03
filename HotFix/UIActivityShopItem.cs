using System;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIActivityShopItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.item.Init();
		}

		protected override void OnDeInit()
		{
			this.item.DeInit();
		}

		public UIItem item;

		public CustomText itemName;

		public CustomImage buyCostImage;

		public CustomText buyCostText;

		public CustomButton m_BuyButton;
	}
}
