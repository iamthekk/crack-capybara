using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Server;
using SuperScrollView;
using UnityEngine.Events;

namespace HotFix
{
	public class PetSelectGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_loopGridView.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
			this.m_btnBack.onClick.AddListener(new UnityAction(this.OnBtnCloseClick));
			this.m_btnSort.onClick.AddListener(new UnityAction(this.OnBtnSortClick));
		}

		protected override void OnDeInit()
		{
			this.m_btnBack.onClick.RemoveListener(new UnityAction(this.OnBtnCloseClick));
			this.m_btnSort.onClick.RemoveListener(new UnityAction(this.OnBtnSortClick));
		}

		public void OnShow(EPetFormationType formationType, ulong rowId)
		{
			this.formationType = formationType;
			this.oldRowId = rowId;
			this.newRowId = rowId;
			base.SetActive(true);
			this.UpdateView();
		}

		public void OnHide()
		{
			base.SetActive(false);
			this.petRanchGroup.CloseDynamicUIGroup();
		}

		public void UpdateView()
		{
			this.RefreshSortText();
			this.RefreshPetListData();
			this.SetLoopList();
		}

		private void RefreshSortText()
		{
			this.m_txtSort.text = PetUtil.GetSortName(this.m_sortType);
		}

		private void RefreshPetListData()
		{
			this.petListData.Clear();
			PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this.petListData = dataModule.GetPetList(EPetFilterType.AllPets).SortByType(this.m_sortType);
		}

		private void SetLoopList()
		{
			this.m_loopGridView.SetListItemCount(this.petListData.Count, true);
			this.m_loopGridView.RefreshAllShownItem();
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
			component.RefreshData(petData);
			bool flag = petData.petRowId.Equals(this.newRowId);
			component.SetFormationTypeActive(true);
			component.SetSelectFrameActive(false);
			component.SetTickActive(flag);
			component.SetMaskActive(flag);
			return loopGridViewItem;
		}

		private void OnPetItemClick(PetItem petItem, bool isActiveClick)
		{
			if (petItem == null || petItem.data == null || petItem.data.petRowId <= 0UL)
			{
				return;
			}
			ulong petRowId = petItem.data.petRowId;
			if (petItem.data.petRowId.Equals(this.newRowId))
			{
				this.newRowId = 0UL;
				this.petRanchGroup.CreateRanchPet(petRowId);
				PetUtil.PetFormationChange(0UL, this.formationType, delegate(bool isOk)
				{
					if (this.petRanchGroup == null)
					{
						return;
					}
					if (isOk)
					{
						this.petRanchGroup.RefreshSlots();
						this.OnHide();
						return;
					}
					this.petRanchGroup.RefreshSlots();
					this.petRanchGroup.RefreshRanch();
				});
				return;
			}
			this.newRowId = petItem.data.petRowId;
			PetUtil.PetFormationChange(this.newRowId, this.formationType, delegate(bool isOk)
			{
				if (this.petRanchGroup == null)
				{
					return;
				}
				this.petRanchGroup.RefreshSlots();
				this.petRanchGroup.RefreshRanch();
				this.OnHide();
			});
		}

		private void OnBtnCloseClick()
		{
			this.OnHide();
		}

		private void OnBtnSortClick()
		{
			this.m_sortType = PetUtil.GetNextSortType(this.m_sortType);
			this.UpdateView();
		}

		public PetRanchGroup petRanchGroup;

		public CustomButton m_btnBack;

		public CustomButton m_btnSort;

		public CustomText m_txtSort;

		public LoopGridView m_loopGridView;

		private EPetSortType m_sortType = EPetSortType.Combat;

		private List<PetData> petListData = new List<PetData>();

		private Dictionary<int, PetItem> itemDict = new Dictionary<int, PetItem>();

		private EPetFormationType formationType;

		private ulong oldRowId;

		private ulong newRowId;
	}
}
