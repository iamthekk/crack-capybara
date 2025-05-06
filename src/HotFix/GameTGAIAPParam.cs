using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class GameTGAIAPParam
	{
		public string Source { get; private set; }

		public int ProductIDInt { get; private set; }

		public string ProductID { get; private set; }

		public decimal ProductPrice { get; private set; }

		public decimal LocalPrice { get; private set; }

		public string Currency { get; private set; }

		private GameTGAIAPParam()
		{
		}

		public static GameTGAIAPParam Create(int configID)
		{
			IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(configID);
			if (elementById == null)
			{
				return null;
			}
			string isoCodeForPlatformID = GameApp.Purchase.Manager.GetIsoCodeForPlatformID(elementById.platformID);
			if (string.IsNullOrEmpty(isoCodeForPlatformID))
			{
				return null;
			}
			float localizedPriceForPlatformID = GameApp.Purchase.Manager.GetLocalizedPriceForPlatformID(elementById.platformID);
			if (localizedPriceForPlatformID < -1f)
			{
				return null;
			}
			IAP_platformID elementById2 = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(elementById.platformID);
			if (elementById2 == null)
			{
				return null;
			}
			GameTGAIAPParam gameTGAIAPParam = new GameTGAIAPParam();
			gameTGAIAPParam.ProductIDInt = configID;
			gameTGAIAPParam.ProductID = elementById.nameID;
			gameTGAIAPParam.ProductPrice = (decimal)elementById2.price;
			gameTGAIAPParam.LocalPrice = (decimal)localizedPriceForPlatformID;
			gameTGAIAPParam.Currency = isoCodeForPlatformID;
			int source = elementById.source;
			string text = "";
			TGASource_IAP elementById3 = GameApp.Table.GetManager().GetTGASource_IAPModelInstance().GetElementById(source);
			if (elementById3 != null)
			{
				text = elementById3.source;
			}
			else
			{
				GameTGATools.TGADebugLogError(string.Format("[Track_Get_IAP]需要在TGASource_IAP表中配置id为{0}的数据", source));
			}
			gameTGAIAPParam.Source = text;
			return gameTGAIAPParam;
		}
	}
}
