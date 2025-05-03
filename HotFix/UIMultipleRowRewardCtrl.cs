using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIMultipleRowRewardCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.copyItem.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.rewardItems.Count; i++)
			{
				this.rewardItems[i].DeInit();
			}
			this.rewardItems.Clear();
		}

		public void SetData(string[] showRate, int dropTimes)
		{
			if (showRate == null)
			{
				return;
			}
			this.textDropTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("uidungeon_drop_time", new object[] { dropTimes });
			for (int i = 0; i < this.rewardItems.Count; i++)
			{
				this.rewardItems[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < showRate.Length; j++)
			{
				List<int> listInt = showRate[j].GetListInt(',');
				if (listInt.Count >= 2)
				{
					int num = listInt[0];
					int num2 = listInt[1];
					UIMultipleRowRewardItem uimultipleRowRewardItem;
					if (j < this.rewardItems.Count)
					{
						uimultipleRowRewardItem = this.rewardItems[j];
					}
					else
					{
						GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
						gameObject.SetParentNormal(this.rewardLayout, false);
						uimultipleRowRewardItem = gameObject.GetComponent<UIMultipleRowRewardItem>();
						uimultipleRowRewardItem.Init();
						this.rewardItems.Add(uimultipleRowRewardItem);
					}
					if (uimultipleRowRewardItem != null)
					{
						uimultipleRowRewardItem.SetData(num, num2);
						uimultipleRowRewardItem.gameObject.SetActiveSafe(true);
					}
				}
			}
		}

		public CustomText textDropTime;

		public RectTransform rewardLayout;

		public GameObject copyItem;

		private List<UIMultipleRowRewardItem> rewardItems = new List<UIMultipleRowRewardItem>();
	}
}
