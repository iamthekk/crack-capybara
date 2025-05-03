using System;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class IAPBattlePassCellBuyScoreUI : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.ButtonBuy.m_onClick = new Action(this.OnClickBuyScore);
		}

		protected override void OnDeInit()
		{
		}

		internal void SetData(IAPBattlePassData data, int score, int cost)
		{
			this.mData = data;
			this.mScore = score;
			this.mCost = cost;
			if (this.mCost <= 0)
			{
				base.SetActive(false);
				return;
			}
			this.TextCost.text = cost.ToString();
			base.SetActive(true);
		}

		private void OnClickBuyScore()
		{
			Action onBuyScore = this.OnBuyScore;
			if (onBuyScore == null)
			{
				return;
			}
			onBuyScore();
		}

		public CustomButton ButtonBuy;

		public CustomText TextCost;

		private int mScore;

		private int mCost;

		private IAPBattlePassData mData;

		public Action OnBuyScore;
	}
}
