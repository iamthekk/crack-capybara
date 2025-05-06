using System;
using DG.Tweening;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine.UI;

namespace HotFix
{
	public class UIFundBattlePassNodeCtrl : UIRechargeGiftFundNodeCtrl
	{
		private IAPDataModule iapDataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.IAPDataModule);
			}
		}

		protected override void OnNodeInit()
		{
			RedPointController.Instance.RegRecordChange("IAPRechargeGift.Fund.BattlePass", new Action<RedNodeListenData>(base.OnRedPoint));
			this.timeCtrl.OnRefreshText += this.OnRefreshNextTimeText;
			this.timeCtrl.OnChangeState += this.OnChangeStateNextTime;
			this.timeCtrl.Init();
		}

		protected override void OnNodeDeInit()
		{
			RedPointController.Instance.UnRegRecordChange("IAPRechargeGift.Fund.BattlePass", new Action<RedNodeListenData>(base.OnRedPoint));
			this.timeCtrl.DeInit();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.timeCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnRefresh()
		{
			IAPBattlePass battlePass = this.iapDataModule.BattlePass;
			if (battlePass == null)
			{
				this.textTips.text = "";
				this.activeObj.SetActiveSafe(false);
				this.inActiveObj.SetActiveSafe(true);
				base.gameObject.SetActiveSafe(false);
				return;
			}
			if (this.iapDataModule.BattlePass.IsAllEnd())
			{
				base.gameObject.SetActiveSafe(false);
				return;
			}
			base.gameObject.SetActiveSafe(true);
			this.activeObj.SetActiveSafe(battlePass.HasBuy);
			this.inActiveObj.SetActiveSafe(!battlePass.HasBuy);
			IAP_BattlePass elementById = GameApp.Table.GetManager().GetIAP_BattlePassModelInstance().GetElementById(battlePass.BattlePassID);
			if (elementById != null)
			{
				this.textTips.text = Singleton<LanguageManager>.Instance.GetInfoByID("uibattlepass_season", new object[] { elementById.seasonID });
			}
			else
			{
				this.textTips.text = "";
			}
			this.textInfo.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiprivilegefund_progress");
			IAPBattlePassData currentData = battlePass.GetCurrentData();
			ValueTuple<int, int, int> showProgress = battlePass.GetShowProgress();
			if (currentData.IsFinal)
			{
				this.textContent.text = string.Format("{0}/{1}", showProgress.Item3 - showProgress.Item1, showProgress.Item2 - showProgress.Item1);
			}
			else
			{
				this.textContent.text = string.Format("{0}/{1}", battlePass.CurrentScore - showProgress.Item1, currentData.Score - showProgress.Item1);
			}
			this.timeCtrl.Play();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.infoRectTrans);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.timeRectTrans);
		}

		protected override void OnClickSelf()
		{
			GameApp.View.OpenView(ViewName.IAPFundViewModule, IAPFundViewModule.FundType.BattlePass, 1, null, null);
		}

		private IAPShopTimeCtrl.State OnChangeStateNextTime(IAPShopTimeCtrl arg)
		{
			if (this.iapDataModule.BattlePass.IsAllEnd())
			{
				return IAPShopTimeCtrl.State.Load;
			}
			if (this.iapDataModule.BattlePass.GetNextTime() <= 0L)
			{
				Sequence sequence = this._seqPool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, 2f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					NetworkUtils.PlayerData.SendUserGetIapInfoRequest(delegate
					{
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIRechargeGift_Refresh, null);
						base.Refresh();
					});
				});
				return IAPShopTimeCtrl.State.Load;
			}
			return IAPShopTimeCtrl.State.Show;
		}

		private string OnRefreshNextTimeText(IAPShopTimeCtrl arg)
		{
			string time = Singleton<LanguageManager>.Instance.GetTime(this.iapDataModule.BattlePass.GetNextTime());
			return Singleton<LanguageManager>.Instance.GetInfoByID("uiprivilegefund_time", new object[] { time });
		}

		public IAPShopTimeCtrl timeCtrl;
	}
}
