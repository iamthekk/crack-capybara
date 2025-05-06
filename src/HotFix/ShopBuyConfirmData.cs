using System;
using System.Collections.Generic;

namespace HotFix
{
	public class ShopBuyConfirmData
	{
		public bool IsFree
		{
			get
			{
				return this.m_costItem == null || this.m_costItem.TotalCount == 0L;
			}
		}

		public void CheckDefault()
		{
			if (string.IsNullOrEmpty(this.m_title))
			{
				this.m_title = Singleton<LanguageManager>.Instance.GetInfoByID("43");
			}
			if (this.m_rewards == null)
			{
				this.m_rewards = new List<ItemData>();
			}
		}

		public void SetCost(int costItemId, int costCount)
		{
			this.m_costItem = new ItemData(costItemId, (long)costCount);
		}

		public void SetCost(ItemData cost)
		{
			if (cost == null)
			{
				return;
			}
			this.m_costItem = cost.Copy();
		}

		public void SetRewards(List<ItemData> rewards)
		{
			this.m_rewards.AddRange(rewards);
		}

		public string m_title = "";

		public List<ItemData> m_rewards = new List<ItemData>();

		public ItemData m_costItem;

		public Action m_onSure;

		public Action m_onCancel;
	}
}
