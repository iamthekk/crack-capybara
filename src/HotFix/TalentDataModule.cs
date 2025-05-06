using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Proto.Common;
using Server;

namespace HotFix
{
	public class TalentDataModule : IDataModule
	{
		public int TalentStage
		{
			get
			{
				return this.talentData.TalentStage;
			}
		}

		public int TalentExp
		{
			get
			{
				return this.talentData.TalentExp;
			}
		}

		public int GetName()
		{
			return 148;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UITalent_RefreshData, new HandlerEvent(this.OnEventRefreshData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UITalent_RefreshData, new HandlerEvent(this.OnEventRefreshData));
		}

		public void Reset()
		{
			this.talentData = null;
			this.talentProgressData = null;
			this.m_addAttributeData.Clear();
		}

		public void SetLoginTalentData(TalentsInfo talentsInfo)
		{
			this.RefreshTalentData(talentsInfo);
		}

		public int GetProgressId(int totalExp)
		{
			IList<TalentNew_talent> allElements = GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetAllElements();
			for (int i = allElements.Count - 1; i >= 0; i--)
			{
				if (totalExp >= allElements[i].talentLevel)
				{
					return allElements[i].id;
				}
			}
			return 0;
		}

		public void RefreshTalentData(TalentsInfo talentsInfo)
		{
			if (talentsInfo == null)
			{
				HLog.LogError("TalentsInfo is null");
				return;
			}
			if (talentsInfo.Step == 0U)
			{
				talentsInfo.Step = 1U;
			}
			if (this.talentData == null)
			{
				this.talentData = new TalentData();
				this.talentData.Init((int)talentsInfo.Step, (int)talentsInfo.ExpProcess);
				this.talentData.UpdateTalentAttributeLevel(talentsInfo.AttributesMap);
			}
			else
			{
				if ((ulong)talentsInfo.Step > (ulong)((long)this.talentData.TalentStage))
				{
					this.talentData.Fix2FullLevel((int)talentsInfo.ExpProcess);
					this.talentData = new TalentData();
					this.talentData.Init((int)talentsInfo.Step, (int)talentsInfo.ExpProcess);
					try
					{
						RedPointController.Instance.ReCalcAsync("Equip");
						goto IL_00DF;
					}
					catch (Exception ex)
					{
						HLog.LogException(ex);
						goto IL_00DF;
					}
				}
				this.talentData.UpdateTalentAttributeLevel(talentsInfo.AttributesMap);
				this.talentData.UpdateExp((int)talentsInfo.ExpProcess);
			}
			IL_00DF:
			this.UpdateProgressData();
			this.MathAddAttributeData();
		}

