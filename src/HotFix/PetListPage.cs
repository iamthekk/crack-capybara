using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Common;
using Proto.Pet;
using Server;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class PetListPage : CustomBehaviour
	{
		private bool IsInShowSelect
		{
			get
			{
				return this.isInShowSelect;
			}
			set
			{
				this.isInShowSelect = value;
				this.m_btnPetShow.gameObject.SetActive(!this.isInShowSelect);
				this.m_btnPetShowOk.gameObject.SetActive(this.isInShowSelect);
				this.m_rectPetShowNum.gameObject.SetActive(this.isInShowSelect);
				float height = this.m_rectPetShowNum.rect.height;
				float height2 = ((RectTransform)this.m_loopGridView.transform.parent).rect.height;
				if (this.IsInShowSelect)
				{
					this.m_ScrollViewport.offsetMin = new Vector2(this.m_ScrollViewport.offsetMin.x, height);
					return;
				}
				this.m_ScrollViewport.offsetMin = new Vector2(this.m_ScrollViewport.offsetMin.x, 0f);
			}
		}

		protected override void OnInit()
		{
			this.petDataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this.m_petInfoNode.Init();
			this.m_petItem.gameObject.SetActive(false);
			this.m_loopGridView.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
			this.m_btnSort.m_onClick = new Action(this.OnBtnSortClick);
			this.m_btnPetShow.m_onClick = new Action(this.OnBtnPetShowClick);
			this.m_btnPetShowOk.m_onClick = new Action(this.OnBtnPetShowOkClick);
			this.m_btnBack.m_onClick = new Action(this.OnBtnBackClick);
			this.m_btnArrowLeft.m_onClick = new Action(this.OnBtnLeftClick);
			this.m_btnArrowRight.m_onClick = new Action(this.OnBtnRightClick);
		}

		protected override void OnDeInit()
		{
			this.m_btnSort.m_onClick = null;
			this.m_btnPetShow.m_onClick = null;
			this.m_btnPetShowOk.m_onClick = null;
			this.m_btnArrowLeft.m_onClick = null;
			this.m_btnArrowRight.m_onClick = null;
			this.m_btnBack.m_onClick = null;
			this.m_petInfoNode.DeInit();
			this.petListData.Clear();
			this.petListData = null;
			this.itemDict.Clear();
			this.itemDict = null;
			this.petDataModule = null;
		}

		public void OnShow()
		{
			this.IsInShowSelect = false;
			base.gameObject.SetActive(true);
			this.RefreshPetListData();
			this.UpdateView();
		}

		public void OnHide()
		{
			this.IsInShowSelect = false;
			base.gameObject.SetActive(false);
		}

		public void UpdateView()
		{
			this.RefreshSortText();
			this.RefreshSelectData();
			this.SetLoopList();
			this.OnPetItemClickImpl(this.curPetData);
			this.UpdateScrollNodeRectTrans();
		}

		private void UpdateScrollNodeRectTrans()
		{
			if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.PetTraining, false))
			{
				int num = Mathf.CeilToInt((float)this.m_petInfoNode.m_petPassiveNodeItem.GetActiveCount() / 2f);
				if (num == 0)
				{
					num = 1;
				}
				float num2 = (float)(745 - (3 - num) * 106);
				float num3 = (float)(-1412 + (3 - num) * 106);
				this.m_petInfoNode.m_petPassiveNodeItem.gameObject.SetActive(true);
				this.scrollNodeRectTrans.offsetMax = new Vector2(this.scrollNodeRectTrans.offsetMax.x, num3);
				this.rectTransImgBg.sizeDelta = new Vector2(this.rectTransImgBg.sizeDelta.x, num2);
				return;
			}
			this.m_petInfoNode.m_petPassiveNodeItem.gameObject.SetActive(false);
			this.scrollNodeRectTrans.offsetMax = new Vector2(this.scrollNodeRectTrans.offsetMax.x, -1010f);
			this.rectTransImgBg.sizeDelta = new Vector2(this.rectTransImgBg.sizeDelta.x, 344f);
		}

		private void RefreshQuickStarUpgrade()
		{
		}

		private void RefreshPetListData()
		{
			this.petListData.Clear();
			PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			if (this.IsInShowSelect)
			{
				List<PetData> list = dataModule.GetPetList(EPetFilterType.Fight).SortByType(this.m_sortType);
				List<PetData> list2 = dataModule.GetPetList(EPetFilterType.Assist).SortByType(this.m_sortType);
				List<PetData> list3 = dataModule.GetPetList(EPetFilterType.Idle).SortByType(this.m_sortType);
				this.petListData.AddRange(list);
				this.petListData.AddRange(list2);
				this.petListData.AddRange(list3);
				return;
			}
			List<PetData> list4 = dataModule.GetPetList(EPetFilterType.Fight).SortByType(this.m_sortType);
			List<PetData> list5 = dataModule.GetPetList(EPetFilterType.Assist).SortByType(this.m_sortType);
			List<PetData> list6 = dataModule.GetPetList(EPetFilterType.Idle).SortByType(this.m_sortType);
			this.petListData.AddRange(list4);
			this.petListData.AddRange(list5);
			this.petListData.AddRange(list6);
		}

		private void RefreshSelectData()
		{
			bool flag = false;
			for (int i = 0; i < this.petListData.Count; i++)
			{
				PetData petData = this.petListData[i];
				if (this.curSelectConfigId > 0 && petData.petId.Equals(this.curSelectConfigId))
				{
					this.curIndex = i;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.curSelectConfigId = ((this.petListData.Count > 0) ? this.petListData[0].petId : 0);
				this.curPetData = ((this.petListData.Count > 0) ? this.petListData[0] : null);
				this.curIndex = 0;
			}
			this.UpdateBtnArrow();
		}

		private void SetLoopList()
		{
			this.m_loopGridView.SetListItemCount(this.petListData.Count, true);
			this.m_loopGridView.RefreshAllShownItem();
			if (this.IsInShowSelect)
			{
				this.UpdateShowSelectState();
			}
		}

		private LoopGridViewItem OnGetItemByIndex(LoopGridView view, int index, int row, int column)
		{
			if (index < 0 || index >= this.petListData.Count)
			{
				return null;
			}
			LoopGridViewItem loopGridViewItem = this.m_loopGridView.NewListViewItem("UI_PetItem");
			int instanceID = loopGridViewItem.gameObject.GetInstanceID();
			PetItem component;
			this.itemDict.TryGetValue(instanceID, out component);
			if (component == null)
			{
				component = loopGridViewItem.gameObject.GetComponent<PetItem>();
				component.Init();
				component.onItemClickCallback = new Action<PetItem, bool>(this.OnPetItemClick);
				this.itemDict[instanceID] = component;
			}
			PetData petData = this.petListData[index];
			component.SetIndex(index);
			component.RefreshData(petData);
			if (this.IsInShowSelect)
			{
				bool flag = this.petShowselectedRowIds.Contains(petData.petRowId);
				component.SetSelectFrameActive(false);
				component.SetTickActive(flag);
				component.SetMaskActive(flag);
				component.SetFormationTypeActive(true);
				component.SetFragmentProgressActive(false, 0, int.MaxValue);
				component.SetRedNodeActive(false);
			}
			else
			{
				component.SetSelectFrameActive(component.data.petId == this.curSelectConfigId);
				component.SetTickActive(false);
				component.SetMaskActive(false);
				component.SetFormationTypeActive(true);
				if (petData.PetItemType == EPetItemType.Fragment)
				{
					component.SetFragmentProgressActive(true, petData.fragmentCount, 1);
				}
				else
				{
					component.SetFragmentProgressActive(false, 0, int.MaxValue);
				}
				bool flag2 = false;
				if (!flag2)
				{
					flag2 = petData.ConditionInDeployOk() && petData.ConditionLevelUpOk();
				}
				component.SetRedNodeActive(flag2);
			}
			return loopGridViewItem;
		}

		private void OnPetItemClick(PetItem petItem, bool isActiveClick)
		{
			if (petItem == null || petItem.data == null)
			{
				return;
			}
			if (this.IsInShowSelect)
			{
				if (isActiveClick)
				{
					this.OnPetItemSelectClick(petItem);
					return;
				}
				if (petItem.data.petRowId.Equals(this.curSelectConfigId))
				{
					return;
				}
				this.OnPetItemClickImpl(petItem.data);
				return;
			}
			else
			{
				if (petItem.data.petRowId.Equals(this.curSelectConfigId))
				{
					return;
				}
				this.OnPetItemClickImpl(petItem.data);
				return;
			}
		}

		private void OnPetItemClickImpl(PetData petData)
		{
			if (petData == null)
			{
				return;
			}
			int petId = petData.petId;
			this.curSelectConfigId = petId;
			this.curPetData = petData;
			this.RefreshSelectData();
			this.RefreshSelectPetInfo();
			if (!this.IsInShowSelect)
			{
				foreach (KeyValuePair<int, PetItem> keyValuePair in this.itemDict)
				{
					PetItem value = keyValuePair.Value;
					value.SetTickActive(false);
					value.SetMaskActive(false);
					value.SetSelectFrameActive(value.data.petId.Equals(this.curSelectConfigId));
				}
			}
			this.UpdateBtnArrow();
		}

		private void OnPetItemSelectClick(PetItem petItem)
		{
			if (petItem == null || petItem.data == null)
			{
				return;
			}
			if (this.petShowselectedRowIds.Contains(petItem.data.petRowId))
			{
				this.petShowselectedRowIds.Remove(petItem.data.petRowId);
			}
			else
			{
				if (this.petShowselectedRowIds.Count >= Singleton<GameConfig>.Instance.PetShowMaxCount)
				{
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("pet_show_limit_tip"));
					return;
				}
				this.petShowselectedRowIds.Add(petItem.data.petRowId);
			}
			this.UpdateShowSelectState();
		}

		private void UpdateShowSelectState()
		{
			foreach (PetItem petItem in this.itemDict.Values)
			{
				ulong petRowId = petItem.data.petRowId;
				bool flag = this.petShowselectedRowIds.Contains(petRowId);
				petItem.SetTickActive(flag);
				petItem.SetMaskActive(flag);
				petItem.SetSelectFrameActive(false);
			}
			this.UpdateShowCountText();
		}

		private void RefreshSelectPetInfo()
		{
			if (this.curPetData != null)
			{
				this.curPetData = GameApp.Data.GetDataModule(DataName.PetDataModule).GetPetDataByConfigId(this.curPetData.petId);
			}
			this.m_petInfoNode.SetData(this.curPetData);
		}

		private void OnBtnBackClick()
		{
			this.m_petViewModule.OnClosePetListPage();
		}

		private void OnBtnSortClick()
		{
			this.m_sortType = PetUtil.GetNextSortType(this.m_sortType);
			this.RefreshSortText();
			this.RefreshPetListData();
			this.RefreshSelectData();
			this.SetLoopList();
		}

		private void OnBtnPetShowClick()
		{
			this.IsInShowSelect = true;
			this.RefreshPetShowSelectRowIds();
			this.RefreshPetListData();
			this.RefreshSelectData();
			this.SetLoopList();
		}

		private void OnBtnPetShowOkClick()
		{
			List<ulong> showPetRowIds = GameApp.Data.GetDataModule(DataName.PetDataModule).GetShowPetRowIds();
			if (this.petShowselectedRowIds.Count == showPetRowIds.Count)
			{
				bool flag = false;
				for (int i = 0; i < this.petShowselectedRowIds.Count; i++)
				{
					if (!showPetRowIds.Contains(this.petShowselectedRowIds[i]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.IsInShowSelect = false;
					this.RefreshPetListData();
					this.RefreshSelectData();
					this.SetLoopList();
					return;
				}
			}
			NetworkUtils.Pet.PetShowRequest(this.petShowselectedRowIds, delegate(bool isOk, PetShowResponse res)
			{
				if (!isOk)
				{
					return;
				}
				this.IsInShowSelect = false;
				if (this != null && base.gameObject != null)
				{
					this.RefreshPetShowSelectRowIds();
					this.RefreshPetListData();
					this.RefreshSelectData();
					this.SetLoopList();
				}
			}, true);
		}

		private void RefreshSortText()
		{
			this.m_txtSortName.text = PetUtil.GetSortName(this.m_sortType);
		}

		private void RefreshPetShowSelectRowIds()
		{
			PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this.petShowselectedRowIds = dataModule.GetShowPetRowIds();
			this.UpdateShowCountText();
		}

		private void UpdateShowCountText()
		{
			this.m_txtPetShowNum.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_show_count", new object[]
			{
				this.petShowselectedRowIds.Count,
				Singleton<GameConfig>.Instance.PetShowMaxCount
			});
		}

		private void OnBtnLeftClick()
		{
			if (this.curIndex <= 0)
			{
				return;
			}
			this.curIndex--;
			foreach (PetItem petItem in this.itemDict.Values)
			{
				if (petItem.index.Equals(this.curIndex))
				{
					this.OnPetItemClick(petItem, false);
					break;
				}
			}
			this.UpdateBtnArrow();
		}

		private void OnBtnRightClick()
		{
			if (this.curIndex >= this.petListData.Count - 1)
			{
				return;
			}
			this.curIndex++;
			foreach (PetItem petItem in this.itemDict.Values)
			{
				if (petItem.index.Equals(this.curIndex))
				{
					this.OnPetItemClick(petItem, false);
					break;
				}
			}
			this.UpdateBtnArrow();
		}

		private void UpdateBtnArrow()
		{
			this.m_btnArrowLeft.gameObject.SetActive(this.curIndex > 0);
			this.m_btnArrowRight.gameObject.SetActive(this.curIndex < this.petListData.Count - 1);
			this.UpdateScrollNodeRectTrans();
		}

		private void OnBtnQuickStarUpgradeClick()
		{
			if (this.petDataModule == null)
			{
				return;
			}
			Dictionary<int, int> oldStarData = this.petDataModule.GetPetStarData();
			NetworkUtils.Pet.PetStarUpgradeRequest(0UL, delegate(bool isOk, PetStarResponse resp)
			{
				if (isOk && resp.PetDto.Count > 1)
				{
					Dictionary<int, int> petStarData = this.petDataModule.GetPetStarData();
					List<ItemData> list = new List<ItemData>();
					for (int i = 0; i < resp.PetDto.Count; i++)
					{
						PetDto petDto = resp.PetDto[i];
						if (petDto.PetType == 1U)
						{
							ItemData itemData = new ItemData();
							itemData.SetID(petDto.ConfigId);
							itemData.SetCount(1L);
							list.Add(itemData);
						}
					}
					PetQuickStarUpgradeResultViewModule.OpenData openData = new PetQuickStarUpgradeResultViewModule.OpenData();
					openData.itemList = list;
					openData.oldStarData = oldStarData;
					openData.newStarData = petStarData;
					GameApp.View.OpenView(ViewName.PetQuickStarUpgradeResultViewModule, openData, 1, null, null);
				}
			});
		}

		public PetViewModule m_petViewModule;

		public CustomButton m_btnBack;

		public PetInfoNode m_petInfoNode;

		public CustomButton m_btnArrowLeft;

		public CustomButton m_btnArrowRight;

		public CustomButton m_btnSort;

		public CustomText m_txtSortName;

		public CustomButton m_btnPetShow;

		public CustomButton m_btnPetShowOk;

		public LoopGridView m_loopGridView;

		public PetItem m_petItem;

		public RectTransform m_ScrollViewport;

		public RectTransform m_rectPetShowNum;

		public CustomText m_txtPetShowNum;

		public RectTransform rectTransImgBg;

		public RectTransform scrollNodeRectTrans;

		private List<PetData> petListData = new List<PetData>();

		private EPetSortType m_sortType;

		private int curIndex;

		private int curSelectConfigId;

		private PetData curPetData;

		private Dictionary<int, PetItem> itemDict = new Dictionary<int, PetItem>();

		private PetDataModule petDataModule;

		private bool isInShowSelect;

		private List<ulong> petShowselectedRowIds = new List<ulong>();
	}
}
