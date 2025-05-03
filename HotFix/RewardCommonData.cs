using System;
using System.Collections.Generic;

namespace HotFix
{
	public class RewardCommonData
	{
		public List<ItemData> GetCombineList()
		{
			return this.list.ToCombineList();
		}

		public string GetTitle()
		{
			if (string.IsNullOrEmpty(this.m_title))
			{
				return Singleton<LanguageManager>.Instance.GetInfoByID("1102");
			}
			return this.m_title;
		}

		public List<ItemData> list;

		public List<EquipData> epuipList;

		public string m_title;

		public bool m_hideTitle;

		public bool m_isFly = true;

		public bool m_isInSweep;

		public float m_autoCloseTime;

		public Action OnClose;
	}
}
