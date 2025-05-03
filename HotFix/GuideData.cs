using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace HotFix
{
	public class GuideData
	{
		public int ID { get; private set; }

		public int Group { get; private set; }

		public List<GuideTriggerKindData> TriggerKind { get; private set; }

		public bool IfCanBreak { get; private set; }

		public bool IsMarkGroupOver { get; private set; }

		public string[] GuideAction { get; private set; }

		public int SortWeight { get; private set; }

		public bool GuideFullScreen { get; private set; }

		public bool AutoLockScroll { get; private set; }

		public string GuideTargetName { get; private set; }

		public int DelayMSec { get; private set; }

		public int NeedPVELevelID
		{
			get
			{
				if (this.ConditionList == null)
				{
					return 0;
				}
				int i = 0;
				while (i < this.ConditionList.Count)
				{
					if (this.ConditionList[i].Kind == GuideConditionKind.CompletePVELevel)
					{
						if (this.ConditionList[i].Agvs == null || this.ConditionList[i].Agvs.Length < 2)
						{
							HLog.LogError("[新手引导]配表错误，错误的条件：" + this.ID.ToString());
							return 0;
						}
						int num;
						if (int.TryParse(this.ConditionList[i].Agvs[1], out num))
						{
							return num;
						}
						HLog.LogError("[新手引导]配表错误，错误的条件：" + this.ID.ToString());
						return 0;
					}
					else
					{
						i++;
					}
				}
				return 0;
			}
		}

		private GuideData()
		{
		}

		public bool OnInit(Guide_guide tab)
		{
			this.ID = tab.id;
			this.Group = tab.guideGroup;
			this.GuideTargetName = tab.guideTarget;
			this.GuideFullScreen = tab.isLockScreen == 1;
			this.AutoLockScroll = tab.autoLockScroll == 1;
			this.GuideAction = tab.guideAction;
			this.IsMarkGroupOver = tab.groupOver == 1;
			this.IfCanBreak = tab.canBreak == 1;
			this.SortWeight = tab.weight;
			this.DelayMSec = tab.showDelay;
			this.SettedViewOrder = tab.additional;
			this.Styles = GuideStyleData.CreateStyles(tab.guideStyles);
			string[] guideTrigger = tab.guideTrigger;
			if (guideTrigger == null || guideTrigger.Length < 1)
			{
				return false;
			}
			this.TriggerKind = new List<GuideTriggerKindData>();
			for (int i = 0; i < guideTrigger.Length; i++)
			{
				if (!string.IsNullOrEmpty(guideTrigger[i]))
				{
					string[] array = guideTrigger[i].Split(':', StringSplitOptions.None);
					GuideTriggerKindData guideTriggerKindData = new GuideTriggerKindData();
					guideTriggerKindData.Kind = (GuideTriggerKind)int.Parse(array[0]);
					guideTriggerKindData.Args = ((array.Length > 1) ? array[1] : "");
					this.TriggerKind.Add(guideTriggerKindData);
				}
			}
			this.ConditionList.Clear();
			string[] guideCondition = tab.guideCondition;
			for (int j = 0; j < guideCondition.Length; j++)
			{
				GuideCondition guideCondition2 = GuideCondition.Create(guideCondition[j]);
				if (guideCondition2 != null)
				{
					this.ConditionList.Add(guideCondition2);
				}
			}
			this.CompleteList = new List<GuideCompleteData>();
			if (tab.guideComplete != null && tab.guideComplete.Length != 0)
			{
				for (int k = 0; k < tab.guideComplete.Length; k++)
				{
					string text = tab.guideComplete[k];
					if (!string.IsNullOrEmpty(text))
					{
						GuideCompleteData guideCompleteData = GuideCompleteData.Create(text);
						if (guideCompleteData != null)
						{
							this.CompleteList.Add(guideCompleteData);
						}
					}
				}
			}
			if (this.CompleteList.Count <= 0)
			{
				this.CompleteList.Add(new GuideCompleteData
				{
					GuideKind = GuideCompleteKind.ButtonClick,
					Argstr = ""
				});
			}
			return true;
		}

		public static GuideData Create(Guide_guide data)
		{
			GuideData guideData = new GuideData();
			if (guideData.OnInit(data))
			{
				return guideData;
			}
			return null;
		}

		internal static int GroupInSort(GuideData x, GuideData y)
		{
			return x.ID.CompareTo(y.ID);
		}

		public List<GuideStyleData> Styles;

		public List<GuideCondition> ConditionList = new List<GuideCondition>();

		public List<GuideCompleteData> CompleteList;

		public int SettedViewOrder = -1;
	}
}
