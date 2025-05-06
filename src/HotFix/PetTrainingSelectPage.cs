using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using SuperScrollView;

namespace HotFix
{
	public class PetTrainingSelectPage : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.btnBack.m_onClick = new Action(this.OnBtnBackClick);
			this.btnSort.m_onClick = new Action(this.OnBtnSortClick);
			if (!this.isLoopGridViewInit)
			{
				this.isLoopGridViewInit = true;
				this.loopGridView.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
			}
		}

		protected override void OnDeInit()
		{
			this.btnBack.m_onClick = null;
			this.btnSort.m_onClick = null;
			this.itemDict.Clear();
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
			this.UpdateView();
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		public void UpdateView()
		{
			this.RefreshSortText();
			this.loopGridView.SetListItemCount(this.petTrainingViewModule.petListData.Count, true);
			this.loopGridView.RefreshAllShownItem();
			if (this.petTrainingViewModule.petListData.Count > 0)
			{
				this.loopGridView.MovePanelToItemByIndex(this.petTrainingViewModule.curIndex, 0f, -20f);
			}
		}

		private void OnBtnBackClick()
		{
			this.Hide();
		}

		private void OnBtnSortClick()
		{
			this.petTrainingViewModule.m_sortType = PetUtil.GetNextSortType(this.petTrainingViewModule.m_sortType);
			this.RefreshSortText();
			this.petTrainingViewModule.UpdateCurSelectConfigId(this.petTrainingViewModule.curSelectConfigId);
			this.UpdateView();
		}

		private void RefreshSortText()
		{
			this.txtSortName.text = PetUtil.GetSortName(this.petTrainingViewModule.m_sortType);
		}

		private LoopGridViewItem OnGetItemByIndex(LoopGridView view, int index, int row, int column)
		{
			if (index < 0 || index >= this.petTrainingViewModule.petListData.Count)
			{
				return null;
			}
			LoopGridViewItem loopGridViewItem = this.loopGridView.NewListViewItem("UI_PetItem");
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
			PetData petData = this.petTrainingViewModule.petListData[index];
			component.SetIndex(index);
			component.RefreshData(petData);
			bool flag = petData.petId == this.petTrainingViewModule.curSelectConfigId;
			component.SetSelectFrameActive(false);
			component.SetTickActive(flag);
			component.SetMaskActive(flag);
			component.SetFormationTypeActive(true);
			component.SetFragmentProgressActive(false, 0, int.MaxValue);
			component.SetRedNodeActive(false);
			return loopGridViewItem;
		}

		private void OnPetItemClick(PetItem petItem, bool isActiveClick)
		{
			if (petItem == null || petItem.data == null || petItem.data.petId == this.petTrainingViewModule.curSelectConfigId)
			{
				return;
			}
			this.petTrainingViewModule.UpdateCurSelectConfigId(petItem.data.petId);
			this.Hide();
		}

		public PetTrainingViewModule petTrainingViewModule;

		public CustomButton btnBack;

		public LoopGridView loopGridView;

		public CustomButton btnSort;

		public CustomText txtSortName;

		private Dictionary<int, PetItem> itemDict = new Dictionary<int, PetItem>();

		private bool isLoopGridViewInit;
	}
}
