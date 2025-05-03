using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Pet;
using SuperScrollView;
using UnityEngine.Events;

namespace HotFix
{
	public class PetShowGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_loopGridView.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
			this.m_btnBack.onClick.AddListener(new UnityAction(this.OnBtnCloseClick));
			this.m_btnSort.onClick.AddListener(new UnityAction(this.OnBtnSortClick));
			this.m_btnPetShowOk.m_onClick = new Action(this.OnBtnPetShowOkClick);
		}

		protected override void OnDeInit()
		{
			this.m_btnBack.onClick.RemoveListener(new UnityAction(this.OnBtnCloseClick));
			this.m_btnSort.onClick.RemoveListener(new UnityAction(this.OnBtnSortClick));
			this.m_btnPetShowOk.m_onClick = null;
		}

		public void OnShow()
		{
			this.RefreshPetShowSelectRowIds();
			base.SetActive(true);
			this.UpdateView();
		}

		public void OnHide()
		{
			base.SetActive(false);
		}

		public void UpdateView()
		{
			this.RefreshSortText();
			this.RefreshPetListData();
			this.SetLoopList();
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
			bool flag = this.petShowselectedRowIds.Contains(petData.petRowId);
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
			PetData data = petItem.data;
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
					this.OnHide();
					return;
				}
			}
			NetworkUtils.Pet.PetShowRequest(this.petShowselectedRowIds, delegate(bool isOk, PetShowResponse res)
			{
				if (!isOk)
				{
					return;
				}
				if (this != null && base.gameObject != null)
				{
					this.RefreshPetShowSelectRowIds();
					this.RefreshPetListData();
					this.SetLoopList();
				}
			}, true);
			this.OnHide();
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

		public CustomButton m_btnBack;

		public CustomButton m_btnSort;

		public CustomButton m_btnPetShowOk;

		public CustomText m_txtSort;

		public LoopGridView m_loopGridView;

		public CustomText m_txtPetShowNum;

		private EPetSortType m_sortType;

		private List<PetData> petListData = new List<PetData>();

		private Dictionary<int, PetItem> itemDict = new Dictionary<int, PetItem>();

		private List<ulong> petShowselectedRowIds = new List<ulong>();
	}
}
