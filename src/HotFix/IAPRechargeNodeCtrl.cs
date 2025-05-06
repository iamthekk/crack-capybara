using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class IAPRechargeNodeCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
		}

		protected override void OnDeInit()
		{
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.passTime <= 0L)
			{
				return;
			}
			long num = this.passTime - DxxTools.Time.ServerTimestamp;
			string time = Singleton<LanguageManager>.Instance.GetTime((num < 0L) ? 0L : num);
			this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("uigiftrecharge_time", new object[] { time });
			if (num <= 0L)
			{
				this.passTime = 0L;
				this.timeObj.SetActiveSafe(false);
				this.OnClickClose();
			}
		}

		public void SetData(IAP_PushPacks table, ulong time, Action onClose)
		{
			this.passTime = (long)time;
			this.OnClose = onClose;
			this.timeObj.SetActiveSafe(this.passTime > 0L);
			if (!string.IsNullOrEmpty(table.nameID))
			{
				this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(table.nameID);
			}
			else
			{
				this.textTitle.text = "";
			}
			if (!string.IsNullOrEmpty(table.descID))
			{
				this.textDes.text = Singleton<LanguageManager>.Instance.GetInfoByID(table.descID);
			}
			else
			{
				this.textDes.text = "";
			}
			if (table.valueDescID != null && table.valueDescID.Length >= 1)
			{
				this.textDiscount1.text = Singleton<LanguageManager>.Instance.GetInfoByID(table.valueDescID[0]);
			}
			else
			{
				this.textDiscount1.text = "";
			}
			List<ItemData> rewardItemData = table.GetRewardItemData();
			this.rewardCtrl.SetData(rewardItemData, true);
			this.rewardCtrl.Init();
			this.rewardCtrl.SetActiveForReceive(false);
			this.payButtonCtrl.SetData(table.id, "", new Action<bool>(this.OnPaySuccess), new Action(this.OnCloseReward));
		}

		private void OnClickClose()
		{
			Action onClose = this.OnClose;
			if (onClose == null)
			{
				return;
			}
			onClose();
		}

		private void OnPaySuccess(bool isOk)
		{
			if (isOk)
			{
				this.payButtonCtrl.gameObject.SetActiveSafe(false);
				this.OnClickClose();
			}
		}

		private void OnCloseReward()
		{
			if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Equip, false) && Singleton<GameFunctionController>.Instance.CheckNewFunctionOpen() && !GameApp.View.IsOpened(ViewName.FunctionOpenViewModule))
			{
				GameApp.View.OpenView(ViewName.FunctionOpenViewModule, null, 2, null, null);
			}
		}

		[SerializeField]
		private CustomText textTitle;

		[SerializeField]
		private CustomText textDes;

		[SerializeField]
		private CustomText textDiscount1;

		[SerializeField]
		private IAPShopRewardCtrl rewardCtrl;

		[SerializeField]
		private PurchaseButtonCtrl payButtonCtrl;

		[SerializeField]
		private CustomButton buttonClose;

		[SerializeField]
		private GameObject timeObj;

		[SerializeField]
		private CustomText textTime;

		private long passTime;

		private Action OnClose;
	}
}
