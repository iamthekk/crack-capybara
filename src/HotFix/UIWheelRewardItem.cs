using System;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIWheelRewardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(TurntableSymbolData data)
		{
			if (data == null || data.cfg == null)
			{
				return;
			}
			this.mData = data;
			if (data.cfg.again > 0)
			{
				this.textInfo.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_wheel_again", new object[] { data.cfg.again });
				return;
			}
			if (string.IsNullOrEmpty(data.cfg.xNumtId))
			{
				this.textInfo.text = "";
				return;
			}
			this.textInfo.text = Singleton<LanguageManager>.Instance.GetInfoByID(data.cfg.xNumtId);
		}

		public void RefreshRate(int selectRate)
		{
			if (this.mData != null)
			{
				this.textNum.text = string.Format("<color={0}>{1}</color>", this.mData.cfg.textColor, this.mData.cfg.reward * selectRate);
			}
		}

		public CustomText textNum;

		public CustomText textInfo;

		private TurntableSymbolData mData;
	}
}
