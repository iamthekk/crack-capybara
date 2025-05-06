using System;
using Framework;

namespace HotFix
{
	public class UIMainCity_ShopNode : UIBaseMainCityNode
	{
		public override int FunctionOpenID
		{
			get
			{
				return 1053;
			}
		}

		public override MainCityName Name
		{
			get
			{
				return MainCityName.Shop;
			}
		}

		public override string RedName
		{
			get
			{
				return "Main.Shop";
			}
		}

		public override int NameLanguageID
		{
			get
			{
				return 400268;
			}
		}

		public override string NameLanguageIDStr
		{
			get
			{
				return "400268";
			}
		}

		protected override void OnClickUnlockBt()
		{
			base.OnClickUnlockBt();
			GameApp.View.OpenView(ViewName.MainShopViewModule, new MainShopViewModuleData(ShopType.BlackMarket), 1, null, null);
		}
	}
}
