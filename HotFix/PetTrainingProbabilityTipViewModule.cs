using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class PetTrainingProbabilityTipViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.petDataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this.InitLoopList();
		}

		public override void OnOpen(object data)
		{
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
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.popCommon.OnClick = null;
		}

		private void OnPopCommonClick(UIPopCommon.UIPopCommonClickType type)
		{
			GameApp.View.CloseView(ViewName.PetTrainingProbabilityTipViewModule, null);
		}

		private void UpdateView()
		{
			int petTrainingLv = this.petDataModule.PetTrainingLv;
			this.petTrainingProbTable = GameApp.Table.GetManager().GetPet_PetTrainingProbModelInstance().GetElementById(petTrainingLv);
			this.petTrainingProbNextTable = GameApp.Table.GetManager().GetPet_PetTrainingProbModelInstance().GetElementById(petTrainingLv + 1);
			this.txtLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_training_cur_lv", new object[] { this.petTrainingProbTable.id });
			this.txtLevelNext.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_training_next_lv", new object[] { petTrainingLv + 1 });
			this.txtLevelNext.gameObject.SetActive(this.petTrainingProbNextTable != null);
			this.goArrow.SetActive(this.petTrainingProbNextTable != null);
			this.RefreshLoopListItem();
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
			this.itemNodeDatas.Clear();
			int num = ((this.petTrainingProbNextTable != null) ? this.petTrainingProbNextTable.probability.Length : this.petTrainingProbTable.probability.Length);
			bool flag = this.petTrainingProbNextTable != null;
			for (int i = 0; i < num; i++)
			{
				int num2 = ((this.petTrainingProbTable.probability.Length > i) ? int.Parse(this.petTrainingProbTable.probability[i]) : 0);
				int num3 = ((this.petTrainingProbNextTable != null && this.petTrainingProbNextTable.probability.Length > i) ? int.Parse(this.petTrainingProbNextTable.probability[i]) : 0);
				this.itemNodeDatas.Add(new PetTrainingProbabilityTipViewModule.ItemNodeData
				{
					qualityId = i + 1,
					probability = num2,
					probabilityNext = num3,
					hasNext = flag
				});
			}
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
			PetTrainingProbabilityTipViewModule.ItemNodeData itemNodeData = this.itemNodeDatas[index];
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
				loopListViewItem = view.NewListViewItem("PetTrainingProbabilityItem");
				int instanceID3 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour3;
				this.serverLoopItemDict.TryGetValue(instanceID3, out customBehaviour3);
				if (customBehaviour3 == null)
				{
					PetTrainingProbabilityItem component3 = loopListViewItem.gameObject.GetComponent<PetTrainingProbabilityItem>();
					component3.Init();
					this.serverLoopItemDict[instanceID3] = component3;
					customBehaviour3 = component3;
				}
				PetTrainingProbabilityItem petTrainingProbabilityItem = customBehaviour3 as PetTrainingProbabilityItem;
				if (petTrainingProbabilityItem != null)
				{
					petTrainingProbabilityItem.SetData(itemNodeData);
				}
			}
			return loopListViewItem;
		}

		[GameTestMethod("界面", "打开宠物培育概率提示界面", "", 0)]
		private static void OpenPetProbabilityTipViewModule()
		{
			GameApp.View.OpenView(ViewName.PetTrainingProbabilityTipViewModule, null, 1, null, null);
		}

		public LoopListView2 loopList;

		public UIPopCommon popCommon;

		public CustomText txtLevel;

		public CustomText txtLevelNext;

		public GameObject goArrow;

		private PetDataModule petDataModule;

		private Pet_PetTrainingProb petTrainingProbTable;

		private Pet_PetTrainingProb petTrainingProbNextTable;

		private List<PetTrainingProbabilityTipViewModule.ItemNodeData> itemNodeDatas = new List<PetTrainingProbabilityTipViewModule.ItemNodeData>();

		private Dictionary<int, CustomBehaviour> serverLoopItemDict = new Dictionary<int, CustomBehaviour>();

		public class ItemNodeData
		{
			public bool isTopSpace;

			public bool isBottomSpace;

			public int qualityId;

			public int probability;

			public int probabilityNext;

			public bool hasNext;
		}
	}
}
