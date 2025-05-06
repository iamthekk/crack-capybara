using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIActivitySlotTrainProbabilityViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonClose.m_onClick = new Action(this.OnClickClose);
			this.buttonMask.m_onClick = new Action(this.OnClickClose);
			this.dataModule = GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule);
			this.normalItems = this.normalNodes.GetComponentsInChildren<UIItem>();
			foreach (UIActivitySlotTrainProbabilityViewModule.LimitItem limitItem in this.limitItems)
			{
				limitItem.uiItem.Init();
			}
			UIItem[] array = this.normalItems;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Init();
			}
		}

		public override void OnDelete()
		{
			this.buttonClose.m_onClick = null;
			this.buttonMask.m_onClick = null;
			foreach (UIActivitySlotTrainProbabilityViewModule.LimitItem limitItem in this.limitItems)
			{
				limitItem.uiItem.DeInit();
			}
			UIItem[] array = this.normalItems;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DeInit();
			}
			this.normalItems = null;
		}

		public override void OnOpen(object data)
		{
			this.posItemDic = (Dictionary<int, PropData>)data;
			this.mainCfg = GameApp.Table.GetManager().GetActivityTurntable_ActivityTurntableModelInstance().GetElementById(this.dataModule.TurntableId);
			this.FreshTexts();
			this.RefreshItems();
		}

		public override void OnClose()
		{
			this.posItemDic = null;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void FreshTexts()
		{
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_probability_title");
			this.textLimitProbabilityTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_probability_limit");
			this.textLimitProbability.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_probability_value", new object[] { DxxTools.GetPercentageString(int.Parse(this.mainCfg.showRate[1]), "0.00") });
			this.textNormalProbabilityTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_probability_normal");
			this.textNormalProbability.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_probability_value", new object[] { DxxTools.GetPercentageString(int.Parse(this.mainCfg.showRate[0]), "0.00") });
		}

		private void RefreshItems()
		{
			foreach (KeyValuePair<int, PropData> keyValuePair in this.posItemDic)
			{
				UIItem uiitem;
				if (keyValuePair.Key > 1 && keyValuePair.Key < 14)
				{
					uiitem = this.normalItems[keyValuePair.Key - 2];
				}
				else
				{
					CustomText customText;
					if (keyValuePair.Key == 14)
					{
						List<UIActivitySlotTrainProbabilityViewModule.LimitItem> list = this.limitItems;
						uiitem = list[list.Count - 1].uiItem;
						List<UIActivitySlotTrainProbabilityViewModule.LimitItem> list2 = this.limitItems;
						customText = list2[list2.Count - 1].textLeftCount;
					}
					else
					{
						uiitem = this.limitItems[keyValuePair.Key].uiItem;
						customText = this.limitItems[keyValuePair.Key].textLeftCount;
					}
					int num;
					int num2;
					if (keyValuePair.Key == 0)
					{
						num = 1;
						num2 = 1;
					}
					else if (keyValuePair.Key == 1)
					{
						num2 = int.Parse(this.mainCfg.limitItems[0].Split(',', StringSplitOptions.None)[1]);
						if (this.dataModule.SmallGuaranteeCount != null && this.dataModule.SmallGuaranteeCount.ContainsKey(keyValuePair.Key))
						{
							num = this.dataModule.SmallGuaranteeCount[keyValuePair.Key];
						}
						else
						{
							num = num2;
						}
					}
					else
					{
						num2 = int.Parse(this.mainCfg.limitItems[1].Split(',', StringSplitOptions.None)[1]);
						if (this.dataModule.SmallGuaranteeCount != null && this.dataModule.SmallGuaranteeCount.ContainsKey(keyValuePair.Key))
						{
							num = this.dataModule.SmallGuaranteeCount[keyValuePair.Key];
						}
						else
						{
							num = num2;
						}
					}
					customText.text = Singleton<LanguageManager>.Instance.GetInfoByID("activity_turntable_probability_limitcount", new object[] { num, num2 });
				}
				uiitem.SetData(keyValuePair.Value);
				uiitem.OnRefresh();
			}
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.UIActivitySlotTrainProbabilityViewModule, null);
		}

		public CustomButton buttonMask;

		public CustomButton buttonClose;

		public CustomText textTitle;

		public CustomText textLimitProbabilityTitle;

		public CustomText textLimitProbability;

		public CustomText textNormalProbabilityTitle;

		public CustomText textNormalProbability;

		public List<UIActivitySlotTrainProbabilityViewModule.LimitItem> limitItems;

		public Transform normalNodes;

		private UIItem[] normalItems;

		private ActivitySlotTrainDataModule dataModule;

		private ActivityTurntable_ActivityTurntable mainCfg;

		private Dictionary<int, PropData> posItemDic = new Dictionary<int, PropData>();

		[Serializable]
		public class LimitItem
		{
			public UIItem uiItem;

			public CustomText textLeftCount;
		}
	}
}
