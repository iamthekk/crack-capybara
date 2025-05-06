using System;
using Framework;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	[RequireComponent(typeof(CustomButton))]
	public class UISpeedButtonCtrl : MonoBehaviour
	{
		private void Awake()
		{
			if (this.speedButton)
			{
				this.speedButton.onClick.AddListener(new UnityAction(this.OnClick));
			}
			this.speedRate = 1;
		}

		private void OnDestroy()
		{
			if (this.speedButton)
			{
				this.speedButton.onClick.RemoveListener(new UnityAction(this.OnClick));
			}
		}

		public void SetData(UISpeedButtonCtrl.SpeedType type)
		{
			this.speedType = type;
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.isSpeedx2 = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.BattleSpeedx2, false);
			this.isSpeedx3 = dataModule.MonthCard.IsActivePrivilege(CardPrivilegeKind.BattleSpeedUp);
			if (this.isSpeedx3)
			{
				this.isSpeedx2 = true;
			}
			UISpeedButtonCtrl.SpeedType speedType = this.speedType;
			if (speedType != UISpeedButtonCtrl.SpeedType.PvE)
			{
				if (speedType != UISpeedButtonCtrl.SpeedType.PvP)
				{
					this.speedRate = Singleton<GameManager>.Instance.SpeedUpRate;
				}
				else
				{
					this.speedRate = Singleton<GameManager>.Instance.PvPSpeedUpRate;
				}
			}
			else
			{
				this.speedRate = Singleton<GameManager>.Instance.PvESpeedUpRate;
			}
			if (!this.isSpeedx3 && this.speedRate > 2)
			{
				this.speedRate = 1;
			}
			else if (!this.isSpeedx2 && this.speedRate > 1)
			{
				this.speedRate = 1;
			}
			this.Refresh();
		}

		private void Refresh()
		{
			this.imageBtn.sprite = ((this.speedRate > 1) ? this.spriteSpeedUp : this.spriteNormal);
			this.speedText.text = string.Format("x{0}", this.speedRate);
		}

		private void OnClick()
		{
			if (this.isClickDisabled)
			{
				return;
			}
			if (this.isSpeedx2 || this.isSpeedx3)
			{
				this.speedRate++;
			}
			if (this.isSpeedx3 && this.speedRate > 3)
			{
				this.speedRate = 1;
			}
			else if (this.isSpeedx2 && !this.isSpeedx3 && this.speedRate > 2)
			{
				this.speedRate = 1;
			}
			if (!this.isSpeedx2)
			{
				string lockTips = Singleton<GameFunctionController>.Instance.GetLockTips(100);
				GameApp.View.ShowStringTip(lockTips);
			}
			this.Refresh();
			UISpeedButtonCtrl.SpeedType speedType = this.speedType;
			if (speedType == UISpeedButtonCtrl.SpeedType.PvE)
			{
				Singleton<GameManager>.Instance.SetPvESpeedRate(this.speedRate);
				return;
			}
			if (speedType != UISpeedButtonCtrl.SpeedType.PvP)
			{
				Singleton<GameManager>.Instance.SetSpeedRate(this.speedRate);
				return;
			}
			Singleton<GameManager>.Instance.SetPvPSpeedRate(this.speedRate);
		}

		public void SetClickDisabled(bool isDisabled)
		{
			this.isClickDisabled = isDisabled;
		}

		[SerializeField]
		private CustomButton speedButton;

		[SerializeField]
		private CustomText speedText;

		[SerializeField]
		private Image imageBtn;

		[SerializeField]
		private Sprite spriteNormal;

		[SerializeField]
		private Sprite spriteSpeedUp;

		private UISpeedButtonCtrl.SpeedType speedType;

		private int speedRate = 1;

		private bool isClickDisabled;

		private bool isSpeedx2;

		private bool isSpeedx3;

		public enum SpeedType
		{
			Editor,
			Main,
			PvE,
			PvP
		}
	}
}