		public void UpdateProgressData()
		{
			List<int> list = new List<int>();
			int num = 1;
			IList<TalentNew_talent> allElements = GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				if (allElements[i].talentLevel > this.talentData.TalentExp)
				{
					num = allElements[i].id;
					break;
				}
				if (i + 1 == allElements.Count)
				{
					num = allElements[i].id;
				}
			}
			if (num % 2 == 1)
			{
				list.Add(num - 1);
				list.Add(num);
				list.Add(num + 1);
			}
			else
			{
				list.Add(num - 2);
				list.Add(num - 1);
				list.Add(num);
			}
			if (this.talentProgressData == null || this.talentProgressData.cacheIds[0] != list[0])
			{
				if (this.talentProgressData != null)
				{
					this.talentProgressData.curLevel = this.talentData.TalentExp;
				}
				this.talentProgressData = new TalentProgressData();
				this.talentProgressData.cacheIds = list;
				int num2 = 0;
				TalentNew_talent elementById = GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetElementById(list[0]);
				if (elementById != null)
				{
					num2 = elementById.talentLevel;
				}
				TalentNew_talent elementById2 = GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetElementById(list[1]);
				if (elementById2 != null)
				{
					TalentProgressRewardData talentProgressRewardData = new TalentProgressRewardData(elementById2);
					this.talentProgressData.rewardList.Add(talentProgressRewardData);
				}
				TalentNew_talent elementById3 = GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetElementById(list[2]);
				if (elementById3 != null)
				{
					TalentProgressRewardData talentProgressRewardData2 = new TalentProgressRewardData(elementById3);
					this.talentProgressData.rewardList.Add(talentProgressRewardData2);
				}
				int exp = this.talentProgressData.rewardList[this.talentProgressData.rewardList.Count - 1].exp;
				this.talentProgressData.startLevel = num2;
				this.talentProgressData.maxLevel = exp;
				int num3 = this.talentProgressData.rewardList[this.talentProgressData.rewardList.Count - 1].id + 1;
				if (GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetElementById(num3) == null)
				{
					this.talentProgressData.isTheEndProgress = true;
				}
				else
				{
					this.talentProgressData.isTheEndProgress = false;
				}
			}
			this.talentProgressData.curId = num;
			this.talentProgressData.curLevel = this.talentData.TalentExp;
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Talent);
		}

		private void MathAddAttributeData()
		{
			this.m_addAttributeData.Clear();
			IList<TalentNew_talentEvolution> allElements = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetAllElements();
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			for (int i = 0; i < allElements.Count; i++)
			{
				TalentNew_talentEvolution talentNew_talentEvolution = allElements[i];
				if (this.talentData.TalentStage > talentNew_talentEvolution.id)
				{
					List<MergeAttributeData> mergeAttributeData = talentNew_talentEvolution.attributeGroup.GetMergeAttributeData();
					List<MergeAttributeData> mergeAttributeData2 = talentNew_talentEvolution.evolutionAttributes.GetMergeAttributeData();
					for (int j = 0; j < mergeAttributeData.Count; j++)
					{
						mergeAttributeData[j].Multiply(talentNew_talentEvolution.levelLimit);
					}
					list.AddRange(mergeAttributeData);
					list.AddRange(mergeAttributeData2);
				}
				else if (this.talentData.TalentStage == talentNew_talentEvolution.id)
				{
					List<MergeAttributeData> mergeAttributeData3 = talentNew_talentEvolution.attributeGroup.GetMergeAttributeData();
					for (int k = 0; k < mergeAttributeData3.Count; k++)
					{
						int num = (this.talentData.AttributeMap.ContainsKey(mergeAttributeData3[k].Header) ? this.talentData.AttributeMap[mergeAttributeData3[k].Header].curLevel : 0);
						mergeAttributeData3[k].Multiply(num);
					}
					list.AddRange(mergeAttributeData3);
					break;
				}
			}
			IList<TalentNew_talent> allElements2 = GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetAllElements();
			for (int l = 0; l < allElements2.Count; l++)
			{
				TalentNew_talent talentNew_talent = allElements2[l];
				if (this.talentData.TalentExp < talentNew_talent.talentLevel)
				{
					break;
				}
				if (talentNew_talent.rewardType == 2 || talentNew_talent.rewardType == 3)
				{
					List<MergeAttributeData> mergeAttributeData4 = talentNew_talent.reward.GetMergeAttributeData();
					list.AddRange(mergeAttributeData4);
				}
			}
			this.m_addAttributeData.m_attributeDatas = list.Merge();
		}

		public bool IsCheckMaxLevel()
		{
			bool flag = false;
			if (this.talentData != null)
			{
				int num = 74;
				Function_Function function_Function = GameApp.Table.GetManager().GetFunction_Function(72);
				if (function_Function != null)
				{
					num = int.Parse(function_Function.unlockArgs);
				}
				TalentNew_talentEvolution talentNew_talentEvolution = GameApp.Table.GetManager().GetTalentNew_talentEvolution(num);
				if (this.talentData.TalentExp >= talentNew_talentEvolution.exp)
				{
					flag = true;
				}
			}
			return flag;
		}

		private void OnEventRefreshData(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsTalentRefreshData eventArgsTalentRefreshData = eventArgs as EventArgsTalentRefreshData;
		}

		public TalentData talentData;

		public TalentProgressData talentProgressData;

		public AddAttributeData m_addAttributeData = new AddAttributeData();
	}
}
