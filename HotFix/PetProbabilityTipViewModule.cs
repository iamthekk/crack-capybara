using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using SuperScrollView;

namespace HotFix
{
	public class PetProbabilityTipViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.petDataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this.petSummonTableList = GameApp.Table.GetManager().GetPet_petSummonModelInstance().GetAllElements();
			this.InitLoopList();
		}

		public override void OnOpen(object data)
		{
			int drawLevel = this.petDataModule.m_petDrawExpData.DrawLevel;
			for (int i = 0; i < this.petSummonTableList.Count; i++)
			{
				if (drawLevel == this.petSummonTableList[i].id)
				{
					this.curIndex = i;
				}
			}
			this.UpdateBtnArrow();
			this.UpdateView();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.DeInitLoopList();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.popCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnPopCommonClick);
			this.btnLeft.m_onClick = new Action(this.OnBtnLeftClick);
			this.btnRight.m_onClick = new Action(this.OnBtnRightClick);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.popCommon.OnClick = null;
			this.btnLeft.m_onClick = null;
			this.btnRight.m_onClick = null;
		}

		private void OnPopCommonClick(UIPopCommon.UIPopCommonClickType type)
		{
			GameApp.View.CloseView(ViewName.PetProbabilityTipViewModule, null);
		}

		private void OnBtnLeftClick()
		{
			if (this.curIndex <= 0)
			{
				return;
			}
			this.curIndex--;
			this.UpdateBtnArrow();
			this.UpdateView();
		}

		private void OnBtnRightClick()
		{
			if (this.curIndex >= this.petSummonTableList.Count - 1)
			{
				return;
			}
			this.curIndex++;
			this.UpdateBtnArrow();
			this.UpdateView();
		}

		private void UpdateView()
		{
			Pet_petSummon pet_petSummon = this.petSummonTableList[this.curIndex];
			this.txtLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_draw_level", new object[] { pet_petSummon.id });
			this.RefreshLoopListItem();
		}

		private void UpdateBtnArrow()
		{
			this.btnLeft.gameObject.SetActive(this.curIndex > 0);
			this.btnRight.gameObject.SetActive(this.curIndex < this.petSummonTableList.Count - 1);
		}

		private void InitLoopList()
		{
			this.loopList.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetServerItemByIndex), null);
		}

		private void DeInitLoopList()
		{
			foreach (CustomBehaviour customBehaviour in this.serverLoopItemDict.Values)
			{
				if (customBehaviour != null)
				{
					customBehaviour.DeInit();
				}
			}
			this.serverLoopItemDict.Clear();
		}

		private void RefreshLoopListItem()
		{
			Pet_petSummon pet_petSummon = this.petSummonTableList[this.curIndex];
			this.itemNodeDatas.Clear();
			this.itemNodeDatas.Add(new PetProbabilityTipViewModule.ItemNodeData
			{
				isTopSpace = true
			});
			for (int i = 0; i < pet_petSummon.probability.Length; i++)
			{
				this.itemNodeDatas.Add(new PetProbabilityTipViewModule.ItemNodeData
				{
					qualityId = i + 1,
					probability = int.Parse(pet_petSummon.probability[i])
				});
			}
			this.itemNodeDatas.Add(new PetProbabilityTipViewModule.ItemNodeData
			{
				isBottomSpace = true
			});
			this.loopList.SetListItemCount(this.itemNodeDatas.Count, true);
			this.loopList.RefreshAllShownItem();
			this.loopList.MovePanelToItemIndex(0, 0f);
		}

		private LoopListViewItem2 OnGetServerItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.itemNodeDatas.Count)
			{
				return null;
			}
			PetProbabilityTipViewModule.ItemNodeData itemNodeData = this.itemNodeDatas[index];
			LoopListViewItem2 loopListViewItem;
			if (itemNodeData.isTopSpace)
			{
				loopListViewItem = view.NewListViewItem("TopSpace");
				int instanceID = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour;
				this.serverLoopItemDict.TryGetValue(instanceID, out customBehaviour);
				if (customBehaviour == null)
				{
					UIItemSpace component = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component.Init();
					this.serverLoopItemDict[instanceID] = component;
				}
			}
			else if (itemNodeData.isBottomSpace)
			{
				loopListViewItem = view.NewListViewItem("BottomSpace");
				int instanceID2 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour2;
				this.serverLoopItemDict.TryGetValue(instanceID2, out customBehaviour2);
				if (customBehaviour2 == null)
				{
					UIItemSpace component2 = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component2.Init();
					this.serverLoopItemDict[instanceID2] = component2;
				}
			}
			else
			{
				loopListViewItem = view.NewListViewItem("PetProbabilityItem");
				int instanceID3 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour3;
				this.serverLoopItemDict.TryGetValue(instanceID3, out customBehaviour3);
				if (customBehaviour3 == null)
				{
					PetProbabilityItem component3 = loopListViewItem.gameObject.GetComponent<PetProbabilityItem>();
					component3.Init();
					this.serverLoopItemDict[instanceID3] = component3;
					customBehaviour3 = component3;
				}
				PetProbabilityItem petProbabilityItem = customBehaviour3 as PetProbabilityItem;
				if (petProbabilityItem != null)
				{
					petProbabilityItem.SetData(itemNodeData);
				}
			}
			return loopListViewItem;
		}

		public LoopListView2 loopList;

		public UIPopCommon popCommon;

		public CustomText txtLevel;

		public CustomButton btnLeft;

		public CustomButton btnRight;

		private PetDataModule petDataModule;

		private IList<Pet_petSummon> petSummonTableList = new List<Pet_petSummon>();

		private int curIndex;

		private List<PetProbabilityTipViewModule.ItemNodeData> itemNodeDatas = new List<PetProbabilityTipViewModule.ItemNodeData>();

		private Dictionary<int, CustomBehaviour> serverLoopItemDict = new Dictionary<int, CustomBehaviour>();

		public class ItemNodeData
		{
			public bool isTopSpace;

			public bool isBottomSpace;

			public int qualityId;

			public int probability;
		}
	}
}
