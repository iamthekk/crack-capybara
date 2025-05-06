using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIPaySlotItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.buttonPay.Init();
			this.slotRollCtrl.Init();
		}

		protected override void OnDeInit()
		{
			this.buttonPay.DeInit();
			this.slotRollCtrl.DeInit();
			this.sequencePool.Clear(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.slotRollCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public void SetData(int seed, int showWeight, int price, int rewardGroup, int index, Action<bool> showMask)
		{
			this.mShowWeight = showWeight;
			this.mPrice = price;
			this.mRewardGroup = rewardGroup;
			this.onShowMask = showMask;
			BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
			int num = ((playerData != null) ? playerData.Chips.mVariable : 0);
			this.buttonPay.SetData(101, "icon_chips", price, num, new Action(this.OnClickPay), true);
			this.slotRollCtrl.SetData(seed, this.mRewardGroup, index, new Action(this.OnEndRoll));
			this.Refresh();
		}

		private void Refresh()
		{
			this.textWeight.text = string.Format("{0}%", this.mShowWeight);
		}

		private void ShowWeight()
		{
			Sequence sequence = this.sequencePool.Get();
			this.weightObj.transform.localScale = Vector3.one * 0.5f;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.weightObj.transform, 1.2f, 0.15f));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.weightObj.transform, 1f, 0.05f));
		}

		private void HideWeight()
		{
			Sequence sequence = this.sequencePool.Get();
			this.weightObj.transform.localScale = Vector3.one;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.weightObj.transform, 1.2f, 0.05f));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.weightObj.transform, 0f, 0.15f));
		}

		private void OnClickPay()
		{
			if (this.isRoll)
			{
				return;
			}
			BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
			if (playerData != null)
			{
				if (playerData.Chips.IsDataValid() && playerData.Chips.mVariable >= this.mPrice)
				{
					this.isRoll = true;
					Action<bool> action = this.onShowMask;
					if (action != null)
					{
						action(this.isRoll);
					}
					this.HideWeight();
					playerData.UpdateAttribute(GameEventAttType.Chips, (double)(-(double)this.mPrice));
					this.slotRollCtrl.Roll();
					return;
				}
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uipayslot_no_chips");
				GameApp.View.ShowStringTip(infoByID);
			}
		}

		private void OnEndRoll()
		{
			this.ShowWeight();
			this.isRoll = false;
			Action<bool> action = this.onShowMask;
			if (action == null)
			{
				return;
			}
			action(this.isRoll);
		}

		public void OnHide()
		{
			this.slotRollCtrl.OnHide();
		}

		public GameObject weightObj;

		public CustomText textWeight;

		public UICurrencyButton buttonPay;

		public UISlotRollCtrl slotRollCtrl;

		private int mShowWeight;

		private int mPrice;

		private int mRewardGroup;

		private bool isRoll;

		private SequencePool sequencePool = new SequencePool();

		private Action<bool> onShowMask;
	}
}
