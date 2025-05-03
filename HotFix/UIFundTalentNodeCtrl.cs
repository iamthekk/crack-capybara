using System;
using Framework;
using Framework.Logic.UI;
using UnityEngine.UI;

namespace HotFix
{
	public class UIFundTalentNodeCtrl : UIRechargeGiftFundNodeCtrl
	{
		protected override void OnNodeInit()
		{
			RedPointController.Instance.RegRecordChange("IAPRechargeGift.Fund.TalentFund", new Action<RedNodeListenData>(base.OnRedPoint));
		}

		protected override void OnNodeDeInit()
		{
			RedPointController.Instance.UnRegRecordChange("IAPRechargeGift.Fund.TalentFund", new Action<RedNodeListenData>(base.OnRedPoint));
		}

		protected override void OnRefresh()
		{
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			TalentDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.TalentDataModule);
			IAPLevelFundGroup currentFundGroup = dataModule.LevelFund.GetCurrentFundGroup(IAPLevelFundGroupKind.TalentLevel);
			if (currentFundGroup == null)
			{
				base.gameObject.SetActiveSafe(false);
				return;
			}
			this.activeObj.SetActiveSafe(currentFundGroup.HasBuy);
			this.inActiveObj.SetActiveSafe(!currentFundGroup.HasBuy);
			this.timeRectTrans.gameObject.SetActiveSafe(false);
			this.textTips.text = Singleton<LanguageManager>.Instance.GetInfoByID("uifundtalent_notes");
			this.textInfo.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiprivilegefund_progress");
			this.textContent.text = string.Format("{0}/{1}", dataModule2.TalentExp, currentFundGroup.GetNextShowProgress());
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.infoRectTrans);
		}

		protected override void OnClickSelf()
		{
			GameApp.View.OpenView(ViewName.IAPFundViewModule, IAPFundViewModule.FundType.Talent, 1, null, null);
		}
	}
}
