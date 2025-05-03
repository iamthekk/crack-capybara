using System;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine.UI;

namespace HotFix
{
	public class UIFundTowerNodeCtrl : UIRechargeGiftFundNodeCtrl
	{
		protected override void OnNodeInit()
		{
			RedPointController.Instance.RegRecordChange("IAPRechargeGift.Fund.TowerFund", new Action<RedNodeListenData>(base.OnRedPoint));
		}

		protected override void OnNodeDeInit()
		{
			RedPointController.Instance.UnRegRecordChange("IAPRechargeGift.Fund.TowerFund", new Action<RedNodeListenData>(base.OnRedPoint));
		}

		protected override void OnRefresh()
		{
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			TowerDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.TowerDataModule);
			IAPLevelFundGroup currentFundGroup = dataModule.LevelFund.GetCurrentFundGroup(IAPLevelFundGroupKind.TowerLevel);
			if (currentFundGroup == null)
			{
				base.gameObject.SetActiveSafe(false);
				return;
			}
			this.activeObj.SetActiveSafe(currentFundGroup.HasBuy);
			this.inActiveObj.SetActiveSafe(!currentFundGroup.HasBuy);
			this.timeRectTrans.gameObject.SetActiveSafe(false);
			this.textTips.text = Singleton<LanguageManager>.Instance.GetInfoByID("uifundtower_notes");
			this.textInfo.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiprivilegefund_progress");
			int num = dataModule2.CalculateShouldChallengeLevelID(dataModule2.CompleteTowerLevelId);
			TowerChallenge_Tower towerConfigByLevelId = dataModule2.GetTowerConfigByLevelId(num);
			int towerConfigNum = dataModule2.GetTowerConfigNum(towerConfigByLevelId);
			int levelNumByLevelId = dataModule2.GetLevelNumByLevelId(num);
			this.textContent.text = string.Format("{0}-{1}/{2}", towerConfigNum, levelNumByLevelId, currentFundGroup.GetNextShowProgress());
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.infoRectTrans);
		}

		protected override void OnClickSelf()
		{
			GameApp.View.OpenView(ViewName.IAPFundViewModule, IAPFundViewModule.FundType.Tower, 1, null, null);
		}
	}
}
