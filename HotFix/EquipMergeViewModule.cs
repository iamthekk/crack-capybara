using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Common;
using Proto.Equip;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class EquipMergeViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_equipDataModule = GameApp.Data.GetDataModule(DataName.EquipDataModule);
			this.m_propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.m_equipGroup.m_onClickEquipItem = new Action<UIHeroEquipItem>(this.OnClickEquipItemForMergeEquipGroup);
			this.m_equipGroup.m_onClickPropItem = new Action<UIItem>(this.OnClickPropItemForMergeEquipGroup);
			this.m_equipGroup.m_onClickNextEquipItem = new Action<UIHeroEquipItem>(this.OnClickNextEquipItem);
			this.m_titleGroup.m_onClickOneClickMergeBt = new Action<UIEquipMergeTitleGroup>(this.OnClickOneClickMergeBt);
			this.m_titleGroup.m_onClickFilterBt = new Action<UIEquipMergeTitleGroup>(this.OnClickFilterBt);
			this.mFlyEquipGroup.Init();
			this.m_filterSelectGroup.Init();
			this.m_equipGroup.Init();
			this.m_titleGroup.Init();
			this.mFlyEquipGroup.SetActive(false);
			this.m_gridView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		public override void OnOpen(object data)
		{
			this.mFlyEquipGroup.SetActive(false);
			this.mFlyPropGroup.SetActive(false);
			this._curTime = Time.unscaledTime;
			this.m_currentEquipType = 0;
			this.m_currentSelectRowID = 0UL;
			this.m_mergeRowIDs.Clear();
			this.m_closeBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			this.m_mergeBt.onClick.AddListener(new UnityAction(this.OnClickMergeBt));
			this.m_mergeBt.gameObject.SetActive(false);
			this.m_filterSelectGroup.m_onClose = new Action<int>(this.OnCloseForFilterSelectGroup);
			this.m_filterSelectGroup.SetSelect(this.m_currentEquipType);
			this.m_filterSelectGroup.SetActive(false);
			this.OnRefreshEquipList();
			this.OnRefreshTitleGroup();
			this.OnCheckMergeBt();
			this.m_viewAnimator.Play("open");
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.mFlyEquipGroup != null)
			{
				this.mFlyEquipGroup.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (this.m_flyItems != null && this.m_flyItems.Count > 0)
			{
				for (int i = 0; i < this.m_flyItems.Count; i++)
				{
					CustomBehaviour customBehaviour = this.m_flyItems[i];
					if (!(customBehaviour == null))
					{
						customBehaviour.OnUpdate(deltaTime, unscaledDeltaTime);
					}
				}
			}
		}

		public override void OnClose()
		{
			this.OnUnSelectMainEquip();
			this.m_currentSelectRowID = 0UL;
			this.m_mergeRowIDs = new List<long>();
			this.m_currentEquipType = 0;
			if (this.m_closeBt != null)
			{
				this.m_closeBt.onClick.RemoveListener(new UnityAction(this.OnClickCloseBt));
			}
			if (this.m_mergeBt != null)
			{
				this.m_mergeBt.onClick.RemoveListener(new UnityAction(this.OnClickMergeBt));
			}
			this.m_equipDatas.Clear();
			this.m_nodeDatas.Clear();
		}

		public override void OnDelete()
		{
			this.m_closeBt = null;
			this.m_mergeBt = null;
			this.m_filterSelectGroup.DeInit();
			this.m_filterSelectGroup = null;
			this.m_equipGroup.DeInit();
			this.m_equipGroup = null;
			this.m_titleGroup.DeInit();
			this.m_titleGroup = null;
			this.mFlyEquipGroup.DeInit();
			this.mFlyEquipGroup = null;
			foreach (KeyValuePair<int, CustomBehaviour> keyValuePair in this.m_nodeDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.m_nodeDic.Clear();
			this.m_gridView = null;
			this.m_equipDataModule = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnCheckMergeBt()
		{
			bool flag = this.m_currentSelectRowID > 0UL;
			for (int i = 0; i < this.m_mergeRowIDs.Count; i++)
			{
				if (this.m_mergeRowIDs[i] == 0L)
				{
					flag = false;
					break;
				}
			}
			this.m_mergeBt.gameObject.SetActive(flag);
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.m_nodeDatas.Count)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = null;
			EquipMergeViewModule.NodeData nodeData = this.m_nodeDatas[index];
			if (nodeData.m_isTopSpace)
			{
				return view.NewListViewItem("TopSpace");
			}
			if (nodeData.m_isBottomSpace)
			{
				loopListViewItem = view.NewListViewItem("BottomSpace");
				int instanceID = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour;
				this.m_nodeDic.TryGetValue(instanceID, out customBehaviour);
				if (customBehaviour == null)
				{
					UIItemSpace component = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component.Init();
					this.m_nodeDic[instanceID] = component;
				}
				return loopListViewItem;
			}
			if (nodeData.m_equipDatas != null)
			{
				int num = index - 1;
				float num2;
				if (num == 0)
				{
					num2 = this._curTime;
				}
				bool flag;
				if (num < 4)
				{
					num2 = this._curTime + (float)(num * this.m_columnCount) * 0.02f + 0.02f;
					flag = true;
				}
				else
				{
					num2 = this._curTime + (float)(num * this.m_columnCount) * 0.02f + 0.02f;
					flag = false;
				}
				loopListViewItem = view.NewListViewItem("Node");
				int instanceID2 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour2;
				this.m_nodeDic.TryGetValue(instanceID2, out customBehaviour2);
				if (customBehaviour2 == null)
				{
					UIEquipMergeEquipItemGroup component2 = loopListViewItem.gameObject.GetComponent<UIEquipMergeEquipItemGroup>();
					component2.SetData(this.m_columnCount, new Action<UIHeroEquipItem>(this.OnClickEquipItemForItem));
					component2.Init();
					customBehaviour2 = component2;
					this.m_nodeDic[instanceID2] = component2;
				}
				UIEquipMergeEquipItemGroup uiequipMergeEquipItemGroup = customBehaviour2 as UIEquipMergeEquipItemGroup;
				if (uiequipMergeEquipItemGroup != null)
				{
					uiequipMergeEquipItemGroup.RefreshData(nodeData, num2, 0.2f, flag);
				}
				return loopListViewItem;
			}
			if (!string.IsNullOrEmpty(nodeData.m_titleName))
			{
				loopListViewItem = view.NewListViewItem("Title");
				int instanceID3 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour3;
				this.m_nodeDic.TryGetValue(instanceID3, out customBehaviour3);
				if (customBehaviour3 == null)
				{
					UIHeroEquipItemTitle component3 = loopListViewItem.gameObject.GetComponent<UIHeroEquipItemTitle>();
					component3.Init();
					this.m_nodeDic[instanceID3] = component3;
					customBehaviour3 = component3;
				}
				UIHeroEquipItemTitle uiheroEquipItemTitle = customBehaviour3 as UIHeroEquipItemTitle;
				if (uiheroEquipItemTitle != null)
				{
					uiheroEquipItemTitle.RefreshData(nodeData.m_titleName);
				}
				return loopListViewItem;
			}
			if (nodeData.m_propDatas != null)
			{
				int num3 = index - 1;
				if (num3 <= 0)
				{
					int count = nodeData.m_propDatas.Count;
				}
				else
				{
					int count2 = nodeData.m_propDatas.Count;
					int columnCount = this.m_columnCount;
				}
				float num4;
				if (num3 == 0)
				{
					num4 = this._curTime;
				}
				bool flag2;
				if (num3 < 4)
				{
					num4 = this._curTime + (float)(num3 * this.m_columnCount) * 0.02f + 0.02f;
					flag2 = true;
				}
				else
				{
					num4 = this._curTime + (float)(num3 * this.m_columnCount) * 0.02f + 0.02f;
					flag2 = false;
				}
				loopListViewItem = view.NewListViewItem("PropNode");
				int instanceID4 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour4;
				this.m_nodeDic.TryGetValue(instanceID4, out customBehaviour4);
				if (customBehaviour4 == null)
				{
					UIEquipMergePropItemGroup component4 = loopListViewItem.gameObject.GetComponent<UIEquipMergePropItemGroup>();
					component4.SetData(this.m_columnCount, new Action<UIItem, PropData, object>(this.OnClickPropItemForItem));
					component4.Init();
					customBehaviour4 = component4;
					this.m_nodeDic[instanceID4] = component4;
				}
				UIEquipMergePropItemGroup uiequipMergePropItemGroup = customBehaviour4 as UIEquipMergePropItemGroup;
				if (uiequipMergePropItemGroup != null)
				{
					uiequipMergePropItemGroup.RefreshData(nodeData, num4, 0.2f, flag2);
				}
				return loopListViewItem;
			}
			return loopListViewItem;
		}

		private void IsShowMask(EquipData current, EquipData target, out bool isShowLock, out bool IsShowTick)
		{
			if (current == null || target == null)
			{
				isShowLock = false;
				IsShowTick = false;
				return;
			}
			if (this.m_equipDataModule.IsPutOn(target.rowID))
			{
				IsShowTick = current == target;
				isShowLock = current != target;
				return;
			}
			bool flag = false;
			bool flag2 = true;
			for (int i = 0; i < this.m_mergeRowIDs.Count; i++)
			{
				if (this.m_mergeRowIDs[i] == 0L)
				{
					flag2 = false;
				}
				if (this.m_mergeRowIDs[i] == (long)target.rowID)
				{
					flag = true;
				}
			}
			if (current == target || flag)
			{
				isShowLock = false;
				IsShowTick = true;
				return;
			}
			if (flag2)
			{
				isShowLock = true;
				IsShowTick = false;
				return;
			}
			bool flag3 = this.m_equipDataModule.IsCanToMerge(current, target, true);
			isShowLock = !flag3;
			IsShowTick = false;
		}

		private void IsShowMask(EquipData current, PropData target, out bool isShowLock, out bool IsShowTick)
		{
			if (current == null || target == null)
			{
				isShowLock = true;
				IsShowTick = false;
				return;
			}
			bool flag = this.m_equipDataModule.IsCanToMerge(current, target);
			bool flag2 = false;
			bool flag3 = true;
			for (int i = 0; i < this.m_mergeRowIDs.Count; i++)
			{
				if (this.m_mergeRowIDs[i] == 0L)
				{
					flag3 = false;
				}
				if (this.m_mergeRowIDs[i] == -1L)
				{
					flag2 = true;
				}
			}
			if (flag2 && flag)
			{
				isShowLock = false;
				IsShowTick = true;
				return;
			}
			if (flag3)
			{
				isShowLock = true;
				IsShowTick = false;
				return;
			}
			isShowLock = !flag;
			IsShowTick = false;
		}

		private void OnRefreshEquipList()
		{
			bool flag = true;
			EquipData equipDataByRowId = this.m_equipDataModule.GetEquipDataByRowId(this.m_currentSelectRowID);
			if (this.m_currentSelectRowID == 0UL)
			{
				this.m_equipDatas = this.m_equipDataModule.GetEquipDatasForMerge(this.m_currentEquipType);
			}
			else
			{
				flag = false;
				this.m_equipDatas = this.m_equipDataModule.GetEquipDatasForMerge((int)equipDataByRowId.equipType);
			}
			this.m_nodeDatas.Clear();
			this.m_nodeDatas.Add(new EquipMergeViewModule.NodeData
			{
				m_isTopSpace = true
			});
			for (int i = 0; i < this.m_equipDatas.Count; i += this.m_columnCount)
			{
				EquipMergeViewModule.NodeData nodeData = new EquipMergeViewModule.NodeData();
				nodeData.m_equipDatas = new List<UIEquipMergeEquipItemGroup.NodeItemData>();
				for (int j = 0; j < this.m_columnCount; j++)
				{
					int num = i + j;
					UIEquipMergeEquipItemGroup.NodeItemData nodeItemData = new UIEquipMergeEquipItemGroup.NodeItemData();
					if (num < this.m_equipDatas.Count)
					{
						Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(this.m_equipDatas[num].composeId);
						bool flag2 = elementById == null || elementById.composeTo <= 0;
						nodeItemData.m_equipData = this.m_equipDatas[num];
						nodeItemData.m_putonActive = this.m_equipDataModule.IsPutOn(nodeItemData.m_equipData.rowID);
						nodeItemData.m_isCanMerge = this.m_equipDataModule.IsCanMerge(nodeItemData.m_equipData.rowID);
						this.IsShowMask(equipDataByRowId, nodeItemData.m_equipData, out nodeItemData.m_lockActive, out nodeItemData.m_tickActive);
						if (flag2)
						{
							nodeItemData.m_lockActive = flag2;
						}
						nodeData.m_equipDatas.Add(nodeItemData);
					}
				}
				this.m_nodeDatas.Add(nodeData);
			}
			List<ItemData> itemDatas = this.m_propDataModule.GetItemDatas(ItemType.eUseItem, new PropType?(PropType.EquipComposeMaterial));
			if (itemDatas.Count > 0)
			{
				this.m_nodeDatas.Add(new EquipMergeViewModule.NodeData
				{
					m_titleName = Singleton<LanguageManager>.Instance.GetInfoByID("title_equip_material")
				});
				for (int k = 0; k < itemDatas.Count; k += this.m_columnCount)
				{
					EquipMergeViewModule.NodeData nodeData2 = new EquipMergeViewModule.NodeData();
					nodeData2.m_propDatas = new List<UIEquipMergePropItemGroup.NodePropData>();
					for (int l = 0; l < this.m_columnCount; l++)
					{
						int num2 = k + l;
						UIEquipMergePropItemGroup.NodePropData nodePropData = new UIEquipMergePropItemGroup.NodePropData();
						if (num2 < itemDatas.Count)
						{
							nodePropData.m_propData = itemDatas[num2].ToPropData();
							this.IsShowMask(equipDataByRowId, nodePropData.m_propData, out nodePropData.m_lockActive, out nodePropData.m_tickActive);
							nodeData2.m_propDatas.Add(nodePropData);
						}
					}
					this.m_nodeDatas.Add(nodeData2);
				}
			}
			this.m_nodeDatas.Add(new EquipMergeViewModule.NodeData
			{
				m_isBottomSpace = true
			});
			this.m_gridView.SetListItemCount(this.m_nodeDatas.Count, true);
			this.m_gridView.RefreshAllShownItem();
			if (flag)
			{
				this.m_gridView.MovePanelToItemIndex(0, 0f);
			}
			bool flag3 = this.m_equipDataModule.IsCanOneClickEquipCompose();
			if (this.m_titleGroup != null)
			{
				this.m_titleGroup.SetOneClickBtGray(!flag3);
			}
		}

		private void OnRefreshLocalEquipList()
		{
			EquipData equipDataByRowId = this.m_equipDataModule.GetEquipDataByRowId(this.m_currentSelectRowID);
			for (int i = 0; i < this.m_nodeDatas.Count; i++)
			{
				EquipMergeViewModule.NodeData nodeData = this.m_nodeDatas[i];
				if (nodeData != null)
				{
					if (nodeData.m_equipDatas != null)
					{
						for (int j = 0; j < nodeData.m_equipDatas.Count; j++)
						{
							UIEquipMergeEquipItemGroup.NodeItemData nodeItemData = nodeData.m_equipDatas[j];
							if (nodeItemData != null && nodeItemData.m_equipData != null)
							{
								nodeItemData.m_putonActive = this.m_equipDataModule.IsPutOn(nodeItemData.m_equipData.rowID);
								this.IsShowMask(equipDataByRowId, nodeItemData.m_equipData, out nodeItemData.m_lockActive, out nodeItemData.m_tickActive);
							}
						}
					}
					else if (nodeData.m_propDatas != null)
					{
						for (int k = 0; k < nodeData.m_propDatas.Count; k++)
						{
							UIEquipMergePropItemGroup.NodePropData nodePropData = nodeData.m_propDatas[k];
							if (nodePropData != null && nodePropData.m_propData != null)
							{
								this.IsShowMask(equipDataByRowId, nodePropData.m_propData, out nodePropData.m_lockActive, out nodePropData.m_tickActive);
							}
						}
					}
				}
			}
			foreach (KeyValuePair<int, CustomBehaviour> keyValuePair in this.m_nodeDic)
			{
				UIEquipMergeEquipItemGroup uiequipMergeEquipItemGroup = keyValuePair.Value as UIEquipMergeEquipItemGroup;
				if (uiequipMergeEquipItemGroup != null && uiequipMergeEquipItemGroup.m_data != null)
				{
					uiequipMergeEquipItemGroup.RefreshData(uiequipMergeEquipItemGroup.m_data, 0f, 0f, false);
				}
				else
				{
					UIEquipMergePropItemGroup uiequipMergePropItemGroup = keyValuePair.Value as UIEquipMergePropItemGroup;
					if (uiequipMergePropItemGroup != null && uiequipMergePropItemGroup.m_data != null)
					{
						uiequipMergePropItemGroup.RefreshData(uiequipMergePropItemGroup.m_data, 0f, 0f, false);
					}
				}
			}
		}

		private void OnRefreshTitleGroup()
		{
			this.m_titleGroup.SetEquipType(this.m_currentEquipType);
			this.m_titleGroup.SetUnfold(false);
		}

		private void OnClickCloseBt()
		{
			GameApp.View.CloseView(ViewName.EquipMergeViewModule, null);
		}

		private void OnClickMergeBt()
		{
			List<EquipComposeData> list = new List<EquipComposeData>();
			EquipComposeData equipComposeData = new EquipComposeData();
			equipComposeData.MainRowId = this.m_currentSelectRowID;
			for (int i = 0; i < this.m_mergeRowIDs.Count; i++)
			{
				if (this.m_mergeRowIDs[i] > 0L)
				{
					equipComposeData.RowIds.Add((ulong)this.m_mergeRowIDs[i]);
				}
			}
			list.Add(equipComposeData);
			EquipData equipDataByRowId = this.m_equipDataModule.GetEquipDataByRowId(this.m_currentSelectRowID);
			EquipmentDto equipmentDto = equipDataByRowId.ToEquipmentDto();
			EquipData equipData = new EquipData();
			equipData.SetEquipData(equipmentDto);
			equipmentDto.RowId = 0UL;
			equipmentDto.Level = equipDataByRowId.level;
			equipmentDto.EquipId = (uint)this.m_equipDataModule.GetNextEquipTableIDForQuality((int)equipDataByRowId.id);
			EquipData equipData2 = new EquipData();
			equipData2.SetEquipData(equipmentDto);
			EquipMergeFinishedViewModule.OpenData openData = new EquipMergeFinishedViewModule.OpenData();
			openData.m_lastEquipData = equipData;
			openData.m_equipData = equipData2;
			NetworkUtils.Equip.DoEquipComposeRequest(list, false, delegate(bool isok, EquipComposeResponse response)
			{
				if (!isok)
				{
					return;
				}
				this.ShowComposeResult(response, openData);
			});
		}

		private void ShowComposeResult(EquipComposeResponse resp, EquipMergeFinishedViewModule.OpenData openData)
		{
			EquipMergeViewModule.<>c__DisplayClass40_0 CS$<>8__locals1 = new EquipMergeViewModule.<>c__DisplayClass40_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.openData = openData;
			CS$<>8__locals1.resp = resp;
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			EquipMergeAnimationViewModule.OpenData openData2 = new EquipMergeAnimationViewModule.OpenData();
			openData2.Callback = new Action(CS$<>8__locals1.<ShowComposeResult>g__showResult|0);
			GameApp.View.OpenView(ViewName.EquipMergeAnimationViewModule, openData2, 1, null, null);
		}

		private void OnClickOneClickMergeBt(UIEquipMergeTitleGroup obj)
		{
			if (!this.m_equipDataModule.IsCanOneClickEquipCompose())
			{
				return;
			}
			NetworkUtils.Equip.DoEquipComposeRequest(this.m_equipDataModule.GetOneClickEquipComposeDatas(), true, delegate(bool isok, EquipComposeResponse response)
			{
				if (!isok)
				{
					return;
				}
				this.ShowComposeResultOneKey(response);
			});
		}

		private void ShowComposeResultOneKey(EquipComposeResponse resp)
		{
			EquipMergeViewModule.<>c__DisplayClass42_0 CS$<>8__locals1 = new EquipMergeViewModule.<>c__DisplayClass42_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.resp = resp;
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			EquipMergeAnimationViewModule.OpenData openData = new EquipMergeAnimationViewModule.OpenData();
			openData.Callback = new Action(CS$<>8__locals1.<ShowComposeResultOneKey>g__showResult|0);
			GameApp.View.OpenView(ViewName.EquipMergeAnimationViewModule, openData, 1, null, null);
		}

		private void OnClickFilterBt(UIEquipMergeTitleGroup obj)
		{
			this.m_filterSelectGroup.SetActive(true);
			this.m_titleGroup.SetUnfold(true);
		}

		private void OnClickNextEquipItem(UIHeroEquipItem obj)
		{
			EquipDetailsViewModule.OpenData openData = new EquipDetailsViewModule.OpenData();
			openData.m_equipData = obj.m_equipData;
			openData.m_isShowButtons = false;
			GameApp.View.OpenView(ViewName.EquipDetailsViewModule, openData, 1, null, null);
		}

		private void OnClickEquipItemForMergeEquipGroup(UIHeroEquipItem obj)
		{
			this.OnClickEquipItem(obj, true);
		}

		private void OnClickPropItemForMergeEquipGroup(UIItem uiItem)
		{
			for (int i = 0; i < this.m_mergeRowIDs.Count; i++)
			{
				if (this.m_mergeRowIDs[i] == -1L)
				{
					this.m_mergeRowIDs[i] = 0L;
				}
			}
			this.OnUnSelectComposeProp();
			this.OnRefreshLocalEquipList();
			this.OnCheckMergeBt();
		}

		private void OnClickEquipItemForItem(UIHeroEquipItem obj)
		{
			this.OnClickEquipItem(obj, false);
		}

		private void OnClickPropItemForItem(UIItem uiItem, PropData propData, object obj)
		{
			this.OnClickPropItem(uiItem, false);
		}

		private void OnClickEquipItem(UIHeroEquipItem obj, bool isMergeGroup)
		{
			if (obj.m_equipData == null)
			{
				return;
			}
			EquipData equipData = ((this.m_currentSelectRowID > 0UL) ? this.m_equipDataModule.GetEquipDataByRowId(this.m_currentSelectRowID) : null);
			if (this.m_currentSelectRowID == 0UL)
			{
				if (!isMergeGroup)
				{
					int mergeCount = this.m_equipDataModule.GetMergeCount(obj.m_equipData.composeId);
					this.m_equipGroup.SetCostCount(mergeCount);
					this.mFlyEquipGroup.Fly(obj.m_equipData, obj.transform.position, this.m_equipGroup.GetSelectPosition(), delegate
					{
						this.OnSelectMainEquip(obj.m_equipData);
						this.OnRefreshEquipList();
						this.OnCheckMergeBt();
						this.mFlyEquipGroup.SetActive(false);
					});
					this.mFlyEquipGroup.SetActive(true);
					this.mFlyEquipGroup.m_equipItem.SetNameActive(true);
					return;
				}
				this.OnSelectMainEquip(obj.m_equipData);
				this.OnRefreshEquipList();
				this.OnCheckMergeBt();
				return;
			}
			else
			{
				if (this.m_currentSelectRowID == obj.m_equipData.rowID)
				{
					this.OnUnSelectMainEquip();
					this.OnRefreshEquipList();
					this.OnCheckMergeBt();
					return;
				}
				int count = this.m_equipDataModule.GetMergeCount(equipData.composeId);
				int index = -1;
				for (int i = 0; i < this.m_mergeRowIDs.Count; i++)
				{
					long num = this.m_mergeRowIDs[i];
					if (num != 0L && obj.m_equipData.rowID == (ulong)num)
					{
						index = i;
						break;
					}
				}
				if (index >= 0)
				{
					this.OnUnSelectComposeEquip(index, count);
					this.OnRefreshLocalEquipList();
					this.OnCheckMergeBt();
				}
				if (index == -1)
				{
					for (int j = 0; j < this.m_mergeRowIDs.Count; j++)
					{
						if (this.m_mergeRowIDs[j] == 0L)
						{
							index = j;
							break;
						}
					}
					if (index >= 0)
					{
						if (!isMergeGroup)
						{
							this.mFlyEquipGroup.Fly(obj.m_equipData, obj.transform.position, this.m_equipGroup.GetMergeNodePosition(count, index), delegate
							{
								this.OnSelectComposeEquip(obj.m_equipData, index, count);
								this.OnRefreshLocalEquipList();
								this.OnCheckMergeBt();
								this.mFlyEquipGroup.SetActive(false);
							});
							this.mFlyEquipGroup.SetActive(true);
							this.mFlyEquipGroup.m_equipItem.SetNameActive(false);
							return;
						}
						this.OnSelectComposeEquip(obj.m_equipData, index, count);
						this.OnRefreshLocalEquipList();
						this.OnCheckMergeBt();
					}
				}
				return;
			}
		}

		private void OnClickPropItem(UIItem obj, bool isMergeGroup)
		{
			if (this.m_currentSelectRowID <= 0UL)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < this.m_mergeRowIDs.Count; i++)
			{
				if (this.m_mergeRowIDs[i] == -1L)
				{
					flag = true;
					this.m_mergeRowIDs[i] = 0L;
				}
			}
			if (flag)
			{
				this.OnUnSelectComposeProp();
				this.OnRefreshLocalEquipList();
				this.OnCheckMergeBt();
				return;
			}
			EquipData equipData = ((this.m_currentSelectRowID > 0UL) ? this.m_equipDataModule.GetEquipDataByRowId(this.m_currentSelectRowID) : null);
			int count = this.m_equipDataModule.GetMergeCount(equipData.composeId);
			long count2 = (long)obj.m_propData.count;
			long num = 0L;
			for (int j = 0; j < count; j++)
			{
				int num2 = j;
				if (this.m_mergeRowIDs.Count <= j)
				{
					this.m_mergeRowIDs.Add(0L);
				}
				if (this.m_mergeRowIDs[j] == 0L)
				{
					if (num >= count2)
					{
						break;
					}
					num += 1L;
					EquipMergeFlyPropGroup flyPropGroup = Object.Instantiate<EquipMergeFlyPropGroup>(this.mFlyPropGroup, this.mFlyPropGroup.transform.parent);
					this.m_flyItems.Add(flyPropGroup);
					flyPropGroup.SetActive(true);
					flyPropGroup.Init();
					flyPropGroup.Fly(num2, obj.m_propData, obj.transform.position, this.m_equipGroup.GetMergeNodePosition(count, num2), delegate
					{
						if (flyPropGroup != null)
						{
							this.m_flyItems.Remove(flyPropGroup);
							Object.Destroy(flyPropGroup.gameObject);
						}
						this.OnSelectComposeProp(obj.m_propData, flyPropGroup.m_index, count);
						this.OnRefreshLocalEquipList();
						this.OnCheckMergeBt();
					});
					flyPropGroup.m_propItem.SetCountText("");
				}
			}
		}

		private void OnCloseForFilterSelectGroup(int index)
		{
			this.m_currentEquipType = index;
			this.OnRefreshTitleGroup();
			this.OnRefreshEquipList();
		}

		private void OnSelectMainEquip(EquipData equipData)
		{
			if (equipData == null)
			{
				return;
			}
			int mergeCount = this.m_equipDataModule.GetMergeCount(equipData.composeId);
			this.m_currentSelectRowID = equipData.rowID;
			this.m_mergeRowIDs.Clear();
			for (int i = 0; i < mergeCount; i++)
			{
				this.m_mergeRowIDs.Add(0L);
			}
			this.m_equipGroup.SetSelectEquip(equipData, this.m_equipDataModule.IsPutOn(this.m_currentSelectRowID));
			EquipmentDto equipmentDto = equipData.ToEquipmentDto();
			equipmentDto.RowId = 0UL;
			equipmentDto.Level = equipData.level;
			equipmentDto.EquipId = (uint)this.m_equipDataModule.GetNextEquipTableIDForQuality((int)equipData.id);
			EquipData equipData2 = new EquipData();
			equipData2.SetEquipData(equipmentDto);
			this.m_equipGroup.SetNextEquip(equipData2);
			this.m_equipGroup.SetOneEquipItem(null, (int)equipData.id, equipData.composeId);
			this.m_equipGroup.SetTwoEquipItem1(null, (int)equipData.id, equipData.composeId);
			this.m_equipGroup.SetTwoEquipItem2(null, (int)equipData.id, equipData.composeId);
			this.m_equipGroup.SetCostActive(true);
			this.m_equipGroup.SetCost(equipData);
			this.m_equipGroup.SetInfo(equipData, equipData2);
			this.m_titleGroup.SetFilterActive(false);
		}

		private void OnUnSelectMainEquip()
		{
			this.m_currentSelectRowID = 0UL;
			this.m_mergeRowIDs.Clear();
			this.m_equipGroup.SetSelectEquip(null, false);
			this.m_equipGroup.SetNextEquip(null);
			this.m_equipGroup.SetCostCount(0);
			this.m_equipGroup.SetCostActive(false);
			this.m_equipGroup.SetInfo(null, null);
			this.m_titleGroup.SetFilterActive(true);
		}

		private void OnSelectComposeEquip(EquipData equipData, int index, int composeCount)
		{
			this.m_mergeRowIDs[index] = (long)equipData.rowID;
			if (composeCount == 1)
			{
				this.m_equipGroup.SetOneEquipItem(equipData, 0, 0);
				return;
			}
			if (index == 0)
			{
				this.m_equipGroup.SetTwoEquipItem1(equipData, 0, 0);
				return;
			}
			if (index == 1)
			{
				this.m_equipGroup.SetTwoEquipItem2(equipData, 0, 0);
			}
		}

		private void OnSelectComposeProp(PropData propData, int index, int composeCount)
		{
			this.m_mergeRowIDs[index] = -1L;
			if (composeCount == 1)
			{
				this.m_equipGroup.SetOnePropItem(propData);
				return;
			}
			if (index == 0)
			{
				this.m_equipGroup.SetTwoPropItem1(propData);
				return;
			}
			if (index == 1)
			{
				this.m_equipGroup.SetTwoPropItem2(propData);
			}
		}

		private void OnUnSelectComposeEquip(int index, int composeCount)
		{
			if (index < 0)
			{
				return;
			}
			this.m_mergeRowIDs[index] = 0L;
			if (this.m_currentSelectRowID > 0UL)
			{
				EquipData equipDataByRowId = this.m_equipDataModule.GetEquipDataByRowId(this.m_currentSelectRowID);
				if (composeCount == 1)
				{
					this.m_equipGroup.SetOneEquipItem(null, (int)equipDataByRowId.id, equipDataByRowId.composeId);
					return;
				}
				if (index == 0)
				{
					this.m_equipGroup.SetTwoEquipItem1(null, (int)equipDataByRowId.id, equipDataByRowId.composeId);
					return;
				}
				if (index == 1)
				{
					this.m_equipGroup.SetTwoEquipItem2(null, (int)equipDataByRowId.id, equipDataByRowId.composeId);
				}
			}
		}

		private void OnUnSelectComposeProp()
		{
			this.m_equipGroup.SetOnePropItem(null);
			this.m_equipGroup.SetTwoPropItem1(null);
			this.m_equipGroup.SetTwoPropItem2(null);
		}

		public CustomButton m_closeBt;

		public CustomButton m_mergeBt;

		public UIEquipMergeEquipGroup m_equipGroup;

		public UIEquipMergeTitleGroup m_titleGroup;

		public UIEquipMergeFilterSelectGroup m_filterSelectGroup;

		public EquipMergeFlyEquipGroup mFlyEquipGroup;

		public EquipMergeFlyPropGroup mFlyPropGroup;

		public Animator m_viewAnimator;

		public int m_columnCount = 5;

		public LoopListView2 m_gridView;

		public Dictionary<int, CustomBehaviour> m_nodeDic = new Dictionary<int, CustomBehaviour>();

		public List<EquipData> m_equipDatas = new List<EquipData>();

		public List<EquipMergeViewModule.NodeData> m_nodeDatas = new List<EquipMergeViewModule.NodeData>();

		public ulong m_currentSelectRowID;

		public List<long> m_mergeRowIDs = new List<long>();

		public int m_currentEquipType;

		public EquipDataModule m_equipDataModule;

		private PropDataModule m_propDataModule;

		private float _curTime;

		private const int SingleAnimLine = 4;

		private const float AnimDelayPerItem = 0.02f;

		private const float AnimDuring = 0.2f;

		private List<CustomBehaviour> m_flyItems = new List<CustomBehaviour>();

		public class NodeData
		{
			public bool m_isTopSpace;

			public bool m_isBottomSpace;

			public List<UIEquipMergeEquipItemGroup.NodeItemData> m_equipDatas;

			public List<UIEquipMergePropItemGroup.NodePropData> m_propDatas;

			public string m_titleName = string.Empty;
		}
	}
}
