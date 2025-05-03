using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class EquipShopProbabilityRewardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Ctrl_Item.Init();
		}

		protected override void OnDeInit()
		{
			this.Ctrl_Item.DeInit();
		}

		public void SetData(Shop_SummonPool summonPoolCfg, int poolRate, int totalItemRate)
		{
			if (summonPoolCfg == null)
			{
				return;
			}
			ItemData itemData = new ItemData(summonPoolCfg.itemID, (long)summonPoolCfg.num);
			this.Ctrl_Item.SetData(itemData.ToPropData());
			this.Ctrl_Item.OnRefresh();
			float num = (float)poolRate / 100f;
			float num2 = (float)summonPoolCfg.weight / (float)totalItemRate;
			this.Text_Probability.text = HLog.StringBuilder((num * num2).ToString("F2"), "%");
		}

		public CustomText Text_Probability;

		public UIItem Ctrl_Item;
	}
}
