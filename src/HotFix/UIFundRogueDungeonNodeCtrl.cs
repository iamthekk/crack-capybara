using System;
using Framework;
using Framework.Logic.UI;
using UnityEngine.UI;

namespace HotFix
{
	public class UIFundRogueDungeonNodeCtrl : UIRechargeGiftFundNodeCtrl
	{
		protected override void OnNodeInit()
		{
			RedPointController.Instance.RegRecordChange("IAPRechargeGift.Fund.RogueDungeonFund", new Action<RedNodeListenData>(base.OnRedPoint));
		}

		protected override void OnNodeDeInit()
		{
			RedPointController.Instance.UnRegRecordChange("IAPRechargeGift.Fund.RogueDungeonFund", new Action<RedNodeListenData>(base.OnRedPoint));
		}

		protected override void OnRefresh()
		{
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			RogueDungeonDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule);
			IAPLevelFundGroup currentFundGroup = dataModule.LevelFund.GetCurrentFundGroup(IAPLevelFundGroupKind.RogueDungeonFloor);
			if (currentFundGroup == null)
			{
				base.gameObject.SetActiveSafe(false);
				return;
			}
			this.activeObj.SetActiveSafe(currentFundGroup.HasBuy);
			this.inActiveObj.SetActiveSafe(!currentFundGroup.HasBuy);
			this.timeRectTrans.gameObject.SetActiveSafe(false);
			this.textTips.text = Singleton<LanguageManager>.Instance.GetInfoByID("uifundroguedungeon_notes");
			this.textInfo.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiprivilegefund_progress");
			int num = (int)(dataModule2.CurrentFloorID - 1U);
			this.textContent.text = string.Format("{0}/{1}", num, currentFundGroup.GetNextShowProgress());
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.infoRectTrans);
		}

		protected override void OnClickSelf()
		{
			GameApp.View.OpenView(ViewName.IAPFundViewModule, IAPFundViewModule.FundType.RogueDungeon, 1, null, null);
		}
	}
}
