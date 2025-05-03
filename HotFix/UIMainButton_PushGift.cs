using System;
using Framework;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMainButton_PushGift : BaseUIMainButton
	{
		protected override void OnInit()
		{
			this._pushGiftDataModule = GameApp.Data.GetDataModule(DataName.PushGiftDataModule);
			this._functionDataModule = GameApp.Data.GetDataModule(DataName.FunctionDataModule);
			this.button.onClick.AddListener(new UnityAction(this.OnClickButton));
		}

		private void OnClickButton()
		{
			GameApp.View.OpenView(ViewName.PushGiftBundleViewModule, PushGiftPosType.LeftOne, 0, null, null);
		}

		protected override void OnDeInit()
		{
			this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
		}

		public override bool IsShow()
		{
			return this._pushGiftDataModule != null && this._functionDataModule != null && this._pushGiftDataModule.HaveValidPushGiftData(PushGiftPosType.LeftOne) && this._functionDataModule.IsFunctionOpened(FunctionID.PushGift);
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 7;
			subPriority = 0;
		}

		public override void OnRefresh()
		{
		}

		public override void OnLanguageChange()
		{
		}

		public override void OnRefreshAnimation()
		{
		}

		protected override void OnUpdatePerSecond()
		{
			base.OnUpdatePerSecond();
			bool flag = this.IsShow();
			base.gameObject.SetActive(flag);
			if (flag)
			{
				long num = this._pushGiftDataModule.PushGiftDataDicByPosType[PushGiftPosType.LeftOne][0].EndTime - DxxTools.Time.ServerTimestamp;
				if (num < 0L)
				{
					GameApp.Event.DispatchNow(null, 248, null);
					this.timeText.text = DxxTools.FormatTime(0L);
					return;
				}
				this.timeText.text = DxxTools.FormatTime(num);
			}
		}

		[SerializeField]
		private CustomButton button;

		[SerializeField]
		private CustomText countDownTimeText;

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private CustomText nameText;

		[SerializeField]
		private CustomText timeText;

		private PushGiftDataModule _pushGiftDataModule;

		private FunctionDataModule _functionDataModule;
	}
}
