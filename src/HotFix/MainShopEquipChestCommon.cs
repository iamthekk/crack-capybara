using System;

namespace HotFix
{
	public class MainShopEquipChestCommon : MainShopEquipChestBase
	{
		public override void SetData()
		{
			base.SetData();
			if (this.chestType == eEquipChestType.SilverChest)
			{
				this.TriggerGuide();
			}
		}

		public void TriggerGuide()
		{
			GuideController.Instance.DelTarget("BtnBuyFree");
			GuideController.Instance.AddTarget("BtnBuyFree", this.btnFreeBuy.transform);
		}
	}
}
