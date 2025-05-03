using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using SuperScrollView;
using UnityEngine.Events;

namespace HotFix
{
	public class PetCollectionViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.topRes.Init();
			this.topRes.SetStyle(EModuleId.PetSystem, new List<int> { 1, 2, 9 });
			this.loopListView2.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.petCollectionNode.gameObject.SetActive(false);
			this.txtCollectionTip.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_collection_tip");
		}

		public override void OnOpen(object data)
		{
			this.UpdateCollectionNodeDatas();
			this.loopListView2.SetListItemCount(this.nodeDatas.Count, true);
			this.loopListView2.RefreshAllShownItem();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.topRes.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.btnBack.onClick.AddListener(new UnityAction(this.OnBtnBackClick));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.btnBack.onClick.RemoveListener(new UnityAction(this.OnBtnBackClick));
		}

		private void OnBtnBackClick()
		{
			GameApp.View.CloseView(ViewName.PetCollectionViewModule, null);
		}

		private void UpdateCollectionNodeDatas()
		{
			this.nodeDatas.Clear();
			List<PetCollectionData> petCollectionDataList = GameApp.Data.GetDataModule(DataName.PetDataModule).PetCollectionDataList;
			for (int i = 0; i < petCollectionDataList.Count; i++)
			{
				PetCollectionData petCollectionData = petCollectionDataList[i];
				PetCollectionViewModule.NodeData nodeData = new PetCollectionViewModule.NodeData();
				nodeData.collectionData = petCollectionData;
				this.nodeDatas.Add(nodeData);
			}
			this.nodeDatas.Insert(0, new PetCollectionViewModule.NodeData
			{
				m_isTopSpace = true
			});
			this.nodeDatas.Add(new PetCollectionViewModule.NodeData
			{
				m_isBottomSpace = true
			});
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.nodeDatas.Count)
			{
				return null;
			}
			PetCollectionViewModule.NodeData nodeData = this.nodeDatas[index];
			LoopListViewItem2 loopListViewItem;
			if (nodeData.m_isTopSpace)
			{
				loopListViewItem = view.NewListViewItem("TopSpace");
				int instanceID = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour;
				this.itemsDict.TryGetValue(instanceID, out customBehaviour);
				if (customBehaviour == null)
				{
					UIItemSpace component = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component.Init();
					this.itemsDict[instanceID] = component;
				}
				return loopListViewItem;
			}
			if (nodeData.m_isBottomSpace)
			{
				loopListViewItem = view.NewListViewItem("BottomSpace");
				int instanceID2 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour2;
				this.itemsDict.TryGetValue(instanceID2, out customBehaviour2);
				if (customBehaviour2 == null)
				{
					UIItemSpace component2 = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component2.Init();
					this.itemsDict[instanceID2] = component2;
				}
				return loopListViewItem;
			}
			loopListViewItem = view.NewListViewItem("Node");
			int instanceID3 = loopListViewItem.gameObject.GetInstanceID();
			CustomBehaviour customBehaviour3;
			this.itemsDict.TryGetValue(instanceID3, out customBehaviour3);
			if (customBehaviour3 == null)
			{
				PetCollectionNode component3 = loopListViewItem.gameObject.GetComponent<PetCollectionNode>();
				component3.Init();
				this.itemsDict[instanceID3] = component3;
				customBehaviour3 = component3;
			}
			PetCollectionNode petCollectionNode = customBehaviour3 as PetCollectionNode;
			if (petCollectionNode != null)
			{
				petCollectionNode.SetData(nodeData.collectionData, index);
			}
			return loopListViewItem;
		}

		public ModuleCurrencyCtrl topRes;

		public PetCollectionNode petCollectionNode;

		public CustomButton btnBack;

		public CustomText txtCollectionTip;

		public LoopListView2 loopListView2;

		private List<PetCollectionViewModule.NodeData> nodeDatas = new List<PetCollectionViewModule.NodeData>();

		private Dictionary<int, CustomBehaviour> itemsDict = new Dictionary<int, CustomBehaviour>();

		public class NodeData
		{
			public bool m_isTopSpace;

			public bool m_isBottomSpace;

			public PetCollectionData collectionData;
		}
	}
}
