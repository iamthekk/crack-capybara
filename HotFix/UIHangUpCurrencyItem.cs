using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class UIHangUpCurrencyItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(CurrencyType currencyId, long currencyNum, int drop)
		{
			Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item((int)currencyId);
			if (item_Item == null)
			{
				HLog.LogError(string.Format("Table item_item not found id={0}", currencyId));
				return;
			}
			string atlasPath = GameApp.Table.GetAtlasPath(item_Item.atlasID);
			this.imageIcon.SetImage(atlasPath, item_Item.icon);
			this.textRewardNum.text = DxxTools.FormatNumber(currencyNum);
			if (currencyId == CurrencyType.Diamond)
			{
				this.textDropTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("uihangup_drop_hour", new object[] { DxxTools.FormatNumber((long)drop) });
				return;
			}
			this.textDropTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("uihangup_drop_min", new object[] { DxxTools.FormatNumber((long)drop) });
		}

		public CustomImage imageIcon;

		public CustomText textRewardNum;

		public CustomText textDropTime;
	}
}
