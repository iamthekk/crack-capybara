using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.ViewModule;
using SuperScrollView;

namespace HotFix
{
	public class PetLevelEffectTipViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.InitLoopList();
		}

		public override void OnOpen(object data)
		{
			if (data is PetLevelEffectTipViewModule.OpenData)
			{
				this.openData = data as PetLevelEffectTipViewModule.OpenData;
				this.RefreshLoopListItem();
				return;
			}
			this.OnPopCommonClick(UIPopCommon.UIPopCommonClickType.ButtonClose);
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
			GameApp.View.CloseView(ViewName.PetLevelEffectTipViewModule, null);
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
			int petMaxLevel = Singleton<GameConfig>.Instance.PetMaxLevel;
			for (int i = 5; i <= petMaxLevel; i += 5)
			{
				this.itemNodeDatas.Add(new PetLevelEffectTipViewModule.ItemNodeData
				{
					petId = this.openData.petId,
					petLevel = this.openData.petLevel,
					itemLevel = i
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
			PetLevelEffectTipViewModule.ItemNodeData itemNodeData = this.itemNodeDatas[index];
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
				loopListViewItem = view.NewListViewItem("PetLevelEffectItem");
				int instanceID3 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour3;
				this.serverLoopItemDict.TryGetValue(instanceID3, out customBehaviour3);
				if (customBehaviour3 == null)
				{
					PetLevelEffectItem component3 = loopListViewItem.gameObject.GetComponent<PetLevelEffectItem>();
					component3.Init();
					this.serverLoopItemDict[instanceID3] = component3;
					customBehaviour3 = component3;
				}
				PetLevelEffectItem petLevelEffectItem = customBehaviour3 as PetLevelEffectItem;
				if (petLevelEffectItem != null)
				{
					petLevelEffectItem.SetData(itemNodeData, index == this.itemNodeDatas.Count - 1);
				}
			}
			return loopListViewItem;
		}

		public LoopListView2 loopList;

		public UIPopCommon popCommon;

		private List<PetLevelEffectTipViewModule.ItemNodeData> itemNodeDatas = new List<PetLevelEffectTipViewModule.ItemNodeData>();

		private PetLevelEffectTipViewModule.OpenData openData;

		private Dictionary<int, CustomBehaviour> serverLoopItemDict = new Dictionary<int, CustomBehaviour>();

		public class OpenData
		{
			public int petId;

			public int petLevel;
		}

		public class ItemNodeData
		{
			public bool isTopSpace;

			public bool isBottomSpace;

			public int petId;

			public int petLevel;

			public int itemLevel;
		}
	}
}
