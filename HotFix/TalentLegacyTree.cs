using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using HotFix.EventArgs;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class TalentLegacyTree : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_talentLegacyModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			this.Ctrl_StudyItem.Init();
			this.Button_Back.m_onClick = new Action(this.OnClickBack);
			this.ListView_TreeItem.InitListView(this.m_initCreateCount, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemIndex), null);
			for (int i = 1; i <= this.m_initCreateCount; i++)
			{
				this.m_CfgDic.Add(i, new List<TalentLegacy_talentLegacyNode>());
			}
			this.ListView_TreeItem.SetListItemCount(this.m_CfgDic.Count, false);
			this.ListView_TreeItem.RefreshAllShowItems();
		}

		protected override void OnDeInit()
		{
			this.Ctrl_StudyItem.DeInit();
			this.Button_Back.m_onClick = null;
			foreach (KeyValuePair<int, CustomBehaviour> keyValuePair in this.m_TreeItems)
			{
				TalentLegacyTreeItem component = keyValuePair.Value.GetComponent<TalentLegacyTreeItem>();
				if (component != null && component != null)
				{
					component.DeInit();
				}
			}
			for (int i = 0; i < this.LineList.Count; i++)
			{
				TalentLegacySkillLineItem component2 = this.LineList[i].GetComponent<TalentLegacySkillLineItem>();
				if (component2 != null)
				{
					component2.DeInit();
				}
			}
		}

		public void OnShow(int careerId, int talentLegacyNodeId)
		{
			if (this.m_talentLegacyModule == null)
			{
				return;
			}
			this.m_careerId = careerId;
			this.m_talentLegacyNodeId = talentLegacyNodeId;
			GameApp.Event.RegisterEvent(463, new HandlerEvent(this.OnTalentLegacyNodeTimeEnd));
			GameApp.Event.RegisterEvent(465, new HandlerEvent(this.OnTalentLegacyNodeLevelUpBack));
			GameApp.Event.RegisterEvent(466, new HandlerEvent(this.OnTalentLegacyNodeLevelUpSpeedBack));
			string talentLegacyNodeFinish = PlayerPrefsKeys.GetTalentLegacyNodeFinish();
			if (!string.IsNullOrEmpty(talentLegacyNodeFinish))
			{
				int num = int.Parse(talentLegacyNodeFinish.Split("_", StringSplitOptions.None)[0]);
				int num2 = int.Parse(talentLegacyNodeFinish.Split('_', StringSplitOptions.None)[1]);
				TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo = this.m_talentLegacyModule.OnGetTalentLegacySkillInfo(num, num2);
				if (talentLegacySkillInfo != null && talentLegacySkillInfo.LevelUpTime <= 0L)
				{
					GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).MathAddAttributeData();
					GameApp.Event.DispatchNow(null, 145, null);
					this.m_talentLegacyModule.OpenStudyFinishPanel(num, num2);
				}
			}
			this.Ctrl_StudyItem.OnShow();
			this.OnRefreshView(true);
			if (this.m_startCoroutine != null)
			{
				base.StopCoroutine(this.m_startCoroutine);
			}
			this.m_startCoroutine = base.StartCoroutine(this.OnRefresh());
			GameTGAExtend.OnViewOpen("TalentLegacyTree");
		}

		public void OnClose()
		{
			this.Ctrl_StudyItem.OnClose();
			foreach (KeyValuePair<int, CustomBehaviour> keyValuePair in this.m_TreeItems)
			{
				TalentLegacyTreeItem component = keyValuePair.Value.GetComponent<TalentLegacyTreeItem>();
				if (component != null && component != null)
				{
					component.OnClose();
				}
			}
			for (int i = 0; i < this.LineList.Count; i++)
			{
				this.LineQueue.Enqueue(this.LineList[i]);
			}
			if (this.m_coroutine != null)
			{
				base.StopCoroutine(this.m_coroutine);
			}
			if (this.m_startCoroutine != null)
			{
				base.StopCoroutine(this.m_startCoroutine);
			}
			if (this.ListView_TreeItem != null)
			{
				this.ListView_TreeItem.MovePanelToItemIndex(0, 0f);
			}
			GameApp.Event.UnRegisterEvent(463, new HandlerEvent(this.OnTalentLegacyNodeTimeEnd));
			GameApp.Event.UnRegisterEvent(465, new HandlerEvent(this.OnTalentLegacyNodeLevelUpBack));
			GameApp.Event.UnRegisterEvent(466, new HandlerEvent(this.OnTalentLegacyNodeLevelUpSpeedBack));
			GameTGAExtend.OnViewClose("TalentLegacyTree");
		}

		private void OnClickBack()
		{
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_UIOpenTalentLegacyMain, null);
		}

		private IEnumerator OnRefresh()
		{
			yield return null;
			ValueTuple<int, int> valueTuple = this.m_talentLegacyModule.IsHaveStudyingNode();
			int num;
			if (valueTuple.Item1 == -1 && valueTuple.Item2 == -1)
			{
				num = this.GetShowNodeId();
			}
			else if (valueTuple.Item1 == this.m_careerId)
			{
				num = valueTuple.Item2;
			}
			else
			{
				num = this.GetShowNodeId();
			}
			TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(num);
			if (talentLegacy_talentLegacyNode != null)
			{
				int num2 = int.Parse(talentLegacy_talentLegacyNode.pos[0]);
				this.ListView_TreeItem.MovePanelToItemIndex(num2 - 1, 0f);
			}
			else
			{
				this.ListView_TreeItem.MovePanelToItemIndex(0, 0f);
			}
			this.ListView_TreeItem.FinishSnapImmediately();
			this.OnInitLine();
			yield break;
		}

		private void OnRefreshView(bool isInit = false)
		{
			ValueTuple<int, int> valueTuple = this.m_talentLegacyModule.IsHaveStudyingNode();
			if (valueTuple.Item1 == -1 && valueTuple.Item2 == -1)
			{
				this.Ctrl_StudyItem.gameObject.SetActiveSafe(false);
			}
			else
			{
				this.Ctrl_StudyItem.gameObject.SetActiveSafe(true);
				this.Ctrl_StudyItem.SetData(valueTuple.Item1, valueTuple.Item2);
			}
			this.m_CfgDic = this.m_talentLegacyModule.OnGetTree(this.m_careerId);
			this.m_isFirstSpecialNode = this.OnHaveFirstSpecialNode();
			this.itemSpaceHeight = (float)(this.m_isFirstSpecialNode ? 150 : 0);
			this.ListView_TreeItem.SetListItemCount(this.m_CfgDic.Count + 2, false);
			if (isInit)
			{
				this.ListView_TreeItem.PrepareAllItemSize(new Vector2(this.m_treeItemWidth, this.m_treeItemHeight + this.rowSpace));
			}
			this.ListView_TreeItem.RefreshAllShowItems();
		}

		private int GetShowNodeId()
		{
			int num = 0;
			List<TalentLegacy_talentLegacyNode> careerTalentLegacyListAllCfg = this.m_talentLegacyModule.GetCareerTalentLegacyListAllCfg(this.m_careerId, -1);
			for (int i = 0; i < careerTalentLegacyListAllCfg.Count; i++)
			{
				bool flag = this.m_talentLegacyModule.IsUnlockTalentLegacyNode(careerTalentLegacyListAllCfg[i].id);
				bool flag2 = this.m_talentLegacyModule.OnGetTalentLegacyCareerRed(this.m_careerId, careerTalentLegacyListAllCfg[i].id, true) == 1;
				if (flag && flag2)
				{
					num = careerTalentLegacyListAllCfg[i].id;
					break;
				}
			}
			return num;
		}

		private bool OnHaveFirstSpecialNode()
		{
			using (Dictionary<int, List<TalentLegacy_talentLegacyNode>>.Enumerator enumerator = this.m_CfgDic.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<int, List<TalentLegacy_talentLegacyNode>> keyValuePair = enumerator.Current;
					if (keyValuePair.Key == 1)
					{
						int num = 0;
						if (num < keyValuePair.Value.Count)
						{
							if (keyValuePair.Value[num].type == 3 || keyValuePair.Value[num].type == 4)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		private LoopListViewItem2 OnGetItemIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			if (index == 0 && this.m_isFirstSpecialNode)
			{
				LoopListViewItem2 loopListViewItem = listView.NewListViewItem("TopSpaceItem");
				CustomBehaviour customBehaviour;
				this.m_TreeItems.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out customBehaviour);
				if (customBehaviour == null)
				{
					UIItemSpace component = loopListViewItem.GetComponent<UIItemSpace>();
					this.m_TreeItems[loopListViewItem.gameObject.GetInstanceID()] = component;
				}
				return loopListViewItem;
			}
			if (index > this.m_CfgDic.Count)
			{
				LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("TopSpaceItem");
				CustomBehaviour customBehaviour2;
				this.m_TreeItems.TryGetValue(loopListViewItem2.gameObject.GetInstanceID(), out customBehaviour2);
				if (customBehaviour2 == null)
				{
					UIItemSpace component2 = loopListViewItem2.GetComponent<UIItemSpace>();
					this.m_TreeItems[loopListViewItem2.gameObject.GetInstanceID()] = component2;
				}
				return loopListViewItem2;
			}
			int num = index;
			if (!this.m_isFirstSpecialNode)
			{
				num = index + 1;
			}
			if (!this.m_CfgDic.ContainsKey(num))
			{
				return null;
			}
			List<TalentLegacy_talentLegacyNode> list = this.m_CfgDic[num];
			LoopListViewItem2 loopListViewItem3 = listView.NewListViewItem("TalentLegacyTreeItem");
			CustomBehaviour component3;
			this.m_TreeItems.TryGetValue(loopListViewItem3.gameObject.GetInstanceID(), out component3);
			if (component3 == null)
			{
				component3 = loopListViewItem3.GetComponent<TalentLegacyTreeItem>();
				component3.Init();
				this.m_TreeItems[loopListViewItem3.gameObject.GetInstanceID()] = component3;
			}
			TalentLegacyTreeItem component4 = component3.GetComponent<TalentLegacyTreeItem>();
			if (component4 != null)
			{
				component4.OnShow();
			}
			TalentLegacyTreeItem component5 = component3.GetComponent<TalentLegacyTreeItem>();
			if (component5 != null)
			{
				component5.SetData(index, this.m_careerId, list);
			}
			return loopListViewItem3;
		}

		private void OnTalentLegacyNodeLevelUpSpeedBack(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnRefreshView(false);
		}

		private void OnTalentLegacyNodeLevelUpBack(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnRefreshView(false);
		}

		private void OnTalentLegacyNodeTimeEnd(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsNodeTimeEnd eventArgsNodeTimeEnd = eventargs as EventArgsNodeTimeEnd;
			if (eventArgsNodeTimeEnd != null)
			{
				this.m_talentLegacyModule.OpenStudyFinishPanel(eventArgsNodeTimeEnd.CareerId, eventArgsNodeTimeEnd.TalentLegacyNodeId);
			}
			this.OnRefreshView(false);
		}

		private void OnCollectLineRelation()
		{
			List<TalentLegacy_talentLegacyNode> careerTalentLegacyListAllCfg = this.m_talentLegacyModule.GetCareerTalentLegacyListAllCfg(this.m_careerId, -1);
			this.m_treeDic.Clear();
			for (int i = 0; i < careerTalentLegacyListAllCfg.Count; i++)
			{
				this.m_treeDic.Add(careerTalentLegacyListAllCfg[i].id, new List<int>());
			}
			for (int j = 0; j < careerTalentLegacyListAllCfg.Count; j++)
			{
				if (careerTalentLegacyListAllCfg[j].condition.Length != 0)
				{
					for (int k = 0; k < careerTalentLegacyListAllCfg[j].condition.Length; k++)
					{
						int num = int.Parse(careerTalentLegacyListAllCfg[j].condition[k].Split(',', StringSplitOptions.None)[0]);
						if (this.m_treeDic.ContainsKey(num))
						{
							this.m_treeDic[num].Add(careerTalentLegacyListAllCfg[j].id);
						}
					}
				}
			}
		}

		private void OnInitLine()
		{
			for (int i = 0; i < this.LineList.Count; i++)
			{
				TalentLegacySkillLineItem component = this.LineList[i].GetComponent<TalentLegacySkillLineItem>();
				if (component != null)
				{
					component.DeInit();
				}
			}
			this.LineList.Clear();
			this.OnCollectLineRelation();
			if (this.m_coroutine != null)
			{
				base.StopCoroutine(this.m_coroutine);
			}
			this.m_coroutine = base.StartCoroutine(this.OnCreateLine());
		}

		private IEnumerator OnCreateLine()
		{
			this.m_createLineIndex = 0;
			foreach (KeyValuePair<int, List<int>> keyValuePair in this.m_treeDic)
			{
				TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(keyValuePair.Key);
				for (int i = 0; i < keyValuePair.Value.Count; i++)
				{
					TalentLegacy_talentLegacyNode talentLegacy_talentLegacyNode2 = GameApp.Table.GetManager().GetTalentLegacy_talentLegacyNode(keyValuePair.Value[i]);
					int num = int.Parse(talentLegacy_talentLegacyNode.pos[0]);
					int num2 = int.Parse(talentLegacy_talentLegacyNode.pos[1]);
					int num3 = int.Parse(talentLegacy_talentLegacyNode2.pos[0]);
					int num4 = int.Parse(talentLegacy_talentLegacyNode2.pos[1]);
					if (num3 > num)
					{
						if (num2 == num4)
						{
							GameObject gameObject = this.OnGetLine();
							gameObject.name = HLog.StringBuilder(num.ToString(), "_", num2.ToString());
							RectTransform component = gameObject.GetComponent<RectTransform>();
							component.pivot = new Vector2(0f, 1f);
							component.eulerAngles = new Vector3(0f, 0f, 0f);
							component.sizeDelta = new Vector2(14f, this.m_treeItemHeight + this.rowSpace);
							float num5 = this.startLeftEdge + (float)num2 * this.m_nodeItemWidth + (float)(num2 - 1) * this.colSpace;
							num5 -= this.m_nodeItemWidth / 2f;
							float num6 = -((float)num * this.m_treeItemHeight + (float)(num - 1) * this.rowSpace + this.itemSpaceHeight);
							num6 += this.m_treeItemHeight / 2f;
							gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(num5, num6, 0f);
							TalentLegacySkillLineItem component2 = gameObject.GetComponent<TalentLegacySkillLineItem>();
							if (component2 != null)
							{
								component2.SetData(this.m_careerId, keyValuePair.Value[i]);
							}
						}
						else if (num4 > num2 && keyValuePair.Value.Count < 2)
						{
							GameObject gameObject2 = this.OnGetLine();
							gameObject2.name = HLog.StringBuilder(num.ToString(), "_", num2.ToString());
							RectTransform component3 = gameObject2.GetComponent<RectTransform>();
							component3.eulerAngles = new Vector3(0f, 0f, 90f);
							component3.pivot = new Vector2(1f, 1f);
							int num7 = Utility.Math.Abs(num4 - num2);
							component3.sizeDelta = new Vector2(14f, (float)num7 * this.m_nodeItemWidth + this.colSpace);
							float num8 = this.startLeftEdge + (float)num2 * this.m_nodeItemWidth + (float)(num2 - 1) * this.colSpace;
							num8 -= this.m_nodeItemWidth / 2f;
							float num9 = -((float)num * this.m_treeItemHeight + (float)(num - 1) * this.rowSpace + this.itemSpaceHeight);
							num9 += this.m_treeItemHeight / 2f;
							gameObject2.GetComponent<RectTransform>().anchoredPosition = new Vector3(num8, num9, 0f);
							TalentLegacySkillLineItem component4 = gameObject2.GetComponent<TalentLegacySkillLineItem>();
							if (component4 != null)
							{
								component4.SetData(this.m_careerId, keyValuePair.Value[i]);
							}
							num2 = num4;
							GameObject gameObject3 = this.OnGetLine();
							gameObject3.name = HLog.StringBuilder(num.ToString(), "_", num2.ToString());
							RectTransform component5 = gameObject3.GetComponent<RectTransform>();
							component5.pivot = new Vector2(1f, 1f);
							component5.eulerAngles = new Vector3(0f, 0f, 0f);
							component5.sizeDelta = new Vector2(14f, this.m_treeItemHeight + this.rowSpace);
							num8 = this.startLeftEdge + (float)num2 * this.m_nodeItemWidth + (float)(num2 - 1) * this.colSpace;
							num8 -= this.m_nodeItemWidth / 2f;
							num8 -= 22f;
							num9 = -((float)num * this.m_treeItemHeight + (float)(num - 1) * this.rowSpace + this.itemSpaceHeight);
							num9 += this.m_treeItemHeight / 2f;
							gameObject3.GetComponent<RectTransform>().anchoredPosition = new Vector3(num8, num9, 0f);
							TalentLegacySkillLineItem component6 = gameObject3.GetComponent<TalentLegacySkillLineItem>();
							if (component6 != null)
							{
								component6.SetData(this.m_careerId, keyValuePair.Value[i]);
							}
						}
						else if (num4 < num2 && keyValuePair.Value.Count < 2)
						{
							GameObject gameObject4 = this.OnGetLine();
							gameObject4.name = HLog.StringBuilder(num.ToString(), "_", num2.ToString());
							RectTransform component7 = gameObject4.GetComponent<RectTransform>();
							component7.eulerAngles = new Vector3(0f, 0f, -90f);
							component7.pivot = new Vector2(0f, 1f);
							int num10 = Utility.Math.Abs(num4 - num2);
							component7.sizeDelta = new Vector2(14f, (float)num10 * this.m_nodeItemWidth + this.colSpace);
							float num11 = this.startLeftEdge + (float)num2 * this.m_nodeItemWidth + (float)(num2 - 1) * this.colSpace;
							num11 -= this.m_nodeItemWidth / 2f;
							float num12 = -((float)num * this.m_treeItemHeight + (float)(num - 1) * this.rowSpace + this.itemSpaceHeight);
							num12 += this.m_treeItemHeight / 2f;
							gameObject4.GetComponent<RectTransform>().anchoredPosition = new Vector3(num11, num12, 0f);
							TalentLegacySkillLineItem component8 = gameObject4.GetComponent<TalentLegacySkillLineItem>();
							if (component8 != null)
							{
								component8.SetData(this.m_careerId, keyValuePair.Value[i]);
							}
							num2 = num4;
							GameObject gameObject5 = this.OnGetLine();
							gameObject5.name = HLog.StringBuilder(num.ToString(), "_", num2.ToString());
							RectTransform component9 = gameObject5.GetComponent<RectTransform>();
							component9.pivot = new Vector2(0f, 1f);
							component9.eulerAngles = new Vector3(0f, 0f, 0f);
							component9.sizeDelta = new Vector2(14f, this.m_treeItemHeight + this.rowSpace);
							num11 = this.startLeftEdge + (float)num2 * this.m_nodeItemWidth + (float)(num2 - 1) * this.colSpace;
							num11 -= this.m_nodeItemWidth / 2f;
							num11 += 22f;
							num12 = -((float)num * this.m_treeItemHeight + (float)(num - 1) * this.rowSpace + this.itemSpaceHeight);
							num12 += this.m_treeItemHeight / 2f;
							gameObject5.GetComponent<RectTransform>().anchoredPosition = new Vector3(num11, num12, 0f);
							TalentLegacySkillLineItem component10 = gameObject5.GetComponent<TalentLegacySkillLineItem>();
							if (component10 != null)
							{
								component10.SetData(this.m_careerId, keyValuePair.Value[i]);
							}
						}
						else if (num4 > num2 && keyValuePair.Value.Count >= 2)
						{
							GameObject gameObject6 = this.OnGetLine();
							gameObject6.name = HLog.StringBuilder(num.ToString(), "_", num2.ToString());
							RectTransform component11 = gameObject6.GetComponent<RectTransform>();
							component11.pivot = new Vector2(0f, 1f);
							component11.eulerAngles = new Vector3(0f, 0f, 0f);
							component11.sizeDelta = new Vector2(14f, this.m_treeItemHeight + this.rowSpace);
							float num13 = this.startLeftEdge + (float)num2 * this.m_nodeItemWidth + (float)(num2 - 1) * this.colSpace;
							num13 -= this.m_nodeItemWidth / 2f;
							num13 += 22f;
							float num14 = -((float)num * this.m_treeItemHeight + (float)(num - 1) * this.rowSpace + this.itemSpaceHeight);
							num14 += this.m_treeItemHeight / 2f;
							gameObject6.GetComponent<RectTransform>().anchoredPosition = new Vector3(num13, num14, 0f);
							TalentLegacySkillLineItem component12 = gameObject6.GetComponent<TalentLegacySkillLineItem>();
							if (component12 != null)
							{
								component12.SetData(this.m_careerId, keyValuePair.Value[i]);
							}
							int num15 = num2;
							num = num3;
							num2 = num4;
							num4 = num15;
							GameObject gameObject7 = this.OnGetLine();
							gameObject7.name = HLog.StringBuilder(num.ToString(), "_", num2.ToString(), "右边");
							RectTransform component13 = gameObject7.GetComponent<RectTransform>();
							component13.eulerAngles = new Vector3(0f, 0f, 90f);
							component13.pivot = new Vector2(1f, 0f);
							int num16 = Utility.Math.Abs(num4 - num2);
							component13.sizeDelta = new Vector2(14f, (float)num16 * this.m_nodeItemWidth + this.colSpace);
							num13 = this.startLeftEdge + (float)num2 * this.m_nodeItemWidth + (float)(num2 - 1) * this.colSpace;
							num13 -= this.m_nodeItemWidth / 2f;
							num14 = -((float)num * this.m_treeItemHeight + (float)(num - 1) * this.rowSpace + this.itemSpaceHeight);
							num14 += this.m_treeItemHeight / 2f;
							gameObject7.GetComponent<RectTransform>().anchoredPosition = new Vector3(num13, num14, 0f);
							TalentLegacySkillLineItem component14 = gameObject7.GetComponent<TalentLegacySkillLineItem>();
							if (component14 != null)
							{
								component14.SetData(this.m_careerId, keyValuePair.Value[i]);
							}
						}
						else if (num4 < num2 && keyValuePair.Value.Count >= 2)
						{
							GameObject gameObject8 = this.OnGetLine();
							gameObject8.name = HLog.StringBuilder(num.ToString(), "_", num2.ToString());
							RectTransform component15 = gameObject8.GetComponent<RectTransform>();
							component15.pivot = new Vector2(1f, 1f);
							component15.eulerAngles = new Vector3(0f, 0f, 0f);
							component15.sizeDelta = new Vector2(14f, this.m_treeItemHeight + this.rowSpace);
							float num17 = this.startLeftEdge + (float)num2 * this.m_nodeItemWidth + (float)(num2 - 1) * this.colSpace;
							num17 -= this.m_nodeItemWidth / 2f;
							num17 -= 22f;
							float num18 = -((float)num * this.m_treeItemHeight + (float)(num - 1) * this.rowSpace + this.itemSpaceHeight);
							num18 += this.m_treeItemHeight / 2f;
							gameObject8.GetComponent<RectTransform>().anchoredPosition = new Vector3(num17, num18, 0f);
							TalentLegacySkillLineItem component16 = gameObject8.GetComponent<TalentLegacySkillLineItem>();
							if (component16 != null)
							{
								component16.SetData(this.m_careerId, keyValuePair.Value[i]);
							}
							int num19 = num2;
							num = num3;
							num2 = num4;
							num4 = num19;
							GameObject gameObject9 = this.OnGetLine();
							gameObject9.name = HLog.StringBuilder(num.ToString(), "_", num2.ToString(), "左边");
							RectTransform component17 = gameObject9.GetComponent<RectTransform>();
							component17.eulerAngles = new Vector3(0f, 0f, -90f);
							component17.pivot = new Vector2(0f, 0f);
							int num20 = Utility.Math.Abs(num4 - num2);
							component17.sizeDelta = new Vector2(14f, (float)num20 * this.m_nodeItemWidth + this.colSpace);
							num17 = this.startLeftEdge + (float)num2 * this.m_nodeItemWidth + (float)(num2 - 1) * this.colSpace;
							num17 -= this.m_nodeItemWidth / 2f;
							num18 = -((float)num * this.m_treeItemHeight + (float)(num - 1) * this.rowSpace + this.itemSpaceHeight);
							num18 += this.m_treeItemHeight / 2f;
							gameObject9.GetComponent<RectTransform>().anchoredPosition = new Vector3(num17, num18, 0f);
							TalentLegacySkillLineItem component18 = gameObject9.GetComponent<TalentLegacySkillLineItem>();
							if (component18 != null)
							{
								component18.SetData(this.m_careerId, keyValuePair.Value[i]);
							}
						}
					}
					else if (num3 == num)
					{
						if (num4 > num2)
						{
							GameObject gameObject10 = this.OnGetLine();
							gameObject10.name = HLog.StringBuilder(num.ToString(), "_", num2.ToString(), "右边");
							RectTransform component19 = gameObject10.GetComponent<RectTransform>();
							component19.eulerAngles = new Vector3(0f, 0f, 90f);
							component19.pivot = new Vector2(1f, 1f);
							int num21 = Utility.Math.Abs(num4 - num2);
							component19.sizeDelta = new Vector2(14f, (float)num21 * this.m_nodeItemWidth + this.colSpace);
							float num22 = this.startLeftEdge + (float)num2 * this.m_nodeItemWidth + (float)(num2 - 1) * this.colSpace;
							num22 -= this.m_nodeItemWidth / 2f;
							float num23 = -((float)num * this.m_treeItemHeight + (float)(num - 1) * this.rowSpace + this.itemSpaceHeight);
							num23 += this.m_treeItemHeight / 2f;
							gameObject10.GetComponent<RectTransform>().anchoredPosition = new Vector3(num22, num23, 0f);
							TalentLegacySkillLineItem component20 = gameObject10.GetComponent<TalentLegacySkillLineItem>();
							if (component20 != null)
							{
								component20.SetData(this.m_careerId, keyValuePair.Value[i]);
							}
						}
						else if (num4 < num2)
						{
							GameObject gameObject11 = this.OnGetLine();
							gameObject11.name = HLog.StringBuilder(num.ToString(), "_", num2.ToString(), "左边");
							RectTransform component21 = gameObject11.GetComponent<RectTransform>();
							component21.eulerAngles = new Vector3(0f, 0f, -90f);
							component21.pivot = new Vector2(0f, 1f);
							int num24 = Utility.Math.Abs(num4 - num2);
							component21.sizeDelta = new Vector2(14f, (float)num24 * this.m_nodeItemWidth + this.colSpace);
							float num25 = this.startLeftEdge + (float)num2 * this.m_nodeItemWidth + (float)(num2 - 1) * this.colSpace;
							num25 -= this.m_nodeItemWidth / 2f;
							float num26 = -((float)num * this.m_treeItemHeight + (float)(num - 1) * this.rowSpace + this.itemSpaceHeight);
							num26 += this.m_treeItemHeight / 2f;
							gameObject11.GetComponent<RectTransform>().anchoredPosition = new Vector3(num25, num26, 0f);
							TalentLegacySkillLineItem component22 = gameObject11.GetComponent<TalentLegacySkillLineItem>();
							if (component22 != null)
							{
								component22.SetData(this.m_careerId, keyValuePair.Value[i]);
							}
						}
					}
				}
				this.m_createLineIndex++;
				if (this.m_createLineIndex % this.m_ObjectsPerFrame == 0)
				{
					yield return null;
				}
			}
			Dictionary<int, List<int>>.Enumerator enumerator = default(Dictionary<int, List<int>>.Enumerator);
			yield break;
			yield break;
		}

		private GameObject OnGetLine()
		{
			if (this.LineQueue.Count > 0)
			{
				GameObject gameObject = this.LineQueue.Dequeue();
				this.LineList.Add(gameObject);
				return gameObject;
			}
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.LineItem, this.Obj_Line.transform);
			gameObject2.gameObject.SetActiveSafe(true);
			this.LineList.Add(gameObject2);
			TalentLegacySkillLineItem component = gameObject2.GetComponent<TalentLegacySkillLineItem>();
			if (component != null)
			{
				component.Init();
			}
			return gameObject2;
		}

		public LoopListView2 ListView_TreeItem;

		public GameObject Obj_Line;

		public GameObject LineItem;

		public TalentLegacyStudyItem Ctrl_StudyItem;

		public CustomButton Button_Back;

		private TalentLegacyDataModule m_talentLegacyModule;

		private int m_careerId;

		private int m_talentLegacyNodeId;

		private Dictionary<int, CustomBehaviour> m_TreeItems = new Dictionary<int, CustomBehaviour>();

		private Dictionary<int, List<TalentLegacy_talentLegacyNode>> m_CfgDic = new Dictionary<int, List<TalentLegacy_talentLegacyNode>>();

		private Dictionary<int, List<int>> m_treeDic = new Dictionary<int, List<int>>();

		private List<GameObject> LineList = new List<GameObject>();

		private Queue<GameObject> LineQueue = new Queue<GameObject>();

		private float m_treeItemWidth = 958f;

		private float m_treeItemHeight = 174f;

		private float m_nodeItemWidth = 174f;

		private float m_nodeItemHeight = 174f;

		private float startLeftEdge = 60f;

		private float colSpace = 22f;

		private float rowSpace = 126f;

		private float itemSpaceHeight = 150f;

		private Coroutine m_coroutine;

		private Coroutine m_startCoroutine;

		private bool m_isFirstSpecialNode;

		private int m_initCreateCount = 5;

		private int m_ObjectsPerFrame = 2;

		private int m_createLineIndex;
	}
}
