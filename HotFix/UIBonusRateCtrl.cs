using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIBonusRateCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.copyItem.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.bonusRateItems.Count; i++)
			{
				this.bonusRateItems[i].DeInit();
			}
			this.bonusRateItems.Clear();
		}

		public void SetData(List<Mining_showRate> rateList, int mode)
		{
			if (mode != 0)
			{
				if (mode == 1)
				{
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_get_title");
					string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_get_info");
					this.textTitle.text = infoByID;
					this.textInfo.text = infoByID2;
				}
			}
			else
			{
				string infoByID3 = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_show_title");
				string infoByID4 = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_show_info");
				this.textTitle.text = infoByID3;
				this.textInfo.text = infoByID4;
			}
			for (int i = 0; i < rateList.Count; i++)
			{
				UIBonusRateItem uibonusRateItem;
				if (i < this.bonusRateItems.Count)
				{
					uibonusRateItem = this.bonusRateItems[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
					gameObject.SetParentNormal(this.parent, false);
					uibonusRateItem = gameObject.GetComponent<UIBonusRateItem>();
					uibonusRateItem.Init();
					this.bonusRateItems.Add(uibonusRateItem);
				}
				uibonusRateItem.gameObject.SetActiveSafe(true);
				uibonusRateItem.SetData(rateList[i], mode);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.layoutRT);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.rootRT);
		}

		public RectTransform rootRT;

		public CustomText textTitle;

		public CustomText textInfo;

		public GameObject parent;

		public RectTransform layoutRT;

		public GameObject copyItem;

		private List<UIBonusRateItem> bonusRateItems = new List<UIBonusRateItem>();
	}
}
