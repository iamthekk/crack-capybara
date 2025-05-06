using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Pay;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class IAPBattlePassCellFinal : IAPBattlePassBaseCell
	{
		protected override void OnInit()
		{
			this.mDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.PrefabItem.SetActive(false);
			this.buttonBox.onClick.AddListener(new UnityAction(this.OnGetFinalReward));
		}

		protected override void OnDeInit()
		{
			this.buttonBox.onClick.RemoveListener(new UnityAction(this.OnGetFinalReward));
			this.DelInitItems();
		}

		private void DelInitItems()
		{
		}

		public override void SetData(IAPBattlePassData data, int index, Action onClickReward)
		{
			this.mData = data;
			this.Index = index;
		}

		public override void RefreshUI(SequencePool m_seqPool)
		{
			IAPBattlePass battlePass = this.mDataModule.BattlePass;
			if (battlePass == null || this.mData == null)
			{
				this.TextTips.text = "";
				this.TextCount.text = "";
				this.DelInitItems();
				return;
			}
			int num = this.mData.Score;
			int num2 = this.mData.LastScore + this.mData.Score;
			int currentScore = battlePass.CurrentScore;
			int num3 = battlePass.FinalRewardMaxCount;
			num3--;
			while (currentScore >= num2 && num3 > 0)
			{
				num3--;
				num2 += this.mData.Score;
			}
			num = num2;
			int num4 = num - this.mData.Score;
			this.sliderProgress.minValue = (float)num4;
			this.sliderProgress.maxValue = (float)num;
			this.sliderProgress.value = (float)currentScore;
			this.textProgress.text = string.Format("{0}/{1}", this.sliderProgress.value - this.sliderProgress.minValue, this.sliderProgress.maxValue - this.sliderProgress.minValue);
			IAP_BattlePass elementById = GameApp.Table.GetManager().GetIAP_BattlePassModelInstance().GetElementById(battlePass.BattlePassID);
			if (elementById == null)
			{
				HLog.LogError(string.Format("[IAP]BattlePass table not find : {0}", battlePass.BattlePassID));
				return;
			}
			bool flag = !battlePass.HasBuy || battlePass.FinalRewardCanGetCount <= 0;
			int finalRewardGetCount = battlePass.FinalRewardGetCount;
			int finalRewardMaxCount = battlePass.FinalRewardMaxCount;
			if (battlePass.HasBuy)
			{
				this.redNode.SetActive(!flag);
				if (battlePass.FinalRewardCanGetCount <= 0)
				{
					this.btnAni.Play("Idle");
				}
				else
				{
					this.btnAni.Play("Shake");
				}
			}
			else
			{
				this.redNode.SetActive(false);
				this.btnAni.Play("Shake");
			}
			this.TextTips.text = Singleton<LanguageManager>.Instance.GetInfoByID("uibattlepass_final_reward", new object[]
			{
				this.mData.Score,
				elementById.finalRewardTimes
			});
			this.TextCount.text = Singleton<LanguageManager>.Instance.GetInfoByID("uibattlepass_final_reward_collect", new object[] { battlePass.FinalRewardGetCount, battlePass.FinalRewardMaxCount });
		}

		private void OnClickBuyBattlePass()
		{
			Action onClickGotoBuy = this.OnClickGotoBuy;
			if (onClickGotoBuy == null)
			{
				return;
			}
			onClickGotoBuy();
		}

		private void OnGetFinalReward()
		{
			if (this.mDataModule.BattlePass == null)
			{
				return;
			}
			if (!this.mDataModule.BattlePass.HasBuy)
			{
				this.OnClickBuyBattlePass();
				return;
			}
			if (this.mDataModule.BattlePass.FinalRewardGetCount == this.mDataModule.BattlePass.FinalRewardMaxCount)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("uibattlepass_noreward"));
				return;
			}
			if (this.mDataModule.BattlePass.FinalRewardCanGetCount <= 0)
			{
				UIBoxInfoViewModule.Transfer transfer = new UIBoxInfoViewModule.Transfer
				{
					nodeType = UIBoxInfoViewModule.UIBoxInfoNodeType.Up,
					rewards = this.mData.PayRewards.ToItemDatas(),
					position = this.buttonBox.transform.position,
					anchoredPositionOffset = new Vector3(0f, 50f, 0f),
					secondLayer = true
				};
				GameApp.View.OpenView(ViewName.RewardBoxInfoViewModule, transfer, 1, null, null);
				return;
			}
			int preFinalRewardCount = this.mDataModule.BattlePass.FinalRewardGetCount;
			NetworkUtils.Purchase.BattlePassFinalRewardRequest(delegate(bool result, BattlePassFinalRewardResponse resp)
			{
				if (result && resp.CommonData.Reward != null)
				{
					DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
					int level = this.mDataModule.BattlePass.Level;
					int num = this.mDataModule.BattlePass.FinalRewardGetCount;
					num -= preFinalRewardCount;
					int id = this.mDataModule.BattlePass.DataList.Find((IAPBattlePassData a) => a.Type == BattlePassType.FinalLoop).ID;
					List<int> list = new List<int>();
					for (int i = 0; i < num; i++)
					{
						list.Add(id);
					}
					GameApp.SDK.Analyze.Track_BattlePassGet_BattlePass(level, resp.CommonData.Reward, this.mDataModule.BattlePass.BattlePassPurchaseID, null, null, list);
				}
			});
		}

		private void OnClickItem(UIItem item, PropData data, object arg3)
		{
			DxxTools.UI.OnItemClick(item, data, arg3);
		}

		public CustomText TextTips;

		public CustomText TextCount;

		public GameObject PrefabItem;

		public Transform TFItems;

		public CustomButton buttonBox;

		public Slider sliderProgress;

		public CustomText textProgress;

		public GameObject redNode;

		public Animator btnAni;

		private IAPDataModule mDataModule;

		private IAPBattlePassData mData;
	}
}
